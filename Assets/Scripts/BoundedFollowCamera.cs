using UnityEngine;
using System.Collections;
using UnityEditor.IMGUI.Controls;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class BoundedFollowCamera : MonoBehaviour
{
    public Transform target;
    public GameObject background;
    public Vector2 offset;
    public float smoothing;

    private Camera followCamera;
    private Vector3 _viewportHalfSize;
    private Bounds bounds;

    private Vector3 _shakeOffset;

    // Start is called before the first frame update
    void Start()
    {
        followCamera = GetComponent<Camera>();

        CalculateBounds();

        _viewportHalfSize = new(followCamera.aspect * followCamera.orthographicSize, followCamera.orthographicSize);

        bounds.extents -= _viewportHalfSize;

        Vector3 desiredPosition = target.position + new Vector3(offset.x, offset.y, transform.position.z);
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, bounds.min.x, bounds.max.x);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, bounds.min.y, bounds.max.y);
        desiredPosition.z = transform.position.z;

        transform.position = desiredPosition;
    }

    private void CalculateBounds()
    {
        bounds.center = background.transform.position;
        Vector3 bgExtents = background.GetComponent<MeshFilter>().mesh.bounds.extents;
        Vector3 bgScale = background.transform.lossyScale;
        Vector3 scaledExtents = new(bgExtents.x * bgScale.x, bgExtents.y * bgScale.y, bgExtents.z * bgScale.z);
        bounds.extents = Quaternion.Inverse(background.transform.rotation) * scaledExtents;
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + new Vector3(offset.x, offset.y, transform.position.z) + _shakeOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, 1 - Mathf.Exp(-smoothing * Time.deltaTime));

        smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, bounds.min.x, bounds.max.x);
        smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, bounds.min.y, bounds.max.y);
        smoothedPosition.z = transform.position.z;

        transform.position = smoothedPosition;
    }

    public void Shake(float intensity, float duration)
    {
        StartCoroutine(ShakeCoroutine(intensity, duration));
    }

    private IEnumerator ShakeCoroutine(float intensity, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            _shakeOffset = Random.insideUnitCircle * intensity;
            elapsed += Time.deltaTime;
            yield return null;
        }
        _shakeOffset = Vector3.zero;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(bounds.center, bounds.extents * 2);
    }
}