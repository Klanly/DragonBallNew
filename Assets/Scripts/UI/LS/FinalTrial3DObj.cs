using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FinalTrial3DObj : MonoBehaviour
{
	[HideInInspector]
	public float mAngle = 30.0f;
	MiniItween.DelegateFuncWithObject _mm = null;

	public void MyRotate(MiniItween.DelegateFuncWithObject mm)
	{
		_mm = mm;
		StartCoroutine(BeginToRotate());
	}

	IEnumerator BeginToRotate()
	{
		MiniItween mm = MiniItween.RotateTo(this.gameObject, new Vector3(gameObject.transform.localRotation.x-60.0f,0.0f,0.0f), 2.0f);
		mm.myDelegateFuncWithObject += _mm;
		yield return null;
	}
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
