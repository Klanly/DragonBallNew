using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
public static class RED {
	
	public static bool bDebug = true;
	
    public static Color btnClickColor = new Color(1f,215f/255f,0,1f);
    public static Color btnDefaultColor = new Color(1f,215f/255f,190f/255f,1f);
	public static Color btnWhiteColor = Color.white;
	
	static public void AddChild (GameObject child,GameObject parent,Vector3? LocalPostion = null)
	{
		if (child !=null && parent != null)
		{
			Transform t = child.transform;
			t.parent = parent.transform;
			if(LocalPostion == null) LocalPostion = Vector3.zero;
			t.localPosition = (Vector3)LocalPostion;
			t.localRotation = Quaternion.identity;
			t.localScale = Vector3.one;
			child.layer = parent.layer;
		}
	}

	//保留旋转的状态
	static public void AddChildResvere (GameObject child,GameObject parent)
	{
		if (child !=null && parent != null) {
			Transform t = child.transform;
			t.parent = parent.transform;
			t.localPosition = Vector3.zero;
			t.localScale = Vector3.one;
			child.layer = parent.layer;
		}
	}

	public static void RemoveChilds(Transform parent)
	{
        if (parent != null) {
            while (parent.childCount > 0) {
                GameObject.Destroy (parent.GetChild (0).gameObject);
            }
        }
	}

	public static void RemoveChildsImmediate(Transform parent)
	{
        if (parent != null) {
            while (parent.childCount > 0) {
                GameObject.DestroyImmediate (parent.GetChild (0).gameObject);
            }
        }
	}
	static public void Log(string msg)
	{
		if(bDebug)
		{
			Debug.Log(msg);
		}
	}



	public static string Replace(string str,params object[] objs)
	{
		string newString = "";
		
		string[] strs = str.Split('#');
		
		for(int i=0;i<strs.Length;i++)
		{
			newString +=strs[i];
			if(i<objs.Length)
			{
				newString += ""+objs[i];
			}
		}
		return newString;
	}
	public static string Replace(int strId,params object[] objs)
	{
		return Replace(Core.Data.stringManager.getString(strId),objs);
	}
	public static string getString(int strId)
	{
		return Core.Data.stringManager.getString(strId);
	}


	//added by zhangqiang at 2014-03-10 
	public static void SetActive(bool bActive, params GameObject[] objs)
	{
		for(int i = 0; i < objs.Length; i++)
		{
			NGUITools.SetActive(objs[i], bActive);
		}
	}

	public static bool IsActive(GameObject obj)
	{
		return obj.activeSelf;
	}

	static public void LogWarning(string msg)
	{
		if(bDebug)
		{
			Debug.LogWarning(msg);
		}
	}
	
	public static void LogError(string msg)
	{
		if(bDebug)
		{
			Debug.LogError(msg);
		}
	}

	public static void SetBtnSprite(UIButton btn, string strSpriteName)
	{
		btn.normalSprite = strSpriteName;
	}

	public static void changeButtonState(UIButton button, bool isEnabled)
	{
		button.enabled = isEnabled;
		button.isEnabled = isEnabled;
	}

    /// <summary>
    /// 加 topUI  black mask
    /// visible 0表示对象物体被干掉，1 表示物体隐藏
    /// </summary>
    /// <param name="curPanel">Current panel.</param>
    public static void TweenShowDialog(GameObject curPanel , int visible = 0 )
	{   
		if (DBUIController.mDBUIInstance == null)
		{
			return;
		}
        CloseUILogic Cul;
		//RED.Log(" tween show");
        if (curPanel.GetComponent<UIPanel>().depth < 20)
        {
            curPanel.GetComponent<UIPanel>().depth = 100;
        }
        else
        {
			DBUIController.mDBUIInstance.blackMaskPanel.depth = curPanel.GetComponent<UIPanel> ().depth - 1;
        }
        GameObject tBlackAlpha;
        if (curPanel.transform.FindChild("pbBlackAlphaMask(Clone)")== null)
        {
            DBUIController.mDBUIInstance.InitBlackMask();
            tBlackAlpha = DBUIController.mDBUIInstance.GetBlackMask();
           
        }
        else
        {
            tBlackAlpha = curPanel.transform.FindChild("pbBlackAlphaMask(Clone)").gameObject;
        }

        curPanel.transform.localScale = Vector3.zero;



//		if (ts != null)
//			ts.animationCurve = anim;
//        else
//        {
//			curPanel.AddComponent<TweenScale> ();
//			ts = curPanel.GetComponent<TweenScale> ();
//        }
//		ts.enabled = true;
//		ts.updateTable = true;
//		ts.animationCurve = anim;


        if (tBlackAlpha.GetComponent<CloseUILogic>() != null ) 
        {
            Cul = tBlackAlpha.GetComponent<CloseUILogic>(); 

        }else
        {
            Cul = tBlackAlpha.AddComponent<CloseUILogic>(); 
        }
        if (visible == 0 )
        {
            Cul._game = curPanel  ;

        }else
        {
            Cul._gameVisible = curPanel  ;
        }


//		TweenScale.Begin(curPanel, SQYMainController.TWEENSCALE_TIME+0.2f, Vector3.one);
		TweenScale  ts = curPanel.GetComponent<TweenScale> ();
		if (ts == null) {
			curPanel.AddComponent<TweenScale> ();
			ts = curPanel.GetComponent<TweenScale> ();
		}
		ts.from = Vector3.zero;
		AnimationCurve anim = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(0.4f, 1.25f, 0.5f, 0.5f), new Keyframe(1f, 1f, 1f, 0f));
		ts.animationCurve = anim;
		ts.duration = 0.3f;

		ts.enabled = true;
        tBlackAlpha.transform.parent = curPanel.transform;
        tBlackAlpha.transform.localScale = Vector3.one;
        
    }

	public static string GetChineseNum(int num)
	{
		string strNum = "一";
		switch (num)
		{
			case 1:
				strNum = "-";
				break;
			case 2:
				strNum = "二";
				break;
			case 3:
				strNum = "三";
				break;
			case 4:
				strNum = "四";
				break;
			case 5:
				strNum = "五";
				break;
			case 6:
				strNum = "六";
				break;
			case 7:
				strNum = "七";
				break;
			case 8:
				strNum = "八";
				break;
			case 9:
				strNum = "九";
				break;
		}
		return strNum;
	}
	
}
