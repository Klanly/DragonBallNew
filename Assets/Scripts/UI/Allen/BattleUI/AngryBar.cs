using UnityEngine;
using System.Collections;

public class AngryBar : MonoBehaviour {

    //怒气的上限为100
    private const int Max_Value = 100;

    /// <summary>
    /// 缓动的时间
    /// </summary>
    private const float SlowAnimTime = 0.5f;

    /// <summary>
    /// 怒气条
    /// </summary>
    public UISprite bar;

    /// <summary>
    /// 缓慢移动的怒气条
    /// </summary>
    public UISprite slowBar;

    public UILabel txtNum;

    //浮动文字的位置
    public GameObject WordPos;

    //当前的怒气点数
    private int _curAngryPoint;
    public int curAP {
        get {
            return _curAngryPoint;
        }

        set {

            int prePoint   = _curAngryPoint;
            _curAngryPoint = value > Max_Value ? Max_Value : value;

            if(_curAngryPoint > prePoint)
                StartCoroutine(SlowAnimGrow(prePoint, _curAngryPoint));
            else  
                StartCoroutine(SlowAnimReduce(prePoint, _curAngryPoint));

            createUpAnim(_curAngryPoint - prePoint, WordPos);

            txtNum.text    = _curAngryPoint.ToString();
        }

    }

    IEnumerator SlowAnimGrow(float from, float to) {
        var t    = 0f;
        var fill = 0f;

        from     = from / Max_Value;
        to       = to   / Max_Value;

        while (t < SlowAnimTime) {
            fill = Mathf.Lerp(from, to, t / SlowAnimTime); 
            t += Time.deltaTime;

            slowBar.fillAmount = fill;
            yield return null; //makes the coroutine stop this frame and return next frame to continue by coroutine magic.
        }

        bar.fillAmount = _curAngryPoint * 1.0f / Max_Value;
    }

    IEnumerator SlowAnimReduce(float from, float to) {
        bar.fillAmount = _curAngryPoint * 1.0f / Max_Value;

        var t    = 0f;
        var fill = 0f;

        from     = from / Max_Value;
        to       = to   / Max_Value;

        while (t < SlowAnimTime) {
            fill = Mathf.Lerp(from, to, t / SlowAnimTime); 
            t += Time.deltaTime;

            slowBar.fillAmount = fill;
            yield return null; //makes the coroutine stop this frame and return next frame to continue by coroutine magic.
        }
       
    }

    #region 怒气值改变的时候，有文字浮动

    private AngryChgAnim lastOne = null;

    /// <summary>
    /// 展现文字的动画
    /// </summary>
    void createUpAnim(int changed, GameObject GoParent) {
        if(changed == 0) return;
        if(lastOne != null) lastOne.UpAnimImmediate();

        Object obj = null;
        obj = PrefabLoader.loadFromUnPack("Ban/AngryChg", false);
        GameObject go = Instantiate(obj) as GameObject;
        RED.AddChild(go, GoParent);

        AngryChgAnim aca = go.GetComponent<AngryChgAnim>();
        aca.showAnim(changed);

        lastOne = aca;

        AsyncTask.QueueOnMainThread( ()=> { lastOne = null; }, aca.StayTime );
    }

    #endregion
}
