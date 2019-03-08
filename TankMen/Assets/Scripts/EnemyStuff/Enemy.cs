using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

	public Transform[] moveSpots;
	public Sprite[] damageSprites;

	private int randomSpot;

	public float speed, stoppingDistance;
	public float startWaitTime, startWaitTurn;
	private float waitTime, waitTurn;

	public float startTimeBetweenShots;
	private float timeBetweenShots;

	private int health =100;


	private GameObject player;
	[SerializeField]
	private GameObject projectile, hitExplosionAP,hitExplosionHE;
	private GameObject muzzle;
	private GameObject fire;
	private GameObject muzzleFlash;

	public bool canShoot;

	private bool justDied;

	private EnemySound _sounds;

	public enum EnemyTypes{
		Armored, Soldier
	}
	public enum EnemyStates
	{
		Patrol,
		Attack,
		Damaged,
		Dead
	}

	public EnemyTypes enemyType;

	public EnemyStates currentState;

	void Start ()
	{
		player = GameObject.Find ("PlayerTank");
		waitTime = startWaitTime;
		waitTurn = startWaitTurn;
		currentState = EnemyStates.Patrol;
		muzzle = transform.GetChild (1).gameObject;
		muzzleFlash = transform.GetChild (3).gameObject;

		_sounds = GetComponent<EnemySound> ();

		//muzzleLight = transform.GetChild (2).gameObject;
		timeBetweenShots = startTimeBetweenShots;

		foreach(Transform t in moveSpots){
			if (t == null) {
				Debug.Log ("The enemy: --" + gameObject.name + "-- is missing one or more movespots");
				currentState = EnemyStates.Dead;
			}
		}
		if (enemyType == EnemyTypes.Armored) {
			fire = transform.GetChild (4).gameObject;
		}
	}


	void Update ()
	{
		
		StateHandler ();

		//check on updtade if we have life
		if (enemyType == EnemyTypes.Armored) {
			if (health <= 0) {
				currentState = EnemyStates.Dead;
			}
		} else if (enemyType == EnemyTypes.Soldier) {
			if (transform.GetChild (0).gameObject.GetComponent<SpriteRenderer> ().sprite == damageSprites [0]) {
				currentState = EnemyStates.Dead;
			}
		}

	}

	void StateHandler ()
	{
		if (enemyType == EnemyTypes.Armored) {
			
			if (currentState == EnemyStates.Patrol) {
				Patrol ();
			} else if (currentState == EnemyStates.Attack) {
				Attack ();
			} else if (currentState == EnemyStates.Damaged) {
				DamagedAttack ();
				//Just died is here so it doesn't explode forever
			} else if (currentState == EnemyStates.Dead&&justDied==false) {
				justDied = true;
				Dead ();
			}
		}


		if (enemyType == EnemyTypes.Soldier) {
			if (currentState == EnemyStates.Patrol) {
				Patrol ();
			}else if (currentState == EnemyStates.Attack) {
				Attack ();
			}else if (currentState == EnemyStates.Dead&&justDied==false) {
				justDied = true;
				Dead ();
			}
		}
	}
		
	void Patrol ()
	{
		//The movespot
		Vector3 point = moveSpots [randomSpot].position - transform.position;

		//If we are far from the movespot....
		if (Vector2.Distance (transform.position, moveSpots [randomSpot].position) > 5f) {

			//.. we look at it through the lerp...(0,0008f) (0.1f*time.delta)

			if (enemyType == EnemyTypes.Soldier) {
				transform.up = Vector3.Lerp (transform.up, point, 1*Time.deltaTime);
			} else {
				transform.up = Vector3.Lerp (transform.up, point, 0.1f*Time.deltaTime);
			}


	

			//...wait until we have are looking at it
			waitTurn -= Time.deltaTime;

			//...then we go for it
			if (waitTurn <= 0) {
				transform.Translate (point.normalized * Time.deltaTime * speed, Space.World);
			}

			//If we are near it, we count it down how long we should stay in it, then reset the counters.
		} else {
			if (waitTime <= 0) {
				randomSpot = Random.Range (0, moveSpots.Length);
				waitTime = startWaitTime;
				waitTurn = startWaitTurn;
			} else {
				waitTime -= Time.deltaTime;

			}
		}
		 
	}

	void Attack ()
	{
		if (currentState != EnemyStates.Dead) {
			//we reset the patrol time so it doesn't spring out sideways
			waitTurn = startWaitTurn;
			Debug.Log ("ataatc");
			Vector3 distToPlayer = player.transform.position - transform.position;
			transform.up = Vector3.Lerp (transform.up, distToPlayer, 0.1f*Time.deltaTime);
			//transform.up = player.transform.position - transform.position;
			if (Vector2.Distance (transform.position, player.transform.position) > stoppingDistance) {
				transform.Translate (distToPlayer.normalized * Time.deltaTime * speed, Space.World);
			}
			if (timeBetweenShots <= 0) {
				Shoot ();
				timeBetweenShots = startTimeBetweenShots;
			} else {
				timeBetweenShots -= Time.deltaTime;
			}
		}

	}
		
	//only armored enemies can still atack after being hit, humans simply die. The can shoot only means they shoot when you are in sight
	void DamagedAttack(){
		Debug.Log ("damaged ataatc");
		Vector3 distToPlayer = player.transform.position - transform.position;

		transform.GetChild (0).gameObject.GetComponent<SpriteRenderer> ().sprite = damageSprites [1];

		transform.up = Vector3.Lerp (transform.up, distToPlayer, 0.01f*Time.deltaTime);
		//transform.up = player.transform.position - transform.position;

		if(canShoot){
			if (timeBetweenShots <= 0) {
				Shoot ();
				timeBetweenShots = startTimeBetweenShots;
			} else {
				timeBetweenShots -= Time.deltaTime;
			}
		}
	}

	//both foot patrol and armored bois have the death sprite located at 0,however only the armored bois can caught on fire
	void Dead(){

		GameManager gameMan = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		gameMan.enemyCount--;
		transform.GetChild (0).gameObject.GetComponent<SpriteRenderer> ().sprite = damageSprites [0];
		if (enemyType == EnemyTypes.Armored) {
			GetComponent<ExplosionSript> ().StartExplosion ();

			fire.SetActive (true);
		} else if (enemyType == EnemyTypes.Soldier) {
			GetComponent<BoxCollider2D> ().enabled = false;
		}

	}

	void OnCollisionEnter2D (Collision2D other)
	{
		if (enemyType == EnemyTypes.Armored) {

		

			if (other.gameObject.tag == "AP") {
				GetComponent<EnemySound> ().Explosion ();
				Instantiate (hitExplosionAP, other.gameObject.transform.position,Quaternion.identity);
				//sound handling goes here
				if (currentState != EnemyStates.Dead) {
					currentState = EnemyStates.Damaged;
				}
	
				if (Vector2.Distance (transform.position, player.transform.position) < 40) {
					Debug.Log ("55 dealt damage");
					health -= 55;
				} else {
					Debug.Log ("45 dealt damage");
					health -= 45;
				}
				Destroy (other.gameObject);

			}

			if (other.gameObject.tag == "HE") {
				GetComponent<EnemySound> ().Explosion ();
				Instantiate (hitExplosionHE, other.gameObject.transform.position,Quaternion.identity);
				if (currentState != EnemyStates.Dead) {
					currentState = EnemyStates.Damaged;
				}
				if (Vector2.Distance (transform.position, player.transform.position) < 20) {
					Debug.Log ("105 dealt damage");
					health -= 105;
				} else {
					Debug.Log ("95 dealt damage");
					health -= 95;
				}
				Destroy (other.gameObject);
			}
		}

		if (enemyType == EnemyTypes.Soldier) {

			if (other.gameObject.tag == "AP" || other.gameObject.tag == "HE") {
				currentState = EnemyStates.Dead;
			}
		}
	}


	void Shoot(){
		StartCoroutine ("MuzzleFlash");
		_sounds.CannonSound ();
		GameObject round = Instantiate (projectile, muzzle.transform.position, muzzle.transform.rotation)as GameObject;
		round.GetComponent<Rigidbody2D> ().AddRelativeForce (new Vector2 (0,20f), ForceMode2D.Impulse);
	}

	IEnumerator MuzzleFlash(){
		muzzleFlash.SetActive (true);
		yield return new WaitForSeconds (0.1f);
		muzzleFlash.SetActive (false);
	}

}

