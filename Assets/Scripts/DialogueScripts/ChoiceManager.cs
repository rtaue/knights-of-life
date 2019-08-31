using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChoiceManager : MonoBehaviour {

    #region Singleton

    public static ChoiceManager instance;

    private void Awake()
    {
        if (instance != null)
        {

            Debug.LogWarning("More than one instance of ChoiceManager found!");
            return;

        }

        instance = this;
    }

    #endregion


    public TMP_Text[] choiceText;

    public Animator anim;
    readonly int _isOpen = Animator.StringToHash("isOpen");

    private Choice choice;

    public void StartChoice(Choice choice)
    {

        this.choice = choice;

        anim.SetBool(_isOpen, true);

        for(int i = 0; i < choice.sentences.Length; i++)
        {

            choiceText[i].text = choice.sentences[i];

        }

    }

    public void Choice01()
    {

        choice.dialogues[0].TriggerDialogue();
        EndChoice();

    }

    public void Choice02()
    {

        choice.dialogues[1].TriggerDialogue();
        EndChoice();

    }

    void EndChoice()
    {

        anim.SetBool(_isOpen, false);

    }

}
