using UnityEngine;
using System.Collections;

public class ActivityExplain : MonoBehaviour
{
    public UILabel explainLabel  ;
    TweenScale ts ;
	// Use this for initialization
    Vector3 v3 = new Vector3 (-193.4f ,132.8f,0) ;
    Vector3 uiP;
    float localY;
    BoxCollider boxc ;
	void Start ()
    {
        ts = gameObject.GetComponent<TweenScale>();
	}
    //设置说明
    public string setExplainLabel
    {

        set 
        {

            boxc = explainLabel.gameObject.GetComponent<BoxCollider>();


            explainLabel.text = value   ;

            //初始化位置
            uiP = explainLabel.transform.parent.localPosition ;
         
            localY  =    v3.y -  uiP.y ; 

            explainLabel.transform.localPosition = new Vector3( -193.4f , localY , 0 ) ;



            boxc.size = new Vector3 (explainLabel.width , explainLabel.height ,  0) ;

         
            boxc.center = new Vector3( boxc.center.x ,-explainLabel.height/2 ,  0 ) ; 
        }
    }
    //隐藏界面
//    public void  visibleUI(GameObject go)
//    {
//		ts.ResetToBeginning();
//        this.gameObject.SetActive(false);
//        ts.enabled= true ;
//
//    }
    public void visibleUI()
    {
		ts.ResetToBeginning();
        this.gameObject.SetActive(false);
        ts.enabled= true ;
    }
}
