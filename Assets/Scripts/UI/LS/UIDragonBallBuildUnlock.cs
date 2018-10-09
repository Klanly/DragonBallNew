using UnityEngine;
using System.Collections;

public class UIDragonBallBuildUnlock : RUIMonoBehaviour
{

	static UIDragonBallBuildUnlock mInstance;
	public static UIDragonBallBuildUnlock GetInstance()
	{
		return mInstance;
	}
	
	public UILabel mContent;
	public UILabel mHaveunLock;
	public UISprite mIcon;     //buildatlas
	public UISprite mIcon_Two;    //challenge doungen
	public UISprite mIcon_Three ; //Head

	public System.Action OnClose;

	void OnClick()
	{
		Core.Data.deblockingBuildMgr.BeginUnlock();
		this.dealloc();
		if(OnClose != null)
		{
			 mInstance = null;
			OnClose();
		}
	}

	void OnDestroy()
	{
		mInstance = null;
	}

	int IsBuildAtlas(string m_Atlas)
	{
		switch(m_Atlas)
		{
			case "bgAtlas":
				return 1;
			case "chinaAtlas":
				return 3;
			case "pveAtlas":
			case "challengebAtlas":
				return 2;
		}
		return 0;
	}

	void ResetIcon(string m_Atlas, string m_icon)
	{
		if(m_Atlas == "bgAtlas")
		{
			mIcon.MakePixelPerfect();
			mIcon.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
		}
		else if(m_Atlas == "challengebAtlas" || m_Atlas == "pveAtlas")
		{
			if(m_icon == "challenge-0005" || m_icon == "challenge-0006" || m_icon == "jineng" || m_icon == "tanbao" || m_icon == "zhanhun")
			{
				mIcon_Two.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0f,0f,90f));
				mIcon_Two.width = 140;
				mIcon_Two.height = 130;
			}
			else
			{
				mIcon_Two.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0f,0f,0f));
				mIcon_Two.width = 130;
				mIcon_Two.height = 140;
			}

		}
		else
		{
			if(m_icon == "EUA0-13")
			{
				mIcon_Three.spriteName = "EUA0-13";
			}
			else
			{
				mIcon_Three.spriteName = "wuzhe";
			}
			mIcon_Three.MakePixelPerfect();
		}
	}

	public static UIDragonBallBuildUnlock OpenUI(string m_content, string m_icon, string m_Atlas)
	{
		if(mInstance == null)
		{
			Object obj = PrefabLoader.loadFromPack("LS/pbLSDragonBallBuildUnlock");
			if(obj != null)
			{
				GameObject go = Instantiate(obj) as GameObject;
				if(go != null)
				{
					RED.AddChild(go, DBUIController.mDBUIInstance._TopRoot);
					mInstance = go.GetComponent<UIDragonBallBuildUnlock>();
	                mInstance.mContent.SafeText(m_content);
					if(mInstance.IsBuildAtlas(m_Atlas) == 1)
					{
						mInstance.mIcon.gameObject.SetActive(true);
						mInstance.mIcon_Two.gameObject.SetActive(false);
						mInstance.mIcon_Three.gameObject.SetActive(false);
						mInstance.mIcon.spriteName = m_icon;
					}
					else if(mInstance.IsBuildAtlas(m_Atlas) == 2)
					{
						mInstance.mIcon_Two.gameObject.SetActive(true);
						mInstance.mIcon.gameObject.SetActive(false);
						mInstance.mIcon_Three.gameObject.SetActive(false);
						mInstance.mIcon_Two.spriteName = m_icon;
					}
					else if(mInstance.IsBuildAtlas(m_Atlas) == 3)
					{
						mInstance.mIcon_Two.gameObject.SetActive(false);
						mInstance.mIcon.gameObject.SetActive(false);
						mInstance.mIcon_Three.gameObject.SetActive(true);
						mInstance.mIcon_Three.spriteName = m_icon;
					}

					mInstance.ResetIcon(m_Atlas,m_icon);
	            }
	        }
		}
		return mInstance;
    }
}
