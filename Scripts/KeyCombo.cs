using UnityEngine;
public class KeyCombo
{
	public string[] buttons;
	private int currentIndex = 0; //moves along the array as buttons are pressed
	
	public float allowedTimeBetweenButtons = 0.4f; //tweak as needed
	private float timeLastButtonPressed;
	
	public KeyCombo(string[] b)
	{
		buttons = b;
	}
	
	//usage: call this once a frame. when the combo has been completed, it will return true
	public bool Check(string player)
	{
		if (Time.time > timeLastButtonPressed + allowedTimeBetweenButtons) currentIndex = 0;
		{
			if (currentIndex < buttons.Length)
			{
                if ((buttons[currentIndex] == "X" && Input.GetButtonDown("X_P" + player)) || 
                    (buttons[currentIndex] == "Y" && Input.GetButtonDown("Y_P" + player)) ||
				    (buttons[currentIndex] == "B" && Input.GetButtonDown("B_P"+player)) ||
				    (buttons[currentIndex] != "X" && buttons[currentIndex] != "Y" && buttons[currentIndex] != "B" && Input.GetButtonDown(buttons[currentIndex])))
				{
					timeLastButtonPressed = Time.time;
					currentIndex++;
				}
				
				if (currentIndex >= buttons.Length)
				{
					currentIndex = 0;
					return true;
				}
				else return false;
			}
		}
		
		return false;
	}
}
