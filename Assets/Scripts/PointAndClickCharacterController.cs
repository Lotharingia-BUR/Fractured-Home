using Pathfinding;
using UnityEngine;

public class PointAndClickCharacterController : MonoBehaviour
{


    private ABPath _path;

    private Seeker _navAgent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _navAgent = GetComponent<Seeker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetPos = new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z);

            _navAgent.StartPath(transform.position, targetPos, OnPathComplete);
        }
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
