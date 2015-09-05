using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	//------------Variables----------------//
	private Transform target;
	public int moveSpeed;
	public int maxdistance;
	private Transform myTransform;
	//------------------------------------//    
    private UnityEngine.Object m_explosionPrefab;
	void Awake()
	{
		myTransform = transform;

		

        m_explosionPrefab = Resources.Load("LargeExplosion");
        FindTarget();
	}
	
	
	void Start ()
	{
		maxdistance = 2;

		GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().RegisterTarget(this.gameObject.transform);
	}

    void FindTarget()
    {
        int randomPick = Mathf.Abs(Random.Range(0, 10));
        if (randomPick > 3)
        {
            target = GameObject.Find("Player").transform;
        }
        else
        {
            target = GameObject.Find("spacestation_01").transform;
        }
    }
	
	void Update ()
	{
        if (target == null)
        {
            return;
        }
		Vector3 dir = (target.position - transform.position)	.normalized;
		RaycastHit hit;

		if(Physics.Raycast(transform.position, transform.forward, out hit, 20)){
			if(hit.transform != transform){
				dir += hit.normal * moveSpeed;
			}
		}

		Vector3 leftR = transform.position;
		Vector3 rightR = transform.position;
		Vector3 topR = transform.position;
		Vector3 bottomR = transform.position;

		leftR.x -= 5;
		rightR.x += 5;
		topR.y += 2;
		bottomR.y += 2;

		if(Physics.Raycast(leftR, transform.forward, out hit, 20)){
			if(hit.transform != transform){
				dir += hit.normal * moveSpeed;
			}
		}

		if(Physics.Raycast(rightR, transform.forward, out hit, 20)){
			if(hit.transform != transform){
				dir += hit.normal * moveSpeed;
			}
		}

		if(Physics.Raycast(topR, transform.forward, out hit, 20)){
			if(hit.transform != transform){
				dir += hit.normal * moveSpeed;
			}
		}

		if(Physics.Raycast(bottomR, transform.forward, out hit, 20)){
			if(hit.transform != transform){
				dir += hit.normal * moveSpeed;
			}
		}

		Quaternion desiredRotation = Quaternion.LookRotation (dir);
		transform.rotation = Quaternion.Slerp (transform.rotation, desiredRotation, Time.deltaTime);
		transform.position += transform.forward * 20 * Time.deltaTime;

        if (GetComponent<Target>().Health < 0)
        {
            GameObject explosion = (GameObject)Instantiate(m_explosionPrefab);
            explosion.GetComponent<Transform>().position = transform.position;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().UnregisterTarget(this.gameObject.transform);
			GameObject scoreCounter = GameObject.Find("ScoreCounter");
			scoreCounter.GetComponent<ScoreScript>().increaseScore(1);
            Destroy(gameObject, 0);
        }
	}
}