using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Shooting : MonoBehaviour {

	[SerializeField]
	private GameObject projectileAP, projectileHE, tracer,mgMuzzleLight,tracerHit;
	private GameObject muzzle, machineMuzzle, mgMuzzleFlash, muzzleFlash, muzzleLight;

	private PlayerShootingSounds _sounds;

	public int roundsAP, roundsHE;

	[SerializeField]
	private bool isReloading, isLoaded, isAP, isMachineGunning;

	//MachineGun regulators
	public float startTimeBetweenShots;
	private float timeBetweenShots;
	private int counterTracer;

	public float startMgHeat =3f;
	private float mgHeat;
	[SerializeField]
	private bool isCoolingDown;


	private Animator _anim;

	[SerializeField]
	private Image empty, apImage, heImage;
	[SerializeField]
	private Text apText, heText;

	//used by the enemie's hearing to know where the player is
	public bool justShoot;


	void Start () {
		
		isLoaded = true;
		isAP = true;


		_anim = GetComponent<Animator> ();
		_sounds = GetComponent<PlayerShootingSounds> ();

		muzzle = transform.GetChild (1).gameObject;
		machineMuzzle = transform.GetChild (3).gameObject;
		mgMuzzleFlash = transform.GetChild (2).gameObject;
		muzzleFlash = transform.GetChild (0).gameObject;
		muzzleLight = transform.GetChild (4).gameObject;
		//mgMuzzleFlash = transform.GetChild (5).gameObject;
		mgHeat=startMgHeat;
	}
	

	void Update () {
		ImageHandler ();
		try{
		apText.text = "" + roundsAP;
		heText.text = "" + roundsHE;
		}catch{
			Debug.Log ("MISSING TEXTS");
		}

		//test

	}

	void FixedUpdate(){
		MachineGun ();
	}

	void ImageHandler(){
		if (isLoaded) {
			if (isAP) {
				apImage.enabled = true;
				heImage.enabled = false;
			} else if (!isAP) {
				apImage.enabled = false;
				heImage.enabled = true;
			}
		} else {
			apImage.enabled = false;
			heImage.enabled = false;
		}

		if (transform.root.GetComponent<PlayerHealth> ().health < 80) {
			Debug.Log ("changing sprite");
			_anim.SetBool ("IsDamaged", true);


		}
	}

	//the button calls this function, decides throug the isAP bool which shooting function will be called
	public void ShootingHandler ()
	{
		if (isAP) {
			
			ShootAP ();
		} 

		if (!isAP) {
			ShootHE ();
		} 
	}

	void ShootAP(){
		if (!isReloading && isLoaded) {
			_sounds.CannonSound ();
			isLoaded = false;
			StartCoroutine ("MuzzleFlash");
			justShoot = true;

			GameObject round = Instantiate (projectileAP, muzzle.transform.position, muzzle.transform.rotation)as GameObject;
			round.GetComponent<Rigidbody2D> ().AddRelativeForce (new Vector2 (0f, 75f), ForceMode2D.Impulse);
			_anim.SetTrigger ("ShootTrigger");


			//recoil
			Vector2 dir = transform.position - muzzle.transform.position;
			if (Mathf.Abs (transform.rotation.z) > 0.6f && Mathf.Abs (transform.rotation.z) < 0.8f) {
				transform.root.GetComponent<Rigidbody2D> ().AddForce (dir *400* Time.deltaTime);

			} else {
				transform.root.GetComponent<Rigidbody2D> ().AddForce (dir * 300 * Time.deltaTime);
			}
			StartCoroutine ("StopJustShot");
		}
	}

	void ShootHE(){
		if (!isReloading && isLoaded) {
			_sounds.CannonSound ();
			isLoaded = false;
			StartCoroutine ("MuzzleFlash");
			justShoot = true;

			GameObject round = Instantiate (projectileHE, muzzle.transform.position, muzzle.transform.rotation)as GameObject;
			round.GetComponent<Rigidbody2D> ().AddRelativeForce (new Vector2 (0f, 50f), ForceMode2D.Impulse);
			_anim.SetTrigger ("ShootTrigger");


			//recoil
			Vector2 dir = transform.position - muzzle.transform.position;
			if (Mathf.Abs (transform.rotation.z) > 0.6f && Mathf.Abs (transform.rotation.z) < 0.8f) {
				transform.root.GetComponent<Rigidbody2D> ().AddForce (dir * 600*Time.deltaTime);

			} else {
				transform.root.GetComponent<Rigidbody2D> ().AddForce (dir * 500*Time.deltaTime);
			}
			StartCoroutine ("StopJustShot");
		}
	}

	//The rounds with the reloadScript call these functions. Wheter they are ap or he
	public void ReloadAP ()
	{
		if (!isLoaded && roundsAP > 0 &&!isReloading) {
			roundsAP--;
			isAP = true;
			StartCoroutine ("ReloadCoroutine");
		} else {
			Debug.Log ("Unable to load");
		}
	}

	public void ReloadHE ()
	{
		if (!isLoaded && roundsHE > 0 &&!isReloading) {
			roundsHE--;
			isAP = false;
			StartCoroutine ("ReloadCoroutine");
		} else {
			Debug.Log ("Unable to load");
		}
	}


	public void MachineGun(){
		if (isMachineGunning) {
			mgHeat -= Time.deltaTime;

			if (mgHeat > 0 ) {
				if (timeBetweenShots <= 0) {

					_sounds.MachineGunSound ();

					StartCoroutine ("MGMuzzleFlash");

					RaycastHit2D hit = Physics2D.Raycast (muzzle.transform.position - new Vector3 (+0.3f, 0), muzzle.transform.up, 1000);

					if (hit) {
						Debug.DrawRay (muzzle.transform.position - new Vector3 (+0.3f, 0), muzzle.transform.up * 100, Color.red, 0.6f);
						if (hit.collider.gameObject.tag == "Enemy") {
							if (hit.collider.gameObject.GetComponent<Enemy> ().enemyType == Enemy.EnemyTypes.Soldier) {
								hit.collider.gameObject.GetComponent<Enemy> ().currentState = Enemy.EnemyStates.Dead;
							} else {
							
								//Debug.Log ("sparkles for tank goes here");

								Vector2 hitPoint = hit.point;
								Instantiate (tracerHit, hitPoint, transform.rotation);
							}
						} else if (hit.collider.gameObject.tag == "Dummy") {
							Vector2 hitPoint = hit.point;
							Instantiate (tracerHit, hitPoint, transform.rotation);
						}
					}
					//one every three rounds is a tracer
					counterTracer++;
					if (counterTracer == 3) {
						GameObject tracerObj = Instantiate (tracer, machineMuzzle.transform.position, machineMuzzle.transform.rotation)as GameObject;
						tracerObj.GetComponent<Rigidbody2D> ().AddRelativeForce (new Vector2 (0, 150f), ForceMode2D.Impulse);
						counterTracer = 0;
					}

					timeBetweenShots = startTimeBetweenShots;

				} else {
					timeBetweenShots -= Time.deltaTime;
				}

			} else {
				MachineGunStop ();
				StartCoroutine ("CoolDown");
			}
		}
	}

	public void MachineGunFire(){
		if (!isCoolingDown) {
			isMachineGunning = true;
			justShoot = true;
		}

	}
	public void MachineGunStop(){
		mgHeat = startMgHeat;
		isMachineGunning = false;
		StartCoroutine ("StopJustShot");
	}

	//Cannon muzzle flash
	IEnumerator MuzzleFlash ()
	{
		muzzleFlash.SetActive (true);
		muzzleLight.SetActive (true);
		yield return new WaitForSeconds (0.07f);
		muzzleFlash.SetActive (false);
		muzzleLight.SetActive (false);
	}
	IEnumerator MGMuzzleFlash (){
		mgMuzzleFlash.SetActive (true);
		mgMuzzleLight.SetActive (true);
		yield return new WaitForSeconds (0.15f);
		mgMuzzleFlash.SetActive (false);
		mgMuzzleLight.SetActive (false);

	}

	IEnumerator CoolDown(){
		isCoolingDown = true;
		yield return new WaitForSeconds (startMgHeat+1);
		isCoolingDown = false;
	}

	IEnumerator ReloadCoroutine ()
	{
		Debug.Log ("reloading");
		isReloading = true;
		empty.enabled = false;
		_sounds.ReloadSound ();
		yield return new WaitForSeconds (2f);
		isReloading = false;
		isLoaded = true;
		empty.enabled = true;
	}

	IEnumerator StopJustShot(){
		yield return new WaitForSeconds (0.7f);
		justShoot = false;
	}

}


/*
				RaycastHit2D hit = Physics2D.Raycast (muzzle.transform.position-new Vector3(+0.3f,0), muzzle.transform.up, 1000);
				Debug.DrawRay (muzzle.transform.position-new Vector3(+0.3f,0), muzzle.transform.up*100,Color.red, 0.6f);
				Debug.Log (hit.collider.gameObject);
				if (hit.collider.gameObject.tag == "Enemy") {
					if (hit.collider.gameObject.GetComponent<Enemy> ().enemyType == Enemy.EnemyTypes.Soldier) {
						hit.collider.gameObject.GetComponent<Enemy> ().currentState = Enemy.EnemyStates.Dead;
					} else {
						Debug.Log ("sparkles for tank goes here");
					}
				}

*/
