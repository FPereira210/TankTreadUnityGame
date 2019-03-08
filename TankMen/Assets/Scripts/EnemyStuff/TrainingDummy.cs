using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummy : MonoBehaviour {


	public Transform[] moveSpots;
	public Sprite damageSprite;

	private int randomSpot;
	public float speed, stoppingDistance;
	public float startWaitTime, startWaitTurn;
	private float waitTime, waitTurn;

	private int health =100;

	[SerializeField]
	private GameObject  hitExplosionAP,hitExplosionHE;

	private GameObject fire;

	bool justDied;
	public enum DummyStates{
		StaticDummy,MovingDummy, DamagedDummy
	}

	public DummyStates currentState;
	void Start () {
		waitTime = startWaitTime;
		waitTurn = startWaitTurn;

		fire = transform.GetChild (1).gameObject;

		//Only moving dummys need waypoints
		if (currentState == DummyStates.MovingDummy) {
			foreach(Transform t in moveSpots){
				if (t == null) {
					Debug.Log ("The dummy: --  " + gameObject.name + "  -- is missing one or more movespots");
				}
			}
		}

	}
	

	void Update () {
		StateHandler ();
	}
	void StateHandler(){
		
		if (currentState == DummyStates.MovingDummy) {
			Patrol ();
		} else if (currentState == DummyStates.DamagedDummy && justDied==false) {
			justDied = true;
			transform.GetChild (0).gameObject.GetComponent<SpriteRenderer> ().sprite = damageSprite;
			fire.SetActive (true);
			GetComponent<ExplosionSript> ().StartExplosion ();
		}
	}

	void Patrol ()
	{
		//The movespot

		Vector3 point = moveSpots [randomSpot].position - transform.position;
	
	

		//If we are far from the movespot....
		if (Vector2.Distance (transform.position, moveSpots [randomSpot].position) > 5f) {

			//.. we look at it through the lerp...
			transform.up = Vector3.Lerp (transform.up, point, 0.0008f);
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

	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.tag == "AP") {
			GetComponent<AudioSource> ().Play ();
			Instantiate (hitExplosionAP, other.gameObject.transform.position,Quaternion.identity);
			currentState = DummyStates.DamagedDummy;
			Debug.Log ("dummy got wrecked");
			health -= 45;
			Destroy (other.gameObject);

		}

		if (other.gameObject.tag == "HE") {
			GetComponent<AudioSource> ().Play ();
			Instantiate (hitExplosionHE, other.gameObject.transform.position,Quaternion.identity);
			currentState = DummyStates.DamagedDummy;
			health -= 55;

			Destroy (other.gameObject);
		}
	}
}
