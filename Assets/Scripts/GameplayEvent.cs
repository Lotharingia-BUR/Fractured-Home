using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameplayEvent
{
    public UnityEvent[] events;

    public bool isRunning { get; private set; }

    public IEnumerator Run()
    {
        isRunning = true;

        for (int i = 0; i < events.Length; i++)
        {
            events[i]?.Invoke();

            yield return new WaitUntil(() => PauseModeManager.Instance.pauseMode == PauseMode.Unpaused);
        }

        isRunning = false;
    }
}
