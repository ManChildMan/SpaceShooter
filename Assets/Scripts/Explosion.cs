using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour 
{
        AudioSource m_audio;
    public AudioClip a;
    public AudioClip b;
  
	// Use this for initialization
	void Start () {
        Destroy(gameObject, 3); 
        m_audio = GetComponent<AudioSource>();
        m_audio.PlayOneShot(m_audio.clip);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
