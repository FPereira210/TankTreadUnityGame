using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {


	public void LoadLevel(int level){
		SceneManager.LoadScene (level);
	}

	public void RestartLevel(){
		Scene currentScene = SceneManager.GetActiveScene ();
		string sceneName = currentScene.name;
		SceneManager.LoadScene (sceneName);

	}

	public void  LoadNextLevel(){
		if (SceneManager.GetActiveScene ().buildIndex != 4) {
			int sceneIndex = SceneManager.GetActiveScene ().buildIndex;
			SceneManager.LoadScene (sceneIndex + 1);
		} else {
			SceneManager.LoadScene (0);
		}
	
	}
	public void Exit(){
		Application.Quit ();
	}
}
