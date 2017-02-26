using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearTrap : MonoBehaviour {
	public GameObject spearPrefab;
	public GameObject spear;
	public float delay = 0f;
	public float timerThreshold = 5f;
	public float timer = 0f;
	public bool start = false;
	public float spearSpeed = 0.07f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (!start) {
			start = true;
			spear = Instantiate (spearPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
			spear.transform.rotation = gameObject.transform.rotation;
			spear.name = "Spear : (" + spear.transform.forward.x +", "+ spear.transform.forward.y +", "+ spear.transform.forward.z + ")";
		}
		if (spear != null) {
			Debug.Log ("Spear not null!");
			spear.transform.position += gameObject.transform.up * spearSpeed * Time.deltaTime;
		}
		if (timer >= timerThreshold) {
			if (spear != null) {
				Destroy (spear);
			}
			timer = 0f;
			spear = Instantiate (spearPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
			spear.transform.rotation = gameObject.transform.rotation;
			spear.name = "Spear : (" + spear.transform.forward.x +", "+ spear.transform.forward.y +", "+ spear.transform.forward.z + ")";
			Debug.Log ("time up!");
		}
	}
}
