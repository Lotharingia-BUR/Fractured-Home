using UnityEngine;
using UnityEngine.UIElements;

public class UISoundPlayer : MonoBehaviour
{
    public AudioClip clickSound;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        var ui = GetComponent<UIDocument>();
        if (ui == null) return;

        var root = ui.rootVisualElement;
        // Add sound to every button
        var buttons = root.Query<Button>().ToList();

        foreach (var btn in buttons)
        {
            btn.clicked += () => {
                audioSource.PlayOneShot(clickSound);
            };
        }
    }
}
