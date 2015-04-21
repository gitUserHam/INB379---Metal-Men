using UnityEngine;
using System.Collections;

public class portalScript : MonoBehaviour {
	
	public Transform destination; 

	void OnTriggerEnter2D(Collider2D player){
		GameObject thing = player.gameObject;
		if(player.gameObject.tag.Contains("Player")){
			thing = player.transform.parent.gameObject;
		}
		if (destination != null) {
			int i;
			// Take note of where the player is, and where they will end up.
			Vector2 entrance = this.transform.position;
			Vector2 exit = destination.transform.position;

			// LATER I WANT TO CHANGE THE DEST TELEPORT TO THE OBJECT SIZE/2 + 1.
			if(this.tag == "horizontalPortal") {
				// Accounts for spawning on either side of the map. Negative for the left, positive for the right.
				if (entrance.x > exit.x) {
					i = 2;
				} else { 
					i = -2;
				}
				thing.transform.position = new Vector2 (exit.x+i, player.transform.position.y);
			} else if (this.tag == "verticalPortal") {
				i = 1;				
				thing.transform.position = new Vector2 (player.transform.position.x, exit.y);
			}

		} else {
			print ("Invalid portal destination.");
			thing.transform.position = this.transform.position;
		}
	}
}
