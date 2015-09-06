using UnityEngine;
using System.Collections;

public class SpaceStation : MonoBehaviour 
{
    private UnityEngine.Object m_explosionPrefab;
	private GameObject HUD;


    private bool m_exploding = false;
    private float m_explodeTime = 0f;
	void Start () {
        m_explosionPrefab = Resources.Load("MassiveExplosion");
        GameObject.Find("Player").GetComponent<Player>().RegisterTarget(transform);
		HUD = GameObject.FindGameObjectWithTag ("HUD");

	}
	
	void Update () {
        if (GetComponent<Target>().Health < 0)
        {

            //GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().UnregisterTarget(this.gameObject.transform);
            m_exploding = true;


           



        }

        if (m_exploding)
        {
            m_explodeTime += Time.deltaTime;

            if (Random.value <= 0.4)
            {
                Vector3 position = new Vector3(Random.Range(-20,20),Random.Range(-20,20),Random.Range(75,150));
                GameObject explosion = (GameObject)Instantiate(m_explosionPrefab);
                explosion.GetComponent<Transform>().position = position;
            }
            if (m_explodeTime > 6)
            {
                Destroy(gameObject, 0);
                Time.timeScale = 0;
                HUD.gameObject.AddComponent<GameOver>();
            }
        }
	}
}
