using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteMain : MonoBehaviour {

	bool musicOn;
	void Start () {
		musicOn = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void MusicHandling(){
		musicOn = !musicOn;

		if (!musicOn) {
			GetComponent<AudioSource> ().volume = 0;
		} else {
			GetComponent<AudioSource> ().volume = 1f;
		}

	}
}
