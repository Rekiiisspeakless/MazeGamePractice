using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour {

	public Animator anim;
	public float _sightAngle = 30f; 
	/*int scream;
	int basicAttack;
	int getHit;
	int walk;
	int die;*/
	GameObject player;
	Rigidbody enemyRigidbody;
	Transform enemyTransform;
	Maze maze;
	float attackDelay = 0f;
	const float _attackDelay = 3f;
	float alertDist = 2f;
	float chaseDist = 0.7f;
	float moveSpeed = 0.3f;
	CharacterController enemyController;
	public float  _getHitDelay = 15f; 
	public float enemyHealth = 100f;
	public float enemyCurrentHealth = 100f;
	public bool synchLock = false;
	public float getHitDelay = 0f;
	void OnTriggerEnter(Collider other){
		//synchLock = true;
		if (enemyCurrentHealth <= 0) {
			return;
		}
		if (other.tag == "PlayerAttackRange" && player.GetComponent<PlayerMovementControl>().attacking) {
			
			GetHit ();
			enemyCurrentHealth -= 5f;
			Debug.Log ("currentHealth = " + enemyCurrentHealth);

			//anim.SetTrigger("GetHit");
			getHitDelay = _getHitDelay;
			if (enemyCurrentHealth == 0f) {
				Die ();
				//Destroy (gameObject);
			}
			Debug.Log ("Dragon" + "( " + enemyRigidbody.position.x + ", " 
				+ enemyRigidbody.position.y + ", " + enemyRigidbody.position.z + " ) hit!");
			//anim.SetBool ("isHit", false);
		}
		//synchLock = false;
	}

	void Awake () 
	{
		anim = GetComponent<Animator>();
		enemyController = GetComponent<CharacterController> ();
		enemyRigidbody = GetComponent<Rigidbody> ();
		enemyTransform = GetComponent<Transform> ();
		player = GameObject.Find ("mummy_rig");
		maze = GameObject.Find ("GameManager").GetComponent<Maze>();
		/*scream = Animator.StringToHash("Scream");
		basicAttack = Animator.StringToHash("Basic Attack");
		getHit = Animator.StringToHash("Get Hit");
		walk = Animator.StringToHash("Walk");
		die = Animator.StringToHash("Die");*/
	}

	void Update(){
		if (enemyCurrentHealth <= 0) {
			return;
		}
		/*if (synchLock) {
			return;
		}*/
		/*if (getHitDelay > 0) {
			getHitDelay--;
			return;
		}*/
		

		Vector3 enemyDir = player.transform.position  - enemyRigidbody.position;
		float angle = Vector3.Angle (enemyDir, enemyTransform.forward);
		if (Vector3.Distance (enemyRigidbody.position, player.transform.position) < alertDist && angle < _sightAngle) {
			//north: 1, west: 2, south: 3, east: 4
		//int dir = (enemyDir.x > 0)?((enemyDir.z > 0) ? : ): ;
			//int dir = 0;
			Vector3 leftup = new Vector3 (Mathf.Floor(enemyDir.x), 0f, Mathf.Ceil(enemyDir.z));
			Vector3 rightup = new Vector3 (Mathf.Ceil(enemyDir.x),  0f, Mathf.Ceil(enemyDir.z));
			Vector3 leftdown = new Vector3 (Mathf.Floor(enemyDir.x), 0f, Mathf.Floor(enemyDir.z));
			Vector3 rightdown = new Vector3 (Mathf.Ceil(enemyDir.x), 0f, Mathf.Floor(enemyDir.z));

			float leftupSlope = (leftup.z - transform.position.z) / (leftup.x - transform.position.x);
			float rightupSlope = (rightup.z - transform.position.z) / (rightup.x - transform.position.x);
			float leftdownSlope = (leftdown.z - transform.position.z) / (leftdown.x - transform.position.x);
			float rightdownSlope = (rightdown.z - transform.position.z) / (rightdown.x - transform.position.x);	
			
			float playerX = player.transform.position.x;
			float playerZ = player.transform.position.z;
			float enemyX = transform.position.x;
			float enemyZ = transform.position.z;
			int checkPointX = Mathf.FloorToInt (enemyX);
			int checkPointZ = Mathf.FloorToInt (enemyZ);
			//MazeCell checkCell = maze.cells [checkPointX, checkPointZ];

			bool bumpToWall = false;

			/*if (playerZ < rightupSlope * (playerX - enemyX) && playerZ > rightdownSlope * (playerX - enemyX)) {
				//check south
				Debug.Log("check south!");
				bumpToWall = (maze.cells [checkPointX, checkPointZ].south == null) ? false : true;
			}else if(playerZ > rightupSlope * (playerX - enemyX) && playerZ > leftupSlope * (playerX - enemyX)){
				//check east
				Debug.Log("check east!");
				bumpToWall = (maze.cells [checkPointX, checkPointZ].east == null) ? false : true;
			}else if(playerZ > leftdownSlope * (playerX - enemyX) && playerZ < leftupSlope * (playerX - enemyX)){
				//check north
				Debug.Log("check north!");
				bumpToWall = (maze.cells [checkPointX - 1, checkPointZ].south == null) ? false : true;
			}else if(playerZ < leftdownSlope * (playerX - enemyX) && playerZ < rightdownSlope * (playerX - enemyX)){
				//check west
				Debug.Log("check west!");
				bumpToWall = (maze.cells [checkPointX, checkPointZ - 1].east == null) ? false : true;
			}*/	

			

			if (attackDelay > 0) {
				attackDelay -= Time.deltaTime;
			}
			enemyDir.y = 0;
			enemyRigidbody.rotation = Quaternion.Slerp (enemyRigidbody.rotation, Quaternion.LookRotation (enemyDir), 0.05f);
			if (enemyDir.magnitude > chaseDist && !bumpToWall) {
				//enemyTransform.Translate (0, 0, 0.005f);

				Vector3 enemyMovePosition = enemyRigidbody.position + enemyDir.normalized * moveSpeed * Time.deltaTime;
				//Debug.Log ("move " + enemyMovement.magnitude);
				//Debug.Log ("( " + enemyMovePosition.x + ", " + enemyMovePosition.y + ", " + enemyMovePosition.z + " )");
				enemyRigidbody.MovePosition (enemyMovePosition);
				Walk ();
			} else if (getHitDelay > 0) {
				getHitDelay--;
			} else if (attackDelay <= 0 && !bumpToWall) {
				if(!player.GetComponent<PlayerMovementControl>().attacking){
					BasicAttack ();
					attackDelay = _attackDelay;
				}
			} else {
				Idle ();
			}
		}else if (getHitDelay > 0) {
			getHitDelay--;
		} else {
			
			Idle ();
		}
	}
	public void Idle(){
		anim.SetBool("isAttack", false);
		anim.SetBool ("isIdle", true);
		anim.SetBool ("isWalk", false);
		anim.SetBool ("isHit", false);
	}


	public void Scream ()
	{
		//anim.SetTrigger(scream);
	}

	public void BasicAttack ()
	{
		anim.SetBool ("isHit",false);
		anim.SetBool("isAttack", true);
		anim.SetBool ("isIdle", false);
		anim.SetBool ("isWalk", false);
	}

	public void GetHit ()
	{
		Debug.Log ("GetHit, isHit = " + anim.GetBool("isHit"));
		anim.SetBool ("isHit",true);
		anim.SetBool("isAttack", false);
		anim.SetBool ("isIdle", false);
		anim.SetBool ("isWalk", false);
		Debug.Log ("GetHit, isHit = " + anim.GetBool("isHit"));
	}

	public void Walk ()
	{
		anim.SetBool ("isHit",false);
		anim.SetBool("isAttack", false);
		anim.SetBool ("isIdle", false);
		anim.SetBool ("isWalk", true);
	}

	public void Die ()
	{
		anim.SetBool ("isHit",false);
		anim.SetBool("isAttack", false);
		anim.SetBool ("isIdle", false);
		anim.SetBool ("isWalk", false);
		anim.SetBool ("isDead", true);
	}
}
