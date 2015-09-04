using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TargetIndicator : MonoBehaviour {
    
    // public float IndicatorOffset = 0.05f;
    public float clampBorderSize = 0.05f; 
    public Material NeutralMaterial;
    public Material FriendlyMaterial;
    public Material EnemyMaterial;
    private Transform m_player;
    private Transform m_target;
    private int m_sphereColliderMask;
    private int m_boxColliderMask;

    private Transform m_targetInfo; 
    private UnityEngine.Object m_targetInfoPrefab;
	void Awake ()
    {

        m_targetInfoPrefab = Resources.Load("TargetInformation");

        transform.parent = GameObject.Find("Situation").transform;
        m_player = GameObject.Find("Player").transform;

        m_sphereColliderMask = 1 << LayerMask.NameToLayer("SituationSphereCollider");
        m_boxColliderMask = 1 << LayerMask.NameToLayer("SituationBoxCollider");


        GameObject targetInfo = (GameObject)Instantiate(m_targetInfoPrefab);
        targetInfo.transform.SetParent(GameObject.Find("HUD").transform);
        m_targetInfo = targetInfo.transform;
 
	}

    public void SetTarget(Transform transform)
    {      
        m_target = transform;
        if (m_target.CompareTag("Neutral"))
        {
            GetComponentInChildren<MeshRenderer>().material = NeutralMaterial;          
        }
        if (m_target.CompareTag("Friendly"))
        {
            GetComponentInChildren<MeshRenderer>().material = FriendlyMaterial;
        } 
        if (m_target.CompareTag("Enemy"))
        {
            GetComponentInChildren<MeshRenderer>().material = EnemyMaterial;
        }
        m_targetInfo.GetComponent<Text>().text = m_target.GetComponent<Target>().Label;
    }

	void Update ()
    {
        if (m_target == null) return;

        bool targetInFront = false;
        Vector3 direction = (m_target.position - transform.position).normalized;
        float dot = Vector3.Dot(direction, transform.forward);
        if (dot > 0)
        {
            targetInFront = true;
        }

        if (targetInFront)
        {

        }

        // Get angle from forward vector to target.
        float angle = Vector3.Angle(m_player.forward, m_target.position );


        Vector3 screenPoint = Camera.main.WorldToScreenPoint(m_target.position);
        if (targetInFront && screenPoint.x > 0 + 20 && screenPoint.y > 0 + 20 &&
            screenPoint.x < Screen.width - 20 && screenPoint.y < Screen.height -20)
        {
            Vector3 displacement = (m_target.transform.position -
                transform.parent.position).normalized;
            transform.position = transform.parent.position + displacement;
            m_targetInfo.transform.position = Camera.main.WorldToScreenPoint(transform.position);
            m_targetInfo.GetComponent<Text>().text = m_target.GetComponent<Target>().Label + "\n" + (m_target.transform.position -
               transform.parent.position).magnitude.ToString("n2");

        }
        else
        {
            // Cast ray from target onto sphere collider at players 0, 0, 1.
            RaycastHit sphereHit;
            Physics.Raycast(m_target.position, (GameObject.Find("SphereCollider").transform.position -
                    m_target.position).normalized, out sphereHit, float.MaxValue, m_sphereColliderMask);
            if (sphereHit.collider != null)
            {
                // Cast ray from target onto sphere collider hit point to plane
                // collider at players 0, 0, 1.
                RaycastHit boxHit;
                Physics.Raycast(sphereHit.point, targetInFront ? transform.parent.forward : -transform.parent.forward,
                    out boxHit, float.MaxValue, m_boxColliderMask);
                if (boxHit.collider != null)
                {
                    //Vector3 dd = (boxHit.point - transform.position).normalized;
                    //transform.position = boxHit.point;
                    Vector3 displacement = (boxHit.point -
                        transform.parent.position).normalized * 2;


                    //float a = Vector3.Angle(transform.parent.right, displacement) * Mathf.Deg2Rad;
                    //double x = 0.5f * Math.Cos(a) + transform.parent.position.x;
                    //double y = 0.5f * Math.Sin(a) + transform.parent.position.y;
                    //displacement = new Vector3((float)x, (float)y, displacement.z);

                    transform.position = transform.parent.position + displacement;
                    m_targetInfo.transform.position = Camera.main.WorldToScreenPoint(transform.position);
                    m_targetInfo.GetComponent<Text>().text = m_target.GetComponent<Target>().Label + "\n" + (m_target.transform.position -
                       transform.parent.position).magnitude.ToString("n2");
                }
                else
                {
                           Physics.Raycast(sphereHit.point, targetInFront ? -transform.parent.forward : transform.parent.forward,
                    out boxHit, float.MaxValue, m_boxColliderMask);
                           if (boxHit.collider != null)
                           {
                               Vector3 displacement = (boxHit.point -
                                   transform.parent.position).normalized * 2;
                               //float a = Vector3.Angle(transform.parent.right, displacement) * Mathf.Deg2Rad;
                               //double x = 1 * Math.Cos(a) + transform.parent.position.x;
                               //double y = 1 * Math.Sin(a) + transform.parent.position.y;
                               //displacement = new Vector3((float)x, (float)y, displacement.z);

                               transform.position = transform.parent.position + displacement;
                               m_targetInfo.transform.position = Camera.main.WorldToScreenPoint(transform.position);
                               m_targetInfo.GetComponent<Text>().text = m_target.GetComponent<Target>().Label + "\n" + (m_target.transform.position -
                                  transform.parent.position).magnitude.ToString("n2");
                           }
                }
            }
        }


	}
}





//// if out of view indicator mapped to circle

//// Get angle from forward vector to target.
//float angle = Vector3.Angle(m_player.forward, m_target.position -  transform.parent.position);

//// If angle outside hud

//if (angle < -30 || angle > 30)
//{



//    // Is target in front of situation box collider origin or behind?
//    bool inFront = false;
//    bool onSide = false;
//    Vector3 direction = (m_target.position - transform.position).normalized;
//    float dot = Vector3.Dot(direction, transform.forward);
//    if(dot < 0.1)
//    {
//        inFront = false;
//    }
//    else if(dot > 0.1)
//    {
//        inFront = true;
//    }
//    else 
//    {
//        onSide = true;
//    }


//    // Cast ray from target onto sphere collider at players 0, 0, 1.
//    RaycastHit sphereHit;
//    Physics.Raycast(m_target.position, 
//        GameObject.Find("SphereCollider").transform.position -
//            m_target.transform.position, out sphereHit, float.MaxValue, m_sphereColliderMask);
//    if (sphereHit.collider != null)
//    {
//        if(onSide)
//        {
//            Vector3 dd = (sphereHit.point - transform.position).normalized;
//            transform.position = sphereHit.point;
//        }
//        else
//        {
//            // Cast ray from target onto sphere collider hit point to plane
//            // collider at players 0, 0, 1.
//            RaycastHit boxHit;
//            Physics.Raycast(sphereHit.point, inFront ? transform.forward : -transform.forward, 
//                out boxHit, float.MaxValue, m_boxColliderMask);
//            if (boxHit.collider != null)
//            {
//                //Vector3 dd = (boxHit.point - transform.position).normalized;
//                //transform.position = boxHit.point;
//                Vector3 displacement = (boxHit.point -
//                    transform.parent.position).normalized;
//                transform.position = transform.parent.position + displacement;
//            }
//        }
//    }

//    // flatten point on unit sphere

//    // scale point onto unit sphere

//    // update position
//}
