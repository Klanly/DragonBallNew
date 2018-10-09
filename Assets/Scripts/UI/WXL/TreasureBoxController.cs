using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreasureBoxController : MonoBehaviour
{
    /*
     *  {"discount":[],"description":"开启铜箱子的钥匙。","ID":110021,"star":3,"type":3,"type2":[],"price":[],"num":[[110021,0,100,1]],"Visible":[],"num2":[],"name":"铜钥匙"}
        {"discount":[],"description":"开启银箱子的钥匙。","ID":110022,"star":4,"type":3,"type2":[],"price":[],"num":[[110022,0,100,1]],"Visible":[],"num2":[],"name":"银钥匙"}
        {"discount":[],"description":"开启金箱子的钥匙。","ID":110023,"star":5,"type":3,"type2":[],"price":[],"num":[[110023,0,100,1]],"Visible":[],"num2":[],"name":"金钥匙"}
*/
    private static TreasureBoxController instance;

    public static TreasureBoxController Instance {
        get {
            return instance;
        }
    }



    public UILabel[] lblKeyNum;
    public UIButton[] btnUseKey;
    public UIButton btn_Get;
    public UIButton againBtn;
    public UIButton BackBtn;
    public GameObject details;
    //第一层
    public GameObject openBox;
    //第二层
    public GameObject rewardObj;
    public GameObject selectObj;
    public GameObject root;
    //双倍 横栏
    public GameObject doubleObj;
    public GameObject despRoot;
    public UISprite rewardIcon;
    public List<UILabel> ItemsObj;
    public UILabel rewardItemLabel;
    public UILabel rewardName;
    public UILabel doubleLbl;
    public UILabel virtualLbl;
    public UILabel openKeyNumLbl;
    public UILabel labTitle;
    private ItemOfReward[] m_data;
    private int m_nIndex = 0;
    //0：Cu   // 1:Ag   //2: Au
    private int curState = 0;
    public const int basickeyId = 110020;
    Vector3 orginPos = new Vector3 (-10f, 80f, 0);
 
    public UISprite openBox_KeySpt;
    public UILabel[] lbl_double;
    private List<TBGiftDespItem> itemTBGDespList;
    public List<List<string>> nameList;
    public static List<int> doubleNum;
    string diaSpName = "110052"; // "common-0014";   
    // string keyspName = "baoxiang-";  // 1 金  2 银 3 铜
    int keyspName = 110020;
    public const int AuNeedNum = 100;
    public const int AgNeedNum = 30;
    public const int CuNeedNum = 10;
    public UISprite[] btnSp;
    public GameObject giftNum;
    public UILabel[] lblNeedStone;
    public UILabel rewardNum;
    private int doubleType = 0;
    Vector3 btnIconPos =  new Vector3(-80,-328,0);
    Vector3 btnKeyPos = new Vector3(-40,-328,0);

    public static TreasureBoxController CreatTreasureBoxCtr ()
    {

        UnityEngine.Object obj = WXLLoadPrefab.GetPrefab (WXLPrefabsName.UITreasurePanel);
        if (obj != null) {
            GameObject go = Instantiate (obj) as GameObject;
            TreasureBoxController fc = go.GetComponent<TreasureBoxController> ();
            Transform goTrans = go.transform;
            go.transform.parent = DBUIController.mDBUIInstance._bottomRoot.transform;
            go.transform.localPosition = Vector3.zero;
            goTrans.localScale = Vector3.one;
            return fc;
        }
        return null;    
    }

    void Awake ()
    {
        instance = this;
        itemTBGDespList = new List<TBGiftDespItem> ();
        nameList = new List<List<string>> ();
        doubleNum = new List<int> ();
    }

    public void Start ()
    {
        lblNeedStone [0].text =  "x"+ CuNeedNum.ToString ();
        lblNeedStone [1].text =  "x"+ AgNeedNum.ToString ();
        lblNeedStone [2].text =  "x"+ AuNeedNum.ToString ();

        DBUIController.mDBUIInstance.HiddenFor3D_UI (true);
        InitSelectItemInfo ();
        this.Init ();
        root.GetComponent<UIGrid> ().Reposition ();
        root.transform.localPosition =  Vector3.up * 75;
        ActivityNetController.GetInstance ().GetTreasureBoxState ();
    }

    public void Init ()
    {
        details.transform.localScale = Vector3.zero;
        details.SetActive (true);

        openBox.SetActive (false);
        openBox.transform.localScale = Vector3.one;

        rewardObj.transform.localScale = Vector3.zero;
        rewardObj.SetActive (true);
        selectObj.transform.localScale = Vector3.one;
        selectObj.SetActive (true);

        this.ShowBoxState ();
    }

    public  void OnClose ()
    {
        DBUIController.mDBUIInstance.ShowFor2D_UI ();
		if (UIWXLActivityMainController.Instance != null) {
			if (UIWXLActivityMainController.Instance.gameObject.activeInHierarchy == false) {
				UIWXLActivityMainController.Instance.SetActive (true);
				//   UIWXLActivityMainController.Instance.Refresh ();
            }
        }
        Destroy (gameObject);
    }

    void Refresh ()
    {
        int keyNum = Core.Data.itemManager.GetBagItemCount (basickeyId + curState);

		if(BackBtn != null)
        	BackBtn.isEnabled = true;
				//     BackBtn.tweenTarget.GetComponent<UISprite>().spriteName = normalSpName;


        if (keyNum == 0) {
            bool isAgainBtn = BackBool(curState);
            againBtn.isEnabled = isAgainBtn;
//            if (isAgainBtn == false) {
//                againBtn.tweenTarget.GetComponent<UISprite> ().spriteName = disableSpName;
//            } else {
//             
//                againBtn.tweenTarget.GetComponent<UISprite>().spriteName = normalSpName;
//            }
            if (openBox.activeInHierarchy == true) {
                int curStone = 0;
                if (curState == 1) {
                    curStone = CuNeedNum;
                } else if (curState == 2) {
                    curStone = AgNeedNum;
                } else if (curState == 3) {
                    curStone = AuNeedNum;
                }

                openKeyNumLbl.text =  "x" + curStone.ToString ();
                openBox_KeySpt.spriteName = diaSpName ; 
            }
        } else { 
            if (openBox.activeInHierarchy == true) {
                openKeyNumLbl.text =  "x" + keyNum.ToString ();
                openBox_KeySpt.spriteName = (keyspName +  curState).ToString (); 
            }
            againBtn.isEnabled = true;
        }

        this.ShowBoxState ();
        this.RefreshStateUI ();

    }

    public  void ShowBoxDoubleState (GetTreasureStateResponse resp)
    {

        if (resp != null) {
            if (resp.data.boxstatus.Length != 0) {
                doubleNum.Clear ();
                for (int i = 0; i < resp.data.boxstatus.Length; i++) {
                    doubleNum.Add (resp.data.boxstatus [i]);
                }
            }
        }
        Refresh ();
    }

    bool BackBool(int type){
        bool isAgainBtn = true;
		int curStone = Core.Data.playerManager.RTData.curStone;

		if (type == 1) {
			if (curStone < CuNeedNum)
				isAgainBtn = false;
			else
				isAgainBtn = true;
		} else if (type == 2) {
			if (curStone < AgNeedNum)
				isAgainBtn = false;
			else
				isAgainBtn = true;
		} else if (type == 3) {
			if (curStone < AuNeedNum)
				isAgainBtn = false;
			else
				isAgainBtn = true;
        }
        return isAgainBtn;
    }

    void RefreshStateUI(){
        for (int i = 0; i < doubleNum.Count; i++) {
            if (doubleNum [i] > 1) {
                lbl_double [i].transform.parent.gameObject.SetActive (true);
                lbl_double [i].text = "x" + doubleNum [i];
            } else {
                lbl_double [i].transform.parent.gameObject.SetActive (false);
            }
        }

        if (curState > 0) {
            if (doubleNum [curState - 1] > 1) {
                doubleLbl.gameObject.SetActive (true);
                doubleLbl.text = string.Format (Core.Data.stringManager.getString (7319), doubleNum [curState - 1]);
            } else {
                doubleLbl.gameObject.SetActive (false);
            }
        } 
    }

    void ShowBoxState ()
    {
        bool canClick = true;
        for (int i = 1; i <= 3; i++) {
            lblKeyNum [i - 1].text = string.Format(Core.Data.stringManager.getString(7333),Core.Data.stringManager.getString(7316-i))+"x" + Core.Data.itemManager.GetBagItemCount (basickeyId + i);
            if (Core.Data.itemManager.GetBagItemCount (basickeyId + i) == 0) {
                canClick = BackBool (i);

                btnSp [i - 1].spriteName =  diaSpName;
                btnSp[i - 1].transform.localPosition = btnIconPos;
                btnUseKey [i - 1].isEnabled = canClick;
                lblNeedStone [i - 1].gameObject.SetActive (true);
            } else {
                btnSp [i - 1].spriteName = (keyspName + i).ToString();
                btnSp[i - 1].transform.localPosition =  btnKeyPos;
                lblNeedStone [i - 1].gameObject.SetActive (false);
                btnUseKey [i - 1].isEnabled = true;

            }
        }
    }

    public void UseKey (int type)
    {
        int openType = 1;
        int keyNum = Core.Data.itemManager.GetBagItemCount (basickeyId + type);
        if (keyNum == 0) {
            bool isPass = BackBool (type);
            openType = 2;

            int curStone = 0;
            if (type == 1) {
                curStone = CuNeedNum;
            } else if (type == 2) {
                curStone = AgNeedNum;
            } else if (type == 3) {
                curStone = AuNeedNum;
            }

            openKeyNumLbl.text = "x" + curStone.ToString ();
            openBox_KeySpt.spriteName = diaSpName; 

            if (isPass == false) {
                ActivityNetController.ShowDebug (Core.Data.stringManager.getString (35006));
                this.ShowBoxState ();
                return;
            }
        } else {
            openKeyNumLbl.text =  "x" + keyNum.ToString ();
            openBox_KeySpt.spriteName = (keyspName +type).ToString (); 
        }

        if (type != 0) {
			if (type >= 1 && type - 1 < doubleNum.Count) {
				if (doubleNum [type - 1] > 1) {
					doubleLbl.gameObject.SetActive (true);
					doubleLbl.text = string.Format (Core.Data.stringManager.getString (7319), doubleNum [type - 1]);
				} else {
					doubleLbl.gameObject.SetActive (false);
				}
			} else {
				doubleLbl.gameObject.SetActive (false);
			}
        }

        againBtn.isEnabled = false;
				//  againBtn.tweenTarget.GetComponent<UISprite>().spriteName = disableSpName;
        BackBtn.isEnabled = false;
		// BackBtn.tweenTarget.GetComponent<UISprite>().spriteName = disableSpName;

        //0：Cu   // 1:Ag   //2: Au
        ActivityNetController.GetInstance ().OpenTreasureBox (type,openType);

        doubleType = doubleNum [type - 1];
    }


    //服务器返回
    public void ShowReward (ItemOfReward[] IOR)
    {
        m_data = IOR;
        root.transform.localPosition = Vector3.up*75;
        StartCoroutine (ShowRewardAni (m_data));
        this.ShowIcon ();
    }

    IEnumerator ShowRewardAni (ItemOfReward[] IOR)
    {
        openBox.SetActive (true);
        selectObj.SetActive (true);

        InitListAndReward (IOR);
        yield return new WaitForSeconds (0.8f);
        yield return StartCoroutine (ShowItemRotateAni ());
        yield return new WaitForSeconds (1.5f);
        this.ShowResult (m_data);

    }

    void InitListAndReward (ItemOfReward[] IOR)
    {

        for (int i = 0; i < ItemsObj.Count; i++) {
            int tNum = i % nameList [curState - 1].Count;
            ItemsObj [i].text = nameList [curState - 1] [tNum];
        }
	
		rewardItemLabel.text = GetStringById (IOR [0].pid);
    }

    void ShowResult (ItemOfReward[] reward)
    {

//        if (doubleNum [curState - 1] > 1 ){
//            doubleObj.SetActive (true);
//            int tId = doubleNum [curState - 1] * 100 + 40000;
//            doubleObj.GetComponentInChildren<UILabel> ().text = string.Format (Core.Data.stringManager.getString (7331), Core.Data.stringManager.getString (tId));
//        } else {
//            doubleObj.SetActive (false);
//        }

        if (doubleType > 1 ){
            doubleObj.SetActive (true);
            int tId = doubleType* 100 + 40000;
            doubleObj.GetComponentInChildren<UILabel> ().text = string.Format (Core.Data.stringManager.getString (7331), Core.Data.stringManager.getString (tId));
        } else {
            doubleObj.SetActive (false);
        }



        if (rewardObj != null) {
            if (rewardObj != null) {
                if (rewardObj.GetComponent<TweenScale> () != null) {
                    TweenScale.Begin (rewardObj, 0.2f, Vector3.one);
                } else {
                    rewardObj.AddComponent<TweenScale> ();
                    TweenScale.Begin (rewardObj, 0.2f, Vector3.one);
                }
            }
            selectObj.SetActive (false);
        }
        if (reward [0].num > 1) {
            giftNum.SetActive (true);
            giftNum.GetComponentInChildren<UILabel>().text = reward [0].num.ToString ();
        }
        virtualLbl.text = rewardItemLabel.text;
        root.transform.localPosition = Vector3.up*75;
       this.Refresh ();
    }

    IEnumerator ShowItemRotateAni ()
    {
        root.transform.localPosition = Vector3.up*75;
		Vector3 tempTargetPos = Vector3.up * 1520;//高度
        if (root.GetComponent<TweenPosition> () == null) {
            root.AddComponent<TweenPosition> ();
        } 
        TweenPosition.Begin (root, 1f, tempTargetPos);
        Core.Data.soundManager.SoundFxPlay (SoundFx.Fx_openTRBox);
        yield return null;
    }

    //关闭说明
    void OnCloseDetail ()
    {
        if (details != null) {
            if (details.GetComponent<TweenScale> () != null) {
                TweenScale.Begin (details, 0.2f, Vector3.zero);
            } else {
                details.AddComponent<TweenScale> ();
                TweenScale.Begin (details, 0.2f, Vector3.zero);
            }
        }
        for (int i = 0; i < itemTBGDespList.Count; i++) {
            Destroy (itemTBGDespList [i].gameObject);
        }
        itemTBGDespList.Clear ();
        despRoot.GetComponent<UIGrid> ().Reposition (); 
    }

    //收取
    void GotRewardClose ()
    {
        if (selectObj != null) {
            TweenScale.Begin (rewardObj, 0.2f, Vector3.zero);
            selectObj.SetActive (true);
        }
        againBtn.isEnabled = true;
				//    againBtn.tweenTarget.GetComponent<UISprite>().spriteName = normalSpName;
        BackBtn.isEnabled = true;
				//BackBtn.tweenTarget.GetComponent<UISprite>().spriteName = normalSpName;

        doubleType = doubleNum [curState - 1];

        Refresh ();
    }

    void OnCloseShowReward ()
    {
        openBox.SetActive (false);
        Refresh ();
    }

    public void ShowDetails (int type)
    {
        switch (type) {
        case 1:
            labTitle.text = string.Format(Core.Data.stringManager.getString(7332),Core.Data.stringManager.getString (7315));
            break;
        case 2:
            labTitle.text =string.Format(Core.Data.stringManager.getString(7332),Core.Data.stringManager.getString (7314));
            break;
        case 3:
            labTitle.text = string.Format(Core.Data.stringManager.getString(7332),Core.Data.stringManager.getString (7313));
            break;
        default:
            break;
        }
        if (details != null) {
            if (details.GetComponent<TweenScale> () != null) {
                TweenScale.Begin (details, 0.2f, Vector3.one);
            } else {
                details.AddComponent<TweenScale> ();
                TweenScale.Begin (details, 0.2f, Vector3.one);
            }
        }
        InitDespInfo (type);
    }

    public void InitDespInfo (int type)
    {

		int[] tGiftList = Core.Data.ActivityManager.GetRewardDataList (type).Show;
        despRoot.GetComponent<UIGrid> ().Reposition ();
		for (int i = 0; i < tGiftList.Length; i++) {
            UnityEngine.Object obj = WXLLoadPrefab.GetPrefab (WXLPrefabsName.UITBDespItem);
            if (obj != null) {
                GameObject go = Instantiate (obj) as GameObject;
                TBGiftDespItem fc = go.GetComponent<TBGiftDespItem> ();
                go.transform.parent = despRoot.transform;
                go.transform.localPosition = orginPos; 
                go.transform.localScale = Vector3.one;
                fc.id = tGiftList [i];
                itemTBGDespList.Add (fc);
            }
        }
        SpringPanel.Begin (despRoot.transform.parent.gameObject, Vector3.up * 57, 8f);
        despRoot.transform.parent.gameObject.GetComponent<SpringPanel> ().enabled = true;
        despRoot.GetComponent<UIGrid> ().Reposition ();
    }

    public void InitSelectItemInfo ()
    {
        //  Cu AG AU
        for (int i = 1; i <= Core.Data.ActivityManager.listTreasureBoxDesp.Count; i++) {
            List<string > tStr = new List<string> ();
		    int[] tGiftList = Core.Data.ActivityManager.GetRewardDataList (i).Show;
            for (int j = 0; j < tGiftList.Length; j++) {
                tStr.Add (GetStringById (tGiftList [j]));
            }
            nameList.Add (tStr);
        }

        if (nameList.Count != 0 && curState > 0)
            virtualLbl.text = nameList [curState - 1] [0];
    }

    string GetStringById (int id)
    {
        string backStr = "";
        if (id == 0)
            return backStr;
        switch (DataCore.getDataType (id)) {
        case ConfigDataType.Monster:
            backStr = Core.Data.monManager.getMonsterByNum (id).name;
            break;
        case ConfigDataType.Item:
            backStr = Core.Data.itemManager.getItemData (id).name;
            break;
        case ConfigDataType.Equip:
            backStr = Core.Data.EquipManager.getEquipConfig (id).name;
          
            break;
        case ConfigDataType.Gems:
            backStr = Core.Data.gemsManager.getGemData (id).name;
        
            break;
        case ConfigDataType.Frag:
            SoulData soul = Core.Data.soulManager.GetSoulConfigByNum (id);

            if (soul != null) {
                backStr = soul.name;
            } else {
                Debug.LogError (" not found " + id);
            }
            break;
        default:
            return backStr;
        }
        return backStr;
    }

    #region analysis icon sprite

    void ShowIcon ()
    {
        if (m_nIndex >= m_data.Length) {
            return;
        }
        ItemOfReward reward = m_data [m_nIndex];

        if (reward.getCurType () == ConfigDataType.Monster) {
            Monster data = reward.toMonster (Core.Data.monManager);
            rewardName.text = data.config.name;
            AtlasMgr.mInstance.SetHeadSprite (rewardIcon, data.num.ToString ());

        } else if (reward.getCurType () == ConfigDataType.Equip) {
            Equipment data = reward.toEquipment (Core.Data.EquipManager, Core.Data.gemsManager);
            rewardName.text = data.ConfigEquip.name;
            rewardIcon.atlas = AtlasMgr.mInstance.equipAtlas;
            rewardIcon.spriteName = data.Num.ToString ();

        } else if (reward.getCurType () == ConfigDataType.Gems) {
            Gems data = reward.toGem (Core.Data.gemsManager);
            rewardName.text = data.configData.name;
            rewardIcon.atlas = AtlasMgr.mInstance.commonAtlas;
            rewardIcon.spriteName = data.configData.anime2D;

        } else if (reward.getCurType () == ConfigDataType.Item) {
            Item item = reward.toItem (Core.Data.itemManager);
            rewardName.text = item.configData.name;
            rewardIcon.atlas = AtlasMgr.mInstance.itemAtlas;
//            rewardIcon.spriteName = item.RtData.num.ToString ();
			rewardIcon.spriteName = item.configData.iconID.ToString ();

        } else if (reward.getCurType () == ConfigDataType.Frag) {
            Soul soul = reward.toSoul (Core.Data.soulManager);
            rewardName.text = soul.m_config.name;
            rewardIcon.atlas = AtlasMgr.mInstance.itemAtlas;
            rewardIcon.spriteName = soul.m_RTData.num.ToString ();
        } else {
            RED.LogWarning ("unknow reward type");
        }
        rewardIcon.MakePixelPerfect ();

        rewardNum.text =  ItemNumLogic.setItemNum(reward.num, rewardNum , rewardNum.gameObject.transform.parent.gameObject.GetComponent<UISprite>()); // yangchenguang 
    }

    #endregion

    void AuBtn ()
    {
        btnUseKey [2].isEnabled = false;
        this.UseKey (3);
        curState = 3;

    }

    void CuBtn ()
    { 
        btnUseKey [0].isEnabled = false;
        this.UseKey (1);
        curState = 1;
       
    }

    void AgBtn ()
    {
        btnUseKey [1].isEnabled = false;
        this.UseKey (2);
        curState = 2;

    }

    void OnOpenAuDetails ()
    {
        this.ShowDetails (3);
    }

    void OnOpenAgDetails ()
    {
        this.ShowDetails (2);
    }

    void OnOpenCuDetails ()
    {
        this.ShowDetails (1);
    }
    //再来一次
    void AgainBtn ()
    {
        UseKey (curState);
    }

}
