using UnityEngine;
using System.Collections;

public class MissionSystemCell : RUIMonoBehaviour {

	public UILabel _MissionName;
	public UISprite _IsComplete;
	public UISprite _IsChoose;

	void Start () 
	{
		_IsChoose.gameObject.SetActive(false);
	}

	void Press_OnClick()
	{
		SetChoose();
		MissionSystem.GetInstance()._MissionSystemMain._MissionSystemCell.SetHide();
		MissionSystem.GetInstance()._MissionSystemMain._MissionSystemCell = this;
	}

	public void SetChoose()
	{
		_IsChoose.gameObject.SetActive(true);
	}

	public void SetHide()
	{
		_IsChoose.gameObject.SetActive(false);
	}

	public void SetDetail()
	{

	}

	void OnDestroy()
	{
		_MissionName = null;
		_IsComplete = null;
		_IsChoose = null;
	}

}
