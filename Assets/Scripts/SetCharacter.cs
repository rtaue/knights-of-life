using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetCharacter : MonoBehaviour {

    public GameObject playerMale, playerFemale;
    public Transform initialPosition;

    public Text playerName;

	// Use this for initialization
	void Start () {

        if (MainData.isPlayerMale)
        {
            playerMale.SetActive(true);
            playerFemale.SetActive(false);
        }
            
        else
        {

            playerMale.SetActive(false);
            playerFemale.SetActive(true);

        }

        playerName.text = MainData.playerName;

    }
	
}
