using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour {

	public Slider slider;
	public Text progressText;

	// Use this for initialization
	void Start () {
		
		StartCoroutine (LoadAsynchronously (MainData.nextScene));

	}
	
	IEnumerator LoadAsynchronously (string nextScene) {

		yield return null;

		AsyncOperation ao = SceneManager.LoadSceneAsync (nextScene);

		ao.allowSceneActivation = false;

		while (!ao.isDone)	{

			float progress = Mathf.Clamp01 (ao.progress / 0.9f);
			
			slider.value = progress;

			progressText.text = progress * 100 + "%";

			if (ao.progress == 0.9f)	{

				ao.allowSceneActivation = true;

			}

			yield return null;

		}

	}
}
