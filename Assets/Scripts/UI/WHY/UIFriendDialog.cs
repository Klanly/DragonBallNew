using UnityEngine;
using System;
using System.Collections;

public class UIFriendDialog : MonoBehaviour
{
	#region 基本变量
	public GameObject friendActionButtonRoot;
	public GameObject suDiActionButtonRoot;

	public OtherUserInfo otherUserInfo;

	public GameObject friendCell;

	public enum FriendDialogType
	{
		FriendDialog,
		SuDiDialog,
		None
	}

	protected FriendDialogType friendDialogType = FriendDialogType.None;

	public Action addFriendDelegate;
	#endregion

	void onColse()
	{
		Destroy(gameObject);
	}

	public static UIFriendDialog getFriendDialog(Transform parent)
	{
		GameObject prb;

		prb = PrefabLoader.loadFromPack("WHY/pbUIFriendDialog") as GameObject;


		GameObject obj = Instantiate(prb) as GameObject;

		obj.transform.parent = parent;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localScale = Vector3.one;
		return obj.GetComponent<UIFriendDialog>();
	}

	public void setActionButtonRoot(FriendDialogType friendDialogType)
	{
		//  friendDialogType = friendDialogType;
		this.friendActionButtonRoot.SetActive(friendDialogType == FriendDialogType.FriendDialog);
		this.suDiActionButtonRoot.SetActive(friendDialogType == FriendDialogType.SuDiDialog);
	}

	public void onSendMessage()
	{
		UISendMessageDialog.getSendMessageDialog(transform.parent);
	}

	void onDeleteButtonClick()
	{
		ComLoading.Open();
		if(friendDialogType == FriendDialogType.FriendDialog)
		{
			Core.Data.FriendManager.addOrDeleteFriendRequestCompletedDelegate = deleteFriendRequestCompleted;
			Core.Data.FriendManager.addOrDeleteFriendRequest(otherUserInfo, 2);
		}
		else if(friendDialogType == FriendDialogType.FriendDialog)
		{
			//Core.Data.FriendManager.deleteSuDiData = this.otherUserInfo;
			//Core.Data.FriendManager.deleteSuDiCompletedDelegate = deleteSuDiCompleted;
			//Core.Data.FriendManager.deleteSuDiRequest(this.otherUserInfo.eyid);
		}
	}

	void deleteFriendRequestCompleted(int op, int state)
	{
		Core.Data.FriendManager.addOrDeleteFriendRequestCompletedDelegate = null;
		if(op == 2 && state == 1)
		{
			UIMainFriend.Instance.setCell(UIMainFriend.ShowType.Friend, Core.Data.FriendManager.friendList);
		}
	}

	void deleteSuDiCompleted()
	{
		//Core.Data.FriendManager.deleteSuDiCompletedDelegate = null;
		//UIMainFriend.Instance.setCell(UIMainFriend.ShowType.SuDi, Core.Data.FriendManager.suDiList);
	}

	void onWaterButtonClick()
	{

	}

	void onFightButtonClick()
	{
		ComLoading.Open();
		if(friendDialogType == FriendDialogType.FriendDialog)
		{

		}
		else if(friendDialogType == FriendDialogType.SuDiDialog)
		{

		}
	}

	void onRequestFriendButtonClick()
	{
		if(addFriendDelegate != null)
		{
			addFriendDelegate();
		}
//		ComLoading.Open();
//		Core.Data.FriendManager.addOrDeleteFriendRequestCompletedDelegate = addOrDeleteFriendRequestCompleted;
//		Core.Data.FriendManager.addOrDeleteFriendRequest(this.otherUserInfo, 1);
	}

//	void addOrDeleteFriendRequestCompleted(int op, int state)
//	{
//		Core.Data.FriendManager.addOrDeleteFriendRequestCompletedDelegate = null;
//		if(op == 1)
//		{
//			if(state == 1 || state == 2)
//			{
//
//			}
//		}
//	}
}
