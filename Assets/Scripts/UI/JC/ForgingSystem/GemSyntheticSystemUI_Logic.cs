using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FrogingSystem;

public class GemSyntheticSystemUI_Logic : MonoBehaviour
{
    public System.Action<Gems> SelectGemDelegate;
    GemSyntheticSystemUI_View view;
    string LastClickGemButtonName = "Btn_LGem";

    public Gems Selected_Frist_GemData{ get; set; }

    public Gems Selected_Second_GemData{ get; set; }

    int SelectedMouldNum{ get; set; }
    /*当前背包中宝石模具的数量
	 * */
    int GemMouldNumInBag{ get; set; }
    /*宝石合成成功率
	 * */
    float SyntheticSucessRate = 0;
    /*当前背包中宝石精华的数量
	 * */
    int GemDebrisCountInBag = 0;
    /*合成花费金币
	 * */
    int NeedCoin = 0;
    /*合成花费钻石
	 * */
    int NeedStone = 0;

    #region wxl

    int successNum = 0;
    int tempId = 0;
    int tempLv = 0;

	int CombineTimes = 0;
    private int defaultNum = 1;
    private int maxLimitNum;
    private int GemId;
    int count = 0;
    List<int> keylist = null;

    #endregion

    void Awake()
    {
        /*注册监听
		**/
        view = gameObject.GetComponent<GemSyntheticSystemUI_View>();
        view.ButtonClick += ButtonClick;

    }

    void Start()
    {		
        defaultNum = 1;
    }

    void OnEnable()
    {
        SelectedMouldNum = 0;
        GemMouldNumInBag = Core.Data.itemManager.GetBagItemCount(110064);	
        if (Selected_Frist_GemData != null && Selected_Second_GemData != null)
            view.SetSelectedGemNumMouldNum(0, GemMouldNumInBag);
    }

    void ButtonClick(GameObject button)
    {
        switch (button.name)
        {
            case "Btn_Gem_120101":
            case "Btn_Gem_120102":
            case "Btn_Gem_120201":
            case "Btn_Gem_120202":
            case "Btn_Gem_120301":
            case "Btn_Gem_120302":
                {
                    int DuiHuanNum = System.Convert.ToInt32(button.name.Substring(8, 6));
                    SendDuiHuanMsg(DuiHuanNum);
                }
                break;
            case "Btn_LGem":
            case "Btn_RGem":
                FillGemBtn(button);
                break;		
            case "Btn_Describe":
                Debug.Log("Btn_Describe");
                DescribeMessageBox.Open(TEXT(5055) + TEXT(9004), TEXT(9015));
                break;
            case "Btn_Synthetic":
		/*宝石合成
		 */
            //	SendHeChengMsg();
                ReCombineGem();
                break;
            case "+":
                PlusModelNum();
                break;
            case "-":
                ReduceModelNum();
                break;
//            case "-Num":
//                if(defaultNum >1)
//                    ReduceBtn();
//                break;
//            case "+Num":
//                PlusBtn();
//                break;
            case "DespBtn":
                view.ShowDesp();
                break;
            case "CloseDespBtn":
                view.CloseDesp();
                break;

            case "Btn_Synthetic_Baodi":
                //     SendHeChengMsg(true);
                break;
        }
    }

    void UIInformationSure()
    {
        UIDragonMallMgr.GetInstance().OpenUI(ShopItemType.Item, ShopExit);		
    }

    void UIInformationCancel()
    {
        //ClearLastSelected();
    }

    void ShopExit()
    {
        UIMiniPlayerController.Instance.SetActive(true);
        if (gameObject != null)
            gameObject.SetActive(true);
		
		
        GemMouldNumInBag = Core.Data.itemManager.GetBagItemCount(110064);	
        if (Selected_Frist_GemData != null && Selected_Second_GemData != null)
            view.SetSelectedGemNumMouldNum(SelectedMouldNum, GemMouldNumInBag - (defaultNum * SelectedMouldNum), defaultNum);
		
		
    }
    /*清除最近的选择
	 * */
    public void ClearLastSelected()
    {
        SelectedMouldNum = 0;
        view.SetSelectedGemNumMouldNum(0, SelectedMouldNum);
        SyntheticSucessRate = 0;
        view.SetScuessRate(SyntheticSucessRate);
        NeedCoin = 0;
        view.SetNeedCoin(NeedCoin);
        NeedStone = 0;
        defaultNum = 1;
        view.SetCombineGemNum(defaultNum);
        view.SetNeedStone(NeedStone);
        Selected_Frist_GemData = null;
        Selected_Second_GemData = null;
        view.SetLGem(null);
        view.SetRGem(null);
		view.SetLGemLv (0);
		view.SetRGemLv (0);
        view.InitInterface();

    }

    public void SelectGem(Gems gemdata)
    {
        if (Selected_Frist_GemData == null)
        {
            Selected_Frist_GemData = gemdata;
            Selected_Second_GemData = null;
			
        }
        else if(Selected_Frist_GemData != null && Selected_Second_GemData != null){
            if (gemdata != null)
            {
                Selected_Frist_GemData = null;
                Selected_Second_GemData = null;
                Selected_Frist_GemData = gemdata;
            }
        }
        else
        {
            Selected_Second_GemData = gemdata;
        }
		
        if (LastClickGemButtonName == "Btn_LGem")
        {
            view.SetLGem(gemdata.configData.anime2D);
			view.SetLGemLv (gemdata.configData.level);
            //view.SetRGem(null);
        }
        else
        {
            view.SetRGem(gemdata.configData.anime2D);
			view.SetRGemLv (gemdata.configData.level);
        }
		
		//自动填充另一个宝石
        AutoFillOtherGem();
		
        /*计算宝石合成成功率
		 * */
        CalculateSucessRate();
		
    }
    //批量合成
    void ReCombineGem()
    {
        if (Selected_Frist_GemData == null || Selected_Second_GemData == null)
        {
            SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(9012),null);
            return;
        }
            if (Core.Data.playerManager.Coin < defaultNum*Core.Data.gemsManager.getGemData(Selected_Frist_GemData.configData.target).coin)
            {
                JCRestoreEnergyMsg.OpenUI(ItemManager.COIN_PACKAGE,ItemManager.COIN_BOX,2);
                return ;
            }
            successNum = 0;
            count = 0;
            tempId = 0;
            keylist = new List<int>();
		CombineTimes = defaultNum;
		//Debug.Log (" default  == " + defaultNum);

            for (int i = 0; i < defaultNum; i++)
            {
                SendCombineGems();
            }
            ComLoading.Open();
        
    }
    //发送消息
    void SendCombineGems()
    {
        Send_GemSynticSystem param = new Send_GemSynticSystem();
        param.gid = Core.Data.playerManager.PlayerID;
        param.g_id1 = 0;
        param.g_id2 = 0;
        if (Selected_Frist_GemData != null && Selected_Second_GemData != null)
        {
            if (Core.Data.gemsManager.getSameGemCount(Selected_Frist_GemData) > 1)
            {
                Dictionary<int,Gems> sameGems = Core.Data.gemsManager.sameGems;
                foreach (int tk in sameGems.Keys)
                {
                    if (tk != Selected_Frist_GemData.id && tk != Selected_Second_GemData.id)
                    {
                        if (!keylist.Contains(tk))
                        {
                            if (param.g_id1 == 0)
                            {
                                param.g_id1 = tk;
                            }
                            else if (param.g_id2 == 0 && tk != param.g_id1)
                            {
                                param.g_id2 = tk;
                            }
                        }
                    }
                }
            }
            tempLv = Selected_Frist_GemData.configData.level;

            if (param.g_id1 == 0 && Selected_Frist_GemData.id != param.g_id2)
            {
                param.g_id1 = Selected_Frist_GemData.id;
            }

            if (param.g_id2 == 0 && param.g_id1 != Selected_Second_GemData.id)
            {
                param.g_id2 = Selected_Second_GemData.id;
            }
        }

        keylist.Add(param.g_id1);
        keylist.Add(param.g_id2);

        if (param.g_id1 == 0 || param.g_id2 == 0)
        {
            SQYAlertViewMove.CreateAlertViewMove(view.TEXT(9012));
            return;
        }
      
        param.nm = SelectedMouldNum;
        param.bd = false;

        HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
        task.AppendCommonParam(RequestType.GEM_SYNTHETIC, param);

        task.ErrorOccured += testHttpResp_Error;
        task.afterCompleted += BackSendCombineGems;

        task.DispatchToRealHandler();
      
    }
    // 返回
    void BackSendCombineGems(BaseHttpRequest request, BaseResponse response)
    {
        
        if (response.status != BaseResponse.ERROR)
        {       
            HttpRequest rq = request as HttpRequest;
            if (rq.Type == RequestType.GEM_SYNTHETIC)
            {           
                GemSyntheitcResponse resp = response as GemSyntheitcResponse;
                if (resp.data.succ == 1)
                {
                    successNum++;
                    tempId = resp.data.upId;
                }
                else
                {
                    //正常失败
                    SQYAlertViewMove.CreateAlertViewMove(view.TEXT(9014));     
                    ClearLastSelected();        
                }
                //talking data add by wxl 
                if (resp.data != null)
                {
                    if (resp.data.stone != 0)
                    {
                        Core.Data.ActivityManager.OnPurchaseVirtualCurrency(ActivityManager.ForgingType, 1, Mathf.Abs(resp.data.stone));
                    }
                }
                count++;
                BackGemGroupCombine();
            }
           
        }
        else
        {
			// SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
            ComLoading.Close();
        }
    }
    //全部合成返回
    void BackGemGroupCombine()
    {
		if (count == CombineTimes)
        { 
            if (tempId != 0)
            {
                Gems resultGem = Core.Data.gemsManager.getGems(tempId);
                if (resultGem != null)
                {
                    GetGemSucUI.OpenUI(resultGem.configData, view.TEXT(5064));
                }
            }
            else
            {
				// SQYAlertViewMove.CreateAlertViewMove(view.TEXT(9014));     
                ClearLastSelected(); 
            }

			Debug.Log (" lv  =" + tempLv   +";  defaultNum =" + defaultNum+"; successNum = "+  successNum );
			view.SetCombineGemsResult(tempLv, CombineTimes, successNum);
            GemMouldNumInBag = Core.Data.itemManager.GetBagItemCount(110064); 
            SelectedMouldNum = 0;
            view.SetSelectedGemNumMouldNum(SelectedMouldNum, GemMouldNumInBag);
            DBUIController.mDBUIInstance.RefreshUserInfo();
            ClearLastSelected();
			CombineTimes = 0;
        }
        ComLoading.Close();
    }

    void testHttpResp_Error(BaseHttpRequest request, string error)
    {
        SQYAlertViewMove.CreateAlertViewMove(error);
    }

    public string TEXT(int num_text)
    {
        return	Core.Data.stringManager.getString(num_text);
    }
    //计算宝石成功率
    public void CalculateSucessRate()
    {

        if (Selected_Frist_GemData != null && Selected_Second_GemData != null)
        {
            int uplevel = Selected_Frist_GemData.configData.target;
            GemData upGem = Core.Data.gemsManager.getGemData(uplevel);
            int NeedStone = 0;
            NeedCoin = 0;
            if (upGem != null)
            {
                SyntheticSucessRate = upGem.probability + SelectedMouldNum * 5;		
                NeedCoin = upGem.coin;
                NeedStone = upGem.stone;
				
                //显示要升级的宝石的信息
                view.ShowTargetGemInfo(upGem);
            }

            if (SyntheticSucessRate > 100f)
                SyntheticSucessRate = 100f;
            view.SetScuessRate(SyntheticSucessRate);
			
            CalculateMoney(defaultNum, NeedCoin, NeedStone);

//			view.SetNeedCoin(NeedCoin);
//			view.SetNeedStone(NeedStone);
        }
        else
        {
            view.ShowTargetGemInfo(null);
        }
    }
    /*向服务器发送兑换信息
	 * */
    public void SendDuiHuanMsg(int GemNum)
    {
        Send_GemExchangeSystem param = new Send_GemExchangeSystem();
        param.gid = Core.Data.playerManager.PlayerID;
        param.pid = GemNum;
        HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
        task.AppendCommonParam(RequestType.GEM_EXCHANGE, param);
        task.ErrorOccured += testHttpResp_Error;
        task.afterCompleted += HttpRespGemExChange;
        task.DispatchToRealHandler();
        ComLoading.Open();
    }

    public void HttpRespGemExChange(BaseHttpRequest request, BaseResponse response)
    {
        ComLoading.Close();
        if (response.status != BaseResponse.ERROR)
        {		
            HttpRequest rq = request as HttpRequest;
            if (rq.Type == RequestType.GEM_EXCHANGE)
            {					
                GemExChangeResponse resp = response as GemExChangeResponse;
				
                GemData tempdata = Core.Data.gemsManager.getGemData(resp.data.pid);
                if (tempdata != null)
                //if(Core.Data.gemsManager.gemPriceConfig.ContainsKey(resp.data.pid))
                {	
                    int price = tempdata.price;
                    Debug.Log("Price:" + price);
                    GemDebrisCountInBag -= price;
                    //view.SetGemDebrisNum(GemDebrisCountInBag);
                }
                GetGemSucUI.OpenUI(Core.Data.gemsManager.getGems(resp.data.ppid).configData, view.TEXT(9014));
            }
        }
        else
        {
			
            SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
        }
    }

    public void Quit()
    {
        ClearLastSelected();
    }

    #region  add by wxl  增加 批量合成功能

    //原来 switch 里面的 方法 拷贝
    void PlusModelNum()
    {
        //改成 2 了   最多用两个
        if (SelectedMouldNum + 1 <= 2 && ((SelectedMouldNum + 1)*defaultNum) <= GemMouldNumInBag)
        {
            SelectedMouldNum++;
            view.SetSelectedGemNumMouldNum(SelectedMouldNum * defaultNum, GemMouldNumInBag - SelectedMouldNum * defaultNum, defaultNum);
            CalculateSucessRate();
        }
        else if (((SelectedMouldNum + 1)* defaultNum)  > GemMouldNumInBag)
        {
            //跳转商城
            UIInformation.GetInstance().SetInformation(TEXT(9058), TEXT(5030), UIInformationSure, UIInformationCancel);
        }
    }

    void ReduceModelNum()
    {
        if (SelectedMouldNum - 1 >= 0)
        {
            SelectedMouldNum--;
            view.SetSelectedGemNumMouldNum(SelectedMouldNum * defaultNum, GemMouldNumInBag - SelectedMouldNum * defaultNum, defaultNum);
            CalculateSucessRate();
        }
    }
    //原来  switch  方法 copy
    void FillGemBtn(GameObject button)
    {
        if (Selected_Frist_GemData == null)   //第一个选的 为空 
        {
            int count = 0;
            for (short i = 1; i < 6; i++)
            {
                List<Gems> list = Core.Data.gemsManager.GetGemsByStar(i, SplitType.Split_If_InTeam, 10);
                if (list != null)
                {
                    count += list.Count;
                }
            }
            if (count == 0)
            {
				if(LuaTest.Instance != null && !LuaTest.Instance.ConvenientBuy)return;
                UIInformation.GetInstance().SetInformation(TEXT(9019), TEXT(5030), UIInformationSure, UIInformationCancel);
                return;
            }

            DBUIController.mDBUIInstance.SetViewState(RUIType.EMViewState.S_Bag, RUIType.EMBoxType.GEM_HECHENG_MAIN);
            LastClickGemButtonName = button.name;
            ForgingRoomUI.Instance.Visible = false;
        }
        else if (Selected_Frist_GemData != null && Selected_Second_GemData == null)   //第二个为空
        {

            if (LastClickGemButtonName != button.name)
            {
                int SameGemCount = Core.Data.gemsManager.getSameGemCount(Selected_Frist_GemData);
                if (SameGemCount > 1)
                {                       
                    DBUIController.mDBUIInstance.SetViewState(RUIType.EMViewState.S_Bag, RUIType.EMBoxType.GEM_HECHENG_SUB);                
                    LastClickGemButtonName = button.name;
                    ForgingRoomUI.Instance.Visible = false;

                }
                else
					if(LuaTest.Instance != null && !LuaTest.Instance.ConvenientBuy)return;
                    UIInformation.GetInstance().SetInformation(TEXT(9019), TEXT(5030), UIInformationSure, UIInformationCancel);
            }
            else
            {
                Selected_Frist_GemData = null;
                this.FillGemBtn(button);

            }
        }
        else if (Selected_Frist_GemData != null && Selected_Second_GemData != null )//都不为空
        {
            int count = 0;
            for (short i = 1; i < 6; i++)
            {
                List<Gems> list = Core.Data.gemsManager.GetGemsByStar(i, SplitType.Split_If_InTeam, 10);
                if (list != null)
                {
                    count += list.Count;
                }
            }
            if (count == 0)
            {
                UIInformation.GetInstance().SetInformation(TEXT(9019), TEXT(5030), UIInformationSure, UIInformationCancel);
                return;
            }

            DBUIController.mDBUIInstance.SetViewState(RUIType.EMViewState.S_Bag, RUIType.EMBoxType.GEM_HECHENG_MAIN);
            LastClickGemButtonName = button.name;
            ForgingRoomUI.Instance.Visible = false;

        }


//        else
//        {
//
//            //Debug.Log("SameGemCount=="+SameGemCount +"LastClickGemButtonName="+LastClickGemButtonName+"=="+"button.name=="+button.name);
//            if( LastClickGemButtonName == button.name )
//            {
//                ClearLastSelected();
//                DBUIController.mDBUIInstance.SetViewState (RUIType.EMViewState.S_Bag, RUIType.EMBoxType.GEM_HECHENG_MAIN);
//                LastClickGemButtonName=button.name;
//                ForgingRoomUI.Instance.Visible=false;
//            }
//            else
//            {
//                int SameGemCount= Core.Data.gemsManager.getSameGemCount(Selected_Frist_GemData);
//                //Debug.Log("SameGemCount=="+SameGemCount);
//                if(SameGemCount >1)
//                {                       
//                    DBUIController.mDBUIInstance.SetViewState (RUIType.EMViewState.S_Bag,RUIType.EMBoxType.GEM_HECHENG_SUB);                
//                    LastClickGemButtonName=button.name;
//                    ForgingRoomUI.Instance.Visible=false;
//
//                }
//                else
//                    UIInformation.GetInstance().SetInformation(TEXT(9019),TEXT(5030),UIInformationSure,UIInformationCancel);
//            }

        //     }



    }

    public void PlusNumBtn()
    {

        if (CanPress(defaultNum + 1))
        {
            defaultNum++;
            CalculateSucessRate();
        }

    }

    public void ReduceNumBtn()
    {
        if (defaultNum > 1)
            defaultNum--;
        CalculateSucessRate();

    }
    //算钱
    void CalculateMoney(int GemNum, int tCoin, int tStone)
    {
        if (Selected_Frist_GemData != null && Selected_Second_GemData != null)
        {
            if (GemNum >= 1)
            {
                NeedCoin = tCoin * GemNum;
                NeedStone = tStone * GemNum;
            }
            view.SetNeedCoin(NeedCoin);
            view.SetNeedStone(NeedStone);
            view.SetCombineGemNum(defaultNum);
            view.SetSelectedGemNumMouldNum(SelectedMouldNum * defaultNum, GemMouldNumInBag - SelectedMouldNum * defaultNum, defaultNum);
        }
    }

    bool CanPress(int selectNum)
    {
        if (Selected_Frist_GemData != null && Selected_Second_GemData != null)
        {
            //宝石不足
            int maxDNum = Core.Data.gemsManager.getSameGemCount(Selected_Frist_GemData);
            if (maxDNum % 2 != 0)
            {
                maxDNum = (maxDNum - 1) / 2;
            }
            else
                maxDNum /= 2;
            if (selectNum > maxDNum)
            {
                SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(7392));
                return false;
            }

            // 模具 不足
           
            if (selectNum * SelectedMouldNum > GemMouldNumInBag)
            {
                UIInformation.GetInstance().SetInformation(TEXT(9058), TEXT(5030), UIInformationSure, UIInformationCancel);
                return false;
            }

            //计算钱  钻石保底已经去掉
           // int moneyMax = 0;


            GemData upGem = Selected_Frist_GemData.configData;


            int curMoney = Core.Data.playerManager.Coin;
            int needUnit = upGem.coin;

            //   moneyMax = curMoney / needUnit;
          
            if (selectNum * needUnit > curMoney)
            {
                SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(35006));
                return false;
            }
           
            return  true;

        }
        else
        {
            return false;
        }

    }
    //自动补全功能
    void AutoFillOtherGem()
    {
        if (Selected_Frist_GemData != null)
        {
            List<Gems> NextGemList = Core.Data.gemsManager.GetGemByFirstSelectedAndStar(Selected_Frist_GemData.configData.star);
            if (NextGemList != null)
            {
                for (int i = 0; i < NextGemList.Count; i++)
                {
                    if (NextGemList[i].id != Selected_Frist_GemData.id)
                    {
                        Selected_Second_GemData = NextGemList[i];
                        break;
                    }
                }
            }


            view.SetCombineGemsResult(0,0,0);

            if (Selected_Second_GemData == null)
            {
                if (LastClickGemButtonName == "Btn_LGem")
                {
                    view.SetRGem(null);
					view.SetRGemLv(0);
                }
                else
                {
                    view.SetLGem(null);
					view.SetLGemLv(0);
                }

				if(LuaTest.Instance != null && !LuaTest.Instance.ConvenientBuy){;}
                else UIInformation.GetInstance().SetInformation(TEXT(9019), TEXT(5030), UIInformationSure, UIInformationCancel);
                return;
            }
        }

        if (LastClickGemButtonName == "Btn_LGem")
        {
            view.SetRGem(Selected_Second_GemData.configData.anime2D);
			view.SetRGemLv (Selected_Second_GemData.configData.level);
        }
        else
        {
            view.SetLGem(Selected_Second_GemData.configData.anime2D);
			view.SetLGemLv (Selected_Second_GemData.configData.level);
        }
    }

    #endregion
}
