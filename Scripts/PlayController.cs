using UnityEngine;
using System.Collections;

public class PlayController : MonoBehaviour {
	
	Animator animator;
	Rigidbody2D character;
	string[] players = {"1", "2", "3", "4"};
	string playerNum;
	int playerInt;
	public float Points = 0;
	public int scrapCount = 5;
	public int level = 1;
	float jumpCount = 0;
    float scrapCountLevel2;

	// who the player is currently touching
	public GameObject enemy;
	bool facingRight = true;
	int combo = 0;

	GameController controller;
	GameObject gameControllerObject;

	// all possible combos for player
	private KeyCombo Special= new KeyCombo(new string[] {"Y"});
	private KeyCombo Punch= new KeyCombo(new string[] {"X"});
	private KeyCombo DoublePunch= new KeyCombo(new string[] {"X", "X"});
	private KeyCombo SuperPunch= new KeyCombo(new string[] {"X", "Y"});
	private KeyCombo falconPunch= new KeyCombo(new string[] {"X", "X", "Y"});
	private KeyCombo falconKick= new KeyCombo(new string[] {"X", "Y","Y"});
     
	// stand in player health 
	public float playerHealth = 100;
	// true if player is not currently in combo
	bool endCombo = true;

	// true if player is touching the ground
	bool isGrounded = false;
	public Transform GroundCheck1;
	public LayerMask ground_layers;
	public GameObject[] levels;
	
	// Use this for initialization
	void Start () {

		// get gameController object
		gameControllerObject = GameObject.FindWithTag ("GameController"); 
		controller = gameControllerObject.GetComponent<GameController> ();

		// to animate players
		animator = GetComponent<Animator> ();

		// player is not touching another player
		enemy = null;
		character = GetComponent<Rigidbody2D> ();
        controller.gameOver.enabled = false;
        controller.nearlyOver.enabled = false;
        controller.warningFlash.enabled = false;

		// deternime which player this scrpit belongs to
		if (this.CompareTag ("Player1")) {
			playerNum = players [0];
			playerInt = 0;
			facingRight = true;
		}
		if (this.CompareTag ("Player2")) {
			playerNum = players [1];
			playerInt = 1;
			facingRight = false;
		}
		if (this.CompareTag ("Player3")) {
			playerNum = players [2];
			playerInt = 3;
			facingRight = true;
		}
		if (this.CompareTag ("Player4")) {
			playerNum = players [3];
			playerInt = 4;
			facingRight = false;
		}
        controller.getPlayerNum(playerInt);
        //Update health on start
        controller.updateHUD(playerHealth, playerInt, level);
        controller.updateScrapUI(scrapCount, playerInt);

        foreach (Transform t in transform){
            foreach (Transform child in t)
            {
                if (child.name == "GroundCheck")
                {
                    GroundCheck1 = child;
                }
            }
        }

        //GameObject child = gameObject.GetComponentInChildren<Transform>().gameObject;
        //GroundCheck1 = child.GetComponentInChildren<Transform>();

		// seperate thread to determin if player is in combo
		StartCoroutine ("comboComplete");
	}

	public void increaseScrap(){
        if (scrapCount >= 10) {
            scrapCount = 10;
        } else {
            scrapCount++;
        }

        if (scrapCountLevel2 >= 10) {
            scrapCountLevel2 = 10;
        } else {
            scrapCountLevel2++;
        }
	}

	// set player enemy to player its touching
	void OnTriggerEnter2D(Collider2D player){
		if (player.gameObject.layer == 9) {
			enemy = player.gameObject.transform.parent.gameObject;
		}		
	}

	// set to null when not touching player
	void OnTriggerExit2D(Collider2D player){
		enemy = null;
	}

	IEnumerator comboComplete(){
		float lastPressed = 0;
		while (true) {
			// determine the time of last button press
			if(Input.GetButtonDown("X_P"+playerNum) 
			   || Input.GetButtonDown("B_P"+playerNum) 
			   || Input.GetButtonDown("Y_P"+playerNum)){
				lastPressed = Time.time;
			}

			// determine if still in combo
			if(Time.time >= (lastPressed + 0.45f) || combo > 5){
				combo = 0;
				endCombo = true;
				yield return new WaitForSeconds(0.1f);
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

		// wait for half a second
		yield return new WaitForSeconds (0.5f);
		// save combo status
		bool comboTemp = endCombo;
		yield return new WaitForSeconds (0.5f);

		// do damage if player is touching another
		if (enemy != null) {
			combo++;
			if(enemy.gameObject.GetComponent<PlayController>().doDamage(damage)){
				// points for killing enemy
				controller.score[playerInt] += 100;
			}
			// points for damage
			controller.score[playerInt] += damage;
			//enemy.gameObject.SendMessage ("doDamage", damage);
			// if finished combo move enemy backwards
			if (comboTemp) {
				int knockBack = 0;
				GameObject tempEnemy = enemy;
				Rigidbody2D tempRigid = tempEnemy.GetComponent<Rigidbody2D> ();
				while(knockBack < 10){
					tempRigid.velocity = new Vector2(40*i, tempRigid.velocity.y);
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
		// determine if player has completed combo move
        if (Special.Check(playerNum)){
            damage = 1;
        }

		if (Punch.Check (playerNum)) {
			damage = 5;
		}
		if (DoublePunch.Check (playerNum)) {
			damage = 10;
		}
        if (falconPunch.Check(playerNum)){
			damage = 30;
		}

		// apply damage if there is any
		if (damage > 0) {
			StartCoroutine ("knockBackDamage", damage);
		}
	}

	// receive damage from other player
	bool doDamage(float value){
		playerHealth -= value;
        controller.updateHUD(playerHealth, playerInt, level);
		return (playerHealth <= 0);
		//controller.spawnScrap (gameObject);
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
		levelUp ();

        controller.timerUpdate();
        if (level == 2){
            controller.updateScrapUI(scrapCountLevel2, playerInt);
        } else {
            controller.updateScrapUI(scrapCount, playerInt);
        }
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
			controller.StartCoroutine("SpawnPlayer", playerInt);
            controller.spawnScrap(gameObject, (scrapCount + scrapCountLevel2));
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

	void levelUp(){
		if (scrapCount >= 10 && level < 2) {
			if(Input.GetButtonDown("B_P" + playerNum)){

				GameObject level2 = Instantiate (levels[1], transform.position, Quaternion.identity) as GameObject;
				foreach (Transform child in transform) {
					GameObject.Destroy(child.gameObject);
				}
				level2.transform.parent = transform;

				if(playerNum == "1"){
					facingRight = true;
				}else{
					facingRight = false;
				}
                                character.mass *= 1.2f;

                level++;
                playerHealth += 50;
                scrapCountLevel2 = 0;
                controller.updateHUD(playerHealth, playerInt, level);

                foreach (Transform t in transform)
                {
                    foreach (Transform child in t)
                    {
                        if (child.name == "GroundCheck")
                        {
                            GroundCheck1 = child;
                        }
                    }
                }
			}
		}
	}
}