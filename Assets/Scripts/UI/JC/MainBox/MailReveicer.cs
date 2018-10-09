using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

public enum MailState
{
	None = 0,
	newFight = 1,
	newMsg = 2,
	AllNew = newFight|newMsg,
}



public class MailReveicer : MonoBehaviour {

	
	public List<FightMegCellData> list_fight = new List<FightMegCellData>();
	public List<MegMailCellData> list_message = new List<MegMailCellData>();
	
	public System.Action<List<FightMegCellData>,bool> FightMsgBack = null; 
		
	public MailState mailState {get;set;}
	
	public static MailReveicer Instance;
	
	void Awake()
	{
		Instance = this;
	}
	
	
	public static void Create()
	{
		GameObject MailReveicerObj = new GameObject();
		MailReveicerObj.name = "MailReveicer";
		MailReveicerObj.transform.parent = GameObject.Find("Gobal").transform;
		MailReveicerObj.AddComponent<MailReveicer>();
	}
	
	
	void Start () 
	{
	     InvokeRepeating("GetMail",1f,120f);
	}
	
	void GetMail ()
	{
		if(!Core.Data.guideManger.isGuiding)
		{
			 SendFightMegRequest();
		     SendMegRequest();	
		}
	}
	
//	void OnGUI()
//	{
//		if(GUI.Button(new Rect(0,300,100,50),"GET MAIL MESSAGE"))
//		{
//			GetMail ();
//		}
//	}
	
	
	
    //发送战报请求
	public void SendFightMegRequest(int sync = 0)
    {
        HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
        task.AppendCommonParam(RequestType.MESSAGE_INFORMATION, new PlayerIDParam(Core.Data.playerManager.PlayerID, sync));       
        task.afterCompleted += FightMegCompleted;  
		task.ErrorOccured =(BaseHttpRequest request, string error) =>{ Debug.Log("SendFightRequest is error!");   ComLoading.Close ();};
        task.DispatchToRealHandler();
    }
     
	public void FightMegCompleted(BaseHttpRequest request, BaseResponse response)
	{
		ComLoading.Close ();
		if (response.status != BaseResponse.ERROR) 
		{
		    MessageInformationResponse resp = response as MessageInformationResponse;
			FightMegCellData[] list_msg = resp.data.msg;

			#region 降序排列
//			IEnumerable<FightMegCellData> query = null;
//			query = from items in list_msg orderby items.ctm descending select items;
			list_msg = SortFight(list_msg,false);
			#endregion

	        list_fight.Clear();
			//list_fight.AddRange(query.ToArray());	
		    list_fight.AddRange(list_msg);
			HaveNewFight();
            CheckMail();

			#region added by zhangqiang to update mainui when sync all data
			HttpRequest req = request as HttpRequest;
			PlayerIDParam param = req.ParamMem as PlayerIDParam;

			if(param.sync == 100 && SQYMainController.mInstance != null)
			{
				SQYMainController.mInstance.UpdateUI();
				if(BuildScene.mInstance != null)
				{
					BuildScene.mInstance.UpdateAllBuilds();
				}
				if(UISourceBuilding.mInstance!= null){
					Building buildData = Core.Data.BuildingManager.GetBuildFromBagByNum(830001);
					UISourceBuilding.mInstance.SetData(buildData);

				}
			}
			#endregion
		}
	}
    //检测龙珠是否被抢
    void CheckMail( ){
        if (list_fight != null)
        {
            foreach (FightMegCellData mail in list_fight)
            {
                if (mail.type == 1)
                {
                    if (mail.islost == 1)
                    {
						Core.Data.dragonManager.SyncCallDragonTime ();
						break;
                    }
                }
            }
        }

    }

	void CheckRob()
	{
		if (list_fight != null)
		{
			foreach (FightMegCellData mail in list_fight)
			{
				if (mail.type == 3)
				{
					Core.Data.playerManager.RTData.curCoin -= mail.lost;
					break;
				}
			}
		}
	}

	public bool isHaveNewMail
	{
		get
		{
			foreach(MegMailCellData data in list_message)
			{
				if(data.status == 0)
					return true;
			}
			return false;
		}
	}
	
	public bool isHaveNewFight
	{
		get
		{
			foreach(FightMegCellData data in list_fight)
			{
				if(data.status == 0)
				{
					return true;
				}
			}
			return false;
		}
	}
	
	
	//有了新的战报
	void HaveNewFight()
	{
		//判断是否有新的战报
		if(isHaveNewFight)
		{
			if(mailState == MailState.None)
				mailState = MailState.newFight;
			else if(mailState == MailState.newMsg)
				mailState = MailState.AllNew;
		}
		else
		{
			if(mailState == MailState.newFight)
			    mailState = MailState.None;
		}
	}
	
	//发送信息请求
	public void SendMegRequest()
    {
        HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
        task.AppendCommonParam(RequestType.MESSAGE_EMAIL, new PlayerIDParam(Core.Data.playerManager.PlayerID));       
        task.afterCompleted += MegCompleted;  
		task.ErrorOccured =(BaseHttpRequest request, string error) =>{ Debug.Log("SendMegRequest is error!  ["  + error+"]");  };
        task.DispatchToRealHandler();
    }
	
	public void MegCompleted(BaseHttpRequest request, BaseResponse response)
	{
		if (response.status != BaseResponse.ERROR) 
		{
		    MegMailResponse resp = response as MegMailResponse;
			MegMailCellData[] list_msg = resp.data.msg;
			
            #region 降序排列
//			IEnumerable<MegMailCellData> query = null;
//			query = from items in list_msg orderby items.ctm descending select items;
			list_msg = SortMail(list_msg,false);
			#endregion
			
			list_message.Clear();
			//list_message.AddRange(query.ToArray());				
			list_message.AddRange(list_msg);
			HaveNewMsg();
		}
	}
	
	void DebugListMsg()
	{
		Debug.Log("-------------------------------");
		foreach(FightMegCellData m in list_fight)
		{
			Debug.Log(m.ctm.ToString()+"    "+m.id.ToString());
		}
		Debug.Log("-------------------------------");
	}
	

    //有了新的信息
	void HaveNewMsg()
	{
		//判断是否有新的信息
		if(isHaveNewMail)
		{
			if(mailState == MailState.None)
				mailState = MailState.newMsg;
			else if(mailState == MailState.newFight)
				mailState = MailState.AllNew;
		}
		else
		{
			if(mailState == MailState.newMsg)
			    mailState = MailState.None;
		}
	}
	
	
	//获取fightdata
	public FightMegCellData GetFightMegCellData(string id)
	{
		return GetFightMegCellData((int)System.Convert.ToInt32(id));
	}
	public FightMegCellData GetFightMegCellData(int id)
	{
		FightMegCellData data = null;
		foreach(FightMegCellData cell in list_fight)
		{
			if(cell.id == id )
			{
				data = cell;
				return data;
			}
		}
		return data;
	}
	
	//获取messagedata
	public MegMailCellData GetMegCellData(string id)
	{
		return GetMegCellData((int)System.Convert.ToInt32(id));
	}
	public MegMailCellData GetMegCellData(int id)
	{
		MegMailCellData data = null;
		foreach(MegMailCellData cell in list_message)
		{
			if(cell.id == id )
			{
				data = cell;
				return data;
			}
		}
		return data;
	}
	
	public void SetMailState(int id,int operation)
	{
		MegMailCellData tempcell = null;
		foreach(MegMailCellData cell in list_message)
		{
			if(cell.id == id )
			{
				tempcell = cell;
				break;
			}
		}
		
		if(operation == 1)
		{
			if(tempcell!=null)
			tempcell.status = 1;		
		}
		else if(operation == 2)
		{
			if(tempcell!=null)
			list_message.Remove(tempcell);
		}
		HaveNewMsg();
	}
	
	//提取附件
	public void GetMailAttachment(int id)
	{
		MegMailCellData tempcell = null;
		foreach(MegMailCellData cell in list_message)
		{
			if(cell.id == id )
			{
				tempcell = cell;
				break;
			}
		}
		Debug.Log(" get mail   attach  ");
		if(tempcell != null)
		{

			Debug.Log(" change state ");
			tempcell.status = 2;
		}
		else
		{
			SQYAlertViewMove.CreateAlertViewMove("find not local mail ["+id.ToString()+"]");
		}
	    HaveNewMsg();
	}
	
	
	//强制刷新邮件状态
	public void RefreshMailState()
	{
		HaveNewFight();
		HaveNewMsg();
	}
	
	  //AscOrDes:true 升序  false 降序  
	  public FightMegCellData[] SortFight(FightMegCellData[] arr,bool AscOrDes = true)        
	  {   
	      for (int i = 1; i < arr.Length; i++)         
	      {               
	            FightMegCellData t = arr[i];           
	            int j = i;       
			    if(AscOrDes)
				{
		            while ((j > 0) && (arr[j - 1].ctm > t.ctm))                                    
		            {                    
		               arr[j] = arr[j - 1];           
		               --j;          
		           }     
				}
			    else
			    {
			      	while ((j > 0) && (arr[j - 1].ctm < t.ctm))                                    
		            {                    
		               arr[j] = arr[j - 1];           
		               --j;          
		           }     
			    }
	           arr[j] = t;         
	       }
		   return arr;
	   }
	  //AscOrDes:true 升序  false 降序  
	  public MegMailCellData[] SortMail(MegMailCellData[] arr,bool AscOrDes = true)        
	  {   
	      for (int i = 1; i < arr.Length; i++)         
	      {               
	            MegMailCellData t = arr[i];           
	            int j = i;       
			    if(AscOrDes)
				{
		            while ((j > 0) && (arr[j - 1].ctm > t.ctm))                                    
		            {                    
		               arr[j] = arr[j - 1];           
		               --j;          
		           }     
				}
			    else
			    {
			      	while ((j > 0) && (arr[j - 1].ctm < t.ctm))                                    
		            {                    
		               arr[j] = arr[j - 1];           
		               --j;          
		           }     
			    }
	           arr[j] = t;         
	       }
		   return arr;
	   }

	//销毁邮件监听器
	public void DeleteMailReveicer()
	{
		Destroy(gameObject);
	}


}
