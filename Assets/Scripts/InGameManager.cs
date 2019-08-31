using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour {

    #region Singleton

    public static InGameManager instance;

    private void Awake()
    {

        if (instance != null)
        {

            Debug.Log("More than one instance of InGameManager found!");
            return;

        }

        instance = this;

    }

    #endregion

    public GameObject winPanel, losePanel;
    public Text timerText;

    public float levelTime;

    float timer;

	// Use this for initialization
	void Start () {

        timer = levelTime;

	}
	
	// Update is called once per frame
	void Update () {

        timer -= Time.deltaTime;

        timerText.text = timer.ToString("00");

        if (timer <= 0f)
        {

            Win();

        }

	}

    public void Win()
    {

        Time.timeScale = 0.1f;
        winPanel.SetActive(true);

    }

    public void Lose()
    {

        Time.timeScale = 0.1f;
        losePanel.SetActive(true);

    }

    public void Restart()
    {

        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void Menu()
    {

        Time.timeScale = 1f;

        SceneManager.LoadScene("Menu");

    }

    public void LoadSceneFast(string sceneToGo)
    {

        Time.timeScale = 1f;

        MainData.nextScene = sceneToGo;

        SceneManager.LoadScene("Loading");

    }

}
