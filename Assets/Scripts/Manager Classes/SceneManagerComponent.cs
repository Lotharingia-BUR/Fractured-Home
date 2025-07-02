using UnityEngine;
using UnityEngine.SceneManagement;

// Scene Manager Component
// MonoBehaviour class that acts as an interface to the Scene Manager class
public class SceneManagerComponent : Manager<SceneManagerComponent>
{
    public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);
    public void LoadScene(int sceneID) => SceneManager.LoadScene(sceneID);
}
