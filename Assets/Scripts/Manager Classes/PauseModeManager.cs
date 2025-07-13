using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Pause Mode enum
//Represents the paused/unpaused state of the scene
public enum PauseMode
{
    Unpaused,
    Dialogue,
    FadeOut
}

//Pause Mode Manager
//Class that manages pausing and unpausing objects in the scene
public class PauseModeManager : Manager<PauseModeManager>
{
    //The current paused/unpaused state
    public PauseMode pauseMode;

    //Canvas holding the dialogue box
    public GameObject dialogueBox;

    protected override void Initialize()
    {
        base.Initialize();

        SetPauseMode(pauseMode);
    }

    /// <summary>
    /// Set Pause Mode method
    /// Sets the paused/unpaused state of the scene and updates objects accordingly
    /// </summary>
    /// <param name="mode"> The paused/unpaused state to change to </param>
    public void SetPauseMode(PauseMode pauseMode)
    {
        switch (pauseMode)
        {
            case PauseMode.Dialogue:
                dialogueBox.SetActive(true);

                foreach (var v in FindObjectsByType<PointAndClickCharacterController>(FindObjectsSortMode.None))
                {
                    v.enabled = false;
                }

                foreach (var v in FindObjectsByType<Interactable>(FindObjectsSortMode.None))
                {
                    v.enabled = false;
                }

                foreach (var v in FindObjectsByType<EventTriggerBox>(FindObjectsSortMode.None))
                {
                    v.enabled = false;
                }

                foreach (var v in FindObjectsByType<ConditionalEvent>(FindObjectsSortMode.None))
                {
                    v.enabled = false;
                }
                break;

            case PauseMode.FadeOut:
                dialogueBox.SetActive(false);

                foreach (var v in FindObjectsByType<PointAndClickCharacterController>(FindObjectsSortMode.None))
                {
                    v.enabled = false;
                }

                foreach (var v in FindObjectsByType<Interactable>(FindObjectsSortMode.None))
                {
                    v.enabled = false;
                }

                foreach (var v in FindObjectsByType<EventTriggerBox>(FindObjectsSortMode.None))
                {
                    v.enabled = false;
                }

                foreach (var v in FindObjectsByType<ConditionalEvent>(FindObjectsSortMode.None))
                {
                    v.enabled = false;
                }
                break;

            case PauseMode.Unpaused:
            default:
                dialogueBox.SetActive(false);

                foreach (var v in FindObjectsByType<PointAndClickCharacterController>(FindObjectsSortMode.None))
                {
                    v.enabled = true;
                }

                foreach (var v in FindObjectsByType<Interactable>(FindObjectsSortMode.None))
                {
                    v.enabled = true;
                }

                foreach (var v in FindObjectsByType<EventTriggerBox>(FindObjectsSortMode.None))
                {
                    v.enabled = false;
                }

                foreach (var v in FindObjectsByType<ConditionalEvent>(FindObjectsSortMode.None))
                {
                    v.enabled = false;
                }
                break;
        }

        this.pauseMode = pauseMode;

        Debug.Log("Pause mode set to " + pauseMode.ToString());
    }

    public void SetPauseMode(int pauseMode)
    {
        SetPauseMode((PauseMode)pauseMode);
    }
}
