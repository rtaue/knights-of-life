using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceTrigger : MonoBehaviour {

    public Choice choice;

    public void TriggerChoice()
    {

        ChoiceManager _choiceManager = ChoiceManager.instance;
        _choiceManager.StartChoice(choice);

    }
}
