using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageMgr
{
    static MessageMgr mMessageMgr;
    public static MessageMgr GetInstance()
    {
        if(mMessageMgr == null)
        {
            mMessageMgr  = new MessageMgr();
        }
        return mMessageMgr;
    }

    private MessageMgr()
    {

    }

    int m_MaxCount;
    public int MaxCount{
        get{
            return m_MaxCount;
        }
        set{
            m_MaxCount = value;
        }
    }

    bool m_IsTypeOne;
    public bool IsTypeOne
    {
        get{
            return m_IsTypeOne;
        }
        set{
            m_IsTypeOne = value;
        }
    }

    const int _MaxNum = 50;
	int _NowChatCellIndex = 0;
	public int _NowCount;

	public Queue<SockWorldChatData>  mChatArray = new Queue<SockWorldChatData>();  //聊天队列缓存
	Queue<ChatCellStruct> mChatCellStructList = new Queue<ChatCellStruct>();  //用来显示的聊天列表

    UIMessageTag mUIMessageTag = null;
    UIMessageMain mUIMessageMain = null;
    UIMessageMail mUIMessageMail = null;

	public string Upgrape = "";

    //main panel 
    void CreateInfoPanel()
    {
        UnityEngine.Object obj = PrefabLoader.loadFromPack("LS/pbLSInformation");
        if(obj != null)
        {
            GameObject go = RUIMonoBehaviour.Instantiate(obj) as GameObject;
            mUIMessageMain = go.GetComponent<UIMessageMain>();
            RED.AddChild(go.gameObject,DBUIController.mDBUIInstance._bottomRoot);
        }
    }

    public void SetInfoPanel(bool key)
    {
        if(mUIMessageMain == null)
        {
            CreateInfoPanel();
        }
        mUIMessageMain.OnShow();
        mUIMessageMain.gameObject.SetActive(key);
    }


	UIMessageChatCell CreateMessageChatCell()   //创建聊天的Cell
	{
		UIMessageChatCell mm = null;
		GameObject obj1 = PrefabLoader.loadFromPack("LS/pbLSInformationChatCell", true, true) as GameObject;

		if(obj1 != null)
		{ 
			GameObject go = NGUITools.AddChild (mUIMessageMain.mUITable2.gameObject, obj1);
			go.gameObject.name =  (999 - _NowChatCellIndex).ToString();
			mm = go.gameObject.GetComponent<UIMessageChatCell>();
			_NowChatCellIndex ++;
		}
		return mm;
	}
		
    
    public void Reposition(UIScrollView m, UIGrid g)
    {
        g.Reposition();

    }


    // information  tag...  include delete, add friend, receive gift, send message and so on
    void CreateMessageTag()
    {
        UnityEngine.Object obj = PrefabLoader.loadFromPack("LS/pbLSInformationTag");
        if(obj != null)
        {
            GameObject go = RUIMonoBehaviour.Instantiate(obj) as GameObject;
            mUIMessageTag = go.GetComponent<UIMessageTag>();
            RED.AddChild(go.gameObject, DBUIController.mDBUIInstance._bottomRoot);
        }
    }

    public void SetMessageTag(bool key, SystemInformationTypes mTypes)
    {
        ReduceMessageCount();

        if(mUIMessageTag == null)
        {
            CreateMessageTag();
        }
//        mUIMessageTag.OnShow(mTypes);
//        mUIMessageTag.gameObject.SetActive(key);
    }

    //send mail
    void CreateMessageMail()
    {
        UnityEngine.Object obj = PrefabLoader.loadFromPack("LS/pbLSInformationMail");
        if(obj != null)
        {
            GameObject go = RUIMonoBehaviour.Instantiate(obj) as GameObject;
            mUIMessageMail = go.GetComponent<UIMessageMail>();
            RED.AddChild(go.gameObject, DBUIController.mDBUIInstance._bottomRoot);
        }
    }

    public void SetMessageMail(bool key)
    {
        if(mUIMessageMail == null)
        {
            CreateMessageMail();
        }
        mUIMessageMail.OnShow();
        mUIMessageMail.gameObject.SetActive(key);
    }

    public void MessageRequest(bool key)
    {
//        IsTypeOne = key;
//        MessageInfoParam param = new MessageInfoParam(Core.Data.playerManager.PlayerID, MaxCount);
//        HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
//        task.AppendCommonParam(RequestType.MESSAGE_INFORMATION, param);
//        
//        task.afterCompleted += SetMessageInfoData;  
//        task.DispatchToRealHandler();
    }

	public void Clear()
	{
		this._NowChatCellIndex = 0;

		foreach(ChatCellStruct cell in mChatCellStructList){
			if(cell != null && cell._UIMessageChatCell != null) {
				cell._UIMessageChatCell.dealloc();
				cell._UIMessageChatCell = null;
			}
		}

	}

    void ReduceMessageCount()
    {
        MaxCount--;
    }
	

    //聊天Socket的请求处理
	public void RegisterChatSock()
	{
		SocketTask task = new SocketTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.SOCK_WORLDCHAT, new SockSendWorldChatParam(""));
		task.ErrorOccured = SocketResp_Error;
		task.afterCompleted = SocketResp_UI;

		Core.NetEng.SockEngine.RegisterSocketTask(task);
	}

	public void SendWorldChatLogin()
	{
		RegisterChatSock();

		SocketTask task = new SocketTask(ThreadType.MainThread, TaskResponse.Default_Response);
        task.AppendCommonParam(RequestType.SOCK_WORLDCHATLOGIN, new SockWorldChatLoginParam(Core.Data.playerManager.PlayerID, "abc", 
            Core.Data.playerManager.NickName, Core.Data.playerManager.Lv, Core.SM.curServer.sid, (long)Core.Data.playerManager.RTData.headID));

		task.ErrorOccured = SocketResp_Error;
		task.afterCompleted = SocketResp_UI;
		
		task.DispatchToRealHandler();
	}

	public void SendWorldChat(string _Content)
	{
		if(SensitiveFilterManager.getInstance().check(_Content, false))
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(75003));
			return;
        }
		if(string.IsNullOrEmpty(_Content))
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(75002));
			return;
		}
        
		SocketTask task = new SocketTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.SOCK_WORLDCHAT, new SockSendWorldChatParam(_Content));
		task.ErrorOccured = SocketResp_Error;
		task.afterCompleted = SocketResp_UI;
		task.DispatchToRealHandler();
		ComLoading.Open();
	}

	public void SendWorldChat2(string _Content)
	{
		
		SocketTask task = new SocketTask(ThreadType.MainThread, TaskResponse.Default_Response);
        task.AppendCommonParam(RequestType.SOCK_WORLDCHAT, new SockSendWorldChatParam(_Content));
		task.ErrorOccured = SocketResp_Error;
		task.afterCompleted = SocketResp_UI;
		
		task.DispatchToRealHandler();


	}

	public void CloseWorldChatSocket()
	{
		SocketTask task = new SocketTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCmdParam(InternalRequestType.SHUT_DOWN);
		task.DispatchToRealHandler();
	}

	void SocketResp_Error(BaseSocketRequest request, string error)
	{
		ConsoleEx.DebugLog("---- Socket Resp - Error has ocurred." + error);
	}
	
    //聊天Socket的返回处理
	void SocketResp_UI(BaseSocketRequest request, BaseResponse response)
	{
		ComLoading.Close();
		if (response != null && response.status != BaseResponse.ERROR) 
		{
            SocketRequest sR = request as SocketRequest;
			switch(sR.Type)
			{
			case RequestType.SOCK_WORLDCHATLOGIN:
				SockLoginWorldChatResponse SockLogin = response as SockLoginWorldChatResponse;
				_NowCount = SockLogin.data.chatCountPerDay;
//					mUIMessageMain.SetFreeNum(_NowCount.ToString());
				ActivityNetController.isInActivity = false;
					LoginRequestList(SockLogin.data);
					break;
            case RequestType.SOCK_WORLDCHAT:
                SockWorldChatResponse SockChat = response as SockWorldChatResponse;
                AddChatArray (SockChat.data);
				if(string.IsNullOrEmpty(SockChat.data.content))return;
				if(!CheckNormalOrVideo(SockChat.data.content) && SockChat.data.roleid.ToString() == Core.Data.playerManager.PlayerID)
				{
					if(SockChat.data.chatCountPerDay <=0)
                    {

                        if(Core.SM.CurScenesName == SceneName.GAME_BATTLE)
                        {

                           // RED.AddChild(go.gameObject, BanBattleManager.Instance.go_uiPanel);

                            SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(25129),BanBattleManager.Instance.go_uiPanel);

                        }else
                        {
                            SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(25129));
                        }


                       
                    }
                       
					else 
                    {
                        if(Core.SM.CurScenesName == SceneName.GAME_BATTLE)
                        {
                            SQYAlertViewMove.CreateAlertViewMove(string.Format(Core.Data.stringManager.getString(25053), SockChat.data.chatCountPerDay),BanBattleManager.Instance.go_uiPanel);
                        }else
                        {
                            SQYAlertViewMove.CreateAlertViewMove(string.Format(Core.Data.stringManager.getString(25053), SockChat.data.chatCountPerDay));
                        }



                    }
                        
				}
                if (mUIMessageMain != null)
				{
					if (SockChat.data.roleid.ToString ().Equals (Core.Data.playerManager.PlayerID))
					{
	                    _NowCount = SockChat.data.chatCountPerDay;

	                    if (mUIMessageMain.gameObject.activeInHierarchy)
						{

                        mUIMessageMain.SetFreeNum (_NowCount);
						}
					}
					Debug.Log("Here is always connect!");
					ShowChatCellUpdata ();
				} 
				else if(SQYMainController.mInstance != null)
				{
					SQYMainController.mInstance.SetChatAnimEnable (true);
				}
				break;
			}
		}
		else if(response != null && response.status == BaseResponse.ERROR)
		{
            if(Core.SM.CurScenesName == SceneName.GAME_BATTLE)
            {
                SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode),BanBattleManager.Instance.go_uiPanel);

            }else
            {
                SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
            }

			
		}
	}

	bool CheckNormalOrVideo(string _Str)
	{
		//false 视频   true 普通聊天
		if((_Str.Length - _Str.Replace("#", string.Empty).Length == 2) && (_Str.Length-_Str.Replace("$", string.Empty).Length == 2) && (_Str.Length-_Str.Replace("&", string.Empty).Length == 2))
		{
			if(_Str.Contains("#$&"))
			{
				return false;
			}
		}
		return true;
    }

	void LoginRequestList(SockLoginWorldChatData data)
	{
		mChatArray.Clear();
		mChatCellStructList.Clear();

		if(data.contentList != null) {
			int length = data.contentList.Length;
			for(int i=0; i< length; i++)
				mChatArray.Enqueue(data.contentList[i]);
		}

	}

	//时时创建UI cell
	void ChatCellUpdata()
	{
		int count = mChatArray.Count;
		for(int i=0; i<count; i++)
		{
			if(mChatCellStructList.Count > _MaxNum)
			{
				mChatCellStructList.Dequeue()._UIMessageChatCell.dealloc();
			}
			
			SockWorldChatData _tempdata = mChatArray.Dequeue();
			ChatCellStruct _ChatCellStruct;
			if(CheckNormalOrVideo(_tempdata.content))
			{
				_ChatCellStruct = new ChatCellStruct(CreateMessageChatCell(), _tempdata, ChatCellType.ChatCellType_NORMAL, _NowCount);
			}
			else
			{
				_ChatCellStruct = new ChatCellStruct(CreateMessageChatCell(), _tempdata, ChatCellType.ChatCellType_VIDEO, _NowCount);
			}
			_ChatCellStruct._UIMessageChatCell.OnShow(_ChatCellStruct._SockWorldChatData, _ChatCellStruct._ChatCellType);
            mChatCellStructList.Enqueue(_ChatCellStruct);
        }
    }
    
    //缓存聊天列表用于没打开UI情况
	void CacheChatList()
	{
		int count = mChatArray.Count;
		for(int i=0; i<count; i++)
		{
			if(mChatCellStructList.Count > _MaxNum)
			{
				mChatCellStructList.Dequeue()._UIMessageChatCell.dealloc();
			}
			
			SockWorldChatData _tempdata = mChatArray.Dequeue();
			if(CheckNormalOrVideo(_tempdata.content))
			{
				mChatCellStructList.Enqueue(new ChatCellStruct(CreateMessageChatCell(), _tempdata, ChatCellType.ChatCellType_NORMAL, _NowCount));
			}
			else
			{
				mChatCellStructList.Enqueue(new ChatCellStruct(CreateMessageChatCell(), _tempdata, ChatCellType.ChatCellType_VIDEO, _NowCount));
            }
//			_NowCount++;
        }
    }
    
    //Init列表设置内容
    void SetChatListData()
	{
		for(int i=0; i<mChatCellStructList.Count; i++)
		{
			ChatCellStruct mm = mChatCellStructList.Dequeue();
			if(mm._UIMessageChatCell == null)
			{
				mm.SetCellScript(CreateMessageChatCell());
			}
//			mm._UIMessageChatCell.OnShow(mm._SockWorldChatData,mm._ChatCellType);
			mChatCellStructList.Enqueue(mm);
		}
	}

	//聊天列表设置内容
	void SetChatListAllData()
	{
		for(int i=0; i<mChatCellStructList.Count; i++)
		{
			ChatCellStruct mm = mChatCellStructList.Dequeue();
			mm._UIMessageChatCell.OnShow(mm._SockWorldChatData,mm._ChatCellType);
            mChatCellStructList.Enqueue(mm);
		}
	}

	//用于时时更新ChatCell
	void ShowChatCellUpdata()
	{
		ChatCellUpdata();
//		Reposition(mUIMessageMain.mUIScrollView2,mUIMessageMain.mUIGrid2);
		mUIMessageMain.mUITable2.Reposition();
	}

	//用于初始化或者更新标签页
	public void ShowChatList()
	{
		SetChatListData();
        CacheChatList();
		SetChatListAllData();
//		Reposition(mUIMessageMain.mUIScrollView2,mUIMessageMain.mUIGrid2);
		mUIMessageMain.mUITable2.Reposition();
    }


//聊天Cell的队列管理  时时创建更新
	void AddChatArray(SockWorldChatData data)
	{
		if(mChatArray.Count > 29)
		{
			for(int i=29; i<mChatArray.Count; i++)
			{
				mChatArray.Dequeue();
			}
		}
		mChatArray.Enqueue(data);
	}

	public void DeleteChatCell()
	{
		for(int i=0; i<mChatCellStructList.Count; i++)
		{
			ChatCellStruct _ChatCellStruct = mChatCellStructList.Dequeue();
			_ChatCellStruct._UIMessageChatCell.dealloc();
			mChatCellStructList.Enqueue(_ChatCellStruct);
		}
	}

	public void UnRegister()
	{
		mChatArray.Clear();
		mChatCellStructList.Clear();
		mUIMessageTag = null;
		mUIMessageMain = null;
		mUIMessageMail = null;
	}
}

public class ChatCellStruct
{
	public UIMessageChatCell _UIMessageChatCell;
	public SockWorldChatData _SockWorldChatData;
	public ChatCellType _ChatCellType;
	public int _IndexLabel;

	public ChatCellStruct(UIMessageChatCell _cell, SockWorldChatData _data, ChatCellType _celltype, int _index)
	{
		_UIMessageChatCell = _cell;
		_SockWorldChatData = _data;
		_ChatCellType = _celltype;
		_IndexLabel = _index;
	}

	public void SetCellScript(UIMessageChatCell _cell)
	{
		_UIMessageChatCell = _cell;
	}
}

public enum ChatCellType
{
	ChatCellType_NONE = 0,
	ChatCellType_NORMAL = 1,
	ChatCellType_VIDEO = 2,
}

public enum MessageAnalysisType
{
    Message_Friend,
    Message_Rank,
    Message_GrabGold,
    Message_GrabBall,
    Message_Boss,
    Message_KuaFu,
    Message_Share,
    Message_Team,
    Message_Alliance,
    Message_System,
    Message_Other,
}

public enum SystemInformationTypes
{
    InformationTypes_NONE,
    InformationTypes_Lose,
    InformationTypes_Win,
    InformationTypes_Gift,
    InformationTypes_Delete,
}
