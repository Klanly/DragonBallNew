using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//yangcg
public class PhysicalLogic : MonoBehaviour 
{
    public UILabel dayCount ; // 每日次数
    public UILabel uilabel_1;// 每次购买的钱和精力描述
    public GameObject par;//
    private int _stoneNum;
    List<BuyEnergy> listBuyE = new List<BuyEnergy>();
	// Use this for initialization
	void Start () 
    {
	
	}
    public void sellJL()
    {
        if (Core.Data.playerManager.Stone  < _stoneNum) 
        {
            SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(7310));
            return ;
        }



        if(Core.Data.playerManager.dayStatus.buyEnyCount == Core.Data.vipManager.GetVipInfoData(Core.Data.playerManager.curVipLv).buy)
        {
            SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(20058));
            return ;
        }

        sendJLData param = new sendJLData(int.Parse(Core.Data.playerManager.PlayerID),_stoneNum);
        HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
        task.AppendCommonParam(RequestType.BUY_ENERGY,param);
        task.afterCompleted +=  CompletedMsg;
        task.DispatchToRealHandler ();
    }
    public void CompletedMsg(BaseHttpRequest request, BaseResponse response)
    {
        if(response != null && response.status != BaseResponse.ERROR)
        {

            getJLData re = response as getJLData;
            Core.Data.playerManager.dayStatus.buyEnyCount =   re.data.buycount;

            listBuyE =  Core.Data.vipManager.GetBuyEnergyDataConfig();
         

            BuyEnergy buyEnergy  ; 
            if (re.data.buycount >=listBuyE.Count )
            {
                buyEnergy = listBuyE[listBuyE.Count - 1];
            }
            else
            {
                buyEnergy =listBuyE[re.data.buycount];
            }
           
            _stoneNum = buyEnergy.cost_D;
            string str =Core.Data.stringManager.getString(9040)+ re.data.buycount +"/"+Core.Data.vipManager.GetVipInfoData(Core.Data.playerManager.curVipLv).buy.ToString();
            dayCount.text = str ;

            string strtxt = Core.Data.stringManager.getString(6133);

            strtxt = string.Format(strtxt,buyEnergy.cost_D,buyEnergy.num );
            uilabel_1.text = strtxt;

            // yangcg 购买成功回复的精力
            string strTxt = Core.Data.stringManager.getString(5214);
            string strJiLi = Core.Data.stringManager.getString(5038);
            strTxt = string.Format(strTxt,re.data.eny.ToString() ,strJiLi);
            SQYAlertViewMove.CreateAlertViewMove (strTxt);



            DBUIController.mDBUIInstance.RefreshUserInfo();// 刷新金币钻石等

        }
    }
    public void closeBtn()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }

    void DestroyPanel()
    {
        gameObject.SetActive(false);

        Destroy(gameObject.transform.parent.gameObject);
    }

    public int stoneNum
    {
        set {_stoneNum = value ;}
        get {return _stoneNum;}
    }
	
}
