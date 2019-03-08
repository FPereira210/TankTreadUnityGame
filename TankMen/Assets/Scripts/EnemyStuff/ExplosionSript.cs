using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSript : MonoBehaviour {

	public GameObject explosion;
	


	public void StartExplosion(){
		GameObject obj = Instantiate(explosion,this.transform.position,Quaternion.identity);
	}
}
