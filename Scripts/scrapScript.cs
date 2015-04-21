using UnityEngine;
using System.Collections;

public class scrapScript : MonoBehaviour {


	int numPlayers = 2; // Will later be imported from a Game Controller.

	// After 2 seconds the scrap parts tags are updated, only then are they able to be picked up.
	IEnumerator Start () {
		float randX = Random.Range(-500,500);
		float randY = Random.Range(400, 700);
		Vector2 force = new Vector2 (randX, randY);
		GetComponent<Rigidbody2D>().AddForce(force);
		yield return new WaitForSeconds (2);
		updateTag ();
	}
	
	// Update is called once per frame
	void updateTag () {
		this.gameObject.tag = "scrapPart";
                this.gameObject.layer = 11;
	}

	void OnCollisionEnter2D (Collision2D other) {
		// If the other object is a scrap part, pick it up and destroy the item.
		for (int i = 1; i <= numPlayers; i++) {
			if (other.gameObject.tag == "Player"+i && this.gameObject.tag == "scrapPart") {
				other.gameObject.GetComponent<PlayController>().increaseScrap();
				Destroy (gameObject);
			}
		}
	}
}