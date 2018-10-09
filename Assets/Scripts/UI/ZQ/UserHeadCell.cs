using UnityEngine;
using System.Collections;

public class UserHeadCell : MonoBehaviour 
{
	public UISprite m_spHead;
	public UISprite m_objSel;
	private UserHeadData m_data;

	public UISprite VipIcon;
	public UISprite Vipbg;
	public UISprite SprLock;
	//public UISprite IsLockbg;
	public UILabel mNum;
	
	public UISprite cellback;
	//private int _mostCombo;
	
	public UserHeadData Headdata
	{
		get 
		{
			return m_data;
		}
	}

	[HideInInspector]
	public bool IsChoose;

	public void InitUI(UserHeadData data)
	{
		if(data == null)
		{
			RED.LogError("data is null" );
		}
		m_data = data;
		AtlasMgr.mInstance.SetHeadSprite(m_spHead, data.id.ToString());
		
		switch((PlayerHeadType)data.type)
		{
		case PlayerHeadType.General:
			Vipbg.spriteName = "main-1005";
			IsChoose = true;
			mNum.gameObject.SetActive(false);
			VipIcon.gameObject.SetActive(false);
			SetCellState(false);
//			IsLock.gameObject.SetActive(false);
//			IsLockbg.gameObject.SetActive(false);
			break;
		case PlayerHeadType.Vip:
			VipIcon.gameObject.SetActive(true);
			mNum.gameObject.SetActive(true);
			mNum.text = data.VIPLv.ToString();
			if(Core.Data.playerManager.curVipLv < data.VIPLv)
			{
				IsChoose = false;
//				IsLock.gameObject.SetActive(true);
//				IsLockbg.gameObject.SetActive(true); 
				SetCellState(true);
			}
			else
			{
				IsChoose = true;
//				IsLock.gameObject.SetActive(false);
//				IsLockbg.gameObject.SetActive(false);
				SetCellState(false);
			}

			SetVipDetail(data);
			break;
		case PlayerHeadType.Combo:
			Vipbg.spriteName = "main-1005";
			VipIcon.gameObject.SetActive (true);
			VipIcon.spriteName = "common-2021";
			VipIcon.MakePixelPerfect ();
			mNum.gameObject.SetActive (true);
			mNum.text = data.Batter.ToString ();
			//		Debug.Log ( "    total combo   ======   "+ Core.Data.playerManager.RTData.TotalCombo +  "        total  combo     ===  "  + Core.Data.temper.TotalCombo +"    data  batter  === " + data.Batter);

			if(Core.Data.playerManager.RTData.TotalCombo >= data.Batter)
			{
				IsChoose = true;
//				IsLock.gameObject.SetActive(false);
//				IsLockbg.gameObject.SetActive(false);
				SetCellState(false);
			}
			else
			{
				IsChoose = false;
//				IsLock.gameObject.SetActive(true);
//				IsLockbg.gameObject.SetActive(true);
				SetCellState(true);
			}
            break;
		case PlayerHeadType.Reward:
			Vipbg.spriteName = "main-1005";
			VipIcon.gameObject.SetActive (true);
			VipIcon.spriteName = "caizhong";
			VipIcon.MakePixelPerfect ();
			mNum.gameObject.SetActive (true);
			mNum.text = data.Gamble.ToString ();
			Debug.Log (    " TotalGambleWin ==  " + Core.Data.playerManager.RTData.TotalGambleWin);
            
			if(Core.Data.playerManager.RTData.TotalGambleWin > data.Gamble)
			{
				IsChoose = true;
//				IsLock.gameObject.SetActive(false);
//				IsLockbg.gameObject.SetActive(false);
				SetCellState(false);
			}
			else
			{
				IsChoose = false;
//				IsLock.gameObject.SetActive(true);
//				IsLockbg.gameObject.SetActive(true);
				SetCellState(true);
			}
			break;
		}
		//RED.SetActive (false, m_objSel);
	     //m_objSel.enabled = false;
		m_objSel.alpha = 0;
	}

	void SetVipDetail(UserHeadData data)
	{	
		if(data.VIPLv < 4)
		{
			Vipbg.spriteName = "starvip1";
			VipIcon.spriteName = "common-2008";
		}
		else if(data.VIPLv < 8)
		{
			Vipbg.spriteName = "starvip2";
			VipIcon.spriteName = "common-2009";
		}
		else if(data.VIPLv < 12)
		{
			Vipbg.spriteName = "starvip3";
			VipIcon.spriteName = "common-2007";
		}else if( data.VIPLv < 16){
			Vipbg.spriteName = "star0";
			VipIcon.spriteName = "common-2109";
		}
		VipIcon.MakePixelPerfect();
	}

	void OnClick()
	{
		if (UIOptionController.Instance.m_selHeadUI.m_selHead != null) 
		{
			UIOptionController.Instance.m_selHeadUI.m_selHead.SetSelected (false);
		}

		UIOptionController.Instance.m_selHeadUI.m_selHead = this;
		SetSelected (true);
	}

	void SetSelected(bool bSel)
	{
		//RED.SetActive (bSel, m_objSel);
		//m_objSel.enabled = bSel;
		m_objSel.alpha = (float)System.Convert.ToUInt32(bSel);
	}
	
	
	void SetCellState(bool isClock)
	{
		//上锁状态
		if(isClock)
		{
			m_spHead.color = new Color(0,0,0,1f);
			cellback.color = new Color(0.5f,0.5f,0.5f,1f);
			SprLock.enabled = true;
		}
		//解锁状态
		else
		{
			m_spHead.color = new Color(1f,1f,1f,1f);
			cellback.color = new Color(1f,1f,1f,1f);
			SprLock.enabled = false;
		}
	}
	
}
