using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

	void OnGUI()
	{
		const int buttonWidth = 120;
		const int buttonHeight = 60;
		int highScoreDisplay;

		GameObject ScoreCounter = GameObject.Find ("ScoreCounter");
		Score scoreScript = ScoreCounter.GetComponent<Score> ();
		highScoreDisplay = scoreScript.getHighScore();
        GameObject.Find("HighScore").GetComponent<Text>().text = "High Score: " + (scoreScript.getHighScore() * 100).ToString();

		GUI.Label(new Rect(
			Screen.width / 2 - (buttonWidth / 3),
			(1 * Screen.height / 4), 
			buttonWidth,
			buttonHeight),
			"Game Over");

		GUI.Label(new Rect(
			Screen.width / 2 - (buttonWidth / 3),
			(1 * Screen.height / 3.5f), 
			buttonWidth,
			buttonHeight),
		    "High Score =" + highScoreDisplay);
		
		if (
			GUI.Button(
			// Center in X, 1/3 of the height in Y
			new Rect(
			Screen.width / 2 - (buttonWidth / 2),
			(1 * Screen.height / 3) - (buttonHeight / 2),
			buttonWidth,
			buttonHeight
			),
			"Retry!"
			)
			)
		{
			Time.timeScale = 1.0f;
			// Reload the level
			Application.LoadLevel("PlayerPrototype");
		}
		
		if (
			GUI.Button(
			// Center in X, 2/3 of the height in Y
			new Rect(
			Screen.width / 2 - (buttonWidth / 2),
			(2 * Screen.height / 3) - (buttonHeight / 2),
			buttonWidth,
			buttonHeight
			),
			"Back to menu"
			)
			)
		{
			// Reload the level
			Application.LoadLevel("Main");
		}
	}
}
