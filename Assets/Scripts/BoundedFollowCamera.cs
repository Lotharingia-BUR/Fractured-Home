using UnityEngine;
using System.Collections;
using UnityEditor.IMGUI.Controls;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class BoundedFollowCamera : MonoBehaviour
{
    public Transform target;
    public Bounds bounds;
    public Vector2 offset;
    public Camera followCamera;
    public float smoothing;

    private Vector3 _viewportHalfSize;

    private Vector3 _shakeOffset;

    // Start is called before the first frame update
    void Start()
    {
        _viewportHalfSize = new(followCamera.aspect * followCamera.orthographicSize, followCamera.orthographicSize);

        bounds.extents -= _viewportHalfSize;
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + new Vector3(offset.x, offset.y, transform.position.z) + _shakeOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, 1 - Mathf.Exp(-smoothing * Time.deltaTime));

        smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, bounds.min.x, bounds.max.x);
        smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, bounds.min.y, bounds.max.y);

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


#if UNITY_EDITOR
[CustomEditor(typeof(BoundedFollowCamera))]
public class BoundedFollowCameraEditor : Editor
{
    private BoxBoundsHandle _boundsHandle = new BoxBoundsHandle();

    private bool _editBoundsMode = false;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (!_editBoundsMode && EditorGUILayout.LinkButton("Edit Camera Bounds"))
        {
            _editBoundsMode = true;
        }
        else if (_editBoundsMode && EditorGUILayout.LinkButton("Stop Editing Bounds"))
        {
            _editBoundsMode = false;
        }
    }

    void OnSceneGUI()
    {
        if (!_editBoundsMode) { return; }

        BoundedFollowCamera obj = (BoundedFollowCamera)target;

        _boundsHandle.SetColor(Color.blue);
        _boundsHandle.center = obj.bounds.center;
        _boundsHandle.size = obj.bounds.size;

        EditorGUI.BeginChangeCheck();
        _boundsHandle.DrawHandle();
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(obj, "Change Camera Bounds");

            obj.bounds.center = _boundsHandle.center;
            obj.bounds.extents = _boundsHandle.size / 2;
        }
    }
}
#endif