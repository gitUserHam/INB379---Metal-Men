using UnityEngine;
using System.Collections;

public class PlayControler : MonoBehaviour {

	Rigidbody2D character;
	string[] players = {"1", "2", "3", "4"};
	string playerNum;
	float jumpCount = 0;
	public GameObject enemy;

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


	void attack(){
		if (falconPunch.Check (playerNum) && enemy != null) {
			enemy.gameObject.SendMessage ("doDamage", 30.0F);
		}else if(SuperPunch.Check(playerNum) && enemy != null){
			enemy.gameObject.SendMessage ("doDamage", 20.0F);
		}else if(DoublePunch.Check(playerNum) && enemy != null){
			enemy.gameObject.SendMessage ("doDamage", 15.0F);
		}else if (Punch.Check (playerNum) && enemy != null) {
			enemy.gameObject.SendMessage ("doDamage", 5.0F);
		}
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

		character.velocity = new Vector2(horizontal*speed, character.velocity.y);
	}

	void playerStatus() {
		if (playerHealth <= 0) {
			Destroy(gameObject);
		}
		
	}
}
