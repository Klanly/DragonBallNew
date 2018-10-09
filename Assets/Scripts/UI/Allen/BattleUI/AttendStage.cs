using UnityEngine;
using System.Collections;

public class AttendStage : MonoBehaviour {
	public UISprite UI_Ready;
	public UISprite UI_Go;

	private float ReadyAnimTime = 0.12f;
	private float ReadyAnimTimeS = 0.08f;
	private float ReadyStay = 0.6f;

	private float GoAinmTime = 0.2f;
	private float GoStay = 0.3f;
	private float GoSmal = 0.04f;

	void Start() {
		UI_Ready.gameObject.SetActive(false);
		UI_Go.gameObject.SetActive(false);
	}

	public void showAnim() {
		StartCoroutine(Anim());
	}

	IEnumerator Anim() {
		yield return StartCoroutine(ReadyAnim());
		yield return StartCoroutine(GoAnim());
	}

	IEnumerator ReadyAnim () {
		///
		/// 显示Ready 动画
		///
		UI_Ready.gameObject.SetActive(true);
		UI_Ready.transform.localScale = Vector3.zero;

		MiniItween.ScaleTo(UI_Ready.gameObject, 1.1f * Vector3.one, ReadyAnimTime);
		yield return new WaitForSeconds(ReadyAnimTime);

		MiniItween.ScaleTo(UI_Ready.gameObject, Vector3.one, ReadyAnimTimeS);
		yield return new WaitForSeconds(ReadyAnimTimeS);

		yield return new WaitForSeconds(ReadyStay);

		///往回显示Ready动画
		MiniItween.ScaleTo(UI_Ready.gameObject, 1.1f * Vector3.one, ReadyAnimTimeS);
		yield return new WaitForSeconds(ReadyAnimTimeS);

		MiniItween.ScaleTo(UI_Ready.gameObject, Vector3.zero, ReadyAnimTime);
		yield return new WaitForSeconds(ReadyAnimTime);

		UI_Ready.gameObject.SetActive(false);
	}

	/// <summary>
	/// 显示Go动画
	/// </summary>
	IEnumerator GoAnim () {
		UI_Go.gameObject.SetActive(true);
		UI_Go.transform.localScale = 5 * Vector3.one;
		UI_Go.alpha = 0.2f;

		MiniItween.ScaleTo(UI_Go.gameObject, 0.9f * Vector3.one, GoAinmTime, MiniItween.EasingType.EaseInExpo);
		MiniItween.ColorTo(UI_Go.gameObject, new V4(Color.white), GoAinmTime, MiniItween.Type.ColorWidget);
		yield return new WaitForSeconds(GoAinmTime);

		yield return new WaitForEndOfFrame();
		MiniItween.ScaleTo(UI_Go.gameObject, Vector3.one, GoSmal);
		yield return new WaitForSeconds(GoSmal);

		///停留
		yield return new WaitForSeconds(GoStay);

		MiniItween.ScaleTo(UI_Go.gameObject, Vector3.one * 1.1f, GoSmal);
		yield return new WaitForSeconds(GoSmal);

		MiniItween.ScaleTo(UI_Go.gameObject, 5 * Vector3.one, GoAinmTime);
		MiniItween.ColorTo(UI_Go.gameObject, new V4(Vector3.zero), GoAinmTime, MiniItween.Type.ColorWidget);
		yield return new WaitForSeconds(GoAinmTime);

		UI_Go.gameObject.SetActive(false);
	}

	/*void OnGUI() {
		if(GUI.Button(new Rect(0, 0, 100, 100), "Test")) {
			showAnim();
		}
	}*/
}
