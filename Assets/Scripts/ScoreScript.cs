using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {

	int score = 0;
	Text scoreDisplay;
	
	public void increaseScore(int amount)
	{
		score += amount;
	}
	// Use this for initialization
	void Start () {
		scoreDisplay = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		scoreDisplay.text = "Score = " + score.ToString ();
	}
}
