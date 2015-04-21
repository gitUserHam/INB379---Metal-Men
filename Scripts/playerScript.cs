using UnityEngine;
using System.Collections;

public class playerScript : MonoBehaviour {
	
	public GameObject[] PlayerPrefabs;
	
	public Vector2[] spawnPoints = new Vector2[4];
	public GameObject[] Players = new GameObject[4];
	public int[] playerNum = {1, 2, 3, 4};	// The player number (1-4)
	int deathCount; // Tracks the number of times each player has died.
	
	const int startScrap = 10; // How much crap each player starts with.
	const int startHealth = 100;
	const int scrapValue = 5;	// As a percentage of max HP. (ie 5%)

	public static int currentScrap; // Current amount of scrap parts held by each player.
	int health;
	int playerLevel;
	bool isDamaged;	// Used to check when a player is damaged.
	
	// Temporary magic variables/methods.
	int damageAmount = 3;
//	Vector3 level1Scale = new Vector3(10.0f, 10.0f, 10.0f);
//	Vector3 level2Scale = new Vector3(12.0f, 15.0f, 10.0f);
//	Vector3 level3Scale = new Vector3(15.0f, 20.0f, 10.0f);

	// Was used to test things earlier, not working with new lists yet.
	void tempTakeDamage() {
		if (Input.GetKeyDown (KeyCode.Q)) {
			isDamaged = true;
		}
		if (Input.GetKeyDown (KeyCode.R)) {
			//GameController.spawnScrap(5);
		}
		if (Input.GetKeyDown (KeyCode.Z)) {
			Death(1);
		}
	}
	
//	void Upgrade() {
//		if (this.parts >= 350) {
//			playerLevel = 3;
//			transform.localScale = level3Scale;
//		} else if (this.parts >= 225) {
//			playerLevel = 2;
//			transform.localScale = level2Scale;
//		} else {
//			playerLevel = 1;
//			transform.localScale = level1Scale;
//		}
//	}

	/* Will be used to create player objects, (OOP). */
	void Player() {
	
	}

	void Damaged(int dmgAmount) {
		// Will need to be balanced later.
		dropScrap (dmgAmount);
	}

	void setupPlayer() {
		deathCount = 0;
		health = startHealth;
		currentScrap = startScrap;
		//playerNum;
	}

	// Use this for initialization
	void Start () {
		//		for (int i=1; i < numPlayers; i++) {
		//			this.playerNum = i;
		//			SpawnPlayer(playerNum);
		//		}

	}
	
	// Update is called once per frame
	void Update () {
		tempTakeDamage ();
//		for (int i = 0; i < numPlayers; i++) {
//			if (isDamaged [i] == true) {
//				dropScrap (i, damageAmount);
//				isDamaged [i] = false;
//				tempTakeDamage ();
//			}
//		}
	}
	
	// Drops a Scrap Part for each damage taken, and removes that from the players count.
	void dropScrap (int amount) {
		// If the player still has scrap avaliable.
		if (currentScrap > 0) {
			// Take the scrap from the source players count.
			currentScrap -= amount;
			// Create however many Scrap objects that are needed.
				//GameController.spawnScrap (amount, player);
		}
	}

	void Death(int player) {
		// Adjust for 0's in strings
		int num = player+1;
		// Destroy the current player object.
		GameObject target = GameObject.FindGameObjectWithTag ("Player"+num.ToString());
		DestroyObject (target);
		// Increment the death counter.
		deathCount++;
		// Respawn the player.
		SpawnPlayer (player);
	}
		
	void SpawnPlayer(int player) {
		// Get a random spawn point from the list.
		int randomizer = Random.Range(0,spawnPoints.Length);
		GameObject newPlayer = Instantiate (Players[player], spawnPoints[randomizer], Quaternion.identity) as GameObject;
		// Enable the player controlling components on the new Game Object.
		newPlayer.GetComponent<playerScript> ().enabled = true;
		newPlayer.GetComponent<PlayController> ().enabled = true;
		// HAS TO BE FIXED. Needs the same collider or well have to do more checks.
		//newPlayer.GetComponent<CircleCollider2D> ().enabled = true;
		newPlayer.GetComponent<BoxCollider2D> ().enabled = true;
	}
}