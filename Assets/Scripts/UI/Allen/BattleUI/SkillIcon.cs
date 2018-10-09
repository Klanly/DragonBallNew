using UnityEngine;
using System.Collections;
using System.Reflection;

public class SkillIcon : MonoBehaviour {

	private int skillId;
	private int skillLv;

	//该按钮是否显示
	private bool isOn;
	//是否满了30级别才来显示
	private bool isForbid;

	//绝不展示
	private bool NeverShow;

	public UISprite skillIcon;
	public UISprite mask;
	public UISprite Forbid;

	private TemporyData temp;
	void Awake() {
		temp = Core.Data.temper;

		if(temp.currentBattleType == TemporyData.BattleType.WorldBossWar) {
			gameObject.SetActive(false);
			NeverShow = true;
		} else {
			NeverShow = false;
		}
	}

	///
	///  每有人物登场的时候，有一个特效
	///  最后一个参数控制卡牌未满30级别的时候，第二个技能无效
	///
	public IEnumerator attendAnim(int skId, int iconId, int skLv, bool forbid = false) {
		yield return null;

		if(!NeverShow) {
			if(iconId == -1 || iconId == 0) isOn = false;
			else isOn = true;


			if(!isOn) {
				skillIcon.gameObject.SetActive(false);
				mask.color = new Color(1.0F, 1.0F, 1.0F, 0.8F);
			} else {
				skillIcon.gameObject.SetActive(true);
				skillIcon.spriteName = iconId.ToString();
				mask.color = new Color(1.0F, 1.0F, 1.0F, 0F);
			} 

			if(forbid) {
				if(Forbid) Forbid.gameObject.SetActive(true);
				mask.color = new Color(1.0F, 1.0F, 1.0F, 0.8F);
			} else {
				if(Forbid) Forbid.gameObject.SetActive(false);
			}

			skillId = skId;
			skillLv = skLv;

			//保存
			isForbid = forbid;
		}

	}

	/// <summary>
	/// 释放技能的时候
	/// </summary>

	public IEnumerator CastAnim(int castId) {
		yield return null;

		if(!NeverShow) {
			if(isOn && skillId == castId) {
				mask.color = Color.white;
				/// play anim
				Object obj = PrefabLoader.loadFromUnPack("Ban/NormalSkUI", false);
				GameObject go = Instantiate(obj) as GameObject;
				RED.AddChild(go, gameObject);
			}
		}

	}

	/// <summary>
	/// 技能动画播放完毕的时候，播放的动画
	/// </summary>

	public IEnumerator finishAnim(int castId) {
		yield return null;

		if(isOn && skillId == castId) {
			mask.color = Color.white;
			/// play anim
		}
	}

	/// <summary>
	/// 展示描述文件
	/// </summary>

	public void ShowDes() {
		///未来之战不展示
		if (temp.currentBattleType == TemporyData.BattleType.FightWithFulisa)  {
			return;
		}

		string lockInfo = string.Empty;
		StringManager strMgr = Core.Data.stringManager;

		if(skillId == 0 || skillId == -1) {
			lockInfo = strMgr.getString(33);
			BT_SkillDes.Instance.showTips(gameObject, string.Empty, string.Empty, lockInfo);
		} else {
			Skill sk = createSkill();
			if(sk != null) {
				string content = sk.EffecDescription;
				string name    = sk.sdConfig.name;
				if(isForbid) lockInfo = strMgr.getString(32);

				BT_SkillDes.Instance.showTips(gameObject, name, content, lockInfo);
			}
		}
	}

	public void HideDes() {
		BT_SkillDes.Instance.HideTips();
	}

	Skill createSkill() {
		SkillManager skMgr = Core.Data.skillManager;

		SkillData sd = skMgr.getSkillDataConfig(skillId);
		SkillOpData sod = skMgr.getSkillOpDataConfig(sd.op);
		SkillLvData lvd = skMgr.GetSkillLvDataConfig(skillId);

		return new Skill(sd, sod, lvd, skillLv);
	}

}
