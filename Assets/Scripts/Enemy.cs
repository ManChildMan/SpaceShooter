using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	//------------Variables----------------//
	private Transform target;
	public int moveSpeed;
	public int maxdistance;
	private Transform myTransform;
	//------------------------------------//    
	
	void Awake()
	{
		myTransform = transform;

		int randomPick = Mathf.Abs(Random.Range(0,10));


		if (randomPick > 3) {
			target = GameObject.Find ("Player").transform;
		} else {
			target = GameObject.Find ("spacestation_01").transform;
		}
	}
	
	
	void Start ()
	{
		maxdistance = 2;

		GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().RegisterTarget(this.gameObject.transform);
	}
	
	
	void Update ()
	{	
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
	}
}