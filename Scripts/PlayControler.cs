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

	private KeyCombo Punch= new KeyCombo(new string[] {"X"});
	private KeyCombo DoublePunch= new KeyCombo(new string[] {"X", "X"});
	private KeyCombo SuperPunch= new KeyCombo(new string[] {"X", "B"});
	private KeyCombo falconPunch= new KeyCombo(new string[] {"X", "X","B"});
	private KeyCombo falconKick= new KeyCombo(new string[] {"X", "B","B"});
	
	public float playerHealth = 100;

	bool isGrounded = false;
	public Transform GroundCheck1;
	public LayerMask ground_layers;

	// Use this for initialization
	void Start () {



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
	}

	void OnCollisionEnter2D(Collision2D player){
		if (player.gameObject.layer == 9) {
			enemy = player.gameObject;
		}

	}

	void OnCollisionExit2D(Collision2D player){
		enemy = null;
	}

	IEnumerator wait( float damage)
	{
		int i;
		if (facingRight) {
			i = 1;
		}else {
			i = -1;
		}
		
		yield return new WaitForSeconds (1f);

		if (enemy != null) {
			enemy.gameObject.SendMessage ("doDamage", damage);
			enemy.GetComponent<Rigidbody2D> ().AddForce (Vector2.right * i * 2000);
		}
	}
	
	void attack(){
		float damage = 0;
		combo = 0;
		if (Punch.Check (playerNum)) {
			if (DoublePunch.Check (playerNum)) {
				if(falconPunch.Check(playerNum)){
					damage = 15;
					combo = 3;
				}else{
					damage = 10;
					combo = 2;
				}
			}else{
				damage = 5;

				combo = 1;
			}

			StartCoroutine("wait", damage);
		}


//		if (falconPunch.Check (playerNum) && enemy != null) {
//			enemy.gameObject.SendMessage ("doDamage", 30.0F);
//		}else if(SuperPunch.Check(playerNum) && enemy != null){
//			enemy.gameObject.SendMessage ("doDamage", 20.0F);
//		}else if(DoublePunch.Check(playerNum) && enemy != null){
//			enemy.gameObject.SendMessage ("doDamage", 15.0F);
//		}else if (Punch.Check (playerNum) && enemy != null) {
//			enemy.gameObject.SendMessage ("doDamage", 5.0F);
//		}
	}

	void doDamage(float value){
		playerHealth -= value;
	}

	void FixedUpdate(){
		//check if the empty object created overlaps with the 'ground_layers' within a radius of 1 units
		isGrounded = Physics2D.OverlapCircle(GroundCheck1.position, 1, ground_layers);

	}
	
	// Update is called once per frame
	void Update () {
		attack ();
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
