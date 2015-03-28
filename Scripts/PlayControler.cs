using UnityEngine;
using System.Collections;

public class PlayControler : MonoBehaviour {
	
	Animator animator;
	Rigidbody2D character;
	string[] players = {"1", "2", "3", "4"};
	string playerNum;
	float jumpCount = 0;
	public GameObject enemy;
	bool facingRight = true;
	int combo = 0;

	GameController controller;
	GameObject gameControllerObject;

	private KeyCombo Special= new KeyCombo(new string[] {"B"});
	private KeyCombo Punch= new KeyCombo(new string[] {"X"});
	private KeyCombo DoublePunch= new KeyCombo(new string[] {"X", "X"});
	private KeyCombo SuperPunch= new KeyCombo(new string[] {"X", "B"});
	private KeyCombo falconPunch= new KeyCombo(new string[] {"X", "X","B"});
	private KeyCombo falconKick= new KeyCombo(new string[] {"X", "B","B"});
	
	public float playerHealth = 100;

	bool endCombo = true;
	bool isGrounded = false;
	public Transform GroundCheck1;
	public LayerMask ground_layers;
	
	// Use this for initialization
	void Start () {

		gameControllerObject = GameObject.FindWithTag ("GameController"); 
		controller = gameControllerObject.GetComponent<GameController> ();
		
		animator = GetComponent<Animator> ();
		
		enemy = null;
		
		character = GetComponent<Rigidbody2D> ();
		
		if (this.CompareTag ("Player1")) {
			playerNum = players [0];
		}
		if (this.CompareTag ("Player2")) {
			playerNum = players [1];
		}
		if (this.CompareTag ("Player3")) {
			playerNum = players [2];
		}
		if (this.CompareTag ("Player4")) {
			playerNum = players [3];
		}

		StartCoroutine ("comboComplete");
	}
	
	void OnCollisionEnter2D(Collision2D player){
		if (player.gameObject.layer == 9) {
			enemy = player.gameObject;
		}		
	}
	
	void OnCollisionExit2D(Collision2D player){
		enemy = null;
	}

	IEnumerator comboComplete(){
		float lastPressed = 0;
		while (true) {
			float currentTime = Time.time;
			if(Input.GetButtonDown("X_P"+playerNum) || Input.GetButtonDown("B_P"+playerNum)){
				lastPressed = Time.time;
			}
			if(Time.time >= (lastPressed + 0.45f) || combo > 5){
				combo = 0;
				endCombo = true;

			}else{
				endCombo = false;
			}
			yield return null;
		}
	}

	IEnumerator knockBackDamage(float damage)
	{
		int i;
		if (facingRight) {
			i = 1;
		}else {
			i = -1;
		}
		
		yield return new WaitForSeconds (0.5f);
		bool comboTemp = endCombo;
		yield return new WaitForSeconds (0.5f);
		
		if (enemy != null) {
			combo++;
			enemy.gameObject.SendMessage ("doDamage", damage);
			if (comboTemp) {
				int knockBack = 0;
				GameObject tempEnemy = enemy;
				Rigidbody2D tempRigid = tempEnemy.GetComponent<Rigidbody2D> ();
				while(knockBack < 10){
					tempRigid.velocity = new Vector2(15*i, tempRigid.velocity.y);
					knockBack++;
					yield return null;
				}
			}
		} else {
			combo = 0;
		}
	}
	
	void attack(){
		float damage = 0;
//		if (Special.Check (playerNum)) {
//			damage = 1;
//			StartCoroutine ("knockBackDamage", damage);
//		}

		if (Punch.Check (playerNum)) {
			if (DoublePunch.Check (playerNum)) {
				if (falconPunch.Check (playerNum)) {
					damage = 50;
				} else {
					//damage = 10;
				}
			} else {
				//damage = 5;
			}
			
			StartCoroutine ("knockBackDamage", damage);
		}

	}
	
	void doDamage(float value){
		playerHealth -= value;

		//controller.spawnScrap ();
	}
	
	void FixedUpdate(){
		//check if the empty object created overlaps with the 'ground_layers' within a radius of 1 units
		isGrounded = Physics2D.OverlapCircle(GroundCheck1.position, 1, ground_layers);
		attack ();
		
	}
	
	// Update is called once per frame
	void Update () {
		playerStatus ();
		Movement ();
	}
	
	bool canJump(){
		if (isGrounded) {
			jumpCount = 0;
			return true;
		} else if (jumpCount < 2) {
			return true;
		} else {
			return false;
		}
	}
	
	void Movement(){
		
		float speed = 14;
		float horizontal = Input.GetAxis ("Horizontal_P"+playerNum);
		
		if ((Input.GetButtonDown ("Jump_P"+playerNum)) && canJump()){
			jumpCount++;
			character.velocity = new Vector2(character.velocity.x, 0);
			character.AddForce(Vector2.up*1800);
		}
		if(horizontal > 0 && !facingRight){
			flip();
		}else if (horizontal < 0 && facingRight){
			flip();
		}
		character.velocity = new Vector2(horizontal*speed, character.velocity.y);
	}
	
	void playerStatus() {
		if (playerHealth <= 0) {
			Destroy(gameObject);
		}
		
	}
	
	void flip(){
		// Switch the way the player is labelled as facing
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}