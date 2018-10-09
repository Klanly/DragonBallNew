using UnityEngine;
using System.Collections;
using System.Threading;

public class BattleHitCount : MonoBehaviour {

    public UISpriteAnimation hitBtnAnim;

    //点击的UI
    public GameObject GoHitCount;
    public GameObject GoBtn;
    //左右两个淡入淡出效果
    public UISprite leftLight;
    public UISprite RightLight;

    public float FadeOutTime = 0.1f;
    public Color FadeOutColor = new Color(1.0f, 1.0f, 1.0f, 0.125f);
    public Vector3 FadeOutScale = new Vector3(2.0f, 2.0f, 2.0f);

    //--- 淡出的位置 ---
    public Vector3 RigthFadeOutPos;
    public Vector3 LeftFadeOutPos;
    //记录下来淡入的位置
    private Vector3 RightFadeInPos;
    private Vector3 LeftFadeInPos;

    //
    public ShowBattleHitCount CountAnim;

    //这个是左边的吗？
    public bool Left = true;

    //记录连击数
    public BattleHitMgr mgr;

	// Use this for initialization
	void Start () {
        RightFadeInPos = RightLight.transform.localPosition;
        LeftFadeInPos  = leftLight.transform.localPosition;
	}

    public void ShowBtn() {
        gameObject.SetActive(true);
        GoBtn.SetActive(true);
        GoHitCount.SetActive(false);

        leftLight.gameObject.SetActive(false);
        RightLight.gameObject.SetActive(false);
    }

    //隐藏按钮和数量UI
    public void HideBtnAndCount() {
        gameObject.SetActive(true);
        GoBtn.SetActive(false);
        GoHitCount.SetActive(false);

        leftLight.gameObject.SetActive(false);
        RightLight.gameObject.SetActive(false);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    //不会展示连击的数量
    public IEnumerator pressOnce() {
        //play animation
		hitBtnAnim.ResetToBeginning();
        // set Active
        leftLight.gameObject.SetActive(true);
        RightLight.gameObject.SetActive(true);

        //reset Pos 
        leftLight.transform.localPosition = LeftFadeInPos;
        RightLight.transform.localPosition = RightFadeInPos;
        //reset Color
        Color FadeInColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        leftLight.color = FadeInColor;
        RightLight.color = FadeInColor;

        //Change Position
        TweenPosition.Begin(leftLight.gameObject, FadeOutTime, LeftFadeOutPos);
        TweenPosition.Begin(RightLight.gameObject, FadeOutTime, RigthFadeOutPos);

        //Change Color
        TweenColor.Begin(leftLight.gameObject, FadeOutTime, FadeOutColor);
        TweenColor.Begin(RightLight.gameObject, FadeOutTime, FadeOutColor);

        //Large scale
        TweenScale.Begin(leftLight.gameObject, FadeOutTime, FadeOutScale);
        TweenScale.Begin(RightLight.gameObject, FadeOutTime, FadeOutScale);

        yield return new WaitForSeconds(FadeOutTime + 0.1f);
  
    }

    //展示连击的数量，但是不会出现按钮
    public void showCount() {
        gameObject.SetActive(true);

        GoBtn.SetActive(false);
        GoHitCount.SetActive(true);
        leftLight.gameObject.SetActive(false);
        RightLight.gameObject.SetActive(false);

        if(!GoHitCount.activeSelf) GoHitCount.SetActive(true);

		CountAnim.showCount(mgr.getCount, 0);
    }

}
