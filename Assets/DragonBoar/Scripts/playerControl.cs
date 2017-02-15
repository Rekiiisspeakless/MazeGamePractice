using UnityEngine;
using System.Collections;

public class playerControl : MonoBehaviour 
{
	public Animator anim;
	int scream;
	int basicAttack;
	int getHit;
	int walk;
	int die;
	GameObject player;
	Rigidbody enemyRigidbody;
	Transform enemyTransform;
	float alertDist = 2f;
	float chaseDist = 0.1f;


	void Awake () 
	{
		anim = GetComponent<Animator>();
		enemyRigidbody = GetComponent<Rigidbody> ();
		enemyTransform = GetComponent<Transform> ();
		player = GameObject.Find ("mummy_rig");
		scream = Animator.StringToHash("Scream");
		basicAttack = Animator.StringToHash("Basic Attack");
		getHit = Animator.StringToHash("Get Hit");
		walk = Animator.StringToHash("Walk");
		die = Animator.StringToHash("Die");
	}

	void Update(){
		if (Vector3.Distance (enemyRigidbody.position, player.transform.position) < alertDist) {
			Vector3 enemyDir = enemyRigidbody.position - player.transform.position;
			enemyDir.y = 0;
			enemyRigidbody.rotation = Quaternion.Slerp (enemyRigidbody.rotation, Quaternion.LookRotation(- enemyDir), 0.05f);
			if (enemyDir.magnitude > chaseDist) {
				enemyRigidbody.transform.Translate (0, 0, 0.05f);
				Walk ();
			} else {
				BasicAttack ();
			}
		}
	}


	public void Scream ()
	{
		anim.SetTrigger(scream);
	}

	public void BasicAttack ()
	{
		anim.SetTrigger(basicAttack);
	}

	public void GetHit ()
	{
		anim.SetTrigger(getHit);
	}

	public void Walk ()
	{
		anim.SetTrigger(walk);
	}

	public void Die ()
	{
		anim.SetTrigger(die);
	}
		
}
