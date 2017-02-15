using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPControl : MonoBehaviour {
	PlayerMovementControl player;
	Slider slider;
	void Awake(){
		player = GameObject.Find("mummy_rig").GetComponentInParent<PlayerMovementControl> ();
		slider = GetComponent<Slider>();
	}
	void LateUpdate(){
		slider.value = player.currentHealth;
	}
}
