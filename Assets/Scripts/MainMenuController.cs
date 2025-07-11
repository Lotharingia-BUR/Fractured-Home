using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

//grabbing the buttons from the ui document to be recognized for click events
public class MainMenuController : MonoBehaviour
{

    private Button _startBtn;
    private AudioSource _audioSource;


    //   private Button _saveBtn;
    //   private Button _settingsBtn;
    //   private Button _creditsBtn;
    //   private Button _exitBtn;

    private void Awake()
    {
        

       // _startBtn = _doc.rootVisualElement.Q<Button>("StartGameBtn");
       var uiDocument = GetComponent<UIDocument>();
     //   _doc = uiDocument.rootVisualElement;
        var root = uiDocument.rootVisualElement;

        _startBtn = root.Q<Button>("StartBtn");
        _startBtn.clicked += StartBtnOnClicked; // go to the first level of the game
    }

    private void StartBtnOnClicked()
    {
        
        SceneManager.LoadScene("Lawn"); //load main scene
    }
}
        //_saveBtn = _doc.rootVisualElement.Q<Button>("SaveBtn");
       // _settingsBtn = _doc.rootVisualElement.Q<Button>("SettingsBtn");
       // _exitBtn = _doc.rootVisualElement.Q<Button>("ExitBtn");
  //  }
//}
