using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class PauseScreenController : MonoBehaviour
{
    private UIDocument pauseDocument;

    private VisualElement pauseVT;

    private UnityEngine.UIElements.Button resumeBtn;



    private bool isPaused = false;

    private void Awake()
    {
        pauseDocument = GetComponent<UIDocument>();
        pauseVT = pauseDocument.rootVisualElement;

        // Reference the button by the name in UXML
        resumeBtn = pauseVT.Q<UnityEngine.UIElements.Button>("ResumeBtn");


        // Hide pause menu initially
        pauseVT.style.display = DisplayStyle.None;

        // Hook up button functionality
        resumeBtn.clicked += ResumeGame;
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

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseVT.style.display = DisplayStyle.Flex;
    }

    void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseVT.style.display = DisplayStyle.None;
    }
}
