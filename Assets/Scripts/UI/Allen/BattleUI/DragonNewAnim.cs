using UnityEngine;
using System;
using System.Collections;

using UObj = UnityEngine.Object;

[Flags]
public enum DragonAnimStatus {
	None            = 0x00,
	Att_Blood_Fire  = 0x01,
	Def_Blood_Fire  = 0x02,
	Att_Blood_Light = 0x04,
	Def_Blood_Light = 0x08,
}

public static class DragonStatusExt {
	public static bool check( this DragonAnimStatus flags, DragonAnimStatus totest ) {
		return (flags & totest) == totest;
	}

	public static DragonAnimStatus set(this DragonAnimStatus flags, DragonAnimStatus totest) {
		return flags | totest;
	}

	public static DragonAnimStatus clear(this DragonAnimStatus flags, DragonAnimStatus totest) {
		return flags & ~totest;
	}
}

/// <summary>
/// 新版本的神龙奥义，战斗时候的动画
/// </summary>
public class DragonNewAnim : MonoBehaviour {

	public enum AoYiEfType {
		Blood_Fire,
		Blood_Light,
	}

	public float Dragon_FadeOut_Time = 0.2f;

	//神龙消失的时间
	public float Dragon_Disappear_Time = 0.1f;

	//神龙出现的时间
	public float Dragon_Appear_Time = 0.1f;

	public float Dragon_Word_Appear_Time = 0.7f;

	//神龙文字时间
	public float Dragon_Word_Time = 0.4f;

	//移动红点的时间
	public float MoveRedPointTime = 1f;

	public float RedPointHideAlpha = 0.2f;

	public Vector3 RedPointHideScale = new Vector3(0.5f, 0.5f, 0);

	public Vector3 BG_DragonScale = new Vector3(2f, 2f, 2f);

	//延迟移动Icon的时间
	public float DelayMoveIcon = 0.2f;
	//延迟隐藏图标的时间
	public float DelayHideIcon = 0.5f;
	//设定帧率
	public int ExploreFrameRate = 10;
	//设定帧率
	public int BloodFrameRate = 10;

	//奥义的列表 --- 初始化的行为由其他的地方控制
	public DragonSkillManager Att_AoYiList;
	public DragonSkillManager Def_AoYiList;

	//奥义爆裂的动画
	public UISpriteAnimation Att_Explore;
	public UISpriteAnimation Def_Explore;

	/// <summary>
	/// 攻击方的血条上的效果
	/// </summary>
	public UISpriteAnimation Att_Blood_Fire;
	public UISpriteAnimation Att_Blood_Light;

	/// <summary>
	/// 防御方的血条上的效果
	/// </summary>
	public UISpriteAnimation Def_Blood_Fire;
	public UISpriteAnimation Def_Blood_Light;

	//龙出现
	public GameObject BigDragon;
	//主龙
	public UITexture ForeDragon;
	//虚幻的龙
	public UISprite BackDragon;
	//神龙的名字
	public EffectManager DragonWord;

	//记录当前神龙的状态
	private DragonAnimStatus Status;

	private Color CHidden = new Color(1.0f, 1.0f, 1.0f, 0.0f);

	public DragonAnimStatus curAnimStatus {
		get {
			return this.Status;
		}
	}

	// -------------  3D 神龙效果  ----------------

	private GameObject SelfEnhanced {
		get {
			UObj obj = PrefabLoader.loadFromUnPack("Ban/Lightning_Up",false);
			GameObject _enhanced = Instantiate(obj) as GameObject;
			return _enhanced;
		}
	}

	private GameObject Hurt {
		get {
			UObj obj = PrefabLoader.loadFromUnPack("Ban/Lightning_Down", false);
			GameObject _Hurt = Instantiate(obj) as GameObject;

			return _Hurt;
		}
	}

	// --------- 记录下初始的位置 ---------
	//红点爆炸的位置
	private Vector3 initAttRedPosition;
	private Vector3 initDefRedPosition;

	//奥义队列的位置
	private Vector3 initAttAoYiPosition;
	private Vector3 initDefAoYiPosition;

	private  int  atk_AYNum;
	private int def_AYNum;

	void Start() {
		initAttRedPosition = Att_Explore.transform.localPosition;
		initDefRedPosition = Def_Explore.transform.localPosition;

		initAttAoYiPosition = Att_AoYiList.AoYiList.transform.localPosition;
		initDefAoYiPosition = Def_AoYiList.AoYiList.transform.localPosition;
		SaveAoYiList ();
	}

	public void init() {
		//闪电和火要隐藏
		Att_Blood_Fire.gameObject.SetActive(false);
		Att_Blood_Light.gameObject.SetActive(false);

		//闪电和火要隐藏
		Def_Blood_Fire.gameObject.SetActive(false);
		Def_Blood_Light.gameObject.SetActive(false);
		//wxl 
		resetRedPoint(null,true);


	}


	void SaveAoYiList(){
		atk_AYNum = 0;
		def_AYNum = 0;
		for (int i = 0; i < Att_AoYiList.items.Length; i++) {
			if (Att_AoYiList.items [i].gameObject.activeSelf == true) {
				atk_AYNum++;
			}
		}
		for (int i = 0; i < Def_AoYiList.items.Length; i++) {
			if (Def_AoYiList.items [i].gameObject.activeSelf == true) {
				def_AYNum++;
			}
		}

	}
	void ResetAYList(){
		for (int i = 0; i < atk_AYNum; i++) {
			Att_AoYiList.items [i].gameObject.SetActive (true);
		}
		for (int i = 0; i < def_AYNum; i++) {
			Def_AoYiList.items [i].gameObject.SetActive (true);
		}

		Att_AoYiList.AoYiList.GetComponent<UIGrid> ().Reposition ();
		Def_AoYiList.AoYiList.GetComponent<UIGrid> ().Reposition ();


	}


	//重置红点的位置
	private void resetRedPoint(string aYName =null,bool resetAoYiList = false) {
		//位置
		int count = Att_AoYiList.items.Length;

		int posNum = -1;


		for (int i = 0; i < count; i++) {
			if (Att_AoYiList.items [i] != null) {
				if (Att_AoYiList.items [i].Icon.spriteName != null) {
					if (Att_AoYiList.items [i].gameObject.activeInHierarchy == true) {
						int tAoYiNum = int.Parse (Att_AoYiList.items [i].Icon.spriteName);
						AoYiData tData = Core.Data.dragonManager.getAoYiData (tAoYiNum);
						if (tData != null && tData.name == aYName) {
							posNum += (i+1);
							break;
						}
					} else {
						posNum--;	
					}
				}
			}
		}

		if (posNum == -1) {
			posNum = 0;
		}
		if(WhoAttack)
			Att_Explore.transform.localPosition = initAttRedPosition + (40 * posNum) * Vector3.right;
		else
			Def_Explore.transform.localPosition = initDefRedPosition + (40 * posNum) * Vector3.right;

//		Debug.Log ("     attt  pos  ===  " +Att_Explore.transform.localPosition  );


		if(resetAoYiList) {
			ResetAYList ();
//			Att_AoYiList.AoYiList.transform.localPosition = initAttAoYiPosition;
//			Att_AoYiList.AoYiList.transform.localPosition = initAttAoYiPosition;
//			Def_AoYiList.AoYiList.transform.localPosition = initDefAoYiPosition;
		}




		//颜色
		UISprite sprite = Att_Explore.GetComponent<UISprite>();
		sprite.color = Color.white;

		sprite = Def_Explore.GetComponent<UISprite>();
		sprite.color = Color.white;

		//大小
		Att_Explore.transform.localScale = Vector3.one;
		Def_Explore.transform.localScale = Vector3.one;

		//
		Att_Explore.gameObject.SetActive(false);
		Def_Explore.gameObject.SetActive(false);

		//龙要隐藏
		BigDragon.SetActive(false);
	}

	#region 显示血条动画

	private string AoYiName = string.Empty;
	private bool WhoAttack;
	private bool DamageEnemy;
	//遗气奥义，不播放火焰或闪电
	private bool SkipLightAndFire;
	//播放加血或者加气的动画
	private Action playApOrBpAnim;

	public void PlayDragon(bool attack, bool damageEnemy, int pos, string name, bool skipLightAndFire, Action LightOrFireComplete) {
		AoYiName = name;
		WhoAttack = attack;
		DamageEnemy = damageEnemy;
		this.SkipLightAndFire = skipLightAndFire;
		this.playApOrBpAnim = LightOrFireComplete;

		//	resetRedPoint();//wxl 

		resetRedPoint (name,false);

		AoYiExplore(attack);
		StartCoroutine(AoYiDisappear(attack,pos, name));
	}

	//红点爆裂的动画
	private void AoYiExplore(bool attack) {
		UISpriteAnimation explore = null;
		//爆裂的动画效果
		explore = attack ? Att_Explore : Def_Explore;

		explore.gameObject.SetActive(true);
		explore.framesPerSecond = ExploreFrameRate;
		explore.AnimationEndDelegate = AoYiExploreMove;
		explore.Forward();
	}

	void AoYiExploreMove(UISpriteAnimation whichone) {
		whichone.AnimationEndDelegate = null;

		UISprite explore = whichone.GetComponent<UISprite>();
		explore.spriteName = "blood6";
		MiniItween.MoveTo(explore.gameObject, Vector3.zero, MoveRedPointTime, true);
		MiniItween.ColorTo(explore.gameObject, new V4( new Color(1.0f, 1.0f, 1.0f, RedPointHideAlpha)), MoveRedPointTime, MiniItween.EasingType.EaseInQuad, MiniItween.Type.ColorWidget);
		MiniItween.ScaleTo(explore.gameObject, RedPointHideScale, MoveRedPointTime).FinishedAnim = () => { StartCoroutine(DragonAppear());  whichone.gameObject.SetActive(false); };
	}

	IEnumerator DragonAppear() {

		BigDragon.SetActive(true);
		BackDragon.gameObject.SetActive(false);
		ForeDragon.gameObject.SetActive(true);
		ForeDragon.color = CHidden;

		MiniItween.ColorTo(ForeDragon.gameObject, new V4(Color.white), Dragon_Appear_Time, MiniItween.EasingType.Linear, MiniItween.Type.ColorWidget);

		yield return new WaitForSeconds(Dragon_Word_Appear_Time);

		//显示奥义Name
		DragonWord.Text = AoYiName;
		DragonWord.PlayAnimation();

		yield return new WaitForSeconds(Dragon_Word_Time);

		BackDragon.gameObject.SetActive(true);
		BackDragon.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
		BackDragon.transform.localScale = Vector3.one;

		MiniItween.ScaleTo(BackDragon.gameObject, BG_DragonScale, Dragon_FadeOut_Time);
		MiniItween.ColorTo(BackDragon.gameObject, new V4(CHidden), Dragon_FadeOut_Time, MiniItween.EasingType.EaseInQuad, MiniItween.Type.ColorWidget);
		yield return new WaitForSeconds(Dragon_Disappear_Time);
		ForeDragon.gameObject.SetActive(false);

		//跳过闪电或火焰吗？
		if(!SkipLightAndFire) {
			Main3DManager main3d = BanBattleManager.Instance.main3DManager;
			Vector3 pos = Vector3.zero;
			if(DamageEnemy) {
				ShowFireOrLight(!WhoAttack, AoYiEfType.Blood_Light);

				if(!WhoAttack) {
					pos = main3d.Man_L.transform.position;
					main3d.Free2_Ban(true);
				} else {
					pos = main3d.Man_R.transform.position;
					main3d.Free2_Ban(false);
				}

				Hurt.transform.position = pos;
			} else {
				ShowFireOrLight(WhoAttack, AoYiEfType.Blood_Fire);

				if(WhoAttack) {
					pos = main3d.Man_L.transform.position;
					main3d.Free1_Ban(true);
				} else {
					pos = main3d.Man_R.transform.position;
					main3d.Free1_Ban(false);
				}

				SelfEnhanced.transform.position = pos;
			}
		}

		//播放结算的画面
		if(playApOrBpAnim != null) playApOrBpAnim();

	}

	//奥义Icon消失并且移动的效果
	private IEnumerator AoYiDisappear(bool attack, int pos ,string name = null ) {
		DragonSkillManager mgr = null;
		mgr = attack ? Att_AoYiList : Def_AoYiList;

		yield return new WaitForSeconds(DelayHideIcon);
		yield return StartCoroutine(mgr.HideIcon(pos, DelayMoveIcon,name));
	}


	/// <summary>
	/// 第一个表示攻击方还是防守方，第二个是火还是闪电
	/// </summary>
	/// <param name="attack">If set to <c>true</c> attack.</param>
	/// <param name="aType">A type.</param>
	public void ShowFireOrLight(bool attack, AoYiEfType aType) {
		UISpriteAnimation blood = null;

		if(aType == AoYiEfType.Blood_Fire) {
			blood = attack ? Att_Blood_Fire : Def_Blood_Fire;
			Status = attack ? Status.set(DragonAnimStatus.Att_Blood_Fire) : Status.set(DragonAnimStatus.Def_Blood_Fire);
		} else {
			blood = attack ? Att_Blood_Light : Def_Blood_Light;
			Status = attack ? Status.set(DragonAnimStatus.Att_Blood_Light) : Status.set(DragonAnimStatus.Def_Blood_Light);
		}

		blood.gameObject.SetActive(true);
		blood.framesPerSecond = BloodFrameRate;
		blood.Forward();
	}

	/// <summary>
	/// 当有一方死亡的时候，调用这个方法
	/// </summary>
	/// <param name="attack">If set to <c>true</c> attack.</param>
	public void HideFireAndLight(bool attack) {
		UISpriteAnimation blood = null;

		blood = attack ? Att_Blood_Fire : Def_Blood_Fire;
		Status = attack ? Status.clear(DragonAnimStatus.Att_Blood_Fire) : Status.clear(DragonAnimStatus.Def_Blood_Fire);
		blood.gameObject.SetActive(false);

		blood = attack ? Att_Blood_Light : Def_Blood_Light;
		Status = attack ? Status.clear(DragonAnimStatus.Att_Blood_Light) : Status.clear(DragonAnimStatus.Def_Blood_Light);
		blood.gameObject.SetActive(false);
	}
		

	#endregion

	#region 测试

//	private int index = 0;
//	void OnGUI() {
//		string name = "test";
//		if(GUI.Button( new Rect(0, 0, 100, 100), "Left AoYi")) {
//			PlayDragon(true, true, index ++, name);
//		}
//
//		if(GUI.Button( new Rect(0, 110, 100, 100), "Right AoYi")) {
//			PlayDragon(false, false, index ++, name);
//		}
//	}

	#endregion


}
