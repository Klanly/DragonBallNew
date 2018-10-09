using UnityEngine;
using System.Collections;

public class HolidayActivityData : Manager {

	// Use this for initialization
//    HolidayData _har;
	NewActivityResponse _har;
    //开始倒计时
    public HolidayActivityData() {
        _har = null;
    }

	public int mActivityIndex = 0;
	
    public void setHourChanged() {

		ActivityLimitTimeRequest ();
//        sendSer(null);
//
        long curSysTime = Core.TimerEng.curTime;
		TimerTask tt = new TimerTask (curSysTime,curSysTime +86400, 300, ThreadType.BackGround);
        tt.onEvent = sendSer; 
        tt.DispatchToRealHandler();
    }

    //派发消息
    public void sendSer(TimerTask t) {
        GuideManager guideMgr = Core.Data.guideManger;
//        去掉新手引导限制
        if(!guideMgr.isGuiding) {
			ActivityLimitTimeRequest ();
        }
    }

    //消息正方返回
    void activityAward_UI(BaseHttpRequest request, BaseResponse response)
    {
        if(response != null && response.status != BaseResponse.ERROR) {
			har = response as NewActivityResponse ;
            if(showIcon && Core.SM.CurScenesName == SceneName.MAINUI) {

				if (SQYMainController.mInstance != null) {
					SQYMainController.mInstance.ArrangeNewActivity ();
				}
            }else if (Core.SM.CurScenesName == SceneName.MAINUI)
            {
                if (SQYMainController.mInstance != null) {
                    SQYMainController.mInstance.ArrangeNewActivity ();
                }
            }
        } 
        else if(response != null && response.status == BaseResponse.ERROR)
        {
            ConsoleEx.DebugLog("Error Code = " + response.errorCode.ToString());
        }

    }

	public NewActivityResponse har {
        set 
        {
            _har = value;
        }
        get{
            return  _har; 
        }
    }

    public bool showIcon {
		get {
			bool show = false;
			if (_har != null) {
				if (_har.data != null) {
					if (_har.data.Length > 0) {
						show = true;
					}
				}
			}
			return show;
		}
	}

	void ActivityLimitTimeRequest()
	{
		ActivityLimitTimeParam param = new ActivityLimitTimeParam(int.Parse(Core.Data.playerManager.PlayerID));
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);

		task.AppendCommonParam(RequestType.ACTIVITYLIMITTIME, param);

		task.afterCompleted += activityAward_UI;

		task.DispatchToRealHandler ();
	}

	public void SetNewActivityResponseData(int _ItemIndex, NewActivityItem _newActivityItem)
	{
		if(_har != null && _har.data != null && _har.data.Length != 0)
		{
			if(mActivityIndex >=0 && mActivityIndex < _har.data.Length)
			{
				if(_har.data[mActivityIndex].items != null && _har.data[mActivityIndex].items.Length != 0)
				{
					if(_ItemIndex >= 0 && _ItemIndex < _har.data[mActivityIndex].items.Length)
					{
						_har.data[mActivityIndex].items[_ItemIndex] = _newActivityItem;
					}
				}

			}
		}
	}

//	void ActivityLimitTimeReponse(BaseHttpRequest request, BaseResponse response)
//	{
//		ComLoading.Close();
//		if (response != null && response.status != BaseResponse.ERROR) 
//		{
//
//			NewActivityResponse res = response as NewActivityResponse;
//
//			if(res != null)
//			{
//				har = res;
//				activityAward_UI (request,response);
//			}
//		}
//		if(response != null && response.status == BaseResponse.ERROR)
//		{
//			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
//		}
//	}





}
