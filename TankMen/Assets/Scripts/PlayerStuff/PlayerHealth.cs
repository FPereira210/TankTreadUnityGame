using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	public int health=100;

	[SerializeField]
	private GameManager gameManager;
	[SerializeField]
	private Sprite[] damagedSprite;

	[SerializeField]
	private GameObject hitExplosionHE;

	private GameObject fire,exhausts;
	private bool explodeOnce;

	void Start(){
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		fire = transform.GetChild (2).gameObject;
		exhausts = transform.GetChild (3).gameObject;
	}

	void Update(){
		if (health <= 80&& health >50) {
			transform.GetChild (1).gameObject.GetComponent<SpriteRenderer> ().sprite = damagedSprite[0];
		} else if (health <= 50) {
			transform.GetChild (1).gameObject.GetComponent<SpriteRenderer> ().sprite = damagedSprite[1];
		}

		if (health <= 0) {
			Debug.Log ("o no i died");

			gameManager.currentState = GameManager.GameStates.Dead;
			fire.SetActive (true);
			exhausts.SetActive (false);

			if (!explodeOnce) {
				GetComponent<ExplosionSript> ().StartExplosion ();
				explodeOnce = true;
			}


		}
	}
	void OnCollisionEnter2D(Collision2D other){
		if (other.collider.gameObject.tag == "EnemyProjectile") {
			Instantiate (hitExplosionHE, other.gameObject.transform.position,Quaternion.identity);
			health -= 15;
			Debug.Log ("ouch");
			Destroy (other.gameObject);
			GetComponent<PlayerMovementSound> ().Explosion ();
		}
	}
}
