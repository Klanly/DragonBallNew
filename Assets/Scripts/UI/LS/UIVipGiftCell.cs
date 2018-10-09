using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIVipGiftCell : RUIMonoBehaviour
{
	VipInfoData m_data;
	public UIPanel m_Panel;
	public List<RewardCell> m_RewardList;
	public UILabel m_Des;
	public UIButton m_GetReward;
	public UILabel m_Title;
	public UIScrollView m_ScrollView;
	public UIScrollBar m_ScrollBar;
	public UIGrid m_Grid;

	private int m_Id;

	Vector4 m_Size = Vector3.zero;

	public void SetDetail(VipInfoData _data)
	{
		m_data = _data;
		if(m_data == null)return;
		if(m_data.vipLv == 0)return;
		if(m_Panel != null)
		{
			m_Size = m_Panel.baseClipRegion;
        }
		HideRewardList();
		m_Des.SafeText("");
		m_ScrollBar.value = 0f;
		m_Grid.Reposition();
		m_Des.SafeText(DealWithStr(m_data.tips));
		BoxCollider _box = m_Des.GetComponent<BoxCollider>();
		_box.size = new Vector3(m_Des.width,  (float)m_Des.height, _box.size.z);
		_box.center = new Vector3((float)m_Des.width/2, (float)-m_Des.height/2, _box.center.z);



		int _Count = 0;
		for(;_Count <_data.reward.Count; _Count++)
		{
			m_RewardList[_Count].gameObject.SetActive(true);
			ItemOfReward itemodreward = new ItemOfReward();
			itemodreward.pid = _data.reward[_Count][0];
			itemodreward.num = _data.reward[_Count][1];
			m_RewardList[_Count].InitUI(itemodreward);
		}

		m_Id = _data.vipLv;
		VipShopInfo _info;

		if(m_Id > Core.Data.playerManager.curVipLv)
		{
			m_Title.gameObject.SetActive(true);
			m_GetReward.gameObject.SetActive(false);
			m_Title.SafeText(string.Format(Core.Data.stringManager.getString(25167), m_Id));
        }
		else
		{
			m_Title.gameObject.SetActive(false);
			m_GetReward.gameObject.SetActive(true);
			m_GetReward.isEnabled = false;
			if(Core.Data.vipManager.IsFirstLogin_Vip)
			{
				m_GetReward.isEnabled = false;
			}
			else
			{
				if(!UIDragonMallMgr.GetInstance().m_VipStatusDic.TryGetValue(m_Id, out _info))
				{
					m_GetReward.isEnabled = true;
                }
            }
            
        }

    }
    
    public void SetPanel(float _Alpha)
	{
		m_Panel.alpha = _Alpha;
	}

	void HideRewardList()
	{
		for(int i=0; i<m_RewardList.Count; i++)
		{
			m_RewardList[i].gameObject.SetActive(false);
		}
	}

	string DealWithStr(string _str)
	{
		_str = _str.Replace("{","");
		_str = _str.Replace("}","");
		string[] _temp = _str.Split(',');
		string newstr = "";

		int _index = 0;
		for(int i=0; i<_temp.Length; i++, _index++)
		{
			string[] strT =  _temp[i].ToString().Split('：');
			
			
			string strText = Core.Data.stringManager.getString(10019);
			
			
			if (strT.Length == 1 )
			{
				strText = string.Format(strText , strT[0],"");
				
			}else if (strT.Length ==2)
			{
				string  str=":" +strT[1];
				strText = string.Format(strText , strT[0],str);
			}
			newstr = newstr +strText +  "\r\n" ;
			
			
		}
		return newstr;
	}

	void GetVipRewardRequest()
	{
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.GETVIPLEVELREWARD, new GetVipLevelRewardParam(int.Parse(Core.Data.playerManager.PlayerID), m_Id));
		
		task.afterCompleted = getDateCompleted;
		task.ErrorOccured = getDateError;
		
		task.DispatchToRealHandler ();
		ComLoading.Open();
    }
	
	void getDateCompleted(BaseHttpRequest request, BaseResponse response)
	{
		ComLoading.Close();
		if(response != null && response.status != BaseResponse.ERROR)
		{
			GetVipLevelRewardResponse res = response as GetVipLevelRewardResponse;
			GetRewardSucUI.OpenUI(res.data.p,Core.Data.stringManager.getString(5047));
			if(!UIDragonMallMgr.GetInstance().m_VipStatusDic.ContainsKey(m_Id))
			{
				UIDragonMallMgr.GetInstance().m_VipStatusDic.Add(m_Id, null);
				m_GetReward.isEnabled = false;
			}


			if (UIDragonMallMgr.GetInstance ().GetIsVipGift == true) {
				Core.Data.ActivityManager.SetActState (ActivityManager.vipLibaoType, "1");
			} else {
				Core.Data.ActivityManager.SetActState (ActivityManager.vipLibaoType, "2");
			}


			//  领取vip成功
			if(UIWXLActivityMainController.Instance != null)
			{
				UIWXLActivityMainController.Instance.Refresh ();
			}


		}
		else if(response != null && response.status == BaseResponse.ERROR)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
		}
		
	}
	
	void getDateError(BaseHttpRequest request, string error)
	{
		ComLoading.Close();
		ConsoleEx.DebugLog("Error = " + error);
	}
}
