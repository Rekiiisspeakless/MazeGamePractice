using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMovementControl : MonoBehaviour {

	Animator anim;
	bool boolper, boolper2, boolper3;
	public float turnSmoothing = 150f;
	public float dampingTime = 0.1f;
	public float audioFootstepDelay = 0f;
	public const float _audioFootstepDelay = 0.175f;
	public float currentHealth = 100f;
	public float fullHealth = 100f;
	public static float _getHitDelay = 12f;
	public bool getHit = false;
	public Maze maze;
	public bool attacking = false;
	public static float _attackingDelay = 10f;
	public bool isSwordEquipped = false;
	public float sneakSpeed = 0.5f;
	public bool isSneak = false;

	private float attackingDelay = _attackingDelay;
	private float getHitDelay = _getHitDelay;
	private float delay = 0.001f;
	private HashID hash;
	private Rigidbody playerRidgidBody;
	private Transform playerTransform;
	private Quaternion playerRotation;
	private GameObject menu;
	private GameObject sword;
	private GameObject headerText;
	private GameObject retryText;
	private TrapDetector trapDetector;
	private bool gameover = false;
	private bool gamewin = false;
	void Start(){
		playerRotation = playerTransform.rotation;
		trapDetector = GameObject.Find ("TrapDetector").GetComponent<TrapDetector>();
		if (trapDetector == null) {
			Debug.Log ("trapDetector is null!");
		}
		sword = GameObject.Find ("Sword");
		sword.SetActive (false);
		maze = GameObject.Find ("GameManager").GetComponent<Maze> ();
		headerText = GameObject.Find ("HeaderText");
		retryText = GameObject.Find ("RetryText");
		menu = GameObject.Find ("GameOverMenu");
		menu.SetActive (false);
	}

	void Awake ()
	{
		
		anim = GetComponentInChildren<Animator>();
		playerRidgidBody = GetComponent<Rigidbody>();
		playerTransform = GetComponent<Transform> ();
	}

	public void Walk ()
	{
		anim.SetBool ("isHit", false);
		boolper = anim.GetBool("isWalk");
		anim.SetBool ("isWalk", !boolper);
		anim.SetBool ("isIdle", false);
		anim.SetBool ("isAttack", false);
		anim.SetBool ("isRoll", false);
		anim.SetBool ("isSneak", false);
		isSneak = false;
	}


	public void Attack()
	{
		anim.SetBool ("isSneak", false);
		anim.SetBool ("isHit", false);
		anim.SetBool ("isWalk", false);
		anim.SetBool ("isIdle", false);
		anim.SetBool ("isAttack", true);
		anim.SetBool ("isRoll", false);
	}

	public void Idle(){
		anim.SetBool ("isSneak", false);
		anim.SetBool ("isHit", false);
		anim.SetBool ("isWalk", false);
		anim.SetBool ("isIdle", true);
		anim.SetBool ("isAttack", false);
		anim.SetBool ("isRoll", false);
		isSneak = false;
	}
	public void GetHit(){
		anim.SetBool ("isSneak", false);
		anim.SetBool ("isHit", true);
		anim.SetBool ("isWalk", false);
		anim.SetBool ("isIdle", false);
		anim.SetBool ("isAttack", false);
		anim.SetBool ("isRoll", false);
	}
	public void Sneak(){
		anim.SetBool ("isSneak", true);
		anim.SetBool ("isHit", false);
		anim.SetBool ("isWalk", false);
		anim.SetBool ("isIdle", false);
		anim.SetBool ("isAttack", false);
		anim.SetBool ("isRoll", false);
		isSneak = true;
	}
	public void Roll(){
		anim.SetBool ("isSneak", false);
		anim.SetBool ("isRoll", true);
	}
	public void Die(){
		anim.SetBool ("isDead", true);
		anim.SetBool ("isSneak", false);
		anim.SetBool ("isHit", false);
		anim.SetBool ("isWalk", false);
		anim.SetBool ("isIdle", false);
		anim.SetBool ("isAttack", false);
		anim.SetBool ("isRoll", false);
	}

	void Update () {
		
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		bool x = Input.GetButton("Attack");
		if (gameover) {
			return;
		}
		if (gamewin) {
			return;
		}
		if (currentHealth == 0f) {
			gameover = true;
			menu.SetActive (true);
		}
		MovementManagement(h, v);
		AudioManagement ();
	}

	void OnTriggerEnter(Collider other){
		Debug.Log ("Hit!");
		if (other.tag == "Apple") {
			currentHealth = (currentHealth + 20f > fullHealth) ? fullHealth : currentHealth + 20f ;
		}
		if (other.tag == "Potion") {
			currentHealth = fullHealth;
		}
		if (other.tag == "Sword") {
			isSwordEquipped = true;
			sword.SetActive (true);
		}
		if(other.tag == "Goal"){
			menu.SetActive (true);
			headerText.GetComponent<Text> ().text = "You Win!";
			retryText.GetComponent<Text>().text = "Play Again";
			gamewin = true;
		}
		if (other.tag == "Portal") {
			Debug.Log ("Portal Hit");
			for (int i = 0; i < maze.currentPortalNum; ++i) {
				if (Vector3.Distance (playerTransform.position, maze.portalPosition[i]) < 1) {
					playerTransform.position = new Vector3(maze.teleportPosition [i].x, 
						playerTransform.position.y, maze.teleportPosition [i].z);
				}
			}
		}
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
		if (getHit) {
			getHitDelay --;
		}
		if (attacking) {
			attackingDelay--;
			if (attackingDelay <= 0) {
				attacking = false;
			}
		}
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("roll")) {
			playerRidgidBody.MovePosition(playerRidgidBody.position + playerTransform.forward * 1f * Time.deltaTime);
		}

		if (maze.playerHit && !anim.GetCurrentAnimatorStateInfo(0).IsName("roll")) {
			getHit = true;
			getHitDelay = _getHitDelay;

		}else if (trapDetector.isTrapHit) {
			if (!anim.GetCurrentAnimatorStateInfo (0).IsName ("roll")) {
				GetHit ();
				currentHealth -= 5f;
			}
			trapDetector.isTrapHit = false;

		}else if(Input.GetButtonDown("Roll")){
			Roll ();
		}else if(getHitDelay <= 0 && getHit ){
			GetHit ();
			currentHealth -= 5f;
			getHit = false;
		}else if (Input.GetButtonDown ("Attack")) {
			Attack ();
			attacking = true;
			attackingDelay = _attackingDelay;
		}
		else if((Input.GetButton("Horizontal") || Input.GetButton("Vertical")) 
			&& !anim.GetCurrentAnimatorStateInfo(0).IsName("roll"))
		{
			if (Input.GetButton ("Shift")) {
				Sneak ();
			} else {
				Walk ();
			}
			if(Vector2.Distance(Vector2.zero, new Vector2(horizontal, vertical)) > delay){
				Vector3 moveDir = new Vector3 (horizontal, 0f, vertical);
				moveDir = playerRidgidBody.rotation * moveDir; //moveDir  * playerRidgidBody.rotation.eulerAngles;
				if (Input.GetButton ("Shift")) {
					playerRidgidBody.MovePosition (playerRidgidBody.position + moveDir * sneakSpeed * Time.deltaTime);
				} else {
					playerRidgidBody.MovePosition (playerRidgidBody.position + moveDir * Time.deltaTime);
				}
			}
		}
		else
		{
			Idle ();
		}

		if (currentHealth <= 0) {
			Die ();
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
		if (anim.GetBool("isWalk") == true && anim.GetBool("isRoll") == false && anim.GetBool("isSneak") == false)
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
