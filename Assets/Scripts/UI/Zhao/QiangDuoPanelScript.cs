using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class QiangDuoPanelScript : RUIMonoBehaviour 
{

	public GameObject BattleVideoRoot;

	public GameObject BattleVideoPanelObj;

	public GameObject tianXiaDiYiRoot;

	public UILabel TimeLabel;

	public UITable tianXiaDiYiTable;

	public UIScrollBar tianXiaDiYiScrollBar;

	public UIScrollView tianXiaDiYiScrollView;

	public GameObject SuDiRoot;   

	public GameObject DuiHuaRoot;

	public GameObject DuiHuanPanelObj;

	public GameObject duiHuanButton;

	public GameObject tianXiaDiYiPanelobj;

	public GameObject SuDiPanelobj;

	public UILabel suDiTotal; //宿敌上限值

	public UILabel suDiCount; //宿敌总数量
	public UILabel suDiNameTitle;

	public UITable suDiTable;

	public UILabel suDiPageLabel;

	public GameObject suDiUpPageButton;

	public GameObject suDiDownPageButton;

	public UISprite shuaXinBackgroundSprite;

	public GameObject ShuaXinObj;

	public UIButton shuaXinButton; // 右上角刷新按扭引用
   
	public UILabel actionButtonLabel;

	public UILabel suDiButtonLabel;

	public UILabel duiHuanButtonLabel;

	public static QiangDuoPanelScript Instance;

	public GameObject infoBackgroundObj;

	public UILabel tianXiaDiYiCurrentRankLabel;
	
	public UILabel tianXiaDiYiEveryMinZhangong;

	public UIButton m_QiangduoDuihuanIntroducebtn;
	public UILabel m_QiangduoDuihuanIntroduce;

	public bool isChangeDuiHuanZhanGong;

	public UILabel tianXiaDiYiFreeNameTitleLabel;
	public UILabel tianXiaDiYiTiaoZhanCountLabel;
	public UILabel tianXiaDiYiGloryInfoLabel;

	public GameObject fightCellPrb;

	public GameObject duiHuanCellPrb;

	public UITable duiHuanTable;

	public UILabel duiHuanZhanGong;

	public UILabel duiHuanZhanGongLabel;

	public GameObject qiangDuoGoldRoot;

	public UILabel qiangDuoGold;

	public UILabel qiangDuoGoldCount;

	public UILabel qiangDuoCount;
	public UILabel qiangDuoTitleName;


	public UIScrollBar qiangDuoScrollBar;

	public GameObject qiangDuoDragonBallRoot;
	public UIScrollBar qiangDuoDragonBallScrollBar;

	public UIScrollBar DuihuanScrollBar;

	public GameObject BtnHuifang;

	public UISprite[] m_QiangduoBtnChoose;

	public UISprite m_JifenOrZhangongIcon;


	public UIButton qiangDuoToggle;
	public UIButton suDiToggle;
	public UIButton duiHuanToggle;
	public UIButton huifangToggle;

	public UILabel lbl_RobDragonBallTimes;
	public UILabel lbl_RobDragonBallTimesTitle;

	public UILabel m_RankLeftTimeLab;
	public UILabel m_RevengeLeftTimeLab;
	public UILabel m_RobLeftTimeLab;
	public UILabel m_BallLeftTimeLab;

	public UILabel m_ZhangongRefreshTitle;
	
	public List<FightCell> ListCell = new List<FightCell>();
	public List<UIBattleVideoCell> mBattleVideoList = new List<UIBattleVideoCell>();

	long m_BallPvpLeftTime = 0;
	long m_RankPvpLeftTime = 0;
	long m_RobPvpLeftTime = 0;
	long m_RevengePvpLeftTime = 0;

	public static bool EnterfromQiangduo = false;

	private int _TimeCount = 0;

	TrialEnum m_TrialEnum
	{
		get{
			return FinalTrialMgr.GetInstance().NowEnum;
		}
	}

	QiangduoEnum m_qiangduoEnum;
	


	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		UIMiniPlayerController.ElementShowArray = new bool[]{true,true,false,true,true};
        DBUIController.mDBUIInstance.HiddenFor3D_UI();
	}

	public void ChangeShowType(QiangduoEnum m_QiangduoEnum)
	{
		m_qiangduoEnum = m_QiangduoEnum;
		if(m_TrialEnum == TrialEnum.TrialType_QiangDuoDragonBall || m_TrialEnum == TrialEnum.TrialType_QiangDuoGold)
		{
			duiHuanButtonLabel.text = Core.Data.stringManager.getString(20086);
			BtnHuifang.gameObject.SetActive(false);
		}
		else
		{
			duiHuanButtonLabel.text = Core.Data.stringManager.getString(20085);
			BtnHuifang.gameObject.SetActive(true);
		}
            
			hideAll();
		    this.shuaXinButton.gameObject.SetActive(true);
			SetShuaxinBtnName(true);
		switch (m_QiangduoEnum)
			{
			case QiangduoEnum.QiangduoEnum_List:
				if(m_TrialEnum == TrialEnum.TrialType_TianXiaDiYi)
				{
					SetBtnChooseOrNot(true, false, false, false);
					ShowTianXiaDiYi ();
				}
				else if(m_TrialEnum == TrialEnum.TrialType_QiangDuoGold)
				{
					SetBtnChooseOrNot(true, false, false, false);
					ShowQiangDuoJinBi();
				}
				else if(m_TrialEnum == TrialEnum.TrialType_QiangDuoDragonBall)
				{
					SetBtnChooseOrNot(true, false, false, false);
					ShowQiangDuoDragonBall();
            }
				break;
			case QiangduoEnum.QiangduoEnum_Duihuan:
			//wxl change 
				SetBtnChooseOrNot (false, false, true, false);
				ShowDuiHuan ();
				break;
			case QiangduoEnum.QiangduoEnum_Sudi: //宿敌
				SetBtnChooseOrNot(false, true, false, false);
				ShowSuDi ();
				break;
			case QiangduoEnum.QiangduoEnum_Playback:
				if(m_TrialEnum == TrialEnum.TrialType_QiangDuoDragonBall || m_TrialEnum == TrialEnum.TrialType_QiangDuoGold)SetBtnChooseOrNot(false, false, true, false);
				else SetBtnChooseOrNot(false, false, false, true);
				ShowBattleVideo();
				break;

		}

	}

	void SetBtnChooseOrNot(bool _key1,bool _key2,bool _key3,bool _key4)
	{

		Debug.Log (" change btn  ");
		if(m_QiangduoBtnChoose.Length == 4)
		{
			Debug.Log ( " 1 = "+_key1 + " 2 " +_key2 + " 3  " + _key3 );
			if(m_QiangduoBtnChoose[0] != null)
			{
				if(_key1)
				{
					qiangDuoToggle.normalSprite = "Symbol 31";
//					qiangDuoToggle.SetState(UIButtonColor.State.Pressed,true);
//					m_QiangduoBtnChoose[0].spriteName = "Symbol 31";
				}
				else 
				{
					qiangDuoToggle.normalSprite = "Symbol 32";
//					qiangDuoToggle.SetState(UIButtonColor.State.Normal,true);
//					m_QiangduoBtnChoose[0].spriteName = "Symbol 32";
				}
            }
			if(m_QiangduoBtnChoose[1] != null)
			{
				if(_key2)
				{
					suDiToggle.normalSprite = "Symbol 31";
//					suDiToggle.SetState(UIButtonColor.State.Pressed,true);
//					m_QiangduoBtnChoose[1].spriteName = "Symbol 31";
				}
				else
				{
					suDiToggle.normalSprite = "Symbol 32";
//					suDiToggle.SetState(UIButtonColor.State.Normal,true);
//					m_QiangduoBtnChoose[1].spriteName = "Symbol 32";
				}
            }
			if(m_QiangduoBtnChoose[2] != null)
			{
				if(_key3)
				{
					duiHuanToggle.normalSprite = "Symbol 31";
//					duiHuanToggle.SetState(UIButtonColor.State.Pressed,true);
//					m_QiangduoBtnChoose[2].spriteName = "Symbol 31";
				}
				else 
				{
					duiHuanToggle.normalSprite = "Symbol 32";
//					duiHuanToggle.SetState(UIButtonColor.State.Normal,true);
//					m_QiangduoBtnChoose[2].spriteName = "Symbol 32";
				}
            }
			if(m_QiangduoBtnChoose[3] != null)
			{
				if(_key4)
				{
					huifangToggle.normalSprite = "Symbol 31";
//					huifangToggle.SetState(UIButtonColor.State.Pressed,true);
//					m_QiangduoBtnChoose[3].spriteName = "Symbol 31";
				}
				else
				{
					huifangToggle.normalSprite = "Symbol 32";
//					huifangToggle.SetState(UIButtonColor.State.Normal,true);
//					m_QiangduoBtnChoose[3].spriteName = "Symbol 32";
				}
            }
        }
        

	}

	void hideAll()
	{
		HideTianXiaDiYi();
		HideSuDi ();
		HideDuiHuan ();
		HideQiangDuoJinBi();
		HideQiangDuoDragonBall();
		HideBattleVideo();

		DeleteVideoChild();
	}

	void ShowBattleVideo()
	{
		TimeLabel.text = "";
		ShuaXinObj.SetActive (false);
		BattleVideoRoot.SetActive(true);
		BattleVideoPanelObj.gameObject.SetActive(true);
		infoBackgroundObj.gameObject.SetActive(false);

		UIGrid mm = BattleVideoPanelObj.gameObject.GetComponentInChildren<UIGrid>();
		while(mm.transform.childCount > 0 )
		{
			GameObject g =  mm.transform.GetChild(0).gameObject;
			g.transform.parent = null;
            Destroy(g);
        }
        CreateBattleVideoCell(FinalTrialMgr.GetInstance().m_BattleVideoPlaybackdata, mm);
	}

	void CreateBattleVideoCell(BattleVideoPlaybackData[] data, UIGrid obj)
	{
		if(data != null)
		{
			GameObject obj1 = PrefabLoader.loadFromPack("LS/pbLSBattleVideoCell") as GameObject ;
			if(obj1 != null)
			{ 
				for(int i=0; i<data.Length; i++)
				{
					GameObject go = NGUITools.AddChild (obj.gameObject, obj1);
					go.gameObject.name = "pbLSBattleVideoCell" + i.ToString();
					UIBattleVideoCell mm = go.gameObject.GetComponent<UIBattleVideoCell>();
					mm.OnShow(data[i]);
					mBattleVideoList.Add (mm);
				}
			}
			obj.Reposition();
		}

	}

	void DeleteBattleVideoCell()
	{
		foreach(UIBattleVideoCell item in mBattleVideoList)
		{
			item.dealloc();
		}
	}

	void DeleteVideoChild()
	{
		foreach(UIBattleVideoCell item in mBattleVideoList)
		{
			item.dealloc();
		}
	}

	void ShowTianXiaDiYi()
	{
		TimeLabel.text = "";
		shuaXinButton.isEnabled = true;
		tianXiaDiYiRoot.SetActive (true);

		m_RankPvpLeftTime = FinalTrialMgr.GetInstance().m_PVPRankLeftTime;
		CancelInvoke("RepeatRankPVPTime");
		SetRankPVPTimeLab(m_RankPvpLeftTime);
	
		tianXiaDiYiPanelobj.gameObject.SetActive (true);
		actionButtonLabel.text = Core.Data.stringManager.getString(6000);// "挑战";
		infoBackgroundObj.SetActive(true);
		Debug.Log(FinalTrialMgr.GetInstance().tianXiaDiYiInfo);
		tianXiaDiYiCurrentRankLabel.text = FinalTrialMgr.GetInstance().tianXiaDiYiInfo.rank.ToString();
		tianXiaDiYiEveryMinZhangong.SafeText("+ " + FinalTrialMgr.GetInstance().EveryMinZhangong.ToString());
		if(FinalTrialMgr.GetInstance().allPVPRobData!=null && FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus!= null && FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.rank != null 
		   && FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.rank.count - FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.rank.passCount >= 0)
		{
			tianXiaDiYiTiaoZhanCountLabel.text = (FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.rank.count - FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.rank.passCount).ToString();
		}
		else
		{
			tianXiaDiYiTiaoZhanCountLabel.text = "0";
		}
		tianXiaDiYiScrollView.verticalScrollBar = tianXiaDiYiScrollBar;
		ShowFightCell(tianXiaDiYiTable, FinalTrialMgr.GetInstance().tianXiaDiYiInfo.roles);
	}
    // --------宿敌
	public void ShowSuDi()
	{
		TimeLabel.text = "";
		if(m_TrialEnum == TrialEnum.TrialType_QiangDuoDragonBall)
		{
			SuDiRoot.SetActive(false);
			infoBackgroundObj.SetActive(false);
		}
		else
		{	
			SuDiRoot.SetActive (true);
			infoBackgroundObj.SetActive(true);
		}

		ShuaXinObj.SetActive (true);
		SuDiPanelobj.gameObject.SetActive (true);

		m_RevengePvpLeftTime = FinalTrialMgr.GetInstance().m_PVPRevengeLeftTime;
		CancelInvoke("RepeatRevengePVPTime");
		SetRevengePVPTimeLab(m_RevengePvpLeftTime);

        VipInfoData vipConfig = Core.Data.vipManager.GetVipInfoData(Core.Data.playerManager.RTData.curVipLevel);
        if (vipConfig != null) 
		{
			this.suDiTotal.text =FinalTrialMgr.GetInstance().sudiDataQueue.Count.ToString() + "/" + vipConfig.enemyLimit.ToString();
		}
			
		if (Core.Data.playerManager.revengeData != null)
		{
			int _curprogress = Core.Data.playerManager.revengeData.curProgress;
			int _maxprogress = Core.Data.playerManager.revengeData.maxProgress;
			if(_curprogress >= _maxprogress)
			{
				this.suDiCount.SafeText(_curprogress.ToString() + "/" + _curprogress.ToString());
			}
			else
			{
				this.suDiCount.SafeText(_curprogress.ToString() + "/" + _maxprogress.ToString());
			}
		}
       
     
        //设置Cell 
        ShowFightCell(suDiTable, getSuDiData(5));
	}

   //  从队列取数据 参数需要的数量
    FightOpponentInfo[] getSuDiData(int needNum){
        int len = FinalTrialMgr.GetInstance().sudiDataQueue.Count;//
        if (needNum > len)
            needNum = len;
        FightOpponentInfo[] temp= new FightOpponentInfo[needNum];
		FinalTrialMgr.GetInstance().suDiTempList.Clear();
        for(int i=0;i<needNum;i++){
            FightOpponentInfo fightinfo= FinalTrialMgr.GetInstance().sudiDataQueue.Dequeue();
            temp [i] = fightinfo;
            FinalTrialMgr.GetInstance().suDiTempList.Add(fightinfo); //保存使用的数据，用于删除时恢复使用
        }
        return temp;
    }

	void ShowDuiHuan()
	{
		TimeLabel.text = "";
		this.shuaXinButton.gameObject.SetActive(false);
		if(m_TrialEnum == TrialEnum.TrialType_QiangDuoDragonBall)
		{
			SuDiRoot.SetActive (false);
			DuiHuaRoot.gameObject.SetActive(false);
			infoBackgroundObj.SetActive(false);
		}
		else 
		{
			DuiHuaRoot.gameObject.SetActive(true);

			infoBackgroundObj.SetActive(true);

		}
		DuiHuanPanelObj.gameObject.SetActive (true);

		SetShuaxinBtnName(false);
		if(m_TrialEnum == TrialEnum.TrialType_TianXiaDiYi)
		{
			duiHuanZhanGongLabel.text = Core.Data.stringManager.getString(6008);
			m_QiangduoDuihuanIntroducebtn.gameObject.SetActive(true);
			m_QiangduoDuihuanIntroduce.SafeText(Core.Data.stringManager.getString(25159));
			ShowDuiHuanItemCell(Core.Data.DuiHuanManager.buyItemIDList);
			m_JifenOrZhangongIcon.spriteName = "common-0077";
		}
		else if(m_TrialEnum == TrialEnum.TrialType_QiangDuoGold)
		{
			m_JifenOrZhangongIcon.spriteName = "jifen";
			duiHuanZhanGongLabel.text = Core.Data.stringManager.getString(6023);
			m_QiangduoDuihuanIntroducebtn.gameObject.SetActive(true);
			m_QiangduoDuihuanIntroduce.SafeText(Core.Data.stringManager.getString(25155));
			duiHuanZhanGong.text = FinalTrialMgr.GetInstance().TotalJifen.ToString();
			ShowDuiHuanItemCell(Core.Data.DuiHuanManager.buyItemTotalList);
		}
		m_ZhangongRefreshTitle.SafeText(Core.Data.stringManager.getString(6004) + (FinalTrialMgr.GetInstance().m_ZhangongRefreshMaxTimes-FinalTrialMgr.GetInstance().m_ZhangongsurplusRefreshTimes)
		                                + "/" + FinalTrialMgr.GetInstance().m_ZhangongRefreshMaxTimes);
	}

	void ShowDuiHuanItemCell(List<GoldBuyItemBuyTotalInfo>  item)
	{
		if(this.duiHuanCellPrb == null)
		{
			this.duiHuanCellPrb = PrefabLoader.loadFromPack("WHY/pbDuiHuanShowCell") as GameObject;;
		}
		
		while(duiHuanTable.transform.childCount > 0 )
		{
			GameObject g =  duiHuanTable.transform.GetChild(0).gameObject;
			g.transform.parent = null;
			Destroy(g);
		}
		
		int _index = 0;
		foreach(GoldBuyItemBuyTotalInfo zhangongitem in item)
		{
			GameObject duiHuanCellObj = Instantiate(this.duiHuanCellPrb) as GameObject;
			duiHuanCellObj.name = _index.ToString();
			UIDuiHuanCell duiHuanCell = duiHuanCellObj.GetComponent<UIDuiHuanCell>();
			
			duiHuanCell.gameObject.transform.parent = duiHuanTable.transform;
			duiHuanCell.transform.localScale = Vector3.one;
			
			duiHuanCell.Goldbuyitem = zhangongitem;

			duiHuanCell.m_TypeIcon.spriteName = "jifen";
			if(DataCore.getDataType(zhangongitem.pid) == ConfigDataType.Frag)
			{
				duiHuanCell.m_NameLab.SafeText(Core.Data.soulManager.GetSoulConfigByNum(zhangongitem.pid).name);
				duiHuanCell.m_Dec.SafeText(Core.Data.soulManager.GetSoulConfigByNum(zhangongitem.pid).description);
				duiHuanCell.m_StarUI.SetStar(Core.Data.soulManager.GetSoulConfigByNum(zhangongitem.pid).star);
				duiHuanCell.m_Attr.gameObject.SetActive(true);
				if(Core.Data.soulManager.GetSoulConfigByNum(zhangongitem.pid).type == (int)ItemType.Monster_Frage)
				{
					MonsterData mon = Core.Data.monManager.getMonsterByNum(Core.Data.soulManager.GetSoulConfigByNum(zhangongitem.pid).updateId);
					AtlasMgr.mInstance.SetHeadSprite(duiHuanCell.m_ItemIcon, mon.ID.ToString());
					duiHuanCell.m_Attr.spriteName = "bag-0003";
				}
				else if(Core.Data.soulManager.GetSoulConfigByNum(zhangongitem.pid).type == (int)ItemType.Equip_Frage)
				{

					duiHuanCell.m_ItemIcon.atlas = AtlasMgr.mInstance.equipAtlas;
					duiHuanCell.m_ItemIcon.spriteName = Core.Data.soulManager.GetSoulConfigByNum(zhangongitem.pid).updateId.ToString();
					duiHuanCell.m_Attr.spriteName = "sui";
				}

			}
			else if(DataCore.getDataType(zhangongitem.pid) == ConfigDataType.Equip)
			{
				duiHuanCell.m_NameLab.SafeText(Core.Data.EquipManager.getEquipConfig(zhangongitem.pid).name);
				duiHuanCell.m_Dec.SafeText(Core.Data.EquipManager.getEquipConfig(zhangongitem.pid).description);
				duiHuanCell.m_StarUI.SetStar(Core.Data.EquipManager.getEquipConfig(zhangongitem.pid).star);
				duiHuanCell.m_ItemIcon.atlas = AtlasMgr.mInstance.equipAtlas;
				duiHuanCell.m_ItemIcon.spriteName = zhangongitem.pid.ToString();
				duiHuanCell.m_Attr.gameObject.SetActive(false);
			}
			else
			{
				duiHuanCell.m_NameLab.SafeText(Core.Data.itemManager.getItemData(zhangongitem.pid).name);
				duiHuanCell.m_Dec.SafeText(Core.Data.itemManager.getItemData(zhangongitem.pid).description);
				duiHuanCell.m_StarUI.SetStar(Core.Data.itemManager.getItemData(zhangongitem.pid).star);
				duiHuanCell.m_ItemIcon.atlas = AtlasMgr.mInstance.itemAtlas;
				ItemData _ItemData = Core.Data.itemManager.getItemData(zhangongitem.pid);
				if(_ItemData != null)duiHuanCell.m_ItemIcon.spriteName = _ItemData.iconID.ToString();
				duiHuanCell.m_Attr.gameObject.SetActive(false);
			}
			duiHuanCell.m_ItemIcon.MakePixelPerfect();
			duiHuanCell.m_Num.SafeText(zhangongitem.num.ToString());
			duiHuanCell.currentDuiHuanType = UIDuiHuanCell.DuiHuanType.JinBiDuiHuanCell;
			duiHuanCell.m_BtnName.text = Core.Data.stringManager.getString(6003);
			duiHuanCell.m_Money.SafeText(zhangongitem.jifen.ToString());
			if(zhangongitem.discountjf == 0)
			{
				duiHuanCell.m_MoneySale.SafeText("");
				duiHuanCell.m_SaleLine.gameObject.SetActive(false);
			}
			else
			{
				duiHuanCell.m_MoneySale.SafeText(zhangongitem.discountjf.ToString());
				duiHuanCell.m_SaleLine.gameObject.SetActive(true);
			}


			_index++;
			
		}
		duiHuanTable.Reposition();
	}

	void ShowDuiHuanItemCell(List<ZhanGongItem>  item)
	{
		if(this.duiHuanCellPrb == null)
		{
			this.duiHuanCellPrb = PrefabLoader.loadFromPack("WHY/pbDuiHuanShowCell") as GameObject;;
		}

		while(duiHuanTable.transform.childCount > 0 )
		{
			GameObject g =  duiHuanTable.transform.GetChild(0).gameObject;

			g.transform.parent = null;
			Destroy(g);
		}

		int _index = 0;
		foreach(ZhanGongItem zhangongitem in item)
		{
			GameObject duiHuanCellObj = Instantiate(this.duiHuanCellPrb) as GameObject;
			duiHuanCellObj.name = _index.ToString();
			UIDuiHuanCell duiHuanCell = duiHuanCellObj.GetComponent<UIDuiHuanCell>();

			duiHuanCell.gameObject.transform.parent = duiHuanTable.transform;
			duiHuanCell.transform.localScale = Vector3.one;

			duiHuanCell.duiHuanItem = zhangongitem;
			duiHuanCell.m_TypeIcon.spriteName = "common-0077";
			if(zhangongitem.canBuy)duiHuanCell.buyButton.isEnabled = true;
			else duiHuanCell.buyButton.isEnabled = false;
			if(DataCore.getDataType(zhangongitem.pid) == ConfigDataType.Frag)
			{
				duiHuanCell.m_NameLab.SafeText(Core.Data.soulManager.GetSoulConfigByNum(zhangongitem.pid).name);
				duiHuanCell.m_Dec.SafeText(Core.Data.soulManager.GetSoulConfigByNum(zhangongitem.pid).description);
				duiHuanCell.m_StarUI.SetStar(Core.Data.soulManager.GetSoulConfigByNum(zhangongitem.pid).star);
				duiHuanCell.m_Attr.gameObject.SetActive(true);
				if(Core.Data.soulManager.GetSoulConfigByNum(zhangongitem.pid).type == (int)ItemType.Monster_Frage)
				{
					MonsterData mon = Core.Data.monManager.getMonsterByNum(Core.Data.soulManager.GetSoulConfigByNum(zhangongitem.pid).updateId);
					AtlasMgr.mInstance.SetHeadSprite(duiHuanCell.m_ItemIcon, mon.ID.ToString());
					duiHuanCell.m_Attr.spriteName = "bag-0003";
				}

				else if(Core.Data.soulManager.GetSoulConfigByNum(zhangongitem.pid).type == (int)ItemType.Equip_Frage)
				{
					EquipData equip = Core.Data.EquipManager.getEquipConfig (Core.Data.soulManager.GetSoulConfigByNum(zhangongitem.pid).updateId);
					duiHuanCell.m_ItemIcon.atlas = AtlasMgr.mInstance.equipAtlas;
					duiHuanCell.m_ItemIcon.spriteName = Core.Data.soulManager.GetSoulConfigByNum(zhangongitem.pid).updateId.ToString();
					duiHuanCell.m_Attr.spriteName = "sui";
				}
			}
			else if(DataCore.getDataType(zhangongitem.pid) == ConfigDataType.Equip)
			{
				duiHuanCell.m_NameLab.SafeText(Core.Data.EquipManager.getEquipConfig(zhangongitem.pid).name);
				duiHuanCell.m_Dec.SafeText(Core.Data.EquipManager.getEquipConfig(zhangongitem.pid).description);
				duiHuanCell.m_StarUI.SetStar(Core.Data.EquipManager.getEquipConfig(zhangongitem.pid).star);
				duiHuanCell.m_ItemIcon.atlas = AtlasMgr.mInstance.equipAtlas;
				duiHuanCell.m_ItemIcon.spriteName = zhangongitem.pid.ToString();
				duiHuanCell.m_Attr.gameObject.SetActive(false);
			}
			else
			{
				duiHuanCell.m_Attr.gameObject.SetActive(false);
				duiHuanCell.m_NameLab.SafeText(Core.Data.itemManager.getItemData(zhangongitem.pid).name);
				duiHuanCell.m_Dec.SafeText(Core.Data.itemManager.getItemData(zhangongitem.pid).description);
				duiHuanCell.m_StarUI.SetStar(Core.Data.itemManager.getItemData(zhangongitem.pid).star);
				duiHuanCell.m_ItemIcon.atlas = AtlasMgr.mInstance.itemAtlas;
				ItemData _ItemData = Core.Data.itemManager.getItemData(zhangongitem.pid);
				if(_ItemData != null)duiHuanCell.m_ItemIcon.spriteName = _ItemData.iconID.ToString();
			}
			duiHuanCell.m_ItemIcon.MakePixelPerfect();
			duiHuanCell.m_Money.SafeText(zhangongitem.price.ToString());
			duiHuanCell.m_Num.SafeText(zhangongitem.num.ToString());
			if(zhangongitem.discount == 0)
			{
				duiHuanCell.m_MoneySale.SafeText("");
				duiHuanCell.m_SaleLine.gameObject.SetActive(false);
			}
			else
			{
				duiHuanCell.m_MoneySale.SafeText(zhangongitem.discount.ToString());
				duiHuanCell.m_SaleLine.gameObject.SetActive(true);
			}



			if(m_TrialEnum == TrialEnum.TrialType_TianXiaDiYi)
			{
				duiHuanCell.currentDuiHuanType = UIDuiHuanCell.DuiHuanType.ZhanGongDuiHuanCell;

				if(zhangongitem.price != 0)
				{
					duiHuanCell.m_BtnName.text = Core.Data.stringManager.getString(6003);
				}
				else
				{
					duiHuanCell.m_BtnName.text = Core.Data.stringManager.getString(20072);
					if(!Core.Data.DuiHuanManager.CheckLingqu(zhangongitem.rank))
					{
						duiHuanCell.buyButton.isEnabled = false;
						duiHuanCell.m_BtnName.text = Core.Data.stringManager.getString(20075);
					}
					else
					{
						duiHuanCell.buyButton.isEnabled = true;
						duiHuanCell.m_BtnName.text = Core.Data.stringManager.getString(20072);
					}

				}
			}
			else if(m_TrialEnum == TrialEnum.TrialType_QiangDuoGold)
			{
				duiHuanCell.currentDuiHuanType = UIDuiHuanCell.DuiHuanType.JinBiDuiHuanCell;
				if(zhangongitem.price != 0)
				{
					duiHuanCell.m_BtnName.text = Core.Data.stringManager.getString(6003);
				}
				else
				{
					duiHuanCell.m_BtnName.text = Core.Data.stringManager.getString(20072);
				}

			}
			_index++;
		}
		duiHuanTable.Reposition();
	}

	void ShowQiangDuoJinBi()
	{
		tianXiaDiYiPanelobj.SetActive(true);
		qiangDuoGoldRoot.gameObject.SetActive (true);
		actionButtonLabel.text = Core.Data.stringManager.getString(6001);// "抢夺";
		infoBackgroundObj.SetActive(true);

		m_RobPvpLeftTime = FinalTrialMgr.GetInstance().m_PVPRobLeftTime;
		CancelInvoke("RepeatRobPVPTime");
		SetRobPVPTimeLab(m_RobPvpLeftTime);

		if(FinalTrialMgr.GetInstance().allPVPRobData != null && FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus != null && FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.robs != null 
		   && FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.robs.count - FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.robs.passCount >= 0)
		{
			qiangDuoCount.text = (FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.robs.count - FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.robs.passCount).ToString();
		}
		else
		{
			qiangDuoCount.text = "0";
		}
	
		tianXiaDiYiScrollView.verticalScrollBar = qiangDuoScrollBar;


		if(_TimeCount > 0)
		{
			shuaXinButton.isEnabled = false;
			shuaXinButton.Text = "";
		}
		else 
		{
			TimeLabel.text = "";
			changeShuaXinButtonEnabled(true);
		}

		ShowFightCell(tianXiaDiYiTable, FinalTrialMgr.GetInstance().qiangDuoGoldOpponentsInfo.player);
 
	}

    //展示 抢夺龙珠列表
	void ShowQiangDuoDragonBall()
	{
		tianXiaDiYiRoot.SetActive (false);
		tianXiaDiYiPanelobj.gameObject.SetActive (true);
		actionButtonLabel.text = Core.Data.stringManager.getString(6001);// "抢夺";
		infoBackgroundObj.SetActive(true);
		SuDiRoot.SetActive(false);
		qiangDuoDragonBallRoot.SetActive(true);
		tianXiaDiYiScrollView.verticalScrollBar = qiangDuoDragonBallScrollBar;

		m_BallPvpLeftTime = FinalTrialMgr.GetInstance().m_PVPBallLeftTime;
		CancelInvoke("RepeatBallPVPTime");
		SetBallPVPTimeLab(m_BallPvpLeftTime);
	
		this.SetDragonBallTimes ();

		if(_TimeCount > 0)
		{
			shuaXinButton.isEnabled = false;
			shuaXinButton.Text = "";
		}
		else
		{
			TimeLabel.text = ""; 
		}

		duiHuanButtonLabel.text = Core.Data.stringManager.getString(20086);
		BtnHuifang.gameObject.SetActive(false);
        FinalTrialMgr.GetInstance ().NowEnum = TrialEnum.TrialType_QiangDuoDragonBall;
		
		ShowFightCell(tianXiaDiYiTable, FinalTrialMgr.GetInstance().currentQiangDuoDragonBallList.ToArray());
		
		if(Core.Data.guideManger.isGuiding && Core.Data.guideManger.LastTaskID == Core.Data.guideManger.ShowDialogueBeforeRob)
		{
			Core.Data.guideManger.AutoRUN();
		}
	}

	void SetDragonBallTimes(){
		GetDuoBaoLoginInfoDataPvpStatus robBallStatus = null;
		if(FinalTrialMgr.GetInstance().allPVPRobData != null && FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus != null && FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.ball != null)
		    robBallStatus = FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.ball;
		if (robBallStatus != null)
		{
			int tNum = robBallStatus.count - robBallStatus.passCount;
			if (tNum <= 0)
				tNum = 0;
			lbl_RobDragonBallTimes.text  = tNum.ToString();
		}
	}


	void HideTianXiaDiYi()
	{
		tianXiaDiYiRoot.SetActive (false);
		tianXiaDiYiPanelobj.gameObject.SetActive (false);
	}

	void HideSuDi()
	{
		SuDiRoot.SetActive (false);
		ShuaXinObj.SetActive (true);
		SuDiPanelobj.gameObject.SetActive (false);
	}

	void HideDuiHuan()
	{
		DuiHuaRoot.gameObject.SetActive (false);
		DuiHuanPanelObj.gameObject.SetActive (false);
	}

	void HideQiangDuoJinBi()
	{
		tianXiaDiYiRoot.SetActive (false);
		qiangDuoGoldRoot.SetActive(false);
	}

	void HideQiangDuoDragonBall()
	{
		tianXiaDiYiRoot.SetActive (false);
//		ChangeSpriteArray [0].gameObject.SetActive (false);
		tianXiaDiYiPanelobj.gameObject.SetActive (false);
		qiangDuoDragonBallRoot.SetActive(false);
	}

	void HideBattleVideo()
	{
		ShuaXinObj.SetActive (true);
		BattleVideoRoot.SetActive(false);
		BattleVideoPanelObj.gameObject.SetActive(false);
	}
		//创建 抢夺场景的 cell
	void ShowFightCell(UITable table, FightOpponentInfo[] roles)
	{
		   
        if(this.fightCellPrb == null)
		{
			this.fightCellPrb = PrefabLoader.loadFromPack("GX/pbFightCell") as GameObject;;
		}

		while(table.transform.childCount > 0 )
		{
			GameObject g =  table.transform.GetChild(0).gameObject;
			g.transform.parent = null;
			Destroy(g);
		}
		table.Reposition();

		if(FinalTrialMgr.GetInstance()._IsNullPlayer)
		{
			FinalTrialMgr.GetInstance()._IsNullPlayer = false;
			return;
		}
		else
		{
			if(m_qiangduoEnum == QiangduoEnum.QiangduoEnum_List)
			{
				if(m_TrialEnum == TrialEnum.TrialType_TianXiaDiYi)
				{
					if(FinalTrialMgr.GetInstance().TianXiaRoleQueue.Count <= 0)
					{
						changeShuaXinButtonEnabled(false);
						FinalTrialMgr.GetInstance().getTianXiaDiYiOpponentsCompletedDelegate = getTianXiaDiYiOpponentsCompleted;
						ComLoading.Open();
						FinalTrialMgr.GetInstance().RequestByQiangduoType(QiangduoEnum.QiangduoEnum_List);
						return;
					}
				}
				
				if(m_TrialEnum == TrialEnum.TrialType_QiangDuoGold)
				{
					if(FinalTrialMgr.GetInstance().QiangduoRoleQueue.Count <= 0)
					{
						changeShuaXinButtonEnabled(false);
						ComLoading.Open();
						
						FinalTrialMgr.GetInstance().RequestByQiangduoType(QiangduoEnum.QiangduoEnum_List);
						return;
					}
				}
			}
        }
        

        if(roles == null || roles.Length == 0)
        {
            return;
        }
        
        ListCell.Clear();
		int length = 0; 
		if(m_qiangduoEnum == QiangduoEnum.QiangduoEnum_List)
		{
			if(m_TrialEnum == TrialEnum.TrialType_TianXiaDiYi )
			{
				if(FinalTrialMgr.GetInstance().TianXiaRoleQueue.Count < 6)
				{
					length = FinalTrialMgr.GetInstance().TianXiaRoleQueue.Count;
				}
				else
				{
					length = 5;
				}
			}
			else if( m_TrialEnum == TrialEnum.TrialType_QiangDuoGold)
			{
				if(FinalTrialMgr.GetInstance().QiangduoRoleQueue.Count < 6)
				{
					length = FinalTrialMgr.GetInstance().QiangduoRoleQueue.Count;
				}
				else
				{
					length = 5;
				}
			}
			else
			{
				if(roles.Length < 6)length = roles.Length;
				else
				{
					length = 5;
				}
			}
		}
        else
		{
			if(roles.Length < 6)length = roles.Length;
            else
            {
				length = 5;
			}
		}


		for(int i = 0; i < length; i++)
		{
			FightOpponentInfo foi;
			if(m_qiangduoEnum == QiangduoEnum.QiangduoEnum_List)
			{
				if(m_TrialEnum == TrialEnum.TrialType_TianXiaDiYi)foi = FinalTrialMgr.GetInstance().TianXiaRoleQueue.Dequeue(); 
				else if(m_TrialEnum == TrialEnum.TrialType_QiangDuoGold)foi = FinalTrialMgr.GetInstance().QiangduoRoleQueue.Dequeue(); 
				else
				{
					foi = roles[i]; 
				}
			}
			else
			{
				foi = roles[i]; 
			}

     		GameObject fightCellObj = Instantiate(fightCellPrb) as GameObject;
			FightCell fightCell = fightCellObj.GetComponent<FightCell>();
			
			ListCell.Add(fightCell);
			
			fightCell.gameObject.transform.parent = table.transform;
			fightCell.transform.localScale = Vector3.one;

			if(m_TrialEnum == TrialEnum.TrialType_TianXiaDiYi)
			{
				fightCell.mOkBtn.isEnabled = true;
			}

			fightCell.nameLabel.text = foi.n;
			if( foi.r <= 0)fightCell.rankLabel.text = "???";
			else fightCell.rankLabel.text = Core.Data.stringManager.getString(6005) + foi.r.ToString();

			fightCell.levelLabel.text = "Lv:" + foi.lv.ToString();

			if(Core.Data.playerManager.RTData.headID == 0)AtlasMgr.mInstance.SetHeadSprite(fightCell.mSelfHead, "10103");
			else
			{
				AtlasMgr.mInstance.SetHeadSprite(fightCell.mSelfHead, Core.Data.playerManager.RTData.headID.ToString());
			}


			if(Core.Data.playerManager.curVipLv <= 0)
			{
				fightCell.mVipLevel.text = "";
				fightCell.mCircleBg.spriteName = "main-1005";
			}
			else
			{
				fightCell.mVipLevel.text = Core.Data.playerManager.curVipLv.ToString();
				if(Core.Data.playerManager.curVipLv > 0 && Core.Data.playerManager.curVipLv < 4)
				{
					fightCell.mCircleBg.spriteName = "starvip1";
					fightCell.mVip.spriteName = "common-2008";
				}
				else if(Core.Data.playerManager.curVipLv > 3 && Core.Data.playerManager.curVipLv < 8)
				{
					fightCell.mCircleBg.spriteName = "starvip2";
					fightCell.mVip.spriteName = "common-2009";
                }
				else if(Core.Data.playerManager.curVipLv > 7)
                {
					fightCell.mCircleBg.spriteName = "starvip3";
					fightCell.mVip.spriteName = "common-2007";
                }
            }
			fightCell.info2.text = "";
			fightCell.info.text = "";
            string infoStr = "";
			
			if(m_qiangduoEnum == QiangduoEnum.QiangduoEnum_List)
			{
				if(m_TrialEnum == TrialEnum.TrialType_TianXiaDiYi)
				{
					infoStr = Core.Data.stringManager.getString(6010);
					fightCell.info.text = infoStr;
					fightCell.info.gameObject.SetActive(true);
					fightCell.m_EveryMinZhangong.SafeText("+ "+foi.izg.ToString());
					fightCell.buttonLabel.text = Core.Data.stringManager.getString(6000);
					fightCell.currentFightType = FightCell.FightType.TianXiaDiYi;

				}
				else if(m_TrialEnum == TrialEnum.TrialType_QiangDuoGold)
				{
					/* ［0～50000）金币：一贫如洗
						［50000～100000）：不值一抢
						［100000～150000）:尚可一抢 
					      150000 up   富可敌国*/

					int res = foi.c;
					if(res > 200000)
					{
						res = 200000;
					}

					if(res < 50000 )
					{
						infoStr = Core.Data.stringManager.getString(6030);
					}
					else if(res >= 50000 && res < 100000)
					{
						infoStr = Core.Data.stringManager.getString(6031);
					}
					else if(res >= 100000 && res < 150000)
					{
						infoStr = Core.Data.stringManager.getString(6032);
					}
					else
					{
						infoStr = Core.Data.stringManager.getString(20094);
					}

					fightCell.info.gameObject.SetActive(false);
					fightCell.info2.text = infoStr;
					fightCell.buttonLabel.text = Core.Data.stringManager.getString(6001);
					fightCell.currentFightType = FightCell.FightType.QiangDuoGold;
				}
				else if(m_TrialEnum == TrialEnum.TrialType_QiangDuoDragonBall)
				{
					fightCell.currentFightType = FightCell.FightType.QiangDuoDragonBall;
					infoStr = Core.Data.stringManager.getString(6101) ;
	                float tRate = 0;
	                if (foi.lv > Core.Data.playerManager.RTData.curLevel) {
	                    tRate = 0.4f+ 0.05f * ( foi.lv-Core.Data.playerManager.RTData.curLevel );
	                } else if(foi.lv <= Core.Data.playerManager.RTData.curLevel){
	                    tRate = 0.4f / Mathf.Pow (1.1f, Core.Data.playerManager.RTData.curLevel-foi.lv);
	                }
	                //低
	                string sRate = "";
	                if (tRate < 0.4f) {
	                    sRate = Core.Data.stringManager.getString (6104);
	                } else if (tRate >= 0.4 && tRate < 0.7) {//中
	                    sRate = Core.Data.stringManager.getString (6103);
	                } else {//  高
	                    sRate = Core.Data.stringManager.getString (6102);
	                }
	                //                Debug.Log (" in rob ball" + sRate);
	                infoStr += sRate;
					fightCell.info2.SafeText(infoStr);
					fightCell.info.gameObject.SetActive(false);
				}

				fightCell.deleteSuDiButtonObj.SetActive(false);
            }

			else if(m_qiangduoEnum == QiangduoEnum.QiangduoEnum_Sudi)
			{
				fightCell.currentFightType = FightCell.FightType.Sudi;
				fightCell.buttonLabel.text = Core.Data.stringManager.getString(6086);
				fightCell.deleteSuDiButtonObj.SetActive(true);
				fightCell.info.gameObject.SetActive(false);
				fightCell.info2.gameObject.SetActive(false);
			}



			fightCell.fightOpponentInfo = foi;

			fightCell.index = i;

			fightCell.setRoleIcons();
		}

		table.Reposition();
		
	}

	public void OnBtnClose()
	{

        if (m_TrialEnum == TrialEnum.TrialType_TianXiaDiYi)
		{
            UIMiniPlayerController.Instance.SetActive(true);
			DeleteBattleVideoCell();
			if(m_TrialEnum == TrialEnum.TrialType_QiangDuoGold || m_TrialEnum == TrialEnum.TrialType_QiangDuoDragonBall)Core.Data.AccountMgr.SaveFinalTrial();
			DBUIController.mDBUIInstance.RefreshUserInfo ();

        }
		else if (m_TrialEnum == TrialEnum.TrialType_QiangDuoDragonBall)
        {
            UIMiniPlayerController.Instance.SetActive(false);
            if (UIShenLongManager.Instance != null)
            {
                UIShenLongManager.Instance.ShowLeftTimes();
            }

            FinalTrialMgr.GetInstance().currentQiangDuoDragonBallList.Clear();
        }

		if(FinalTrialMgr.GetInstance()._BackCallBack != null)
		{
			FinalTrialMgr.GetInstance()._BackCallBack();
			FinalTrialMgr.GetInstance()._BackCallBack = null;
			DBUIController.mDBUIInstance.RefreshUserInfo ();

		}

		if(FinalTrialMgr.GetInstance()._MissionBackCallBack != null)
		{
			FinalTrialMgr.GetInstance()._MissionBackCallBack();
			FinalTrialMgr.GetInstance()._MissionBackCallBack = null;
			DBUIController.mDBUIInstance.ShowFor2D_UI();
			DBUIController.mDBUIInstance.RefreshUserInfo ();

		}
        
		CancelInvoke("RepeatRevengePVPTime");
		CancelInvoke("RepeatRobPVPTime");
		CancelInvoke("RepeatRankPVPTime");

		FinalTrialMgr.GetInstance().m_SelectDuihuancell = null;

        Destroy(gameObject);
	}
	public void OnDestroy(){
		BattleVideoRoot = null;

		BattleVideoPanelObj= null;

		tianXiaDiYiRoot= null;

		tianXiaDiYiTable= null;

		tianXiaDiYiScrollBar= null;

		tianXiaDiYiScrollView= null;

		SuDiRoot= null;    

		DuiHuaRoot= null;

		DuiHuanPanelObj= null;

		duiHuanButton= null;

		tianXiaDiYiPanelobj= null;

		SuDiPanelobj= null;
	
		suDiTotal= null; //宿敌上限值

		suDiCount= null; //宿敌总数量

		suDiTable= null;

		suDiPageLabel= null;

		suDiUpPageButton= null;

		suDiDownPageButton= null;

		shuaXinBackgroundSprite= null;

		ShuaXinObj= null;

		shuaXinButton= null; // 右上角刷新按扭引用

		actionButtonLabel= null;

		suDiButtonLabel= null;

		duiHuanButtonLabel= null;


		infoBackgroundObj= null;

		tianXiaDiYiCurrentRankLabel= null;



		tianXiaDiYiFreeNameTitleLabel= null;
		tianXiaDiYiTiaoZhanCountLabel= null;
		tianXiaDiYiGloryInfoLabel= null;

		fightCellPrb= null;

		duiHuanCellPrb= null;

		duiHuanTable = null;
		

		duiHuanZhanGong= null;

		duiHuanZhanGongLabel= null;

		qiangDuoGoldRoot= null;

		qiangDuoGold= null;

		qiangDuoGoldCount= null;


		qiangDuoScrollBar= null;

		qiangDuoDragonBallRoot= null;
		qiangDuoDragonBallScrollBar= null;

		DuihuanScrollBar= null;

		BtnHuifang= null;


		qiangDuoToggle= null;
		suDiToggle= null;
		duiHuanToggle= null;
		huifangToggle= null;
		ListCell = null;

	

	}
	void OnBtnQiangDuo()
	{
		FinalTrialMgr.GetInstance().m_QiangduoEnum = QiangduoEnum.QiangduoEnum_List;
		if (m_TrialEnum == TrialEnum.TrialType_QiangDuoGold)
        {
			FinalTrialMgr.GetInstance().NowEnum = TrialEnum.TrialType_QiangDuoGold;
            FinalTrialMgr.GetInstance().RequestByQiangduoType(QiangduoEnum.QiangduoEnum_List);
        } 
		else if (m_TrialEnum == TrialEnum.TrialType_QiangDuoDragonBall)
        {
			FinalTrialMgr.GetInstance().NowEnum = TrialEnum.TrialType_QiangDuoDragonBall;
            Core.Data.dragonManager.getQiangDuoDragonBallOpponentsRequest(Core.Data.temper.DragonballNum);
        }
		else if(m_TrialEnum == TrialEnum.TrialType_TianXiaDiYi)
		{
			FinalTrialMgr.GetInstance().NowEnum = TrialEnum.TrialType_TianXiaDiYi;
			ChangeShowType (QiangduoEnum.QiangduoEnum_List);
		}

	}
    #region
    //点击宿敌执行。。。。figh
	void OnBtnSuDi()
	{
        FinalTrialMgr.GetInstance().m_QiangduoEnum = QiangduoEnum.QiangduoEnum_Sudi;
		CancelTime();
		FinalTrialMgr.GetInstance().RequestByQiangduoType(QiangduoEnum.QiangduoEnum_Sudi);

	}

    #endregion 

	public void OnBtnDuiHuan()
	{
		FinalTrialMgr.GetInstance().m_QiangduoEnum = QiangduoEnum.QiangduoEnum_Duihuan;
		CancelTime();
		if(m_TrialEnum == TrialEnum.TrialType_TianXiaDiYi)
		{
			FinalTrialMgr.GetInstance().RequestByQiangduoType(QiangduoEnum.QiangduoEnum_Duihuan);
		}
		else if(m_TrialEnum == TrialEnum.TrialType_QiangDuoDragonBall)
		{
			FinalTrialMgr.GetInstance().RequestByQiangduoType(QiangduoEnum.QiangduoEnum_Playback);
		}
		else if(m_TrialEnum == TrialEnum.TrialType_QiangDuoGold)
		{
			FinalTrialMgr.GetInstance().RequestByQiangduoType(QiangduoEnum.QiangduoEnum_Playback);
		}
	}

	void OnBtnBattleVideo()
	{
		FinalTrialMgr.GetInstance().m_QiangduoEnum = QiangduoEnum.QiangduoEnum_Playback;
		CancelTime();
		FinalTrialMgr.GetInstance().RequestByQiangduoType(QiangduoEnum.QiangduoEnum_Playback);
	}

	void Introduction()
	{
		if(m_TrialEnum == TrialEnum.TrialType_TianXiaDiYi)
		{
			UIRobIntroduct.OpenUI(true);
		}
		else if(m_TrialEnum == TrialEnum.TrialType_QiangDuoGold)
		{
			UIRobIntroduct.OpenUI(false);
		}

    }

	public void RefreshZhangongShop()
	{
		string tStr = "";
		if(FinalTrialMgr.GetInstance().m_ZhangongRefreshMoneyType == 0)tStr = string.Format(Core.Data.stringManager.getString(7394),FinalTrialMgr.GetInstance().m_RefreshZhangongMoney.ToString(), Core.Data.stringManager.getString(5070));
		else if(FinalTrialMgr.GetInstance().m_ZhangongRefreshMoneyType == 1)tStr = string.Format(Core.Data.stringManager.getString(7394),FinalTrialMgr.GetInstance().m_RefreshZhangongMoney.ToString(), Core.Data.stringManager.getString(5037));
		UIInformation.GetInstance ().SetInformation (tStr,Core.Data.stringManager.getString (5030),RefreshZhangongRequest,null);

	}

	void RefreshZhangongRequest()
	{
		if(FinalTrialMgr.GetInstance().m_ZhangongRefreshMoneyType == 0)
		{			
			if(Core.Data.playerManager.RTData.curStone < FinalTrialMgr.GetInstance().m_RefreshZhangongMoney)
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(35006));
				return;
			}
		}
		else if(FinalTrialMgr.GetInstance().m_ZhangongRefreshMoneyType == 1)
		{			
			if(Core.Data.playerManager.RTData.curCoin < FinalTrialMgr.GetInstance().m_RefreshZhangongMoney)
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(35000));
                return;
            }
        }
		if(FinalTrialMgr.GetInstance().m_ZhangongsurplusRefreshTimes <= 0)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(25172));
            return;
        }
        if(m_TrialEnum == TrialEnum.TrialType_TianXiaDiYi)
		{
			FinalTrialMgr.GetInstance().RefreshZhanGongShop();
        }
    }
    
    // 点击刷新按扭处理函数
    public void OnShuaXin()
	{
		if(m_qiangduoEnum == QiangduoEnum.QiangduoEnum_Duihuan)
		{
			DBUIController.mDBUIInstance.SetViewState(RUIType.EMViewState.S_Bag, RUIType.EMBoxType.LOOK_Props);
			
			EnterfromQiangduo = true;
        }
		if (m_qiangduoEnum == QiangduoEnum.QiangduoEnum_Sudi)
		{
            ComLoading.Open();
            //刷新
            if (FinalTrialMgr.GetInstance().sudiDataQueue.Count == 0)
            {
				FinalTrialMgr.GetInstance().RequestByQiangduoType(QiangduoEnum.QiangduoEnum_Sudi);
            } else
            {
                //设置Cell 
                ShowFightCell(suDiTable, getSuDiData(5));
            }

            ComLoading.Close();
        }
		else if(m_qiangduoEnum == QiangduoEnum.QiangduoEnum_List)
        {
            if(m_TrialEnum == TrialEnum.TrialType_TianXiaDiYi)
			{
				if(FinalTrialMgr.GetInstance().TianXiaRoleQueue.Count <= 0)
				{
					changeShuaXinButtonEnabled(false);
					FinalTrialMgr.GetInstance().getTianXiaDiYiOpponentsCompletedDelegate = getTianXiaDiYiOpponentsCompleted;
					ComLoading.Open();
					FinalTrialMgr.GetInstance().RequestByQiangduoType(QiangduoEnum.QiangduoEnum_List);
				}
				else
				{
					ShowFightCell(tianXiaDiYiTable, FinalTrialMgr.GetInstance().tianXiaDiYiInfo.roles);
                }
                    
            }
                
			else if(m_TrialEnum == TrialEnum.TrialType_QiangDuoDragonBall)
            {
				SetTime();
				Core.Data.dragonManager.getQiangDuoDragonBallOpponentsRequest(Core.Data.temper.DragonballNum);
			}
			else if(m_TrialEnum == TrialEnum.TrialType_QiangDuoGold)
			{
				SetTime();
				FinalTrialMgr.GetInstance().RequestByQiangduoType(QiangduoEnum.QiangduoEnum_List);
			}
		}

	}

	void SetTime()
	{
		_TimeCount = 10;
		shuaXinButton.isEnabled = false;
		shuaXinButton.Text = "";
		InvokeRepeating("BeginTime", 0f, 1f);
	}

	void BeginTime()
	{
		_TimeCount -- ;
		if(_TimeCount >= 10)
		{
			TimeLabel.text = "00:" + _TimeCount.ToString();
		}
		else
		{
			TimeLabel.text = "00:0" + _TimeCount.ToString();
		}

		if(_TimeCount <=0 )
		{
			CancelTime();
		}
	}

	void CancelTime()
	{
		TimeLabel.text = "";
		_TimeCount = 0;
		shuaXinButton.isEnabled = true;
		shuaXinButton.Text = Core.Data.stringManager.getString(6004);
		CancelInvoke("BeginTime");
	}

	void getTianXiaDiYiOpponentsCompleted(TianXiaDiYiInfo tianXiaDiYiInfo)
	{
		FinalTrialMgr.GetInstance().getTianXiaDiYiOpponentsCompletedDelegate = null;
		changeShuaXinButtonEnabled(true);
	}

	public void changeShuaXinButtonEnabled(bool isEnabled)
	{
		this.shuaXinButton.enabled = isEnabled;
		this.shuaXinButton.isEnabled = isEnabled;
	}

	public void SetShuaxinBtnName(bool key)
	{
		if(key )
		{
			shuaXinButton.Text = Core.Data.stringManager.getString(6004);
		}
		else
		{
			shuaXinButton.Text = Core.Data.stringManager.getString(20077);
		}
	}

	public static QiangDuoPanelScript CreateQiangDuoPanel()
	{
		UnityEngine.Object obj = PrefabLoader.loadFromPack("GX/pbQiangDuoPanel");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			QiangDuoPanelScript cc = go.GetComponent<QiangDuoPanelScript>();
			return cc;
		}
		return null;
	}

    public static UITrailAddAttribute CreateShaLuPanel()
    {
        UnityEngine.Object obj = PrefabLoader.loadFromPack("LS/pbLSPrepareEnter_AttributeAdd");
        if(obj != null)
        {
            GameObject go = Instantiate(obj) as GameObject;
            UITrailAddAttribute cc = go.GetComponent<UITrailAddAttribute>();
            return cc;
        }
        return null;
    }

	public void MoveToTarget()
	{
		MiniItween.MoveTo(duiHuanTable.gameObject,new Vector3(duiHuanTable.gameObject.transform.localPosition.x,436,duiHuanTable.gameObject.transform.localPosition.z),0.2f);
	}

	#region PVP倒计时部分  1 ball 2 rank 3 rob 4 revenge
	public void SetBallPVPTimeLab(long m_cooltime)
	{
		if(m_cooltime > 0)
		{
			InvokeRepeating("RepeatBallPVPTime",0f,1f);
			lbl_RobDragonBallTimes.gameObject.SetActive(false);
			lbl_RobDragonBallTimesTitle.gameObject.SetActive(false);
		}
		else
		{
			lbl_RobDragonBallTimes.SafeText("");
            lbl_RobDragonBallTimes.gameObject.SetActive(true);
			lbl_RobDragonBallTimesTitle.gameObject.SetActive(true);
			m_BallLeftTimeLab.SafeText("");
		}
	}
	
	void RepeatBallPVPTime()
	{
		int l = 0;
		string output = "";
		output += (m_BallPvpLeftTime/3600).ToString("d2");
		l = (int)m_BallPvpLeftTime % 3600;
		output += ":" + (l / 60).ToString ("d2");
		l = (int)l % 60;
		output+=":"+l.ToString("d2");
		
		m_BallLeftTimeLab.SafeText(output);
		
		m_BallPvpLeftTime--;
		if(m_BallPvpLeftTime <= 0)
		{
			CancelInvoke("RepeatBallPVPTime");

        }
    }
    
    public void SetRankPVPTimeLab(long m_cooltime)
	{
		if(m_cooltime > 0)
		{
			InvokeRepeating("RepeatRankPVPTime",0f,1f);
			tianXiaDiYiTiaoZhanCountLabel.gameObject.SetActive(false);
			tianXiaDiYiFreeNameTitleLabel.gameObject.SetActive(false);
		}
		else
		{
			tianXiaDiYiTiaoZhanCountLabel.SafeText("");
            tianXiaDiYiTiaoZhanCountLabel.gameObject.SetActive(true);
			tianXiaDiYiFreeNameTitleLabel.gameObject.SetActive(true);
			m_RankLeftTimeLab.SafeText("");
		}
	}

	void RepeatRankPVPTime()
	{
		int l = 0;
		string output = "";
		output += (m_RankPvpLeftTime/3600).ToString("d2");
		l = (int)m_RankPvpLeftTime % 3600;
		output += ":" + (l / 60).ToString ("d2");
		l = (int)l % 60;
		output+=":"+l.ToString("d2");
		
		m_RankLeftTimeLab.SafeText(output);
		
		m_RankPvpLeftTime--;
		if(m_RankPvpLeftTime <= 0)
		{
			CancelInvoke("RepeatRankPVPTime");

        }
	}

	public void SetRobPVPTimeLab(long m_cooltime)
	{
		if(m_cooltime > 0)
		{
			InvokeRepeating("RepeatRobPVPTime",0f,1f);
			qiangDuoCount.gameObject.SetActive(false);
			qiangDuoTitleName.text = Core.Data.stringManager.getString (6007);
			qiangDuoTitleName.gameObject.SetActive(false);

		}
		else
		{
			qiangDuoCount.SafeText("");
            qiangDuoCount.gameObject.SetActive(true);
			qiangDuoTitleName.text = Core.Data.stringManager.getString (6007);
			qiangDuoTitleName.gameObject.SetActive(true);
			m_RobLeftTimeLab.SafeText("");
		}
	}
	
	void RepeatRobPVPTime()
	{
		int l = 0;
		string output = "";
		output += (m_RobPvpLeftTime/3600).ToString("d2");
		l = (int)m_RobPvpLeftTime % 3600;
		output += ":" + (l / 60).ToString ("d2");
		l = (int)l % 60;
		output+=":"+l.ToString("d2");
		
		m_RobLeftTimeLab.SafeText(output);
		
		m_RobPvpLeftTime--;
		if(m_RobPvpLeftTime <= 0)
		{
			CancelInvoke("RepeatRobPVPTime");
        }
	}

	public void SetRevengePVPTimeLab(long m_cooltime)
	{
		if(m_cooltime > 0)
		{
			InvokeRepeating("RepeatRevengePVPTime",0f,1f);
			suDiCount.gameObject.SetActive(false);
			suDiNameTitle.gameObject.SetActive(false);
		}
		else
		{
			suDiCount.SafeText("");
            suDiCount.gameObject.SetActive(true);
			suDiNameTitle.gameObject.SetActive(true);
			m_RevengeLeftTimeLab.SafeText("");
		}
	}

	void RepeatRevengePVPTime()
	{
		int l = 0;
		string output = "";
		output += (m_RevengePvpLeftTime/3600).ToString("d2");
		l = (int)m_RevengePvpLeftTime % 3600;
		output += ":" + (l / 60).ToString ("d2");
		l = (int)l % 60;
		output+=":"+l.ToString("d2");
		
		m_RevengeLeftTimeLab.SafeText(output);
		
		m_RevengePvpLeftTime--;
		if(m_RevengePvpLeftTime <= 0)
		{
			CancelInvoke("RepeatRevengePVPTime");
		}
	}
	#endregion


}
