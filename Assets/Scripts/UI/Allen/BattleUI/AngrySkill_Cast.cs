using UnityEngine;
using System.Collections;
using AW.Battle;

public class AngrySkill_Cast : MonoBehaviour {

    public UISprite Icon_BG;
    public UISprite Icon;
    public UILabel  txtPoint;
    //整个花费的点
    public GameObject goPoint;
    //粒子效果, ignore now
    public ParticleSystem goParticle;
	public ParticleSystem goPartExplore;

	//当前的怒气技能ID
	private int _skId = 0;
	[HideInInspector]
	public int skillId {
		get {
			return _skId;
		} 

		set {
			_skId = value;
			if(_skId == 0 || _skId == -1) {
				txtPoint.text = "0";
			} else {
				SkillManager skMgr = Core.Data.skillManager;
				SkillData sd = skMgr.getSkillDataConfig(_skId);
				if(sd != null) {
					txtPoint.text = sd.param.nuqi.ToString();
				} else {
					txtPoint.text = "0";
				}
			}

		}
	}

	//当前怒气技能的等级
	private int _skLv = 1;
	[HideInInspector]
	public int skillLv {
		get {
			return _skLv;
		}

		set {
			_skLv = value;
		}
	}

    //-------------- 动画的参数 ------------

    public string ___ = "动画的参数";
    //缩小到多少
    public Vector3 Min_Scale = new Vector3(0.6f, 0.6f, 0.6f);
    //缩放的时间
    public float Scale_Time  = 0.3f;
    //旋转一半的时间
    public float Half_Rotate_Time = 0.1f;
    //放大缩小的模式
    public MiniItween.EasingType ScaleType;
    //震动
    public Vector3 ShakeVec = new Vector3(0.1f, 0.1f, 0.1f);

    public float Shake_Time = 0.1f;

	//True == Open, False == Closed
	private bool isOpenOrClose = false;

	// Use this for initialization
	void Awake () {
        InitStatus();
	}
	
    //显示怒气技能的图标
    public void ShowSkill(SkillData skill) {
		isOpenOrClose = true;
        if(skill != null) {
            int skillIconId = skill.Icon;
            int NuQi        = skill.param.nuqi;

            StartCoroutine(ShowAnimationOpen(skillIconId, NuQi));
        }
    }


    /// <summary>
    /// 翻转动画，可以使用技能
    /// </summary>
    /// <returns>The animation.</returns>
    IEnumerator ShowAnimationOpen(int skillIconId, int NuQi) {
        Vector3 Quart = new Vector3(0f, 90f, 0f);
        Vector3 Half  = new Vector3(0f, 180f, 0f);

		//播放声效
		Core.Data.soundManager.SoundFxPlay(SoundFx.Btn2);
		//播放特效
		goPartExplore.gameObject.SetActive(true);

        //先放大
        MiniItween.ScaleTo(Icon_BG.gameObject, Min_Scale, Scale_Time, ScaleType);
        yield return new WaitForSeconds(Scale_Time);

        //再旋转
        MiniItween.RotateTo(Icon_BG.gameObject, Quart, Half_Rotate_Time);
        yield return new WaitForSeconds(Half_Rotate_Time);

        // 换贴图
        Icon_BG.spriteName = "nuqijizhengmian";
        //开始显示技能图标
        Icon.transform.localScale = Min_Scale;
        Icon.transform.localRotation = Quaternion.Euler(-Quart);
        Icon.gameObject.SetActive(true);
//        Icon.spriteName = skillIconId.ToString();

		Icon.gameObject.GetComponent<UIButton> ().normalSprite = skillIconId.ToString ();
        txtPoint.text   = NuQi.ToString();

        //再次旋转90°
        MiniItween.RotateTo(Icon_BG.gameObject, Half, Half_Rotate_Time);
        MiniItween.RotateTo(Icon.gameObject, Vector3.zero, Half_Rotate_Time);
        yield return new WaitForSeconds(Half_Rotate_Time);

        //显示粒子效果
        goParticle.gameObject.SetActive(true);

        //再次缩小回去
        MiniItween.ScaleTo(Icon_BG.gameObject, Vector3.one, Scale_Time, ScaleType);
        MiniItween.ScaleTo(Icon.gameObject, Vector3.one, Scale_Time, ScaleType);
        yield return new WaitForSeconds(Scale_Time);

        //震动
		Core.Data.soundManager.SoundFxPlay(SoundFx.Btn3);
        MiniItween.Shake(gameObject, ShakeVec, Shake_Time, MiniItween.EasingType.EaseOutCirc);
        //goPoint.SetActive(true);

		//1S后消除
		Invoke("hideExplore", 1f);
    }

	void hideExplore(){
		goPartExplore.gameObject.SetActive(false);
	} 

    /// <summary>
    /// 翻转动画，不可以使用技能
    /// </summary>
    /// <returns>The animation close.</returns>

    IEnumerator ShowAnimationClose(){
        Vector3 Quart = new Vector3(0f, 90f, 0f);

        //先关闭消耗怒气点信息，再次关闭特效
        //goPoint.SetActive(false);
        goParticle.gameObject.SetActive(false);

		Core.Data.soundManager.SoundFxPlay(SoundFx.Btn2);
        //先放大
        MiniItween.ScaleTo(Icon_BG.gameObject, Min_Scale, Scale_Time, ScaleType);
        MiniItween.ScaleTo(Icon.gameObject, Min_Scale, Scale_Time, ScaleType);
        yield return new WaitForSeconds(Scale_Time);

        //再旋转
        MiniItween.RotateTo(Icon_BG.gameObject, Quart, Half_Rotate_Time);
        MiniItween.RotateTo(Icon.gameObject, Quart, Half_Rotate_Time);
        yield return new WaitForSeconds(Half_Rotate_Time);

        //隐藏技能图标
        Icon.gameObject.SetActive(false);
        Icon.transform.localRotation = Quaternion.Euler(Vector3.zero);

        Icon_BG.spriteName = "nuqijibeimian";

        //再次旋转90°
        MiniItween.RotateTo(Icon_BG.gameObject, Vector3.zero, Half_Rotate_Time);
        yield return new WaitForSeconds(Half_Rotate_Time);

        //再次缩小回去
        MiniItween.ScaleTo(Icon_BG.gameObject, Vector3.one, Scale_Time, ScaleType);
        yield return new WaitForSeconds(Scale_Time);

        //震动
		Core.Data.soundManager.SoundFxPlay(SoundFx.Btn3);
        MiniItween.Shake(gameObject, ShakeVec, Shake_Time, MiniItween.EasingType.EaseOutCirc);
    }

    public bool HideSkill() {
		if(isOpenOrClose == false) 
			return false;
		isOpenOrClose = false;
        StartCoroutine(ShowAnimationClose());
		return true;
    }

    /// <summary>
    /// 恢复到初始状态
    /// </summary>

    void InitStatus() {
        //缩放先到初始值
        Icon_BG.gameObject.transform.localScale    = Vector3.one;
        Icon_BG.gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);

        goPoint.SetActive(true);
        Icon.gameObject.SetActive(false);
        Icon.gameObject.transform.localScale       = Vector3.one; 
        Icon.gameObject.transform.localRotation    = Quaternion.Euler(Vector3.zero);

        Icon_BG.spriteName = "nuqijibeimian";
        //关闭
        goParticle.gameObject.SetActive(false);
		goPartExplore.gameObject.SetActive(false);

		isOpenOrClose = false;
    }

	#region 展示Tip的功能

	void ShowTip() {
		if(isOpenOrClose == false) {
			TemporyData temp = Core.Data.temper;
			///未来之战不展示
			if (temp.currentBattleType == TemporyData.BattleType.FightWithFulisa)  {
				return;
			}
			string lockInfo = string.Empty;
			StringManager strMgr = Core.Data.stringManager;

			if(skillId == 0 || skillId == -1) {
				lockInfo = strMgr.getString(33);
				BT_SkillDes.Instance.showTips(Icon_BG.gameObject, string.Empty, string.Empty, lockInfo);
			} else {
				Skill sk = createSkill();
				if(sk != null) {
					string content = sk.EffecDescription;
					string name    = sk.sdConfig.name;
					BT_SkillDes.Instance.showTips(Icon_BG.gameObject, name, content, lockInfo);
				}
			}
		}
	}

	void HideTip() {
		BT_SkillDes.Instance.HideTips();
	}

	Skill createSkill() {
		SkillManager skMgr = Core.Data.skillManager;

		SkillData sd = skMgr.getSkillDataConfig(skillId);
		SkillOpData sod = skMgr.getSkillOpDataConfig(sd.op);
		SkillLvData lvd = skMgr.GetSkillLvDataConfig(skillId);

		return new Skill(sd, sod, lvd, skillLv);
	}

	#endregion

    /****  测试代码 
    void OnGUI() {
        if(GUI.Button(new Rect(0, 0, 100, 100), "Show Flipp")) {
            int NuQi = 20;
            int skillIconId = 21077;
            StartCoroutine(ShowAnimationOpen(skillIconId, NuQi));
        }

        if(GUI.Button(new Rect(0, 200, 100,100), "Init")) {
            StartCoroutine(ShowAnimationClose());
        }
    }*/

}
