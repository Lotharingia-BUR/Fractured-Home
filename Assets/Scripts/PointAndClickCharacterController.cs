using System.Collections;
using Pathfinding;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PointAndClickCharacterController : MonoBehaviour
{
    public enum FacingDirection
    {
        Left = -1, Right = 1
    }

    public float maxSpeed = 5f;
    public float accelerationTime = 0.2f;

    public float nodeReachedDistance = 0.01f;

    private float _currentSpeed;
    private Vector3 _currentVelocity;
    private float _acceleration;

    private FacingDirection _facingDirection = FacingDirection.Right;
    private bool _interruptMovement;

    private Coroutine _movementOverrideCoroutine = null;
    private Coroutine _currentlyFollowingPath = null;

    private ABPath _path;

    private Seeker _seeker;
    private Animator _animator;

    private int _speedHash;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _seeker = GetComponent<Seeker>();
        _animator = GetComponent<Animator>();

        _speedHash = Animator.StringToHash("speed");

        _acceleration = maxSpeed / accelerationTime;
    }

    void OnDisable()
    {
        _currentSpeed = 0f;
        _currentVelocity = Vector3.zero;
        _interruptMovement = false;

        if (_movementOverrideCoroutine != null)
        {
            StopCoroutine(_movementOverrideCoroutine);
            _movementOverrideCoroutine = null;
        }
        if (_currentlyFollowingPath != null)
        {
            StopCoroutine(_currentlyFollowingPath);
            _currentlyFollowingPath = null;
        }
        AnimUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !MouseController.Instance.isOverObject && PauseModeManager.Instance.pauseMode == PauseMode.Unpaused)
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SetDestination(mouseWorldPos);
        }

        AnimUpdate();
    }

    private void AnimUpdate()
    {
        if (_facingDirection == FacingDirection.Right && _currentVelocity.x > 0.01f)
        {
            _facingDirection = FacingDirection.Left;
            transform.localScale = new(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (_facingDirection == FacingDirection.Left && _currentVelocity.x < -0.01f)
        {
            _facingDirection = FacingDirection.Right;
            transform.localScale = new(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        _animator.SetFloat(_speedHash, _currentSpeed);
    }

    void OnMouseDown()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hitsArray = Physics2D.RaycastAll(mousePos, Vector2.zero);

        foreach (RaycastHit2D hit in hitsArray)
        {
            Interactable o = hit.collider?.GetComponent<Interactable>();
            if (o != null)
            {
                o.Click();
            }
        }
    }

    public void SetDestination(Vector2 destination)
    {
        StartCoroutine(SetDestinationCoroutine(destination));
    }

    private IEnumerator SetDestinationCoroutine(Vector2 destination)
    {
        EndCurrentPath();

        yield return new WaitUntil(() => _currentlyFollowingPath == null);

        Vector3 targetPos = new Vector3(destination.x, destination.y, transform.position.z);
        _seeker.StartPath(transform.position, targetPos, OnPathComplete);
    }

    public void SetDestination(PointAndClickObject sender, Transform destinationNode)
    {
        if (_movementOverrideCoroutine != null || _currentlyFollowingPath != null) { return; }

        if (destinationNode != null)
        {
            SetDestination(destinationNode.position);
        }
        else
        {
            SetDestination(sender.transform.position);
        }

        _movementOverrideCoroutine = StartCoroutine(MoveToObjectCoroutine(sender));
    }

    private IEnumerator MoveToObjectCoroutine(PointAndClickObject pncObject)
    {
        yield return new WaitUntil(() => _currentlyFollowingPath == null);
        yield return new WaitForEndOfFrame();

        yield return new WaitUntil(() => _path != null);

        yield return new WaitUntil(() => _currentlyFollowingPath == null);
        yield return new WaitForEndOfFrame();

        if (pncObject != null) { pncObject.SendMessage("ObjectReached", gameObject.name); }

        _movementOverrideCoroutine = null;
    }

    private void OnPathComplete(Path p)
    {
        if (p.error)
        {
            Debug.LogError("Path calculation failed " + p.errorLog);
            return;
        }

        _path = (ABPath)p;

        Debug.DrawLine(_path.endPoint, _path.originalEndPoint, Color.magenta, 2f);

        StartNewPath();
    }

    private void StartNewPath()
    {
        _currentlyFollowingPath = StartCoroutine(FollowPath());
    }

    public void EndCurrentPath()
    {
        if (_currentlyFollowingPath != null)
        {
            _interruptMovement = true;
        }

        if (_movementOverrideCoroutine != null)
        {
            StopCoroutine(_movementOverrideCoroutine);
        }
    }

    private IEnumerator FollowPath()
    {
        yield return new WaitUntil(() => _path != null);

        if (_path.path.Count > 1)
        {
            Vector3 currentLine;

            float distanceOnLine;

            for (int i = 1; i < _path.path.Count - 1; i++)
            {
                currentLine = (Vector3)(_path.path[i].position - _path.path[i - 1].position);
                _currentSpeed = Vector3.Dot(_currentVelocity, currentLine) / currentLine.magnitude;

                distanceOnLine = 0f;
                do
                {
                    _currentSpeed = Mathf.Clamp(_currentSpeed + _acceleration * Time.deltaTime, 0f, maxSpeed);
                    _currentVelocity = _currentSpeed * currentLine.normalized;

                    distanceOnLine += _currentSpeed * Time.deltaTime;

                    transform.position = Vector3.Lerp((Vector3)_path.path[i - 1].position, (Vector3)_path.path[i].position, distanceOnLine / currentLine.magnitude);

                    yield return null;
                } while (distanceOnLine < currentLine.magnitude - nodeReachedDistance);

                Debug.Log($"Reached node {i} of {_path.path.Count - 1}");

                if (_interruptMovement)
                {
                    _interruptMovement = false;

                    Coroutine temp = _currentlyFollowingPath;
                    _currentlyFollowingPath = null;
                    StopCoroutine(temp);
                }
            }

            currentLine = (Vector3)(_path.endNode.position - _path.path[_path.path.Count - 2].position);
            _currentSpeed = Vector3.Dot(_currentVelocity, currentLine) / currentLine.magnitude;
            distanceOnLine = 0f;
            do
            {
                float decelerationDistance = -Mathf.Pow(_currentSpeed, 2f) / (2 * -_acceleration);
                if (currentLine.magnitude - distanceOnLine > decelerationDistance || _interruptMovement)
                {
                    _currentSpeed = Mathf.Clamp(_currentSpeed + _acceleration * Time.deltaTime, 0f, maxSpeed);
                }
                else
                {
                    _currentSpeed = Mathf.Clamp(_currentSpeed - _acceleration * Time.deltaTime, 0f, maxSpeed);
                }

                distanceOnLine += _currentSpeed * Time.deltaTime;

                transform.position = Vector3.Lerp((Vector3)_path.path[_path.path.Count - 2].position, (Vector3)_path.endNode.position, distanceOnLine / currentLine.magnitude);

                yield return null;
            } while (distanceOnLine < currentLine.magnitude - nodeReachedDistance);

            Debug.Log($"Reached final node of path");
        }

        _currentSpeed = 0f;
        _currentVelocity = Vector3.zero;

        _interruptMovement = false;
        _currentlyFollowingPath = null;
    }
}
