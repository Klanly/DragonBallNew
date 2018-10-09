using UnityEngine;
using System.Collections;

public class AttrSwapUI : MonoBehaviour 
{
	public UIButton m_btnSwap;
	public UILabel m_txtTitle;
	public UILabel m_txtDesp;
	public UILabel m_txtWuXinWan;
	public UILabel m_txtTip;
	public Card3DUI m_3dCard;
    public UISprite proSp;
    public UISprite proSp_1; 
	private Monster m_data;

	void Start()
	{
		m_btnSwap.TextID = 5067;
		m_txtDesp.text =  Core.Data.stringManager.getString (5068);
		m_txtTitle.text = Core.Data.stringManager.getString (5069);
		UpdateUI ();
	}

	public void SetShow(bool bShow)
	{
		RED.SetActive (bShow, this.gameObject);
		if(bShow)
		{
			m_3dCard.InitUI();
		}
	}

	void UpdateUI()
	{
		m_txtWuXinWan.text = "X" + Core.Data.itemManager.GetBagItemCount (ItemManager.WUXINGWAN).ToString ();
		if (m_data != null)
		{
			m_txtTip.text = "";
			m_btnSwap.isEnabled = false;
			Invoke ("PlayAnimFinish", 3);
			m_3dCard.Show3DCard(m_data);
		}
		else
		{
			m_txtTip.text = Core.Data.stringManager.getString (5057);
		}
	}


	void PlayAnimFinish()
	{
		m_btnSwap.isEnabled = true;
	}

	public void SetData(Monster mon)
	{
       

        proType = (int)mon.RTData.Attribute;
        StartCoroutine("scaleProSp" , proSp);

		SetShow (true);
		m_data = mon;
		m_txtTip.text = "";
		UpdateUI ();
	}

	void OnClickBack()
	{
		m_data = null;
		UpdateUI ();
		m_3dCard.Del3DModel ();
		RED.SetActive (false, this.gameObject);
		RED.SetActive (true, TrainingRoomUI.mInstance.m_mainTraining);

        if (proSp.gameObject.activeSelf )
        {
            proSp.gameObject.SetActive(false);
        }
        if (proSp_1.gameObject.activeSelf)
        {
            proSp_1.gameObject.SetActive(false) ;
        }


		if (TrainingRoomUI.mInstance.m_callBack != null)
		{
			TrainingRoomUI.mInstance.OnClickExit ();
		}
	}

	void OnClickMain()
	{
		DBUIController.mDBUIInstance.SetViewState (RUIType.EMViewState.S_Bag, RUIType.EMBoxType.ATTR_SWAP);
		TrainingRoomUI.mInstance.SetShow (false);
	}

	void OnClickSwap()
	{
		if (m_data == null)
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5058));
			return;
		}

		int needWuXin = 0;
		int needCoin = 0;
		if (m_data.RTData.m_nStage == RuntimeMonster.NORMAL_MONSTER)
		{
			needWuXin = 5;
			needCoin = 10000;
		}
		else if (m_data.RTData.m_nStage == RuntimeMonster.ZHEN_MONSTER)
		{
			needCoin = 50000;
			needWuXin = 20;
		}
		else
		{
			RED.LogWarning ("monster stage is wrong!");
			return;
		}

		if (Core.Data.itemManager.GetBagItemCount (ItemManager.WUXINGWAN) < needWuXin )
		{
			if (Core.Data.playerManager.RTData.curLevel < 10)
			{
				string strText = Core.Data.stringManager.getString (34062);
				SQYAlertViewMove.CreateAlertViewMove (strText);
			} 
			else
			{
				if(LuaTest.Instance != null && !LuaTest.Instance.ConvenientBuy){;}
				else UIInformation.GetInstance ().SetInformation (Core.Data.stringManager.getString (5205), Core.Data.stringManager.getString (5030), OpenDuihuanUI, null);
			}
			return;
		}

		if (Core.Data.playerManager.RTData.curCoin < needCoin )
		{
            JCRestoreEnergyMsg.OpenUI(ItemManager.COIN_PACKAGE,ItemManager.COIN_BOX,2);

			//SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(35000));
			return;
		}


		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.ATTR_SWAP, new AttrSwapParam(Core.Data.playerManager.PlayerID, m_data.pid));

		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;

		//then you should dispatch to a real handler
		task.DispatchToRealHandler ();

		ComLoading.Open ();
	}
		
	//打开兑换ui
	void OpenDuihuanUI()
	{
		TrainingRoomUI.mInstance.SetShow(false);
		RED.SetActive (false, TrainingRoomUI.mInstance.m_attrSwapUI.m_3dCard.mShowOne.gameObject);

		UIDragonMallMgr.GetInstance().OpenUI(ShopItemType.HotSale, OpenduihuanCallback);
//		FinalTrialMgr.GetInstance().OpenDuihuanRequest(OpenduihuanCallback, DuihuanFromType.Type_Zhaomu);
	}

	void OpenduihuanCallback()
	{
		TrainingRoomUI.mInstance.SetShow(true);
		RED.SetActive (true, TrainingRoomUI.mInstance.m_attrSwapUI.m_3dCard.mShowOne.gameObject);
		m_txtWuXinWan.text = "X" + Core.Data.itemManager.GetBagItemCount (ItemManager.WUXINGWAN).ToString ();
	}

	#region 网络返回

	void testHttpResp_UI (BaseHttpRequest request, BaseResponse response)
	{
		ComLoading.Close ();
		if (response.status != BaseResponse.ERROR) 
		{
			HttpRequest rq = request as HttpRequest;
			if (rq.Type == RequestType.ATTR_SWAP)
			{
				m_data = Core.Data.monManager.getMonsterById (m_data.pid);
                proType =  (int )m_data.RTData.Attribute; // yangchenguang
                StartCoroutine("scaleProSp1" , proSp_1);

				UpdateUI ();
				DBUIController.mDBUIInstance.RefreshUserInfo();
			}
		} 
		else 
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getNetworkErrorString(response.errorCode));
		}
	}

	void testHttpResp_Error (BaseHttpRequest request, string error)
	{
		ComLoading.Close ();
		ConsoleEx.DebugLog ("---- Http Resp - Error has ocurred." + error);
	}
	#endregion
    int proType =-1 ;
    IEnumerator scaleProSp(UISprite ProSp)
    {
        proSp_1.gameObject.SetActive(false);
        ProSp.gameObject.SetActive(true);
        yield return  null;
        ProSp.spriteName = "Attribute_"+ proType.ToString();
        proSp_1.spriteName = "Attribute_"+ proType.ToString();
        ProSp.gameObject.transform.localScale  = new Vector3(5,5,5) ;
        yield return  null;
        MiniItween.ScaleTo(ProSp.gameObject, new Vector3(1f, 1f, 1f), 0.2f, MiniItween.EasingType.EaseOutCubic);
    }

    IEnumerator scaleProSp1(UISprite ProSp)
    {
        ProSp.gameObject.SetActive(true);
        yield return  null;
        ProSp.spriteName = "Attribute_"+ proType.ToString();
        ProSp.gameObject.transform.localScale  = new Vector3(5,5,5) ;
        yield return  null;
        //yield return new WaitForSeconds(0.2f) ;
        MiniItween.ScaleTo(ProSp.gameObject, new Vector3(1f, 1f, 1f), 0.2f, MiniItween.EasingType.EaseOutCubic);
        yield return new WaitForSeconds(0.2f) ;
        ProSp.gameObject.SetActive(false);
        proSp.spriteName = "Attribute_"+ proType.ToString();
    }
}
