using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MailBox_View : MonoBehaviour {

	public UISprite newFight;
	public UISprite newMessage;
	public MailBox _logic;
	[HideInInspector]
	public List<UIMessageCell>  ListCells = new List<UIMessageCell>();

	public UIScrollView _scrollView;
	
	public UISprite Btn_Fight_back;
	public UISprite Btn_Msg_back;

    public string nomal    = "Symbol 32";
    public string selected = "Symbol 31" ; 


	public UIPanel _uipanel;
	void Start () {
	
	}
	
	
	

	public void  SetNewSgin(MailState state)
	{
		newFight.enabled = false;
		newMessage.enabled = false;
	    if(state == MailState.newFight)
			newFight.enabled = true;
		else if(state == MailState.newMsg)
		    newMessage.enabled = true;
		else if(state == MailState.AllNew)
		{
			newFight.enabled = true;
		    newMessage.enabled = true;
		}
	}
	
	//显示战报
	public void ShowFight()
	{
        if(Btn_Fight_back.spriteName == nomal)  Btn_Fight_back.spriteName =selected;
		else  return;
        if(Btn_Msg_back.spriteName == selected) Btn_Msg_back.spriteName =nomal ;
			
		RefreshFight();
	}
	
    //显示信息
	public void ShowMsg()
	{
	#region 按钮高亮	
        if(Btn_Msg_back.spriteName == nomal )  
			Btn_Msg_back.spriteName = selected;
//		else 
//			return;
        if(Btn_Fight_back.spriteName == selected)
			Btn_Fight_back.spriteName =  nomal;
	#endregion
		RefreshMsg();
	}
	
	
	//刷新信息列表
	public void RefreshMsg()
	{
		_uipanel.alpha = 0;
		List<MegMailCellData> list_message = MailReveicer.Instance.list_message;

     #region 保证对象池够显示
		int cha =list_message.Count - ListCells.Count;
		if(cha>0) _logic.CreateCellObject(cha);
     #endregion

		int i = 0;
		for(;i<list_message.Count;i++)
		{
			ListCells[i].gameObject.SetActive(true);
			//设置单个元素的显示
			ListCells[i].SetCellData(list_message[i]);
		}
	#region 隐藏对象池不用的子	
		for(;i<ListCells.Count;i++)
		{
			ListCells[i].gameObject.SetActive(false);
		}
	#endregion
		_logic.uiguide.repositionNow = true;


	#region 刷新邮件状态 
		if( MailReveicer.Instance.isHaveNewMail )
		{
			if(MailReveicer.Instance.mailState == MailState.newFight)
				MailReveicer.Instance.mailState = MailState.AllNew;
		}
		else
		{
			if(MailReveicer.Instance.mailState == MailState.AllNew)
				MailReveicer.Instance.mailState = MailState.newFight;
			else if (MailReveicer.Instance.mailState == MailState.newMsg)
				MailReveicer.Instance.mailState = MailState.None;
		}
		MailBox._mInstance._view.SetNewSgin(MailReveicer.Instance.mailState);
	#endregion



		StartCoroutine(PanelRepostion());
	}
	
	//刷新战报列表
	public void RefreshFight()
	{
		_uipanel.alpha = 0;
		List<FightMegCellData> list_fight = MailReveicer.Instance.list_fight;
		int i = 0;
		
		int cha =list_fight.Count - ListCells.Count;
		if(cha>0) _logic.CreateCellObject(cha);
		
		for(;i<list_fight.Count;i++)
		{
			ListCells[i].gameObject.SetActive(true);
			//设置单个元素的显示
			ListCells[i].SetCellData(list_fight[i]);
		}
		
		for(;i<ListCells.Count;i++)
		{
			ListCells[i].gameObject.SetActive(false);
		}
		
		_logic.uiguide.repositionNow = true;
		StartCoroutine(PanelRepostion());
	}
	
	
	IEnumerator PanelRepostion()
	{
		yield return new WaitForSeconds(0.1f);
		_scrollView.verticalScrollBar.value = 0.002f;
		_uipanel.alpha = 1f;
	}
	
}
