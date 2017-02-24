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
		if (other.tag == "BladeFanTrap") {
			isTrapHit = true;
		}
		if (other.tag == "AxeTrap") {
			isTrapHit = true;
		}
		if (other.tag == "SpearTrap") {
			isTrapHit = true;
		}

	}
	void OnTriggerExit(Collider other){
		if (other.tag == "BladeTrap") {
			isTrapHit = false;
		}
		if (other.tag == "BladeFanTrap") {
			isTrapHit = false;
		}
		if (other.tag == "AxeTrap") {
			isTrapHit = false;
		}
		if (other.tag == "SpearTrap") {
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
