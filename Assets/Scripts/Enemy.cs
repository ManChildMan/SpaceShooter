using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	//------------Variables----------------//
	private Transform target;
	public int moveSpeed;
	public int maxdistance;
	private Transform myTransform;

	public float Timer;
	public float LaserCooldown = 2f;
	public float LaserOffset = 2.2f;
	private float m_laserLastFired = 0f;
	private UnityEngine.Object m_laserBeamPrefab;
	//------------------------------------//    
	
	void Awake()
	{
		myTransform = transform;
	}
	
	
	void Start ()
	{
		m_laserBeamPrefab = Resources.Load("LaserBeam");
		maxdistance = 2;

		GameObject.Find("Player").GetComponent<SpacecraftController>().RegisterTarget(transform);
		target = GameObject.FindGameObjectWithTag ("Player").transform;
	}
	
	
	void Update ()
	{
		Timer += Time.deltaTime;
		
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

		if (Vector3.Distance (target.gameObject.transform.position, myTransform.position) > maxdistance && Timer > LaserCooldown) 
		{
			FireLaser();
			Timer = 0.0f;
		}
	}

	void FireLaser()
	{
		GameObject laser0 = (GameObject)Instantiate(m_laserBeamPrefab);
		laser0.transform.rotation = transform.rotation;
		laser0.transform.position = transform.position +
			transform.right * LaserOffset;
		
		GameObject laser1 = (GameObject)Instantiate(m_laserBeamPrefab);
		laser1.transform.rotation = transform.rotation;
		laser1.transform.position = transform.position - 
			transform.right * LaserOffset;
	}
}
