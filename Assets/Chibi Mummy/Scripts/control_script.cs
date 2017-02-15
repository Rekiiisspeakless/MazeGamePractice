using UnityEngine;
using System.Collections;

public class control_script : MonoBehaviour {

	Animator anim;
	bool boolper, boolper2, boolper3;
	public float turnSmoothing = 75f;
	public float dampingTime = 0.1f;
	public float audioFootstepDelay = 0f;
	public const float _audioFootstepDelay = 0.175f;

	private float delay = 0.001f;
	private HashID hash;
	private Rigidbody playerRidgidBody;
	private Transform playerTransform;
	private Quaternion playerRotation;
	void Start(){
		playerRotation = playerTransform.rotation;
	}

	void Awake ()
	{
		anim = GetComponentInChildren<Animator>();
		playerRidgidBody = GetComponent<Rigidbody>();
		playerTransform = GetComponent<Transform> ();
	}

	public void Walk ()
	{

		boolper = anim.GetBool("isWalk");
		anim.SetBool ("isWalk", !boolper);
		anim.SetBool ("isIdle", false);
		anim.SetBool ("isAttack", false);
	}


	public void Attack()
	{
		anim.SetBool ("isWalk", false);
		anim.SetBool ("isIdle", false);
		anim.SetBool ("isAttack", true);
	}

	public void Idle(){
		anim.SetBool ("isWalk", false);
		anim.SetBool ("isIdle", true);
		anim.SetBool ("isAttack", false);
	}

	void Update () {
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		bool x = Input.GetButton("Attack");

		MovementManagement(h, v);
		AudioManagement ();
	}

	void OnTriggerEnter(Collider ohter){
		Debug.Log ("Hit!");
	}

	void MovementManagement(float horizontal, float vertical)
	{
		/*if(horizontal != 0f && vertical != 0f)
		{
			Debug.Log ("Player Rotate!");
			PlayerRotate(horizontal, vertical);
		}*/

		if (horizontal != 0f) {
			Debug.Log ("Player Rotate!");
			playerRotation *= Quaternion.AngleAxis (horizontal * turnSmoothing * Time.deltaTime, Vector3.up);
			playerTransform.rotation = playerRotation;
			//playerRidgidBody.MoveRotation (PlayerRot);
		}
		if (Input.GetButton ("Attack")) {
			Attack ();
		}
		else if(Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
		{
			Walk ();
			if(Vector2.Distance(Vector2.zero, new Vector2(horizontal, vertical)) > delay){
				Vector3 moveDir = new Vector3 (horizontal, 0f, vertical);
				moveDir = playerRidgidBody.rotation * moveDir; //moveDir  * playerRidgidBody.rotation.eulerAngles;
				playerRidgidBody.MovePosition(playerRidgidBody.position + moveDir * Time.deltaTime);
			}
		}
		else
		{
			Idle ();
		}



	}

	void PlayerRotate(float horizontal, float vertical)
	{
		Vector3 playerDir = new Vector3(horizontal, 0f, vertical);
		//Debug.Log ("Before rotate: dir = " +"( " + playerDir.x + ", " + playerDir.y + ", " + playerDir.z + " )");
		playerDir = playerRidgidBody.rotation * playerDir;
		//Debug.Log ("After rotate: dir = " +"( " + playerDir.x + ", " + playerDir.y + ", " + playerDir.z + " )");
		Quaternion playerRot = Quaternion.LookRotation(playerDir, Vector3.forward);
		Quaternion newRot = Quaternion.Lerp(playerRidgidBody.rotation, playerRot, turnSmoothing * Time.deltaTime);
		playerRidgidBody.MoveRotation(newRot);

	}
	void AudioManagement()
	{
		//GetComponent<AudioSource>().Play();
		if (anim.GetBool("isWalk") == true)
		{
			audioFootstepDelay -= Time.deltaTime;
			if (!GetComponent<AudioSource>().isPlaying && audioFootstepDelay <= 0)
			{
				GetComponent<AudioSource>().Play();
				audioFootstepDelay = _audioFootstepDelay;
			}
		}
		else
		{
			GetComponent<AudioSource>().Stop();
		}
	}
}
