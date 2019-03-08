using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{

	public float waitToPatrolAgain;
	private float counter;
	private Enemy enemy;
	private bool isCountingDown;

	void Start ()
	{
		counter = waitToPatrolAgain;
		enemy = transform.root.GetComponent<Enemy> ();
	}

	//this allows the enemy to follow the target a litte after getting out of the collider
	void Update(){
		if (isCountingDown) {
			if (counter <= 0) {
				counter = waitToPatrolAgain;
				enemy.currentState = Enemy.EnemyStates.Patrol;
				isCountingDown = false;
			} else {
				counter -= Time.deltaTime;
			}
		}
	}

	void OnTriggerStay2D (Collider2D other)
	{
		//if the enemy is not dead, well, he's not dead then, he can do stuff
		if (enemy.currentState != Enemy.EnemyStates.Dead) {
			//if he's not damaged we change the state from patrol to attack, reset the counter if he exits the collider
			if (other.gameObject.tag == "Player" && enemy.currentState != Enemy.EnemyStates.Damaged) {
				counter = waitToPatrolAgain;
		
				Vector2 distToPlayer = other.transform.position - transform.root.GetChild (1).position;
				RaycastHit2D hit = Physics2D.Raycast (transform.root.GetChild (1).position, distToPlayer, 100);
				if (hit.collider.gameObject.tag == "Player") {
					Debug.DrawRay (transform.root.GetChild (1).position, distToPlayer * 10, Color.red, 10f);
					enemy.currentState = Enemy.EnemyStates.Attack;
				} else {
					Debug.DrawRay (transform.root.GetChild (1).position, distToPlayer * 10, Color.blue, 10f);
				}
			}

			//if he's damaged we can only shoot at him as long as he is inside the collider
			if (other.gameObject.tag == "Player" && enemy.currentState == Enemy.EnemyStates.Damaged) {
				Debug.Log ("I am damaged and I can shoot you!");
				enemy.canShoot = true;
			}
		}
	}

	void OnTriggerExit2D (Collider2D other)
	{

		if (enemy.currentState != Enemy.EnemyStates.Dead) {
			//if the player leaves the zone, we change the bool
			if (other.gameObject.tag == "Player" && enemy.currentState!=Enemy.EnemyStates.Damaged) {
				isCountingDown = true;
			}

			if (other.gameObject.tag == "Player" && enemy.currentState == Enemy.EnemyStates.Damaged) {
				Debug.Log ("I am damaged and I can NOT shoot you!");
				enemy.canShoot = false;
			}
		}

	}

}

///In case u need to know the object:
//Debug.Log (hit.collider.gameObject.name);