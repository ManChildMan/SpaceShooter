using UnityEngine;
using System.Collections;

public class LaserBeam : MonoBehaviour {
    public float Velocity = 6.5f;
    private UnityEngine.Object m_explosionPrefab;
	// Use this for initialization
	void Start () {
        m_explosionPrefab = Resources.Load("Explosion");
        Destroy(gameObject, 15);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.forward * Velocity;     
	}

    void OnTriggerEnter(Collider collider)
    {
        GameObject explosion = (GameObject)Instantiate(m_explosionPrefab);
        explosion.GetComponent<Transform>().position =
            gameObject.transform.position;
        Destroy(gameObject, 0);
    }
}
