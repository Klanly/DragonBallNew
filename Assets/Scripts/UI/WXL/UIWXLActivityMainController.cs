using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SuperSocket.ClientEngine;

//main activity menu
public class UIWXLActivityMainController : RUIMonoBehaviour
{
    private static UIWXLActivityMainController instance;

    public static UIWXLActivityMainController Instance {
        get {
            return instance;
        }
    }

    public readonly int ItemId_fe = 1;
    public readonly int ItemId_Mon = 2;
    public readonly int ItemId_GPSGroup = 3;
	public readonly int ItemId_HappyScratch = 4;
	public readonly int ItemId_ZhuanPan =5 ;
	public readonly int ItemId_Treasure = 5;
	public readonly int ItemId_Vip = 6;
	public readonly int ItemId_SuperGift = 7;
	public readonly int ItemId_Bunk = 8;
    GameObject actItemObj;
    public GameObject groupObj;
    public List<ActItem> itemObjList;
    public static object localObj;
    public List<string> dataStr;
    public UISprite actMainBG;
    readonly int actStartLv = 20;
    readonly int gpsWarStartLv = 10;
	readonly int rollStartLv = 10;
	readonly int rollSuperGift = 10;
	readonly int happyStratch = 10;
	readonly int bankStartLv = 20;
    int actMainBgWidth = 320;
    int actMainBgHeight = 320;
	private BtnWheelCtrller m_btnWheel;	

    public void Awake ()
    {
        instance = this;
    }

    void Start ()
    {
        this.InitItem ();
    }

    void OnEnable ()
    {

    }

    public void InitItem ()
    {
        itemObjList = new List<ActItem> ();

        //活动数量
		#if Google
		float tdata = 8.0f;
		#else 
		float tdata = 7.0f;
		#endif
        actMainBG.height = actMainBgHeight;
        float tRow = tdata / 2.0f;
		actMainBG.pivot = UIWidget.Pivot.Right;
		actMainBG.width = 590;
        SpawnItem ((int)tdata);
		groupObj.transform.localPosition = new Vector3 (-300,groupObj.transform.localPosition.y,groupObj.transform.localPosition.z);
		ActivityNetController.GetInstance ().SendGetAllStatus ();
    }

    public static  UIWXLActivityMainController CreateActivityMainPanel (GameObject tLocal)
    {
        UnityEngine.Object obj = WXLLoadPrefab.GetPrefab (WXLPrefabsName.UIActivityMain);
        if (obj != null) {
            GameObject	go = Instantiate (obj)as GameObject;
            UIWXLActivityMainController activityCtrl = go.GetComponent<UIWXLActivityMainController> ();
            localObj = tLocal;
            Transform goTrans = go.transform;
            go.transform.parent = tLocal.transform;
            go.transform.localPosition = new Vector3 (-185, 0, -5);
            activityCtrl.transform.localPosition = new Vector3 (80, -30, -2);
            goTrans.localScale = Vector3.one;
            return activityCtrl;
        }
        return null;
    }

    public void SpawnItem (int num)
    {
        if (actItemObj == null) {
            UnityEngine.Object TempActItemObj = WXLLoadPrefab.GetPrefab (WXLPrefabsName.UIMainItem);
            actItemObj = TempActItemObj as GameObject;
        }
		//限制每行最大值  还是限制列


        int tRow = 0;
        if (num % 2 == 0) {
            tRow = num / 2;
        } else {
            tRow = Mathf.RoundToInt ((float)num / 2.0f);
        }
         
        for (int i = 0; i < num; i++) {
            GameObject go = Instantiate (actItemObj) as GameObject;
            go.transform.parent = groupObj.transform;
            go.transform.localScale = Vector3.one;
			int t = (int)(i/tRow);
			int tnum = i%tRow;

			go.transform.localPosition = new Vector3 (140 * tnum, -130 * t, 0);
            go.GetComponent<UIButtonMessage> ().target = gameObject;
            itemObjList.Add (go.GetComponent<ActItem> ());
            this.InitActivityItem (go, i);

        }
        this.Refresh ();
			
    }

    public void InitActivityItem (GameObject go, int num)
    {
		ActItemData itemD = null;
        switch (num) {
		case 0:
			itemD = new ActItemData (ItemId_fe,Core.Data.stringManager.getString (7203),"Act_1");
			go.GetComponent<ActItem> ().SetItemValue (itemD);
            go.GetComponent<UIButtonMessage> ().functionName = "OnBtnFestival";
            break;

        case 1:
			itemD = new ActItemData (ItemId_Mon,Core.Data.stringManager.getString (7204),"Act_2");
			go.GetComponent<ActItem> ().SetItemValue (itemD);
            go.GetComponent<UIButtonMessage> ().functionName = "OnBtnMonsterCome";
            break;
        case 2://组队 
			itemD = new ActItemData (ItemId_GPSGroup,Core.Data.stringManager.getString (7363),"Act_11");
			go.GetComponent<ActItem> ().SetItemValue (itemD);
            go.GetComponent<UIButtonMessage> ().functionName = "OnBtnGroupWar";
            break;
		case 3:
			//  刮刮乐
			itemD = new ActItemData (ItemId_HappyScratch,Core.Data.stringManager.getString (7397),"caicaikan");
			go.GetComponent<ActItem> ().SetItemValue (itemD);
			go.GetComponent<UIButtonMessage> ().functionName = "HappyScratch";

//			RoultteCtrl (go,num);

            break;
        case 4:
			itemD = new ActItemData (ItemId_Treasure,Core.Data.stringManager.getString (7311),"Act_8");
			go.GetComponent<ActItem> ().SetItemValue (itemD);
            go.GetComponent<UIButtonMessage> ().functionName = "OnBtnOpenTreasure";
            break;

        case 5:
			itemD = new ActItemData (ItemId_Vip,Core.Data.stringManager.getString (7211),"Act_9");
			go.GetComponent<ActItem> ().SetItemValue (itemD);
            go.GetComponent<UIButtonMessage> ().functionName = "OnBtnVipEnter";
            break;

		case 6:
			itemD = new ActItemData (ItemId_SuperGift,Core.Data.stringManager.getString (7396),"xingyunzhuanlun");
			go.GetComponent<ActItem> ().SetItemValue (itemD);
			go.GetComponent<UIButtonMessage> ().functionName = "RollSuperGift";
			break;
			#if Google
		case 7:
			itemD = new ActItemData (ItemId_Bunk, Core.Data.stringManager.getString (7507), "longzhuyinhang");
			go.GetComponent<ActItem> ().SetItemValue (itemD);
			go.GetComponent<UIButtonMessage> ().functionName = "DragonBank";
			break;
			#endif
        default:
            string tD2 = " 1234 " + "," + "off" + "," + "meishi";
            go.GetComponent<ActItem> ().SetItemValue (tD2);
            go.GetComponent<UIButtonMessage> ().functionName = "DonotOpen";
            break;
        }

    }

	void DragonBank(){
		if (Core.Data.playerManager.Lv < bankStartLv) {
			string tLog = string.Format (Core.Data.stringManager.getString (7320), bankStartLv.ToString ());
			SQYAlertViewMove.CreateAlertViewMove (tLog);
			return;
		}
		DBUIController.mDBUIInstance.OnBtnMainViewID(SQYMainController.CLICK_DragonBank);
		DBUIController.mDBUIInstance.HiddenFor3D_UI(true);
	}

	//记得 要改   这个  开关
	void HappyScratch(){
		if (LuaTest.Instance.OpenGuess == false) {
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(7304));
			return;
		}
		if (Core.Data.playerManager.Lv < rollStartLv) {
			string tLog = string.Format (Core.Data.stringManager.getString (7320), rollStartLv.ToString ());
			SQYAlertViewMove.CreateAlertViewMove (tLog);
			return;
		}
		DBUIController.mDBUIInstance.OnBtnMainViewID(SQYMainController.CLICK_HappyScratch);
		DBUIController.mDBUIInstance.HiddenFor3D_UI(true);
	}

	void RollSuperGift(){
		if (LuaTest.Instance.OpenLuckyWheel == false) {
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(7304));
			return;
		}
		if (Core.Data.playerManager.Lv < rollSuperGift) {
			string tLog = string.Format (Core.Data.stringManager.getString (7320), rollSuperGift.ToString ());
			SQYAlertViewMove.CreateAlertViewMove (tLog);
			return;
		}
		DBUIController.mDBUIInstance.OnBtnMainViewID(SQYMainController.CLICK_SuperGift);
		DBUIController.mDBUIInstance.HiddenFor3D_UI(true);

	}



	void RoultteCtrl(GameObject go, int num){
		go.AddComponent<BtnWheelCtrller> ();
		go.GetComponent<BtnWheelCtrller> ().m_spBtnIcon = go.GetComponent<ActItem> ().Usp;
		go.GetComponent<BtnWheelCtrller> ().lbl_name = go.GetComponent<ActItem> ().Lbl_actName;
		m_btnWheel = go.GetComponent<BtnWheelCtrller> ();

		string tName = "";
		switch (ActivityManager.activityZPID) {
		case 0:
			tName = Core.Data.stringManager.getString (7395);
			break;
		case 1:
			tName = Core.Data.stringManager.getString (7396);
			break;
		case 2:
			tName = Core.Data.stringManager.getString (7398);
			break;
		case 3:
			tName = Core.Data.stringManager.getString (7397);
			break;
		case 4:
			tName = Core.Data.stringManager.getString (7399);
			break;
		default:
			tName = Core.Data.stringManager.getString (7395);
			break;
		}
		ActItemData itemD = new ActItemData (ItemId_ZhuanPan,tName,"mingyunzhuanpan");
		go.GetComponent<ActItem> ().SetItemValue (itemD);
		go.GetComponent<UIButtonMessage> ().functionName = "OnBtnDestinyRoll";
	}


    public void DonotOpen ()
    {
        SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (7304));
    }

    public  void OnBtnFestival ()
    {
		if (LuaTest.Instance.OpenFestival == false) {
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(7304));
			return;
		}

        if (Core.Data.playerManager.Lv < actStartLv) {
            string tLog = string.Format (Core.Data.stringManager.getString (7320), actStartLv.ToString ());
            ActivityNetController.ShowDebug (tLog);
            return;
        }
        if (Core.Data.ActivityManager.GetActivityStateData (ActivityManager.festivalType) != "1") {
            ActivityNetController.ShowDebug (Core.Data.stringManager.getString (7327));
            return;
        }
    
		if (ActivityNetController.isInActivity == false) {
			ActivityNetController.ShowDebug (Core.Data.stringManager.getString (29));
			return;
		}
            
        if (ActivityNetController.isActSKTConnect == false) {
            ComLoading.Open ();
   
            ActivityNetController.curWaitState = 1;
            return;
        }
        WillToMainView (ActivityItemType.festivalItem, 1);
        DBUIController.mDBUIInstance.HiddenFor3D_UI ();
        gameObject.SetActive (false);
    }

    public void OnBtnMonsterCome ()
    {
		if (LuaTest.Instance.OpenDevil == false) {
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(7304));
			return;
		}

        if (Core.Data.playerManager.Lv < actStartLv) {
            string tLog = string.Format (Core.Data.stringManager.getString (7320), actStartLv.ToString ());
            ActivityNetController.ShowDebug (tLog);
            return;
        }
		if (ActivityNetController.isInActivity == false) {
			ActivityNetController.ShowDebug (Core.Data.stringManager.getString (29));
			return;
		}

        if (ActivityNetController.isActSKTConnect == false) {
            ComLoading.Open ();
          
            ActivityNetController.curWaitState = 2;
            return;
        }

        WillToMainView (ActivityItemType.mosterComeItem, 1);
        DBUIController.mDBUIInstance.HiddenFor3D_UI ();
        gameObject.SetActive (false);
    }

    

    public void OnBtnTaoBao ()
    {
        WillToMainView (ActivityItemType.taobaoItem, 2);
    }

    public void OnQiandao ()
    {
        WillToMainView (ActivityItemType.qiandaoItem, 2);
    }

    public void OnBtnDinner ()
    {
        WillToMainView (ActivityItemType.dinnerItem, 2);
    }

    public void OnBtnGongGao ()
    {
        WillToMainView (ActivityItemType.gonggaoItem, 2);
    }

    public void OnBtnLevelReward ()
    {
        WillToMainView (ActivityItemType.levelRewardItem, 1);
        gameObject.SetActive (false);
    }

    public void OnBtnOpenTreasure ()
    {
        WillToMainView (ActivityItemType.TreasureBoxItem, 1);
        gameObject.SetActive (false);
    }

    public void OnBtnVipEnter ()
	{
		if (LuaTest.Instance.VipAward == false) {
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(7304));
			return;
		}

        WillToMainView (ActivityItemType.vipEnter, 1);
    }

	public void OnBtnDestinyRoll(){
		this.OnBtnWheel ();
	}

    /// <summary>
    /// gps 组队战
    /// </summary>
    public void OnBtnGroupWar ()
	{
		if (LuaTest.Instance.OpenTeamPlay == false) {
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(7304));
			return;
		}

        if (Core.Data.playerManager.Lv >= gpsWarStartLv) {
            DBUIController.mDBUIInstance.HiddenFor3D_UI ();
            RadarTeamUI.OpenUI ();
        } else {

            string tLog = string.Format (Core.Data.stringManager.getString (7320), gpsWarStartLv.ToString ());
            ActivityNetController.ShowDebug (tLog);
        }
    }

	#region 命运转盘
	void OnBtnWheel()
	{
		if (LuaTest.Instance.OpenFateAward == false) {
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(7304));
			return;
		}

		if (Core.Data.playerManager.Lv < rollStartLv) {
			string tLog = string.Format (Core.Data.stringManager.getString (7320), rollStartLv.ToString ());
			SQYAlertViewMove.CreateAlertViewMove (tLog);
			return;
		}
		int actNum = m_btnWheel.actState;
		switch (actNum)
		{
		case 0:
			DBUIController.mDBUIInstance.OnBtnMainViewID(SQYMainController.CLICK_RollAct);
			break;
		case 1:
			DBUIController.mDBUIInstance.OnBtnMainViewID(SQYMainController.CLICK_SuperGift);
			break;
		case 2:
			DBUIController.mDBUIInstance.OnBtnMainViewID(SQYMainController.CLICK_GodGift);
			break;
		case 3:
			DBUIController.mDBUIInstance.OnBtnMainViewID(SQYMainController.CLICK_HappyScratch);
			break;
		case 4:
			ComLoading.Open();
			DBUIController.mDBUIInstance.HiddenFor3D_UI ();
			RadarTeamUI.OpenUI ();
			//DBUIController.mDBUIInstance.OnBtnMainViewID(SQYMainController.CLICK_RadarGroup);
			break;
		}
		DBUIController.mDBUIInstance.HiddenFor3D_UI(true);
	}


	public void UpdateWheelState(int state)
	{
		m_btnWheel.UpdateState (state);
	}


	#endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type">Type. 1 : bottom， 2: top</param>
    public void WillToMainView (ActivityItemType type, int Pos)
    {
        if (Pos == 1) {
            WXLAcitvityFactory.CreatActivity (type, localObj);
        } else {
			WXLAcitvityFactory.CreatActivity (type, (object)DBUIController.mDBUIInstance._TopRoot);   
        }
    }

    public void  OnBtnClick ()
    {
        ActivityNetController.GetInstance ().closeAllenTcp ();
        Destroy (gameObject);
    }

    /// <summary>
    /// 刷新 活动状态
    /// </summary>
    public void Refresh ()
    {
        Dictionary<int,string> tState = Core.Data.ActivityManager.actDic;
        int curLv = Core.Data.playerManager.Lv;
        if (tState == null)
            return;
        foreach (int keyNum in tState.Keys) {
            if (itemObjList.Count > keyNum) {
                string tStr = "";
                if (tState [keyNum] != null) {
                    tStr = tState [keyNum];
                } else {
                    tStr = "2";
                }
                
				if (keyNum == (int)ActivityManager.vipLibaoType) {
					if (itemObjList.Count > 5) {
						itemObjList [5].ShowState (int.Parse (Core.Data.ActivityManager.GetActivityStateData (ActivityManager.vipLibaoType)));
					}
				} else if (keyNum == (int)ActivityManager.treasureType) {
					if (itemObjList.Count > 4) {
						itemObjList [4].ShowState (int.Parse (Core.Data.ActivityManager.GetActivityStateData (ActivityManager.treasureType)));
					}
				} else {
					itemObjList [keyNum].ShowState (int.Parse (tStr));
				}

                if (curLv < actStartLv) {
                    itemObjList [0].ShowState (0);
                    itemObjList [1].ShowState (0);
                }

            }
        }
	

        if (itemObjList != null && itemObjList.Count != 0) {
            if (Core.Data.playerManager.Lv < actStartLv) {
                itemObjList [0].Usp.color = new Color (0, 0, 0, 1f);
                itemObjList [1].Usp.color = new Color (0, 0, 0, 1f);
                Core.Data.ActivityManager.SetActState (ActivityManager.festivalType,"2");
                Core.Data.ActivityManager.SetActState (ActivityManager.monsterType,"2");
            } else {
                if (tState [0] != "1") {
                    itemObjList [0].Usp.color = new Color (0, 0, 0, 1f);
                } else {
                    itemObjList [0].Usp.color = Color.white;
                }
                itemObjList [1].Usp.color = Color.white;
            }


			if (Core.Data.playerManager.Lv < rollSuperGift) {
				itemObjList [6].Usp.color = new Color (0, 0, 0, 1f);
			} else {
				itemObjList [6].Usp.color = Color.white;
			}

        }


        if (itemObjList != null && itemObjList.Count != 0) {
            Core.Data.ActivityManager.SetActState (ActivityManager.radarType,"2");
            if (Core.Data.playerManager.Lv < gpsWarStartLv) {
                itemObjList [2].Usp.color = new Color (0, 0, 0, 1f);
            } else {
                itemObjList [2].Usp.color = Color.white;
            }
        }

		if (itemObjList != null && itemObjList.Count != 0) {
			if (Core.Data.playerManager.Lv < happyStratch) {
				itemObjList [3].Usp.color = new Color (0, 0, 0, 1f);
			} else {
				itemObjList [3].Usp.color = Color.white;
			}
		}
		if (itemObjList != null && itemObjList.Count != 0) {
			if (Core.Data.playerManager.Lv < bankStartLv) {
				itemObjList [7].Usp.color = new Color (0, 0, 0, 1f);
			} else {
				itemObjList [7].Usp.color = Color.white;
			}
			itemObjList [7].ShowState (0);
		}

		this.SystemHideAct ();
		SQYMainController.mInstance.UpdateActGiftTip();

    }

	void SystemHideAct(){

		if (UIWXLActivityMainController.instance != null && itemObjList != null&& itemObjList.Count >5) {
			if (LuaTest.Instance.OpenFestival == false) {
				itemObjList [0].Usp.color = new Color (0, 0, 0, 1f);	
				itemObjList [0].ShowState (0);
				Core.Data.ActivityManager.SetActState (ActivityManager.festivalType, "2");
			} 

			if (LuaTest.Instance.OpenDevil == false) {
				itemObjList [1].Usp.color = new Color (0, 0, 0, 1f);
				itemObjList [1].ShowState (0);
				Core.Data.ActivityManager.SetActState (ActivityManager.monsterType, "2");
			} 

			if (LuaTest.Instance.OpenTeamPlay == false) {
				itemObjList [2].Usp.color = new Color (0, 0, 0, 1);
				itemObjList [2].ShowState (0);
			} else {
				if (Core.Data.playerManager.Lv < gpsWarStartLv) {
					itemObjList [2].Usp.color = new Color (0, 0, 0, 1);
				} else {
					itemObjList [2].Usp.color = Color.white;
				}
			}

			if (LuaTest.Instance.OpenGuess == false) {
				itemObjList [3].Usp.color = new Color (0, 0, 0, 1);
			} else {
				if (Core.Data.playerManager.Lv >= happyStratch)
					itemObjList [3].Usp.color = Color.white;
				else
					itemObjList [3].Usp.color = new Color (0, 0, 0, 1);
			}

			if (LuaTest.Instance.VipAward == false) {
				itemObjList [5].Usp.color = new Color (0, 0, 0, 1);
				itemObjList [5].ShowState (0);
				Core.Data.ActivityManager.SetActState (ActivityManager.vipLibaoType, "2");
			} else {
				itemObjList [5].Usp.color = Color.white;
			}

			if (LuaTest.Instance.OpenLuckyWheel == false) {
				itemObjList [6].Usp.color = new Color (0, 0, 0, 1);
				itemObjList [6].ShowState (0);
				Core.Data.ActivityManager.SetActState (ActivityManager.superGiftType, "2");
			} else {
				if (Core.Data.playerManager.Lv >= rollSuperGift)
					itemObjList [6].Usp.color = Color.white;
				else 
					itemObjList [6].Usp.color = new Color (0, 0, 0, 1);
			}
		}
	}





}
