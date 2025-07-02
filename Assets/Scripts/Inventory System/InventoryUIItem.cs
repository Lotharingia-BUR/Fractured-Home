using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InventoryUIItem : UIHoverable
{
    public InventoryItem item;

    private Image _image;

    protected override void Initialize()
    {
        base.Initialize();

        _image = GetComponent<Image>();
    }

    protected override void LogicUpdate()
    {
        base.LogicUpdate();

        _image.sprite = item?.sprite;
    }

    public void SetVisibility(bool visible)
    {
        _image.enabled = visible;
    }

    protected override void OnPointerDown()
    {
        if (item == null) { return; }

        MouseController.Instance.draggedItem = this;
    }

    public void Release(bool used)
    {
        if (used)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }
    }
}
