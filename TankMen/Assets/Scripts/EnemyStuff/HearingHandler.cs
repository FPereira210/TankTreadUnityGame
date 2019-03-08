using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearingHandler : MonoBehaviour {

	private GameObject playerTurret;
	private Enemy enemy;
	void Start () {
		playerTurret = GameObject.Find ("TURRET1");
		enemy = transform.root.gameObject.GetComponent<Enemy> ();
	}
	

	void Update () {
		
	}

	//FIX IT BOO
	void OnTriggerStay2D(Collider2D other){

		if (enemy.currentState != Enemy.EnemyStates.Dead) {
			if (other.gameObject == playerTurret.transform.root.gameObject) {
				Debug.Log ("inside hearing zone");
				if (enemy.currentState==Enemy.EnemyStates.Patrol) {
					if (playerTurret.GetComponent<Shooting>().justShoot) {
						Debug.Log ("I heard you motherfucker");
						enemy.currentState = Enemy.EnemyStates.Attack;
					}
				}
			}
		}


	}
}

