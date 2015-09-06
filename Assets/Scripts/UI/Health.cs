using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Health : MonoBehaviour {
    public Slider HealthSlider;
    private Target m_target;

    void Start()
    {
        m_target = GameObject.Find("Player").GetComponent<Target>();
    }

    void Update()
    {
        HealthSlider.value = m_target.Health;
    }

}
