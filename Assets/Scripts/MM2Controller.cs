using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MM2Controller : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button creditsButton; // Optional: reference for clarity

    private void Awake()
    {
        if (startButton != null)
            startButton.onClick.AddListener(OnStartButtonClicked);

        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitButtonClicked);

        if (creditsButton != null)
            creditsButton.onClick.AddListener(OnCreditsButtonClicked);

        if (DialogueManager.Instance != null)
        {
            Destroy(DialogueManager.Instance.transform.root.gameObject);
        }
        if (SceneManagerComponent.Instance != null)
        {
            Destroy(SceneManagerComponent.Instance.transform.root.gameObject);
        }
    }

    private void OnStartButtonClicked()
    {
        SceneManager.LoadScene("Lawn");
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
        Debug.Log("Quit button clicked - app will close when built.");
    }

    public void OnCreditsButtonClicked()
    {
        SceneManager.LoadScene("Credits"); // Change to your actual credits scene name
    }
}


