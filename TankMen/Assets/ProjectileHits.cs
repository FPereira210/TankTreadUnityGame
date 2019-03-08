using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHits : MonoBehaviour {




	void OnCollisionEnter2D(Collision2D other){


		if (other.gameObject.tag == "Enemy"||other.gameObject.tag=="Dummy") {
			Destroy (gameObject);
			}

	}


}
