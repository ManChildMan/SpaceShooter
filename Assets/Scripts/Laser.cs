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
    public AudioClip LaserHitClip;
    public AudioClip LaserMissClip;


    private UnityEngine.Object m_explosion;
    private LineRenderer m_lineRenderer;
    private AudioSource m_audioSource;
    private Player m_player;

    void Start()
    {
        m_explosion = Resources.Load("Explosion");
        m_lineRenderer = gameObject.GetComponentInChildren<LineRenderer>();
        m_lineRenderer.enabled = false;
        m_audioSource = GetComponent<AudioSource>();
        m_player = GameObject.Find("Player").GetComponent<Player>();
	}
	
	void Update()
    {
        LaserTemperature -= LaserTemperatureDecModifier;
        LaserTemperature = Mathf.Clamp(LaserTemperature, 0, LaserTemperatureMax);
        if (Input.GetButton("DesignateTarget"))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, 
                out hit, LaserRange, LayerMask))
            {
                m_player.SetTarget(hit.collider.transform);
            }
        }
        if (Input.GetButtonDown("Fire"))
        {
            StopCoroutine("FireLaser");
            StartCoroutine("FireLaser");
        }
	}

    IEnumerator FireLaser()
    {
        m_audioSource.Play();

        m_lineRenderer.enabled = true;
        while (Input.GetButton("Fire") && LaserTemperature < LaserTemperatureMax)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, LaserRange, LayerMask))
            {
                if (hit.transform.CompareTag("SpaceStation"))
                {
                    m_audioSource.Stop();
                    m_audioSource.clip = LaserHitClip;
                    m_audioSource.Play();
                    hit.transform.parent.parent.GetComponent<Target>().Health -= 5f;
                }
                else if (hit.transform.CompareTag("Enemy"))
                {
                    m_audioSource.Stop();
                    m_audioSource.clip = LaserHitClip;
                    m_audioSource.Play();
                    hit.transform.GetComponent<Target>().Health -= 5f;
                }
                else if (hit.transform.CompareTag("Navigation"))
                {
                    m_audioSource.Stop();
                    m_audioSource.clip = LaserHitClip;
                    m_audioSource.Play();
                }
                else m_audioSource.clip = LaserMissClip;
                m_lineRenderer.SetPosition(1, new Vector3(0, 0, hit.distance));
                GameObject explosion = (GameObject)Instantiate(m_explosion);
                explosion.GetComponent<Transform>().position = hit.point;
            }
            else
            {
                m_lineRenderer.SetPosition(1, new Vector3(0, 0, LaserRange));
            }
            LaserTemperature += LaserTemperatureIncModifier;
            yield return null;
        }
        m_lineRenderer.enabled = false;

        m_audioSource.Stop();
    }
}
