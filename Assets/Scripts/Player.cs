using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Player : MonoBehaviour {
    public float MinSpeed = 0;
    public float MaxSpeed = 0.5f;
    public float PitchModifier = 30;
    public float RollModifier = 30;
    public float YawModifier = 30;
    public float Velocity = 0f;
    public float VelocityModifier = 5;
    public float MissileCooldown = 1f;
    public float MissileOffset = 0.75f;
    public float MissileThreshold = 1;

    private float m_velocity = 0f;
    private float m_missileLastFired = 0f;
    private bool m_missileMode = false;
    private bool m_isLaunchAuthorised = false;
    private List<Transform> m_targets = new List<Transform>();
    private int m_target = -1;
    private UnityEngine.Object m_missile;
    private UnityEngine.Object m_indicator;
	private UnityEngine.Object m_explosionPrefab;
    private Transform m_reticle;
    private Transform m_launchAuthorised;
    private MeshRenderer m_reticleRenderer;
    private Color m_reticleDefaultColor;


    private Text m_missileStatus;
    private Text m_missilesLeft;
    private Color m_oldMissileTextColor;
    private int m_missileCount = 6;
    private AudioSource m_audio;
	private GameObject HUD;

    void Awake()
    {
        m_missile = Resources.Load("Missile");
        m_indicator = Resources.Load("TargetIndicator");
		m_explosionPrefab = Resources.Load("LargeExplosion");
        m_audio = GetComponent<AudioSource>();
    }

	void Start () 
    {
		HUD = GameObject.FindGameObjectWithTag ("HUD");

        m_launchAuthorised = GameObject.Find("LaunchAuthorised").transform;
        m_launchAuthorised.gameObject.SetActive(false);

        m_reticle = GameObject.Find("TargetReticle").transform;
        m_reticle.gameObject.SetActive(false);
        m_reticleRenderer = m_reticle.GetComponent<MeshRenderer>();
        m_reticleDefaultColor = m_reticleRenderer.material.color;

        m_missileStatus = GameObject.Find("MissileStatus").GetComponent<Text>();
        m_missilesLeft = GameObject.Find("MissileCount").GetComponent<Text>();
        m_oldMissileTextColor = m_missileStatus.color;
	}


    public void SetTarget(Transform transform)
    {
        int i = m_targets.FindIndex(x => x == transform);
        if(i != -1)
            m_target = i;
    }

	void Update () 
    {
		if (Input.GetButtonDown("Pause"))
		{
			Time.timeScale = 0.0f;
			HUD.gameObject.AddComponent<Pause>();
		}

        if (Input.GetButtonDown("CycleTargets"))
        {
            CycleTargets();
        }

        if (Input.GetButtonDown("UnlockTarget"))
        {
            UnlockTarget();
        }

        if (Input.GetButtonDown("ToggleMissileMode"))
        {
            m_missileMode = !m_missileMode;
            if (m_missileMode == true)
            {
                m_reticle.gameObject.SetActive(true);
                m_missileStatus.color = Color.red;
                m_missileStatus.text = "Missile Status: Armed";
                m_missilesLeft.color = Color.red;

            }
            else
            {
                m_reticle.gameObject.SetActive(false);
                m_missileStatus.color = m_oldMissileTextColor;
                m_missileStatus.text = "Missile Status: Disarmed";
                m_missilesLeft.color = m_oldMissileTextColor;
            }
        }

        if (m_missileMode)
        {
            if (m_target > -1)
            {
                Vector3 displacement = (m_targets[m_target].position -
                    transform.position).normalized;
                m_reticle.transform.position = transform.position +
                    displacement;
                float angle = Vector3.Angle(transform.forward, displacement);
                if (angle < MissileThreshold)
                {
                    m_isLaunchAuthorised = true;
                    m_launchAuthorised.gameObject.SetActive(true);
                    m_reticleRenderer.material.color = Color.red;
                }
                else
                {
                    m_isLaunchAuthorised = false;
                    m_launchAuthorised.gameObject.SetActive(false);
                    m_reticleRenderer.material.color = m_reticleDefaultColor;
                }
            }
        }
        else
        {
            m_isLaunchAuthorised = false;
            m_launchAuthorised.gameObject.SetActive(false);
        }

        if (Input.GetButton("Fire1") && m_missileLastFired > MissileCooldown && m_isLaunchAuthorised && m_missileCount > 0)
        {
            FireMissile();
            m_missileCount--;
            m_missilesLeft.text = "Missiles Remaining: " + 
                m_missileCount.ToString();
            m_missileLastFired = 0;
        }
        m_missileLastFired += Time.deltaTime;

        float pitchInput = Input.GetAxis("Vertical") * 
            Time.deltaTime * PitchModifier;
        float rollInput = Input.GetAxis("Horizontal") * 
            Time.deltaTime * RollModifier;
        float yawInput = Input.GetAxis("Yaw") * 
            Time.deltaTime * YawModifier;
        float velInput = Input.GetAxis("Velocity") * 
            Time.deltaTime * VelocityModifier;

        transform.rotation *= Quaternion.Euler(pitchInput, 0, 0);
        transform.rotation *= Quaternion.Euler(0, 0, -rollInput);
        transform.rotation *= Quaternion.Euler(0, yawInput, 0);

        m_velocity += velInput * VelocityModifier * Time.deltaTime;
        m_velocity = Mathf.Clamp(m_velocity, MinSpeed, MaxSpeed);
        Velocity = m_velocity;

        if (Velocity > 0)
        {
            if(!m_audio.isPlaying) m_audio.Play();
        }
        else m_audio.Stop();

        transform.position += transform.forward * m_velocity;

        // DEBUG 
        GameObject.Find("TargetStatus").GetComponent<Text>().text =
            m_target == -1 ? "No Target" : m_targets[m_target].gameObject.name +
                " (" + (m_targets[m_target].transform.position - transform.position).magnitude + ")";

		if (GetComponent<Target>().Health < 0)
		{
			GameObject explosion = (GameObject)Instantiate(m_explosionPrefab);
			explosion.GetComponent<Transform>().position = transform.position;
			UnregisterTarget(this.gameObject.transform);
			Time.timeScale = 0;
			HUD.gameObject.AddComponent<GameOver>();
			Destroy(gameObject, 0);
		}
	}

    public void RegisterTarget(Transform transform)
    {
        m_targets.Add(transform);
        GameObject indicator = (GameObject)Instantiate(m_indicator);
        indicator.GetComponent<TargetIndicator>().SetTarget(transform);

    }

    public void UnregisterTarget(Transform transform)
    {
        m_targets.Remove(transform);
        Destroy(transform.gameObject, 0);
    }

    void CycleTargets()
    {
        if (m_targets.Count > 0)
        {
            if (m_target == -1)
            {
                m_target = 0;
            }
            else
            {
                m_target++;
                if (m_target > m_targets.Count - 1)
                    m_target = 0;
            }
        }
    }

    void UnlockTarget()
    {
        m_target = -1;
        m_reticle.position = transform.position + transform.forward;
    }

    void FireMissile()
    {
        GameObject missile = (GameObject)Instantiate(m_missile);
        missile.transform.rotation = transform.rotation;
        missile.transform.position = transform.position - 
            transform.up * MissileOffset;
        missile.GetComponent<Missile>().SetTarget(m_targets[m_target]);
        missile.GetComponent<Missile>().SetVelocity(m_velocity);
    }

}
