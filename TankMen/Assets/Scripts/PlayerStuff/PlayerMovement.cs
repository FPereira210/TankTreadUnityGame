using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

	//the movement sound manager needs speed
	[HideInInspector]
	public float speed, turnSpeed,turretTurnSpeed;

	[SerializeField]
	private float speedMultiplier, turnSpeedMultiplier, turretTurnSpeedMultiplier;
	private Rigidbody2D rb;
	private Transform turret;

	[HideInInspector]
	public bool isPlayingSoundTurret;


	private GameManager gameManager;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		turret = transform.GetChild (0);
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();;
	}



	void FixedUpdate(){

		if (gameManager.currentState != GameManager.GameStates.Dead) {
			MovementHandler ();
		} else {
			speed = 0;
		}

		if (turretTurnSpeed == 0) {
			isPlayingSoundTurret = false;
		}


	}


	//Inputs that interact with the sliders
	public void MovementHandler(){
		rb.AddRelativeForce(new Vector2(0f,speed*Time.deltaTime));
		//transform.Rotate (0, 0, (-turnSpeed) * Time.deltaTime);
		rb.AddTorque((-turnSpeed)*Time.deltaTime, ForceMode2D.Impulse);

		turret.transform.Rotate (0, 0, -turretTurnSpeed * Time.deltaTime);
	}

	public void ForwardsBackWards(float sliderInput){
		speed=sliderInput*speedMultiplier;
	}

	public void RotateTank(float rotateInput){
		turnSpeed = rotateInput*turnSpeedMultiplier;
	}

	public void RotateTurret(float rotator){
		turretTurnSpeed = rotator*turretTurnSpeedMultiplier;
		if (!isPlayingSoundTurret) {
			//_audioSource2.Play ();
			isPlayingSoundTurret = true;
		}

	}

	void OnCollisionEnter2D(Collision2D other){


		if (gameManager.currentState != GameManager.GameStates.Dead) {
			//we stop moving if we hit stuff
			if (other.gameObject.tag == "Dummy"||other.gameObject.layer==8 /*barrier*/) {
				GameObject forwardBackwards = GameObject.Find ("ForwardsBackwards");
				forwardBackwards.GetComponent<Slider> ().value = 0;
			}

			if (other.gameObject.tag == "Enemy") {
				//carmageddon
				if (other.gameObject.GetComponent<Enemy> ().enemyType == Enemy.EnemyTypes.Soldier) {
					other.gameObject.GetComponent<Enemy> ().currentState = Enemy.EnemyStates.Dead;
				} else {
					GameObject forwardBackwards = GameObject.Find ("ForwardsBackwards");
					forwardBackwards.GetComponent<Slider> ().value = 0;
				}
			}
		}

	}
}
