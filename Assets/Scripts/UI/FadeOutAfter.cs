using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeOutAfter : MonoBehaviour {
    public float Interval = 10f;
    public float FadeSpeed = 1f;
    private Text m_text;
    private float m_time;
	void Start () 
    {
        m_text = GetComponent<Text>();
	}
	void Update ()
    {
        m_time += Time.deltaTime;
        if (m_time > Interval)
        {
            m_text.color = Color.Lerp(m_text.color, Color.clear, 
                FadeSpeed * Time.deltaTime);
        }
	}
}
