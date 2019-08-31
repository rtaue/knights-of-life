using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    #region Singleton

    public static MenuManager instance;

    private void Awake()
    {
        if (instance != null)
        {

            Debug.LogWarning("More than one instance of MenuManager found!");
            return;

        }

        instance = this;
    }

    #endregion

    public string automaticScene;
	public float timeToWait;

	void Start ()	{

		if (automaticScene.Length > 0 && timeToWait > 0) {

			Invoke ("LoadScene", timeToWait);

		}

	}

	public void LoadScene() {

		LoadScene (automaticScene);

	}

	public void LoadScene (string sceneToGo)	{

		MainData.nextScene = sceneToGo;

		Invoke ("DelayLoad", 0.5f);

	}

	public void LoadSceneFast (string sceneToGo) {

		MainData.nextScene = sceneToGo;

		SceneManager.LoadScene ("Loading");

	}

	public void DelayLoad ()	{

		SceneManager.LoadScene ("Loading");

	}

	public void Quit ()	{

		Application.Quit();

	}

}
