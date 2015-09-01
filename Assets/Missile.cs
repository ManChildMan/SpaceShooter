using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {

    public float Lifetime = 10;
    public float Velocity = 10f;
    public float AimSpeed = 2f;


    private float m_timeSinceLaunch = 0f;

    private Transform m_target = null;

   
    void Start()
    {
        
    }

    
    void Update()
    {

        if (m_timeSinceLaunch < Lifetime)
        {
            m_timeSinceLaunch += Time.deltaTime;
            // Point missile at player.
            //find the vector pointing from our position to the target

            Vector3 direction = Vector3.forward;
            if (m_target)
            {
                direction = (m_target.transform.position - transform.position).normalized;
            }

     
            Quaternion rotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * AimSpeed);


        }
        else
        {
            Destroy(gameObject, 0);
        }

        transform.position += transform.forward * Velocity * Time.deltaTime;
    }

    public void SetTarget(Transform transform)
    {
        m_target = transform;
    }

}
