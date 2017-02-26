using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if (other.tag == "Wall" || other.tag == "TrapDetector") {
			Debug.Log ("Spear hit the wall !");
			Destroy (gameObject);
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
