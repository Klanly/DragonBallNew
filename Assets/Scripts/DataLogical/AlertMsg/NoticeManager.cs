using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoticeManager : Manager,ICore {
    #region ICore implementation

    public void Dispose ()
    {
        throw new System.NotImplementedException ();
    }

    public void Reset ()
    {
        throw new System.NotImplementedException ();
    }

    public void OnLogin (object obj)
    {
        throw new System.NotImplementedException ();
    }


    //   public static bool isFirstShou
    public static bool isFirstShow = false;

    public static bool openAnnounce = false;
    public static bool openSign = false;
   
    /// <summary>
    /// 登录展示  每日先弹公告     1：7天奖励  2：每日签到  -1：都为空 
    /// </summary>
    public static int firstShowState = -1;
    public static int pairmax = 0;
    public static int pairtms =0;
    public static int signtms = 0;

	public static AlertInfo _AlertInfo = null;
    //公告 
    private Dictionary<string,string> announceDic = null;

    private List<DataGiftData> dateGiftDateList = null;


    public NoticeManager(){
        announceDic = new Dictionary<string, string> ();
        dateGiftDateList = new List<DataGiftData> ();
    }
   
    #endregion

	public override bool loadFromConfig() {
		return true;
	}

    public override void fullfillByNetwork (BaseResponse response)
    {
        //只有开机第一次展示    
    
        if (response != null && response.status != BaseResponse.ERROR) {
            LoginResponse loginResp = response as LoginResponse;
            if (loginResp != null && loginResp.data != null && loginResp.data.alertInfo != null) {
                firstShowState = loginResp.data.alertInfo.isSevenSigin;
				_AlertInfo = loginResp.data.alertInfo;
                if (loginResp.data.alertInfo.notice != null) {
					if (isFirstShow == false) {
						openAnnounce = true;
						isFirstShow = true;
					}
                    announceDic.Clear ();
                    //公告 存入 dic 中 
                    for (int i = 0; i < loginResp.data.alertInfo.notice.Title.Length; i++) {
						if(!announceDic.ContainsKey(loginResp.data.alertInfo.notice.Title [i]))
                            announceDic.Add (loginResp.data.alertInfo.notice.Title [i], loginResp.data.alertInfo.notice.Content [i]);
                    }
                }
				if (loginResp.data.sysStatus != null) {
					if (loginResp.data.sysStatus.openSign == 1) {
						if (firstShowState != -1) {
							openSign = true;
						}
					} else {
						openSign = false;
					}
				}


				if (loginResp.data.alertInfo.dailySgin != null) {
					pairmax = loginResp.data.alertInfo.dailySgin.pairmax;
					pairtms = loginResp.data.alertInfo.dailySgin.pairtms;
					signtms = loginResp.data.alertInfo.dailySgin.signtms;

					dateGiftDateList.Clear ();
//					ConsoleEx.Write ("   in   not null ");
					this.FullFillDateGiftDateList (loginResp.data.alertInfo.dailySgin.days);
				} else {
//					ConsoleEx.Write ("   isssss  null " + dateGiftDateList.Count);
					if (dateGiftDateList != null) {
						dateGiftDateList.Clear ();
						pairmax = 0;
						pairtms = 0;
						signtms = 0;
//						ConsoleEx.Write ("   isssss  null " + pairmax + "   pms " + pairtms + " sign tms " + signtms);
					}
				}
            }
        }
    }


    public List<DataGiftData> GetDateGiftList(){
        return dateGiftDateList;
    }


//	public void ClearList(){
//		dateGiftDateList.Clear ();
//		pairmax = 0;
//		pairtms = 0;
//		signtms = 0;
//	}
    /// <summary>
    /// 覆盖 存储签到 的 list；
    /// </summary>
    /// <param name="dataList">Data list.</param>
    public void FullFillDateGiftDateList (DataGiftData[] dataList){
        if (dataList.Length > 0) {
            dateGiftDateList.Clear ();
            for (int i = 0; i < dataList.Length; i++) {
                dataList[i].signItem.pid = dataList[i].p [0];
                dataList[i].signItem.num = dataList[i].p [1];
                dataList[i].signItem.coin= dataList[i].d [0];
                dataList[i].signItem.vip = dataList[i].d [2];
                dataList[i].signItem.day = dataList[i].d [3];
                dataList[i].signItem.rate= dataList[i].d [4];
                dateGiftDateList.Add (dataList[i]);
            }
        }
    }
       

}
