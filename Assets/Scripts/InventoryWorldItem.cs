using UnityEngine;

public class InventoryWorldItem : MonoBehaviour
{
    public InventoryItem item;
    public InventoryUIItem linkedUIItem;

    private SpriteRenderer _spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    public void Initialize(InventoryUIItem sender, InventoryItem item)
    {
        linkedUIItem = sender;

        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        this.item = item;
        _spriteRenderer.sprite = item.sprite;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Release(bool itemUsed)
    {
        if (itemUsed)
        {
            Destroy(linkedUIItem.gameObject);
        }
        else
        {
            linkedUIItem.ResetState();
        }

        Destroy(gameObject);
    }
}
