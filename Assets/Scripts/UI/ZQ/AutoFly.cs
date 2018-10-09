using UnityEngine;
using System.Collections;

public class AutoFly : MonoBehaviour {

	public Transform target;
	public float time;
	public float angle;
	public int direction = 1;

	// Use this for initialization
	void Start () 
	{
		if(time > 0)
			InvokeRepeating ("SwitchDirection", 0.0f, time);
	}

	void SwitchDirection()
	{
		direction *= -1;
	}

	// Update is called once per frame
	void Update () 
	{
		this.transform.RotateAround (target.position, Vector3.up, Time.deltaTime * angle * direction);
	}
}
