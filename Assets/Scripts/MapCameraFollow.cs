using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCameraFollow : MonoBehaviour {

	public Transform player;

	void LateUpdate(){
		transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
	}

}
