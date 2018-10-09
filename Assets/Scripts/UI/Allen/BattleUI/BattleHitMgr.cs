using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class BattleHitMgr : MonoBehaviour {
	//点击的次数
    private int Count;
	//单价
	private int mPerPrice = 0;

    public static BattleHitMgr Instance;

	//--- 手指
    public GameObject GoBtnLeft;
	//---- 展示数量
    public GameObject GoCount;
	//展示数量的动画
	public ShowBattleHitCount showCountAnim;

	/// <summary>
	/// 左边的效果，右边的效果
	/// </summary>
	public UISprite leftLight;
	public UISprite RightLight;

	/// 
	/// 动画的参数
	/// 
	public float FadeOutTime = 0.1f;
	public Color FadeOutColor = new Color(1.0f, 1.0f, 1.0f, 0.125f);
	public Vector3 FadeOutScale = new Vector3(2.0f, 2.0f, 2.0f);

	//--- 淡出的位置 ---
	public Vector3 RigthFadeOutPos;
	public Vector3 LeftFadeOutPos;
	//记录下来淡入的位置
	private Vector3 RightFadeInPos;
	private Vector3 LeftFadeInPos;

	/// <summary>
	/// Is Button type?
	/// </summary>
	private bool isBtnType;

    //是否可以展现Overskill的按钮
    private bool OverSkill_Appear;

    //记录下每个己方释放OverSkill的Combo数
    private List<int> recordedCombo;

    //记录当前第几个OverSkill，战斗回放使用
    [HideInInspector]
    public int curComboIndex;

	//返回点击次数
	public int getCount {
		get {
			return Count;
		}
	}

    void Awake() {
        Instance = this;
        OverSkill_Appear = false;
        recordedCombo = new List<int>();
		isBtnType = leftLight != null && RightLight != null;
    }

	// Use this for initialization
	void Start () {
        Count = 0;
        curComboIndex = 0;
        gameObject.SetActive(false);

		if(isBtnType) {
			RightFadeInPos = RightLight.transform.localPosition;
			LeftFadeInPos = leftLight.transform.localPosition;
		}
	}
	
    //展示点击的按钮
	//PerPrice 连击的单价
	public void ShowPressButton(int PerPrice, int count = 0) {
		mPerPrice = PerPrice;
        Count = count;
        gameObject.SetActive(true);
        //显示点击按钮
		GoBtnLeft.gameObject.SetActive(true);
		GoCount.gameObject.SetActive(false);

		if(isBtnType) {
			InvokeRepeating("RepeatPress", 0f, 0.1f);
		}

        OverSkill_Appear = true;
        //Invoke("HideWhenOverSkillStart", BanTimeCenter.F_Hide_Btn);
    }

    //告诉当前的ComboCount
    public int NotifyComboCount() {
        OverSkill_Appear = true;
        if(curComboIndex >= recordedCombo.Count) 
            return 0;
        else
            return recordedCombo[curComboIndex ++];
    }

    //隐藏所有的东西
	//这个方法会被调用，只要有FlyGo，而不仅仅是怒气技能
	public void HideWhenOverSkillOver(int result, BanBattleProcess.Item curItem) {
		if(OverSkill_Appear) { //怒气技释放的时候结束
			analyzeStatus(result, curItem);
			Invoke("HideAll", 2f);
		} else {
			//其他情况
			enterStatus(result, curItem);
		}
    }

	//立刻隐藏所有的东西 -- ignore it now
	public void HideImmediatly() {
		HideAll();
	}

    void HideAll() {
		CancelInvoke();
        gameObject.SetActive(false);
        #region 统计一场战斗的连击数

		Core.Data.temper.FingerTotalCombo += Count;
		Core.Data.temper.FingerMaxCombo = Count;

        #endregion
        OverSkill_Appear = false;
        Count = 0;
    }

    //记录下统计Combo
    public void RecordCombo(int combo) {
        if(!Core.Data.temper.hasLiquidate)
            recordedCombo.Add(combo);
    }

    //隐藏按钮，当Overskill开始的时候
    public void HideWhenOverSkillStart() {
		GoBtnLeft.gameObject.SetActive(false);
    }

    //显示数量
    public void ShowCount() {
        if(OverSkill_Appear) {
            if(Core.Data.temper.hasLiquidate) {
                if(Core.Data.temper.SkipBattle) {
                    return;
                } else {
                    gameObject.SetActive(true);
                }
            } 
			GoCount.gameObject.SetActive(true);
            //累计
            Interlocked.Increment(ref Count);
			showCountAnim.showCount(Count, mPerPrice);
        } 
    }
		
	#region 动画代码

	void RepeatPress() {
		StartCoroutine(pressOnce());
	}

	//play animation
	IEnumerator pressOnce() {
		leftLight.gameObject.SetActive(true);
		RightLight.gameObject.SetActive(true);

		leftLight.transform.localScale = Vector3.one;
		RightLight.transform.localScale = 0.6f * Vector3.one;

		//reset Pos 
		leftLight.transform.localPosition = LeftFadeInPos;
		RightLight.transform.localPosition = RightFadeInPos;
		//reset Color
		Color FadeInColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		leftLight.color = FadeInColor;
		RightLight.color = FadeInColor;

		//Change Position
		TweenPosition.Begin(leftLight.gameObject, FadeOutTime, LeftFadeOutPos);
		TweenPosition.Begin(RightLight.gameObject, FadeOutTime, RigthFadeOutPos);

		//Change Color
		TweenColor.Begin(leftLight.gameObject, FadeOutTime, FadeOutColor);
		TweenColor.Begin(RightLight.gameObject, FadeOutTime, FadeOutColor);

		//Large scale
		TweenScale.Begin(leftLight.gameObject, FadeOutTime, FadeOutScale);
		TweenScale.Begin(RightLight.gameObject, FadeOutTime, FadeOutScale);

		yield return new WaitForSeconds(FadeOutTime + 0.1f);

	}

	#endregion

	#region 金币掉落动画
	///
	/// 小金币 
	///

	//金币最大掉落数量
	private const int smallLimit = 4;
	private int curSmall = 0;

	private GameObject[] Go_DropClone = null;
	GameObject getDropCoin() {
		if(isChildEmpty(Go_DropClone)) {
			Go_DropClone = new GameObject[smallLimit];

			Object obj = PrefabLoader.loadFromUnPack("Ban/DropCoin", false);

			for(int i = 0; i < smallLimit; ++ i) {
				Go_DropClone[i] = Instantiate(obj) as GameObject;
			}

			foreach(GameObject go in Go_DropClone) {
				go.SetActive(false);
			}
		}
		curSmall = (curSmall + 1) % smallLimit;
		return Go_DropClone[curSmall];
	}


	/// 
	/// 大金币  
	/// 

	//金币最大掉落数量
	private const int limit = 4;
	private int cur = 0;

	private GameObject[] Go_DropCloneBig = null;
	GameObject getDropCoinEx() {
		if(isChildEmpty(Go_DropCloneBig)) {
			Go_DropCloneBig = new GameObject[limit];

			Object obj = PrefabLoader.loadFromUnPack("Ban/DropCoinEx", false);

			for(int i = 0; i < limit; ++ i) {
				Go_DropCloneBig[i] = Instantiate(obj) as GameObject;
			}

			foreach(GameObject go in Go_DropCloneBig) {
				go.SetActive(false);
			}
		}
		cur = (cur + 1) % limit;
		return Go_DropCloneBig[cur];
	}

	/// <summary>
	/// 基本上现在不在掉落大把的金币，而是小金币
	/// </summary>
	GameObject getDropCoin(int count) {
		if(count <= 1000) {
			return getDropCoin();
		} else {
			return getDropCoinEx();
		}
	}

	public void showCoin() {
		if(OverSkill_Appear) {
			BanBattleManager battleMgr = BanBattleManager.Instance;
			if(battleMgr != null) {
				CRLuo_PlayAnim_FX Suffer = battleMgr.main3DManager.Man_R;

				if(Suffer != null) {
					GameObject coin = getDropCoin(Count);
					coin.SetActive(true);
					RED.AddChild(coin, Suffer.gameObject);
					float hight = Suffer.mainMeshRender.renderer.bounds.extents.y + Suffer.mainMeshRender.renderer.bounds.center.y;
					coin.transform.localPosition = new Vector3(0f, hight, 0f);
					Core.Data.soundManager.SoundFxPlay(SoundFx.FX_BrokeGold);
				}
			}
		}
	}

	bool isChildEmpty(GameObject[] GoArr) {
		bool empty = GoArr == null;
		if(empty) return empty;

		foreach(GameObject go in GoArr) {
			if(go == null) {
				empty = true;
				break;
			}
		}

		return empty;
	}

	#endregion


	#region 龙珠掉落
	const int RESULT_NONE = 0;
	const int RESULT_ATTACKER_WIN = 1;
	const int RESULT_DEFENDER_WIN = 2;
	const int RESULT_BOTH_WIN = 3;

	private const int Status_None    = -1;
	private const int Status_Record  = 0;
	private const int Status_Compare = 1;

	//直接打死
	private const int Status_DirectKill = 2;

	int status = Status_None;

	//进攻ID
	private int AttId = -1;
	//防御ID
	private int DefId = -1;
	//记录下Combo数量
	private int AttCombo = 0;

	//分析状态
	/// <summary>
	/// 只有这怒气技能的情况下，才能进入记录状态
	/// </summary>
	/// <param name="result">Result.</param>
	/// <param name="curItem">Current item.</param>
	void analyzeStatus(int result, BanBattleProcess.Item curItem) {
		status = result == RESULT_ATTACKER_WIN ? Status_DirectKill : Status_Record;
		if(status == Status_DirectKill) { //清空数据
			AttId = -1;
			DefId = -1;
		} else {      // 记录数
			AttId = curItem.attackIndex;
			DefId = curItem.defenseIndex;
		}
		AttCombo = Count;


		if(status == Status_DirectKill) {
			//直接杀死的时候掉落
			Invoke("dropBallEx", 1f);
		}
	}

	//只分析记录状态
	void enterStatus (int result, BanBattleProcess.Item curItem) {
		if(status == Status_Record) {
			if(result == RESULT_ATTACKER_WIN) { //只有进攻方赢则掉落龙珠
				if(AttId == curItem.attackIndex && DefId == curItem.defenseIndex) {
					Invoke("dropBall", 1.8f);
				}
			}
		}
	}


	//在左边死亡
	void dropBall() {
		BanBattleManager battleMgr = BanBattleManager.Instance;
		if(battleMgr != null) {
			bool drop = battleMgr.dropOneBall(AttCombo);
			AttCombo = 0; // clear cache
			if(drop) {
				CRLuo_PlayAnim_FX Suffer = battleMgr.main3DManager.Man_R;
				if(Suffer != null) {
					dropBallAnim(Suffer.gameObject);
				}
			}
		}
	}

	//在右边死亡
	void dropBallEx() {
		BanBattleManager battleMgr = BanBattleManager.Instance;
		if(battleMgr != null) {
			bool drop = battleMgr.dropOneBall(AttCombo);
			AttCombo = 0; // clear cache
			if(drop) {
				CRLuo_PlayAnim_FX Suffer = battleMgr.main3DManager.Man_R;
				if(Suffer != null) {
					dropBallAnim(Suffer.gameObject, false);
				}
			}
		}
	}

	void dropBallAnim(GameObject father, bool isInleftArea = true) {
		Object obj = PrefabLoader.loadFromUnPack("Ban/DropBall", false);
		GameObject ball = Instantiate(obj) as GameObject;
		RED.AddChildResvere(ball, father);

		if(isInleftArea) ball.transform.eulerAngles = new Vector3(0f, 0f, 0f);
		Core.Data.soundManager.SoundFxPlay(SoundFx.FX_DragonBeadDrop);
	}

	#endregion

}
