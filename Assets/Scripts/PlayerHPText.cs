using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPText : MonoBehaviour {

	public Text healthText;
	public PlayerMovementControl player;
	// Use this for initialization
	void Start () {
		player = GameObject.Find("mummy_rig").GetComponentInParent <PlayerMovementControl> ();
		healthText = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		healthText.text = "    HP:  " + player.currentHealth + " / " + player.fullHealth;
		//healthText.text = "HP:  !!!";
	}
}
