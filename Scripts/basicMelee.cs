using UnityEngine;
using System.Collections;
using System;

public class basicMelee : MonoBehaviour {

	public Transform rangeStart, rangeEnd;
	public bool playerDetected = false;
    public bool facingLeft = false;
    public int player1Health, player2Health;
    public int playerDamageDealt = 10;
    public Vector2 velocity;

    public Rigidbody2D opponent;
    public GameObject Player1, Player2;

	// Use this for initialization
	void Start () {
        player1Health = 100;
        player2Health = 100;
	}
	
	// Update is called once per frame
	void Update () {
		lineCasting();
        attackFunction();
        playerStatus();

        if (Input.GetKeyDown(KeyCode.A))
        {
            facingLeft = true;
        }
        else if (Input.GetKeyDown(KeyCode.D)){
            facingLeft = false;
        }
	}

	void lineCasting() {
		//test to see line of sight in scene
		Debug.DrawLine(rangeStart.position, rangeEnd.position, Color.green);


		playerDetected = Physics2D.Linecast(rangeStart.position, rangeEnd.position, 1 << LayerMask.NameToLayer("Player2"));
	}

    void attackFunction() {
        if (playerDetected == true) {
            if (Input.GetButtonDown("Jab")) {
                player2Health -= playerDamageDealt;
                if (facingLeft == true) {
                    opponent.MovePosition(opponent.position - velocity);
                } else {
                    opponent.MovePosition(opponent.position + velocity);
                }
            }
        }
    }

    void playerStatus() {
        if (player2Health <= 0) {
            Destroy(Player2);
        }

    }
}