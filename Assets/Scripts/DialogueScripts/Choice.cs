using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Choice {

    [TextArea(3, 10)]
    public string[] sentences = new string[2];

    public DialogueTrigger[] dialogues = new DialogueTrigger[2];



}
