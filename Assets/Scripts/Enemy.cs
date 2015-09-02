using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	//------------Variables----------------//
	private GameObject target;
	public int moveSpeed;
	public int rotationSpeed;
	public int maxdistance;
	private Transform myTransform;
	private GameObject Level;
	public float Timer;
	public float LaserCooldown = 1f;
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
		target = GameObject.FindGameObjectWithTag ("Player");
		maxdistance = 2;
	}
	
	
	void Update ()
	{
		Timer += Time.deltaTime;
		
		if (Vector3.Distance(target.gameObject.transform.position, myTransform.position) > maxdistance)
		{
			// Get a direction vector from us to the target
			Vector3 dir = target.gameObject.transform.position - myTransform.position;
			
			// Normalize it so that it's a unit direction vector
			dir.Normalize();

			// The step size is equal to speed times frame time.
			float step = moveSpeed * Time.deltaTime;
			
			Vector3 newDir = Vector3.RotateTowards(transform.forward, dir, step, 0.0F);

			// Move our position a step closer to the target.
			transform.rotation = Quaternion.LookRotation(newDir);
			
			// Move ourselves in that direction
			myTransform.position += dir * moveSpeed * Time.deltaTime;
		}

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
