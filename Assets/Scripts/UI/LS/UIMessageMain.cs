using UnityEngine;
using System.Collections;

public class UIMessageMain : RUIMonoBehaviour 
{
	public UIInput mInput;
	public UITable mUITable2;
	public UILabel mFreeNum;
	public UILabel m_txtTitle;
	public UIButton mSendBtn;

	private EmotionUI m_emotionUI;			//表情UI
	private QuickChatUI m_quickChatUI;		//快速聊天

	//当前UI是否活着
//	private bool curViewIsAlive;

    private static UIMessageMain _instance;
    public static UIMessageMain Instance
	{
		get{return _instance;}
	}

	private int m_TotalChatCount
	{
		get{
			return Core.Data.vipManager.GetVipInfoData(Core.Data.playerManager.curVipLv).worldchat;
		}
	}

	private int m_nleftCnt;

	void Awake(){
		_instance = this;
//		curViewIsAlive = true;
	}

    void Start() {
        DBUIController.mDBUIInstance.HiddenFor3D_UI();
		SQYMainController.mInstance.SetChatAnimEnable (false);
		m_txtTitle.text = Core.Data.stringManager.getString (5228);
    }

	void OnDestory() {
//		curViewIsAlive = false;
		_instance      = null;
	}

	public void SetFreeNum(int _Num)
	{
		string strText = _Num.ToString() + "/" + m_TotalChatCount.ToString();
		mFreeNum.text = strText;
		m_nleftCnt = _Num;
		if(m_nleftCnt <= 0)mSendBtn.isEnabled = false;
		else mSendBtn.isEnabled = true;
	}

    public void OnShow()
    {
		CheckSocketConnect();
		mUITable2.Reposition();
		SetFreeNum( MessageMgr.GetInstance()._NowCount);
		MessageMgr.GetInstance().ShowChatList();
    }

	void CheckSocketConnect()
	{
		if(Core.NetEng.SockEngine.curConnectType != SocketEngine.isConnectType.isConnected)
		{
			UIInformation.GetInstance ().SetInformation (Core.Data.stringManager.getString (7365), Core.Data.stringManager.getString (7132), null);
		}
	}

    void Back_OnClick()
    {
		MessageMgr.GetInstance().DeleteChatCell();
		DBUIController.mDBUIInstance.ShowFor2D_UI();
		DBUIController.mDBUIInstance.RefreshUserInfo ();
        Destroy(gameObject);

    }

	void Send_OnClick()
	{
		if(m_nleftCnt <= 0)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(20093));
			return;
		}

		string mContent = "";
		if(mInput != null)
		{
			mContent = mInput.value;
			if(mInput.value.Contains("#$&") || mInput.value.Contains("{{@}}"))
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(75003));
				return;
			}

			if(GetTextLength() > 50)
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(25132));
				return;
            }

			MessageMgr.GetInstance().SendWorldChat(mContent);
			mInput.value = "";
		}
	}   

	int GetTextLength()
	{
		string strText = mInput.value;
		string[] strTemp = strText.Split('<');
		int count = 0;
		for(int i = 0; i < strTemp.Length; i++)
		{
			if(strTemp[i].Contains("<") && strTemp[i].Contains(">"))
			{
				count++;
			}
		}

		return mInput.value.Length - count;
	}

	void OnClickEmotion()
	{
		if (m_emotionUI == null)
		{
			m_emotionUI = EmotionUI.OpenUI ();
		}
		else
		{
			bool bShow = !m_emotionUI.IsShow;
			m_emotionUI.SetShow (bShow);
		}

		if(m_quickChatUI != null && m_quickChatUI.IsShow)
		{
			m_quickChatUI.SetShow(false);
		}
	}

	void OnClickQuickChat()
	{
		if (m_quickChatUI == null)
		{
			m_quickChatUI = QuickChatUI.OpenUI ();
		}
		else
		{
			bool bShow = m_quickChatUI.IsShow;
			m_quickChatUI.SetShow (!bShow);
		}

		if(m_emotionUI != null && m_emotionUI.IsShow)
		{
			m_emotionUI.SetShow(false);
		}
	}

	//add text
	public void AppendText(string strText)
	{
		mInput.value = strText;
		m_quickChatUI.SetShow(false);
	}

	//add emotion
	public void AppendEmotion(string strText)
	{
		mInput.value += strText;
		m_emotionUI.SetShow(false);
	}

	void OnDestroy()
	{
		mInput = null;
		mUITable2 = null;
		mFreeNum = null;

		MessageMgr.GetInstance().Clear();
		_instance = null;
    }

	void HideEmotionAndQuickUI()
	{
		if(m_emotionUI != null && m_emotionUI.IsShow)
		{
			m_emotionUI.SetShow(false);
		}

		if(m_quickChatUI != null && m_quickChatUI.IsShow)
		{
			m_quickChatUI.SetShow(false);
		}
	}
}
