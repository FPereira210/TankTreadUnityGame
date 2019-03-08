using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

	public bool isFollowingTank;
	private bool moveBack;

	private float offset;
	private float fov=60;


	public float speed = 18;
	[SerializeField]
	private GameObject player,camFov;

	[SerializeField]
	private GameObject buttonCamera;


	void Start () {
		isFollowingTank = true;
		player = GameObject.Find ("PlayerTank");
		camFov = GameObject.Find ("CamFOV");
		offset = player.transform.position.z + transform.position.z;
		buttonCamera = GameObject.Find ("CameraButton");
	}
	

	void Update () {
		
		CamMovement ();

		gameObject.GetComponent<Camera> ().fieldOfView = fov;
		if (moveBack) {
			MoveBackToPlayer ();
		}

		if (Input.GetAxis("Mouse ScrollWheel") > 0f ) // zoom out with mouse wheel
		{
			camFov.GetComponent<Slider> ().value -= 3;
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0f ) // zoom in with mouse wheel
		{
			camFov.GetComponent<Slider> ().value += 3;
		}

		if (!isFollowingTank) {
			//button colors
			ColorBlock cb = buttonCamera.GetComponent<Button> ().colors;
			cb.normalColor = Color.green;
			buttonCamera.GetComponent<Button> ().colors = cb;
		} else {
			//button colors
			ColorBlock cb = buttonCamera.GetComponent<Button> ().colors;
			cb.normalColor = Color.white;
			buttonCamera.GetComponent<Button> ().colors = cb;
		}
	}

	// When we are not following the tank, there are limits where it starts following it again
	void CamMovement(){
		float moveHorizontal =- Input.GetAxis ("Horizontal") * Time.deltaTime*speed;
		float moveVertical = Input.GetAxis ("Vertical") * Time.deltaTime*speed;
		if (!isFollowingTank) {
			transform.Translate (Vector3.left * moveHorizontal);
			transform.Translate (Vector3.up * moveVertical);

		}else if (isFollowingTank &&moveBack==false) {
			transform.position =new Vector3 (player.transform.position.x, player.transform.position.y, offset);
		}
			
		//The clamps if !isfollowingPlayer
		if (transform.position.y < player.transform.position.y-15) {
			transform.position = new Vector3(transform.position.x,player.transform.position.y - 15,transform.position.z);
		}
		if (transform.position.y > player.transform.position.y + 30) {
			transform.position = new Vector3(transform.position.x,player.transform.position.y +30,transform.position.z);
		}


		if (transform.position.x < player.transform.position.x-30) {
			transform.position = new Vector3(player.transform.position.x-30,transform.position.y,transform.position.z);
		}
		if (transform.position.x > player.transform.position.x + 30) {
			transform.position = new Vector3(player.transform.position.x+30,transform.position.y,transform.position.z);
		}
	}

	public void FOVController(float SliderInput){
		fov = SliderInput;
	}

	public void SetBoolFollow(){
		isFollowingTank = !isFollowingTank;

		if (isFollowingTank) {
			moveBack = true;

		}
	}

	//This makes the camera move back to the player
	void MoveBackToPlayer (){
		Debug.Log ("moving bakc");

		transform.position = Vector3.Lerp (transform.position,new Vector3 (player.transform.position.x, player.transform.position.y, offset),0.4f);

		if(Vector2.Distance(transform.position,new Vector3 (player.transform.position.x, player.transform.position.y, offset))<1f){
			Debug.Log ("we reached");
			moveBack = false;
		}
	}


}
