using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Scene Manager Component
// MonoBehaviour class that acts as an interface to the Scene Manager class
public class SceneManagerComponent : Manager<SceneManagerComponent>
{
    public float transitionTime = 1f;

    private Animator _animator;

    private int _fadeInHash, _fadeOutHash;

    protected override void Initialize()
    {
        base.Initialize();

        _animator = GetComponent<Animator>();

        _fadeInHash = Animator.StringToHash("FadeIn");
        _fadeOutHash = Animator.StringToHash("FadeOut");
    }

    public void FadeOut(float duration) => StartCoroutine(FadeOutCoroutine(duration));

    private IEnumerator FadeOutCoroutine(float duration)
    {
        _animator.SetTrigger(_fadeOutHash);
        PauseModeManager.Instance.SetPauseMode(PauseMode.FadeOut);

        yield return new WaitForSeconds(duration);

        _animator.SetTrigger(_fadeInHash);
        PauseModeManager.Instance.SetPauseMode(PauseMode.Unpaused);
    }

    private IEnumerator SceneTransition(string sceneName)
    {
        _animator.SetTrigger(_fadeOutHash);
        PauseModeManager.Instance.SetPauseMode(PauseMode.FadeOut);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName);

        yield return new WaitForSeconds(transitionTime);

        _animator.SetTrigger(_fadeInHash);
        PauseModeManager.Instance.SetPauseMode(PauseMode.Unpaused);
    }

    private IEnumerator SceneTransition(int sceneID)
    {
        _animator.SetTrigger(_fadeOutHash);
        PauseModeManager.Instance.SetPauseMode(PauseMode.FadeOut);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneID);

        yield return new WaitForSeconds(transitionTime);

        _animator.SetTrigger(_fadeInHash);
        PauseModeManager.Instance.SetPauseMode(PauseMode.Unpaused);
    }

    public void LoadScene(string sceneName) => StartCoroutine(SceneTransition(sceneName));
    public void LoadScene(int sceneID) => StartCoroutine(SceneTransition(sceneID));
}
