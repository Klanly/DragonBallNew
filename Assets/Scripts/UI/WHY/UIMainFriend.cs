using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIMainFriend : MonoBehaviour {

	#region 基本变量

	// 总页数
	int pageCount = 0;

	// 当前页数
	int currentPage = 0;

	// 总数
	int listCount = 0;

	List<OtherUserInfo> otherUserInfoList;

	public ShowType currentShowType;

	// 1搜索，2推荐	
	public int searchOP = 2;

	public enum ShowType
	{
		Friend,
		Request,
		SuDi,
		Search,
		None
	}

	private static UIMainFriend instance = null;

	static public UIMainFriend Instance
	{
		get
		{
			if (instance == null)
			{
				instance = UnityEngine.Object.FindObjectOfType(typeof(UIMainFriend)) as UIMainFriend;

				if (instance == null)
				{
					GameObject mainFriendPrb = PrefabLoader.loadFromPack("WHY/pbUIMainFriend") as GameObject;
					GameObject mainFriendPrbObj = GameObject.Instantiate(mainFriendPrb) as GameObject;


					instance = mainFriendPrbObj.GetComponent<UIMainFriend>();
				}
			}
			return instance;
		}
	}

	protected GameObject friendCellPrb;

	public GameObject upPageButton;
	public GameObject downPageButton;
	#endregion

	#region 好友
	public UITable friendTable;
	#endregion

	#region 好友请求
	public UITable requestTable;
	#endregion

	#region 宿敌
	public UITable suDiTable;
	#endregion

	#region 搜索
	public UILabel searchTitleLabel;
	public UITable searchTable;
	public UIInput searchMessage;
	#endregion

	#region 基本方法
	void Start()
	{
		this.searchOP = 2;

		DBUIController.mDBUIInstance.HiddenFor3D_UI();
	}

	protected void initPrb()
	{
		if(friendCellPrb == null)
		{
			friendCellPrb = PrefabLoader.loadFromPack("WHY/pbUIFriendCell") as GameObject;
		}
	}

	protected UIFriendCell getFriendCellObj(Transform parent)
	{
		initPrb();
		GameObject friendCellObj = Instantiate(this.friendCellPrb) as GameObject;
		friendCellObj.transform.parent = parent;
		friendCellObj.transform.localPosition = Vector3.zero;
		friendCellObj.transform.localScale = Vector3.one;
		return friendCellObj.GetComponent<UIFriendCell>();
	}

	void ClickBack()
	{
		DBUIController.mDBUIInstance.ShowFor2D_UI();
		Destroy(gameObject);
	}

	public void setMainFriendRoot(GameObject root)
	{
		gameObject.transform.parent = root.transform;
		gameObject.transform.localPosition = Vector3.back * 10;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
	}

	void OnDestroy()
	{
		Core.Data.FriendManager.friendList.Clear();
		Core.Data.FriendManager.suDiList.Clear();
		Core.Data.FriendManager.requestFriendList.Clear();

	}

//	protected void setOtherUserDataListToFriendCell(ShowType showType)
//	{
//		List<OtherUserData> otherUserInfoConfig = null;
//		if(showType == ShowType.Friend)
//		{
//			otherUserInfoConfig = Core.Data.FriendManager.getFriendConfig();
//		}
//		else
//		{
//			otherUserInfoConfig = Core.Data.FriendManager.getSuDiConfig();
//		}
//		if(otherUserInfoConfig == null || otherUserInfoConfig.Count == 0)
//		{
//			return;
//		}
//
//		setFriendCell(showType, otherUserInfoConfig);
//	}

	public void setCell(ShowType showType, List<OtherUserInfo> otherUserInfoList)
	{
		UITable table = null;
		UIFriendCell.FriendCellType friendCellType = UIFriendCell.FriendCellType.None;

		this.currentShowType = showType;

		// 该页显示的个数
		int count = FriendManager.pageShowListItemCount;
		int start = 0;

		if(showType == ShowType.Friend)
		{
			table = this.friendTable;
			friendCellType = UIFriendCell.FriendCellType.FriendCell;
			this.listCount = Core.Data.FriendManager.friendListCount;
			this.currentPage = Core.Data.FriendManager.friendListCurrentPage;
			this.pageCount = Core.Data.FriendManager.friendListPageCount;
		}
		else if(showType == ShowType.SuDi)
		{
			table = this.suDiTable;
			friendCellType = UIFriendCell.FriendCellType.SuDiCell;
			this.listCount = Core.Data.FriendManager.suDiListCount;
			this.currentPage = Core.Data.FriendManager.suDiListCurrentPage;
			this.pageCount = Core.Data.FriendManager.suDiListPageCount;
		}
		else if(showType == ShowType.Request)
		{
			table = this.requestTable;
			friendCellType = UIFriendCell.FriendCellType.RequestCell;
			this.listCount = Core.Data.FriendManager.requestFriendListCount;
			this.currentPage = Core.Data.FriendManager.requestFriendListCurrentPage;
			this.pageCount = Core.Data.FriendManager.requestFriendListPageCount;
		}
		else if(showType == ShowType.Search)
		{
			table = this.searchTable;
			friendCellType = UIFriendCell.FriendCellType.SearchCell;
		}

		if(this.currentPage == 1)
		{
			this.upPageButton.SetActive(false);
		}

		if(this.currentPage == pageCount)
		{
			this.downPageButton.SetActive(false);
			count = this.listCount - (this.currentPage - 1) * FriendManager.pageShowListItemCount;
		}

		while(table.transform.childCount > 0 )
		{
			GameObject g =  table.transform.GetChild(0).gameObject;
			g.transform.parent = null;
			Destroy(g);
		}
		table.Reposition();

		start = FriendManager.pageShowListItemCount * (this.currentPage - 1);

		for(int i = start; i < count; i++)
		{
			OtherUserInfo otherUserInfo = otherUserInfoList[i];
			UIFriendCell friendCell = this.getFriendCellObj(table.transform);
			friendCell.CurrentFriendCellType = friendCellType;

			this.otherUserInfoToFriendCell(friendCell, otherUserInfo);
		}
		table.Reposition();
	}

	protected void otherUserInfoToFriendCell(UIFriendCell friendCell, OtherUserInfo otherUserInfo)
	{
		friendCell.userHead.textureName = otherUserInfo.hi.ToString();
		friendCell.userName.text = otherUserInfo.n;
		friendCell.userLevel.text = otherUserInfo.lv.ToString();
		friendCell.userVIPLevel.text = otherUserInfo.vipLv.ToString();
		friendCell.userRank.text = Core.Data.stringManager.getString(6081) + otherUserInfo.r.ToString();
		friendCell.setRoleIcons(otherUserInfo.p);
		friendCell.otherUserInfo = otherUserInfo;
	}

	#endregion

	#region 好友
	public void getFriendList()
	{
		ComLoading.Open();

		Core.Data.FriendManager.getFriendListRequest();
		Core.Data.FriendManager.getFriendListRequestCompletedDelegate = setFriend;
	}

	void onFriendClick()
	{
		getFriendList();
	}

	public void setFriend()
	{
		Core.Data.FriendManager.getFriendListRequestCompletedDelegate = null;
		this.setCell(ShowType.Friend, Core.Data.FriendManager.friendList);
	}

	#endregion

	#region 好友请求
	void onFriendRequestClick()
	{
		ComLoading.Open();
		Core.Data.FriendManager.getFriendRequestListRequest();
		Core.Data.FriendManager.getFriendRequestListRequestCompletedDelegate = setFriendRequest;
	}

	public void setFriendRequest(List<OtherUserInfo> otherUserDataList)
	{
		Core.Data.FriendManager.getFriendRequestListRequestCompletedDelegate = null;
		this.setCell(ShowType.Request, otherUserDataList);
	}
	#endregion

	#region 宿敌
	void onSuDiClick()
	{
//		henry edit 
//        ComLoading.Open();
//		Core.Data.FriendManager.getSuDiRequest();
//		Core.Data.FriendManager.getSuDiListRequestCompletedDelegate = setSuDi;
	}

	public void setSuDi()
	{
		//Core.Data.FriendManager.getSuDiListRequestCompletedDelegate = null;
		this.setCell(ShowType.SuDi, Core.Data.FriendManager.suDiList);
	}
	#endregion

	#region 搜索
	void onAddClick()
	{
		ComLoading.Open();
		Core.Data.FriendManager.searchFriendRequestCompletedDelegate = setSearchResult;
		Core.Data.FriendManager.searchUserRequest(this.searchMessage.value, this.searchOP);

		this.upPageButton.SetActive(false);
		this.downPageButton.SetActive(false);

	}

	void onSearchClick()
	{
		if(searchMessage.value == "")
		{
			return;
		}
		ComLoading.Open();
		Core.Data.FriendManager.searchFriendRequestCompletedDelegate = setSearchResult;
		Core.Data.FriendManager.searchUserRequest(searchMessage.value, this.searchOP);
		this.searchTitleLabel.text = Core.Data.stringManager.getString(6077);
	}

	public void setSearchResult(List<OtherUserInfo> otherUserDataList)
	{
		this.searchOP = 1;
		this.setCell(ShowType.Search, otherUserDataList);

	}
	#endregion

	#region 翻页
	void onUpPageButtonClick()
	{
		if(this.currentPage == 1)
		{
			return;
		}

		if(this.currentShowType == ShowType.Friend)
		{
			Core.Data.FriendManager.friendListCurrentPage--;
			this.currentPage = Core.Data.FriendManager.friendListCurrentPage;
		}
		else if(this.currentShowType == ShowType.SuDi)
		{
			Core.Data.FriendManager.suDiListCurrentPage--;
			this.currentPage = Core.Data.FriendManager.suDiListCurrentPage;
		}
		else if(this.currentShowType == ShowType.Request)
		{
			Core.Data.FriendManager.requestFriendListCurrentPage--;
			this.currentPage = Core.Data.FriendManager.requestFriendListCurrentPage;
		}
	}

	void onDownPageButtonClick()
	{
		if(this.currentPage == this.pageCount)
		{
			return;
		}
		if(this.currentShowType == ShowType.Friend)
		{
			Core.Data.FriendManager.friendListCurrentPage++;
			this.currentPage = Core.Data.FriendManager.friendListCurrentPage;
			requestDownPage(Core.Data.FriendManager.friendList, onFriendClick);

		}
		else if(this.currentShowType == ShowType.SuDi)
		{
			Core.Data.FriendManager.suDiListCurrentPage++;
			this.currentPage = Core.Data.FriendManager.suDiListCurrentPage;
			requestDownPage(Core.Data.FriendManager.friendList, onSuDiClick);
		}
		else if(this.currentShowType == ShowType.Request)
		{
			Core.Data.FriendManager.requestFriendListCurrentPage++;
			this.currentPage = Core.Data.FriendManager.requestFriendListCurrentPage;
			requestDownPage(Core.Data.FriendManager.friendList, onFriendRequestClick);
		}
	}

	void requestDownPage(List<OtherUserInfo> contentList, Action request)
	{
		if(contentList.Count < this.listCount)
		{
			request();
		}
	}
	#endregion
}
