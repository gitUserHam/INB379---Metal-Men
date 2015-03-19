using UnityEngine;
using System.Collections;

public class teleport_y : MonoBehaviour {
	// Use this for initialization
	void Start () {
		
	}
	
	void OnTriggerEnter2D(Collider2D player){
		player.transform.position = new Vector2( player.attachedRigidbody.position.x, -(player.attachedRigidbody.position.y+2));
		player.attachedRigidbody.velocity = new Vector2(player.attachedRigidbody.velocity.x, 2);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
