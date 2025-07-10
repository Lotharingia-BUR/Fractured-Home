using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class EventTriggerBox : MonoBehaviour
{
    public string triggerID;
    [Tooltip("Whether the event should only trigger once ever or the player should be able to re-trigger it by re-entering the trigger area")]
    public bool triggerOnce = true;

    public GameplayEvent onTriggerEvent;

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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PointAndClickCharacterController>() != null)
        {
            StartCoroutine(onTriggerEvent.Run());

            if (triggerOnce)
            {
                PersistentObjectStateManager.Instance.SaveTriggerState(triggerID, true);
                Destroy(gameObject);
            }
        }
    }
}
