using UnityEngine;
using System.Collections;

public class LabelEffect : MonoBehaviour {
    /// <summary>
    /// 起始位置
    /// </summary>
    public Vector3 startPos;
    /// <summary>
    /// 高度
    /// </summary>
    public float height = 5.0f;
    /// <summary>
    /// 飞起来方法
    /// </summary>
    public float duration = 0.8f;
    public float delayTime = 0;

    public static Color lightGreen = new Color(100f/255f,1f,0f,1f);

    UILabel myNum; 

    void  Start(){
        gameObject.transform.localPosition = startPos;
        if (delayTime != 0)
            Invoke("ShowEffect",delayTime);
        else
            this.ShowEffect();
    }

    void  ShowEffect(){
        //   gameObject.transform.localScale = Vector3.one;
        TweenScale.Begin(gameObject,0.1f,Vector3.one);
        MiniItween.MoveTo(gameObject, new Vector3(startPos.x, startPos.y + height, startPos.z), duration /1.4f );//.myDelegateFunc += OnDestory;

        if (gameObject.GetComponent<TweenAlpha>() == null)
            gameObject.AddComponent<TweenAlpha>();
      
        TweenAlpha.Begin(gameObject, duration/1.2f, 0f).onFinished.Add( new EventDelegate(this,"OnDestory"));//+=OnDestory;
    }

    void OnDestory(){

        TweenAlpha.Begin(gameObject,0.1f,0f).onFinished.Clear();
        Destroy(gameObject);
    }


	public static  LabelEffect CreateLabelEffect(string str,float high,float dur,Color tColor,Vector3 pos,Transform parentTrs,int depth,float tDelayTime =0,bool isChangeParent = false)
    {
        UnityEngine.Object obj = WXLLoadPrefab.GetPrefab(WXLPrefabsName.UILabelEffect);
        if (obj != null)
        {
            GameObject  go = Instantiate(obj)as GameObject;
            LabelEffect LE = go.GetComponent<LabelEffect>();
            Transform goTrans = go.transform;
            go.transform.parent = parentTrs;
            go.transform.localPosition = Vector3.zero;
            goTrans.localScale = Vector3.zero;

			LE.gameObject.layer = LayerMask.NameToLayer ("UITop");
            LE.startPos = pos;
			if(isChangeParent == true)
				go.transform.parent = DBUIController.mDBUIInstance._TopRoot.transform;
            LE.duration = dur;
            LE.height = high;
            LE.myNum = go.GetComponent<UILabel>();
            LE.myNum.text = str;
            LE.myNum.color = tColor;
			LE.myNum.depth = 300;
            LE.delayTime = tDelayTime;

            return LE;
        }
        return null;
    }






}
