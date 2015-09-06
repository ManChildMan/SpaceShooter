using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Velocity : MonoBehaviour {
    public Slider VelocitySlider;
    private Player m_player;

    void Start()
    {
        m_player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        VelocitySlider.value = m_player.Velocity;
    }
}
