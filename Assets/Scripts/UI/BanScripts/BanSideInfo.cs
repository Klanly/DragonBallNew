using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BanSideInfo : MonoBehaviourEx {

    private const int MAX_HP = 1000000;

	#region UI

	//技能Icon的控制器
	public ShowSkillMgr ssMgr;

	//缓动血条
	public BanHpBar hpBar;
	/// <summary>
	/// 顶层血条
	/// </summary>
	public BanHpBar _topHpBar;

	//上方的那个头像位置
	public GameObject go_CurRoleIconPos;

	//怒气槽
    public AngryBar angrySlot;

	//角色数量（如3/5）
	public UILabel text_RoleNum;

	public GameObject goHead;

	//玩家名称
	//public UILabel text_PlayerName;

	//当前宠物战斗力
	public UILabel text_CurRoleBattlePoint;

	//攻防标示
	public UISprite[] atkDefIcon;

	//当前角色图标
	private BanBattleRoleIcon _curRoleIcon = null;

    //伤害掉血
    public Transform DmgPos;

	//怒气最终飞向的目标
	public Transform AngerTarget;

	//建筑物的位置要设定
	public GameObject FightBuilding;

    //一共有多少个人员
    public int Count {
        get {
            return list_Team.Count;
        }
    }

	//当前角色图标, 上面的图标
	public BanBattleRoleIcon curRoleIcon {
		get{
			if(_curRoleIcon == null){
				_curRoleIcon = GenerateRoleIcon(go_CurRoleIconPos.transform.position, go_CurRoleIconPos.transform);
                _curRoleIcon.go_RoleName.SetActive(false);
				if(!IsAttack) _curRoleIcon.text_Level.gameObject.SetActive(false);
			}
			return _curRoleIcon;
		}
	}

	//当前正在场上的队员
	public BanBattleRoleIcon curRole {
		get {
			if(list_Team.Count == 0) return null;
			else {
				return list_Team[0];
			}
		}
	}

	//队伍图标起始点
	public GameObject go_TeamStartPos;

	//下方的图标
	private List<BanBattleRoleIcon> list_Team = new List<BanBattleRoleIcon>();

	//是否是攻击方
	public bool IsAttack = true;

	//队伍图标间隔
	public float teamMemberGap = 3;

    private float LocalTeamGap = 88f;

	public Vector3 _force = new Vector3(-8f,40,0f);

	public float _damageDurtime = 0.5f;

    public EffectManager skillNameEffect;

    public UISprite BgTeam;
    public UISprite BgAttOrDef;

	#endregion

	#region Data

	//最大战斗力点数
    private int _maxBp;
	
	private int _curBp;

	#if VS
	/// <summary>
	/// 伤害数字.
	/// </summary>
	private BattleDamageText _btText;
	float animSpeed = 0.15f;

	private Vector3 _startPos;
	private Vector3 _endPos;
	private Vector3 _midPos;
	#endif

    //记录下右边的攻防UI
    private string SpriteName = string.Empty;
    //1代表右边是攻击，-1代表是防守
    private int leftAttOrDef {
        get {
            if(SpriteName == "BattleUI_Attack") return 1;
            else return -1;
        }
    }

	//当前战斗力点数(有动画形式)
    public int curBp {
        get {
			return _curBp;
		}
        set {
            int changed = value - _curBp;
			_curBp = value;
            text_CurRoleBattlePoint.text = _curBp.ToString();

			startLooksBP = looksBP;
			startSetBpTime = Time.realtimeSinceStartup;
			if(leftAttOrDef <0) 
				if(hpBar!= null)
					hpBar.SpriteHp.spriteName = "xuetiao";
			hpBar.value = (float)curBp/maxBp;


            ShowDamage (changed);
			StartCoroutine(ShowHpChgAnim(changed));
		}
	}

    //重新设定当前战斗力的值
    public int maxAndCurBp {
        set {
            _maxBp = value;
            curBp = _maxBp;
        }
    }

    //返回最大的血量（或者说攻击值）
    public int maxBp {
        get {
            return _maxBp;
        }
    }


    /// <summary>
    /// 记录下此次是否为对决伤害, Update后都复位为false
    /// </summary>
    private bool isVsDmg = false;
    public bool isFightVsDamage {
		set { isVsDmg = value; }
        get {
            bool tmp = isVsDmg;
            return tmp;  
        }

    }

	#region 血量改变动画

    private DamageShowEx CachedShow;
	//设置掉血
    void ShowDamage(int change) {
        if(change != 0) {

			#if VS
			_btText.SetDamage (change);
			StartCoroutine(AnimateBezier());
			#else
			if(CachedShow != null) CachedShow.UpImmediateAnim();

			Object obj = null;
			if(IsAttack) 
				obj = PrefabLoader.loadFromUnPack("Ban/DamageLeft", false);
			else
				obj = PrefabLoader.loadFromUnPack("Ban/DamageRight", false);

			GameObject go = Instantiate(obj) as GameObject;
			DamageShowEx ds = go.GetComponent<DamageShowEx>();
			CachedShow    = ds;
			ds.setDamage(leftAttOrDef, IsAttack, change, DmgPos, isFightVsDamage);

			Invoke("SlowReduceAnim", 3F);
			Invoke("InOneSeconds", 0.95f);
			#endif
        }
	}

	#if VS
	//伤害数字动画
	IEnumerator AnimateBezier() {
		_btText.gameObject.SetActive (true);

		_btText.SetBezier (_startPos, _endPos, _midPos);

		_btText.transform.localPosition = _startPos;
		_btText.transform.localScale = new Vector3(0.5f, 0.5f, 1f);

		TweenScale.Begin (_btText.gameObject, 0.5f, Vector3.one);
		Vector3 toPos = _btText.transform.localPosition;
		toPos.x -= 150f;
		toPos.y += 80f;
		yield return new WaitForSeconds (0.25f);
	}

	#endif

    void InOneSeconds() {
        CachedShow = null;
    }

	void SlowReduceAnim() {
		if(isFightVsDamage) isFightVsDamage = false;
	}

	IEnumerator ShowHpChgAnim(int change) {
		if(change != 0) {
			Vector3 normal = Vector3.one;
			Vector3 large  = Vector3.one;
			if(IsAttack) {
				large = new Vector3(4f, 4f, 4f);
				normal= new Vector3(2.5782f, 2.5782f, 2.5782f);
			} else {
				large = new Vector3(-4f, 4f, 4f);
				normal= new Vector3(-2.5782f, 2.5782f, 2.5782f);
			}
			MiniItween.ScaleTo(text_CurRoleBattlePoint.gameObject, large, 0.1f, MiniItween.EasingType.EaseOutCubic);
			yield return new WaitForSeconds(0.1f);
			MiniItween.ScaleTo(text_CurRoleBattlePoint.gameObject, normal, 0.1f, MiniItween.EasingType.EaseInCubic);
		}
	}

	#endregion

	public void ShowSkillName(string name)
	{
		//BanTools.Log ("ShowSkillName = "+name);
        skillNameEffect.Text = name;
        skillNameEffect.PlayAnimation();
	}

	//设置战斗力点数（没有动画的形式）
	private void SetBp(int curBp,int maxBp){
		this._curBp = curBp;
		this._maxBp = maxBp;
        if(this._curBp >= MAX_HP)
            text_CurRoleBattlePoint.text = "???";
        else
            text_CurRoleBattlePoint.text = curBp.ToString();

		hpBar.value = (float)curBp/maxBp;
		this._topHpBar.value = (float)curBp/maxBp;
	}

	#endregion

	#region 缓慢动的血条
	private float startLooksBP;
	private float looksBP;
	private float startSetBpTime;

	void Update(){
		//为了血条实现动画
		if (looksBP != curBp) {
			float time = 0f;
			if(isFightVsDamage)
				time = BanTimeCenter.F_HP_SLOW_ANIM;
			else 
				time = BanTimeCenter.F_HP_ANIM;

			looksBP = Mathf.Lerp (startLooksBP, curBp, Mathf.Clamp01 ((Time.realtimeSinceStartup - startSetBpTime) / time));

			if(looksBP >= MAX_HP)
				text_CurRoleBattlePoint.text = "???";
			else
				text_CurRoleBattlePoint.text = "" + (int)looksBP;

			_topHpBar.value = looksBP / (float)_maxBp;
		} else {
			looksBP = curBp;

			if(looksBP >= MAX_HP)
				text_CurRoleBattlePoint.text = "???";
			else
				text_CurRoleBattlePoint.text = "" + (int)looksBP;

			_topHpBar.value = looksBP / (float)_maxBp;
		}
	}

	#endregion

	//死掉一个宠物，去除他的图标
	public void RemoveFirstRoleIcon(float delayTime){
		StartCoroutine(RemoveFirstRoleIconIE(delayTime));
	}

	IEnumerator RemoveFirstRoleIconIE(float delayTime){
		yield return new WaitForSeconds(delayTime);

      
        if( getAliveCount > 0){
			float disappearTime = 0.6f;
			BanBattleRoleIcon aIcon = list_Team[0];
            TweenAlpha.Begin(aIcon.gameObject,disappearTime, 0.6f);
            MiniItween.MoveTo(aIcon.gameObject, GetTeamIconLocalPos(list_Team.Count - 1), disappearTime, MiniItween.EasingType.EaseInQuint, false );

			for(int i = 1;i<list_Team.Count;i++){
                MiniItween.MoveTo(list_Team[i].gameObject, GetTeamIconLocalPos(i-1), disappearTime, MiniItween.EasingType.EaseInQuint, false).b_handleScaleTime = true;
			}

            aIcon.toDie();
			list_Team.RemoveAt(0);
            list_Team.Add(aIcon);
            ReadRoleNum(getAliveCount , list_Team.Count); // yangchenguang 战斗队伍士兵显示字符串修改

            yield return new WaitForSeconds(disappearTime);
		}
	}

    private int getAliveCount {
        get {
            int alive = 0;
            foreach(BanBattleRoleIcon aIcon in list_Team) {
                if(aIcon != null && !aIcon.isDie) {
                    alive += 1;
                }
            }
            return alive;
        }
    }

	/// <summary>
	/// 复活角色，复活的时候，需要重新排列队伍
	/// </summary>
	/// <param name="reviveArray">Revive array.</param>
	/// <param name="baseIndex">Base index.</param>
    public void ReviveRole(int[] reviveArray, int baseIndex = 0) {
        float disappearTime = 0.6f;
        List<BanBattleRoleIcon> deadArray = new List<BanBattleRoleIcon>();
		List<BanBattleRoleIcon> AliveArray = new List<BanBattleRoleIcon>();
        

        foreach(int pos in reviveArray) {
        
			foreach(BanBattleRoleIcon icon in list_Team) {
				if(icon != null && icon.banBattleRole.index == pos){
					icon.toLive();
					TweenAlpha.Begin(icon.gameObject, disappearTime, 1.0f);
				}
            }
        }


		foreach(BanBattleRoleIcon aIcon in list_Team) {
			if(aIcon != null) {
				if(aIcon.isDie)
					deadArray.Add(aIcon);
				else 
					AliveArray.Add(aIcon);
			}
		}

		//跟新当前活着的人数
		ReadRoleNum(AliveArray.Count, list_Team.Count);

		AliveArray.AddRange(deadArray);
		list_Team = AliveArray;

		int length = list_Team.Count;
		for(int i = 0; i < length; i++) {
			BanBattleRoleIcon tempRole = list_Team[i];
			Vector3 pos = GetTeamIconPos(i);
			tempRole.transform.position = pos;
		}

    }

	//读取第一个角色
	public void ReadFirstRole(BanBattleRole firstRole){
		SetBp(firstRole.finalBp,firstRole.finalBp);
		curRoleIcon.ReadData(firstRole);

		SkillObj aSk = null;
		if(firstRole.angSkObj != null) aSk = firstRole.angSkObj.Value<SkillObj>(0);

		ssMgr.Attend(firstRole.number, firstRole.level, aSk, firstRole.norSkObj);

		if(IsAttack) {
			MonsterManager monMgr  = Core.Data.monManager;
			MonsterData md = monMgr.getMonsterByNum(firstRole.number);
			PlayerAngryBtn.skillId = md == null ? 0 : md.skill.Value<int>(2);
			PlayerAngryBtn.skillLv = aSk == null ? 1 : aSk.skillLevel;
		}
	}

	//读取角色数量(用于显示下方数字)
	public void ReadRoleNum(int cur,int total){
		text_RoleNum.text = cur+"/"+total;

		float HeadX = goHead.transform.localPosition.x;
		float NumX  = text_RoleNum.transform.localPosition.x;

		if(total == 1) {
			goHead.transform.localPosition = new Vector3(HeadX, -136F, 0F);
			text_RoleNum.transform.localPosition = new Vector3(NumX, -136F, 0F);
			if(FightBuilding != null) FightBuilding.transform.localPosition = new Vector3(3, -180F, 0F);
			BgTeam.height = 136;
		} else if(total == 2) {
			goHead.transform.localPosition = new Vector3(HeadX, -210F, 0F);
			text_RoleNum.transform.localPosition = new Vector3(NumX, -210F, 0F);
			if(FightBuilding != null) FightBuilding.transform.localPosition = new Vector3(3, -256F, 0F);
			BgTeam.height = 210;
		} else {
			//left it there
		}
	}

	//根据索引返回图标位置
    //返回的是世界坐标
	Vector3 GetTeamIconPos(int index){
		if(IsAttack){
            return go_TeamStartPos.transform.position + Vector3.down * index * teamMemberGap;
		}else{
            return go_TeamStartPos.transform.position + Vector3.down * index * teamMemberGap;
		}
	}

    //根据索引返回图标位置
    //返回的是相对坐标
    Vector3 GetTeamIconLocalPos(int index) {
        if(IsAttack) {
            return Vector3.down * index * LocalTeamGap;
        } else {
            return Vector3.down * index * LocalTeamGap;
        }
    }

    void LeftIsAttOrDef () {
        TemporyData temp = Core.Data.temper;
        string spriteName = string.Empty;


        if (IsAttack) { //右边
            if(temp.currentBattleType == TemporyData.BattleType.FinalTrialShalu) {
                spriteName = "BattleUI_Attack";
            } else if(temp.currentBattleType == TemporyData.BattleType.FinalTrialBuou) {
                spriteName = "BattleUI_Defense";
            } else if(temp.currentBattleType == TemporyData.BattleType.BossBattle) {
#if NEWPVE
				NewFloor floor = Core.Data.newDungeonsManager.curFightingFloor;
                bool att = floor.config.FightType == 0;
#else
				Floor floor = CityFloorData.Instance.currFloor;
				bool att= floor.config.gf == 1;
#endif
                spriteName = att ? "BattleUI_Attack" : "BattleUI_Defense";

            } else if(temp.currentBattleType == TemporyData.BattleType.PVPVideo) {
                if(temp.PvpVideo_AttackOrDefense == 1)
                    spriteName = "BattleUI_Attack";
                else 
                    spriteName = "BattleUI_Defense";
            } else {
                spriteName = "BattleUI_Attack";
            }

            for (int i = 0; i < atkDefIcon.Length; i++) {
                atkDefIcon[i].spriteName = spriteName;
            }
			//wxl
//			if (spriteName == "BattleUI_Defense"){
//				if (hpBar != null) {
//					hpBar.SpriteHp.spriteName = "nvqitiao";
//					if (_topHpBar != null) {
//						_topHpBar.SpriteHp.spriteName = "nvqitiao";
//						_topHpBar.SpriteHp.color = Color.gray;
//					}
//				}
//			}
		

            this.SpriteName = spriteName;
        } else {
            if(temp.currentBattleType == TemporyData.BattleType.FinalTrialShalu) {
                spriteName = "BattleUI_Defense";
            } else if(temp.currentBattleType == TemporyData.BattleType.FinalTrialBuou) {
                spriteName = "BattleUI_Attack";
            } else if(temp.currentBattleType == TemporyData.BattleType.BossBattle) {
#if NEWPVE
				NewFloor floor = Core.Data.newDungeonsManager.curFightingFloor;
                bool att = floor.config.FightType == 0;               
#else
				Floor floor = CityFloorData.Instance.currFloor;
				bool att= floor.config.gf == 1;
#endif
                spriteName = att ? "BattleUI_Defense" : "BattleUI_Attack";

            } else if(temp.currentBattleType == TemporyData.BattleType.PVPVideo) {
                if(temp.PvpVideo_AttackOrDefense == 1)
                    spriteName = "BattleUI_Defense";
                else 
                    spriteName = "BattleUI_Attack";
            } else {
                spriteName = "BattleUI_Defense";
            }

            for (int i = 0; i < atkDefIcon.Length; i++) {
                atkDefIcon[i].spriteName = spriteName;
            }
			//wxl 
//			if (spriteName == "BattleUI_Defense") {
//				if (hpBar != null) {
//					hpBar.SpriteHp.spriteName = "nvqitiao";
//					if (_topHpBar != null) {
//						_topHpBar.SpriteHp.spriteName = "nvqitiao";
//						_topHpBar.SpriteHp.color = Color.gray;
//					}
//				}
//
//			}
			this.SpriteName = spriteName;

        }

        if(SpriteName == "BattleUI_Attack") {
            BgTeam.spriteName = "gongjidi";
            BgAttOrDef.spriteName = "gongjitiao";

			text_CurRoleBattlePoint.color = Color.white;//new Color(1.0f, 0.4f, 0, 1.0f);
        } else {
            BgTeam.spriteName = "fangyudi";
            BgAttOrDef.spriteName = "fangyutiao";

			text_CurRoleBattlePoint.color = Color.white;//new Color(0, 0.792f, 1.0f, 1.0f);
        }
    }


	//读取资料(角色信息，玩家名称)
    public void ReadData(List<BanBattleRole> list,string playerName){
        LeftIsAttOrDef();

        ReadFirstRole (list[0]);

        ReadRoleNum (list.Count,list.Count);

		//Generate Team
        if(BanBattleManager.Instance.IsReplay) {
            foreach(BanBattleRoleIcon icon in list_Team) {
                GameObject.Destroy(icon.gameObject);
            }
            list_Team.Clear();

            angrySlot.curAP = 0;
        } 

        for(int i = 0; i < list.Count; i++) {

            BanBattleRole tempRole = list[i];
            Vector3 pos = GetTeamIconPos(i);

            BanBattleRoleIcon tempRoleIcon = GenerateRoleIcon(pos, go_TeamStartPos.transform);
            tempRoleIcon.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            if(i == 0){
                tempRoleIcon.ReadData(tempRole,true);
            }else{
                tempRoleIcon.ReadData(tempRole);
            }

            tempRoleIcon.go_RoleName.SetActive(false);
			if(!IsAttack) tempRoleIcon.text_Level.gameObject.SetActive(false);
            list_Team.Add( tempRoleIcon );
        }

	}

	//生成一个角色图标(位置，父节点)
	public BanBattleRoleIcon GenerateRoleIcon(Vector3 pos,Transform parent){
        return BanTools.CreateNGUIOject(BanBattleManager.Instance.prefab_RoleIcon, pos, Quaternion.identity, parent).GetComponent<BanBattleRoleIcon>();
	}

    //变为全属性
    public void ChangeToAllAttribute() {
        curRoleIcon.changeToAllAttribute();

        BanBattleRoleIcon curRole = list_Team[0];
        if(curRole != null) {
            curRole.changeToAllAttribute();
        }
    }

	void OnDestory() {
		base.dealloc();
	}

    #region 控制怒气技能图标的

	AccountConfigManager accMgr = null;

    /// <summary>
    /// 当前怒气技能的最大点击次数
    /// </summary>
    [HideInInspector]
    public int MaxSkillCount = 0;

    /// <summary>
    /// 当前怒气技的信息
    /// </summary>
    [HideInInspector]
    public SkillData curAnSk = null;

    /// <summary>
    /// 展示出怒气技的按钮
    /// </summary>
    public AngrySkill_Cast PlayerAngryBtn;

    /// <summary>
    /// 展示出怒气技文字
    /// </summary>
    public AngryWord       PlayerAngryWord;

    private int cachedClickCount;

	void Start() {
		accMgr = Core.Data.AccountMgr;

		#if VS
		Object obj = PrefabLoader.loadFromPack("DamageNumber/pbDamageNumberUI", true);
		GameObject go = Instantiate(obj) as GameObject;
		//Vector3 Pos = IsAttack ? new Vector3(-386F, 0F, 0F) : new Vector3(0F, 0F, 0F);
		RED.AddChild(go, gameObject);

		_btText = go.GetComponent<BattleDamageText>();
		if(!IsAttack) _btText.reserveGrid();

		if(IsAttack)
			_startPos = new Vector3 (-350F, 40f, 0f); 
		else 
			_startPos = new Vector3 (-250F, 40f, 0f); 

		if (IsAttack)
			_endPos = new Vector3 (-400f, 110f, 0f);
		else
			_endPos = new Vector3 (-315f, 110f, 0f);

		_midPos = new Vector3 (1f, 90f, 0f); 
		#endif
	}

    public void OnAngryBtnClick() {
        if(curAnSk == null) return;

		Core.Data.soundManager.BtnPlay(ButtonType.Confirm);
        bool added = PlayerAngryWord.ShowCount(MaxSkillCount, curAnSk);
        if(added) {
            angrySlot.curAP -= curAnSk.param.nuqi;
        }
    }

    /// <summary>
    /// 隐藏连接次数的信息
    /// </summary>
    public void HideBtnClick() {
        curAnSk = null;
        MaxSkillCount = 0;
        cachedClickCount = PlayerAngryWord.clickTimes;

        #region 统计一场战斗的连击数
        Core.Data.temper.TotalCombo += cachedClickCount;
        Core.Data.temper.MaxCombo   = cachedClickCount;
        #endregion

        PlayerAngryWord.Hide();
		CancelInvoke();
        //因为PlayerAngryWord.ShowCount是个Coroutine方法，所以没办法很快的中断显示
		AsyncTask.QueueOnMainThread( () => { if(PlayerAngryWord != null) PlayerAngryWord.Hide(); }, 1f);
    }

	/// <summary>
	/// UI准备显示点击按钮的状态,
	/// 目前也准备自动点击的功能
	/// </summary>
	public void InitBtnClick() {
		cachedClickCount = 0;

		InvokeRepeating("autoClick", 0.6f, 0.1f);
	}

	void autoClick() {
		if(accMgr == null) accMgr = Core.Data.AccountMgr;
		if(accMgr.UserConfig.AutoBat == (short)1) {
			AutoAngryClick();
		}
	}

	void AutoAngryClick() {
		if(curAnSk == null) return;

		bool added = PlayerAngryWord.ShowCount(MaxSkillCount, curAnSk);
		if(added) {
			angrySlot.curAP -= curAnSk.param.nuqi;
		}
	}

	/// <summary>
	/// UI 还有下一次怒气点击事件吗？
	/// </summary>
	public bool hasNextClick {
		get {
			//判定当前的怒气能否支持下一次的点击
			int curClick = PlayerAngryWord.clickTimes;
			if(curClick >= MaxSkillCount) {
				return false;
			}

			return true;
		}
	}

    public int getTotalClickCount {
        get {
            return cachedClickCount;
        }
    }

    #endregion
} 