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
	private bool gameover = false;
	private bool gamewin = false;
	void Start(){
		playerRotation = playerTransform.rotation;
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
	}


	public void Attack()
	{
		anim.SetBool ("isHit", false);
		anim.SetBool ("isWalk", false);
		anim.SetBool ("isIdle", false);
		anim.SetBool ("isAttack", true);
	}

	public void Idle(){
		anim.SetBool ("isHit", false);
		anim.SetBool ("isWalk", false);
		anim.SetBool ("isIdle", true);
		anim.SetBool ("isAttack", false);
	}
	public void GetHit(){
		anim.SetBool ("isHit", true);
		anim.SetBool ("isWalk", false);
		anim.SetBool ("isIdle", false);
		anim.SetBool ("isAttack", false);
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
		if (maze.playerHit) {
			getHit = true;
			getHitDelay = _getHitDelay;

		}else if(getHitDelay <= 0 && getHit){
			GetHit ();
			currentHealth -= 5f;
			getHit = false;
		}else if (Input.GetButton ("Attack")) {
			Attack ();
			attacking = true;
			attackingDelay = _attackingDelay;
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
