using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InventoryUIItem : MonoBehaviour
{
    public InventoryItem item;

    private Image _image;

    void OnValidate()
    {
        if (_image == null)
        {
            _image = GetComponent<Image>();
        }

        _image.sprite = item.sprite;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        item.onDestroyEvent += () => Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnWorldItem()
    {
        //TODO: Create InventoryWorldItem class

        // Instantiate correct InventoryWorldItem

        _image.enabled = false;
    }

    //Call this when the linked WorldItem is released
    public void ResetState()
    {
        _image.enabled = true;
    }
}
