using UnityEngine;

public class MouseController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 MousePos = Input.mousePosition;
        transform.position = Camera.main.ScreenToWorldPoint(MousePos) - new Vector3 (0,0,5);
    }
}
