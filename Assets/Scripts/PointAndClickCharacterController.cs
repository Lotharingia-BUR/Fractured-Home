using System.Collections;
using Pathfinding;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointAndClickCharacterController : MonoBehaviour
{
    public float maxSpeed = 5f;
    public float accelerationTime = 0.2f;

    public float nodeReachedDistance = 0.01f;

    private float _currentSpeed;
    private Vector3 _currentVelocity;
    private float _acceleration;

    private Coroutine _movementOverrideCoroutine = null;
    private Coroutine _currentlyFollowingPath = null;

    private ABPath _path;

    private Seeker _seeker;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _seeker = GetComponent<Seeker>();

        _acceleration = maxSpeed / accelerationTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SetDestination(mouseWorldPos);
        }
    }

    public void SetDestination(Vector2 destination)
    {
        if (_movementOverrideCoroutine != null) { return; }

        Vector3 targetPos = new Vector3(destination.x, destination.y, transform.position.z);

        _seeker.StartPath(transform.position, targetPos, OnPathComplete);
    }

    public void SetDestination(PointAndClickObject sender, Transform destinationNode)
    {
        if (destinationNode != null)
        {
            SetDestination(destinationNode.position);
        }
        else
        {
            SetDestination(destinationNode.transform.position);
        }

        _movementOverrideCoroutine = StartCoroutine(MoveToObjectCoroutine(sender));
    }

    private IEnumerator MoveToObjectCoroutine(PointAndClickObject pncObject)
    {
        yield return new WaitUntil(() => _path != null);

        yield return new WaitUntil(() => (_path.endPoint - transform.position).magnitude <= 0.2);

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
        if (_currentlyFollowingPath != null)
        {
            StopCoroutine(_currentlyFollowingPath);
        }

        _currentlyFollowingPath = StartCoroutine(FollowPath());
    }

    private IEnumerator FollowPath()
    {
        yield return new WaitUntil(() => _path != null);

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
        }

        currentLine = (Vector3)(_path.endNode.position - _path.path[_path.path.Count - 2].position);
        _currentSpeed = Vector3.Dot(_currentVelocity, currentLine) / currentLine.magnitude;
        distanceOnLine = 0f;
        do
        {
            float decelerationDistance = -Mathf.Pow(_currentSpeed, 2f) / (2 * -_acceleration);
            _currentSpeed = currentLine.magnitude - distanceOnLine > decelerationDistance ? Mathf.Clamp(_currentSpeed + _acceleration * Time.deltaTime, 0f, maxSpeed) : _currentSpeed - _acceleration * Time.deltaTime;

            distanceOnLine += _currentSpeed * Time.deltaTime;

            transform.position = Vector3.Lerp((Vector3)_path.path[_path.path.Count - 2].position, (Vector3)_path.endNode.position, distanceOnLine / currentLine.magnitude);

            yield return null;
        } while (distanceOnLine < currentLine.magnitude - nodeReachedDistance);

        Debug.Log($"Reached final node of path");

        _currentlyFollowingPath = null;
    }
}
