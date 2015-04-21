using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameController(){

	}
        
    public float[] score;
	public GameObject scrapPart;
	public GameObject[] spawnPoints;
	public GameObject[] Players;
    public Image[] healthMeter;
    public Image[] scrapUI;
    public Image[] profilePics;
    float seconds = 03; //original values 03
    float minutes = 3;  //                 3
    public Text timerText, gameOver, nearlyOver, startCountdown;
    public Image warningFlash;
    int playerNum;

	const int numPlayers = 2;

	/* Spawns a certain amount of scrap parts.
	 * 	location: Can be any game object, ie a player or spawn spot.
	 */
	public void spawnScrap (GameObject player, float amount) {
		// For each scrap required.
		for (int i = 0; i < amount; i++) {
		// Spawn the scrap pieces.
			// Set the spawn to the original game object.
			Vector2 pos = new Vector2(player.transform.position.x + 1, player.transform.position.y);
			// Spawn the scrap pieces.
			Instantiate (scrapPart, pos, transform.rotation);
			// Add temp force to make them fly off.
		}
	}

    public void getPlayerNum(int player){
        playerNum = player;
    }

    //Timer countdown function
    public void timerUpdate() {
        if (seconds <= 0)
        {
            seconds = 59;
            if (minutes >= 1)
            {
                minutes--;
            }
            else
            {
                minutes = 0;
                seconds = 0;
                //Formatting numbers
                timerText.text = minutes.ToString("f0") + ":0" + seconds.ToString("f0");
            }
        }
        else
        {
            seconds -= Time.deltaTime;
            
            if (minutes > 2) {
                StartCoroutine(preGameMovement(playerNum));
                timerText.enabled = false;
                startCountdown.text = seconds.ToString("f0");
                if (seconds < 1) {
                    startCountdown.text = "GO!";
                }
            } else if (minutes <= 2 && seconds <= 59){
                
                timerText.enabled = true;
                startCountdown.enabled = false;
            }

            //During last 10 seconds
            if (minutes == 0 && seconds <= 10){
                timerText.enabled = false;                  //
                nearlyOver.text = seconds.ToString("f0");   //  Switch to warning timer text
                nearlyOver.enabled = true;                  //
                //start flashing screen for time warning
                StartCoroutine(blink(10.0f));
                
            }   
        }

        if (Mathf.Round(seconds) <= 9)
        {
            timerText.text = minutes.ToString("f0") + ":0" + seconds.ToString("f0");
        }
        else
        {
            timerText.text = minutes.ToString("f0") + ":" + seconds.ToString("f0");
        }

        if (minutes == 0 && seconds == 0)
        {
            Destroy(GameObject.FindGameObjectWithTag("Player1"));
            Destroy(GameObject.FindGameObjectWithTag("Player2"));
            gameOver.enabled = true;
            nearlyOver.enabled = false;
            Destroy(warningFlash);
        }
    }

    IEnumerator blink(float duration) {
        if(minutes == 0 && seconds <= 10) {
            while (duration >= 0){
            
                warningFlash.enabled = true;
                yield return new WaitForSeconds(0.2f);
                warningFlash.enabled = false;
                yield return new WaitForSeconds(0.2f);
                duration--;
            }
        }
    }

    IEnumerator meterBlink(int player){
        if (scrapUI[player].fillAmount == 0.5) {
            while (scrapUI[player].fillAmount == 0.5) {
                scrapUI[player].enabled = false;
                profilePics[player].enabled = false;
                yield return new WaitForSeconds(0.9f);
                scrapUI[player].enabled = true;
                profilePics[player].enabled = true;
                yield return new WaitForSeconds(0.9f);
            }
        }
    }

    IEnumerator preGameMovement(int player) {
        Players[player].GetComponent<PlayController>().enabled = false;   
        yield return new WaitForSeconds(3.0f);
        Players[player].GetComponent<PlayController>().enabled = true;   
    }


    //updates the HUD by obtaining HP and Player number from PlayController
    public void updateHUD(float playerHealth, int player, int level)
    {
        //scale health according to player level 1 health

        //check if player's health exceeds 100 after leveling up
        if (level == 2 && level!= 1) {
            //scale health according to player level 2 health
            float scaledHealth;
            scaledHealth = (playerHealth / 300) + 0.5f;

            //Set health meter value to player's current health
            healthMeter[player].fillAmount = scaledHealth;
        } 
        if (level == 1 && level != 2) {
            float characterHealth;
            characterHealth = (playerHealth / 200) + 0.5f;
            healthMeter[player].fillAmount = characterHealth;
        }

    }

    //Update scrap count in HUD
    public void updateScrapUI(float scrapCount, int player)
    {
        float temp = scrapCount / 20; //Scale number for fill amount assignment
        scrapUI[player].color = Color.cyan;
        scrapUI[player].fillAmount = temp;
        //indicates when meter is full
        if (temp == 0.5) {
            scrapUI[player].color = Color.yellow; 
            StartCoroutine(meterBlink(player));
        }

    }

	public IEnumerator SpawnPlayer(int player) {
		// wait half second before respawning player
		yield return new WaitForSeconds (0.5f);

		// Get a random spawn point from the list.
		int randLoc = Random.Range(0,spawnPoints.Length);
		GameObject newPlayer = Instantiate (Players[player], spawnPoints[randLoc].transform.position, Quaternion.identity) as GameObject;

		// invulnrable for 2 seconds
		yield return new WaitForSeconds (2f);
		newPlayer.GetComponent<Rigidbody2D> ().isKinematic = false;
		foreach(Collider2D c in newPlayer.GetComponentsInChildren<Collider2D> ()) {
			c.enabled = true;
		}
        newPlayer.GetComponent<PlayController>().enabled = true;
	}
	
	
}