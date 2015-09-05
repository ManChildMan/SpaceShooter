using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {
    public float Acceleration = 5f;
    public float MinVelocity = 0f;
    public float MaxVelocity = 50f;
    public float StartVelocity = 0f;
    public float MaxLifetime = 10;
    public float TurnConstraint = 2f;

    private float m_velocity = 0f;
    private float m_lifetime = 0f;
    private Transform m_target = null;
    private UnityEngine.Object m_explosionPrefab;

    void Start()
    {
        m_explosionPrefab = Resources.Load("Explosion");
    }
    
    void Update()
    {
        m_lifetime += Time.deltaTime;
        if (m_lifetime > MaxLifetime)
        {
            Destroy(gameObject, 0);
        }

        // Find the vector pointing from missile position to target position. 
        Vector3 direction = Vector3.forward;
        if (m_target) direction = (m_target.transform.position -
            transform.position).normalized;
       
        // Rotate missile part of the way towards the target.
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation,
            rotation, Time.deltaTime * TurnConstraint);
       
        // 
        m_velocity += Acceleration * Time.deltaTime;
        m_velocity = Mathf.Clamp(m_velocity, MinVelocity, MaxVelocity);
        transform.position += transform.forward * m_velocity;
    }

    public void SetTarget(Transform transform)
    {
        m_target = transform;
    }

    public void SetVelocity(float velocity)
    {
        m_velocity = velocity;
    }

    void OnTriggerEnter(Collider collider)
    {
        // Ignore triggers fired by situation colliders.
        if (!collider.CompareTag("Situation") && !collider.CompareTag("Player"))
        {
            // Instantiate an explosion prefab and destroy the missile.
            GameObject explosion = (GameObject)Instantiate(m_explosionPrefab);
            explosion.GetComponent<Transform>().position = gameObject.transform.position;
            Destroy(gameObject, 0);

            if(collider.CompareTag("SpaceStation"))
            {
                GameObject.Find("spacestation_01").GetComponent<Target>().Health -= 50;
            }
            if (collider.CompareTag("Enemy"))
            {
                collider.transform.GetComponent<Target>().Health = -1;
            }

        }
    }
}
