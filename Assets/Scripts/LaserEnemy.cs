using UnityEngine;
using System.Collections;

public class LaserEnemy : MonoBehaviour {

	public float LaserTemperature = 0f;
	public float LaserTemperatureMax = 100f;
	public float LaserTemperatureIncModifier = 0.08f;
	public float LaserTemperatureDecModifier = 0.08f;
	public float LaserRange = 1000f;

	public float Timer;
	public float LaserCooldown = 2f;
	
	public LayerMask LayerMask;
	
	MeshRenderer renderer;
	
	
	private LineRenderer m_lineRenderer;
	private UnityEngine.Object m_explosionPrefab;
	void Start()
	{
		m_explosionPrefab = Resources.Load("Explosion");
		m_lineRenderer = gameObject.GetComponentInChildren<LineRenderer>();
		m_lineRenderer.enabled = false;
	}
	
	void Update()
	{
		Timer += Time.deltaTime;

		LaserTemperature -= LaserTemperatureDecModifier;
		LaserTemperature = Mathf.Clamp(LaserTemperature, 0, LaserTemperatureMax);
		
		if (Timer > LaserCooldown)
		{
			StopCoroutine("FireLaser");
			StartCoroutine("FireLaser");
		}
		
	}
	
	IEnumerator FireLaser()
	{
		m_lineRenderer.enabled = true;
		while (Timer > LaserCooldown && LaserTemperature < LaserTemperatureMax)
		{
			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.forward, out hit, LaserRange, LayerMask))
			{
				GameObject explosion = (GameObject)Instantiate(m_explosionPrefab);
				explosion.GetComponent<Transform>().position = hit.point;
				
				m_lineRenderer.SetPosition(1, new Vector3(0, 0, hit.distance));
				Timer = 0.0f;
			}
			else
			{
				m_lineRenderer.SetPosition(1, new Vector3(0, 0, LaserRange));
				Timer = 0.0f;
			}
			LaserTemperature += LaserTemperatureIncModifier;
			yield return null;
			Timer = 0.0f;
		}
		m_lineRenderer.enabled = false;
		Timer = 0.0f;
	}
}
