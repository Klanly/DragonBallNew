using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//yangchenguang 2d 动画新手引导
public class AnimationGuide2D : MonoBehaviour 
{

    public List<UISprite> liSp = new List<UISprite>();
    private List<Vector3> listV3 = new List<Vector3>();
    int index = -1 ; 
    private List<int> localPos = new List<int>() {397 , -195 ,305 ,-289,225,525,-265};
    private float animationTime =0.5f; // 每张图片移动的速度
    private float scaleTime =0.3f; // 放大的时间
    TweenPosition tp ;
    TweenScale ts ;
    public System.Action onFinished ;
	// Use this for initialization
	void Start ()
    {
        GameObject g ; 
        if (liSp.Count > 0 )
        {
            for (int i = 0;  i < liSp.Count ; i++)
            {
                g = liSp[i].gameObject;
                listV3.Add(g.transform.localPosition);
               // g.SetActive(false);

                if ( i == 5)
                {
                    g.transform.localPosition += new Vector3(localPos[i],0,0);
                }else
                {
                    g.transform.localPosition += new Vector3(0,localPos[i],0);
                }
             
            }
        }
        startMove();

	}
    public static AnimationGuide2D CreateGuidView()
    {
       
        Object obj  = PrefabLoader.loadFromPack("YCG/UIPanel2DAnimation");

        if(obj != null)
        {
            GameObject go = Instantiate(obj) as GameObject;
            AnimationGuide2D cc = go.GetComponent<AnimationGuide2D>();

			RED.AddChild (go , DBUIController.mDBUIInstance._TopRoot );

            return cc;
        }
        return null;
    }

    // 开始运动图片
    void startMove()
    {
        index++;

        if (index >= 7 )
        {
            tp.onFinished.Clear();
           // startScale();
            Invoke("startScale" , 0.5f); //等待的时间
            //动画播放完成
            return ;
        }


        tp  =   TweenPosition.Begin(liSp[index].gameObject , animationTime ,listV3[index] );

        tp.onFinished.Clear();
        tp.onFinished .Add(new EventDelegate(this ,"startMove"));
		tp.ResetToBeginning();
        tp.PlayForward();
    }
    void startScale()
    {

      
        ts = TweenScale.Begin(gameObject ,scaleTime , new Vector3(10,10,10));
        ts.onFinished.Clear();
        ts.onFinished.Add(new EventDelegate(this, "ScaleEnd"));
		ts.ResetToBeginning();
        ts.PlayForward();
        for(int i = 0; i <liSp.Count; i++)
        {
            liSp[i].alpha =0.5f;
        }


    }
    // 所有动画播放完成
    void ScaleEnd()
    {
        if (gameObject != null )
        gameObject.SetActive(false);
        if(onFinished != null)
        onFinished();


        Destroy(gameObject);

    }
    void OnDestroy()
    {
        if (tp != null )
        {
            tp.onFinished.Clear();
        }
        if (ts != null )
        {
            ts.onFinished.Clear();
        }
       
        tp= null ; 
        ts =null ; 

    }
	// Update is called once per frame
//	void Update () 
//    {
//	
//	}
}
