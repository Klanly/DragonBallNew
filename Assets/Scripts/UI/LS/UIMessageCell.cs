using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class UIMessageCell : MonoBehaviour {

    private SystemInformationTypes mTypes;
	
	public GameObject FightOrMsg;
    public UISprite mIcon;
    public UILabel mTime;
    public UILabel mName;
    public UILabel mLevel;
//    public UISprite mRankIcon;
//    public UILabel mRank;
	
//    public UISprite mStarIcon;
//    public UILabel mMoney;
//    public UISprite mBallIcon;
//    public UILabel mBall;
    public UILabel mDescript;
	public UISprite mhead;
	public GameObject rank;
	public GameObject lostStone;
	public List<GameObject> list_object = new List<GameObject>();
	public UIGrid uigrid;
	
	public enum CellType
	{
		Fight,
		Msg,
	}
	
	CellType cellType{get;set;}




    void OnClick()
    {
		if(cellType == CellType.Msg)
		{
			//查看邮件
			MegMailCellData celldata = MailReveicer.Instance.GetMegCellData(gameObject.name);
			UIMessageMail.OpenUI(celldata);
		}
		else if(cellType == CellType.Fight)
		{
			FightMegCellData celldata = MailReveicer.Instance.GetFightMegCellData(gameObject.name);
			if(celldata != null)
			{
				if(Core.Data.playerManager.revengeData == null)
				{
					//如果本地没有复仇进度数据缓存
					FinalTrialMgr.GetInstance().OnRevengeProgress=(RevengeProgressData rpdata)=>
					{
						//网络回调
						if(rpdata != null)
						{
							Core.Data.playerManager.revengeData = rpdata;
							UIMessageTag.OpenUI(celldata, !System.Convert.ToBoolean(celldata.islost) , rpdata);
						}
						else
							SQYAlertViewMove.CreateAlertViewMove("Server Data Error! revengeData=null");
					};			   
					FinalTrialMgr.GetInstance().GetRevengeProgress(celldata.id); 
				}
				else
				{
					UIMessageTag.OpenUI(celldata, !System.Convert.ToBoolean(celldata.islost) , Core.Data.playerManager.revengeData);
				}
			}
		}
	}
	
	//设置战报显示
	public void SetCellData(FightMegCellData data)
	{
		rank.gameObject.SetActive(false);
		lostStone.gameObject.SetActive(false);

		cellType = CellType.Fight;
		if(!FightOrMsg.activeSelf)FightOrMsg.SetActive(true);
		
		gameObject.name = data.id.ToString();
		
		mhead.color = new Color(1f,1f,1f,1f);
		
		if(data.cName != null)
		mName.text = Core.Data.stringManager.getString(25042)+data.cName.ToString();

	 	long timecha =  Core.TimerEng.curTime - (long)data.ctm;
		
		long showtime = timecha / 3600;
		
		if(showtime <= 0)
			mTime.text =Core.Data.stringManager.getString(9041);
		else if( showtime /24 == 0)
			 mTime.text =Core.Data.stringManager.getString(25041).Replace("{0}" , showtime.ToString());
		else
			mTime.text =Core.Data.stringManager.getString(25095).Replace("{0}" , ((int)(showtime/24f)).ToString());

		//	public int type;   //类型 1：抢夺龙珠，2：排行榜，3抢夺金币    
		for(int i=0;i<list_object.Count;i++)
		{
			if(i==data.type-1)
			{
				list_object[i].SetActive(true);
				UILabel lab = list_object[i].GetComponentInChildren<UILabel>();
				if(lab!=null) 
				{
					if(data.type ==1)
					{
						SoulData soul = Core.Data.soulManager.GetSoulConfigByNum(data.lost);
						if(soul != null )
						   lab.text = "-"+Core.Data.soulManager.GetSoulConfigByNum(data.lost).name;
						else
						   list_object[i].SetActive(false);
					}
					else
					{
						if(data.lost > 0)
							lab.text = "-"+Mathf.Abs( data.lost ).ToString();
						else if(data.lost < 0)
							lab.text = "+"+Mathf.Abs( data.lost ).ToString();
						else
					        lab.text = data.lost.ToString();
					}
				}
			}
			else
				list_object[i].SetActive(false);
		}
		
	
		if(data.type == 2)
		{
			rank.gameObject.SetActive(true);
			UILabel lab1 = rank.GetComponentInChildren<UILabel>();
			if(lab1!=null) 
			{
				if(data.rank > 0)
				    lab1.text = "-"+data.rank.ToString();	
				else
					lab1.text = data.rank.ToString();
			}
		}
		else
			rank.gameObject.SetActive(false);
	
		/*
			{"ID":9043,"txt":"{}趁你不备，将你从天下第一排行榜打下了X位，自己上去了！"}
			{"ID":9044,"txt":"{}趁你不备，想将你从天下第一排行榜上打下来，可没想到让你虐了一顿。"}
			{"ID":9045,"txt":"{}大摇大摆的来到你家打劫，圈儿踢了你，拿走了你X金币。"}
			{"ID":9046,"txt":"{}大摇大摆的来到你家打劫，没想到被你暴打一顿，丢下了X金币保命逃跑了。"}
			{"ID":9047,"txt":"{}冲进了你家，海扁了你一顿后，拿了你的X哼着小曲走了。"}
			{"ID":9048,"txt":"{}冲进了你家想抢夺龙珠，可被你胖揍了一顿，丢了出去。"}
			{"ID":9075,"txt":"{}踢开你的家门想对你打劫，但是你没有现钱，他神马也没抢到。"}
			{"ID":9076,"txt":"{}冲进了你家想抢夺龙珠，幸亏你藏的好，他神马也没抢到。"}
		 * */
		//显示战报内容
		/*抢夺龙珠*/
		if(data.type ==1)
		{			
			if(data.islost == 0)
			{	
				 //win
				 //别人打我却被我打了  <笑>
				 mhead.spriteName = "common-1009";
				 mDescript.text = Core.Data.stringManager.getString(9048).Replace("{}",data.cName);	 
			}
			else
			{
				 //lose
				 if(data.lost == 0)
				{
					mhead.spriteName = "common-1014";
					 //别人打败了我，但我没有损失龙珠  <贱>
					mDescript.text = Core.Data.stringManager.getString(9076).Replace("{}",data.cName);
				}
				else if(data.lost > 0)
				{				    
					//别人打败了我，我损失了lost龙珠  <哭>
					mhead.spriteName = "common-1010";
					SoulData soul = Core.Data.soulManager.GetSoulConfigByNum(data.lost);
					string content = Core.Data.stringManager.getString(9047);
					if(soul != null ) content = content.Replace("X",soul.name);			
			         mDescript.text = content.Replace("{}",data.cName);
				}
				else
				{
					//lost <0 服务器BUG
					Debug.LogError("<QiangDuo DragonBall> Server Data Error");
				}
			}
		}
		else if(data.type ==2)
		{
			//排行榜   <笑>
			if(data.islost == 0)
			{
				//win
				mhead.spriteName = "common-1009";
				mDescript.text = Core.Data.stringManager.getString(9044).Replace("{}",data.cName);
			}
			else
			{
				//lose    <哭>
				mhead.spriteName = "common-1010";
				string content = Core.Data.stringManager.getString(9043);
				content = content.Replace("X", Mathf.Abs( data.rank) .ToString());		
				mDescript.text = content.Replace("{}",data.cName);
			}
		}
		/*抢夺金币*/
		else if(data.type ==3)
		{
			//如果大于> 0说明丢失钻石 
			if(data.lostStone >0)
			   lostStone.gameObject.SetActive(true);
			else 
				lostStone.gameObject.SetActive(false);

			UILabel Lab_lostStone = lostStone.GetComponentInChildren<UILabel>();
			if(Lab_lostStone!=null) 
			{
				if(data.lostStone > 0)
					Lab_lostStone.text = "-"+data.lostStone.ToString();	
				else
					Lab_lostStone.text = data.lostStone.ToString();
			}


			if(data.islost == 0)
			{
				//win
				//别人打我却被我打了,别人损失lost钱  ,我增加lost钱   <笑>
				mhead.spriteName = "common-1009";
				string content = Core.Data.stringManager.getString(9046);
				content = content.Replace("X", Mathf.Abs(data.lost).ToString());		
				mDescript.text = content.Replace("{}",data.cName);
			}
			else		
			{
				//lose
				if(data.lost == 0)
				{
					//别人打败了我，但是没有抢到我的钱 ,我损失0钱  <贱> 
					mhead.spriteName = "common-1014";
				    mDescript.text = Core.Data.stringManager.getString(9075).Replace("{}",data.cName);
				}
				else if(data.lost > 0)
				{
					mhead.spriteName = "common-1010";
					//别人打败了我，我损失 lost 钱  <哭> 
					 string content = Core.Data.stringManager.getString(9045);
				     content = content.Replace("X", Mathf.Abs(data.lost).ToString());
					 content = content.Replace("Y", Mathf.Abs(data.lostStone).ToString());		
				     mDescript.text = content.Replace("{}",data.cName);
				}
				else
				{
					//别人打赢了我，但是别人损失钱了，服务器BUG
					SQYAlertViewMove.CreateAlertViewMove("<QiangDuo Coin> Server Data Error");
					//Debug.LogError("<QiangDuo Coin> Server Data Error");
				}
			}
		}
		
		mhead.MakePixelPerfect();
		
		mLevel.text = "Lv"+data.cLevel.ToString();
		
		
		uigrid.repositionNow = true;
	}
	
	
	
	//设置显示邮件
	public void SetCellData(MegMailCellData data)
	{
		mhead.atlas = AtlasMgr.mInstance.commonAtlas;
			
		cellType = CellType.Msg;
		if(FightOrMsg.activeSelf)FightOrMsg.SetActive(false);
		
		gameObject.name = data.id.ToString();

		if(data.cName != null)
		mName.text = Core.Data.stringManager.getString(25042)+data.cName.ToString();

	 	long timecha =  Core.TimerEng.curTime - (long)data.ctm;
		long showtime = timecha / 3600;

		if(showtime <= 0)
			mTime.text =Core.Data.stringManager.getString(9041);
		else if( showtime /24 == 0)
			 mTime.text =Core.Data.stringManager.getString(25041).Replace("{0}" , showtime.ToString());
		else
			mTime.text =Core.Data.stringManager.getString(25095).Replace("{0}" , ((int)(showtime/24f)).ToString());
		
		
		mDescript.text =   data.content.Length>20  ? data.content.Substring(0,20)+"..." : data.content;
		   // 1:系统消息，2:玩家留言
		if(data.type == 1)
		{
			if(data.attachment!=null && data.attachment.Length>0)
			{
				mhead.spriteName = "common-1011";
				//有附件
				if(data.status == 0)
					mhead.color = new Color(1f,1f,1f,1f);
				   // mhead.spriteName = "common-1011";
				else if(data.status == 2)
					//mhead.spriteName = "common-1014";
					mhead.color = new Color(0,0,0,1f);
			}
			else
			{
				mhead.spriteName = "common-1013";
				//没有附件
				 if(data.status == 0)   //未读
				     //mhead.spriteName = "common-1013";
					mhead.color = new Color(1f,1f,1f,1f);
				 else if(data.status == 1|| data.status == 2)   //已读
					// mhead.spriteName = "common-1014";
					mhead.color = new Color(0,0,0,1f);
			}
			
			
		}
		else if(data.type == 2)
		{
			mhead.spriteName = "common-1012";
			if(data.status == 0)   //未读
				mhead.color = new Color(1f,1f,1f,1f);
		       // mhead.spriteName = "common-1012";
			else if(data.status == 1)   //已读
				mhead.color = new Color(0,0,0,1f);
				//mhead.spriteName = "common-10121";
		}
		
		mLevel.text = "Lv"+data.cLevel.ToString();
		
		mhead.MakePixelPerfect();
		
		
	}
	
}
