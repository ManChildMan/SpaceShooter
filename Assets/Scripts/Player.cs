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
    private Transform m_reticle;
    private Transform m_launchAuthorised;
    private MeshRenderer m_reticleRenderer;
    private Color m_reticleDefaultColor;

    void Awake()
    {
        m_missile = Resources.Load("Missile");
        m_indicator = Resources.Load("TargetIndicator");
    }

	void Start () 
    {


        m_launchAuthorised = GameObject.Find("LaunchAuthorised").transform;
        m_launchAuthorised.gameObject.SetActive(false);

        m_reticle = GameObject.Find("TargetReticle").transform;
        m_reticle.gameObject.SetActive(false);
        m_reticleRenderer = m_reticle.GetComponent<MeshRenderer>();
        m_reticleDefaultColor = m_reticleRenderer.material.color;

	}
		



	void Update () 
    {
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
            }
            else m_reticle.gameObject.SetActive(false);
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

        if (Input.GetButton("Fire1") && m_missileLastFired > MissileCooldown && m_isLaunchAuthorised)
        {
            FireMissile();
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

        transform.position += transform.forward * m_velocity;

        // DEBUG 
        GameObject.Find("TargetValue").GetComponent<Text>().text =
            m_target == -1 ? "None" : m_targets[m_target].gameObject.name +
                " (" + (m_targets[m_target].transform.position - transform.position).magnitude + ")";
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
