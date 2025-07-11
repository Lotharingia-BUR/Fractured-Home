using UnityEngine;
using UnityEngine.UI;

public class InventoryDrawer : MonoBehaviour
{
    public RectTransform drawerPanel;
    public Button toggleButton;
    public float slideSpeed = 5f;

    private Vector2 shownPosition;
    private Vector2 hiddenPosition;
    private bool isOpen = false;

    void Start()
    {
        // Get current shown position
        shownPosition = drawerPanel.anchoredPosition;
        // Set hidden position (off screen to the left)
        hiddenPosition = shownPosition + new Vector2(-drawerPanel.rect.width * 0.82f, 0);

        // Start hidden
        drawerPanel.anchoredPosition = hiddenPosition;

        // Set up button
        toggleButton.onClick.AddListener(ToggleDrawer);
    }

    void ToggleDrawer()
    {
        isOpen = !isOpen;
    }

    public void SetToggle(bool open)
    {
        if (isOpen != open)
        {
            ToggleDrawer();
        }
    }

    void Update()
    {
        Vector2 target = isOpen ? shownPosition : hiddenPosition;
        drawerPanel.anchoredPosition = Vector2.Lerp(drawerPanel.anchoredPosition, target, Time.deltaTime * slideSpeed);
    }
}
