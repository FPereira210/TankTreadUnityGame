using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementSound : MonoBehaviour {

	[SerializeField]
	private AudioSource _audioSource1,_audioSource2,_audioSource3,_audioSource4;

	void Start(){
		_audioSource2.volume = 0;
	}

	void Update () {
		SoundHandling ();
	}

	void SoundHandling(){

		float desiredPitch =GetComponent<PlayerMovement> ().speed / 80 +0.5f;
		//_audioSource1.pitch = desiredPitch;

		_audioSource1.pitch = Mathf.Lerp (_audioSource1.pitch, desiredPitch, 0.5f * Time.deltaTime);

		if (_audioSource1.pitch >= 2.5f) {
			_audioSource1.pitch = 2.5f;
		}
		//if we are reversing we wante the sound to behave differently
		if ( GetComponent<PlayerMovement> ().speed < 0) {
			_audioSource1.pitch = (- GetComponent<PlayerMovement> ().speed/70)+0.1f;
		}


		if (GetComponent<PlayerMovement> ().isPlayingSoundTurret) {
			_audioSource2.volume = 1;
			if (GetComponent<PlayerMovement> ().turretTurnSpeed > 10 || GetComponent<PlayerMovement> ().turretTurnSpeed < -10) {
				_audioSource2.pitch = 1.03f;
			} else {
				_audioSource2.pitch = 1f;
			}

		} else if (!GetComponent<PlayerMovement> ().isPlayingSoundTurret) {
			_audioSource2.volume = 0;
		}

		float desiredVolume= (GetComponent<PlayerMovement> ().speed/80)-0.6f;
		//_audioSource3.volume = desiredVolume;
		_audioSource3.volume = Mathf.Lerp (_audioSource3.volume, desiredVolume, 0.5f * Time.deltaTime);

		if (_audioSource3.volume >= 0.8f) {
			_audioSource3.volume = 0.8f;
		}

	}

	public void Explosion(){
		_audioSource4.Play ();
	}
}
