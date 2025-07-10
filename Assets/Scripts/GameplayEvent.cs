using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameplayEvent
{
    public UnityEvent[] events;

    public IEnumerator Run()
    {
        for (int i = 0; i < events.Length; i++)
        {
            yield return new WaitUntil(() => PauseModeManager.Instance.pauseMode == PauseMode.Unpaused);

            events[i]?.Invoke();
        }
    }
}
