using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour {
	
	public float timer = 0.0f;
	private bool spawning = false;
	public GameObject prefab1;
	public GameObject prefab2;
	private GameObject prefab;
	public Transform spawn;
	public Transform spawn1;
	public Transform spawn2;
	public Transform spawn3;
	public Transform spawn4;
	public Transform spawn5;
	public Transform spawn6;
	public Transform spawn7;
	private Transform location;
	
	
	// Update is called once per frame
	void Update () {
		
		//check if spawning at the moment, if not add to timer
		if(!spawning){
			timer += Time.deltaTime;
			//Debug.Log(timer);
		}
		
		//when timer reaches 10 seconds, call Spawn function
		if(timer > 10)
			Spawn();
		
	}
	
	public void Spawn()
	{
		//set spawning to true, to stop timer counting in the Update function
		spawning = true;
		
		//reset the timer to 0 so process can start over
		timer = 0;
		
		//select a random number, inside a maths function absolute command to ensure it is a whole number
		int randomPick = Mathf.Abs(Random.Range(1,9));
		int randomPickEnemy = Mathf.Abs(Random.Range(1,2));

		//check what randomPickEnemy is, and select one of the 2 prefabs, based on that number
		if(randomPickEnemy == 1){
			prefab = prefab1;
			//Debug.Log("Choose enemy 1");
		}
		else if(randomPick == 2){
			prefab = prefab2;
			//Debug.Log("Choose enemy 2");
		}
		
		//check what randomPick is, and select one of the 8 locations, based on that number
		if(randomPick == 1){
			location = spawn;
			//Debug.Log("Choose pos 1");
		}
		else if(randomPick == 2){
			location = spawn1;
			//Debug.Log("Choose pos 2");
		}
		else if(randomPick == 3){
			location = spawn2;
			//Debug.Log("Choose pos 3");
		}
		else if(randomPick == 4){
			location = spawn3;
			//Debug.Log("Choose pos 4");
		}
		else if(randomPick == 5){
			location = spawn4;
			//Debug.Log("Choose pos 5");
		}
		else if(randomPick == 6){
			location = spawn5;
			//Debug.Log("Choose pos 6");
		}
		else if(randomPick == 7){
			location = spawn6;
			//Debug.Log("Choose pos 7");
		}
		else if(randomPick == 8){
			location = spawn7;
			//Debug.Log("Choose pos 8");
		}
		
		//create the object at point of the location variable
		Instantiate(prefab, location.position, Quaternion.identity);
		
		//set spawning back to false so timer may start again
		spawning = false;
	}
	
}

