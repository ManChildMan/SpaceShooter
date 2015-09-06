using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	void OnGUI()
	{
		const int buttonWidth = 84;
		const int buttonHeight = 60;
		
		// Determine the button's place on screen
		// Center in X, 2/3 of the height in Y
		Rect buttonRect = new Rect (
			Screen.width / 2 - (buttonWidth / 2),
			(2 * Screen.height / 4) - (buttonHeight / 2),
			buttonWidth,
			buttonHeight
		);
		
		Rect buttonRect2 = new Rect (
			Screen.width / 2 - (buttonWidth / 2),
			(2 * Screen.height / 3) - (buttonHeight / 2),
			buttonWidth,
			buttonHeight
		);
		
		// Draw a button to start the game
		if (GUI.Button (buttonRect, "Start!")) {
			Time.timeScale = 1.0f;
			// On Click, load the first level.
			// "2dLevel" is the name of the first scene we created.
			Application.LoadLevel ("PlayerPrototype");
		}
		
		// Draw a button to quit the game
		if (GUI.Button (buttonRect2, "Quit")) {
			Time.timeScale = 1.0f;
			Application.Quit ();
		}
	}
}
