using UnityEngine;

public class BoundedFollowCamera : MonoBehaviour
{
    // The target for the camera to follow
    public Transform target;

    //TODO: Make bounds adjustable in scene view
    public Bounds bounds;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
        //TODO: Apply bounds to camera position
    }
}
