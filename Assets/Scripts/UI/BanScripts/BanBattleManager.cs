using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AW.Battle;
using AW.Utils;

public partial class BanBattleManager : MonoBehaviourEx {

	enum AnyWar {
		PVE,
		PVP,
	}

	#region Indicator
	private const short UOBJ_ROLEICON = 0;
	private const short UOBJ_ATTRIBUTE = 1;
	private const short UOBJ_Win = 2;
	private const short UOBJ_Lose = 3;
	#endregion
	public static BanBattleManager Instance = null;
	public bool _replay = false;
	//全部的角色
	public List<BanBattleRole> list_BattleRole = new List<BanBattleRole>();

	//攻击方的奥义
	public List<int> list_AttAoYi = new List<int>();
	//防守方的奥义
	public List<int> list_DefAoYi = new List<int>();

	private List<BanBattleRole> _list_Attack = null;

	//2DUI 照相机
	//public Camera UICamera;

	/// <summary>
	/// 技能动作时间控制.
	/// </summary>
	public bool _bTimeScale = false;
	public float _deltaTime = 0f;
	public float _startScaleTime = 0.5f;
	public float _endScaleTime = 1.2f;

	// ---- 提过按钮的动画 ---
	public InitBattleStr skipBtn;

	// ---- 删除Root节点 -----
	public BattleCamera Root;

    // ---- 普通对打的UI ---
    public FightVsManager VsMgr;

	// ---  KO的UI ---
	public KOAnim koMgr;

	// ---- 怒气技连接部分 -----
	public BattleHitMgr hitCCMgr;

	// ---- 新版奥义动画 ---
	public DragonNewAnim AoyiMgr;
	//----- 新版魂魄 -----
	public GhostFly GhostMgr;
	//----- 暂停 功能 -----
	public GameObject PauseGo;
	//---- 跳过 功能 ----
	public GameObject SkipGo;
	//---- 快速 功能 ----
	public GameObject SpeedUpGo;
	//---- 自动战斗 ----
	public GameObject AutoGo;
	//---- 登场动画 ----
	public AttendStage AttendStageMgr;
	// ----- 杀死的敌人列表 -----
	private List<int> KilledEnemyArray = null;
	//---- 最后登场的敌人索引 --- 如果是-1则代表都死亡了
	private int maxAttendEnemyIndex = -1;

	//攻击方信息
	public BanSideInfo attackSideInfo;

	//防御方信息
	public BanSideInfo defenseSideInfo;

	//3D内容管理器
	public Main3DManager main3DManager;

	//NGUI Main Panel
	public Transform mainPanelTrans;

	//属性相克的点位置
	public GameObject go_AttributeConflictPos;

	//胜利或者失败的点位置
	public GameObject go_WinOrLosePos;

	//UI的panel位置
	public GameObject go_uiPanel;

	//神龙奥义的技能动画
	public DragonAoYiList DragonAoYiAnim;

	//左边怒气技能的物体
	public GameObject AngryPos;

	//加速和跳过的控制器
	public ButtonListener SpeedOrSkipMgr;

	//boss出厂的动画
	public BossComing BossMgr;

	//自动的图标
	public UISprite AutoSp;
	//暂停的图标
	public UISprite PauseSp;
	//加速的图标
	public UISprite SpeedUpSp;

	public int[] _currentRevive = null;
	BanBattleRole.Group _curGroup;

	//核心的运算逻辑
	public BT_Logical War;

	#region 创建新版未来之战的UI
	//新手引导的UI
	private BattleGuide BT_GuideUI = null;
	public BattleGuide getGuideUI {
		get {
			return BT_GuideUI ?? (BT_GuideUI = getBTGuideUI());
		}
	}

	BattleGuide getBTGuideUI() {
		Object obj = PrefabLoader.loadFromUnPack("Ban/BattleGuide", false);
		GameObject go = Instantiate(obj) as GameObject;
		//RED.AddChild(go, BattleCamera.Instance.Camera1.gameObject);
		return go.GetComponent<BattleGuide>();
	}

	#endregion

	//==================================

	// ------ 获取死亡的敌人列表 ----
	public List<int> GetDeadEnmeyList() {

		if(KilledEnemyArray.Count == 0) {
			if(maxAttendEnemyIndex == -1) {
				foreach(var role in list_Defense) {
					KilledEnemyArray.Add(role.number);
				}
			} else {
				foreach(var role in list_Defense) {
					if(role.index < maxAttendEnemyIndex)
						KilledEnemyArray.Add(role.number);
				}
			}
		}

		return KilledEnemyArray;
	}

	public bool ScaleTime {
		get{ return _bTimeScale;}
		set{
			_bTimeScale = value;
			_deltaTime = 0f;
		}
	}
	public List<BanBattleRole> GetRoleList(BanBattleRole.Group side)
	{
		List<BanBattleRole> temp = new List<BanBattleRole> ();
		foreach(BanBattleRole aBanBattleRole in list_BattleRole){
			if(aBanBattleRole.group == side){
				temp.Add(aBanBattleRole);
			}
		}
		return temp;
	}
	//攻击方的角色
	public List<BanBattleRole> list_Attack{
		get{
			if(_list_Attack == null){
				_list_Attack = new List<BanBattleRole>();
				foreach(BanBattleRole aBanBattleRole in list_BattleRole){
					if(aBanBattleRole.group == BanBattleRole.Group.Attack){
						_list_Attack.Add(aBanBattleRole);
					}
				}
			}
			return _list_Attack;
		}
	}

	private List<BanBattleRole> _list_Defense = null;

	//防御方的角色
	public List<BanBattleRole> list_Defense {
		get{
			if(_list_Defense == null){
				_list_Defense = new List<BanBattleRole>();
				foreach(BanBattleRole aBanBattleRole in list_BattleRole){
					if(aBanBattleRole.group == BanBattleRole.Group.Defense){
						_list_Defense.Add(aBanBattleRole);
					}
				}
			}
			return _list_Defense;
		}
	}

	//返回攻击方存活角色的数量
	public int GetAttAliveCount(){
		int count = 0;
		foreach(BanBattleRole aRole in list_Attack){
			if(aRole.curBp > 0){
				count++;
			}
		}
		return count;
	}

	//返回防御方存活角色的数量
	public int GetDefAliveCount(){
		int count = 0;
		foreach(BanBattleRole aRole in list_Defense){
			if(aRole.curBp > 0){
				count++;
			}
		}
		return count;
	}

	public void RefreshAllAliveCount()
	{
		int attAliveCount = GetAttAliveCount();

		int defAliveCount = GetDefAliveCount();

		attackSideInfo.ReadRoleNum(attAliveCount,list_Attack.Count);

		defenseSideInfo.ReadRoleNum(defAliveCount,list_Defense.Count);
	}

	//角色图标的预设体
	public Object prefab_RoleIcon {
		get{
			if(UObjDic.ContainsKey(UOBJ_ROLEICON))
				return UObjDic[UOBJ_ROLEICON];
			else {
				Object o = BanTools.LoadFromUnPack("RoleIcon");
				UObjDic.Add(UOBJ_ROLEICON, o);
				return o;
			}
		}
	}

	//属性相克预设体
	public Object prefab_AttributeConflict{
		get{
			if(UObjDic.ContainsKey(UOBJ_ATTRIBUTE))
				return UObjDic[UOBJ_ATTRIBUTE];
			else {
				Object o = null;
				#if VS
				o = BanTools.LoadFromUnPack("AttributeConflictEx");
				#else
				o = BanTools.LoadFromUnPack("AttributeConflict");
				#endif

				UObjDic.Add(UOBJ_ATTRIBUTE, o);
				return o;
			}
		}
	}

	//胜利预设体
	public Object prefab_Win{
		get{
			if(UObjDic.ContainsKey(UOBJ_Win))
				return UObjDic[UOBJ_Win];
			else {
				Object o = BanTools.LoadFromUnPack("Win");
				UObjDic.Add(UOBJ_Win, o);
				return o;
			}
		}
	}

	//失败预设体
	public Object prefab_Lose {
		get{
			if(UObjDic.ContainsKey(UOBJ_Lose))
				return UObjDic[UOBJ_Lose];
			else {
				Object o = BanTools.LoadFromUnPack("Lose");
				UObjDic.Add(UOBJ_Lose, o);
				return o;
			}
		}
	}

	//是PVE or PVP
	AnyWar KindOfWar {
		get {
			TemporyData temp = Core.Data.temper;

			if(temp.currentBattleType == TemporyData.BattleType.BossBattle 
			    || temp.currentBattleType == TemporyData.BattleType.FinalTrialShalu
			    || temp.currentBattleType == TemporyData.BattleType.FinalTrialBuou
				|| temp.currentBattleType == TemporyData.BattleType.FightWithFulisa)
				return AnyWar.PVE;
			else return AnyWar.PVP;
		}
	}

	void Awake(){
		Instance = this;
		CityFloorData.Instance.isCanClick = true;

		TemporyData temp = Core.Data.temper;

		float ratio = Screen.width / (Screen.height * 1.0f);
		if(Mathf.Approximately(ratio, Consts._16Bi9)) 
			go_WinOrLosePos.transform.localPosition = new Vector3 (0f,  20f, 0f);
		else if(Mathf.Approximately(ratio, Consts._4Bi3))
			go_WinOrLosePos.transform.localPosition = new Vector3 (0f,  110f, 0f);
		else 
			go_WinOrLosePos.transform.localPosition = new Vector3 (0f,  30f, 0f);


		///
		///  --- 跳过功能和暂停功能 ---
		///  PVE 只能有暂停功能
		///  PVP 只能有跳过功能
		///

		//初始化战斗数据
		if(KindOfWar == AnyWar.PVE) {

			//新手引导的时候不出现暂停按钮
			if(Core.Data.guideManger.isGuiding) {
				PauseGo.SetActive(false);
				SpeedUpGo.SetActive(false);
				AutoGo.SetActive(false);
			} else {
				PauseGo.SetActive(true);
				SpeedUpGo.SetActive(true);
				AutoGo.SetActive(true);
			} 

			if(temp.currentBattleType == TemporyData.BattleType.FightWithFulisa) {
				PauseGo.SetActive(false);
				SpeedUpGo.SetActive(false);
				AutoGo.SetActive(false);
			}
				
			SkipGo.SetActive(false);

			if(temp.clientDataResp != null && temp.clientDataResp.data != null && temp.clientDataResp.data.warInfo != null) {
				War = new BT_Logical(temp.clientDataResp.data.warInfo);
                if(War.LocalMode) {
                    if(War.StepMode) {
                        War.startWarOfWarBegin();

                        War.RegisterLogicalCmp(ReadFromLocal);
                    } else {
                        War.startWar();
                        War.EndWar();
                    }
                }
			} 
		} else { // is a PVP war

			PauseGo.SetActive(false);
			SpeedUpGo.SetActive(false);
			AutoGo.SetActive(false);
			//新手引导不应该有跳过
			if(Core.Data.guideManger.isGuiding)
				SkipGo.SetActive(false);
			else 
				SkipGo.SetActive(true);

            //世界战斗 也应该出现怒气技能释放的按钮
			if(temp.currentBattleType == TemporyData.BattleType.WorldBossWar)
	            attackSideInfo.PlayerAngryBtn.gameObject.SetActive(false);
        }

		//展示3D场景
		show3DScene();

		//一场战斗连击数的统计
		temp.hasLiquidate = false;
		temp.SkipBattle   = false;
		temp.GiveUpBattle = false;
		temp.ForceWriteCombo = 0;
		main3DManager.NotifyCombo = (combo) => { hitCCMgr.RecordCombo(combo); };

		KilledEnemyArray = new List<int>();

		AutoJustice();

		//重置时间的管理器
		TimeMgr.getInstance().Reset();
	}

	/// <summary>
	/// 自动匹配下面3个按钮
	/// 本地自动计算的只有“暂停”和“加速”
	/// </summary>
	void AutoJustice() {
		#if LOCAL_AUTO
		AutoSp.gameObject.SetActive(false);
		PauseSp.gameObject.SetActive(true);
		SpeedUpSp.gameObject.SetActive(true);

		PauseSp.transform.localPosition = new Vector3(-44F, 0F, 0F);
		SpeedUpSp.transform.localPosition = new Vector3(-20F, 0F, 0F);
		#else
		AutoSp.gameObject.SetActive(true);
		PauseSp.gameObject.SetActive(true);
		SpeedUpSp.gameObject.SetActive(true);
		#endif
	}


	/// <summary>
	/// 设定是否需要战斗加速
	/// </summary>
	void SetBattleStatus() {
		///
		/// 计时模块，设定战斗的初始值为1.3F
		///
		TemporyData temp = Core.Data.temper;
		bool featurewar = temp.currentBattleType == TemporyData.BattleType.FightWithFulisa;

		if(!featurewar) TimeMgr.getInstance().setBaseLine(ButtonListener.BaseSpeed);

		AccountConfigManager accMgr = Core.Data.AccountMgr;
		if(accMgr.UserConfig.SpeedUp > 0) {
			SpeedOrSkipMgr.OnAddSpeedButtonClick();
		}

		if(accMgr.UserConfig.AutoBat > 0) {
			SpeedOrSkipMgr.OnAutoButtonClick();
		}
	}


	/// <summary>
	/// 根据规则去显示3D的背景场景
	/// </summary>
	void show3DScene() {
		TemporyData temp = Core.Data.temper;
		//展示3D场景
		if(temp.currentBattleType == TemporyData.BattleType.BossBattle ||
			temp.currentBattleType == TemporyData.BattleType.GPSWar)
			main3DManager.ShowScreen(Core.Data.temper.CitySence);
		else if(temp.currentBattleType == TemporyData.BattleType.FightWithFulisa) {
			main3DManager.ShowScreen(3);
		} else {
			int scence = Random.Range(1, 5);
			main3DManager.ShowScreen(scence);
		}
	}

	/// <summary>
	/// 根据 index 获得指定宠物
	/// </summary>
	/// <returns>The role by index.</returns>
	/// <param name="index">Index.</param>
	/// <param name="iGroup">I group.</param>
	BanBattleRole GetRoleByIndex(int index,List<BanBattleRole> rlist )
	{
		foreach (BanBattleRole r in rlist) {
			if (r.index == index)
				return r;
		}
		return null;
	}


	int GetSkillAction(int roleIndex,int skill) 
	{ 
		BanBattleRole role = GetBattleRole (roleIndex);
		MonsterData mon = Core.Data.monManager.getMonsterByNum (role.number);

		if (mon == null || mon.skill == null) {
			return -1;
		}

		for (int i = 0; i < mon.skill.Length; ++i) 
		{ 
			if (skill == mon.skill [i]) 
			{
				return mon.anime3D [i];
			}	 
		}


		return -1; 
	} 

	//判定是SkillID 还是AoYiID
	//非常恶心的判定
	bool isAoYiSkill(int skillId) {
		bool aoyi = false;
		if(skillId / 10000 == 27) {
			aoyi = true;
		} 
		return aoyi;
	}

	BanBattleProcess.Item AddSkillItem(int skillType,int attackIndex,int defenseIndex,int attackBP,int defenseBP,int attackAP,int defenseAP, int extra_1,int extra_2,int extra_3, int skillId, int leftAp, CMsgHeader item = null) {
		BanBattleProcess.Item proItem = null;

		//判定是SkillID 还是AoYiID
		//非常恶心的判定
		if(isAoYiSkill(skillId)) {
			AoYiData aoyiCfg = Core.Data.dragonManager.getAoYiData(skillId);

			int pos = getAoYiSkillPos(skillId, attackIndex == extra_1);

			proItem = new BanBattleProcess.Item(0, 0, attackIndex, defenseIndex, attackBP, defenseBP, attackAP, defenseAP, BanBattleProcess.Period.DragonAoYi, extra_1, extra_2, extra_3, pos, leftAp, aoyiCfg.name, item);
			proItem.aoyiConfig = aoyiCfg;
			BanBattleProcess.Instance.list_Item.Add(proItem);

			return proItem;
		} else {

			int sAction = GetSkillAction(extra_1,skillId);

			SkillData data = Core.Data.skillManager.getSkillDataConfig (skillId);
			string skillname = "";
			if (data != null)
				skillname = data.name;

			CSkill_Type cskType = CSkill_Type.Normal;
			if(System.Enum.IsDefined( typeof(CSkill_Type), skillType) ) {
				cskType = (CSkill_Type)skillType;
			}


			if (cskType == CSkill_Type.Anger) {
				proItem = new BanBattleProcess.Item(data.param.num,0, attackIndex, defenseIndex, attackBP, defenseBP, attackAP, defenseAP, BanBattleProcess.Period.AngrySkill, extra_1, extra_2, extra_3, sAction,leftAp,skillname,item);
			} else if (cskType == CSkill_Type.Normal) { 
				proItem = new BanBattleProcess.Item(0,0, attackIndex, defenseIndex, attackBP, defenseBP, attackAP, defenseAP, BanBattleProcess.Period.NormalSkill, extra_1, extra_2, extra_3, sAction,leftAp,skillname,item); 
			} else if (cskType == CSkill_Type.Die){ 
				proItem = new BanBattleProcess.Item(0,0, attackIndex, defenseIndex, attackBP, defenseBP, attackAP, defenseAP, BanBattleProcess.Period.DieSkill, extra_1, extra_2, extra_3, sAction,leftAp,skillname,item); 
			} else if(cskType == CSkill_Type.AfterWar) {
				proItem = new BanBattleProcess.Item(0,0, attackIndex, defenseIndex, attackBP, defenseBP, attackAP, defenseAP, BanBattleProcess.Period.AfterBattle, extra_1, extra_2, extra_3, sAction,leftAp,skillname,item); 
			} 

			if(proItem != null) {
				proItem.skillType = cskType;
				proItem.skillId = skillId;
				BanBattleProcess.Instance.list_Item.Add(proItem);
			}
		}


		return proItem;
	}
	//处理运涛数据
	public void ReadFromHttp(){
        localCached = new ConvertData();

		//清除之前的一条条信息
		BanBattleProcess.Instance.list_Item.Clear();
		list_BattleRole.Clear();
		list_AttAoYi.Clear();
		list_DefAoYi.Clear();

		#if DEBUG
		ConsoleEx.DebugLog(JsonFx.Json.JsonWriter.Serialize(Core.Data.temper.warBattle));
		#endif

		int headerIndex = 0;
		List<CMsgHeader> battleInfo = Core.Data.temper.warBattle.battleData.info;

		foreach(CMsgHeader aHeader in battleInfo) {
			headerIndex ++;
			switch(aHeader.status) {
			case CMsgHeader.STATUS_WAR_BEGIN:
                WarBegin(aHeader);

				//战斗开始，读取队伍信息，怒气槽等信息
				CMsgWarBegin aWarBegin = aHeader as CMsgWarBegin;  
                localCached.attAPCount = aWarBegin.attTeam.angryCnt;
                localCached.defAPCount = aWarBegin.defTeam.angryCnt;

				break;
			case CMsgHeader.STATUS_ROUND_BEGIN:
				//回合开始
                RoundBegin(aHeader);
				break;
            case CMsgHeader.STATUS_PROPERTY_KILL:
                //属性相克
                Property_Kill(aHeader);
                break;
            case CMsgHeader.STATUS_ATTACK:
                NormalAttack(aHeader);
                break;
            case CMsgHeader.STATUS_WAR_END:
                WarEnd(aHeader);
                break;
            case CMsgHeader.STATUS_NSK_401: case CMsgHeader.STATUS_NSK_402: case CMsgHeader.STATUS_NSK_403:
            case CMsgHeader.STATUS_NSK_404: case CMsgHeader.STATUS_NSK_405: case CMsgHeader.STATUS_NSK_406:
            case CMsgHeader.STATUS_NSK_408: case CMsgHeader.STATUS_NSK_409: case CMsgHeader.STATUS_NSK_410:
            case CMsgHeader.STATUS_NSK_411: case CMsgHeader.STATUS_NSK_412: case CMsgHeader.STATUS_NSK_413:
            case CMsgHeader.STATUS_NSK_414: case CMsgHeader.STATUS_NSK_415: case CMsgHeader.STATUS_NSK_417:
            case CMsgHeader.STATUS_NSK_419: case CMsgHeader.STATUS_NSK_420:
                CastSkill(aHeader.status, aHeader);
				break;
			case CMsgHeader.STATUS_NSK_407:
            case CMsgHeader.STATUS_NSK_418:
                CastSkill(aHeader.status, aHeader, battleInfo, headerIndex);
				break;
			default:
                #if DEBUG
				BanTools.LogWarning(BanTools.Serialize(aHeader));
                #endif
				break;
			}
		}

	}

	private int getAoYiSkillPos (int AoYiId, bool leftSide) {
		int pos = -1;

		List<int> ListAoYi = leftSide ? list_AttAoYi : list_DefAoYi;
		int length = ListAoYi.Count;

		for(int i = 0; i < length; ++ i) {
			if(ListAoYi[i] == AoYiId) {
				pos = i;
				break;
			}
		}
		return pos;
	}


	IEnumerator Start(){
		ReadFromHttp();
		//跳过战斗的播放动画
		BanBattleProcess.Instance.skip = false;
		//攻击方读取数据
		attackSideInfo.ReadData(list_Attack,BanTools.GetAttackPlayerName());
		//防御方读取数据
		defenseSideInfo.ReadData(list_Defense,BanTools.GetDefensePlayerName());

		//攻击方奥义的数据
		DragonAoYiAnim.dragonMgrLeft.ReadData(list_AttAoYi);
		//防御方奥义的数据
		DragonAoYiAnim.dragonMgrRight.ReadData(list_DefAoYi);
		//初始化奥义，血条的状态
		AoyiMgr.init();
		//设定战斗速度
		SetBattleStatus();

		yield return new WaitForSeconds(1f);

        if(War != null && War.LocalMode && War.StepMode) {
            if(Core.Data.temper.hasLiquidate)
                //开始处理信息
                BanBattleProcess.Instance.HandleItem();
            else 
                War.StartWarStep();
        } else {
            //开始处理信息
            BanBattleProcess.Instance.HandleItem();
        }
		
	}


	/*
	 * 全屏幕点击收回两边的队伍
	 * 
	void Update() {
        //如果按下则，考虑收回两边的队伍
        if(!mgr.isGuiding) {
            if(Input.GetMouseButtonDown(0)) {
                Vector3 touch = Input.mousePosition;

                bool inArea = UnityUtils.isInScreenRect(BtnShowAttackTeam, UICamera.currentCamera, touch);
                bool inArea2 = UnityUtils.isInScreenRect(BtnShowDefendTeam, UICamera.currentCamera, touch);

                if( !(inArea || inArea2) ) {
                    attackSideInfo.HideTeam();
                    defenseSideInfo.HideTeam();
                }
            }
        }
	
	}*/

	//根据索引查找角色
	public BanBattleRole GetBattleRole(int index){
		foreach(BanBattleRole aBanBattleRole in list_BattleRole){
			if(aBanBattleRole.index == index){
				return aBanBattleRole;
			}
		}
		Debug.LogError("can not find this index:"+index);
		return null;
	}

	//生成一个角色模型
	public CRLuo_PlayAnim_FX GeneratePlayAnim(int index){
		GameObject temp = EmptyLoad.CreateObj(ModelLoader.get3DModel(index));
		return temp.GetComponent<CRLuo_PlayAnim_FX>();
	}

	/// <summary>
	/// /生成一个属性克制效果
	/// </summary>
	/// <param name="attackAttribute">攻击方的属性.</param>
	/// <param name="defenseAttribute">防御方的属性.</param>
	public bool GenerateAttributeConflict(BanBattleRole.Attribute attackAttribute,BanBattleRole.Attribute defenseAttribute){
		#if VS
		AttributeConflictEx temp = BanTools.CreateNGUIOject( BanBattleManager.Instance.prefab_AttributeConflict,go_AttributeConflictPos.transform.position,
			Quaternion.identity,go_AttributeConflictPos.transform ).GetComponent<AttributeConflictEx>();
		return temp.Set(attackAttribute,defenseAttribute);
		#else
		BanAttributeConflict temp = BanTools.CreateNGUIOject( BanBattleManager.Instance.prefab_AttributeConflict,go_AttributeConflictPos.transform.position,
		Quaternion.identity,go_AttributeConflictPos.transform ).GetComponent<BanAttributeConflict>();
		return temp.Set(attackAttribute,defenseAttribute);
		#endif
	}

	public void UpdateHpData(int attackBP,int defenseBP){
		GetBattleRole(BanBattleProcess.Instance.attackerIndex).curBp = attackBP;
		GetBattleRole(BanBattleProcess.Instance.defenderIndex).curBp = defenseBP;
	}

    /// <summary>
    /// 简化对决时候的参数
    /// </summary>

    public void UpdateHpIfVs(int attackBP, int defenseBP, float delayTime, int count, bool removeIcon) {
        attackSideInfo.isFightVsDamage = true;
        defenseSideInfo.isFightVsDamage = true;
        UpdateHp(attackBP, defenseBP, delayTime, count, removeIcon);
    }

	/// <summary>
	/// 更新双方的战斗力
	/// </summary>
	/// <param name="attackBP">攻击方战斗力.</param>
	/// <param name="defenseBP">防御方战斗力.</param>
	/// <param name="delayTime">延迟时间.</param>
	/// <param name=""> 攻击次数 </param>
    public void UpdateHp(int attackBP, int defenseBP, float delayTime, int count = 1, bool removeIcon = true) {
		if (count == 1) {
			GetBattleRole (BanBattleProcess.Instance.attackerIndex).curBp = attackBP;
			GetBattleRole (BanBattleProcess.Instance.defenderIndex).curBp = defenseBP;
			StartCoroutine (UpdateHpIE (attackBP, defenseBP, delayTime));
		} else {
			int tempBpA = GetBattleRole (BanBattleProcess.Instance.attackerIndex).curBp;
			int tempBpD = GetBattleRole (BanBattleProcess.Instance.defenderIndex).curBp;
			int singlehurtA = (tempBpA - attackBP) / count;
			int singlehurtD = (tempBpD - defenseBP) / count;

			StartCoroutine (SepUpdateHp(tempBpA,singlehurtA,tempBpD,singlehurtD,count));
			GetBattleRole (BanBattleProcess.Instance.attackerIndex).curBp = attackBP;
			GetBattleRole (BanBattleProcess.Instance.defenderIndex).curBp = defenseBP;
		}

		/*if(removeIcon) {
			//存在牺牲，便要更新下方图标
			if(attackBP <= 0){
				attackSideInfo.RemoveFirstRoleIcon(BanTimeCenter.F_HP_ANIM);
			}

			if(defenseBP <= 0){
				defenseSideInfo.RemoveFirstRoleIcon(BanTimeCenter.F_HP_ANIM);
			}
		}*/
	}

	/// <summary>
	/// 属性克制掉血的动画
	/// </summary>
	/// <param name="attackBP">Attack B.</param>
	/// <param name="defenseBP">Defense B.</param>
	public void UpdateHpIfAttriConflict(int attackBP, int defenseBP) {
		GetBattleRole (BanBattleProcess.Instance.attackerIndex).curBp = attackBP;
		GetBattleRole (BanBattleProcess.Instance.defenderIndex).curBp = defenseBP;

		attackSideInfo.curBp = attackBP;
		defenseSideInfo.curBp = defenseBP;
	}

	public int getHp(bool fromAttack) {
		int index = fromAttack ? BanBattleProcess.Instance.attackerIndex : BanBattleProcess.Instance.defenderIndex;
		return GetBattleRole (index).curBp;
	}


	IEnumerator SepUpdateHp(int tempBpA,int singlehurtA,int tempBpD,int singlehurtD,int count)
	{
		for (int i = 1; i <= count-1; i++) {
			yield return new WaitForSeconds (0.55f);
			StartCoroutine (UpdateHpIE (tempBpA - i * singlehurtA, tempBpD - i * singlehurtD, 0.1f));
		}
	}

	IEnumerator UpdateHpIE(int attackBP,int defenseBP,float delayTime){
		yield return new WaitForSeconds(delayTime);
		attackSideInfo.curBp = attackBP;
		defenseSideInfo.curBp = defenseBP;
	}


	public void ReleaseADestory() {

//		if(Root != null) {
//			Root.dealloc();
//			Root = null;
//		}

		if(main3DManager != null) {
			Destroy(main3DManager.gameObject);
			main3DManager = null;
		}

		base.dealloc();
		Instance = null;
		Destroy(this.gameObject);
	}

	UIBossReward mReward;
	UIGetGambleResultController mGambleResult;
	public bool battleWin = false;
	// 0 == attack side , 1 == defend side
	int side = 0;
	public int IAmWhichSide {
		get {
			return side;
		}
	}
	//是否重放
	private bool isReplay = false;
	public bool IsReplay {
		get {
			return isReplay;
		}
	}
	#region 赌博结算

	//add by  wxl 赌博结算 在战斗结算之前  
	public void ShowGambleResult(){
		//   Debug.Log (" show gamble result");
		if (Core.Data.temper.warBattle.gambleResult != null) {
			if (mGambleResult == null) {
				mGambleResult = UIGetGambleResultController.CreateGamblePanel (go_WinOrLosePos, ShowCalculate);
				mGambleResult.transform.parent = go_WinOrLosePos.transform;
				mGambleResult.transform.localPosition = Vector3.zero;
				mGambleResult.transform.localScale = Vector3.one;
				mGambleResult.transform.localRotation = Quaternion.identity;
			}
		} else {
			this.ShowCalculate ();
		}
	}
	#endregion
	
    public void  IsRewardDelay()
    {
        TemporyData tempData = Core.Data.temper;
        if (tempData.currentBattleType == TemporyData.BattleType.BossBattle ||tempData.currentBattleType == TemporyData.BattleType.FinalTrialBuou ||
            tempData.currentBattleType == TemporyData.BattleType.FinalTrialShalu  )
        {
            Invoke("RewardDelay" , 1.5f);
        }else
        {
            ShowCalculate_Is();
        }

    }
    private void RewardDelay()
    {
        CancelInvoke("RewardDelay");
        ShowCalculate_Is();
    }
    public void ShowCalculate()
    {
        IsRewardDelay();

    }
	public void ShowCalculate_Is()
	{
       

		TemporyData tempData = Core.Data.temper;
		//已经清算了。
		tempData.hasLiquidate = true;
		isReplay = true;
		BanBattleProcess.Instance.HandleItemEnd();
        int Teamdef = 0 ;
		if(mReward == null && _replay == false)
		{
			mReward = FhjLoadPrefab.GetPrefabClass<UIBossReward>();
			if(mReward != null)
			{
				Transform t = mReward.transform;
				t.parent = go_WinOrLosePos.transform;
				t.localPosition = Vector3.back;
				t.localRotation = Quaternion.identity;
				t.localScale = Vector3.one;
				mReward.OnReplay = Replay;// 重播按钮

                if (!battleWin)
                {
                    mReward.getRewardLab.gameObject.SetActive(false); // 失败的时候 去点 获得战利品 字符串
                    mReward.FailRewardPosi();
                    //tempData.currentBattleType == TemporyData.BattleType.QiangDuoGoldBattle 
                }

				if (tempData.currentBattleType == TemporyData.BattleType.BossBattle)
                {

					///
					/// 新手引导
					///
					if(Core.Data.guideManger.isGuiding) {
						//UIGuide.Instance.DelayAutoRun(0.3F);
						AsyncTask.QueueOnMainThread( () => {Core.Data.guideManger.AutoRUN();} , 0.3f);

					}

                    Teamdef = mReward.getBattleRightTeamDef();
                   
                    if (battleWin)
                    {
                        if(tempData.warBattle.comboReward != null )
                        {
                            mReward.SetCombe( tempData.FingerTotalCombo.ToString() , tempData.warBattle.comboReward.award.ToString() );

                        }
                        mReward.Show (battleWin, tempData.warBattle.reward,Teamdef);

                    } else {
                        mReward.Show (battleWin, null , Teamdef );
                    }
					
				} 
				else if (tempData.currentBattleType == TemporyData.BattleType.TianXiaDiYiBattle ||
					tempData.currentBattleType == TemporyData.BattleType.QiangDuoGoldBattle ||
					tempData.currentBattleType == TemporyData.BattleType.SuDiBattle ||
					tempData.currentBattleType == TemporyData.BattleType.QiangDuoDragonBallBattle ||
					tempData.currentBattleType == TemporyData.BattleType.Revenge) 
				{

					///
					/// 新手引导
					///
					if(Core.Data.guideManger.isGuiding) {
						Core.Data.guideManger.DelayAutoRun(0.3F);
					}

                    mReward.Show (battleWin, tempData.warBattle.ext, tempData.warBattle.reward, tempData.warBattle.battleData.df );
					Core.Data.AccountMgr.SetBattleResult(battleWin);
				} 
				else if (tempData.currentBattleType == TemporyData.BattleType.FinalTrialBuou ||
					tempData.currentBattleType == TemporyData.BattleType.FinalTrialShalu ) 
				{
                    if (battleWin)
                    {
                        if(tempData.warBattle.reward != null )
                        {
                            mReward.SetCombe( tempData.FingerTotalCombo.ToString() ,  tempData.warBattle.reward.eco.ToString() );
                        }else
                        {
                            mReward.SetCombe( tempData.FingerTotalCombo.ToString() ,  "0");

                        }

                    }
					//battleWin = tempData.pass == 1;
                    if ( tempData.currentBattleType == TemporyData.BattleType.FinalTrialBuou)
                    {
                        mReward.Show (battleWin, 0 );

                    }else
                    {
                        mReward.Show (battleWin, 1 );

                    }
				} else if(tempData.currentBattleType == TemporyData.BattleType.PVPVideo) {
					int att = tempData.PvpVideo_Self_Attack;
					int def = tempData.PvpVideo_Enemy_Defend;
					int enemyLv= tempData.PvpVideo_Enemy_Lv;
					int selfLv = tempData.PvpVideo_Self_Lv;
					int whichside = tempData.PvpVideo_AttackOrDefense;
					int PvpVideo_IWin = tempData.PvpVideo_SelfWin;
					bool MyBussiness = tempData.isMyBussiness;
					battleWin = PvpVideo_IWin == 1;
					mReward.ShowVideo(battleWin, att, def, selfLv, enemyLv, whichside, MyBussiness);
				} else if(tempData.currentBattleType == TemporyData.BattleType.WorldBossWar)
                {
					// Debug.Log("世界boss");
					//世界boss
					battleWin = tempData.warBattle.battleData.iswin == 1;
					mReward.Show (battleWin);
				}

			}
		}
		skipBtn.setEnableBtn(true);
		//展示礼花
		if(battleWin) {
			DragonAoYiAnim.ShowFirework();
		}
		ShowReward(true);
		_replay = false;
	}

	void ShowReward(bool show)
	{
		if(mReward != null)
		{
			if(show)
			{
				mReward.transform.localPosition = Vector3.zero; //yangchenguang
				mReward.transform.localPosition += new Vector3(0,0,0);
			}
			else
			{
				mReward.transform.localPosition = Vector3.right*10000;
			}
		}
	}

	public void Replay()
	{
		// Debug.Log("开始处理战斗信息");
		ShowReward(false);
		//展示删除礼花
		DragonAoYiAnim.DestoryFireWork();
		//BanBattleProcess.Instance.HandleItem ();
		_list_Attack = null;
		_list_Defense = null;
		_replay = true;
		StartCoroutine(Start());
	}
}


//用于存储一个战斗角色
[System.Serializable]
public class BanBattleRole{

	//属性
	public enum Attribute{
		Jin,
		Mu,
		Shui,
		Huo,
		Tu,
		Unknown,
		Quan,

		SJin,
		SMu,
		SShui,
		SHuo,
		STu,
	}

	//阵营
	public enum Group{
		Attack,
		Defense
	}

	public BanBattleRole(int index,int number, Attribute attribute, int level, int attackBP, int defenseBP, Group group, int curBp, bool allFated = false){
		this.index = index;
		this.number = number;
		this.attribute = attribute;
		this.level = level;
		this.attackBP = attackBP;
		this.defenseBP = defenseBP;
		this.group = group;
		this.curBp = curBp;
		this.AllFated = allFated;
		//备份一个初始的属性，有可能某系卡牌会有属性转换
		this.initAttri = attribute;
	}

	//索引
	public int index;
	//编号
	public int number;
	//属性
	public Attribute attribute;
	//备份一个初始的属性，有可能某系卡牌会有属性转换
	public Attribute initAttri;

	//等级
	public int level;
	//攻击战斗力
	public int attackBP;
	//防御战斗力
	public int defenseBP;
	//阵营
	public Group group;
	//当前战斗力
	public int curBp;
	//当前的缘是否配齐
	public bool AllFated;
	//释放是boss
	public short isBoss;
	//当前的卡牌的技能信息
	public SkillObj[] angSkObj;
	//卡牌的普通技能信息
	public SkillObj[] norSkObj;

	//最终的最高战斗力
	public int finalBp {
		get{
			if(group == Group.Attack){
				return attackBP;
			}else{
				return defenseBP;
			}
		}
	}

}