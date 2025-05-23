using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerComponent : Manager<SceneManagerComponent>
{
    public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);
    public void LoadScene(int sceneID) => SceneManager.LoadScene(sceneID);
}
