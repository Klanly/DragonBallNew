using UnityEngine;
using System.Collections;

public enum InformationType
{
    InformationType_NONE,
    InformationType_SINGLE,
    InformationType_DOUBLE,
}

public class UIInformation
{

    static UIInformation mUIInformation;
    public static UIInformation GetInstance()
    {
        if(mUIInformation == null)
        {
            mUIInformation = new UIInformation();
        }
        return mUIInformation;
    }

    private UIInformation()
    {

    }

    public delegate void CallBack();
    CallBack callBack = null;
	CallBack callCancel = null;
    UIMallOldMan _UIMallOldMan = null;

   public  UIMallOldMan mUIMallOldMan
    {
        get{
            if(_UIMallOldMan == null)
            {
                _UIMallOldMan = DragonMallOldManPanelScript.CreateOldManPanel();
				if (DBUIController.mDBUIInstance != null)
				{
					RED.AddChild (_UIMallOldMan.gameObject, DBUIController.mDBUIInstance._TopRoot);
				}
				else
				{
					RED.AddChild (_UIMallOldMan.gameObject, UICamera.mainCamera.gameObject);

					UIButton[] btns = _UIMallOldMan.gameObject.GetComponentsInChildren<UIButton>();
					if(btns != null) {
						foreach(UIButton bn in btns) {
							bn.gameObject.layer = LayerMask.NameToLayer("UI");
						}
					}

				}
            }
            return _UIMallOldMan;
        }
    }


    public void SetInformation(string m_ContentTex, string m_BtnText1, CallBack mcallback , CallBack cancel = null)
    {
        callBack = mcallback;
		callCancel = cancel;
        mUIMallOldMan.SetAllData(InformationType.InformationType_SINGLE,m_ContentTex,m_BtnText1,"","");
        SetOldManPanelActive();
    }

    public void SetInformation(string m_ContentTex, string m_BtnTextOk, string m_BtnTextCancel, CallBack mcallback)
    {
        callBack = mcallback;
		mUIMallOldMan.SetAllData(InformationType.InformationType_DOUBLE, m_ContentTex, m_BtnTextOk, "", m_BtnTextCancel);
        SetOldManPanelActive();
    }

    void SetOldManPanelActive()
    {
        mUIMallOldMan.gameObject.SetActive(true);
    }
	
	public void OnCancel()
	{
		if(callCancel != null)
			callCancel();
		callBack = null;
	}
	
    public void OnCallback()
    {
        if(callBack != null)
        {
            callBack();

        }
        callBack = null;
    }
}

public class  DragonMallOldManPanelScript: RUIMonoBehaviour 
{
    public static UIMallOldMan CreateOldManPanel()
    {
        UnityEngine.Object obj = PrefabLoader.loadFromPack("LS/pbLSDragonBallMallOldMan");
        if(obj != null)
        {
            GameObject go = Instantiate(obj) as GameObject;
            UIMallOldMan cc = go.GetComponent<UIMallOldMan>();
            RED.TweenShowDialog(go);
            return cc;
        }
        return null;
    }
}
