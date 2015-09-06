using UnityEngine;
using System.Collections;

public class SpaceStation : MonoBehaviour 
{
    private UnityEngine.Object m_explosionPrefab;
	private GameObject HUD;
   
	void Start () {
        m_explosionPrefab = Resources.Load("LargeExplosion");
        GameObject.Find("Player").GetComponent<Player>().RegisterTarget(transform);
		HUD = GameObject.FindGameObjectWithTag ("HUD");

	}
	
	void Update () {
        if (GetComponent<Target>().Health < 0)
        {
            GameObject explosion = (GameObject)Instantiate(m_explosionPrefab);
            explosion.GetComponent<Transform>().position = transform.position;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().UnregisterTarget(this.gameObject.transform);
			Time.timeScale = 0;
			HUD.gameObject.AddComponent<GameOver>();
            Destroy(gameObject, 0);
        }
	}
}
