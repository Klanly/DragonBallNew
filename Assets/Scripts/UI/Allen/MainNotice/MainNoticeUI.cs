using UnityEngine;
using System.Collections;

public class MainNoticeUI : MonoBehaviour {

	public UILabel word;
	public TweenScale scaleBg;
	public float speed = 2.0f;

	public const float TWEENSCALE_TIME = 0.1F;

	/*private Color C_ORANGE = new Color(1.0f, 0.4f, 0f, 1.0f);
	private Color C_RED = new Color(1.0f, 0.0f, 0.0f, 1.0f);
	private Color C_YELLOW = new Color(1.0f, 1.0f, 0.0f, 1.0f);
	private Color C_GREEN = new Color(0.0f, 1.0f, 0.0f, 1.0f);
	private Color C_BLUE = new Color(0.0f, 1.0f, 1.0f, 1.0f);*/

	private Vector3 V3_start = new Vector3(315f, -2f, 0.0f);
	private Vector3 V3_end;

	private float time = 0.3f;
	// Use this for initialization
	void Start () {
		getNotice();
	}

	void getNotice() {
		NoticeDataConfig oneTip = Core.Data.MainNoticeMgr.getCurrentNotice();
		if(oneTip != null) {
			word.transform.localPosition = V3_start;
			word.color = Color.white;

			int charLen = oneTip.Tips.Length;

			int length = charLen * word.fontSize + 100;
			word.width = length;
			float totalTIme = time * charLen;
			word.text = oneTip.Tips;
			V3_end = V3_start - new Vector3(length + 500, 0, 0);

			MiniItween.MoveTo(word.gameObject, V3_end, totalTIme, MiniItween.EasingType.Linear).FinishedAnim = ()=>{ StartCoroutine(CycleEnd()); };
		} else {
			Invoke("getNotice", 1f);
		}
	}

	IEnumerator CycleEnd() {
		yield return new WaitForSeconds(0.02f);
		getNotice();
	}


	public void SetShow(bool bShow)
	{
		RED.SetActive (bShow, this.gameObject);
	}

	public void WillShowUI(float time = TWEENSCALE_TIME)
	{
		AnimationCurve anim = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(0.4f, 1.3f, 0.5f, 0.5f), new Keyframe(1f, 1f, 1f, 0f));
		scaleBg.animationCurve = anim;

		TweenScale.Begin(scaleBg.gameObject, time, Vector3.one);
	}

	public void WillHideUI()
	{
		AnimationCurve anim = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));
		scaleBg.animationCurve = anim;

		TweenScale.Begin(scaleBg.gameObject, TWEENSCALE_TIME, Vector3.one * 0.00001f);
	}

	private static MainNoticeUI _ui;
	/// <summary>
	/// 生成一个新的消息
	/// </summary>
	/// <returns>The instance.</returns>
	public static MainNoticeUI getInstance(GameObject root) {
		if(root == null) return null;
		if(_ui == null){
			Object obj = PrefabLoader.loadFromUnPack ("Allen/BroadCastUI", false, false);
			GameObject goBraodcast = Instantiate (obj) as GameObject;
			RED.AddChild (goBraodcast, root);
			goBraodcast.transform.localPosition = new Vector3(24f, 6f, 0f);
			_ui = goBraodcast.GetComponent<MainNoticeUI> ();
			obj = null;
		}
		return _ui;
	}

	//检查UI是否存在
	public static bool checkExist() {
		return _ui != null;
	}

}
