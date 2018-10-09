using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class FightCell : MonoBehaviour 
{
	public UILabel nameLabel;

	public UILabel levelLabel;

	public UILabel rankLabel;

	public UILabel needPower;

	public List<RoleIcon> roleIconList = new List<RoleIcon>();

    public UILabel buttonLabel;
	
	public GameObject deleteSuDiButtonObj;

//	public QiangDuoPanelScript.ShowType showType = QiangDuoPanelScript.ShowType.Unknow;

	public string gid;

	public UILabel info;
	public UILabel info2;
	public UILabel m_EveryMinZhangong;

	public FightOpponentInfo fightOpponentInfo;

	public int index;

	public UISprite mSelfHead;
	public UISprite mVip;
	public UISprite mCircleBg;
	public UILabel mVipLevel;

	public UIButton mOkBtn;

	public GameObject m_NeedMoneyObj;
	public UILabel m_NeedMoneyLab;

	private FinalTrialMgr m_TrialMgr
	{
		get{
			return FinalTrialMgr.GetInstance();
		}
	}

    private int limitGambleLevel = 10;

	int buytime = 0;
	int time = 0;
	int stone = 0;
	// private string stoneColor = "[FFF300]";

    public enum FightType
	{
		TianXiaDiYi,
		QiangDuoGold,
		QiangDuoDragonBall,
		Sudi,
	};

	public FightType currentFightType;


	public void setRoleIcons()
	{

		int index = 0;
		foreach(RoleIcon roleIcon in roleIconList)
		{
			if(fightOpponentInfo.p == null)
			{
				roleIcon.gameObject.SetActive(false);
			}
			else if(index >= fightOpponentInfo.p.Length)
			{
				roleIcon.gameObject.SetActive(false);
			}
			else
			{
				int roleID = fightOpponentInfo.p[index].num;
				roleIcon.gameObject.SetActive(true);
				AtlasMgr.mInstance.SetHeadSprite(roleIcon.headIcon, roleID.ToString());
				try {
					int temp = Core.Data.monManager.getMonsterByNum(roleID).star;
					int starCount = temp + fightOpponentInfo.p[index].star;
					roleIcon.setStars(starCount);
				} catch(Exception ex) {
					Debug.Log(ex.ToString());
				}
			}
			index++;
		}

		if(fightOpponentInfo.hi == 0)AtlasMgr.mInstance.SetHeadSprite(mSelfHead, "10100");
		else 
		{
			AtlasMgr.mInstance.SetHeadSprite(mSelfHead, fightOpponentInfo.hi.ToString());
        }


		mVipLevel.text = fightOpponentInfo.vipLv.ToString();
		CheckPvpVipTitle();
		CheckVipStone();
		SetVipDetail();
		SetFightcellBtnStatus();
	}

	void SetFightcellBtnStatus()
	{
		if(m_TrialMgr.m_QiangduoEnum == QiangduoEnum.QiangduoEnum_List)
		{
			if(m_TrialMgr.NowEnum == TrialEnum.TrialType_TianXiaDiYi)
			{
				if(m_TrialMgr.allPVPRobData.pvpStatus.rank.count - m_TrialMgr.allPVPRobData.pvpStatus.rank.passCount <= 0)
				{
					SetBtnType(true);
				}
				else
				{
					SetBtnType(false);
                }
			}
			else if(m_TrialMgr.NowEnum == TrialEnum.TrialType_QiangDuoGold)
			{
				if(m_TrialMgr.allPVPRobData.pvpStatus.robs != null)
				{
					if(m_TrialMgr.allPVPRobData.pvpStatus.robs.count - m_TrialMgr.allPVPRobData.pvpStatus.robs.passCount <= 0)
					{
						SetBtnType(true);
					}
					else
					{
						SetBtnType(false);
					}
				}

            }
			else if(m_TrialMgr.NowEnum == TrialEnum.TrialType_QiangDuoDragonBall)
			{
				if(m_TrialMgr.allPVPRobData.pvpStatus.ball.count - m_TrialMgr.allPVPRobData.pvpStatus.ball.passCount <= 0)
				{
					SetBtnType(true);
				}
				else
				{
					SetBtnType(false);
                }
            }
        }
		else if(m_TrialMgr.m_QiangduoEnum == QiangduoEnum.QiangduoEnum_Sudi)
		{
			if(m_TrialMgr.allPVPRobData.pvpStatus.revenge.count - m_TrialMgr.allPVPRobData.pvpStatus.revenge.passCount <= 0)
			{
				SetBtnType(true);
			}
			else
			{
				SetBtnType(false);
            }
        }
    }

	void SetBtnType(bool _key)
	{
		if(_key)
		{
			m_NeedMoneyObj.gameObject.SetActive(true);
			m_NeedMoneyLab.SafeText(stone.ToString());
			buttonLabel.gameObject.SetActive(false);
        }
		else
		{
			m_NeedMoneyObj.gameObject.SetActive(false);
			buttonLabel.gameObject.SetActive(true);
        }
		SetBtnEnable();
    }

	void SetBtnEnable()
	{
//		if(Core.Data.guideManger.isGuiding)
//		{
//			mOkBtn.isEnabled = true;
//		}
//		else
//		{
//			if(buytime >= time)mOkBtn.isEnabled = false;
//			else mOkBtn.isEnabled = true;
//		}

	}
    
    void SetVipDetail()
    {
        if(fightOpponentInfo.vipLv < 4)
		{
			mCircleBg.spriteName = "starvip1";
			mVip.spriteName = "common-2008";
		}
		else if(fightOpponentInfo.vipLv > 3 && fightOpponentInfo.vipLv < 8)
		{
			mCircleBg.spriteName = "starvip2";
			mVip.spriteName = "common-2009";
		}
		else if(fightOpponentInfo.vipLv > 7 && fightOpponentInfo.vipLv < 12)
		{
			mCircleBg.spriteName = "starvip3";
			mVip.spriteName = "common-2007";
        }
		else
		{
			mCircleBg.spriteName = "star0";
			mVip.spriteName = "common-2109";
		}
    }
    
	public void onFight()
	{

		if(nameLabel != null)Core.Data.temper._PvpEnemyName = nameLabel.text;
		

		if(currentFightType == FightType.TianXiaDiYi)
		{
//			if(FinalTrialMgr.GetInstance().tianXiaDiYiInfo.aktms >= Core.Data.vipManager.GetVipInfoData(Core.Data.playerManager.RTData.curVipLevel).freeChallenges)
//			{
//				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(6036));
//				return;
//			}

			ComLoading.Open();
			m_TrialMgr.currentFightOpponentInfo = fightOpponentInfo;
			m_TrialMgr.tianXiaDiYiFightRequest(fightOpponentInfo.g, fightOpponentInfo.r, RUIType.EMViewState.S_QiangDuo,Core.Data.temper.gambleTypeId);
		}
		else if(currentFightType == FightType.QiangDuoGold)
		{
			ComLoading.Open();
			m_TrialMgr.currentFightOpponentInfo = fightOpponentInfo;
			m_TrialMgr.qiangDuoGoldFightRequest(fightOpponentInfo.g,fightOpponentInfo.r, RUIType.EMViewState.S_QiangDuo ,Core.Data.temper.gambleTypeId);
		}
		else if(currentFightType == FightType.Sudi)
		{
			ComLoading.Open();
			m_TrialMgr.currentFightOpponentInfo = fightOpponentInfo;
			m_TrialMgr.qiangDuoGoldFightRequest(fightOpponentInfo.g,fightOpponentInfo.r, RUIType.EMViewState.S_QiangDuo ,Core.Data.temper.gambleTypeId,1);
		}
		else if(currentFightType == FightType.QiangDuoDragonBall)
		{
			if(Core.Data.dragonManager.mianZhanTime != 0)
			{
				UIInformation.GetInstance().SetInformation(
				Core.Data.stringManager.getString(6114),
				Core.Data.stringManager.getString(6120),
				qiangDuoDragonBallFight);
			}
			else
			{
				qiangDuoDragonBallFight();
			}
		}

	}
	//主要的 战斗方法 按钮
    void OnFightAddGambleAndBuyTimes(){


        StringBuilder tstrBuilder = new StringBuilder("");
		if(m_TrialMgr.allPVPRobData == null || m_TrialMgr.allPVPRobData.pvpStatus == null)return;
		int tNeedStone = 0;
        if(currentFightType == FightType.TianXiaDiYi)
        {
			if(m_TrialMgr.allPVPRobData.pvpStatus.rank != null)
			{
				if (m_TrialMgr.allPVPRobData.pvpStatus.rank.count - m_TrialMgr.allPVPRobData.pvpStatus.rank.passCount <= 0)
				{
					tNeedStone = m_TrialMgr.allPVPRobData.pvpStatus.rank.needStone;
				}
				else
				{
					OnFightAddGamble();
					return;
				}
			}

        }else if(currentFightType == FightType.QiangDuoGold)
        {
			if(m_TrialMgr.allPVPRobData.pvpStatus.robs != null)
			{
				if (m_TrialMgr.allPVPRobData.pvpStatus.robs.count - m_TrialMgr.allPVPRobData.pvpStatus.robs.passCount <= 0)
	            {
					tNeedStone = m_TrialMgr.allPVPRobData.pvpStatus.robs.needStone;
	                //UIDragonBallBuyNum.OpenUI(string.Format(Core.Data.stringManager.getString(25152), FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.robs.needStone));
	            }
	            else
	            {
	                OnFightAddGamble();
	                return;
	            }
			}
        }else if(currentFightType == FightType.QiangDuoDragonBall)
        {
			if(m_TrialMgr.allPVPRobData.pvpStatus.ball != null)
			{
				if (m_TrialMgr.allPVPRobData.pvpStatus.ball.count - m_TrialMgr.allPVPRobData.pvpStatus.ball.passCount <= 0)
				{
					tNeedStone = m_TrialMgr.allPVPRobData.pvpStatus.ball.needStone;
					//   UIDragonBallBuyNum.OpenUI(string.Format(Core.Data.stringManager.getString(25152), FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.ball.needStone));
					//  return;
				}
				else
				{
					OnFightAddGamble();
					return;
				}
			}

        }
		else if(currentFightType == FightType.Sudi)
		{
			if(m_TrialMgr.allPVPRobData.pvpStatus.revenge != null)
			{
				if (m_TrialMgr.allPVPRobData.pvpStatus.revenge.count - m_TrialMgr.allPVPRobData.pvpStatus.revenge.passCount <= 0)
				{
					tNeedStone = m_TrialMgr.allPVPRobData.pvpStatus.revenge.needStone;
					//   UIDragonBallBuyNum.OpenUI(string.Format(Core.Data.stringManager.getString(25152), FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.ball.needStone));
					//  return;
				}
				else
				{
					onFight();
					return;
				}
			}

		}

        if (Core.Data.playerManager.RTData.curStone < tNeedStone)
        {
            SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(7310),null);
            return;
        }

		StringBuilder colorBuilder = new StringBuilder("");
        colorBuilder.Append(tNeedStone.ToString());
				//   colorBuilder.Append("[-]");
        tstrBuilder.Append(string.Format(Core.Data.stringManager.getString(25152), colorBuilder.ToString()));
		string _temp1 = Core.Data.stringManager.getString(25164) + " " + buytime.ToString() + "/" + time.ToString();
		string _temp2 = string.Format(Core.Data.stringManager.getString(25165) , stone.ToString());

		JCPromptBox.OpenUI(_temp1, _temp2).OnBtnBuyClick = CheckPvpBuy;
//			UIDragonBallBuyNum.OpenUI(tstrBuilder.ToString(),OnFightAddGamble);

    }

	void CheckPvpBuy()
	{
		if(buytime >= time)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(25166));
			JCPromptBox.Close();
			return;
		}
		if(Core.Data.playerManager.Stone < stone)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(7310));
			JCPromptBox.Close();
			return;
		}
		OnFightAddGamble();
		JCPromptBox.Close();
	}

	void CheckPvpVipTitle()
	{
		VipInfoData info = Core.Data.vipManager.GetVipInfoData(Core.Data.playerManager.curVipLv);
		if(info != null && m_TrialMgr.allPVPRobData != null && m_TrialMgr.allPVPRobData.pvpStatus != null)
		{
			if(m_TrialMgr.m_QiangduoEnum  == QiangduoEnum.QiangduoEnum_List)
			{
				if(m_TrialMgr.NowEnum == TrialEnum.TrialType_TianXiaDiYi)
				{
					time = info.pvptype2;
					if(m_TrialMgr.allPVPRobData.pvpStatus.rank != null)buytime = m_TrialMgr.allPVPRobData.pvpStatus.rank.buyTime;
				}
				else if(m_TrialMgr.NowEnum == TrialEnum.TrialType_QiangDuoDragonBall)
				{
					time = info.pvptype1;
					if(m_TrialMgr.allPVPRobData.pvpStatus.ball != null)buytime = m_TrialMgr.allPVPRobData.pvpStatus.ball.buyTime;
				}
				else if(m_TrialMgr.NowEnum == TrialEnum.TrialType_QiangDuoGold)
				{
					time = info.pvptype3;
					if(m_TrialMgr.allPVPRobData.pvpStatus.robs != null)buytime = m_TrialMgr.allPVPRobData.pvpStatus.robs.buyTime;
				}
			}
			else if(m_TrialMgr.m_QiangduoEnum == QiangduoEnum.QiangduoEnum_Sudi)
			{
				time = info.pvptype4;
				if(m_TrialMgr.allPVPRobData.pvpStatus.revenge != null)buytime = m_TrialMgr.allPVPRobData.pvpStatus.revenge.buyTime;	
			}
			
		}
	}

	void CheckVipStone()
	{
		VipInfoData info = Core.Data.vipManager.GetVipInfoData(Core.Data.playerManager.curVipLv);
		if(info != null && m_TrialMgr.allPVPRobData != null && m_TrialMgr.allPVPRobData.pvpStatus != null)
		{
			if(m_TrialMgr.m_QiangduoEnum  == QiangduoEnum.QiangduoEnum_List)
			{
				if(m_TrialMgr.NowEnum == TrialEnum.TrialType_TianXiaDiYi)
				{
					if(m_TrialMgr.allPVPRobData.pvpStatus.rank != null)stone = m_TrialMgr.allPVPRobData.pvpStatus.rank.needStone;
				}
				else if(m_TrialMgr.NowEnum == TrialEnum.TrialType_QiangDuoDragonBall)
				{
					if(m_TrialMgr.allPVPRobData.pvpStatus.ball != null)stone = m_TrialMgr.allPVPRobData.pvpStatus.ball.needStone;
				}
				else if(m_TrialMgr.NowEnum == TrialEnum.TrialType_QiangDuoGold)
				{
					if(m_TrialMgr.allPVPRobData.pvpStatus.robs != null)stone = m_TrialMgr.allPVPRobData.pvpStatus.robs.needStone;
				}
			}
			else if(m_TrialMgr.m_QiangduoEnum == QiangduoEnum.QiangduoEnum_Sudi)
			{
				if(m_TrialMgr.allPVPRobData.pvpStatus.revenge != null)stone = m_TrialMgr.allPVPRobData.pvpStatus.revenge.needStone;
			}

		}
	}



    //add by wxl 
    void OnFightAddGamble()
	{
		

        if (Core.Data.playerManager.Lv < limitGambleLevel || Core.Data.guideManger.isGuiding  ) {
            onFight ();
            return;
        }


		if(nameLabel != null)Core.Data.temper._PvpEnemyName = nameLabel.text;
//		if(currentFightType == FightType.TianXiaDiYi)
//		{
//			if((FinalTrialMgr.GetInstance().tianXiaDiYiInfo.totalcount - FinalTrialMgr.GetInstance().tianXiaDiYiInfo.yetcount)<=0)
//			{
//				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(25116));
//				return;
//			}
//		}

		if (currentFightType == FightType.QiangDuoGold || currentFightType == FightType.TianXiaDiYi || currentFightType == FightType.Sudi) {
            this.OnSetCreatGamblePanel (onFight);

        } else {
            if(Core.Data.dragonManager.mianZhanTime != 0)
            {
                UIInformation.GetInstance().SetInformation(
                    Core.Data.stringManager.getString(6114),
                    Core.Data.stringManager.getString(6120),
                    qiangDuoDragonBallFight);
            }
            else
            {

                qiangDuoDragonBallFight();
            }
        }
    }

    void OnSetCreatGamblePanel(System.Action finalMethod = null){
		ComLoading.Open ();
        ActivityNetController.GetInstance ().GetGambleStateList (finalMethod);
        //UIGambleController.CreateGamblePanel ();
    }



	void qiangDuoDragonBallFight()
	{
		if(fightOpponentInfo.htm >= 3)
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(6055));
			return;
		}
		Core.TimerEng.deleteTask(TaskID.DragonMianZhanTimer);
		Core.Data.dragonManager.mianZhanTime = 0;
		ComLoading.Open();
        //wxl
        if (Core.Data.playerManager.Lv < limitGambleLevel || Core.Data.guideManger.isGuiding) {
            this.RobBallBattleRequest ();
        } else {
            this.OnSetCreatGamblePanel (RobBallBattleRequest);
            // UIGambleController.CreateGamblePanel (RobBallBattleRequest);
        }

//		FinalTrialMgr.GetInstance().currentFightOpponentInfo = fightOpponentInfo;
//		FinalTrialMgr.GetInstance().qiangDuoDragonBallFightRequest(fightOpponentInfo.g.ToString());
	}
    //add by wxl 
    void RobBallBattleRequest(){
        if (fightOpponentInfo != null)
        {
            FinalTrialMgr.GetInstance().currentFightOpponentInfo = fightOpponentInfo;
        //移除被抢夺龙珠的人员
     
            //    Core.Data.dragonManager.qiangDuoDragonBallFightOpponentList.Remove (fightOpponentInfo);

            FinalTrialMgr.GetInstance().qiangDuoDragonBallFightRequest(fightOpponentInfo.g.ToString(), Core.Data.temper.gambleTypeId);
            Core.Data.dragonManager.qiangDuoDragonBallFightOpponentList.Remove(fightOpponentInfo);
        }
    }
    /// <summary>
    /// 删除宿敌按扭点击处理
    /// </summary>

	public void deleteSudi()
	{

        ComLoading.Open();
      
        FinalTrialMgr.GetInstance().deleteSuDiRequest(fightOpponentInfo.g);

	}

	public void deleteSuDiCompleted()
	{
//henry edit
//        Core.Data.FriendManager.deleteSuDiCompletedDelegate = null;
//		FinalTrialMgr.GetInstance().suDiList = Core.Data.FriendManager.otherUserInfoListToFightOpponentInfoList(Core.Data.FriendManager.suDiList);
//		QiangDuoPanelScript.Instance.ShowSuDi();
	}

    void OnDestroy(){
         nameLabel=null;

         levelLabel=null;

         rankLabel=null;

         needPower=null;

        roleIconList =null;

         buttonLabel=null;

         deleteSuDiButtonObj=null;

         gid=null;

         info=null;

         fightOpponentInfo=null;



         mSelfHead=null;
         mVip=null;
         mCircleBg=null;
         mVipLevel=null;

    }
}