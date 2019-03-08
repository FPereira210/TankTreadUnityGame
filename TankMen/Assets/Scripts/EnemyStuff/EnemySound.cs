using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour {



	public AudioSource _audioSource1,_audioSource2;



	void Start(){
		_audioSource1 = GetComponent<AudioSource> ();
	}

	public void CannonSound(){
		_audioSource1.Play ();
	}

	public void Explosion(){
		if (transform.root.gameObject.GetComponent<Enemy> ().enemyType == Enemy.EnemyTypes.Soldier) {
			_audioSource2.Play ();
		}
	
		
	}
}
