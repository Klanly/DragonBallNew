using UnityEngine;
using System.Collections;

public class DisableObj : MonoBehaviour {

	public float DisableTime = 1f;
	// Use this for initialization
	void Start () {
		InvokeRepeating("LaterDisable", DisableTime, DisableTime);
	}

	void LaterDisable () {
		if(gameObject.activeSelf)
			gameObject.SetActive(false);
	}
}
