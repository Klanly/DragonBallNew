using UnityEngine;
using System.Collections;
using FrogingSystem;

public class CombineGemsController : RUIMonoBehaviour
{
    private static CombineGemsController instance;
    public static CombineGemsController Instance
    {
        get
        {
            return instance;
        }
    }

    public System.Action callBack = null;
    public enum moneyType
    {
        isCoin,  //一般合成
        isDia,   //保底合成
    }
    public moneyType curMoneyType;

    public static CombineGemsController CreatCombineGemsPanel( System.Action tCallBack , moneyType  tType)
    {

        UnityEngine.Object obj = WXLLoadPrefab.GetPrefab(WXLPrefabsName.UICombineGemPanel);
        if (obj != null)
        {
            GameObject go = Instantiate(obj) as GameObject;
            CombineGemsController fc = go.GetComponent<CombineGemsController>();
            Transform goTrans = go.transform;
            go.transform.parent = DBUIController.mDBUIInstance._TopRoot.transform;
            go.transform.localPosition = Vector3.zero;
            goTrans.localScale = Vector3.one;
            RED.TweenShowDialog(go);
            fc.callBack = tCallBack;
            fc.curMoneyType = tType;
            return fc;
        }
            return null;        
    }


    public UILabel lbl_CoinNum;
    public UILabel lbl_DiaNum;
    public UILabel lbl_TargetNum;
    public UILabel lbl_BtnType;

    public UIButton btn_plus;
    public UIButton btn_reduce;
    public UIButton btn_Combine;

    private int defaultNum = 1;
    int targetGemLevel =0;
    private Gems curGem = null;

    int coinNum = 0;
    int stoneNum = 0;
	// private int maxLimitNum;
//    private int getSelectModelNum = 0;
//    private int getModelNumInBag = 0;

    void Awake(){
        instance = this;
    }

    void Start(){
        curGem =  ForgingRoomUI.Instance.SyntheticSystem.Selected_Frist_GemData;
        targetGemLevel = curGem.configData.level;
//        getSelectModelNum = ForgingRoomUI.Instance.SyntheticSystem.GetSelectModelNum();
//        getModelNumInBag =  ForgingRoomUI.Instance.SyntheticSystem.GetModelNumInBag();
    }
    #region 统计最大数量
    void PlusBtn(){
        defaultNum++;
        RefreshMsg();
    }
    void ReduceBtn(){
        if(defaultNum >1)
            defaultNum--;
        RefreshMsg();
    }

    //最大化按钮 
    void MaxBtnMethod(){
    }

    //计算最大限制数
    void CalculateMaxLimitNum(){
        if(curGem!= null){


            int maxDNum = Core.Data.gemsManager.getSameGemCount(curGem);
            //无限制 最大 合成数
            if (maxDNum % 2 != 0)
            {
                maxDNum = (maxDNum - 1) / 2;
            }
            else
                maxDNum /= 2;
            // 宝石模具
			//   int modelMax = 0;
//            if (getSelectModelNum != 0)
//            {
//				int   modelMax = getModelNumInBag / getSelectModelNum;
//            }
           
            //保底最大值
			// int moneyMax = 0;
//            GemData upGem = Core.Data.gemsManager.getGemData(targetGemLevel);
//            if (curMoneyType == moneyType.isCoin)//非保
//            {
//                int curMoney = Core.Data.playerManager.Coin;
//                //不要钻石
//                int needUnit = upGem.coin;
//				//	int  moneyMax = curMoney / needUnit;
//            }
//            else
//            {
//                int curMoney = Core.Data.playerManager.Stone;
//                //要钻石
//                int needUnit = upGem.stone;
//				//	int  moneyMax = curMoney / needUnit;
//            }


//            if (modelMax == 0)
//            {
//                maxLimitNum = (moneyMax > maxDNum) ? maxDNum : moneyMax;
//            }
//            else
//            {
//                int tMin = (maxDNum > moneyMax) ? moneyMax : maxDNum;
//                maxLimitNum = modelMax > tMin ? tMin : modelMax;  
//            }

        }


    }


    void CalculateMoney(int GemNum){
        if (GemNum >= 1)
        {
            GemData upGem = Core.Data.gemsManager.getGemData(targetGemLevel);

            int NeedCoin = upGem.coin;
            int NeedStone = upGem.stone;
            coinNum =NeedCoin* GemNum;
            stoneNum =  NeedStone * GemNum;
        }
    }

    #endregion
    void RefreshMsg(){
        CalculateMoney(defaultNum); 
        lbl_CoinNum.text = coinNum.ToString();
        lbl_DiaNum.text = stoneNum.ToString();
    }

    void CloseBtn(){
        Destroy(gameObject);
    }

    
    void BtnCombine(){

    }
     


}
