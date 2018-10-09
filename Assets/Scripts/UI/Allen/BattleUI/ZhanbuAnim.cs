using UnityEngine;
using System.Collections;

public class ZhanbuAnim : MonoBehaviour {
	public UILabel lblCon;
	public UISprite Highlight;

	//占卜婆婆
	public GameObject goDes;
	//高亮显示的区域,
	public GameObject goHighArea;
	public GameObject goMaskArea;

	float moveTime = 0.1f;
	Vector3 StartPos = new Vector3(-960f, 0f, 0f);
	Vector3 EndPos   = new Vector3(-310f, 0f, 0f);
	Vector3 AwayPos  = new Vector3(0f, 888f, 0f);

	//因为16级的需要网下一点，所以有新的位置
	Vector3 StartPosLv16 = new Vector3(-960f, -64f, 0f);
	Vector3 EndPosLv16   = new Vector3(-310f, -64f, 0f);

	//动画是否播放完成
	private bool AnimIsOver = false;
	[HideInInspector]
	public float manualHeight;

	//适配屏幕
	float FitScreen(float Y) {
		if(Y > 0)
			return Y + (manualHeight - 640f) * 0.5f;
		else 
			return Y - (manualHeight - 640f) * 0.5f;
	}

	private bool isLevel16 = false;
	public IEnumerator ShowZhanBuAnim(string content, int width, int height, int X, int Y, float scaleX, float scaleY, bool Level16 = false) {
		lblCon.text = content;
		isLevel16   = Level16;

		if(Level16) {
			Vector3 adjustStartPos = new Vector3(StartPosLv16.x, FitScreen(StartPosLv16.y), StartPosLv16.z);
			Vector3 adjustEndPos   = new Vector3(EndPosLv16.x, FitScreen(EndPosLv16.y), EndPosLv16.z);

			goDes.transform.localPosition = adjustStartPos;
			MiniItween.MoveTo(goDes, adjustEndPos, moveTime);
		} else {
			goDes.transform.localPosition = StartPos;
			MiniItween.MoveTo(goDes, EndPos, moveTime);
		}

		goHighArea.transform.localPosition = new Vector3(X, FitScreen(Y), 0f);
		goMaskArea.transform.localPosition = new Vector3(X, FitScreen(Y), 0f);

		goMaskArea.transform.localScale    = new Vector3(scaleX, scaleY, 1f);

		Highlight.width = width;
		Highlight.height= height;

		yield return new WaitForSeconds(moveTime);

		delayMakeUserCanClick();
	}

	//
	//属性克制掉血的动画 - 抛物线掉血
	//
	public IEnumerator ShowZhanBuAnimSpecial(string content, int width, int height, int X, int Y, float scaleX, float scaleY) {
		lblCon.text = content;

		goDes.transform.localPosition = StartPos;
		MiniItween.MoveTo(goDes, EndPos, moveTime);

		goHighArea.transform.localPosition = new Vector3(X, Y, 0f);
		goMaskArea.transform.localPosition = new Vector3(X, Y, 0f);

		goMaskArea.transform.localScale    = new Vector3(scaleX, scaleY, 1f);

		Highlight.width = width;
		Highlight.height= height;

		yield return new WaitForSeconds(moveTime);

		delayMakeUserCanClick();
	}


	/// <summary>
	/// 延迟让用户可以点击
	/// </summary>
	void delayMakeUserCanClick() {
		long now = Core.TimerEng.curTime;
		TimerTask task = new TimerTask(now, now + 2, 1);
		task.onEventEnd = (t) => { AnimIsOver = true; };
		task.DispatchToRealHandler();
	}

	public void HideZhanBuAnim() {
		if(AnimIsOver) {
			if(isLevel16) {
				Vector3 adjustStartPos = new Vector3(StartPosLv16.x, FitScreen(StartPosLv16.y), StartPosLv16.z);
				MiniItween.MoveTo(goDes, adjustStartPos, moveTime);
			} else{
				MiniItween.MoveTo(goDes, StartPos, moveTime);
			}
				
			goHighArea.transform.localPosition = AwayPos;
			goMaskArea.transform.localPosition = AwayPos;
			HideMask();

			//用来判断是否为特殊步骤

			bool over = true;

			BanBattleProcess proc = BanBattleProcess.Instance;
			if(proc != null) {
				over = proc.FeatureWar_Cast(GetComponent<BoxCollider>(), manualHeight);
			}

			if(over) {
				Time.timeScale = 1f;
				AnimIsOver = false;
			}

			if(isLevel16) {
				Invoke("delayDeleteGuide", 1f);
			}
		}
	}

	void delayDeleteGuide() {
		gameObject.SetActive(false);
	}

	/// <summary>
	/// 高亮移开
	/// </summary>
	public void MoveAwayHighlight() {
		gameObject.SetActive(true);
		goMaskArea.SetActive(true);

		goDes.transform.localPosition = StartPos;
		goHighArea.transform.localPosition = AwayPos;
		goMaskArea.transform.localPosition = AwayPos;
	}

	public void HideMask() {
		goMaskArea.SetActive(false);
	}

	public void ShowMask() {
		goMaskArea.SetActive(true);
	}

	void OnClick() {
		HideZhanBuAnim();
	}

}
