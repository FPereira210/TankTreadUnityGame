using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReloadScript : MonoBehaviour, IDropHandler, IDragHandler,IBeginDragHandler {

	public bool isAP;
	private GameObject turret;



	private Vector3 startPos;

	void Start () {
		startPos = transform.localPosition;
		turret = GameObject.Find ("TURRET1");
	}

	void Update(){
		if (isAP && turret.GetComponent<Shooting> ().roundsAP <= 0) {
			this.gameObject.SetActive (false);
		}

		if (!isAP && turret.GetComponent<Shooting> ().roundsHE <= 0) {
			this.gameObject.SetActive (false);
		}

	}
	public void OnBeginDrag(PointerEventData eventData){
		
	}
		

	public void OnDrag(PointerEventData eventData){
		
		transform.position = Input.mousePosition;
		if (transform.localPosition.y > 200) {
			transform.localPosition = startPos;
		}

	}

	public void OnDrop(PointerEventData eventData){
		//Debug.Log (transform.localPosition);

		Debug.Log ("ON DROP");

		if (transform.localPosition.y > 0 && transform.localPosition.x < 70) {
			Debug.Log ("ON THE ZONE");
			if (isAP) {
				turret.GetComponent<Shooting> ().ReloadAP ();
			} else {
				turret.GetComponent<Shooting> ().ReloadHE ();
			}

		}

		transform.localPosition = startPos;
	}

}


