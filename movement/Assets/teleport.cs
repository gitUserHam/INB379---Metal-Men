using UnityEngine;
using System.Collections;

public class teleport : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter2D(Collider2D player){
		int i;
		if(player.attachedRigidbody.position.x < 0){
			i = 1;
		}else {
			i = -1;
		}
		player.transform.position = new Vector2( -(player.attachedRigidbody.position.x +i), player.attachedRigidbody.position.y);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
