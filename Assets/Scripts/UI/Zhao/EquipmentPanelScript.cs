using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RUIType;

public class EquipmentPanelScript : RUIMonoBehaviour {
	/// <summary>
	/// 装备名字
	/// </summary>
	public UILabel EquipNameLabel;
	/// <summary>
	/// 装备等级
	/// </summary>
	public UILabel LVLabel;
	/// <summary>
	/// 装备描述
	/// </summary>
	public UILabel DesLabel;
	/// <summary>
	/// 显示数字进度
	/// </summary>
	public UILabel ProgressLabel;

	public UISlider m_slider;

	/// <summary>
	/// 界面控制根节点
	/// </summary>
	public GameObject PanelRoot;
	/// <summary>
	/// 装备的图标
	/// </summary>
	public UISprite EquipmentICON;

	public UISprite m_spEquiped;

	/// <summary>
	/// 缘分人物名称
	/// </summary>
	public UILabel[] FateNamelabelArray;
	public UISprite[] m_arryMonsterHead;

	public UIButton mBtnStrength;
	public UIButton mBtnSell;
	public UIButton mBtnChange;
	
	public UILabel Lab_Atk;
	public UILabel Lab_Def;
	public UILabel Lab_EquipEffect;
	
	private Equipment mEquipData;
	private bool m_bShowChange;
	private static EquipmentPanelScript _mInstance;
	public static EquipmentPanelScript mInstance
	{
		get
		{
			return _mInstance;
		}
	}
	
    public List<GemRecastingHoleInfo> GemHole = new List<GemRecastingHoleInfo>();
    public StarsUI starObj;
	
	public System.Action GemSoltClick = null;
	public System.Action ExitFrogingClick = null;
	public System.Action FromChatClick = null;
	void Awake()
	{
		_mInstance = this;
		this.transform.parent = DBUIController.mDBUIInstance._TopRoot.transform;
	}
	

	void ClickGemButton(GameObject btn)
	{
		if(FromChatClick != null)
		{
			FromChatClick();
			FromChatClick = null;
			return;
		}

		BaseBuildingData build = Core.Data.BuildingManager.GetConfigByBuildLv(BaseBuildingData.BUILD_YELIAN, 1);
		if(build != null)
		{
			if (Core.Data.playerManager.RTData.curLevel < build.limitLevel)
			{
				string strText = Core.Data.stringManager.getString (6054);
				strText = strText.Replace ("#", build.limitLevel.ToString());
				SQYAlertViewMove.CreateAlertViewMove (strText);
				return;
			}
		}
			
		if(GemSoltClick != null)GemSoltClick();

		FrogingSystem.ForgingRoomUI.OpenUI(ExitFroging);
        UIMiniPlayerController.Instance.SetActive(true);
		FrogingSystem.ForgingRoomUI.Instance.GoTo(FrogingSystem.ForgingPage.Forging_Mosaic);
		gameObject.SetActive(false);
		
		if(mEquipData != null)
		FrogingSystem.ForgingRoomUI.Instance.InlaySystem.SelectEquipment(mEquipData);
	}
	
	void ExitFroging()
	{
		if(ExitFrogingClick!=null)ExitFrogingClick();
		gameObject.SetActive(true);
		if(FrogingSystem.ForgingRoomUI.Instance != null)
		{
		    Destroy(FrogingSystem.ForgingRoomUI.Instance.gameObject);
		}
		//FrogingSystem.ForgingRoomUI.Instance.DestoryForgingRoomUI();
		Core.Data.playerManager.RTData.curTeam.upgradeMember();
		DBUIController.mDBUIInstance._mainViewCtl.RefreshUserInfo();
		
	    //DBUIController.mDBUIInstance.SetViewState(EMViewState.S_Bag);
		ShowGems(mEquipData);
	}
	

	/// <summary>
	/// 关闭按钮
	/// </summary>
	void OnXBtnClick()
	{
		TweenScale tween = PanelRoot.GetComponent<TweenScale>();
		tween.delay = 0;
		tween.duration = 0.25f;
		tween.from =  Vector3.one;
		tween.to = new Vector3(0.01f,0.01f,0.01f);
		tween.onFinished.Clear();
		tween.onFinished.Add(new EventDelegate(this,"DestroyPanel"));
		tween.ResetToBeginning();
		tween.PlayForward();
	}
	/// <summary>
	/// Destroies the panel.
	/// </summary>
	public void DestroyPanel()
	{
		if (gameObject != null) 
		{
			Destroy (gameObject);
		}
		SQYUIManager.getInstance().targetEquip = null;
	}

	/// <summary>
	/// 强化按钮
	/// </summary>
	public void OnStrengBtnClick()
	{
//		List<Equipment> list = Core.Data.EquipManager.GetEquipList (mEquipData.ConfigEquip.type, SplitType.Split_If_InTeam);
//		if (list == null || list.Count == 0)
//		{
//			if (Core.Data.playerManager.RTData.curLevel >= 5) 
//			{
//				UIInformation.GetInstance ().SetInformation (Core.Data.stringManager.getString (5098), Core.Data.stringManager.getString (5030), OpenShopUI);
//			}
//			else
//			{
//				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5212));
//			}
//			return;
//		}
			
		if (mEquipData.RtEquip.lv >= 60)
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(5150));
			return;
		}

		if(mEquipData.ConfigEquip.type == 0)
		{
			DBUIController.mDBUIInstance.SetViewState(EMViewState.S_Bag,EMBoxType.Equip_QH_ATK);
		}
		else if(mEquipData.ConfigEquip.type == 1)
		{
			DBUIController.mDBUIInstance.SetViewState(EMViewState.S_Bag,EMBoxType.Equip_QH_DEF);
		}

		if(TeamUI.mInstance != null)
		{
			TeamUI.mInstance.SetShow (false);
		}

		SetActive(false);
	}

	void OpenShopUI()
	{
		UIDragonMallMgr.GetInstance ().OpenUI (ShopItemType.Egg, ShowTeamInfo);

		if(TeamUI.mInstance != null)
		{
			TeamUI.mInstance.SetShow (false);
		}

		RED.SetActive(false, this.gameObject);
	}

	void ShowTeamInfo()
	{
		if (TeamUI.mInstance != null)
		{
			TeamUI.mInstance.SetShow (true);
		}
		RED.SetActive(true, this.gameObject);
	}

	//替换装备
	void OnChangeEquipBtn()
	{
		List<Equipment> list = Core.Data.EquipManager.GetEquipList (mEquipData.ConfigEquip.type, SplitType.Split_If_InCurTeam);
		if (list == null || list.Count == 0)
		{
			if (Core.Data.playerManager.RTData.curLevel >= 5) 
			{
				if(LuaTest.Instance != null && !LuaTest.Instance.ConvenientBuy){;}
				else UIInformation.GetInstance ().SetInformation (Core.Data.stringManager.getString (5098), Core.Data.stringManager.getString (5030), OpenShopUI);
			}
			else
			{
				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5212));
			}
			return;
		}

		if(mEquipData.ConfigEquip.type == 0)
		{
			DBUIController.mDBUIInstance.SetViewState(EMViewState.S_Bag,EMBoxType.Equipment_SWAP_ATK);
		}
		else if(mEquipData.ConfigEquip.type == 1)
		{
			DBUIController.mDBUIInstance.SetViewState(EMViewState.S_Bag,EMBoxType.Equipment_SWAP_DEF);
		}

		if(TeamUI.mInstance != null)
		{
			TeamUI.mInstance.SetShow (false);
		}
		SetActive(false);
	}
	
	void OnSellBtnClick()
	{
		ComLoading.Open ();
		if(mEquipData.equipped)
		{
			if(Core.Data.playerManager.RTData.curLevel < 5)
			{
				string strText = Core.Data.stringManager.getString(7320);
				strText = string.Format(strText, 5);
				SQYAlertViewMove.CreateAlertViewMove(strText);
				ComLoading.Close ();
				return;
			}

			ChangeEquipmentParam param = new ChangeEquipmentParam();

			param.gid = Core.Data.playerManager.PlayerID;
			param.seqid = mEquipData.RtEquip.id;
			param.teqid =0;
			param.tmid = Core.Data.playerManager.RTData.curTeamId;
			param.pos= (short)(Core.Data.playerManager.RTData.curTeam.GetEquipPosByEquipID(mEquipData.RtEquip.id) + 1);

			HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
			task.AppendCommonParam(RequestType.CHANGE_EQUIPMENT, param);
			
			task.ErrorOccured += testHttpResp_Error;
			task.afterCompleted += testHttpResp_UI;
			
			//then you should dispatch to a real handler
			task.DispatchToRealHandler ();
		}
//		else
//		{
//			int[] arry = {mEquipData.RtEquip.id};
//			HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
//			task.AppendCommonParam(RequestType.SELL_EQUIPMENT, new  SellEquipParam(Core.Data.playerManager.PlayerID, arry));
//			
//			task.ErrorOccured += testHttpResp_Error;
//			task.afterCompleted += testHttpResp_UI;
//			
//			task.DispatchToRealHandler();
//		}
	}




	/// <summary>
	/// 初始化界面信息
	/// </summary>
	public void InitPanel(Equipment o)
	{
		mEquipData = o;
		SQYUIManager.getInstance().targetEquip = o;
		EquipNameLabel.text = o.name;
		LVLabel.text = "LV"+o.RtEquip.lv.ToString();
		DesLabel.text = o.ConfigEquip.description;

        starObj.SetStar(o.ConfigEquip.star);

		EquipmentICON.spriteName = mEquipData.ConfigEquip.ID.ToString ();
		RED.SetActive(mEquipData.equipped, m_spEquiped.gameObject);

		int totalExp = Core.Data.EquipManager.GetEquipTotalExp(o.RtEquip.lv + 1, o.ConfigEquip.star);
		ProgressLabel.text = o.RtEquip.exp.ToString() + "/" + totalExp.ToString();
		float rate = (float)o.RtEquip.exp / (float)totalExp;
		
		if(rate > 0.1f)
		{
			m_slider.value = rate;
		}
		else
		{
			m_slider.value =  0.1f;
		}

		List<MonsterData> monsterlist = Core.Data.fateManager.getMonsterFateByEquipNum (Core.Data.monManager, o.ConfigEquip.ID);;
		int count = monsterlist.Count;
		if(count > 4)
		{
			count = 4;
		}
		int i = 0;

		//  need to change
		for (; i < count; i++) {
			RED.SetActive(true, m_arryMonsterHead[i].transform.parent.gameObject);
			FateNamelabelArray [i].text = monsterlist [i].name; 
			AtlasMgr.mInstance.SetHeadSprite(m_arryMonsterHead[i],monsterlist[i].ID.ToString());
		}

		for (; i < 4; i++) {
			RED.SetActive(false, m_arryMonsterHead[i].transform.parent.gameObject);
		}

		if(mEquipData.equipped)
		{
			mBtnSell.TextID = 5010;
		}
		else
		{
			mBtnSell.TextID = 5008;
		}
		mBtnStrength.TextID = 5009;
		mBtnChange.TextID = 5002;

		RED.SetActive (m_bShowChange, mBtnChange.gameObject);
		
		#region Add by jc
		/*宝石孔和宝石
		 * */

		ShowGems(o);
		

		#endregion
		
	}
	
	
	void ShowGems(Equipment o)
	{
		EquipSlot[] slot = o.RtEquip.slot;
		
        if(slot.Length >= 0)
		{
			for(int i=0;i<GemHole.Count;i++)
			{
				if(i<slot.Length)
				{
					if(!GemHole[i].gameObject.activeSelf)
						GemHole[i].gameObject.SetActive(true);
                    if (slot[i].id > 0)
                    {
                        GemHole[i].SetGem(Core.Data.gemsManager.getGems(slot[i].id).configData.anime2D);
                        GemHole[i].SetLv(Core.Data.gemsManager.getGems(slot[i].id).configData.level);
                    }
                    else
                    {
                        GemHole[i].SetGem(null);
                        GemHole[i].SetLv(0);
                    }
					GemHole[i].SetHoleColor(slot[i].color);
				}
				else
				{
				    if(GemHole[i].gameObject.activeSelf)
						GemHole[i].gameObject.SetActive(false);
				}
			}
		}
		/*攻击力和防御力
		 * */
		Lab_Atk.text = o.getAttack.ToString();
		Lab_Def.text = o.getDefend.ToString();
		
		ShowEquipEffect(o,isHaveEffect(o));
	}
	
	
	
	
	
	
	/*是否满足开启隐藏属性条件
	 * */
	public bool isHaveEffect(Equipment eqi)
	{
		for(int i=0;i<eqi.RtEquip.slot.Length;i++)
		{
			if(eqi.RtEquip.slc[i]==0)
				return false;
			else if(eqi.RtEquip.slot[i].mGem ==null)
				return false;
			//else if(eqi.RtEquip.slc[i]!=eqi.RtEquip.slot[i].mGem.configData.color)
			else if(eqi.RtEquip.slot[i].color!=eqi.RtEquip.slot[i].mGem.configData.color)
				return false;
		}
		return true;
	}
	
	
	/*显示装备隐藏属性
	 * */
	public void ShowEquipEffect(Equipment eqi,bool isActive)
	{
	    int[] effect = eqi.ConfigEquip.effect;
		if(effect.Length>0)
		{
			for(int i=0;i<effect.Length;i++)
			{
			    //Debug.Log("effect:"+effect[i]);
				if(effect[i]>0)
				{
					switch(i)
					{
					case 0:
						if(isActive)
					    SetEquipEffect( "[ffffff]"+TEXT(9016) +":[-]" +"[fffb00]"+TEXT(9010)+effect[i] +"[-]");
						else
						SetEquipEffect( "[f3d7be]"+ TEXT(9016) +":" +TEXT(9010)+effect[i] +"[-]");
						break;
					case 1:
						if(isActive)
					    SetEquipEffect( "[ffffff]"+TEXT(9016) +":[-]" + "[fffb00]"+TEXT(9011)+ effect[i]+"[-]");
						else
						SetEquipEffect(  "[f3d7be]"+TEXT(9016) +":" +TEXT(9011)+effect[i] +"[-]");
						break;
					case 2:
						if(isActive)
						SetEquipEffect( "[ffffff]"+TEXT(9016) +":[-]" + "[fffb00]"+TEXT(9009)+ effect[i]+"[-]");
						else
						SetEquipEffect( "[f3d7be]"+TEXT(9016) +":" +TEXT(9009)+effect[i] +"[-]");
						break;
					} 
				}
			}
		}
	}
	
	
	public void SetEquipEffect(string text)
	{
		Lab_EquipEffect.text=text;
	}
	
	public string TEXT(int num_text)
	{
		return	Core.Data.stringManager.getString(num_text);
	}
	

	/// <summary>
	/// Showpbs the equipment.
	/// </summary>
	/// <returns>The equipment.</returns>
	/// <param name="root">Root.</param>
	public static EquipmentPanelScript OpenUI(Equipment o,  bool bShowChange , System.Action GemSoltEvent = null , System.Action ExitFrogingSystemEvent = null, System.Action FromChatSystemEvent = null)
	{

		if(mInstance == null)
		{
			GameObject obj = PrefabLoader.loadFromPack("GX/pbEquipmentPanel")as GameObject ;
			if(obj !=null)
			{
				NGUITools.AddChild(DBUIController.mDBUIInstance._TopRoot,  obj);
			}
		}
		else
		{
			mInstance.SetActive(true);
		}
		mInstance.m_bShowChange = bShowChange;
		mInstance.InitPanel(o);
		
		mInstance.GemSoltClick = GemSoltEvent;
		mInstance.ExitFrogingClick = ExitFrogingSystemEvent;
		mInstance.FromChatClick = FromChatSystemEvent;
		return mInstance;
	}

//	public void SetActive(bool bActive)
//	{
//		NGUITools.SetActive(this.gameObject, bActive);
//	}

	#region 网络返回
	
	void testHttpResp_UI (BaseHttpRequest request, BaseResponse response)
	{
		ComLoading.Close ();
		if (response.status != BaseResponse.ERROR) 
		{
			HttpRequest rq = request as HttpRequest;

			switch(rq.Type)
			{
				case RequestType.SELL_EQUIPMENT:
					TeamUI.mInstance.FreshCurTeam ();
					SQYMainController.mInstance.UpdateTeamTip ();
					break;

				case RequestType.CHANGE_EQUIPMENT:
					HttpRequest req = request as HttpRequest;
			

					if (EquipmentTableManager.Instance != null)
					{
						EquipmentTableManager.Instance.RefreshEquipment (TeamUI.mInstance.mSelectIndex);
//						DBUIController.mDBUIInstance.SetViewState (EMViewState.S_Team_NoSelect);
						TeamUI.mInstance.SetShow (true);
					}
					else if (SQYPetBoxController.mInstance != null && SQYPetBoxController.mInstance.IsShow)
					{
						DBUIController.mDBUIInstance.SetViewState (EMViewState.S_Bag, EMBoxType.LOOK_Equipment);
					}
					DBUIController.mDBUIInstance.RefreshUserInfoWithoutShow ();
					TeamUI.mInstance.FreshCurTeam ();
					SQYMainController.mInstance.UpdateTeamTip ();
				break;
			}
			DestroyPanel ();
		} 
		else 
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(30000 + response.errorCode));
		}
	}
	
	void testHttpResp_Error (BaseHttpRequest request, string error)
	{
		ComLoading.Close ();
	}
	
	#endregion
}
