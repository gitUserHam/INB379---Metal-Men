﻿using UnityEngine;
using System.Collections;

public class scrapScript : MonoBehaviour {


	int numPlayers = 2; // Will later be imported from a Game Controller.

	// After 2 seconds the scrap parts tags are updated, only then are they able to be picked up.
	IEnumerator Start () {
		yield return new WaitForSeconds (2);
		updateTag ();
	}
	
	// Update is called once per frame
	void updateTag () {
		this.gameObject.tag = "scrapPart";
	}

	void OnCollisionEnter2D (Collision2D other) {
		// If the other object is a scrap part, pick it up and destroy the item.
		for (int i = 1; i < numPlayers; i++) {
			if (other.gameObject.tag == "Player"+i) {
				print ("Player"+i);
				//Increment the player touching the scraps count by one.
				//other.gameObject.GetComponent(playerScript.currentScrap[i-1]);
				playerScript.currentScrap[i-1] += 1;
				//Destroy the scrap part.
				Destroy (gameObject);
			}
		}
	}
}