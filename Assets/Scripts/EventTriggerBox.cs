using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class EventTriggerBox : MonoBehaviour
{
    public string triggerID;
    [Tooltip("Whether the event should only trigger once ever or the player should be able to re-trigger it by re-entering the trigger area")]
    public bool triggerOnce = true;

    public GameplayEvent onTriggerEvent;

    private Coroutine eventCoroutine = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (triggerOnce)
        {
            if (triggerID == null || triggerID == string.Empty)
            {
                triggerID = gameObject.name;
            }

            TriggerObjectState state = PersistentObjectStateManager.Instance.GetTriggerState(triggerID);
            if (state.wasTriggered)
            {
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hitsArray = Physics2D.RaycastAll(mousePos, Vector2.zero);

        foreach (RaycastHit2D hit in hitsArray)
        {
            Interactable o = hit.collider?.GetComponent<Interactable>();
            if (o != null)
            {
                o.Click();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        PointAndClickCharacterController chara = collision.GetComponent<PointAndClickCharacterController>();
        if (chara != null)
        {
            chara.EndCurrentPath();

            if (eventCoroutine == null) { eventCoroutine = StartCoroutine(EventCoroutine()); }
        }
    }

    private IEnumerator EventCoroutine()
    {
        yield return new WaitUntil(() => PauseModeManager.Instance.pauseMode == PauseMode.Unpaused);

        StartCoroutine(onTriggerEvent.Run());

        yield return new WaitUntil(() => !onTriggerEvent.isRunning);

        if (triggerOnce)
        {
            PersistentObjectStateManager.Instance.SaveTriggerState(triggerID, true);
            Destroy(gameObject);
        }
    }
}
