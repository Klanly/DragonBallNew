using UnityEngine;
using System.Collections;

public class BattleGuidePhaseAnim : MonoBehaviour {

	public GameObject GoPhase;
	public GameObject GoTip;
	//缩放的时间
	private float ScaleTime = 0.1f;
	//移动时间
	private float MoveTime  = 0.1f;

	private float HoldTime  = 2f;

	//need modify
	public UILabel lblNum;
	public UILabel[] lblPh;

	public UILabel lblTip;

	//phase结束的位置
	Vector3 PhaseEndVec3   = new Vector3(0f, 78f, 0f);
	Vector3 PhaseStartVec3 = new Vector3(0f, -180f, 0f);
	Color mInitColor       = new Color(0.47f, 1f, 0.98f, 1.0f);
	Color mTrans           = new Color(1.0f, 1.0f, 1.0f, 0.0f);

	Vector3 TipStarVec3    = new Vector3(0f, 360f, 0f);
	Vector3 ShakeVec3      = new Vector3(0f, 0.1f, 0f);

	void Init() {
		GoPhase.SetActive(true);
		GoTip.SetActive(false);

		GoPhase.transform.localPosition = PhaseStartVec3;
		GoPhase.transform.localScale    = new Vector3(0.1f, 0.1f, 0.1f);

		foreach(UILabel label in lblPh) {
			label.color = mTrans;
		}
	}

	IEnumerator showTip() {
		GoTip.SetActive(true);

		GoTip.transform.localPosition = TipStarVec3;
		lblTip.color = new Color(1f, 1f, 0f, 0f);

		MiniItween.MoveTo(GoTip, Vector3.zero, MoveTime);
		MiniItween.ColorTo(lblTip.gameObject, new V4(new Color(1f, 1f, 0f, 1f)), MoveTime, MiniItween.Type.ColorWidget);
		yield return new WaitForSeconds(MoveTime);

		MiniItween.Shake(GoTip, ShakeVec3, 0.2f, MiniItween.EasingType.EaseOutCirc).FinishedAnim = () => { GoTip.transform.localPosition = Vector3.zero; };
	}

	IEnumerator Hide() {
		yield return new WaitForSeconds(HoldTime);

		GoTip.SetActive(false);
		GoPhase.SetActive(false);
	}

	/// <summary>
	/// 开启阶段动画
	/// </summary>

	public IEnumerator playPhaseAnim(string num, string tip) {
		lblNum.text = num;
		lblTip.text = tip;

		Init();
		TweenPosition.Begin(GoPhase, ScaleTime, PhaseEndVec3).ignoreTimeScale = true;
		TweenScale.Begin(GoPhase, ScaleTime, Vector3.one * 1.1f).ignoreTimeScale     = true;
		foreach(UILabel lable in lblPh) {
			TweenColor.Begin(lable.gameObject, ScaleTime, mInitColor).ignoreTimeScale = true;
		}
		yield return new WaitForSeconds(ScaleTime);

		GoPhase.transform.localScale = Vector3.one * 1.05f;
		yield return new WaitForEndOfFrame();
		GoPhase.transform.localScale = Vector3.one;
		yield return new WaitForEndOfFrame();
		GoPhase.transform.localScale = Vector3.one * 0.95f;
		yield return new WaitForEndOfFrame();
		GoPhase.transform.localScale = Vector3.one * 1.05f;
		yield return new WaitForEndOfFrame();
		GoPhase.transform.localScale = Vector3.one;

		yield return StartCoroutine(showTip());

		yield return StartCoroutine(Hide());
	}


/*
	void OnGUI() {

		if(GUI.Button(new Rect(0, 0, 100, 100), "test")) {
			StartCoroutine(playPhaseAnim());
		}

	}
*/

}
