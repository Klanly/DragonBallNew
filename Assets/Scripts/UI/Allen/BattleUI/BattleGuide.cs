using UnityEngine;
using System.Collections;

public class BattleGuide : MonoBehaviour {

	public GameObject GoZhanBu;
	public GameObject GoPhase;

	public BattleGuidePhaseAnim PhaseAnim;
	public ZhanbuAnim ZhanBuAnim;

	//没有文字说明
	private StringManager strMgr;

	void Awake() {
		GoZhanBu.SetActive(false);
		GoPhase.SetActive(false);

		strMgr = Core.Data.stringManager;

		///
		///适配屏幕
		UIRoot root = GetComponent<UIRoot>();
		if(root != null)
			root.manualHeight = (int) (1136f/(640f/(float)Screen.height*(float)Screen.width)*640f);

		ZhanBuAnim.manualHeight = root.manualHeight;
	}

	#region 克制阶段

	public IEnumerator Guide_9(){
		GoPhase.SetActive(true);
		ZhanBuAnim.MoveAwayHighlight();

		string num = strMgr.getString(34);
		string Tip = strMgr.getString(38);

		yield return StartCoroutine(PhaseAnim.playPhaseAnim(num, Tip));
		GoPhase.SetActive(false);
		GoZhanBu.SetActive(false);
		ZhanBuAnim.HideMask();
	}

	/// <summary>
	/// 函数里面宏的判定，主要用来适配掉血动画UI
	/// </summary>
	public IEnumerator Guide_A() {
		GoZhanBu.SetActive(true);
		ZhanBuAnim.MoveAwayHighlight();
		string content = strMgr.getString(42);

		int width  = 855;
		int height = 95;
		int X      = 0;

		#if VS
		int Y      = 106;
		#else
		int Y      = 116;
		#endif
		float scaleX = 8.9f;
		float scaleY = 0.7f;

		#if VS
		yield return StartCoroutine(ZhanBuAnim.ShowZhanBuAnimSpecial(content, width, height, X, Y, scaleX, scaleY));
		#else
		yield return StartCoroutine(ZhanBuAnim.ShowZhanBuAnim(content, width, height, X, Y, scaleX, scaleY));
		#endif


		Time.timeScale = 0f;
	}

	#endregion


	#region 怒气阶段

	public IEnumerator Guide_B() {
		GoPhase.SetActive(true);
		ZhanBuAnim.MoveAwayHighlight();

		string num = strMgr.getString(35);
		string Tip = strMgr.getString(39);

		yield return StartCoroutine(PhaseAnim.playPhaseAnim(num, Tip));
		GoPhase.SetActive(false);
		GoZhanBu.SetActive(false);
	}

	public IEnumerator Guide_C() {
		GoZhanBu.SetActive(true);
		string content = strMgr.getString(43);

		int width  = 207;
		int height = 73;
		int X      = -378;
		int Y      = 235;
		float scaleX = 1.93f;
		float scaleY = 0.66f;

		yield return StartCoroutine(ZhanBuAnim.ShowZhanBuAnim(content, width, height, X, Y, scaleX, scaleY));

		Time.timeScale = 0f;
	}


	public IEnumerator Guide_DX () {
		yield return new WaitForSeconds(1.2f);

		GoZhanBu.SetActive(true);
		string content = strMgr.getString(51);

		int width  = 200;
		int height = 180;
		int X      = 406;
		int Y      = -124;
		float scaleX = 8.9f;
		float scaleY = 0.7f;

		yield return StartCoroutine(ZhanBuAnim.ShowZhanBuAnim(content, width, height, X, Y, scaleX, scaleY));

		Time.timeScale = 0f;
	}

	#endregion


	#region 必杀技能阶段

	public IEnumerator Guide_D() {
		GoPhase.SetActive(true);
		ZhanBuAnim.MoveAwayHighlight();

		string num = strMgr.getString(36);
		string Tip = strMgr.getString(40);

		yield return StartCoroutine(PhaseAnim.playPhaseAnim(num, Tip));
		GoPhase.SetActive(false);
		GoZhanBu.SetActive(false);
	}

	public IEnumerator Guide_E() {
		GoZhanBu.SetActive(true);
		string content = strMgr.getString(44);

		int width  = 832;
		int height = 130;
		int X      = 14;
		int Y      = -267;
		float scaleX = 8.6f;
		float scaleY = 1f;

		yield return StartCoroutine(ZhanBuAnim.ShowZhanBuAnim(content, width, height, X, Y, scaleX, scaleY));

		Time.timeScale = 0f;
	}

	#endregion


	#region 对决阶段

	public IEnumerator Guide_F() {
		GoPhase.SetActive(true);
		ZhanBuAnim.MoveAwayHighlight();

		string num = strMgr.getString(37);
		string Tip = strMgr.getString(41);

		yield return StartCoroutine(PhaseAnim.playPhaseAnim(num, Tip));
		GoPhase.SetActive(false);
		GoZhanBu.SetActive(false);
	}

	public IEnumerator Guide_10() {
		GoZhanBu.SetActive(true);
		string content = strMgr.getString(45);

		int width  = 965;
		int height = 93;
		int X      = 14;
		int Y      = 267;
		float scaleX = 10.2f;
		float scaleY = 0.67f;

		yield return StartCoroutine(ZhanBuAnim.ShowZhanBuAnim(content, width, height, X, Y, scaleX, scaleY));

		Time.timeScale = 0f;
	}


	#endregion

	#region 16级的Boss克制

	public IEnumerator Guide_Level16() {
		GoZhanBu.SetActive(true);
		ZhanBuAnim.MoveAwayHighlight();
		string content = strMgr.getString(49);

		int width  = 460;
		int height = 192;
		int X      = 0;
		int Y      = 0;
		float scaleX = 4.6f;
		float scaleY = 1.7f;

		yield return StartCoroutine(ZhanBuAnim.ShowZhanBuAnim(content, width, height, X, Y, scaleX, scaleY, true));

		Time.timeScale = 0f;
	}

	#endregion

}
