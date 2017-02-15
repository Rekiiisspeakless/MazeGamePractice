using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour {

	public float rotateSpeed = 120f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround (transform.position, Vector3.up, rotateSpeed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player") {
			Destroy (gameObject);
		}
	}
}
