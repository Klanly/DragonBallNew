using UnityEngine;
using System.Collections;

public class BtnWheelCtrller : MonoBehaviour 
{
	public UISprite m_spBtnIcon;
	public UISprite m_spBtnName;
	public UILabel lbl_name;

	private readonly string[] ACT_SPRITE = { "mingyunzhuanpan", "xingyunzhuanlun", "yaoyaole", "caicaikan", "Act_11"};
	private readonly string[] ACT_NAME = { "mingyunzhuanpan", "xingyunzhuanlun", "yaoyaole", "caicaikan", "leidazudui"};

	private int m_nActState = 0;
	public int actState
	{
		get
		{
			return m_nActState;
		}
	}

	void Start()
	{
		if (Core.Data.playerManager.RTData.curLevel < 10)
		{
			//RED.SetActive (false, this.gameObject);
			m_spBtnIcon.color = new Color (0,0,0,1f);
			return;
		}

		InitUI ();
	}

	//更新状态
	public void UpdateState(int state)
	{
		m_nActState = state;
		if (m_nActState >= 0 && m_nActState <= 4)
		{
			if (m_spBtnIcon != null) {
				m_spBtnIcon.spriteName = ACT_SPRITE [m_nActState];
				m_spBtnIcon.MakePixelPerfect ();
			}
			if (m_spBtnName != null) {
				m_spBtnName.spriteName = ACT_NAME [m_nActState];
				m_spBtnName.MakePixelPerfect ();
			}
			if (lbl_name != null)
				UpdateName (m_nActState);
		}
	}


	public void UpdateName(int state){
		string tName = "";
		switch (state) {
		case 0:
			tName = Core.Data.stringManager.getString (7395);
			break;
		case 1:
			tName = Core.Data.stringManager.getString (7396);
			break;
		case 2:
			tName = Core.Data.stringManager.getString (7398);
			break;
		case 3:
			tName = Core.Data.stringManager.getString (7397);
			break;
		case 4:
			tName = Core.Data.stringManager.getString (7399);
			break;
		default:
			tName = Core.Data.stringManager.getString (7395);
			break;
		}
		lbl_name.text = tName;
	}
	public void InitUI()
	{
		//向服务器发送消息，获取当前状态
		if (ActivityManager.hasUpdateValue == false)
			RouletteLogic.sendSer (HttpRequst, HttpRequstError, 0);
		else {
			UpdateState (ActivityManager.activityZPID);
		}


	}

		
	void HttpRequst(BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR)
		{
			GetAwardActivity resp = response as GetAwardActivity;
			if (resp != null && resp.data != null)
			{
				UpdateState (resp.data.status.id);
			}
		}
	}

	void HttpRequstError(BaseHttpRequest request, string strError)
	{
		SQYAlertViewMove.CreateAlertViewMove (strError);
	}

}
