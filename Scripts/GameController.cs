using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameController(){

	}

	public GameObject scrapPart;
	public GameObject[] spawnPoints;
	public GameObject[] Players;

	const int numPlayers = 2;

	/* Spawns a certain amount of scrap parts.
	 * 	location: Can be any game object, ie a player or spawn spot.
	 */
	public void spawnScrap () {
		// For each scrap required.
		for (int i = 0; i < 1; i++) {
		// Spawn the scrap pieces.
			GameObject player = Players[0];
			// Set the spawn to the original game object.
			Vector2 pos = new Vector2(player.transform.position.x + 1, player.transform.position.y);
			// Spawn the scrap pieces.
			Instantiate (scrapPart, pos, transform.rotation);
			// Add temp force to make them fly off.
		}
	}

//	void SpawnPlayer(int player) {
//		// Get a random spawn point from the list.
//		int randLoc = Random.Range(0,spawnPoints.Length);
//		GameObject newPlayer = Instantiate (Players[player], spawnPoints[randLoc], Quaternion.identity) as GameObject;
//		// Enable the player controlling components on the new Game Object.
//		newPlayer.GetComponent<playerScript> ().enabled = true;
//		newPlayer.GetComponent<PlayControler> ().enabled = true;
//		// HAS TO BE FIXED. Needs the same collider or well have to do more checks.
//		//newPlayer.GetComponent<CircleCollider2D> ().enabled = true;
//		newPlayer.GetComponent<BoxCollider2D> ().enabled = true;
//	}


//	void RandomSpawn() {
//		int randLoc = Random.Range(0,spawnPoints.Length);
//		int randNum = Random.Range (3, 8);
//		spawnScrap (randNum, spawnPoints [randLoc]);
//	}
//
//	void Start() {
//		RandomSpawn ();
//	}
}