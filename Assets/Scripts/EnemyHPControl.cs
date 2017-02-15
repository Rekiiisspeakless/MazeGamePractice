using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPControl : MonoBehaviour {
	EnemyControl enemy;
	Image enemyHPFill;
	void Awake(){
		enemy = GetComponentInParent<EnemyControl> ();
		enemyHPFill = GetComponent<Image> ();
	}
	void LateUpdate(){
		enemyHPFill.fillAmount = enemy.enemyCurrentHealth / enemy.enemyHealth;
	}
}
