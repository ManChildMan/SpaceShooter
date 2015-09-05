using UnityEngine;
using System.Collections;

public class EnemyLaser : MonoBehaviour {

	public float LaserTemperature = 0f;
	public float LaserTemperatureMax = 100f;
	public float LaserTemperatureIncModifier = 0.08f;
	public float LaserTemperatureDecModifier = 0.08f;
	public float LaserRange = 1000f;

	public float LaserCooldown = 1.0f;
	public float Timer;
	
	public LayerMask LayerMask;
	
	MeshRenderer renderer;

    private Target m_playerTarget;
    private Target m_stationTarget;
	
	
	private LineRenderer m_lineRenderer;
	private UnityEngine.Object m_explosionPrefab;
	private Player m_controller;
	void Start()
	{
		m_explosionPrefab = Resources.Load("Explosion");
		m_lineRenderer = gameObject.GetComponentInChildren<LineRenderer>();
		m_lineRenderer.enabled = false;

        m_playerTarget = GameObject.Find("Player").GetComponent<Target>();
        m_stationTarget = GameObject.Find("spacestation_01").GetComponent<Target>();
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
                if (hit.transform.CompareTag("Player"))
                {
                    m_playerTarget.Health -= 5;
                }
                if (hit.transform.CompareTag("SpaceStation"))
                {
                    m_stationTarget.Health -= 5;
                }
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
