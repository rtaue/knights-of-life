using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public Dialogue dialogue;

    public DialogueTrigger nextDialogue;
    public ChoiceTrigger choice;
    public string sceneToLoad;

    public void TriggerDialogue()
    {

        DialogueManager _dialogueManager = DialogueManager.instance;
        _dialogueManager.StartDialogue(dialogue);
        _dialogueManager.nextDialogue = nextDialogue;
        _dialogueManager.choice = choice;
        _dialogueManager.sceneToLoad = sceneToLoad;

    }

}
