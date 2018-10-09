using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIGuaGuaLeMain : RUIMonoBehaviour 
{
	static UIGuaGuaLeMain _mInstance;
	public static UIGuaGuaLeMain GetInstance()
	{
		return _mInstance;
	}

	public UILabel _Count;
	public GameObject BigBg;
	public UIButton _BeginBtn;
	public UIButton _BackBtn;
	public UILabel Title1;
	public UILabel Title2;
	public List<UIGuaGuaLeCell> GuaGuaLeList = new List<UIGuaGuaLeCell>();
	[HideInInspector]
	public int GuaGuaTotalNum = 0;

	const int NeedStone = 20;

	private ItemOfReward[] _ItemOfReward;
	private int _NowCount;
	private int _GetRewardStone;

	public bool m_IsBeginClick = false;

	public static void OpenUI()
	{
		Object obj = PrefabLoader.loadFromPack("LS/pbLSDragonBallGuaGuaLe");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			if(go != null)
			{
				RED.AddChild (go, DBUIController.mDBUIInstance._bottomRoot);
				_mInstance = go.GetComponent<UIGuaGuaLeMain>();
				_mInstance.AllCellReset();
			}
		}
    }
    
    void AllCellReset()
	{
		for(int i=0; i<GuaGuaLeList.Count; i++)
		{
			GuaGuaLeList[i].OnReset();
		}
	}

	void AllCellReverse()
	{
		for(int i=0; i<GuaGuaLeList.Count; i++)
		{
			GuaGuaLeList[i].SetGuaGuaColor(false);
            
        }
	}

	void SendGuaGuaStatusMessage()
	{
		ComLoading.Open();
		GuaGuaStatusRequest param = new GuaGuaStatusRequest(int.Parse(Core.Data.playerManager.PlayerID));
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		
		task.AppendCommonParam(RequestType.GUAGUALE_STATUS, param);

		task.afterCompleted += testHttpResp_UI;
		
		task.DispatchToRealHandler ();
    }

	void SendGuaGuaLeMessage()
	{
		ComLoading.Open();
		GuaGuaLeRequest param = new GuaGuaLeRequest(int.Parse(Core.Data.playerManager.PlayerID));
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		
		task.AppendCommonParam(RequestType.GUAGUALE, param);

		task.afterCompleted += testHttpResp_UI;
        
        task.DispatchToRealHandler ();
    }
    
	void testHttpResp_UI(BaseHttpRequest request, BaseResponse response)
	{
		if(response != null && response.status != BaseResponse.ERROR)
		{
			HttpRequest httprequest = request as HttpRequest;
			if(httprequest.Act == HttpRequestFactory.ACTION_GUAGUALE_STATUS)
			{
				GuaGuaStatusResponse res = response as GuaGuaStatusResponse;
				_Count.text = string.Format(Core.Data.stringManager.getString(25118), res.data.status.count.ToString());
				_NowCount = res.data.status.count;
				BigBg.gameObject.SetActive(true);

            }
			else if (httprequest.Act == HttpRequestFactory.ACTION_GUAGUALE)
			{
				BigBg.gameObject.SetActive(false);
				m_IsBeginClick = true;
				_BackBtn.isEnabled = false;
				GuaGuaLeResponse res = response as GuaGuaLeResponse;
				int[] guagualist;
				if(res.data.p == null)
				{
					guagualist = Core.Data.guaGuaLeMgr.GetGuaGuaLeData();
				}
				else 
				{
					_ItemOfReward = res.data.p;
					_GetRewardStone = res.data.p[0].num;
                    Core.Data.ActivityManager.setOnReward(res.data.p[0] , ActivityManager.GUAGUALE) ; 
                    guagualist = Core.Data.guaGuaLeMgr.GetGuaGuaLeData(res.data.p[0].num);
				}
				for(int i=0; i<guagualist.Length; i++)
				{
					GuaGuaLeList[i].SetHead(guagualist[i]); 
				}
				AllCellReverse();


                //talkingdata   add by wxl 
                int stone =Mathf.Abs(res.data.stone);
                Core.Data.ActivityManager.OnPurchaseVirtualCurrency (ActivityManager.ScratchType,1,stone);
				DBUIController.mDBUIInstance.RefreshUserInfo();
			}
		}
		else if(response != null && response.status == BaseResponse.ERROR)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
		}

		ComLoading.Close();
	}

	void Begin_Click()
	{
		if(_NowCount >= 3)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(25117));
			return;
		}
		if(Core.Data.playerManager.Stone < NeedStone)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(35006));
			return;
		}
		else 
		{
			SendGuaGuaLeMessage();
		}
		_BeginBtn.isEnabled = false;
    }
    
    void GuaGuaCallBack()
    {
        SendGuaGuaStatusMessage();
        AllCellReset();
		_BeginBtn.isEnabled = true;
		_BackBtn.isEnabled = true;
		m_IsBeginClick = false;
    }
    
    void GuaGuaDelay()
    {
        Core.Data.playerManager.RTData.curStone += _ItemOfReward[0].num;
		DBUIController.mDBUIInstance.RefreshUserInfo();
		GetRewardSucUI.OpenUI(_ItemOfReward, Core.Data.stringManager.getString (5097),true,GuaGuaCallBack);
    }
    
    void Back_OnClick()
	{
		for(int i=0; i<GuaGuaLeList.Count; i++)
		{
			GuaGuaLeList[i].dealloc();
		}
		this.dealloc();
		DBUIController.mDBUIInstance.ShowFor2D_UI();
	}

	public void BeginChooseAnimation()
	{
		if(GuaGuaTotalNum >= 9)
		{
			if(_GetRewardStone == 0)
			{
				GuaGuaCallBack();
			}
			else
			{
				for(int j=0; j<GuaGuaLeList.Count; j++)
				{
					if(GuaGuaLeList[j].isAround)return;
				}
				for(int i=0; i<GuaGuaLeList.Count; i++)
				{
					if(GuaGuaLeList[i]._StoneNum == _GetRewardStone)
					{
						GuaGuaLeList[i].BeginAnimat();
                    }
                }
                Invoke("GuaGuaDelay", 1.8f);
            }
            
            GuaGuaTotalNum = 0;
        }
    }

	void Start()
	{
		Title1.SafeText(Core.Data.stringManager.getString(25124));
		Title2.SafeText(Core.Data.stringManager.getString(25125));
		m_IsBeginClick = false;
		SendGuaGuaStatusMessage();
	}

	void OnDestroy()
	{

	}

}
