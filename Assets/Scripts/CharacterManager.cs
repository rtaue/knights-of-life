using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour {

    public InputField playerName;

    public void PlayerName()
    {

        MainData.playerName = playerName.text;

        Debug.Log("Player's name: " + MainData.playerName);

    }

    public void SelectGender(bool isMale)
    {

        MainData.isPlayerMale = isMale;
        Debug.Log("Is Player Male: " + MainData.isPlayerMale.ToString());

    }

}
