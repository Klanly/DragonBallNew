using UnityEngine;
using System.Collections;

public class UIDragonBallBuyNum : RUIMonoBehaviour
{

	static UIDragonBallBuyNum mInstance;
	public static UIDragonBallBuyNum GetInstance()
	{
		return mInstance;
	}

	public UILabel mContent;

	public delegate void CallBack();
	CallBack mCallBack;

	public void SetContent(string m_content, CallBack m_CallBack)
	{
		mContent.SafeText(m_content);
		mCallBack = m_CallBack;
	}

	void OkClick()
	{
		if(mCallBack != null)
		{
			mCallBack();
			mCallBack = null;
		}
		CloseClick();
	}

	void CloseClick()
	{
		this.dealloc();
	}

	void OnDestroy()
	{
		mInstance = null;
	}

    public static void OpenUI(string m_content,CallBack tCallB)
	{
		Object obj = PrefabLoader.loadFromPack("LS/pbLSDragonBallBuyNum");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			if(go != null)
			{
				mInstance = go.GetComponent<UIDragonBallBuyNum>();
				mInstance.mContent.SafeText(m_content);
                RED.TweenShowDialog(go);
                mInstance.SetContent(m_content,tCallB);
                go.transform.parent = DBUIController.mDBUIInstance._TopRoot.transform;
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = Vector3.zero;
			}
		}
	}

}
