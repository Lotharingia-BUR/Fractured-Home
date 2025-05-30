using UnityEngine;

public class ObjectController : MonoBehaviour
{
     BoxCollider2D colliderBox; //detecting the box collider in the object

    void Start()
    {
        // Set the default cursor when the game starts
        colliderBox = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // Raycast to check if we are over an interactive object (2D raycast)
        Vector3 mousePos = Input.mousePosition;


        //If the first GameObject's Bounds contains the Transform's position, output a message in the console
        if (colliderBox.bounds.Contains(mousePos))
        {
            Debug.Log("object");
        }




    }
}



