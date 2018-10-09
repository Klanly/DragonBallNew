using UnityEngine;
using System.Collections;

public class BossComing : MonoBehaviour {
	public UISprite BG_Black;
	public UISprite BG_RedTop;
	public UISprite BG_RedBottom;

	public UISpriteAnimation Line;

	public UISprite Blood1;
	public UISprite Blood2;
	public UISprite Blood3;

	public GameObject AllWord;
	public UILabel[] WarningWord;

	private Color Trans = Color.white;
	private bool end = false;

	void Awake() {
		initStatus();
		Trans = new Color(1f,1f,1f,0f);
		end   = false;
	}

	/// <summary>
	/// 播放Boss来了的动画
	/// </summary>
	/// <returns>The animation.</returns>
	public IEnumerator PlayAnim() {
		Core.Data.soundManager.SoundFxPlay(SoundFx.FX_PVEBOSS);
		initStatus();

		yield return StartCoroutine(Step1());
		StartCoroutine(Trinkle());
		yield return StartCoroutine(Step2());
		yield return StartCoroutine(Step3());
		yield return StartCoroutine(Word_B());
		yield return StartCoroutine(Word_O());
		yield return StartCoroutine(Word_Later());
		yield return StartCoroutine(EndAnim());
	}

	//初始化状态
	void initStatus() {
		gameObject.SetActive(true);
		BG_Black.gameObject.SetActive(false);

		AllWord.transform.localScale = Vector3.one;
		BG_RedTop.gameObject.SetActive(false);
		BG_RedBottom.gameObject.SetActive(false);
		Line.gameObject.SetActive(false);
		Blood1.gameObject.SetActive(false);
		Blood2.gameObject.SetActive(false);
		Blood3.gameObject.SetActive(false);

		foreach(UILabel lbl in WarningWord) {
			lbl.gameObject.SetActive(false);
		}
	}

	//结束之后隐藏所有物体
	IEnumerator HideAll() {
		AllWord.transform.localScale = Vector3.one;
		BG_RedTop.gameObject.SetActive(false);
		BG_RedBottom.gameObject.SetActive(false);
		Line.gameObject.SetActive(false);

		MiniItween.ColorTo(Blood1.gameObject, new V4(Trans), 0.1f, MiniItween.Type.ColorWidget);
		MiniItween.ColorTo(Blood2.gameObject, new V4(Trans), 0.1f, MiniItween.Type.ColorWidget);
		MiniItween.ColorTo(Blood3.gameObject, new V4(Trans), 0.1f, MiniItween.Type.ColorWidget);
		yield return new WaitForSeconds(0.1f);

		Blood1.gameObject.SetActive(false);
		Blood2.gameObject.SetActive(false);
		Blood3.gameObject.SetActive(false);

		foreach(UILabel lbl in WarningWord) {
			lbl.gameObject.SetActive(false);
		}
	}


	#region 背景变黑

	float Black_Time = 0.2f;
	IEnumerator Step1() {
		Color transBG = new Color(1f, 1f, 1f, 0.74f);

		BG_Black.gameObject.SetActive(true);
		BG_Black.color = Trans;
		MiniItween.ColorTo(BG_Black.gameObject, new V4(transBG), Black_Time, MiniItween.Type.ColorWidget);
		yield return new WaitForSeconds(Black_Time);
	}

	#endregion


	#region 条子变化

	float Line_Time = 0.21f;
	IEnumerator Step2() {
		Line.gameObject.SetActive(true);

		Line.Forward();
		yield return new WaitForSeconds(Line_Time);
	}

	#endregion

	#region 红点出现
	float Blood1_Time  = 0.1f;
	float Blood1_Scale = 5f;
	float Blood_Alpha = 0.5f;

	IEnumerator Step3() {
		Blood1.gameObject.SetActive(true);

		Blood1.transform.localScale = Blood1_Scale * Vector3.one;
		Blood1.alpha = Blood_Alpha;

		MiniItween.ScaleTo(Blood1.gameObject, Word_Final * Vector3.one, Blood1_Time);
		MiniItween.ColorTo(Blood1.gameObject, new V4(Color.white), Blood1_Time, MiniItween.Type.ColorWidget);
		yield return new WaitForSeconds(Blood1_Time);

		for(int i = 0; i < 2; ++ i)
			yield return new WaitForEndOfFrame();

		Blood1.transform.localScale = Vector3.one;
	}

	#endregion

	float Word_Final = 0.9f;
	float Word_Scale = 3f;
	float Word_Time  = 0.08f;
	/// <summary>
	/// B 字母
	/// </summary>

	IEnumerator Word_B() {
		UILabel lbl_B = WarningWord[0];
		lbl_B.gameObject.SetActive(true);

		lbl_B.alpha = 0f;
		lbl_B.transform.localScale = Word_Scale * Vector3.one;

		MiniItween.ScaleTo(lbl_B.gameObject, Word_Final * Vector3.one, Word_Time, MiniItween.EasingType.EaseInCirc);
		MiniItween.ColorTo(lbl_B.gameObject, new V4(Color.red), Word_Time, MiniItween.Type.ColorWidget);

		yield return new WaitForSeconds(Word_Time);
		yield return new WaitForEndOfFrame();

		lbl_B.transform.localScale = Vector3.one;
	}

	/// <summary>
	/// 字母O
	/// </summary>

	IEnumerator Word_O() {
		UILabel lbl_O = WarningWord[1];
		lbl_O.gameObject.SetActive(true);
		Blood2.gameObject.SetActive(true);

		lbl_O.alpha = 0f;
		lbl_O.transform.localScale = Word_Scale * Vector3.one;

		Blood2.alpha = Blood_Alpha;
		Blood2.transform.localScale = Word_Scale * Vector3.one;


		MiniItween.ScaleTo(lbl_O.gameObject, Word_Final * Vector3.one, Word_Time, MiniItween.EasingType.EaseInCubic);
		MiniItween.ColorTo(lbl_O.gameObject, new V4(Color.red), Word_Time, MiniItween.Type.ColorWidget);

		MiniItween.ScaleTo(Blood2.gameObject, Word_Final * Vector3.one, Word_Time);
		MiniItween.ColorTo(Blood2.gameObject, new V4(Color.white), Word_Time, MiniItween.Type.ColorWidget);

		yield return new WaitForSeconds(Word_Time);
		yield return new WaitForEndOfFrame();

		lbl_O.transform.localScale = Vector3.one;
		Blood2.transform.localScale = Vector3.one;
	}


	float AllWord_Show_Time = 0.7f;
	/// <summary>
	/// 之后的所有字母
	/// </summary>

	IEnumerator Word_Later() {

		for(int index = 2; index < 7; ++ index) {
			UILabel lbl_word = WarningWord[index];
			lbl_word.gameObject.SetActive(true);

			lbl_word.alpha = 0f;
			lbl_word.transform.localScale = Word_Scale * Vector3.one;

			MiniItween.ScaleTo(lbl_word.gameObject, Word_Final * Vector3.one, Word_Time, MiniItween.EasingType.EaseInCubic);
			MiniItween.ColorTo(lbl_word.gameObject, new V4(Color.red), Word_Time, MiniItween.Type.ColorWidget);

			yield return new WaitForSeconds(Word_Time);
			yield return new WaitForEndOfFrame();

			lbl_word.transform.localScale = Vector3.one;

			if(index == 3) {
				StartCoroutine(Blood3Anim());
			}
		}

		yield return new WaitForSeconds(AllWord_Show_Time);
	}


	IEnumerator Blood3Anim() {
		Blood3.gameObject.SetActive(true);

		Blood3.alpha = Blood_Alpha;
		Blood3.transform.localScale = Word_Scale * Vector3.one;
		MiniItween.ScaleTo(Blood3.gameObject, Word_Final * Vector3.one, Word_Time);
		MiniItween.ColorTo(Blood3.gameObject, new V4(Color.white), Word_Time, MiniItween.Type.ColorWidget);

		yield return new WaitForSeconds(Word_Time);
		yield return new WaitForEndOfFrame();

		Blood3.transform.localScale = Vector3.one;
	}


	float Trinke_Time = 0.25f;

	/// <summary>
	/// 红色闪烁
	/// </summary>
	IEnumerator Trinkle() {
		BG_RedTop.gameObject.SetActive(true);
		BG_RedBottom.gameObject.SetActive(true);

		while(!end) {
			MiniItween.ColorTo(BG_RedTop.gameObject, new V4(Color.white), Trinke_Time, MiniItween.Type.ColorWidget);
			MiniItween.ColorTo(BG_RedBottom.gameObject, new V4(Color.white), Trinke_Time, MiniItween.Type.ColorWidget);

			yield return new WaitForSeconds(Trinke_Time);

			MiniItween.ColorTo(BG_RedTop.gameObject, new V4(Trans), Trinke_Time, MiniItween.Type.ColorWidget);
			MiniItween.ColorTo(BG_RedBottom.gameObject, new V4(Trans), Trinke_Time, MiniItween.Type.ColorWidget);
			yield return new WaitForSeconds(Trinke_Time);
		}
	}

	float FadeOut_Scale = 10f;
	float FadeOut_Time  = 0.2f;

	/// <summary>
	/// 结束动画
	/// </summary>
	IEnumerator EndAnim() {
		Line.Reverse();

		MiniItween.ScaleTo(AllWord, FadeOut_Scale * Vector3.one, FadeOut_Time);
		for(int index = 0; index < 7; ++ index) {
			UILabel lbl_word = WarningWord[index];
			MiniItween.ColorTo(lbl_word.gameObject, new V4(Trans), FadeOut_Time, MiniItween.Type.ColorWidget);
		}
		yield return new WaitForSeconds(FadeOut_Time);

		MiniItween.ColorTo(BG_Black.gameObject, new V4(Trans), Black_Time, MiniItween.Type.ColorWidget);
		yield return StartCoroutine(HideAll());
		yield return new WaitForSeconds(Black_Time * 0.5f);
	}



	#if DEBUG
//	void OnGUI() {
//		if(GUI.Button(new Rect(0, 0, 100, 100), "Test")) {
//			StartCoroutine(PlayAnim());
//		}
//	}

	#endif
}
