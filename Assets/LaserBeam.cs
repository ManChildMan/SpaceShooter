using UnityEngine;
using System.Collections;

public class LaserBeam : MonoBehaviour {
    public float Velocity = 6.5f;
	// Use this for initialization
	void Start () {
        Destroy(gameObject, 10);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.forward * Velocity;     
	}
}
