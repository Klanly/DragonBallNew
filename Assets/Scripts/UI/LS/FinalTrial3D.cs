using UnityEngine;
using System.Collections;

public class FinalTrial3D : MonoBehaviour
{
	public FinalTrial3DObj _FinalTrial3DObj;

	void Start()
	{
		if(_FinalTrial3DObj == null)_FinalTrial3DObj = gameObject.GetComponentInChildren<FinalTrial3DObj>();
	}

	public void SetRotate(MiniItween.DelegateFuncWithObject mm)
	{
		if(_FinalTrial3DObj == null)_FinalTrial3DObj = gameObject.GetComponentInChildren<FinalTrial3DObj>();
		if(_FinalTrial3DObj != null)
		{
			_FinalTrial3DObj.MyRotate(mm);
		}
	}

	public void OnShow()
	{

	}

	void OnDestroy()
	{
		_FinalTrial3DObj = null;
	}

}
