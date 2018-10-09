using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIMessageChatCell : RUIMonoBehaviour
{

	public GameObject mVideoType;
	public GameObject mNormalType;
	public UILabel mName;
	public UILabel mLevel;
	public UILabel mVideoName;
	public ChatLabel mVideoDis;
	public ChatLabel mNormalDis;
	public UISprite mHead;
	public UISprite BgBig;
	public UISprite BgSmall;
	public UILabel mTime;

	public string _videoid;

	ChatCellType m_type;

	long gid;
	


	void Start()
	{
		UIButtonMessage btnMsg = GetComponent<UIButtonMessage>();
		btnMsg.target = UIMessageMain.Instance.gameObject;
		btnMsg.functionName = "HideEmotionAndQuickUI";
	}
	
	public void OnShow(SockWorldChatData data, ChatCellType _type)
	{
		m_type = _type;
		gid = data.roleid;
		if(_type == ChatCellType.ChatCellType_NORMAL)
		{
			mNormalType.gameObject.SetActive(true);
			mVideoType.gameObject.SetActive(false);
			int m_hight = mNormalDis.height;
			
			mName.text = data.roleName;
			mNormalDis.SetText(data.content);
			mLevel.text = data.rolelv.ToString();
			
			BgBig.height = BgBig.height + mNormalDis.height - m_hight;
			BgSmall.height = BgSmall.height + mNormalDis.height - m_hight;
			
			BoxCollider box1 = this.gameObject.GetComponent<BoxCollider>();
			box1.size = new Vector3(BgBig.width, BgBig.height, 0f);
			box1.center = new Vector3(0, -(mNormalDis.height - m_hight)/2, 0f);
		}
		else
		{
			mNormalType.gameObject.SetActive(false);
			mVideoType.gameObject.SetActive(true);
//			mVideoDis.gameObject.SetActive(false);

			int m_hight = mVideoDis.height;
			
			mName.text = data.roleName;
			mVideoDis.SetText(data.content);
			mLevel.text = data.rolelv.ToString();
			
			BgBig.height = BgBig.height + mVideoDis.height - m_hight;
			BgSmall.height = BgSmall.height + mVideoDis.height - m_hight;
			
			BoxCollider box1 = this.gameObject.GetComponent<BoxCollider>();
			box1.size = new Vector3(BgBig.width, BgBig.height, 0f);
			box1.center = new Vector3(0, -(mVideoDis.height - m_hight)/2, 0f);

			AnalysisString(data.content);
        }



		if(data.iconid == 0)AtlasMgr.mInstance.SetHeadSprite(mHead, "10104");
		else
		{
			AtlasMgr.mInstance.SetHeadSprite(mHead, data.iconid.ToString ());
		}


		DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
		long lTime = data.time * 10000;
//		TimeSpan toNow = new TimeSpan(lTime+dtStart.Ticks);
		DateTime dt = new DateTime(lTime+dtStart.Ticks);

//		long time_JAVA_Long = data.time;//java长整型日期，毫秒为单位
//		DateTime dt_1970 = new DateTime(1970,1,1);
//		long tricks_1970 = dt_1970.Ticks;//1970年1月1日刻度
//		long time_tricks = tricks_1970 + time_JAVA_Long*10000;//日志日期刻度
//		DateTime dt = new DateTime(time_tricks);//转化为DateTime

		mTime.text = dt.Year + "/" + dt.Month + "/" + dt.Day + " " + dt.Hour + ":" + dt.Minute + ":" + dt.Second;
	}

	void AnalysisString(string _str)
	{
		string[] str = _str.Split(new char[]{'#','$','&'});
		if(str.Length < 3)
		{
			ConsoleEx.DebugLog(str.Length.ToString() + "is not enough long");
			return;
		}
		List<string> strdata = new List<string>();
		for(int k=0; k<str.Length; k++)
		{
			if(!string.IsNullOrEmpty(str[k]))
			{
				if(str[k].Contains("{{@}}"))
				{
					string[] str_temp = str[k].Split(new char[]{'{','{','@','}','}'});
					for(int j=0; j<str_temp.Length; j++)
					{
						if(!string.IsNullOrEmpty(str_temp[j]))
						{
							strdata.Add(str_temp[j]);
							continue;
						}
					}
					if(strdata.Count != 2)return;
                    _videoid = strdata[0];
                    mVideoName.text = strdata[1];

                }
				else
				{
					mVideoDis.gameObject.SetActive(true);
					mVideoDis.SetText(str[k]);
				}
			} 
            
        }
        


	}


	void LookQueueClick()
	{
		DragonBallRankMgr.GetInstance().FinalTrialRankCheckInfoRequest((int)gid) ;
	}

	void ShareClick()
	{
		if(m_type == ChatCellType.ChatCellType_NORMAL || m_type == ChatCellType.ChatCellType_NONE)return;
		FinalTrialMgr.GetInstance().BattleVideoRequestSingle(_videoid, RUIType.EMViewState.S_XiaoXi);
	}
}
