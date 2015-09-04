using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EnemyAI : MonoBehaviour {
	
	public Transform target;
	Vector3 storeTarget;
	Vector3 newTargetPosition;
	bool savePos;
	bool overrideTarget = false;

	public float Timer;
	public float LaserCooldown = 5f;
	public float LaserOffset = 2.2f;
	private float m_laserLastFired = 0f;
	private UnityEngine.Object m_laserBeamPrefab;

	private float maxDistance = 2f;
	Vector3 acceleration;
	Vector3 velocity;
	public float maxSpeed = 5f;
	public float wingSpan = 5f;
	float storeMaxSpeed;
	float targetSpeed;

	public float normalSpeed = 10f;
	public float attackingSpeed = 15f;
	public float evadingSpeed = 5f;

	Rigidbody rb;
	Transform obstacle;

	List<Vector3> EscapeDirections = new List<Vector3>();

	public enum AIStates
	{
		normal,
		attacking,
		evade
	}

	public AIStates aiStates;



	// Use this for initialization
	void Start () {
		storeMaxSpeed = maxSpeed;
		targetSpeed = storeMaxSpeed;

		rb = GetComponent<Rigidbody>();

		m_laserBeamPrefab = Resources.Load("LaserBeam");
		
		GameObject.Find("Player").GetComponent<SpacecraftController>().RegisterTarget(transform);
	}

	void Update ()
	{
		ManageSpeed ();

		switch (aiStates) {
		case AIStates.attacking:
			targetSpeed = attackingSpeed;
			break;
		case AIStates.evade:
			targetSpeed = evadingSpeed;
			break;
		case AIStates.normal:
			targetSpeed = normalSpeed;
			break;
		}
			
		if (Vector3.Distance (target.position, transform.position) > maxDistance && Timer > LaserCooldown) 
		{
			FireLaser();
			Timer = 0.0f;
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		Timer += Time.deltaTime;

		Debug.DrawLine (transform.position, target.position);

		Vector3 forces = MoveTowardsTarget (target.position);

		acceleration = forces;
		velocity += 2 * acceleration * Time.deltaTime;

		if (velocity.magnitude > maxSpeed) {
			velocity = velocity.normalized * maxSpeed;
		}

		rb.velocity = velocity;

		Quaternion desiredRotation = Quaternion.LookRotation (velocity);
		transform.rotation = Quaternion.Slerp (transform.rotation, desiredRotation, Time.deltaTime * 3);

		ObstacleAvoidance (transform.forward, 0);

		if (overrideTarget) {
			target.position = newTargetPosition;
		}
	}

	Vector3 MoveTowardsTarget(Vector3 target)
	{
		Vector3 distance = target - transform.position;

		if (distance.magnitude < 10) {
			aiStates = AIStates.evade;
			return distance.normalized * targetSpeed;
		} else {
			aiStates = AIStates.normal;
			return distance.normalized * targetSpeed;
		}
	}

	void ObstacleAvoidance(Vector3 direction, float offsetX)
	{
		RaycastHit[] hit = Rays(direction, offsetX);

		for(int i = 0; i < hit.Length - 1; i++)
		{
			if(hit[i].transform.root.gameObject != this.gameObject || hit[i].transform.root.gameObject.name != "LaserBeam" || hit[i].transform.root.gameObject.name != "Missile" || hit[i].transform.root.gameObject.name != "Explosion")
			{
				if(!savePos)
				{
					storeTarget = target.position;
					obstacle = hit[i].transform;
					savePos = true;
				}

				FindEscapeDirections(hit[i].collider);
			}
		}

		if(EscapeDirections.Count > 0)
		{
			aiStates = AIStates.evade;

			if(!overrideTarget)
			{
				newTargetPosition = getClosets();
				overrideTarget = true;
			}
		}

		float distance = Vector3.Distance (transform.position, target.position);

		if (distance < wingSpan) {
			if(savePos)
			{
				target.position = storeTarget;
				savePos = false;
			}

			overrideTarget = false;

			EscapeDirections.Clear();
		}
	}

	Vector3 getClosets()
	{
		Vector3 clos = EscapeDirections[0];
		float distance = Vector3.Distance (transform.position, EscapeDirections [0]);

		foreach (Vector3 dir in EscapeDirections) {
			float tempDistance = Vector3.Distance (transform.position, dir);

			if (tempDistance < distance) {
				distance = tempDistance;
				clos = dir;
			}
		}
		return clos;	
	}

	void FindEscapeDirections(Collider col)
	{
		RaycastHit hitUp;

		if(Physics.Raycast(col.transform.position, col.transform.up, out hitUp, col.bounds.extents.y * 2 + wingSpan))
		{

		}
		else
		{
			Vector3 dir = col.transform.position + new Vector3(0, col.bounds.extents.y * 2 + wingSpan, 0);

			if(!EscapeDirections.Contains(dir))
			{
				EscapeDirections.Add(dir);
			}
		}

		RaycastHit hitDown;
		
		if(Physics.Raycast(col.transform.position, -col.transform.up, out hitDown, col.bounds.extents.y * 2 + wingSpan))
		{
			
		}
		else
		{
			Vector3 dir = col.transform.position + new Vector3(0, -col.bounds.extents.y * 2 - wingSpan, 0);
			
			if(!EscapeDirections.Contains(dir))
			{
				EscapeDirections.Add(dir);
			}
		}

		RaycastHit hitRight;
		
		if(Physics.Raycast(col.transform.position, col.transform.right, out hitRight, col.bounds.extents.x * 2 + wingSpan))
		{
			
		}
		else
		{
			Vector3 dir = col.transform.position + new Vector3(0, col.bounds.extents.x * 2 + wingSpan, 0);
			
			if(!EscapeDirections.Contains(dir))
			{
				EscapeDirections.Add(dir);
			}
		}

		RaycastHit hitLeft;
		
		if(Physics.Raycast(col.transform.position, -col.transform.right, out hitLeft, col.bounds.extents.x * 2 + wingSpan))
		{
			
		}
		else
		{
			Vector3 dir = col.transform.position + new Vector3(0, col.bounds.extents.x * 2 - wingSpan, 0);
			
			if(!EscapeDirections.Contains(dir))
			{
				EscapeDirections.Add(dir);
			}
		}
	}

	RaycastHit[] Rays(Vector3 direction, float offsetX)
	{
		Ray ray = new Ray (transform.position + new Vector3 (offsetX, 0, 0), direction);
		Debug.DrawRay(transform.position + new Vector3 (offsetX, 0, 0), direction * 10 * maxSpeed, Color.red);

		float distanceToLookAhead = maxSpeed * 10;

		RaycastHit[] hits = Physics.SphereCastAll (ray, 5, distanceToLookAhead);

		return hits;
	}

	void ManageSpeed()
	{
		maxSpeed = Mathf.MoveTowards (maxSpeed, targetSpeed, Time.deltaTime * 5);
	}

	void FireLaser()
	{
		GameObject laser0 = (GameObject)Instantiate(m_laserBeamPrefab);
		laser0.transform.rotation = this.transform.rotation;
		laser0.transform.position = this.transform.position +
			transform.right * LaserOffset;
		
		GameObject laser1 = (GameObject)Instantiate(m_laserBeamPrefab);
		laser1.transform.rotation = this.transform.rotation;
		laser1.transform.position = this.transform.position - 
			transform.right * LaserOffset;
	}
}
