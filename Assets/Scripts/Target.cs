using UnityEngine;
using System.Collections;
public enum TargetType
{
    Friendly,
    Neutral,
    Enemy
}
public class Target : MonoBehaviour 
{
    public string Label = "unknown";
    public float Health = 100f;
    public TargetType TargetType;

	void Start () 
    {
	
	}
	
	void Update () 
    {
	
	}
}
