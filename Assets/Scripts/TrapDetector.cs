using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDetector : MonoBehaviour {
	public bool isTrapHit = false;
	void OnTriggerEnter(Collider other){
		if (other.tag == "BladeTrap") {
			Debug.Log ("Trap!");
			isTrapHit = true;
		}
	}
	void OnTriggerExit(Collider other){
		if (other.tag == "BladeTrap") {
			isTrapHit = false;
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
