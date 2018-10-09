using UnityEngine;
using System.Collections;

public class UIShareVideoTag : RUIMonoBehaviour {

	public UIInput minput;
	public UILabel mTitle;

	public GameObject Type1;
	public GameObject Type2;

	public UISprite mHead;
	public UILabel mLevel;
	public UILabel mName;
	public UILabel mAttack;
	public UILabel mDefense;

	public UISprite mEHead;
	public UILabel mELevel;
	public UILabel mEName;
	public UILabel mEAttack;
	public UILabel mEDefense;

	public UISprite mSelfWin;
	public UISprite mSelfLose;
	public UISprite mEnemyWin;
	public UISprite mEnemyLose;

	public UILabel mSelf;
	public UILabel mEnemy;
    
	public string _videoid;

	BattleResponse _BattleResponse;

	public void OnShow( string _id, string attname, string defname,  BattleVideoTagType type)
	{
		if(type == BattleVideoTagType.Type_Share)
		{
			Type1.gameObject.SetActive(true);
			Type2.gameObject.SetActive(false);

			_videoid = _id;
			
			mTitle.text = "[ " + attname + " VS " + defname + " ]";
		}
		else
		{
			Type1.gameObject.SetActive(false);
			Type2.gameObject.SetActive(true);
        }


	}

	public void OnShow(int AttOrSuff, BattleVideoTagType type, BattleVideoPlaybackData data)
	{
		int playerid = int.Parse(Core.Data.playerManager.PlayerID);
		TemporyData temp = Core.Data.temper;
		if(playerid != data.battledata.attTeam.roleId && playerid != data.battledata.defTeam.roleId)
		{
			temp.isMyBussiness = false;
		}
		else
		{
			temp.isMyBussiness = true;
		}
		temp.self_name = data.battledata.attTeam.name;
		temp.enemy_name = data.battledata.defTeam.name;

		_videoid = data.id; 
		if(type == BattleVideoTagType.Type_Share)
		{
			Type1.gameObject.SetActive(true);
			Type2.gameObject.SetActive(false);
		}
		else
		{
            Type1.gameObject.SetActive(false);
            Type2.gameObject.SetActive(true);
        }

		if(int.Parse(data.winid) == data.battledata.attTeam.roleId)
		{
			mSelfWin.gameObject.SetActive(true);
			mSelfLose.gameObject.SetActive(false);
			mEnemyWin.gameObject.SetActive(false);
			mEnemyLose.gameObject.SetActive(true);
			mSelf.text = Core.Data.stringManager.getString(25122);
			mEnemy.text = Core.Data.stringManager.getString(25123);
		}
		else
		{
			mSelfWin.gameObject.SetActive(false);
			mSelfLose.gameObject.SetActive(true);
			mEnemyWin.gameObject.SetActive(true);
			mEnemyLose.gameObject.SetActive(false);
			mSelf.text = Core.Data.stringManager.getString(25123);
			mEnemy.text = Core.Data.stringManager.getString(25122);
		}

		if(data.battledata.attTeam.headId == 0)AtlasMgr.mInstance.SetHeadSprite(mHead, "10142");
		else
		{
			AtlasMgr.mInstance.SetHeadSprite(mHead, data.battledata.attTeam.headId.ToString());
		}
		if(data.battledata.defTeam.headId == 0)AtlasMgr.mInstance.SetHeadSprite(mEHead, "10142");
		else
		{
			AtlasMgr.mInstance.SetHeadSprite(mEHead, data.battledata.defTeam.headId.ToString());
		}


		mLevel.text = data.battledata.attTeam.level.ToString();
		mName.text = data.battledata.attTeam.name.ToString();
		mAttack.text = data.battledata.attTeam.at.ToString();
		mDefense.text = data.battledata.attTeam.df.ToString();

		mELevel.text = data.battledata.defTeam.level.ToString();
		mEName.text = data.battledata.defTeam.name.ToString();
		mEAttack.text = data.battledata.defTeam.at.ToString();
		mEDefense.text = data.battledata.defTeam.df.ToString();


		setTemporyData(AttOrSuff, data);

		if(data.winid == Core.Data.playerManager.PlayerID) Core.Data.temper.PvpVideo_SelfWin = 1;
		else Core.Data.temper.PvpVideo_SelfWin = 0;


		_BattleResponse = new BattleResponse();
		_BattleResponse.data = new BattleSequence();
		_BattleResponse.data.battleData = new BattleData();
		_BattleResponse.data.battleData.rsmg = data.content.msgArr;
		_BattleResponse.data.battleData.rsty = data.content.typeArr;
		_BattleResponse.handleResponse();

    }

	/// <summary>
	/// 1代表我是攻击方，-1代表我是挨打方，0 则表示可能和自己没关系
	/// </summary>
	/// <param name="AttOrSuff">Att or suff.</param>
	/// <param name="data">Data.</param>
	void setTemporyData(int AttOrSuff, BattleVideoPlaybackData data) {
		TemporyData temp = Core.Data.temper;

		temp.PvpVideo_Self_Attack = data.battledata.attTeam.at;
		temp.PvpVideo_Enemy_Defend = data.battledata.defTeam.df;
		temp.PvpVideo_Self_Lv = data.battledata.attTeam.level;
		temp.PvpVideo_Enemy_Lv = data.battledata.defTeam.level;
		temp.PvpVideo_AttackOrDefense = 1;
	}


	void PlayWarVideo()
	{
		Core.Data.temper.warBattle = _BattleResponse.data;
		Core.Data.temper.currentBattleType = TemporyData.BattleType.PVPVideo;

		BattleToUIInfo.From = FinalTrialMgr.GetInstance()._EMViewState;
		FinalTrialMgr.GetInstance().jumpTo = FinalTrialMgr.GetInstance().NowEnum;

		Core.SM.beforeLoadLevel(Application.loadedLevelName, SceneName.GAME_BATTLE);
        AsyncLoadScene.m_Instance.LoadScene(SceneName.GAME_BATTLE);
	}

	void Share_Onclik()
	{
		if(MessageMgr.GetInstance()._NowCount <= 0)
		{

            if(Core.SM.CurScenesName == SceneName.GAME_BATTLE)
            {
                //ycg
                SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(20081),BanBattleManager.Instance.go_uiPanel);
            }else
            {
                SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(20081));
            }
			
			return;
		}

		string mContent = "";
		if(minput != null)
		{
			mContent = "#$&" + _videoid + "{{@}}" + mTitle.text + "#$&" + minput.value;

			MessageMgr.GetInstance().SendWorldChat2(mContent);

			minput.value = "";
		}

		Destroy(gameObject);
	}

	void OnDestroy()
	{
		minput = null;
	}

	void Cancel_OnClick()
	{
		Destroy(gameObject);
	}

	void Play()
	{
		PlayWarVideo();
		Destroy(gameObject);
	}

}
