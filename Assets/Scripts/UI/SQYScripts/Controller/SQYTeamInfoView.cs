using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class SQYTeamInfoView : MDBaseViewController 
{
	public const int BTN_BACK = 1;
	public const int BTN_CHANGE = 2;
	public const int BTN_QiangHua = 3;
    public const int BTN_ROTATE_LEFT = 4;
    public const int BTN_ROTATE_RIGHT = 5;
	
	public UISprite sp_Attribute;
	public UILabel lab_PetName;
	public UILabel lab_PetLevel;

	public SkillTableScript m_skillTable;


	public UILabel lab_PetAttack;
	public UILabel lab_PetDefense;
	public UILabel lab_SkillTitle;

	public UILabel[] sz_SkillNames;

	public UIButton btn_change;
	public UIButton btn_QiangHua ;
	public UIButton btn_XunlianWu;

	public StarsUI m_stars;
	public StarsUI m_extStars;

	public System.Action<int> OnBtnWithIndex;

	public GameObject objRay;

	Monster _CurMonster
	{
		get
		{
			return TeamUI.mInstance.curMonster;
		}
	}

	MonsterTeam curTeam
	{
		get
		{
			return TeamUI.mInstance.curTeam;
		}
	}


	public bool FreshSelMonster()
	{
        bool AllFated = false;

		List<Skill> monsterSkill = _CurMonster.getSkill;

		lab_PetName.text = _CurMonster.config.name;

		if (monsterSkill == null)
            return false;

		m_skillTable.RefreshSkillＴable (monsterSkill);

		List<FateData> fateList = _CurMonster.getMyFate(Core.Data.fateManager);

		int count = fateList.Count;

		List<int> atkEffect = new List<int>();
		List<int> defEffect = new List<int>();

		for (int i = 0; i < sz_SkillNames.Length; i++)
		{
			sz_SkillNames[i].text = "";
		}
        int value = 0;
		for (int i = 0; i < count; i++) {

			sz_SkillNames[i].text = fateList[i].name;
			if(_CurMonster.checkMyFate(fateList[i], curTeam, Core.Data.dragonManager.usedToList()))
			{
                value ++;

				sz_SkillNames[i].color = new Color (1f,227f/255,43f/255);

				CheckMonEffect (i);
				if(fateList[i].effect[FateData.EFFECT_SELF_ATTACK] > 0)
				{
					atkEffect.Add(fateList[i].effect[FateData.EFFECT_SELF_ATTACK]);
				}
				else
				{
					defEffect.Add(fateList[i].effect[FateData.EFFECT_SELF_ATTACK]);
				}
			}
			else
			{
				sz_SkillNames[i].color = Color.gray;
			}
		}
	

        if(value == count) AllFated = true;
		
//		int nextExp = Core.Data.monManager.getMonsterNextLvExp(_CurMonster.Star, _CurMonster.RTData.curLevel);

       
//		float tValue = (float)(_CurMonster.RTData.curExp) / (float)(nextExp);
        
		lab_PetLevel.text = "Lv" + _CurMonster.RTData.curLevel.ToString();

		float atkPer = 0;
		float defPer = 0;
		for(int i = 0; i < atkEffect.Count; i++)
		{
			atkPer += atkEffect[i];
		}
		for(int i = 0; i < defEffect.Count; i++)
		{
			defPer += defEffect[i];
		}

		int pos = curTeam.GetMonsterPos(_CurMonster.pid);

		lab_PetAttack.text = curTeam.MemberAttack(pos).ToString();

		lab_PetDefense.text = curTeam.MemberDefend(pos).ToString();


        if (Core.Data.temper.tempId == _CurMonster.pid && Core.Data.temper.tempId != 0 )
        {

            if (Core.Data.temper.infoPetAtk != 0)
            {
                Core.Data.temper.deltaInfoAtk = curTeam.MemberAttack(pos) - Core.Data.temper.infoPetAtk;
            }
            if (Core.Data.temper.infoPetDef != 0)
            {
                Core.Data.temper.deltaInfoDef = curTeam.MemberDefend(pos) - Core.Data.temper.infoPetDef;
            }
            this.ShowUpdate ();

		}
            
        Core.Data.temper.tempId = _CurMonster.pid;
        Core.Data.temper.infoPetAtk = curTeam.MemberAttack(pos);
        Core.Data.temper.infoPetDef = curTeam.MemberDefend(pos);

       
		sp_Attribute.spriteName = "Attribute_" + ((int)(_CurMonster.RTData.Attribute)).ToString();

		RED.SetActive (_CurMonster.config.jinjie == 1, m_extStars.gameObject);
		m_stars.SetStar (_CurMonster.Star);
		m_extStars.SetStar (6);

		//Core.Data.temper.bagSelectMonster = null;
		Core.Data.temper.curShowMonster = _CurMonster;

#if SPLIT_MODEL
		if (!Core.Data.sourceManager.IsModelExist (_CurMonster.num))
		{
			UIDownModel.OpenDownLoadUI(_CurMonster.num, DownLoadFinish, new Vector3(10,-135,0));
		}
		else
		{
            UIDownModel.CloesWin();
		}
#endif
        return AllFated;
	}

	void DownLoadFinish(AssetTask aTask)
	{
		bool allFated = FreshSelMonster();
		TeamUI.mInstance.show3DModel(_CurMonster.num, _CurMonster.RTData.Attribute, allFated);
	}

    /// <summary>
    /// 展示 队伍攻击防御力 
    /// </summary>
    public void ShowUpdate(){
        //  yield return  new WaitForSeconds(durTime);
        //        Debug.Log("Show Update");
        string tDeltaAtk = "";
        string tDeltaDef = "";
        float tHeight = 50;
        float tTime = 2.0f;
       

        if (Core.Data.temper.deltaInfoAtk > 0)
        {
            tDeltaAtk = Core.Data.stringManager.getString(5103)+  "+" + Core.Data.temper.deltaInfoAtk;
			Vector3 vPos = Vector3.up * 100;// CRLuo_ShowStage.mInstance.GetModelHeight() * 200* Vector3.up ;
			LabelEffect.CreateLabelEffect(tDeltaAtk, tHeight, tTime, LabelEffect.lightGreen,vPos,gameObject.transform ,lab_PetAttack.depth,0,true);
        }
        else if(Core.Data.temper.deltaInfoAtk <0)
        {   tDeltaAtk = Core.Data.stringManager.getString(5103) +  Core.Data.temper.deltaInfoAtk.ToString();
			Vector3 vPos = Vector3.up * 100;//CRLuo_ShowStage.mInstance.GetModelHeight() * 200* Vector3.up ;
			LabelEffect.CreateLabelEffect(tDeltaAtk, tHeight, tTime, Color.red,vPos,gameObject.transform,lab_PetAttack.depth,0,true);
        }
        if (Core.Data.temper.deltaInfoDef > 0)
        {
            tDeltaDef = Core.Data.stringManager.getString(5104)+ "+" + Core.Data.temper.deltaInfoDef;
			Vector3 vPos =  Vector3.up * 100 +Vector3.down * 25;//CRLuo_ShowStage.mInstance.GetModelHeight() * 200 * Vector3.up + Vector3.down * 25; 
			LabelEffect.CreateLabelEffect(tDeltaDef,tHeight, tTime, LabelEffect.lightGreen,vPos,gameObject.transform,lab_PetDefense.depth,0,true);
        }
        else if(Core.Data.temper.deltaInfoDef <0)
        {   tDeltaDef =  Core.Data.stringManager.getString(5104)+ Core.Data.temper.deltaInfoDef.ToString();
			Vector3 vPos =  Vector3.up * 100 +Vector3.down * 25;//  CRLuo_ShowStage.mInstance.GetModelHeight() * 200*Vector3.up + Vector3.down * 25; 
			LabelEffect.CreateLabelEffect(tDeltaDef, tHeight, tTime, Color.red,vPos,gameObject.transform,lab_PetDefense.depth,0,true);
        }

    }
        
	public void OnBtnBack()
	{
		if(OnBtnWithIndex !=null)
		{
			OnBtnWithIndex(BTN_BACK);
			Core.Data.temper.curSelectEquip = null;
			Core.Data.temper.curShowMonster = null;
			Core.Data.temper.bagSelectMonster = null;
		}
	}

	public void OnBtnAdd()
	{
		if (_CurMonster == null)
			return;

		if (TrainingRoomUI.IsTrainingRoomUnLock ())
		{
			TrainingRoomUI.OpenUI (ENUM_TRAIN_TYPE.QianLiXunLian, _CurMonster, TraingCallback);
			DBUIController.mDBUIInstance.HiddenFor3D_UI ();

			SetActive (false);
			TeamUI.mInstance.del3DModel ();
            TrainingRoomUI.mInstance.m_qianLiUI.initXX();//yangchenguang
		}
	}


	void TraingCallback()
	{
		TeamUI.mInstance.SetShow(true);
		TeamUI.mInstance.FreshCurTeam ();
		UIMiniPlayerController.Instance.SetActive(false);
	}

	public void OnBtnChange()
	{
		if(Core.Data.playerManager.RTData.curLevel < 5)
		{
			string strText = Core.Data.stringManager.getString(7320);
			strText = string.Format(strText, 5);
			SQYAlertViewMove.CreateAlertViewMove(strText);
			return;
		}
	
		Core.Data.temper.bagSelectMonster = null;
		Core.Data.temper.curShowMonster = _CurMonster;
		Core.Data.temper.curSelectEquip = null;

		List<Monster> list = new List<Monster>();
		for(short i = 1; i <= 6; i++)
		{
			List<Monster> tempList = Core.Data.monManager.getMonsterListByStar(i, SplitType.Split_If_InCurTeam);
			list.AddRange(tempList.ToArray());
		}
		if(list.Count == 0)
		{
			if (TeamUI.mInstance.UIType == FromType.FromMainUI)
			{
				UIInformation.GetInstance ().SetInformation (Core.Data.stringManager.getString (5095), Core.Data.stringManager.getString (5030), OpenZhaoMuUI);
			}
			else
			{
				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5238));
			}
			return;
		}

		if(OnBtnWithIndex !=null)
		{
			OnBtnWithIndex(BTN_CHANGE);
		}
	}

	public void OnBtnQiangHua()
	{	
		if (_CurMonster == null)
			return;

		if (_CurMonster.RTData.curLevel >= 60)
		{
			if (_CurMonster.Star == 6)
			{
				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5159));
			}
			else
			{
				UIInformation.GetInstance ().SetInformation (Core.Data.stringManager.getString (5139), Core.Data.stringManager.getString (5030), OpenMonEvoveUI);
			}
			return;
		}

		if(OnBtnWithIndex !=null)
		{
			OnBtnWithIndex(BTN_QiangHua);
		}
	}

	void OpenMonEvoveUI()
	{
		if (TrainingRoomUI.IsTrainingRoomUnLock ())
		{

            TrainingRoomUI.OpenUI (ENUM_TRAIN_TYPE.MonsterEvolve, _CurMonster,TraingCallback);
		//	DBUIController.mDBUIInstance.SetViewState (RUIType.EMViewState.HIDE_TEAM_VIEW);
            TeamUI.mInstance.SetShow(false);
			DBUIController.mDBUIInstance.HiddenFor3D_UI ();
		}
	}


	void OpenZhaoMuUI()
	{
		ZhaoMuUI.OpenUI();
		DBUIController.mDBUIInstance.SetViewState(RUIType.EMViewState.HIDE_TEAM_VIEW);
		DBUIController.mDBUIInstance.HiddenFor3D_UI();
	}

	public void OnBtnXunLianWu()
	{
		if (_CurMonster == null)
			return;

		if (TrainingRoomUI.IsTrainingRoomUnLock ())
		{
			TrainingRoomUI.OpenUI (ENUM_TRAIN_TYPE.None, null, TraingCallback);
			DBUIController.mDBUIInstance.HiddenFor3D_UI ();

			TeamUI.mInstance.SetShow (false);
		}
	}

	public void OnBtnSkillView()
	{
		if (_CurMonster != null) 
		{
			if (TeamUI.mInstance.UIType == FromType.FromTopUI)
				CombinationSkillPanelScript.ShowpbCombinationSkillPanel (DBUIController.mDBUIInstance._TopRoot, _CurMonster, false);
			else
				CombinationSkillPanelScript.ShowpbCombinationSkillPanel (DBUIController.mDBUIInstance._TopRoot, _CurMonster);
		}
	}

	public void OnBtnSkillUp()
	{
		if (_CurMonster == null)
			return;

		SkillUpUI.CreateSkillUpUI (_CurMonster);
	}


	public void OnShowFateController(){
		if (_CurMonster != null) {
			ShowFatePanelController.CreatShowFatePanel (_CurMonster.config.ID, ShowFatePanelController.FateInPanelType.isInInfoPanel, null);
		}
	}


	public void CheckEffect(){

		TemporyData tempD = Core.Data.temper;
		Monster tLastMon =tempD.curShowMonster;
		Equipment tEquip = tempD.curSelectEquip;
		if (tEquip== null || tLastMon ==null || _CurMonster== null) {
			return;
		}
		EquipData tEquipD = tEquip.ConfigEquip;

		List<FateData> fateList = tLastMon.getMyFate (Core.Data.fateManager);

		//装备的 展示缘
		if (tLastMon != null  && _CurMonster.pid == tLastMon.pid && tEquipD != null) {
//			Debug.Log (" in 1  ");
			if (Core.Data.fateManager.EquipAndMonIsFate (tLastMon.config.ID, tEquipD.ID)) {
				if(fateList != null){
//					Debug.Log (" in  2");
					for (int i = 0; i < fateList.Count; i++) {
						if (fateList[i].WhoesFateId == _CurMonster.config.ID) {

							if (fateList [i].itemID != null) {
//								Debug.Log (" in  3  === " + i );
								for (int j = 0; j < fateList[i].itemID[1].Length; j++) {
									if (fateList [i].itemID [1][j] == tEquipD.ID) {
//										Debug.Log (" in 4  ===  j === " + j  + "   i " + i );
										ShowFateEffect (i);
//										break;
									}
								}
							}
						}
					}
				}
			}
		}
	}
	//依托条件  ： 必然选择 或者替换的 monster  在teamui被展示
	public void CheckMonEffect(int i =-1){
		TemporyData tempD = Core.Data.temper;
		Monster tLastMon =tempD.curShowMonster;
		Monster tSelectMon = tempD.bagSelectMonster;



		if (tSelectMon != null) {
			if (tLastMon != null) {
				//替换
				if (tLastMon.pid != _CurMonster.pid) {
					if (i != -1) {
						ShowFateEffect (i);
					}
				}
			} else {
				//上阵
				List<FateData> fateList = tSelectMon.getMyFate(Core.Data.fateManager);
				if (i != -1 && fateList != null && fateList.Count != 0 && i< fateList.Count) {
					if(tLastMon == null && tSelectMon.checkMyFate(fateList[i], curTeam, Core.Data.dragonManager.usedToList())){
						ShowFateEffect (i);
					}
				}
			}
		}
	}





//	int[] temp = new int[4]{0,0,0,0}

	void ShowFateEffect(int pos ){
		//当前 展示  monster
//		sz_SkillNames [pos].transform.FindChild ("ray").gameObject.SetActive (true);
		if (objRay != null) {
			GameObject tObj =  Instantiate (objRay) as GameObject;
			if (tObj != null) {
				tObj.transform.parent = sz_SkillNames [pos].transform;
				tObj.transform.localScale = Vector3.one;
				tObj.transform.localPosition = Vector3.up * 15;
				
			}
		}
//		temp[pos] = 1;

//		Invoke ("Close",1.5f);
		Core.Data.temper.curSelectEquip = null;
	}
	void Close(){
		for(int i=0;i< sz_SkillNames.Length;i++)
			sz_SkillNames [i].transform.FindChild ("ray").gameObject.SetActive (false);
		Core.Data.temper.bagSelectMonster = null;

	}

//	void OnGUI(){
//		if (GUI.Button (new Rect (100, 100, 200, 50), " fate  1")) {
//			ShowFateEffect (1);
//		}
//	}

}