using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RollGambleController : RUIMonoBehaviour {

    private static RollGambleController instance;
    public static RollGambleController Instance
    {
        get
        {
            return instance;
        }
    }

    public UISprite selectBox;
    public UISprite[] giftsArea;
    public UILabel[] moneyLabels;
    public UISprite[] spDiamond;
    public UIButton btnRoll;
    private  List<int> moneyList = new List<int>();
    //public int targetTest ;
    public float intervalTime = 0.2f;
    private int curPosNum =0;
    private Vector3 offest = new Vector3(2,-2,0);
    public UILabel lblNeedMoney;
    private int needMoneyNum;
    enum moneyType{
        isDiamond,
        isCoin,
    }
    moneyType curType;
    public UISprite spNeedType;

    private GotGodGiftListData nextData;

    private ItemOfReward[] myReward;
    private bool isCanRoll = true;

    const string throundsIcon = "shop-0010";
    const string hundredIcon = "shop-0011";
    const string tenIcon = "shop-0012";
    const string tenThroundIcon = "shop-0009";
    const string maxThroundIcon = "shop-0008";
    const string tenMaxThroundIcon = "shop-0007";

    public static RollGambleController CreatRollGamblePanel(){
        UnityEngine.Object obj = WXLLoadPrefab.GetPrefab (WXLPrefabsName.UIRollGamblePanel);
        if(obj != null)
        {
            GameObject go = Instantiate(obj) as GameObject;
            RollGambleController fc = go.GetComponent<RollGambleController>();
            Transform goTrans = go.transform;
            go.transform.parent = DBUIController.mDBUIInstance._bottomRoot.transform;
            go.transform.localPosition = Vector3.zero;
            goTrans.localScale = Vector3.one;
            return fc;
        }
        return null;        
    }
    void Awake(){
        instance = this;
    }
    void Start(){
        DBUIController.mDBUIInstance.HiddenFor3D_UI ();
        ActivityNetController.GetInstance ().GotGodRewardList ();
    }

    public void InitData(GotGodGiftListData data){

        if (data != null)
        {
            if (data.awardList != null)
                moneyList.Clear();
            //  数据不够  暂用giftsArea
            for (int i = 0; i < giftsArea.Length; i++)
            {
                if (i < data.awardList.Count)
                    moneyList.Add(data.awardList[i][1]);
                else
                    moneyList.Add(data.awardList[data.awardList.Count - 1][1]);
            }

            if (data.awardList[0][0] == 110052)
            {
                curType = moneyType.isDiamond;
            }
            needMoneyNum = data.needStone;
            this.Init();
        }
        else
        {
            List<int[]> tList = Core.Data.ActivityManager.GetRollGamebleList();
            if (tList != null && tList.Count > 0)
            {
                for (int i = 0; i < giftsArea.Length; i++)
                {
                    if (i < tList.Count)
                        moneyList.Add(tList[i][1]);
                    else
                        moneyList.Add(tList[tList.Count - 1][1]);
                }
                this.Init();
            }

            btnRoll.isEnabled = false;
            isCanRoll = false;
        }
     
    }


    public void Init(){
        for (int i = 0; i < moneyLabels.Length; i++) {
            if (i < moneyList.Count) {
                moneyLabels [i].text = moneyList [i].ToString ();
                this.MakeIcon (i);
            }
        }
            
		if (curType == moneyType.isDiamond) {
		}
			lblNeedMoney.text = needMoneyNum.ToString ();
			curPosNum = 0;
		
    }



    void MakeIcon(int tIndex ){

        if (moneyList [tIndex] < 100) {
            spDiamond [tIndex].spriteName = tenIcon;
            spDiamond [tIndex].MakePixelPerfect ();
        } else if (moneyList[tIndex] >= 100 && moneyList[tIndex] < 500) {
            spDiamond [tIndex].spriteName = hundredIcon;
            spDiamond [tIndex].width = 106;
            spDiamond [tIndex].height = 78;

        } else if (moneyList[tIndex] >= 500 && moneyList[tIndex] < 1000) {
            spDiamond [tIndex].spriteName = throundsIcon;
            spDiamond [tIndex].width = 100;
            spDiamond [tIndex].height = 94;
        } else if (moneyList[tIndex] >= 1000 && moneyList[tIndex] < 1500) {
            spDiamond [tIndex].spriteName = maxThroundIcon;
            spDiamond [tIndex].width = 92;
            spDiamond [tIndex].height = 100;

        } else if (moneyList[tIndex] >= 1500) {
            spDiamond [tIndex].spriteName = tenMaxThroundIcon;
            spDiamond [tIndex].width = 120;
            spDiamond [tIndex].height = 110;
        }

    }



    IEnumerator RollAni( int targetNum){

        selectBox.gameObject.SetActive (true);
		float StarDur = intervalTime;
		int count =curPosNum;
        selectBox.transform.parent = giftsArea[count].transform;
        selectBox.transform.localPosition =offest ;
        float accTime = 0.01f;
		int tLen = giftsArea.Length;
        float tDir = -1;
        int stayCount = 50;

        Core.Data.soundManager.SoundFxPlay (SoundFx.Fx_openTRBox);

		while(true){
            if(count >= tLen){
				count %= tLen;
			}
			selectBox.transform.parent = giftsArea[count].transform;
			selectBox.transform.localPosition =offest ;
			yield return new WaitForSeconds(StarDur);

            if(StarDur <0.02f ){
                tDir = 1f;

			}
            if (tDir > 0) {
                stayCount--;
                if (stayCount <= 0) {
                    StarDur += accTime * tDir;
                }
            } else {
                StarDur += accTime * tDir;
            }
			

            if(StarDur >= 0.2f){
                if (count == targetNum ) {
                    selectBox.transform.parent = giftsArea[targetNum].transform;
                    selectBox.transform.localPosition =offest;
                    break;
                }
			}
            count ++;

		}
            

        curPosNum = targetNum;

        for (int i = 0; i < 10; i++) {

            selectBox.alpha = Mathf.Pow(-1,i) ;
            yield return new WaitForSeconds (0.1f);

        }

        yield return new  WaitForSeconds(0.5f);
        GetRewardSucUI.OpenUI (myReward,Core.Data.stringManager.getString(7369),true,ShowReward);

    }

    void ShowReward(){
     
        if (myReward != null) {
            Core.Data.playerManager.ReduceCoin (myReward [0].num, 2);
            Core.Data.ActivityManager.setOnReward(myReward [0] , ActivityManager.YAOYAOLE);
        }

        DBUIController.mDBUIInstance.RefreshUserInfo ();
        selectBox.alpha = 1;
        selectBox.gameObject.SetActive (false);
        btnRoll.isEnabled = true;
        if (nextData != null) {
            this.InitData (nextData);
        }

    }



    public void StartRoll(int target){
        StartCoroutine (RollAni(target));
    }

    void BackBtn(){
        //roll完才能离开  
        if (btnRoll.isEnabled == true || isCanRoll == false) {
            DBUIController.mDBUIInstance.ShowFor2D_UI ();
            Destroy (gameObject);
        }

    }

    public void RollBtn(){
        if (needMoneyNum > Core.Data.playerManager.Stone) {
            ActivityNetController.ShowDebug (Core.Data.stringManager.getString(35006));
            return;
        }
        btnRoll.isEnabled = false;
        ActivityNetController.GetInstance ().BuyGodReward ();

    }
        
    //  专用与刷新 
    public void GotGodGiftRefresh(GotGodGiftResponse resp ){
        if (resp.data != null) {
            nextData = resp.data.nextAward;
            myReward = resp.data.award;
            //自己处理钱的加减
            Core.Data.playerManager.ReduceCoin (resp.data.stone,2);

            int target = moneyList.IndexOf (resp.data.award [0].num);
            this.StartRoll (target);
            DBUIController.mDBUIInstance.RefreshUserInfo ();

        }
       
    }

        
}
