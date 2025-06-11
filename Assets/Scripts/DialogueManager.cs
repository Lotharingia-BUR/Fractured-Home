using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using TMPro;

//Dialogue Manager
//Handles reading dialogue from an Ink story asset and writing it to the dialogue box
public class DialogueManager : Singleton<DialogueManager>
{
    //Ink story asset to read dialogue from
    public TextAsset inkAsset;
    //UI text to write dialogue to
    public TMP_Text mainText;

    //Story object loaded from Ink asset
    private Story story;

    //Currently selected dialogue choice
    private int _selectedChoice = 0;

    protected override void Initialize()
    {
        base.Initialize();

        SetStory(inkAsset);
    }

    /// <summary>
    /// Reset the current Ink story and set the given Ink JSON asset as the current story
    /// </summary>
    /// <param name="inkStory"></param>
    public void SetStory(TextAsset inkStory)
    {
        story?.ResetState();

        //Load story object
        story = new Story(inkStory.text);
        mainText.text = story.Continue();

        PauseModeManager.Instance.SetPauseMode(PauseMode.Dialogue);

        Debug.Log("Story set to " + inkStory.name);
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Add Pause manager
        //if (PauseModeManager.Instance.pauseMode != PauseMode.Dialogue) { return; }

        if (story.canContinue)
        {
            //Play next line of dialogue on interaction
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                LoadNextDialogue();
            }
        }
        else if (story.currentChoices.Count > 0)
        {
            //Navigate dialogue options
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                _selectedChoice--;
                _selectedChoice = Mathf.Clamp(_selectedChoice, 0, story.currentChoices.Count - 1);
            }
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                _selectedChoice++;
                _selectedChoice = Mathf.Clamp(_selectedChoice, 0, story.currentChoices.Count - 1);
            }

            //Write dialogue options to UI
            mainText.text = story.currentText + (story.currentText == "" ? "" : "\n \n");
            for (int i = 0; i < story.currentChoices.Count; i++)
            {
                if (i == _selectedChoice)
                {
                    mainText.text += "> ";
                }
                mainText.text += story.currentChoices[i].text + "\n";
            }

            //Select dialogue option on interaction
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                story.ChooseChoiceIndex(_selectedChoice);
                LoadNextDialogue();
            }
        }
        else
        {
            //If there is no more dialogue, resume game on interaction
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ResetStory();
            }
        }
    }

    public void LoadNextDialogue()
    {
        mainText.text = story.Continue();
    }

    public void ResetStory()
    {
        story.ResetState();
        story = null;

        PauseModeManager.Instance.SetPauseMode(PauseMode.Unpaused);
    }
}
