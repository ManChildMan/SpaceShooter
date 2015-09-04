using UnityEngine;
using System.Collections;

public class SpaceStation : MonoBehaviour 
{
  
   
	void Start () {

        GameObject.Find("Player").GetComponent<Player>().RegisterTarget(transform);
	}
	
	void Update () {
	
	}
}
