using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LaserTemp : MonoBehaviour {
    public Slider LaserTempSlider;
    public Image LaserTempFillImage;
    public Sprite StatusBarGreen;
    public Sprite StatusBarYellow;
    public Sprite StatusBarRed;
    public Laser Laser;
    private Player m_player;

	void Start ()
    {
        m_player = GameObject.Find("Player").GetComponent<Player>();   
	}
	
	void Update ()
    {
        float laserTemp = Laser.LaserTemperature;
        if (laserTemp < 33)
        {
            LaserTempFillImage.sprite = StatusBarGreen;
        }
        else if (laserTemp < 66)
        {
            LaserTempFillImage.sprite = StatusBarYellow;
        }
        else LaserTempFillImage.sprite = StatusBarRed;
        LaserTempSlider.value = laserTemp;     
	}
}
