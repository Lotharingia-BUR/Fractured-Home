using UnityEngine;
using UnityEngine.UI;

public class MM2Controller : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject creditsPanel;

    private void Awake()
    {
        if (startButton != null)
            startButton.onClick.AddListener(OnStartButtonClicked);

        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lawn");
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
        Debug.Log("Quit button clicked - app will close when built.");
    }

    public void OnCreditsButtonClicked()
    {
        creditsPanel.SetActive(true);
    }

    public void OnBackFromCreditsClicked()
    {
        creditsPanel.SetActive(false);
    }
}

