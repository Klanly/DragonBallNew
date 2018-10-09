using UnityEngine;
using System.Collections;

public class TrialTitleCell : MonoBehaviour
{

	public UISprite mSprite;

	void OnClick()
	{
		FinalTrialMgr.GetInstance ().CreateScript (TrialEnum.TrialType_PuWuChoose, QiangduoEnum.QiangduoEnum_None);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
