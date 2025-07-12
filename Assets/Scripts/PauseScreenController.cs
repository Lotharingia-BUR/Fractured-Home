using UnityEngine;
using UnityEngine.UIElements;

public class PauseScreenController : MonoBehaviour
{
    private UIDocument pauseDocument;
    private VisualElement pauseVT;

    private Button resumeBtn;
    private Button quitBtn;

    private bool isPaused = false;

    private void Awake()
    {
        pauseDocument = GetComponent<UIDocument>();
        if (pauseDocument == null)
        {
            Debug.LogError("UIDocument is missing from this GameObject!");
            return;
        }

        pauseVT = pauseDocument.rootVisualElement;
        if (pauseVT == null)
        {
            Debug.LogError("RootVisualElement is null!");
            return;
        }

        // Safely try to find buttons
        resumeBtn = pauseVT.Q<Button>("ResumeBtn");
        quitBtn = pauseVT.Q<Button>("QuitBtn");

        if (resumeBtn != null)
            resumeBtn.clicked += ResumeGame;
        else
            Debug.LogWarning("ResumeBtn not found in UI!");

        if (quitBtn != null)
            quitBtn.clicked += OnQuitButtonClicked;
        else
            Debug.LogWarning("QuitBtn not found in UI!");

        // Hide pause UI at start
        pauseVT.style.display = DisplayStyle.None;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseVT.style.display = DisplayStyle.Flex;
        Debug.Log("Game Paused");
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseVT.style.display = DisplayStyle.None;
        Debug.Log("Game Resumed");
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
        Debug.Log("Quit button clicked – will quit in build.");
    }
}

