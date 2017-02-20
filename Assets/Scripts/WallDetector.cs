using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour {
	public bool isBumpToWall = false;
	void OnTriggerEnter(Collider other){
		if (other.tag == "Wall") {
			isBumpToWall = true;
		}
	}
	void OnTriggerExit(Collider other){
		if (other.tag == "Wall") {
			isBumpToWall = false;
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
