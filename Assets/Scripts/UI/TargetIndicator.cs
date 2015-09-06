using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TargetIndicator : MonoBehaviour {
    
    // public float IndicatorOffset = 0.05f;
    public int ScreenBorderSize = 20; 
    public Material NeutralMaterial;
    public Material FriendlyMaterial;
    public Material EnemyMaterial;
    private Transform m_player;
    private Transform m_target;
    private Target m_targetScript;
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
        m_targetScript = m_target.GetComponent<Target>();
        if (m_targetScript.TargetType == TargetType.Neutral)
        {
            GetComponentInChildren<MeshRenderer>().material = NeutralMaterial;          
        }
        if (m_targetScript.TargetType == TargetType.Friendly)
        {
            GetComponentInChildren<MeshRenderer>().material = FriendlyMaterial;
        } 
        if (m_targetScript.TargetType == TargetType.Enemy)
        {
            GetComponentInChildren<MeshRenderer>().material = EnemyMaterial;
        }
        m_targetInfo.GetComponent<Text>().text = m_target.GetComponent<Target>().Label;
    }

	void Update ()
    {
        // If the target assigned to this target indicator has been destroyed, 
        // destroy the target indicator and target information objects too.
        if (m_target == null)
        {
            Destroy(m_targetInfo.gameObject, 0);
            Destroy(gameObject, 0);
            return;
        }

        // Determine if the target is in front of, or behind, the player.
        bool targetInFront = false;
        Vector3 targetDir = (m_target.position - m_player.position).normalized;
        float targetDist = (m_target.position - m_player.position).magnitude;
        float dot = Vector3.Dot(targetDir, transform.forward);
        if (dot > 0)
        {
            targetInFront = true;
        }

        // Determine if the target is on the screen.
        bool targetOnScreen = false;
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(m_target.position);
        if (targetInFront &&
            screenPoint.x > ScreenBorderSize &&
            screenPoint.y > ScreenBorderSize &&
            screenPoint.x < Screen.width - ScreenBorderSize && 
            screenPoint.y < Screen.height - ScreenBorderSize)
        {
            targetOnScreen = true;
        }

        if(targetOnScreen)
        {
            // Update indicator position.
            transform.position = m_player.position + targetDir;
            // Update and reposition target information.
            m_targetInfo.position = Camera.main.WorldToScreenPoint(transform.position);
            m_targetInfo.GetComponent<Text>().text = m_targetScript.Label +
                "\n" + targetDist.ToString("n2") + "\n" + m_targetScript.Health;
        }
        else
        {
            // Position at edge of screen in the direction of the target.
            // Cast ray from target onto sphere collider at players 0, 0, 1.
            RaycastHit sphereHit;
            Physics.Raycast(m_target.position, (GameObject.Find("SphereCollider").transform.position -
                    m_target.position).normalized, out sphereHit, float.MaxValue, m_sphereColliderMask);
            if (sphereHit.collider != null)
            {
                // Cast ray from target from sphere collider hit point to plane
                // collider at players 0, 0, 1.

                RaycastHit boxHit;
                Physics.Raycast(sphereHit.point, transform.parent.forward,
                    out boxHit, float.MaxValue, m_boxColliderMask);
                if (boxHit.collider != null)
                {

                    Vector3 position = m_player.position + (boxHit.point - m_player.position).normalized;
                    Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);
                    Vector3 screenOrigin = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                    Vector3 screenEdgePosition = (screenPosition - screenOrigin) * 10f;
                    screenEdgePosition = new Vector3(
                        Mathf.Clamp(screenEdgePosition.x, ScreenBorderSize, Screen.width - ScreenBorderSize),
                        Mathf.Clamp(screenEdgePosition.y, ScreenBorderSize, Screen.height - ScreenBorderSize), 1);
                    transform.position = Camera.main.ScreenToWorldPoint(screenEdgePosition);

                    m_targetInfo.transform.position = Camera.main.WorldToScreenPoint(transform.position);
                    m_targetInfo.GetComponent<Text>().text = m_target.GetComponent<Target>().Label + "\n" + (m_target.transform.position -
                       transform.parent.position).magnitude.ToString("n2") + "\n" + m_target.GetComponent<Target>().Health;
                }
                else
                {
                    Physics.Raycast(sphereHit.point, -transform.parent.forward, out boxHit, float.MaxValue, m_boxColliderMask);
                    if (boxHit.collider != null)
                    {
                        Vector3 position = m_player.position + (boxHit.point - m_player.position).normalized;
                        Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);
                        Vector3 screenOrigin = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                        Vector3 screenEdgePosition = (screenPosition - screenOrigin) * 10f;
                        screenEdgePosition = new Vector3(
                            Mathf.Clamp(screenEdgePosition.x, ScreenBorderSize, Screen.width - ScreenBorderSize),
                            Mathf.Clamp(screenEdgePosition.y, ScreenBorderSize, Screen.height - ScreenBorderSize), 1);
                        transform.position = Camera.main.ScreenToWorldPoint(screenEdgePosition);

                        m_targetInfo.transform.position = Camera.main.WorldToScreenPoint(transform.position);
                        m_targetInfo.GetComponent<Text>().text = m_target.GetComponent<Target>().Label + "\n" + (m_target.transform.position -
                            transform.parent.position).magnitude.ToString("n2") + "\n" + m_target.GetComponent<Target>().Health;
                    }
                }
            }
        }
	}
}
