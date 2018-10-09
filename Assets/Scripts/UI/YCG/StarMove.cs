using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//星星移动yangchenguang
public class StarMove : MonoBehaviour 
{
    public UISprite sp ;
    public GameObject tap; // 按钮
    //int TimeCount =-1;
    TweenPosition tp  ;
    TweenScale ts1;
    TweenScale ts;
    public UISprite tapUISprite;// 按钮背景
	// Use this for initialization
	void Start ()
    {

	}
    public void setBtnXing()
    {
        sp.gameObject.transform.localScale = new Vector3(0.4f,0.4f , 0.4f );

        moveX();
    }
   
    private void moveX()
    {
       

        sp.gameObject.SetActive(true);
        int  h   ; 
        int  w  ;
        Vector3 pos ;
        UISprite sp1  ; 
        if (tapUISprite == null )
        {
            sp1=tap.GetComponentInChildren<UISprite>();

        }else
        {
            sp1 = tapUISprite ;
        }
        if (sp1 == null ) 
        {
            if (sp == null ) return ;
            if ( sp.gameObject.activeSelf )
            {
                sp.gameObject.SetActive(false) ; 
            }
            return ;

        }
        h  =   sp1.height ; 
        w  =   sp1.width;
        pos = tap.gameObject.transform.localPosition ;




        float infoX = -(w/2);
        float infoY = (h/2);
        sp.alpha =1f;

        Vector3 posStop ;

        pos +=  new Vector3(infoX,infoY-5,0) ; 

        sp.gameObject.transform.localPosition =pos;
        posStop = pos + new Vector3(w-5,0,0) ;

        sp.gameObject.transform.localScale = new Vector3(0.4f,0.4f , 0.4f );

        //MiniItween.MoveTo(sp.gameObject ,posStop , 0.9f);


//         tp =   TweenPosition.Begin(sp.gameObject , 0.9f ,posStop );
//
//        tp.onFinished.Clear();
//     
//        tp.onFinished .Add(new EventDelegate(this ,"onfinshi"));
//		tp.ResetToBeginning();
//        tp.PlayForward();


        MiniItween.MoveTo(sp.gameObject,posStop,0.9f ).FinishedAnim =onfinshi ;


    }


    //IEnumerator  moveXing(UISprite sp)
    ///{


        // 缩放
//        TweenScale tween = gameObject.GetComponent<TweenScale>();
//        tween.delay = 0;
//        tween.duration = 0.25f;
//        tween.from =  Vector3.one;
//        tween.to = new Vector3(0.01f,0.01f,0.01f);
//        tween.onFinished.Clear();
//        tween.onFinished.Add(new EventDelegate(this,"DestroyPanel"));
//        tween.Reset();
//        tween.PlayForward();
     

   // }
  
    public  void onfinshi()
    {

//        tp.enabled = false ; 
//
//        ts1 =   TweenScale.Begin(sp.gameObject,0.1f,Vector3.one);
//
//        ts1.onFinished.Clear();
//        ts1.onFinished .Add(new EventDelegate(this ,"DestroyPanel"));
//		ts1.ResetToBeginning();
//
//        ts1.PlayForward();

        MiniItween.ScaleTo(sp.gameObject,Vector3.one,0.1f ).FinishedAnim =DestroyPanel ;

    }
   
    public void DestroyPanel()
    {
//        ts1.RemoveOnFinished(new EventDelegate(this ,"DestroyPanel")) ;
//        ts1.enabled = false ; 
//       
//     
//         ts =   TweenScale.Begin(sp.gameObject,0.4f,new Vector3(0.4f, 0.4f , 0.4f ));
//        ts.onFinished.Clear();
//        ts.onFinished .Add(new EventDelegate(this ,"timeScale"));
//		ts.ResetToBeginning();
//        ts.PlayForward();

        MiniItween.ScaleTo(sp.gameObject,new Vector3(0.4f, 0.4f , 0.4f ),0.4f ).FinishedAnim =timeScale ;

    }
    private void timeScale()
    {

//        ts.RemoveOnFinished (new EventDelegate(this ,"timeScale"));
//        ts.enabled = false ;


        sp.gameObject.SetActive(false);
     
        Invoke("loopAnimation",2.5f);
    }
    private void loopAnimation()
    {
       
        CancelInvoke("loopAnimation");
        moveX();
    }

    public void ClearS()
    {

        Destroy(sp.gameObject.GetComponent<MiniItween>());


        CancelInvoke("loopAnimation");
        if (ts != null )
        {
            ts.onFinished.Clear();
			ts.ResetToBeginning();
            ts.enabled =false ; 
        }

        if (ts1 != null )
        {
            ts1.onFinished.Clear();
			ts1.ResetToBeginning();
            ts1.enabled = false;
        }
       
        if (tp != null )
        {
            tp.onFinished.Clear();
			tp.ResetToBeginning();
            tp.enabled = false ; 
        }
      
   
    }

    void OnDestroy()
    {
        sp  = null ; 
        tap = null ;
        ts= null ; 
        ts1 = null ; 
        tp = null ;
    }
}
