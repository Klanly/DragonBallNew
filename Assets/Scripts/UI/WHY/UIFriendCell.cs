using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIFriendCell : MonoBehaviour
{
	#region 基本变量
	public enum FriendCellType
	{
		FriendCell,
		RequestCell,
		SuDiCell,
		SearchCell,
		None
	}

	protected FriendCellType currentFriendCellType = FriendCellType.None;

	public OtherUserInfo otherUserInfo;

	public UIButton addFriendButton;

	#endregion

	#region 用户信息区域
	public UITexture userHead;
	public UILabel userName;
	public UILabel userLevel;
	public UILabel userVIPLevel;
	public UILabel userRank;
	#endregion

	#region 上场角色区域
	public List<RoleIcon> roleIconList = null;

	public GameObject roleHaveButtonRoot;
	public List<RoleIcon> roleHaveButtonIconList = new List<RoleIcon>();

	public GameObject roleNoButtonRoot;
	public List<RoleIcon> roleNoButtonIconList = new List<RoleIcon>();
	#endregion

	#region 功能按钮区域
	public GameObject addFriendButtonsRoot;
	public GameObject requestFriendButtonsRoot;
	#endregion

	public FriendCellType CurrentFriendCellType
	{
		set
		{
			this.setCurrentFriendCellType(value);
		}
		get
		{
			return this.currentFriendCellType;
		}
	}

	protected void setCurrentFriendCellType(FriendCellType friendCellType)
	{
		this.currentFriendCellType = friendCellType;
		if(this.currentFriendCellType == FriendCellType.FriendCell || this.currentFriendCellType == FriendCellType.SuDiCell)
		{
			this.roleHaveButtonRoot.SetActive(false);
			this.roleNoButtonRoot.SetActive(true);
			this.addFriendButtonsRoot.SetActive(false);
			this.requestFriendButtonsRoot.SetActive(false);
			roleIconList = this.roleNoButtonIconList;
		}
		else if(this.currentFriendCellType == FriendCellType.RequestCell)
		{
			this.roleHaveButtonRoot.SetActive(true);
			this.roleNoButtonRoot.SetActive(false);
			this.addFriendButtonsRoot.SetActive(false);
			this.requestFriendButtonsRoot.SetActive(true);
			roleIconList = this.roleHaveButtonIconList;
		}
		else if(this.currentFriendCellType == FriendCellType.SearchCell)
		{
			this.roleHaveButtonRoot.SetActive(true);
			this.roleNoButtonRoot.SetActive(false);
			this.addFriendButtonsRoot.SetActive(true);
			this.requestFriendButtonsRoot.SetActive(false);
			roleIconList = this.roleHaveButtonIconList;
		}
	}

	public void setRoleIcons(SimpleMonster[] p)
	{
		int index = 0;
		foreach(RoleIcon roleIcon in this.roleIconList)
		{
			if(p == null)
			{
				roleIcon.gameObject.SetActive(false);
			}
			else if(index >= p.Length)
			{
				roleIcon.gameObject.SetActive(false);
			}
			else
			{
				int roleID = p[index].num;
				roleIcon.gameObject.SetActive(true);
				roleIcon.headIcon.spriteName = roleID.ToString();
				int starCount = Core.Data.monManager.getMonsterByNum(roleID).star + p[index].star;
				roleIcon.setStars(starCount);
			}
			index++;
		}
	}

	void onClicked()
	{
		if(currentFriendCellType == FriendCellType.FriendCell)
		{
			UIFriendDialog friendDialog = UIFriendDialog.getFriendDialog(UIMainFriend.Instance.transform.parent);
			friendDialog.otherUserInfo = this.otherUserInfo;
			friendDialog.addFriendDelegate = onAddFriendButtonClicked;
			friendDialog.setActionButtonRoot(UIFriendDialog.FriendDialogType.FriendDialog);
		}
		else if(currentFriendCellType == FriendCellType.SuDiCell)
		{
			UIFriendDialog friendDialog = UIFriendDialog.getFriendDialog(UIMainFriend.Instance.transform.parent);
			friendDialog.otherUserInfo = this.otherUserInfo;
			friendDialog.addFriendDelegate = onAddFriendButtonClicked;
			friendDialog.setActionButtonRoot(UIFriendDialog.FriendDialogType.SuDiDialog);
		}
	}

	void onYesButtonClicked()
	{
		ComLoading.Open();
		Core.Data.FriendManager.agreeOrRefusedFriendRequestCompletedDelegate = agreeOrRefusedFriendRequestCompleted;

		// 1为 同意 2为 不同意
		Core.Data.FriendManager.agreeOrRefusedFriendRequest(this.otherUserInfo, 1);
	}
	
	void onNoButtonClicked()
	{
		ComLoading.Open();
		Core.Data.FriendManager.agreeOrRefusedFriendRequestCompletedDelegate = agreeOrRefusedFriendRequestCompleted;

		// 1为 同意 2为 不同意
		Core.Data.FriendManager.agreeOrRefusedFriendRequest(this.otherUserInfo, 2);
	}


	protected void agreeOrRefusedFriendRequestCompleted(int agreeOrRefusedFriend, int state)
	{
		Core.Data.FriendManager.agreeOrRefusedFriendRequestCompletedDelegate = null;
		if(state == 1)
		{
			if(agreeOrRefusedFriend == 1)
			{
				UIMainFriend.Instance.setCell(UIMainFriend.ShowType.Friend, Core.Data.FriendManager.friendList);
			}
			UIMainFriend.Instance.setCell(UIMainFriend.ShowType.Request, Core.Data.FriendManager.searchUserResultList);
		}
	}


	void onAddFriendButtonClicked()
	{
		ComLoading.Open();
		Core.Data.FriendManager.addOrDeleteFriendRequestCompletedDelegate = addOrDeleteFriendRequestCompleted;
		Core.Data.FriendManager.addOrDeleteFriendRequest(this.otherUserInfo, 1);
	}

	void addOrDeleteFriendRequestCompleted(int op, int state)
	{
		Core.Data.FriendManager.addOrDeleteFriendRequestCompletedDelegate = null;
		if(op == 1)
		{
			if(state == 1)
			{
				RED.changeButtonState(this.addFriendButton, false);
			}
			else if(state == 2)
			{
			}
			else if(state == 3)
			{
				RED.changeButtonState(this.addFriendButton, false);
			}
		}
	}
}
