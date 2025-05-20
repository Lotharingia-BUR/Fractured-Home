using UnityEngine;

public class MouseController : MonoBehaviour
{
    public Texture2D defaultCursor;    // cursor placeholder (default)
    public Texture2D interactiveCursor;  // Hand cursor placeholder for interactables
    public Texture2D clickedCursor; //clicked cursor state 
    private Vector2 hotspot = Vector2.zero; // The "click point" for the cursor (top-left)

    void Start()
    {
        // Set the default cursor when the game starts
        Cursor.visible = true; // Ensure system cursor is visible
        Cursor.SetCursor(defaultCursor, hotspot, CursorMode.Auto); // Set the default cursor (red circle)
    }

    void Update()
    {
        // Raycast to check if we are over an interactive object (2D raycast)
        Vector3 mousePos = Input.mousePosition;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePos), Vector2.zero);

        // If the raycast hits something tagged "Interactable", change the cursor to the hand
        if (hit.collider != null && hit.collider.CompareTag("Interactable"))
        {
            Cursor.SetCursor(interactiveCursor, hotspot, CursorMode.Auto); // Change to interactive cursor


            //detect mouse when clicked
            if (Input.GetMouseButtonDown(0)) //left mouse button click
            {
            //change colour of the interactable object to white
            hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
              
                
            }
        }
        else
        {
            Cursor.SetCursor(defaultCursor, hotspot, CursorMode.Auto);  // Set to default cursor placeholder

        }

        }
    }



