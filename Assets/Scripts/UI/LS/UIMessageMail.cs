using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UIMessageMail : MonoBehaviour {
	
    public UILabel mNum;
    public UIInput mInput;
	public int sendPlayerID {get;set;}
	
	static UIMessageMail _this;
	
	public GameObject MailButton;
	public GameObject ReplyButton;

	public System.Action OnClose;
	
	public GameObject AttachmentObj;
	public UISprite InputBack;
	
	MegMailCellData mailData {get;set;}
	
	//public List<UIMailAttachment> list_Attachment = new List<UIMailAttachment>();

	public GameObject ShowMailObject;

	public UILabel Lab_ShowMail;


	public List<UIMailNewAttachment> list_Attachments = new List<UIMailNewAttachment>();

	public UILabel Lab_FuJianWord ;

	void Start ()
	{
	     mInput.onChange.Add(new EventDelegate(OnInput));
	}
	
	public static UIMessageMail OpenUI(MegMailCellData data)
	{
		OpenUI(0);
		 _this.InitReadMail(data);		
		return _this;
	}
	//初始化读取邮件功能
	void InitReadMail(MegMailCellData data)
	{
		isShowAttachment = true;
		
		this.mailData = data;

		// 1:系统消息，2:玩家留言
		if(data.type == 1)
		{
			if(data.attachment!=null && data.attachment.Length>0)
			{
				//有附件
				if(data.status == 0)
				{
				     MailButton.name = "GetReward";
				     MailButton.GetComponentInChildren<UILabel>().text = Core.Data.stringManager.getString(9042);
				}
				else if(data.status == 2)
				{
					//已提取
					MailButton.name = "Delete";
				    MailButton.GetComponentInChildren<UILabel>().text = Core.Data.stringManager.getString(6089);
				}
			}
			else
			{
				//没有附件
				MailButton.name = "Delete";
				MailButton.GetComponentInChildren<UILabel>().text = Core.Data.stringManager.getString(6089);
			}
		}
		else if(data.type == 2)
		{
			isShowAttachment = false;	
			//玩家留言
			MailButton.name = "Delete";
			MailButton.GetComponentInChildren<UILabel>().text = Core.Data.stringManager.getString(6089);
			MailButton.transform.localPosition = new Vector3(-125f,-250f,0);
			ReplyButton.gameObject.SetActive(true);
			ReplyButton.transform.localPosition = new Vector3(125f,-250f,0);

			SetMailRead(data.id);
		}

		mNum.gameObject.SetActive(false);
		mInput.GetComponent<BoxCollider>().enabled = false;
		mInput.characterLimit = 0;
		if(mInput.gameObject.activeSelf)
			mInput.value = data.cName+":\n    "+data.content;
		else
			SetLab_ShowMail(data.cName+":\n    "+data.content);
		
		
		
		//显示附件   
		if(data.attachment != null)
		{
			if(data.status == 0)
			{
				for(int i=0;i<data.attachment.Length;i++)
				{
					list_Attachments[i].isReceived = false;
					list_Attachments[i].SetData(data.attachment[i]);
				}
			}
			else if(data.status == 2)
			{
				for(int i=0;i<data.attachment.Length;i++)
				{
					list_Attachments[i].isReceived = true;
					list_Attachments[i].SetData(data.attachment[i]);
				}
			}
		}
		
	}

	//显示书写界面(回复按钮调用)
	void ShowWritePage()
	{
		sendPlayerID = mailData.cgid;
		isShowAttachment = false;
		ReplyButton.SetActive(false);
		MailButton.transform.localPosition = new Vector3(0,-250f,0);
		MailButton.name = "Send";
		MailButton.GetComponentInChildren<UILabel>().text = Core.Data.stringManager.getString(9122);
		mInput.value = "";
	}



	
	//设置该邮件为已读状态
	void SetMailRead(params int [] mail)
	{
		//如果是未读的邮件则标记为已读状态
		if(mailData.status == 0)
        MailOperation(1,mail);
	}
	
	//删除邮件
	void SetMailDelete(params int [] mail)
	{
        MailOperation(2,mail);
	}
	
	//邮件操作   (操作类型 1：设置为已读，2：删除)
	void MailOperation(int type,params int [] mail)
	{
		ComLoading.Open();
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		ChangeMailStateParam param = new ChangeMailStateParam();
		param.gid = Core.Data.playerManager.PlayerID;
		param.ids = mail;
		param.type = type;
		//消息类型	1：战报，2，邮件
		param.msgType = 2;
        task.AppendCommonParam(RequestType.CHANGE_MAIL_STATE, param);       
        task.afterCompleted = (BaseHttpRequest request, BaseResponse response) => 
		{
			ComLoading.Close();
			if (response.status != BaseResponse.ERROR) 
		    {
			   ChangeMailStateResponse res = response as ChangeMailStateResponse;
			  // Debug.Log(res.data);
			   if(res.data)
				{
					//操作成功
					//标记已读
					if(type == 1)
					{
						foreach(int id in mail)
					    MailReveicer.Instance.SetMailState(id,1);
					}
					else if(type == 2)
					{
						//删除邮件
						foreach(int id in mail)
					    MailReveicer.Instance.SetMailState(id,2);						
					}			
					
					
					//刷新邮件
					MailBox._mInstance._view.RefreshMsg();

//					//刷新邮箱的状态
//					MailBox._mInstance._view.SetNewSgin(MailReveicer.Instance.mailState);
					if(type == 2 ) 
						OnBtnClick("Close");
				}
			}

		};
		task.ErrorOccured =(BaseHttpRequest request, string error) =>{ Debug.Log("Mail["+mail[0]+"]  Operation["+type.ToString()+"] is error!["+error+"]");  };
        task.DispatchToRealHandler();
	}
	
	
	public static UIMessageMail OpenUI( int sendPlayerID ,System.Action action = null)
	{
		if(_this == null)
		{
			Object prefab = PrefabLoader.loadFromPack("LS/pbLSInformationMail") ;
	        if(prefab != null)
	        { 
				GameObject obj = Instantiate(prefab) as GameObject;
				 RED.AddChild(obj, DBUIController.mDBUIInstance._TopRoot);
	             _this = obj.gameObject.GetComponent<UIMessageMail>();
				_this.OnClose = action;
				_this.sendPlayerID = sendPlayerID;
				_this.isShowAttachment = false;
	        }
		}
		else
		{
			_this.gameObject.SetActive(true);
		}
		return _this;
	}

    public void OnShow()
    {

    }

    void OnBtnClick(GameObject btn)
    {

        OnBtnClick(btn.name);
    }
	
	public void OnBtnClick(string btnName)
    {
		switch(btnName)
		{
		case "Close":
			if(this != null && OnClose!=null)
				OnClose();			
			if(this != null && gameObject != null)
				Destroy(gameObject);
			break;
		case "Send":
			SendEmail();
			break;
		case "Delete":
			SetMailDelete(mailData.id);
			break;
		case "GetReward":
			GetReward (mailData.id);
			MailButton.GetComponent<UIButton> ().isEnabled = false;
			break;
		case "Reply":
			ShowWritePage();
			break;
		}
	}
	
	
    void OnInput()
    {
		mNum.text = mInput.value.Length+"/80";
    }
	
	
	//发送邮件
	public void SendEmail()
    {
		if(mInput.value.Length == 0)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(9069));
			return;
		}

		bool check = SensitiveFilterManager.getInstance().check(mInput.value);
		if(check) {
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(75003));
			return;
		}
		
		ComLoading.Open();
        HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
        task.AppendCommonParam(RequestType.SEND_EMAIL, new SendEmailParam(Core.Data.playerManager.PlayerID,this.sendPlayerID,mInput.value ));       
        task.afterCompleted =(BaseHttpRequest request, BaseResponse response)=>
		{	
			ComLoading.Close();
			if(response != null && response.status  != BaseResponse.ERROR){

			
				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(9059));
				OnBtnClick("Close");
			}else if(response.status  == BaseResponse.ERROR){
				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getNetworkErrorString(response.errorCode));
				OnBtnClick("Close");
			}
		};  
		task.ErrorOccured =(BaseHttpRequest request, string error) =>
		{
			ComLoading.Close();
			Debug.Log("SendMegRequest is error!");  
		};
        task.DispatchToRealHandler();
    }
	
	
	//提取附件
	public void GetReward(int id)
	{

		ComLoading.Open();
        HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
        task.AppendCommonParam(RequestType.GET_MAIL_ATTACHMENT, new GetMailAttchment(Core.Data.playerManager.PlayerID,id) );       
        task.afterCompleted =(BaseHttpRequest request, BaseResponse response)=>
		{		

			if(response.status != BaseResponse.ERROR)
			{
				ComLoading.Close();
				GetMailAttachmentResponse res = response as GetMailAttachmentResponse;
				if(res.data == null || res.data.p == null)
				{
					SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(40007));
					return;
				}



				GetRewardSucUI.OpenUI(res.data.p,Core.Data.stringManager.getString(5047));
                foreach(ItemOfReward re in res.data.p)
                {
                    Core.Data.ActivityManager.setOnReward(re , ActivityManager.MAIL);
                }
				
				//如果是世界邮件(id<0)领完以后直接删了
				if(id < 0)
				{
					//删除邮件


					MailReveicer.Instance.SetMailState(id,2);	
					OnBtnClick("Close");
				}
				else
				{
					//邮件提取数据改变
				    MailReveicer.Instance.GetMailAttachment(id);
					Debug.Log(" get  reward  in mails ");
					//按钮变成删除功能
				    MailButton.name = "Delete";
				    MailButton.GetComponentInChildren<UILabel>().text = Core.Data.stringManager.getString(6089);	
					foreach(UIMailNewAttachment data in list_Attachments)
					{
						if(data.gameObject.activeSelf)
						   data.isReceived = true;
					}
						
				}

				//刷新邮件
				MailBox._mInstance._view.RefreshMsg();
				//刷新邮箱的状态
				MailBox._mInstance._view.SetNewSgin(MailReveicer.Instance.mailState);
				
				AddRewardToBag(res.data.p);
				Debug.Log(" refresh  add to bag  ");
				//UIMiniPlayerController.Instance.freshPlayerInfoView ();
		        DBUIController.mDBUIInstance.RefreshUserInfo ();

			}
			else
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(40007));
			MailButton.GetComponent<UIButton> ().isEnabled = true;
		};  
		task.ErrorOccured =(BaseHttpRequest request, string error) =>
		{
			ComLoading.Close(); 
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(40007));    
			MailButton.GetComponent<UIButton> ().isEnabled = true;
		};   
        task.DispatchToRealHandler();   
	}      
	
	 
	//添加到背包中  
	void AddRewardToBag(ItemOfReward[] p)
	{
		foreach(ItemOfReward item in p)
		{
			AddRewardToBag(item);
		}
	}
	
	void AddRewardToBag(ItemOfReward item)
	{	
		ConfigDataType type = DataCore.getDataType(item.pid);
		switch(type)
		{
			case ConfigDataType.Item:
			{
			    Core.Data.itemManager.addItem(item);
			    break;
			}		
			case ConfigDataType.Monster:
			{		 
			    Monster monster = item.toMonster(Core.Data.monManager);
			    if(monster!=null)
			    Core.Data.monManager.AddMonter(monster);
				break;
			}	
			case ConfigDataType.Equip:
			{
				Core.Data.EquipManager.AddEquip(item);
			    break;
			}			
			case ConfigDataType.Gems:
			{
			    Core.Data.gemsManager.AddGems(item);
			    break;
			}				
			case ConfigDataType.Frag:
			{
			    Soul soul = item.toSoul(Core.Data.soulManager);
			    if(soul != null)
			    Core.Data.soulManager.AddSoul(soul);
				break;
			}		
		}
	}
	
	//是否显示附件
	bool isShowAttachment
	{
		set
		{
			Lab_FuJianWord.enabled = value;
			if(value)
			{
				if(list_Attachments.Count == 1)
				{
					GameObject CreateObj = list_Attachments[0].gameObject;
					Vector3 firstPos = CreateObj.transform.localPosition;
					Transform Parent = CreateObj.transform.parent;
					int index = 0;
					for(int i=0;i<2;i++)
					{
						for(int j=0;j<4;j++)
						{
							if(i == 0 && j == 0) continue;
							GameObject item = Instantiate(CreateObj) as GameObject;
							item.transform.parent = Parent;
							item.transform.localScale = Vector3.one;
							item.gameObject.name = "FuJian"+(++index).ToString();
							if(i < 1)
								item.transform.localPosition = new Vector3(firstPos.x,firstPos.y - 34f * j,0);
				            else
								item.transform.localPosition = new Vector3(firstPos.x + 320f ,firstPos.y - 30f * j,0);
							UIMailNewAttachment itemScript = item.GetComponent<UIMailNewAttachment>();
							list_Attachments.Add(itemScript);
						}
					}
				}
			}
			foreach(UIMailNewAttachment element in list_Attachments)
				element.gameObject.SetActive(false);
			
			AttachmentObj.SetActive(value);
//			InputBack.height = value ? 168:310;
//			if(!mInput.gameObject.activeSelf)mInput.gameObject.SetActive(true);
//			mInput.GetComponentInChildren<UILabel>().fontSize = value ? 20:35;
			ShowMailObject.SetActive(value);
			mInput.gameObject.SetActive(!value);
			BoxCollider collider = mInput.GetComponent<BoxCollider>();
			collider.enabled = !value;
//			if(collider != null)
//			{
//				Vector3 center = collider.center;
//				Vector3 size = collider.size;
//				center.y = value ? -33.3f:-107.35f;
//				size.y = value ? 148.57f:310f;
//				collider.center = center;
//				collider.size = size;
//			}
		}
	}
	
	void SetLab_ShowMail(string Content)
	{
		Lab_ShowMail.text = Content;
		BoxCollider collider = Lab_ShowMail.GetComponent<BoxCollider>();
		if(collider != null)
		{
			Vector3 labSize = new Vector3(Lab_ShowMail.width,Lab_ShowMail.height,0);
			if(labSize.y < 135f) labSize.y = 135;
			collider.center = new Vector3(labSize.x/2f,-labSize.y/2f,0);
			collider.size = labSize;
			UISprite sprite = Lab_ShowMail.GetComponentInChildren<UISprite>();
			if(sprite != null)
			{
				sprite.width =(int) labSize.x;
				sprite.height = (int)labSize.y;
			}  
		}
	}    

}
