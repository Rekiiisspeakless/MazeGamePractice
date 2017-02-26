using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMummy : MonoBehaviour {
	Animator anim;
	public float timer = 20f;
	public float pingPong = 1f;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		timer -= Time.deltaTime * pingPong;
		if (timer <= -20f) {
			pingPong = -1f;
		}else if (timer > 20f){
			pingPong = 1f;
		}
		if (timer <= 0) {
			anim.SetBool ("isHipHop", true);
			anim.SetBool("isIdle", false);
		} else {
			anim.SetBool ("isHipHop", false);
			anim.SetBool("isIdle", true);
		}
			
	}
}
