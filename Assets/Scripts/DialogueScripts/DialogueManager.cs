using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DialogueManager : MonoBehaviour {

    #region Singleton

    public static DialogueManager instance;

    private void Awake()
    {
        if (instance != null)
        {

            Debug.LogWarning("More than one instance of DialogueManager found!");
            return;

        }

        instance = this;
    }

    #endregion

    public TMP_Text nameText;
    public TMP_Text dialogueText;

    public Animator anim;
    readonly int _isOpen = Animator.StringToHash("isOpen");

    private Queue<string> sentences;

    public DialogueTrigger nextDialogue;
    public ChoiceTrigger choice;
    public string sceneToLoad;

	// Use this for initialization
	void Start () {

        sentences = new Queue<string>();

	}

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
            DisplayNextSentence();

    }

    public void StartDialogue(Dialogue dialogue)
    {

        anim.SetBool(_isOpen, true);

        nameText.text = dialogue.name;

        
        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {

            sentences.Enqueue(sentence);

        }

        DisplayNextSentence();
        
    }

    public void DisplayNextSentence()
    {

        if (sentences.Count == 0)
        {

            EndDialogue();
            return;

        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

    }

    IEnumerator TypeSentence(string sentence)
    {

        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {

            dialogueText.text += letter;
            yield return null;

        }

    }

    void EndDialogue()
    {

        if (nextDialogue)
            nextDialogue.TriggerDialogue();
        else if (choice)
        {

            anim.SetBool(_isOpen, false);
            choice.TriggerChoice();

        }   
        else if (sceneToLoad.Length > 0)
        {

            anim.SetBool(_isOpen, false);
            MenuManager _menuManager = MenuManager.instance;
            _menuManager.LoadScene(sceneToLoad);

        }
        else
            anim.SetBool(_isOpen, false);

    }

}
