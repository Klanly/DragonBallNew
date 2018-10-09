using UnityEngine;
using System.Collections;

public class CG_Anim : MonoBehaviour {

	public UISprite Sp1;
	public UISprite Sp2;
	public UISprite Sp3;
	public UISprite Sp4;
	public UISprite Sp5;
	public UISprite Sp6;
	public UISprite Mask;

	public UISprite Sp7;
	public UISprite Sp8;
	public UISprite Sp9;
	public UISprite Sp10;
	public UISprite Sp11;
	public UISprite Sp12;
	public UISprite Sp13;

	public GameObject Part1;
	public GameObject Part2;

	/// <summary>
	/// 常用的变量
	/// </summary>
	Color trans = Color.white;
	float Step1Time = 0.4f;
	float Gap1      = 0.2f;
	float Step2Time = 0.4f;
	float Gap2      = 0.3f;
	float Step3Time = 0.4f;
	float Gap3      = 0.5f;

	#region Sp4 滑动的参数
	float Step4FlipTime = 0.25f;
	//Sp4 滑动到0.2F的时候就
	float Step4StartShake = 0.2f;

	Vector3 Step4StarPos = new Vector3(850F, -160F, 0F);
	Vector3 Step4EndPos  = new Vector3(-350F, -160F, 0F);

	//Sp1 Sp2 震动的参数
	Vector3 Step4ShakeVec3 = new Vector3(0.1f, 0.1f, 0.1f);
	float Step4ShakeTime = 0.2f;

	float Gap4      = 1F;
	#endregion

	#region Sp5的参数

	float Step5ScaleTime = 0.2f;
	float InitScaleFac   = 3f;
	float Step5ShakeTime = 0.4f;

	#endregion

	#region Sp6的参数

	float Step6ScaleTime = 0.2f;
	float Step6ShakeTime = 0.2f;
	float Gap6           = 0.35f;
	#endregion

	float GapBetweenPart = 1f;

	#region sp7的参数
	Vector3 Step7StartPos = new Vector3(-878f, 190f, 0f);
	Vector3 Step7EndPos   = new Vector3(-281f, 190f, 0f);
	float Step7Fliptime   = 0.2f;
	float Step7ShakeTime  = 0.1f;
	Vector3 Step7ShakeVec3  = new Vector3(0.1f, 0f, 0f);
	#endregion

	#region sp8的参数
	float Step8ScaleTime = 0.15f;
	float Step8ReduceTime = 0.05f;
	float Step8ScaleLarge = 1.1f;
	float Gap8 = 0.4f;
	#endregion

	float Gap9 = 0.4f;
	float Gap10 = 0.5f;

	#region sp11的参数
	Vector3 Step11StarPos = new Vector3(407f, 650f, 0f);
	Vector3 Step11EndPos  = new Vector3(407f, -14f, 0f);
	Vector3 Step11Pos     = new Vector3(407f, 0f, 0f);
	float Step11MoveTime  = 0.1f;
	float Step11ShakeTime = 0.3f;
	float Gap11 = 0.15f;
	#endregion

	#region sp12的参数
	float Step12ShowTime  = 1.5f;
	#endregion

	#region sp13的参数
	float Step13ScaleTime = 0.1f;
	float Step13InitScale = 0.2f;
	float Step13Large     = 1.1f;
	float Step13ShakeTime = 1.6f;
	Vector3 Step13ShakeVec= new Vector3(0.15f, 0.15f, 0.15f);
	#endregion

	void Awake() {
		trans = new Color(1F, 1F, 1F, 0F);
		init();
	}

	/// <summary>
	/// 初始化状态
	/// </summary>

	void init() {
		Part1.gameObject.SetActive(true);
		Part2.gameObject.SetActive(true);
		Sp1.gameObject.SetActive(false);
		Sp2.gameObject.SetActive(false);
		Sp3.gameObject.SetActive(false);
		Sp4.gameObject.SetActive(false);
		Sp5.gameObject.SetActive(false);
		Sp6.gameObject.SetActive(false);
		Mask.gameObject.SetActive(false);
		Sp7.gameObject.SetActive(false);
		Sp8.gameObject.SetActive(false);
		Sp9.gameObject.SetActive(false);
		Sp10.gameObject.SetActive(false);
		Sp11.gameObject.SetActive(false);
		Sp12.gameObject.SetActive(false);
		Sp13.gameObject.SetActive(false);
	}

	public IEnumerator PlayAnim() {
		init();
		yield return StartCoroutine(Step1());
		yield return StartCoroutine(Step2());
		yield return StartCoroutine(Step3());
		yield return StartCoroutine(Step4());
		yield return StartCoroutine(Step5());
		yield return StartCoroutine(Step6());
		yield return StartCoroutine(Par1End());
		yield return StartCoroutine(Step7());
		yield return StartCoroutine(Step8());
		yield return StartCoroutine(Step9());
		yield return StartCoroutine(Step10());
		yield return StartCoroutine(Step11());
		yield return StartCoroutine(Step12());
		yield return StartCoroutine(Step13());
		yield return StartCoroutine(Part2End());
	}

	#region 分步的动画Part1

	IEnumerator Step1() {
		Sp1.gameObject.SetActive(true);
		Sp1.color = trans;

		MiniItween.ColorTo(Sp1.gameObject, new V4(Color.white), Step1Time, MiniItween.Type.ColorWidget);
		yield return new WaitForSeconds(Step1Time + Gap1);
	}

	IEnumerator Step2() {
		Sp2.gameObject.SetActive(true);
		Sp2.color = trans;

		MiniItween.ColorTo(Sp2.gameObject, new V4(Color.white), Step2Time, MiniItween.Type.ColorWidget);
		yield return new WaitForSeconds(Step2Time + Gap2);
	}

	IEnumerator Step3() {
		Sp3.gameObject.SetActive(true);
		Sp3.color = trans;

		MiniItween.ColorTo(Sp3.gameObject, new V4(Color.white), Step3Time, MiniItween.Type.ColorWidget);
		yield return new WaitForSeconds(Step3Time + Gap3);
	}

	IEnumerator Step4() {
		Sp4.gameObject.SetActive(true);
		Sp4.transform.localPosition = Step4StarPos;
		MiniItween.MoveTo(Sp4.gameObject, Step4EndPos, Step4FlipTime);
		yield return new WaitForSeconds(Step4StartShake);

		MiniItween.Shake(Sp1.gameObject, Step4ShakeVec3, Step4ShakeTime, MiniItween.EasingType.Linear);
		MiniItween.Shake(Sp2.gameObject, Step4ShakeVec3, Step4ShakeTime, MiniItween.EasingType.Linear);

		yield return new WaitForSeconds(Gap4);
	}

	IEnumerator Step5() {
		Sp5.gameObject.SetActive(true);
		Sp5.transform.localScale = Vector3.one * InitScaleFac;
		Sp5.color = trans;

		MiniItween.ScaleTo(Sp5.gameObject, Vector3.one * 0.9f, Step5ScaleTime);
		MiniItween.ColorTo(Sp5.gameObject, new V4(Color.white), Step5ScaleTime, MiniItween.Type.ColorWidget);
		yield return new WaitForSeconds(Step5ScaleTime);

		MiniItween.Shake(Sp4.gameObject, Step4ShakeVec3, Step5ShakeTime, MiniItween.EasingType.Linear);
		MiniItween.ScaleTo(Sp5.gameObject, Vector3.one, 0.1f);
		yield return new WaitForSeconds(Step5ShakeTime);
	}

	IEnumerator Step6() {
		Sp6.gameObject.SetActive(true);
		Sp6.transform.localScale = Vector3.one * InitScaleFac;
		Sp6.color = trans;

		MiniItween.ScaleTo(Sp6.gameObject, Vector3.one * 0.9f, Step6ScaleTime);
		MiniItween.ColorTo(Sp6.gameObject, new V4(Color.white), Step6ScaleTime, MiniItween.Type.ColorWidget);
		yield return new WaitForSeconds(Step6ScaleTime);

		MiniItween.Shake(Sp3.gameObject, Step4ShakeVec3, Step6ShakeTime, MiniItween.EasingType.Linear);
		MiniItween.ScaleTo(Sp6.gameObject, Vector3.one, 0.1f);
		yield return new WaitForSeconds(Step6ShakeTime);

		yield return new WaitForSeconds(Gap6);
	}

	IEnumerator Par1End() {
		Mask.gameObject.SetActive(true);

		Mask.color = trans;
		MiniItween.ColorTo(Mask.gameObject, new V4(Color.white), 1.2f, MiniItween.Type.ColorWidget);
		yield return new WaitForSeconds(GapBetweenPart);
	}


	#endregion

	#region 分部动画Part2
	IEnumerator Step7() {
		Part1.gameObject.SetActive(false);
		Sp7.gameObject.SetActive(true);

		Sp7.transform.localPosition = Step7StartPos;
		MiniItween.MoveTo(Sp7.gameObject, Step7EndPos, Step7Fliptime);
		yield return new WaitForSeconds(Step7Fliptime);
		Sp7.transform.localPosition = Step7EndPos;

		MiniItween.Shake(Sp7.gameObject, Step7ShakeVec3, Step7ShakeTime, MiniItween.EasingType.Linear);

		yield return new WaitForSeconds(Step7ShakeTime);
	}

	IEnumerator Step8() {
		Sp8.gameObject.SetActive(true);
		Sp8.transform.localScale = Vector3.zero;

		MiniItween.ScaleTo(Sp8.gameObject, Step8ScaleLarge * Vector3.one, Step8ScaleTime);
		yield return new WaitForSeconds(Step8ScaleTime);

		MiniItween.ScaleTo(Sp8.gameObject, Vector3.one, Step8ReduceTime);
		yield return new WaitForSeconds(Step8ReduceTime + Gap8);
	}

	IEnumerator Step9 () {
		Sp9.gameObject.SetActive(true);
		yield return new WaitForSeconds(Gap9);
	}

	IEnumerator Step10() {
		Sp10.gameObject.SetActive(true);
		yield return new WaitForSeconds(Gap10);
	}

	IEnumerator Step11() {
		Sp11.gameObject.SetActive(true);
		Sp11.transform.localPosition = Step11StarPos;

		MiniItween.MoveTo(Sp11.gameObject, Step11EndPos, Step11MoveTime);
		yield return new WaitForSeconds(Step11MoveTime);

		MiniItween.Shake(Sp9.gameObject, Step4ShakeVec3, Step11ShakeTime, MiniItween.EasingType.Linear);
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();

		Sp11.transform.localPosition = Step11Pos;
		MiniItween.Shake(Sp10.gameObject, Step4ShakeVec3, Step11ShakeTime, MiniItween.EasingType.Linear);
		yield return new WaitForSeconds(Gap11);
	}

	IEnumerator Step12() {
		Sp12.gameObject.SetActive(true);
		Sp12.color = trans;

		MiniItween.ColorTo(Sp12.gameObject, new V4(Color.white), Step12ShowTime, MiniItween.Type.ColorWidget);
		yield return new WaitForSeconds(Step12ShowTime);
	}

	IEnumerator Step13() {
		Sp13.gameObject.SetActive(true);
		Sp13.transform.localScale = Vector3.one * Step13InitScale;

		MiniItween.ScaleTo(Sp13.gameObject, Vector3.one * Step13Large, Step13ScaleTime);
		yield return new WaitForSeconds(Step13ScaleTime);

		yield return new WaitForEndOfFrame();
		Sp13.transform.localScale = Vector3.one;
		yield return new WaitForEndOfFrame();
		Sp13.transform.localScale = Vector3.one;

		MiniItween.Shake(Sp13.gameObject, Step13ShakeVec, Step13ShakeTime, MiniItween.EasingType.EaseOutCirc);
		yield return new WaitForSeconds(Step13ShakeTime);
	}

	IEnumerator Part2End() {
		Part2.gameObject.SetActive(false);
		yield return new WaitForEndOfFrame();
	}

	#endregion

	/*void OnGUI() {
		if(GUI.Button( new Rect(0, 0, 60, 20), "click")) {
			StartCoroutine(PlayAnim());
		}
	}*/

}
