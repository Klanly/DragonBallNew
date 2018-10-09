using UnityEngine;
using System.Collections;

public enum PVEType
{
	//剧情
	PVEType_Plot,
	//经验
	PVEType_Exp,
	//技能
	PVEType_Skill,
	//战魂
	PVEType_FightSoul,
}

public class JCPVEMainController : RUIMonoBehaviour {

    public static JCPVEMainController Instance;
	
	public JCPVEMainElement PVEType_Exp;
	public JCPVEMainElement PVEType_Skill;
	public JCPVEMainElement PVEType_FightSoul;
	
	//经验副本冷却倒计时
	public System.Action<long> ExpFBCoding;
	//宝石副本冷却倒计时
	public System.Action<long> GemFBCoding;
	[HideInInspector]
	public bool isExpFBCoding = false;
	[HideInInspector]
	public bool isGemFBCoding = false;
 
	public UIAtlas UIAtlas_MapLine;
	public UIAtlas UIAtlas_PVE_Map;
	void Start ()
	{
			

	}
	
	void OnBtnClick(GameObject btn)
	{
		OnBtnClick(btn.name,true);
	}
	
	public void OnBtnClick(string btnName,bool isBtnClick = false)
	{
		switch(btnName)
		{
		case "PVEType_Plot":
			Core.Data.newDungeonsManager .curFightingFBType = "PVEType_Plot";
			JCPVEPlotController.OpenUI().Exit = CallBackBtnReturn;
			gameObject.SetActive (false);
			break;
		case "PVEType_Exp":
			Core.Data.newDungeonsManager .curFightingFBType = "PVEType_Exp";
			JCPVEExpOrGem.OpenUI();
			break;
		case "PVEType_Skill":
			if(!JCPVETimerManager.Instance.isSkillFBCoding)
			{
				NewDungeonsManager ndm = Core.Data.newDungeonsManager;
				if(ndm.explorDoors != null && ndm.explorDoors.skill != null)
				{
					if(ndm.explorDoors.skill.count != ndm.explorDoors.skill.passCount)
					{
						Core.Data.newDungeonsManager .curFightingFBType = "PVEType_Skill";
						JCPVESkillController.OpenUI().Exit = CallBackBtnReturn;
						gameObject.SetActive (false);
					}
				}
			}
			if(isBtnClick)
			AutoShowBuyBox(1);
			break;
		case "PVEType_FightSoul":		
			if(!JCPVETimerManager.Instance.isFightSoulFBCoding)
			{
				NewDungeonsManager ndm = Core.Data.newDungeonsManager;
				if(ndm.explorDoors != null && ndm.explorDoors.souls != null)
				{
					if(ndm.explorDoors.souls.count != ndm.explorDoors.souls.passCount)
					{
						Core.Data.newDungeonsManager .curFightingFBType = "PVEType_FightSoul";
						JCPVEFightSoulController.OpenUI().Exit = CallBackBtnReturn;
						gameObject.SetActive (false);
					}
				}
			}
			if(isBtnClick)
			AutoShowBuyBox(2);
			break;
		case "BackButton":
			{
				gameObject.SetActive (false);
				if (PVEDownloadCartoonController.Instance != null)
				{
					PVEDownloadCartoonController.Instance.ClosePanel ();
				}
				DBUIController.mDBUIInstance.ShowFor2D_UI (false);

				TopMenuUI.DestroyUI ();
				break;
			}
		}
	}

	//安全自动显示购买弹出窗   type -->  //1：技能副本；2：战魂副本；3：经验副本；4：宝石副本
	public void AutoShowBuyBox(int type)
	{
		NewDungeonsManager ndm = Core.Data.newDungeonsManager;
		string title = null;
		string content = null;
		//是否够买一次机会
		bool haveEnoughMoney = false;

		if(type == 1)  //技能副本
		{
			VipInfoData vip =Core.Data.vipManager.GetVipInfoData(Core.Data.playerManager.curVipLv);
			if(vip != null && ndm.explorDoors != null && ndm.explorDoors.skill != null && ndm.explorDoors.skill.passCount >= ndm.explorDoors.skill.count )
			{
				if(ndm.explorDoors.skill.buyCount < vip.specialdoor1)
				{
					title = Core.Data.stringManager.getString(9115)+":"+ndm.explorDoors.skill.buyCount.ToString()+"/"+vip.specialdoor1.ToString();
					content = Core.Data.stringManager.getString(9116).Replace("{}","[ffff00]"+ndm.explorDoors.skill.needStone.ToString()+"[-]");
					haveEnoughMoney = Core.Data.playerManager.Stone >= ndm.explorDoors.skill.needStone;
				}
				else
				{
					VipInfoData data = Core.Data.vipManager.GetNextVipLevelBuyActiveFB(type,ndm.explorDoors.skill.buyCount);
					if(data != null)
						SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(9120).Replace("{}",data.vipLv.ToString()));
				}
			}
		}
		else if(type ==2) //战魂副本
		{
			VipInfoData vip =Core.Data.vipManager.GetVipInfoData(Core.Data.playerManager.curVipLv);
			if(vip != null && ndm.explorDoors != null && ndm.explorDoors.souls != null && ndm.explorDoors.souls.passCount >= ndm.explorDoors.souls.count )
			{
				if(ndm.explorDoors.souls.buyCount < vip.specialdoor2)
				{
					title = Core.Data.stringManager.getString(9115)+":"+ndm.explorDoors.souls.buyCount.ToString()+"/"+vip.specialdoor2.ToString();
					content = Core.Data.stringManager.getString(9116).Replace("{}","[ffff00]"+ndm.explorDoors.souls.needStone.ToString()+"[-]");
					haveEnoughMoney = Core.Data.playerManager.Stone >= ndm.explorDoors.souls.needStone;
				}
				else
				{
					VipInfoData data = Core.Data.vipManager.GetNextVipLevelBuyActiveFB(type,ndm.explorDoors.souls.buyCount);
					if(data != null)
						SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(9120).Replace("{}",data.vipLv.ToString()));
				}
			}
		}
		else if(type ==3) //经验副本
		{
			VipInfoData vip =Core.Data.vipManager.GetVipInfoData(Core.Data.playerManager.curVipLv);
			if(vip != null && ndm.explorDoors != null && ndm.explorDoors.exp != null && ndm.explorDoors.exp.passCount >= ndm.explorDoors.exp.count )
			{
				if(ndm.explorDoors.exp.buyCount < vip.specialdoor3)
				{
					title = Core.Data.stringManager.getString(9115)+":"+ndm.explorDoors.exp.buyCount.ToString()+"/"+vip.specialdoor3.ToString();
					content = Core.Data.stringManager.getString(9116).Replace("{}","[ffff00]"+ndm.explorDoors.exp.needStone.ToString()+"[-]");
					haveEnoughMoney = Core.Data.playerManager.Stone >= ndm.explorDoors.exp.needStone;
				}
				else
				{
					VipInfoData data = Core.Data.vipManager.GetNextVipLevelBuyActiveFB(type,ndm.explorDoors.exp.buyCount);
					if(data != null)
						SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(9120).Replace("{}",data.vipLv.ToString()));
				}
			}
		}
		else if(type ==4) //宝石副本
		{
			VipInfoData vip =Core.Data.vipManager.GetVipInfoData(Core.Data.playerManager.curVipLv);
			if(vip != null && ndm.explorDoors != null && ndm.explorDoors.gems != null && ndm.explorDoors.gems.passCount >= ndm.explorDoors.gems.count )
			{
				if(ndm.explorDoors.gems.buyCount < vip.specialdoor4)
				{
					title = Core.Data.stringManager.getString(9115)+":"+ndm.explorDoors.gems.buyCount.ToString()+"/"+vip.specialdoor4.ToString();
					content = Core.Data.stringManager.getString(9116).Replace("{}","[ffff00]"+ndm.explorDoors.gems.needStone.ToString()+"[-]");
					haveEnoughMoney = Core.Data.playerManager.Stone >= ndm.explorDoors.gems.needStone;
				}
				else
				{
					VipInfoData data = Core.Data.vipManager.GetNextVipLevelBuyActiveFB(type,ndm.explorDoors.gems.buyCount);
					if(data != null)
						SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(9120).Replace("{}",data.vipLv.ToString()));
				}
			}
		}



		//如果调用弹窗
		if(title != null && content != null)
		{
			JCPromptBox.OpenUI(title,content).OnBtnBuyClick = () =>
			{
				//客户端预判钻石
				if(haveEnoughMoney)
				{
					HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
					task.AppendCommonParam(RequestType.BUY_PVEACT_FB, new BuyPVEActivityFB(Core.Data.playerManager.PlayerID,type,1) );
					task.afterCompleted = (BaseHttpRequest request, BaseResponse response) =>
					{
						if (response.status != BaseResponse.ERROR)
						{	
							SyncPveResponse SPR= response as SyncPveResponse;
							Core.Data.newDungeonsManager.explorDoors = SPR.data;
							OnEnable();
							if(FreshExpOrGem != null)
							{
							    FreshExpOrGem();
							}
							JCPromptBox.Close();
							DBUIController.mDBUIInstance.RefreshUserInfo();
						}
						else
						{
							SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
						}
					};
					task.DispatchToRealHandler();
				}
				else
				{
					//35006钻石不足
					SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(35006));
				}
			};
		}
	}
	
	public System.Action FreshExpOrGem;
	
	
	public static JCPVEMainController CreatePVEMainView()
	{
		UnityEngine.Object obj = PrefabLoader.loadFromPack("JC/JCPVEMainController");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			JCPVEMainController cc = go.GetComponent<JCPVEMainController>();
			Instance = cc;
			Instance.Init();
			return cc;
		}
		return null;
	}
	
	void Init()
	{
	 
	}
	
	void OnEnable()
	{
		if(Instance == null)Instance = this;

		//初始化特殊副本是否解锁
		NewDungeonsManager newDM = Core.Data.newDungeonsManager;
		ExploreConfigData exploreConfig = newDM.GetExploreData (3);
		PVEType_Exp.isLock = (newDM.lastFloorId < exploreConfig.openfloor);

		string strText = Core.Data.stringManager.getString (9095);
		NewFloor floor = newDM.GetFloorData (exploreConfig.openfloor);
		if(floor != null)
		{
			int chapID = floor.BelongChapterID % 30000 / 100;
			string strLock = string.Format (strText, RED.GetChineseNum(chapID));
			PVEType_Exp.SetLockTxt (strLock);
		}

		exploreConfig = newDM.GetExploreData (1);
		PVEType_Skill.isLock = (newDM.lastFloorId < exploreConfig.openfloor);
		floor = newDM.GetFloorData (exploreConfig.openfloor);
		if(floor != null)
		{
			int chapID = floor.BelongChapterID % 30000 / 100;
			string strLock = string.Format (strText, RED.GetChineseNum(chapID));
			PVEType_Skill.SetLockTxt (strLock);
		}

		exploreConfig = newDM.GetExploreData (2);
		PVEType_FightSoul.isLock = (newDM.lastFloorId < exploreConfig.openfloor);
		floor = newDM.GetFloorData (exploreConfig.openfloor);
		if(floor != null)
		{
			int chapID = floor.BelongChapterID % 30000 / 100;
			string strLock = string.Format (strText, RED.GetChineseNum(chapID));
			PVEType_FightSoul.SetLockTxt (strLock);
		}

		JCPVETimerManager m = JCPVETimerManager.Instance;
		NewDungeonsManager dm = Core.Data.newDungeonsManager;	
		int SkillCount = 0;
		int SkillPassCount = 2;
		int FightCount = 0;
		int FightPassCount = 2;
		if(dm.explorDoors!= null)
		{
			//技能副本
			if(dm.explorDoors.skill != null)
			{
				SkillPassCount = dm.explorDoors.skill.passCount;
				SkillCount = dm.explorDoors.skill.count;
			    if(SkillPassCount < SkillCount)
				{
					 m.SkillFBCoding = SkillFBCoding;
	                 SkillFBCoding(Core.Data.temper.SkillDJS);
					if(!JCPVETimerManager.Instance.isSkillFBCoding) 
						PVEType_Skill.SetProgress(SkillPassCount,SkillCount,dm.explorDoors.skill.needStone);
				}
			   else
					PVEType_Skill.SetProgress(SkillPassCount,SkillCount,dm.explorDoors.skill.needStone);
				
			}
			//战魂副本
			if(dm.explorDoors.souls != null)
			{
				FightCount = dm.explorDoors.souls.count;
				FightPassCount = dm.explorDoors.souls.passCount;
			    if(FightPassCount < FightCount)
				{
					 m.FightSoulFBCoding = FightSoulFBCoding;	
	                 FightSoulFBCoding(Core.Data.temper.FightSoulDJS);
					if(!JCPVETimerManager.Instance.isFightSoulFBCoding)
						PVEType_FightSoul.SetProgress(FightPassCount,FightCount,dm.explorDoors.souls.needStone);
				}
			   else
					PVEType_FightSoul.SetProgress(FightPassCount,FightCount,dm.explorDoors.souls.needStone);

			}
			int ExpAndGemCount = 0;
			if(dm.explorDoors.exp != null)
				ExpAndGemCount+=dm.explorDoors.exp.count;
			if(dm.explorDoors.gems != null)
				ExpAndGemCount+=dm.explorDoors.gems.count;
			
			int ExpAndGemPassCount = 0;
			if(dm.explorDoors.exp != null)
				ExpAndGemPassCount+=dm.explorDoors.exp.passCount;
			if(dm.explorDoors.gems != null)
				ExpAndGemPassCount+=dm.explorDoors.gems.passCount;
			
			PVEType_Exp.SetProgress2(ExpAndGemPassCount.ToString()+"/"+ExpAndGemCount.ToString());
		}
	}
	
	
	
	
	//每个子界面关闭后的回调
	void CallBackBtnReturn(string btnName)
	{
		gameObject.SetActive(true);
	}
	

	/*技能副本计时*/
    public void SkillFBCoding(long time) 
	{
		if(PVEType_Skill != null)
		{
		    PVEType_Skill.SetTime(time);
		}
	}

	/*战魂副本计时*/
	public void FightSoulFBCoding(long time) 
	{
		if(PVEType_FightSoul != null)
		PVEType_FightSoul.SetTime(time);
	}


	
	
}
