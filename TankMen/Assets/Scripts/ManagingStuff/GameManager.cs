using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public GameObject pauseMenu, deadMenu, controlPanel, shootingPanel, pauseButton, helpMenu, helpButton,soundButton, winMenu,musicButton,musicPlayer;

	public GameObject[] enemies;
	public int enemyCount;

	private bool soundOn,musicOn;

	public enum GameStates
	{
		Normal,Paused,Dead,HelpPaused,Win
	}

	public GameStates currentState;

	void Start () {
		Time.timeScale = 1f;
		currentState = GameStates.Normal;
		pauseMenu = GameObject.Find ("PauseMenu").gameObject;
		controlPanel = GameObject.Find ("ControlPanel").gameObject;
		shootingPanel = GameObject.Find ("ShootingPanel").gameObject;
		deadMenu = GameObject.Find ("DeadMenu").gameObject;
		winMenu = GameObject.Find ("WinMenu").gameObject;
		pauseButton = GameObject.Find ("Pause").gameObject;
		helpMenu = GameObject.Find ("Help").gameObject;
		helpButton = GameObject.Find ("HelpButton");
		soundButton = GameObject.Find ("SoundButton");
		musicButton = GameObject.Find ("MusicButton");
		musicPlayer = GameObject.FindGameObjectWithTag ("MusicPlayer");
		soundOn = true;
		musicOn = true;
		enemies = GameObject.FindGameObjectsWithTag ("Enemy");

		enemyCount = enemies.Length;
	}
	

	void Update () {
		StateHandler ();
		if (enemyCount <= 0&&SceneManager.GetActiveScene().name!= "TrainingGrounds"){
			currentState = GameStates.Win;
		}
	}

	//this controls what can be interacted
	void StateHandler ()
	{
		if (currentState == GameStates.Normal) {
			pauseMenu.SetActive (false);
			deadMenu.SetActive (false);
			controlPanel.SetActive (true);
			shootingPanel.SetActive (true);
			helpMenu.SetActive (false);
			winMenu.SetActive (false);
			Time.timeScale = 1;
		} else if (currentState == GameStates.Paused) {
			pauseMenu.SetActive (true);
			deadMenu.SetActive (false);
			controlPanel.SetActive (false);
			shootingPanel.SetActive (false);
			helpMenu.SetActive (false);
			winMenu.SetActive (false);
			Time.timeScale = 0;
		} else if (currentState == GameStates.Dead) {
			deadMenu.SetActive (true);
			pauseMenu.SetActive (false);
			controlPanel.SetActive (false);
			shootingPanel.SetActive (false);
			pauseButton.SetActive (false);
			helpMenu.SetActive (false);
			helpButton.SetActive (false);
			winMenu.SetActive (false);
		} else if (currentState == GameStates.HelpPaused) {
			deadMenu.SetActive (false);
			pauseMenu.SetActive (false);
			shootingPanel.SetActive (false);
			controlPanel.SetActive (false);
			helpMenu.SetActive (true);
			winMenu.SetActive (false);
			Time.timeScale = 0;
		} else if (currentState == GameStates.Win) {
			deadMenu.SetActive (false);
			pauseMenu.SetActive (false);
			controlPanel.SetActive (false);
			shootingPanel.SetActive (false);
			pauseButton.SetActive (false);
			helpMenu.SetActive (false);
			helpButton.SetActive (false);
			winMenu.SetActive (true);
			Time.timeScale = 0;
		}
	}
		
	public void PauseGame(){

		if (currentState != GameStates.Dead&&currentState!=GameStates.HelpPaused) {
			if (currentState == GameStates.Normal) {
				currentState = GameStates.Paused;
			} else if (currentState == GameStates.Paused) {
				currentState = GameStates.Normal;
			}
		}
	}

	public void PauseHelp(){

		if (currentState != GameStates.Dead&&currentState!=GameStates.Paused) {
			if (currentState == GameStates.Normal) {
				currentState = GameStates.HelpPaused;
			} else if (currentState == GameStates.HelpPaused) {
				currentState = GameStates.Normal;
			}
		}
	}

	public void SoundHandling(){
		soundOn = !soundOn;

		if (!soundOn) {
			AudioListener.volume = 0;
		} else {
			AudioListener.volume = 1;
		}
	}

	public void MusicHandling(){
		musicOn = !musicOn;

		if (!musicOn) {
			musicPlayer.GetComponent<AudioSource> ().volume = 0;
		} else {
			musicPlayer.GetComponent<AudioSource> ().volume = 1f;
		}

	}
}
