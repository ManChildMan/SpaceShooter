using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour
{
    public float LaserTemperature = 0f;
    public float LaserTemperatureMax = 100f;
    public float LaserTemperatureIncModifier = 0.08f;
    public float LaserTemperatureDecModifier = 0.08f;
    public float LaserRange = 1000f;
    
    
    public LayerMask LayerMask;

    MeshRenderer renderer;


    private LineRenderer m_lineRenderer;
    private UnityEngine.Object m_explosionPrefab;
    private Player m_controller;
    void Start()
    {

        m_controller = GameObject.Find("Player").GetComponent<Player>();
        m_explosionPrefab = Resources.Load("Explosion");
        m_lineRenderer = gameObject.GetComponentInChildren<LineRenderer>();
        m_lineRenderer.enabled = false;
	}
	
	void Update()
    {
        LaserTemperature -= LaserTemperatureDecModifier;
        LaserTemperature = Mathf.Clamp(LaserTemperature, 0, LaserTemperatureMax);
       
        if (Input.GetButtonDown("Fire"))
        {
            StopCoroutine("FireLaser");
            StartCoroutine("FireLaser");
        }

	}

    IEnumerator FireLaser()
    {
        m_lineRenderer.enabled = true;
        while (Input.GetButton("Fire") && LaserTemperature < LaserTemperatureMax)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, LaserRange, LayerMask))
            {
                GameObject explosion = (GameObject)Instantiate(m_explosionPrefab);
                explosion.GetComponent<Transform>().position = hit.point;

                m_lineRenderer.SetPosition(1, new Vector3(0, 0, hit.distance));
            }
            else
            {
                m_lineRenderer.SetPosition(1, new Vector3(0, 0, LaserRange));
            }
            LaserTemperature += LaserTemperatureIncModifier;
            yield return null;
        }
        m_lineRenderer.enabled = false;
    }
}
