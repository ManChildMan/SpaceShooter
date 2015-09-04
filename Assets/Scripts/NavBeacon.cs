using UnityEngine;
using System.Collections;

public class NavBeacon : MonoBehaviour {

   
	// Use this for initialization
	void Start () {

        GameObject.Find("Player").GetComponent<Player>().RegisterTarget(transform);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
