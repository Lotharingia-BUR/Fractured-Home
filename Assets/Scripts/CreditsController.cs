using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    public AnimationCurve movementCurve;
    public float scrollDuration;

    public Camera creditsCamera;

    public Vector3 startPosition;
    public Vector3 endPosition;

    private float _t;

    private float _fadeInTimer = 0.5f;

    void Start()
    {
        if (DialogueManager.Instance != null)
        {
            Destroy(DialogueManager.Instance.transform.root.gameObject);
        }
        if (SceneManagerComponent.Instance != null)
        {
            Destroy(SceneManagerComponent.Instance.transform.root.gameObject);
        }
    }

    void Update()
    {
        if (_fadeInTimer > 0f)
        {
            _fadeInTimer -= Time.deltaTime;
            return;
        }

        creditsCamera.transform.position = Vector3.Lerp(startPosition, endPosition, movementCurve.Evaluate(_t));

        _t += Time.deltaTime / scrollDuration;
    }

    public void OnBackToMenuClicked()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
