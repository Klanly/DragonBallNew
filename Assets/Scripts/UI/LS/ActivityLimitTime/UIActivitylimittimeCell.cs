using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class UIActivitylimittimeCell : RUIMonoBehaviour 
{
	public List<RewardCell> m_ActivityItemList =  new List<RewardCell>();

	public UILabel m_Title1;
	public UILabel m_Title2;
	public UIGrid m_Grid;
	public UIButton m_Btn;
	public UILabel m_BtnText;
	public UIScrollBar m_ScrollBar;
	public UIScrollView m_ScrollView;
	NewActivityItem m_Item;
	string m_ActivityId;
	int activityItemIndex;

	public void Init(NewActivityItem _item, string _activityid, int _activityItemIndex)
	{
		m_Item = _item;
		m_ActivityId = _activityid;
		activityItemIndex = _activityItemIndex;
		if(m_Title1 != null)m_Title1.SafeText(_item.completeDesc);
		StringBuilder builder = new StringBuilder();
		builder.Append(_item.completeRate.ToString());
		builder.Append("/");
		builder.Append(_item.completeWhere);
		if(m_Title2 != null)m_Title2.SafeText(builder.ToString());
		SetBtnStatus(_item.status);
		CreateCell(m_Item.rewards);
		m_Grid.Reposition();
		m_ScrollBar.value = 0f;
	}

	void SetBtnStatus(int _status)
	{
		/// const STATUS_OK = 1;        //可以领取
		/// const STATUS_REWARDED = 2;  //已领取
		/// const STATUS_NO = 3;        //不可领取
		/// const STATUS_JUMP = 4;      //跳转到商城

		if(m_Btn != null && m_BtnText != null && m_Item != null)
		{
			m_Btn.isEnabled = (_status == 1 || _status == 4) ? true : false;
			if(_status == 1)
			{
				m_BtnText.SafeText(Core.Data.stringManager.getString(25179));
			}
			else if(_status == 2)
			{
				m_BtnText.SafeText(Core.Data.stringManager.getString(25180));
			}
			else if(_status == 3)
			{
				m_BtnText.SafeText(Core.Data.stringManager.getString(25181));
			}
			else if(_status == 4)
			{
				m_BtnText.SafeText(Core.Data.stringManager.getString(25182));
			}
		}
	}

	void CreateCell(ItemOfReward[] _Item)
	{
		if(_Item != null)
		{
			Object prefab = PrefabLoader.loadFromPack ("LS/pbLSActivityLimitTimeItemCell");
			if (prefab != null)
			{
				for(int i=0; i<_Item.Length; i++)
				{
					GameObject obj = Instantiate (prefab) as GameObject;
					RED.AddChild (obj, m_Grid.gameObject);
					RewardCell mScript = obj.GetComponent<RewardCell> ();
					mScript.InitUI(_Item[i]);
					m_ActivityItemList.Add(mScript);
					mScript.m_AddSpr.gameObject.SetActive(i == _Item.Length -1 ? false : true);
				}
				m_ScrollView.disableDragIfFits = !(_Item.Length > 3);
			}
		}
	}

	void GetActivityLimitTimeRequest()
	{
		GetActivityLimitTimeParam param = new GetActivityLimitTimeParam(int.Parse(Core.Data.playerManager.PlayerID), int.Parse(m_ActivityId), int.Parse(m_Item.itemID));
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		
		task.AppendCommonParam(RequestType.GETACTIVITYLIMITTIME, param);
		
		task.ErrorOccured += GetActivityLimitTimeError;
		task.afterCompleted += GetActivityLimitTimeReponse;
		
		task.DispatchToRealHandler ();
		ComLoading.Open();
	}
	
	void GetActivityLimitTimeReponse(BaseHttpRequest request, BaseResponse response)
	{
		ComLoading.Close();
		if (response != null && response.status != BaseResponse.ERROR) 
		{
			GetActivityLimittimeRewardResponse res = response as GetActivityLimittimeRewardResponse;
			if(res != null)
			{
				GetRewardSucUI.OpenUI(res.data.p,Core.Data.stringManager.getString(5047));
				if(res.data != null && res.data.item != null)
				{
					m_Item = res.data.item;
					StringBuilder builder = new StringBuilder();
					builder.Append(res.data.item.completeRate.ToString());
					builder.Append("/");
					builder.Append(res.data.item.completeWhere);
					if(m_Title2 != null)m_Title2.SafeText(builder.ToString());
					SetBtnStatus(res.data.item.status);
					DBUIController.mDBUIInstance.RefreshUserInfo();

					Core.Data.HolidayActivityManager.SetNewActivityResponseData(activityItemIndex, res.data.item);
				}

			}
		}
		if(response != null && response.status == BaseResponse.ERROR)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
		}
	}
	
	void GetActivityLimitTimeError (BaseHttpRequest request, string error)
	{
		ComLoading.Close();
		ConsoleEx.DebugLog ("---- Http Resp - Error has ocurred." + error);
	}

	public void Onclik()
	{
		SendMessageByStatus();
	}

	void SendMessageByStatus()
	{
		if(m_Item.status == 1)
		{
			GetActivityLimitTimeRequest();
		}
		else if(m_Item.status == 2)
		{
			m_BtnText.SafeText(Core.Data.stringManager.getString(25180));
		}
		else if(m_Item.status == 3)
		{
			m_BtnText.SafeText(Core.Data.stringManager.getString(25181));
		}
		else if(m_Item.status == 4)
		{
			UIDragonMallMgr.GetInstance ().OpenUINew (ShopItemType.HotSale);
			if(HolidayActivityLogic.instence != null)
			{
				Destroy(HolidayActivityLogic.instence);
			}
			if(UIActivitylimittimeMain.GetInstance() != null)
			{
				UIActivitylimittimeMain.GetInstance().Back_Onclick();
			}

		}
	}

	public void DeleteCell()
	{
		foreach(RewardCell cell in m_ActivityItemList)
		{
			Destroy(cell.gameObject);
		}
	}
}
