using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//created by zhangqiang at 2014-3-19
public class LevelUpUI : MonoBehaviour 
{
	public UILabel m_txtTitle;
	public UILabel m_txtLevel;
	public UILabel m_txtEnergy_foreward;
	public UILabel m_txtMember_foreward;
	public UILabel m_txtEnergy_back;
	public UILabel m_txtMember_back;
    //public UILabel m_txtBuild;

	public UILabel m_txtBuildTitle;
	public UILabel[] m_txtLockBuild;
	public UISprite[] m_SpIcon;

	public UIButton m_btnOK;


    public GameObject _NoUnlock;
	
	public System.Action OnClose;
	
	//要展示的目标等级
	private int targetLv;
	
	private static LevelUpUI _mInstance;
	public static LevelUpUI mInstance
	{
        get {
			return _mInstance;
		}
	}
	
	
	//如果TargetLevel不是-1，说明要升到指定的级数,curLv没有在外面升级
	public static LevelUpUI OpenUI(int TargetLevel = -1)
	{
		if(_mInstance == null)
		{
			Object prefab = PrefabLoader.loadFromPack("ZQ/LevelUpUI");
			if(prefab != null)
			{
				GameObject obj = Instantiate(prefab) as GameObject;
				RED.AddChild(obj, DBUIController.mDBUIInstance._TopRoot);
				obj.transform.localScale = Vector3.one;
				obj.transform.localPosition = Vector3.zero;
				obj.transform.localEulerAngles = Vector3.zero;
				_mInstance = obj.GetComponent<LevelUpUI>();
                RED.TweenShowDialog(obj);
				_mInstance.targetLv = TargetLevel == -1 ? Core.Data.playerManager.Lv : TargetLevel;
				mInstance.InitUI(_mInstance.targetLv);
			}
		}
		else
		{
			_mInstance.targetLv = TargetLevel == -1 ? Core.Data.playerManager.Lv : TargetLevel;
			mInstance.InitUI(_mInstance.targetLv);
			mInstance.SetShow(true);
		}
		return _mInstance;
	}

	public  void DestroyUI()
	{
		Core.Data.deblockingBuildMgr.OpenLevelUpUnlock(true);

		if(_mInstance != null)
		{
			Destroy(_mInstance.gameObject);
			_mInstance = null;
			if(OnClose != null) OnClose();
		}
	}

	void Awake()
	{
		if(_mInstance == null)
		_mInstance = this;
		//InitUI();
	}

	void InitUI(int targetLv)
	{
        //TData 
        Core.Data.ActivityManager.SetTDAccountLv ();

		m_btnOK.TextID = 5030;
		m_txtTitle.text = Core.Data.stringManager.getString(5024);

		//	string strLevel = string.Format(Core.Data.stringManager.getString(5025), targetLv);
		m_txtLevel.text = targetLv.ToString();

		UserLevelInfo curInfo  = Core.Data.playerManager.ConfigList[targetLv - 1];
		UserLevelInfo preInfo = Core.Data.playerManager.ConfigList[targetLv - 2];

		//string strEnergy = string.Format(Core.Data.stringManager.getString(5026), preInfo.maxEnergy, curInfo.maxEnergy);
		m_txtEnergy_foreward.text = preInfo.maxEnergy.ToString();
		m_txtEnergy_back.text = curInfo.maxEnergy.ToString ();

//		string strMember = string.Format(Core.Data.stringManager.getString(5027), preInfo.petSlot, curInfo.petSlot);
//		m_txtMember.text = strMember;
		m_txtMember_foreward.text = preInfo.petSlot.ToString ();
		m_txtMember_back.text = curInfo.petSlot.ToString ();
//		string strBuild = string.Format(Core.Data.stringManager.getString(5028), preInfo.maxBuilding, curInfo.maxBuilding);
//		m_txtBuild.text = strBuild;

		m_txtBuildTitle.text = Core.Data.stringManager.getString(5029);

		List<BaseBuildingData> list = Core.Data.BuildingManager.GetLockBuildByLevel(targetLv);

        ActivityNetController.CheckUnGotGift();

		int i = 0;
		if(list == null)
		{
			for(i = 0; i < m_txtLockBuild.Length; i++)
			{
				RED.SetActive(false, m_txtLockBuild[i].gameObject);
			}
            //当没有建筑开启的时候添加一个提示
//            if (_NoUnlock.activeSelf == false ) // yangchenguang 7yue 16 
//            {
//                _NoUnlock.SetActive(true ) ; 
//            }

		}
		else
		{
            //当没有建筑开启的时候添加一个提示
//            if (_NoUnlock.activeSelf == true  )//yangchenguang 7yue 16 
//            {
//                _NoUnlock.SetActive(false) ; 
//            }

			for(; i < list.Count; i++)
			{
				System.Text.StringBuilder sb = new System.Text.StringBuilder (list [i].name);
				sb.Append ("\n");
				sb.Append ("Lv" + list [i].Lv);
				m_txtLockBuild [i].text = sb.ToString ();

				RED.SetActive(true, m_txtLockBuild[i].gameObject);
				RED.SetActive(false, m_SpIcon[i].gameObject);
			}

			for(; i < m_SpIcon.Length; i++)
			{
				RED.SetActive(false, m_txtLockBuild[i].gameObject);
				RED.SetActive(true, m_SpIcon[i].gameObject);
			}
		}
	}

	void SetShow(bool bShow)
	{
		RED.SetActive(bShow, this.gameObject);
	}

	public void OnBtnOK()
	{
        ActivityNetController.CheckUnGotGift(); // yangchengguang 升级的时候检测 是否满足活动条件
		if (BuildScene.mInstance != null)
		{
			List<BaseBuildingData> list = Core.Data.BuildingManager.GetLockBuildByLevel(targetLv);
			if (list != null)
			{
				foreach (BaseBuildingData build in list)
				{
					Building bd = Core.Data.BuildingManager.GetBuildFromBagByNum (build.id);
					if (bd != null)
					{
						if (BuildScene.mInstance != null)
						{
							BuildScene.mInstance.UpdateBuildByNum (bd.RTData.num);
						}
					}
					else
					{
						RED.LogWarning(bd.RTData.id + "not find in buildmgr");
					}
				}
			}
		}

		UserLevelInfo curInfo  = Core.Data.playerManager.curConfig;
		UserLevelInfo preInfo = Core.Data.playerManager.GetUserLvlInfo(Core.Data.playerManager.Lv - 1);

		if (curInfo.petSlot > preInfo.petSlot )
		{
			if (TeamUI.mInstance != null)
			{
				TeamUI.mInstance.RefreshSlot (curInfo.petSlot - 1);
			}
		}
		
#region 引导强制返回界面
		if(Core.Data.guideManger.isGuiding)
		{
			JCPVESaoDangPanel.SafeDelete();
			JCPVEPlotDes.SafeDelete();
			JCPVEExpOrGem.SafeDelete();
		    Core.Data.guideManger.Init();
		}
#endregion
		
		//如果在阵容界面不要刷新，因为阵容界面没有
		if(TeamUI.mInstance == null)
		DBUIController.mDBUIInstance.RefreshUserInfo ();


		if (Core.Data.playerManager.RTData.curVipLevel > Core.Data.temper.mPreVipLv)
		{
            LevelUpUIOther.OpenUI();
            LevelUpUIOther.mInstance.showVipUpdate(Core.Data.playerManager.RTData.curVipLevel);
			Core.Data.temper.mPreVipLv = Core.Data.playerManager.RTData.curVipLevel;
		}


		if(targetLv == 20)
		{
			//sync team info
			MailReveicer.Instance.SendFightMegRequest(100);
		}
		


		DestroyUI();
	}
	
	void UIInformationSure()
	{
		if(UICityFloorManager.Instance != null)
			UICityFloorManager.Instance.gameObject.SetActive(false);
		WXLAcitvityFactory.CreatActivity(ActivityItemType.levelRewardItem,DBUIController.mDBUIInstance._TopRoot,LevelRewardCallBack);
		
	}
	
	
	void LevelRewardCallBack()
	{
		if(UICityFloorManager.Instance != null)
			UICityFloorManager.Instance.gameObject.SetActive(true);
	}
}
