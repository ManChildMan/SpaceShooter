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
    public float VelocityModifier = 5;

    public float MissileCooldown = 1f;
    public float MissileOffset = 0.75f;




    private float m_velocity = 0f;

    private float m_missileLastFired = 0f;
    private bool m_missileMode = false;
    private bool m_launchAuthorised = false;
    private List<Transform> m_targets = new List<Transform>();
    private List<Transform> m_targetIndicators = new List<Transform>();
    private int m_target = -1;
    private UnityEngine.Object m_laserBeamPrefab;
    private UnityEngine.Object m_missilePrefab;
    private UnityEngine.Object m_targetIndicatorPrefab;
    private Transform m_targetReticle;
    private Transform m_launchAuthorisedText;

    void Awake()
    {
        m_laserBeamPrefab = Resources.Load("LaserBeam");
        m_missilePrefab = Resources.Load("Missile");
        m_targetIndicatorPrefab = Resources.Load("TargetIndicator");
    }

	void Start () 
    {
        m_targetReticle = GameObject.Find("TargetReticle").transform;
        m_targetReticle.gameObject.SetActive(false);
        m_launchAuthorisedText = GameObject.Find("LaunchAuthorised").transform;
        m_launchAuthorisedText.gameObject.SetActive(false);
        m_targetReticleRenderer = m_targetReticle.GetComponent<MeshRenderer>();
        m_targetReticleOldColor = m_targetReticleRenderer.material.color;
	}
		

    private MeshRenderer m_targetReticleRenderer;
    private Color m_targetReticleOldColor;

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
                m_targetReticle.gameObject.SetActive(true);
            }
            else m_targetReticle.gameObject.SetActive(false);
        }

        if (m_missileMode)
        {
            if (m_target > -1)
            {
                Vector3 displacement = (m_targets[m_target].position -
                    transform.position).normalized;
                m_targetReticle.transform.position = transform.position +
                    displacement;

                float angle = Vector3.Angle(transform.forward, displacement);
                if (angle < 1)
                {
                    m_targetReticleRenderer.material.color = Color.red;
                    m_launchAuthorised = true;
                    m_launchAuthorisedText.gameObject.SetActive(true);
                }
                else
                {
                    m_targetReticleRenderer.material.color = m_targetReticleOldColor;
                    m_launchAuthorised = false;
                    m_launchAuthorisedText.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            m_launchAuthorised = false;
            m_launchAuthorisedText.gameObject.SetActive(false);
        }

        if (Input.GetButton("Fire1") && m_missileLastFired > MissileCooldown && m_launchAuthorised)
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

        transform.position += transform.forward * m_velocity;

        // DEBUG 
        GameObject.Find("PositionValue").GetComponent<Text>().text =
            transform.position.ToString();
        GameObject.Find("VelocityValue").GetComponent<Text>().text = 
            m_velocity.ToString() + " units per second";
        GameObject.Find("TargetValue").GetComponent<Text>().text =
            m_target == -1 ? "None" : m_targets[m_target].gameObject.name +
                " (" + (m_targets[m_target].transform.position - transform.position).magnitude + ")";
	}   

    public void RegisterTarget(Transform transform)
    {
        m_targets.Add(transform);
        GameObject targetIndicator = (GameObject)Instantiate(m_targetIndicatorPrefab);
        targetIndicator.GetComponent<TargetIndicator>().SetTarget(transform);
    }

    public void UnregisterTarget(Transform transform)
    {
        m_targets.Add(transform);
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
        m_targetReticle.position = transform.position + transform.forward;
    }

    void FireMissile()
    {
        GameObject missile = (GameObject)Instantiate(m_missilePrefab);
        missile.transform.rotation = transform.rotation;
        missile.transform.position = transform.position - 
            transform.up * MissileOffset;
        missile.GetComponent<Missile>().SetTarget(m_targets[m_target]);
        missile.GetComponent<Missile>().SetVelocity(m_velocity);
    }

}
