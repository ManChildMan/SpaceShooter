﻿using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

	private GameObject HUD;

	void OnGUI()
	{
		const int buttonWidth = 120;
		const int buttonHeight = 60;

		int highScoreDisplay;
		
		GameObject ScoreCounter = GameObject.Find ("ScoreCounter");
		Score scoreScript = ScoreCounter.GetComponent<Score> ();
		highScoreDisplay = scoreScript.getHighScore();

		GUI.Label(new Rect(
			Screen.width / 2 - (buttonWidth / 2),
			(1 * Screen.height / 3.5f) - (buttonHeight / 2), 
			buttonWidth,
			buttonHeight),
		          "High Score = " + highScoreDisplay);
		
		if (
			GUI.Button(
			// Center in X, 1/3 of the height in Y
			new Rect(
			Screen.width / 2 - (buttonWidth / 2),
			(1 * Screen.height / 3) - (buttonHeight / 2),
			buttonWidth,
			buttonHeight
			),
			"Continue"
			)
			)
		{
			HUD = GameObject.FindGameObjectWithTag ("HUD");
			Time.timeScale = 1.0f;
			Destroy(HUD.gameObject.GetComponent<Pause>());
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
