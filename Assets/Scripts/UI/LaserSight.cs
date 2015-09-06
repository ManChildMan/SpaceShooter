using UnityEngine;
using System.Collections;

public class LaserSight : MonoBehaviour
{
    public float LaserRange = 1000;
    public LayerMask LayerMask;

    private MeshRenderer m_renderer;
    private Color m_startColor;

	void Start() 
    { 
        m_renderer = GameObject.Find("LaserSight").GetComponent<MeshRenderer>();
        m_startColor = m_renderer.material.color;
	}

	void Update() 
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, LaserRange, LayerMask))
        {
            m_renderer.material.color = Color.red;
        }
        else m_renderer.material.color = m_startColor;

	}
}
