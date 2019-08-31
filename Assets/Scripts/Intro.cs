using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour {

    public DialogueTrigger firstDialogue;

    private void Start()
    {

        Invoke("StartIntro", 3f);

    }

    void StartIntro()
    {

        firstDialogue.TriggerDialogue();

    }

}
