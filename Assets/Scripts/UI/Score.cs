using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour {

	int score = 0;
	Text scoreDisplay;
	public int highScore = 20;
	
	public void increaseScore(int amount)
	{
		score += amount;

		if (score > highScore) {
			highScore = score;
		}
	}
	// Use this for initialization
	void Start () {
		scoreDisplay = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		scoreDisplay.text = "Score: " + score.ToString ();
	}

	public int getHighScore()
	{
		return highScore;
	}
}
