using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : Manager<MouseController>
{
    public Texture2D defaultCursor;    // cursor placeholder (default)
    public Texture2D interactiveCursor;  // Hand cursor placeholder for interactables
    public Texture2D clickedCursor; //clicked cursor state 
    private Vector2 hotspot; // The "click point" for the cursor

    public InventoryUIItem draggedItem = null;

    public bool isOverObject { get; private set; }

    protected override void Initialize()
    {
        base.Initialize();

        hotspot = new(defaultCursor.width / 2, defaultCursor.height / 2);

        // Set the default cursor when the game starts
        Cursor.visible = true; // Ensure system cursor is visible
        Cursor.SetCursor(defaultCursor, hotspot, CursorMode.Auto); // Set the default cursor (red circle)
    }

    void Update()
    {
        // TODO: Refactor this to clean up the nested if statements

        // Raycast to check if we are over an interactive object (2D raycast)
        Vector3 mousePos = Input.mousePosition;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePos), Vector2.zero);

        // If the raycast hits something with the Interactable component, change the cursor to the hand
        if ((hit.collider?.gameObject != null && hit.collider.gameObject.tag == "Interactable") || hit.collider?.GetComponent<Interactable>() != null)
        {
            isOverObject = true;

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Cursor.SetCursor(interactiveCursor, hotspot, CursorMode.Auto); // Change to interactive cursor
            }


            //detect mouse when clicked
            if (Input.GetMouseButtonDown(0)) //left mouse button click
            {
                //TODO: This doesn't do anything since the color is already white
                //change colour of the interactable object to white
                //Also throws and error like 50% of the time cause we're mixing SpriteRenderers and Meshes
                //hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.white;


            }

            PointAndClickObject pcObj = hit.collider.GetComponent<PointAndClickObject>();
            if (draggedItem != null && pcObj != null && Input.GetKeyUp(KeyCode.Mouse0) && draggedItem.item.id == pcObj.itemKey?.id)
            {
                draggedItem.Release(true);
                pcObj.Unlock();
            }
        }
        else
        {
            Cursor.SetCursor(defaultCursor, hotspot, CursorMode.Auto);  // Set to default cursor placeholder

            if (EventSystem.current.IsPointerOverGameObject())
            {
                isOverObject = true;
            }
            else
            {
                isOverObject = false;
            }
        }

        if (draggedItem != null)
        {
            draggedItem.transform.position = mousePos;

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                draggedItem.Release(false);
                draggedItem = null;
            }
        }
    }
}



