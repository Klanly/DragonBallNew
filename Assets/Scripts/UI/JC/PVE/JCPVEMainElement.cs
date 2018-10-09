using UnityEngine;
using System.Collections;

public class JCPVEMainElement : MonoBehaviour {

	public UISprite Spr_WaiKuang;
	public UISprite Spr_Back1;
	public UISprite Spr_Back2;
	public UISprite Spr_Back3;
	public UISprite ChapterSprite;
    public GameObject UnLock;
	public GameObject Lock;
	public BoxCollider _Collider;
	public GameObject Remaining;
	//倒计时模块
	public GameObject DJS;
	public UILabel Lab_Time;
	
	private bool _isCooling;
	
	public UILabel Lab_Remaining;

	public UILabel lab_OpenFloor;				//解锁关卡

	public UILabel Lab_NeedStone;

	public bool isLock
	{
		set
		{
			Color color = value ?  new Color(0,0,0,1f) : new Color(1f,1,1f,1f);
			Spr_WaiKuang.color = color;
			Spr_Back1.color = color;
			Spr_Back2.color = color;
			Spr_Back3.color = color;
			ChapterSprite.color = color;
			UnLock.SetActive(!value);
			Lock.SetActive(value);
			_Collider.enabled = !value;
		}
	}

	//设置解锁文本内容
	public void SetLockTxt(string strText)
	{
		lab_OpenFloor.text = strText;
	}


	//设置时间
	public void SetTime(long time)
	{
		if(this!= null && gameObject.activeSelf)
		{
			isCooling = time != 0;
			Lab_Time.text =  DateHelper.getDateFormatFromLong(  time );
		}
	}
	
	
	
	public bool isCooling
	{
		set
		{
			_isCooling = value;
			DJS.SetActive(_isCooling);
			Remaining.SetActive(!_isCooling);
		}
		get
		{
			return _isCooling;
		}
	} 
	
	public void SetRemainingCount(int count)
	{
		isCooling = false;
	}
	
	//设置进度值
	public void SetProgress(int SkillPassCount,int SkillCount,int NeedStone)
	{
		if(Lab_Remaining != null)
		{
			if(SkillPassCount < SkillCount)
			{
				if(Lab_NeedStone.gameObject.activeSelf)Lab_NeedStone.gameObject.SetActive(false);
				if(!Lab_Remaining.gameObject.activeSelf) Lab_Remaining.gameObject.SetActive(true);
				Lab_Remaining.text = SkillPassCount.ToString()+"/"+SkillCount.ToString();
			}
			else
			{


				if(NeedStone > 0)
				{
					if(!Lab_NeedStone.gameObject.activeSelf)Lab_NeedStone.gameObject.SetActive(true);
					if(Lab_Remaining.gameObject.activeSelf) Lab_Remaining.gameObject.SetActive(false);
					Lab_NeedStone.text = NeedStone.ToString();
				}
				else
				{
					if(Lab_NeedStone.gameObject.activeSelf)Lab_NeedStone.gameObject.SetActive(false);
					if(!Lab_Remaining.gameObject.activeSelf) Lab_Remaining.gameObject.SetActive(true);
//					if(Core.Data.playerManager.curVipLv < Core.Data.vipManager.MaxVipLevel)
//					SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(9119));
					Lab_Remaining.text = Core.Data.stringManager.getString(9121);
				}
			}
		}
	}

	public void SetProgress2(string str)
	{
		if(Lab_Remaining != null)
		{
			Lab_Remaining.text = str;
		}
	}
	
}
