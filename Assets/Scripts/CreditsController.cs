using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    public void OnBackToMenuClicked()
    {
        SceneManager.LoadScene("Main Menu"); 
    }
}
