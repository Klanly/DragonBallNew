using UnityEngine;
using System.Collections;
using System.Text;

public class MonEvolveUI : MonoBehaviour 
{
	//标题
	public UILabel m_txtTitle;
	//战魂个数
	public UILabel m_txtBattleSoulCnt;
	//?
	public UILabel m_txtIcon;
	//tip
	public UILabel m_txtClickTip;
	//金币
	public UILabel m_txtCoin;
	//战魂
	public UISprite m_spBattleSoul;
	//进化按钮
	public UIButton m_btnEvolve;
	//说明按钮
	public UIButton m_btnDetial;
	//
	public UILabel[] m_txtTip;

	public Card3DUI[] m_3dCard;
	
	private Monster m_data;

    public UILabel lbl_FightSoul;

	void Start()
	{
		InitUI ();
	}

	void InitUI()
	{
		m_data = null;
		m_txtTitle.text = Core.Data.stringManager.getString (5052);
		for (int i = 0; i < m_3dCard.Length; i++)
		{
			m_3dCard [i].Del3DModel ();
			m_3dCard[i].InitUI();
		}
		m_btnEvolve.TextID = 5036;
		m_btnDetial.TextID = 5126;
		RED.SetActive (false, m_spBattleSoul.gameObject);
		m_btnEvolve.isEnabled = false;
		m_txtBattleSoulCnt.text = Core.Data.itemManager.GetBagItemCount (ItemManager.HIGH_BATTLE_SOUL).ToString () + "/0";
		m_txtCoin.text = "0";
		m_txtTip [0].text = Core.Data.stringManager.getString (5057);
		m_txtTip [1].text = Core.Data.stringManager.getString (5133); 
		m_txtClickTip.text = Core.Data.stringManager.getString (5203); 
        StringBuilder  tStringB = new StringBuilder("x");

		lbl_FightSoul.text = tStringB.Append(Core.Data.itemManager.GetBagItemCount(ItemManager.HIGH_BATTLE_SOUL)).ToString();
	}

	public void FreshUI()
	{
		UpdateUI(m_data);
	}

	void UpdateUI(Monster mon)
	{
		m_data = mon;
		RED.SetActive ((mon != null), m_spBattleSoul.gameObject);
		RED.SetActive ((mon != null), m_spBattleSoul.gameObject, m_txtBattleSoulCnt.gameObject);
		RED.SetActive(m_data == null, m_txtIcon.gameObject);
		m_btnEvolve.isEnabled = (mon != null && mon.Star < 6);
		m_txtCoin.text = GetCostCoin ().ToString ();

		if (m_data != null)
		{
			m_txtTip [0].text = "";
			m_txtTip [1].text = "";
		}

		lbl_FightSoul.text = Core.Data.itemManager.GetBagItemCount(ItemManager.HIGH_BATTLE_SOUL).ToString();
		m_txtBattleSoulCnt.text = Core.Data.itemManager.GetBagItemCount (ItemManager.HIGH_BATTLE_SOUL).ToString () + "/" + GetCostSoul().ToString() ;
	}

	int GetCostCoin()
	{
		if (m_data == null)
		{
			return 0;
		}
		return Core.Data.monManager.GetUpCostCoin (m_data.config.star, m_data.Star + 1);
	}

	int GetCostSoul()
	{
		int soul = 0;
		if (m_data != null)
		{
			soul = Core.Data.monManager.GetUpCostBattleSoul (m_data.config.star, m_data.Star + 1);
		}
		return soul;
	}

	//得到宠物攻击值
	int GetMonNextAtk()
	{
		if (m_data == null)
		{
			return 0;
		}

		float param = Core.Data.monManager.GetMonUpAtkParam (m_data.config.star, m_data.Star + 1);
		return (int)(m_data.getAttack * param);
	}

	//得到宠物防御值
	int GetMonNextDef()
	{
		if (m_data == null)
		{
			return 0;
		}

		float param = Core.Data.monManager.GetMonUpDefParam (m_data.config.star, m_data.Star + 1);
		return (int)(m_data.getDefend * param);
	}

	void OnClickMain()
	{
		DBUIController.mDBUIInstance.SetViewState (RUIType.EMViewState.S_Bag, RUIType.EMBoxType.EVOLVE_MONSTER);
		TrainingRoomUI.mInstance.SetShow (false);
	}

	CRLuo_ShowStage stage = null;
	public void SetSelData(Monster mon)
	{
		bool isdifferent = true;
		if(m_data!= null)
			isdifferent =  mon.config.ID == m_data.config.ID;

		if(stage != null) 
		{
			if(!isdifferent)
			{
				m_3dCard[0].Del3DModel();
				m_3dCard[1].Del3DModel();
				m_3dCard[0].mShowOne = null;
				m_3dCard[1].mShowOne = null;
				stage = null;
				Resources.UnloadUnusedAssets();
			}
		}

		m_data = mon;
		UpdateUI (mon);

		stage = m_3dCard[0].Show3DCard(mon,true,stage);

		Monster newMon = new Monster ();
		newMon.RTData = new RuntimeMonster ();
		newMon.RTData.addStar = mon.RTData.addStar + 1;
		newMon.RTData.Attribute = mon.RTData.Attribute;
		newMon.RTData.curLevel = mon.RTData.curLevel;
		newMon.config = Core.Data.monManager.getMonsterByNum (mon.num);
		newMon.num = newMon.config.ID;
		newMon.RTData.ChaKeLa_Attck = mon.RTData.ChaKeLa_Attck;
		newMon.RTData.ChaKeLa_Defend = mon.RTData.ChaKeLa_Defend;

		newMon.InitConfig();
		m_3dCard[1].Show3DCard(newMon,true,stage);
	}

	public void SetShow(bool bShow)
	{
		RED.SetActive (bShow, this.gameObject);
		if(bShow)
		{
			InitUI();
		}

	}

	void OnBtnBack()
	{
		SetShow (false);
		for (int i = 0; i < m_3dCard.Length; i++)
		{
			m_3dCard [i].Del3DModel ();
		}
		stage = null;

		InitUI ();
		UpdateUI (null);
		RED.SetActive (true, TrainingRoomUI.mInstance.m_mainTraining);

		if (TrainingRoomUI.mInstance.m_callBack != null)
		{
			TrainingRoomUI.mInstance.OnClickExit ();
		}
	}

	void OnBtnDesp()
	{
		string strText = Core.Data.stringManager.getString (5132);
		string[] strTexts = strText.Split ('\n');
		TrainingRoomUI.mInstance.m_despUI.SetText (Core.Data.stringManager.getString (5131), strTexts);
	}

	void OnBtnEvolve()
	{
		if (m_data == null)
		{
			return;
		}
			
		if (GetCostSoul () > Core.Data.itemManager.GetBagItemCount(ItemManager.HIGH_BATTLE_SOUL))
			{
				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5128));
				return;
			}
			
		if (GetCostCoin() > Core.Data.playerManager.RTData.curCoin)
		{
            JCRestoreEnergyMsg.OpenUI(ItemManager.COIN_PACKAGE,ItemManager.COIN_BOX,2);

			//SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(35000));
			return;
		}

		UIInformation.GetInstance ().SetInformation (Core.Data.stringManager.getString(5130), Core.Data.stringManager.getString(5030), SendEvolveMonMsg, null);
	}
		
	void SendEvolveMonMsg()
	{
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.EVOLVE_MONSTER, new EvolveMonsterParam(Core.Data.playerManager.PlayerID, m_data.pid));

		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;

		//then you should dispatch to a real handler
		task.DispatchToRealHandler ();
	}

	public void OnClickBtttleSoul()
	{
		DBUIController.mDBUIInstance.SetViewState (RUIType.EMViewState.S_Bag, RUIType.EMBoxType.DECOMPOSE_MONSTER);
		TrainingRoomUI.mInstance.SetShow (false);
	}

	#region 网络返回

	void testHttpResp_UI (BaseHttpRequest request, BaseResponse response)
	{
		if (response.status != BaseResponse.ERROR)
		{
			DBUIController.mDBUIInstance.RefreshUserInfo ();
			EvolveMonsterResponse resp = response as EvolveMonsterResponse;
		
			//			GetRewardSucUI.OpenUI (resp.data.p, Core.Data.stringManager.getString(5129));

//			if (TeamUI.mInstance != null)
//			{
//				TeamUI.mInstance.RefreshMonster (Core.Data.monManager.getMonsterById(m_data.pid));
//			}

			Monster mon = resp.data.p [0].toMonster (Core.Data.monManager);

//			m_3dCard [0].Del3DModel ();
//			m_3dCard [1].Del3DModel ();

			m_3dCard[0].Show3DCard(mon,true,stage);

			Monster newMon = new Monster ();
			newMon.RTData = new RuntimeMonster ();

			if (mon.Star < 6)
			{
				newMon.RTData.addStar = mon.RTData.addStar + 1;
			}
			else
			{
				newMon.RTData.addStar = mon.RTData.addStar;
			}
			newMon.RTData.Attribute = mon.RTData.Attribute;
			newMon.RTData.curLevel = mon.RTData.curLevel;
			newMon.config = Core.Data.monManager.getMonsterByNum (mon.num);
			newMon.num = newMon.config.ID;
			newMon.InitConfig();
			newMon.RTData.ChaKeLa_Attck = mon.RTData.ChaKeLa_Attck;
			newMon.RTData.ChaKeLa_Defend = mon.RTData.ChaKeLa_Defend;
			m_3dCard[1].Show3DCard(newMon);

			UpdateUI(mon);
		}
		else
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(30000 + response.errorCode));
		}
	}
		

	void testHttpResp_Error (BaseHttpRequest request, string error)
	{
		ConsoleEx.DebugLog ("---- Http Resp - Error has ocurred." + error);
		SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(5111));

	}
	#endregion
}
