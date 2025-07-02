using System.Collections;
using Pathfinding;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointAndClickCharacterController : MonoBehaviour
{
    private Coroutine _movementOverrideCoroutine = null;

    private ABPath _path;

    private Seeker _seeker;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _seeker = GetComponent<Seeker>();
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

    public void SetDestination(PointAndClickObject destinationObject)
    {
        if (destinationObject.objectDestinationNode != null)
        {
            SetDestination(destinationObject.objectDestinationNode.position);
        }
        else
        {
            SetDestination(destinationObject.transform.position);
        }

        _movementOverrideCoroutine = StartCoroutine(MoveToObjectCoroutine(destinationObject));
    }

    private IEnumerator MoveToObjectCoroutine(PointAndClickObject pncObject)
    {
        yield return new WaitUntil(() => _path != null);

        yield return new WaitUntil(() => (_path.endPoint - transform.position).magnitude <= 0.2);

        pncObject.SendMessage("ObjectReached");

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
    }
}
