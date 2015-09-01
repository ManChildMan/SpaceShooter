using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpacecraftController : MonoBehaviour {
    public float MinSpeed = 0;
    public float MaxSpeed = 0.5f;
    public float PitchModifier = 30;
    public float RollModifier = 30;
    public float YawModifier = 30;
    public float VelocityModifier = 5;
    public float LaserCooldown = 1f;
    public float MissileCooldown = 1f;

    private float m_velocity = 0f;
    private float m_laserLastFired = 0f;
    private float m_missileLastFired = 0f;
    private bool m_missileMode = false;
    private List<Transform> m_targets = new List<Transform>();
    private int m_target = -1;
    private bool m_targetLock = false;
    private UnityEngine.Object m_laserBeamPrefab;
    private UnityEngine.Object m_missilePrefab;
    private GameObject m_reticle;

    void Awake()
    {
    }

	void Start () {
        m_laserBeamPrefab = Resources.Load("LaserBeam");
        m_missilePrefab = Resources.Load("Missile");
        m_reticle = GameObject.Find("TargetReticle");
	}
		
	void Update () {
        if (Input.GetButtonDown("ToggleMissileMode"))
        {
            m_missileMode = !m_missileMode;
            if (m_missileMode == true)
            {
                m_reticle.SetActive(true);
            }
            else m_reticle.SetActive(false);
        }
        if (Input.GetButtonDown("CycleTargets"))
        {
            CycleTargets();
        }
        if (Input.GetButtonDown("UnlockTarget"))
        {
            UnlockTarget();
        }

        m_laserLastFired += Time.deltaTime;
        m_missileLastFired += Time.deltaTime;
        if (Input.GetButton("Fire") && m_laserLastFired > LaserCooldown)
        {
            FireLaser();
            m_laserLastFired = 0;
        }

        if (Input.GetButton("Fire1") && m_missileLastFired > MissileCooldown && m_target != -1)
        {
            FireMissile();
            m_missileLastFired = 0;
        }

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
    }

    void FireLaser()
    {
        Vector3 offset1 = new Vector3(-1f, 0, 0);
        Vector3 offset2 = new Vector3(1f, 0, 0);
        GameObject projectile = (GameObject)Instantiate(m_laserBeamPrefab);
        projectile.GetComponent<Transform>().rotation =
            transform.rotation;
        projectile.GetComponent<Transform>().position =
            transform.position + offset1;
        GameObject projectile2 = (GameObject)Instantiate(m_laserBeamPrefab);
        projectile2.GetComponent<Transform>().rotation =
            transform.rotation;
        projectile2.GetComponent<Transform>().position =
            transform.position + offset2;
    }

    void FireMissile()
    {
        Vector3 offset1 = new Vector3(0, 0, 0);
        GameObject projectile = (GameObject)Instantiate(m_missilePrefab);
        //projectile.GetComponent<Transform>().rotation =
        //    transform.rotation;
        projectile.GetComponent<Transform>().position =
            transform.position + offset1;
        projectile.GetComponent<Missile>().SetTarget(m_targets[m_target]);
    }
}
