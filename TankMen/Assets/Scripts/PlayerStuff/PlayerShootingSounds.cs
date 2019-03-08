using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingSounds : MonoBehaviour {

	//shooting stuff
	[SerializeField] 
	private AudioClip cannonFire, machineGunFire,reload;

	private AudioSource _audioSource;


	void Start () {
		_audioSource = GetComponent<AudioSource> ();

	}

	public void MachineGunSound(){
		AudioSource.PlayClipAtPoint (machineGunFire, Camera.main.transform.position + new Vector3 (0, 0, 3f), 0.5f);

	}
	public void CannonSound(){
		_audioSource.Play ();

	}

	public void ReloadSound(){
		AudioSource.PlayClipAtPoint (reload, Camera.main.transform.position, 0.5f);

	}

}
