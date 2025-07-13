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

    void Start()
    {

    }

    void Update()
    {
        creditsCamera.transform.position = Vector3.Lerp(startPosition, endPosition, movementCurve.Evaluate(_t));

        _t += Time.deltaTime / scrollDuration;
    }

    public void OnBackToMenuClicked()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
