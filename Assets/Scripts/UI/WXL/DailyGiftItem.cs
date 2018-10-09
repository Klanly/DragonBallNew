using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DailyGiftItem : MonoBehaviour,IItem {
 
    DailyGiftItemClass myData;
    public UISprite spIcon;
    public UILabel lblName;
    public UILabel lblDesp;
    public DailyGiftItemClass.dailyItemType curType;
    public GameObject dinnerObj;
    public GameObject giftObj;
    public UIButton btnDinner;
    public UIButton btnGet;
    public UIButton getVipBtn;
    public UILabel dinnerTime;
    public UILabel lblCanGet;

    private const string s_canGet = "deng-001";
    private const string s_canNotGet = "deng-002";

    public List<GiftItem> giftItemList;

    public void SetItemValue (object obj)
    {
        myData = obj as DailyGiftItemClass;
        if (myData != null) {
            this.Refresh ();
        }
    }

    public object ReturnValue ()
    {
        return myData;
    }


    public void Refresh(){
        if(myData != null){
            curType = myData.curItemType;
            lblDesp.text = Core.Data.stringManager.getString (7376);
            UISprite sp_Btn =  btnGet.GetComponent<UISprite >();
            getVipBtn.gameObject.SetActive (false);
            lblCanGet.gameObject.SetActive (false);

            switch (curType)
            {
			case DailyGiftItemClass.dailyItemType.dinnerType:
				spIcon.spriteName = "meishi";
				dinnerObj.SetActive (true);
				giftObj.SetActive (false);
				lblName.text = Core.Data.stringManager.getString (7378);
				btnGet.gameObject.SetActive (false);
				btnDinner.gameObject.SetActive (true);
				if (myData.canGet == true) {
					btnGet.isEnabled = true;
					btnDinner.isEnabled = true;
					dinnerTime.gameObject.SetActive (false);
				} else {
					dinnerTime.gameObject.SetActive (true);
					btnDinner.isEnabled = false;
				}
				UISprite tSp = dinnerObj.GetComponentInChildren<UISprite> ();
                    if (myData.id == 1)
					{	
						if(tSp!= null)
						tSp.spriteName = "chilamian-3";
                    }
                    else
					{	
						if(tSp!= null)
						tSp.spriteName = "chilamian-1";
                    }
                    int[] tNum = Core.Data.ActivityManager.GetDinnerTimeShow();
                    if(tNum != null)
                        dinnerTime.text = string.Format(Core.Data.stringManager.getString(7383), tNum[0].ToString() + ":00", tNum[1].ToString() + ":00");

                lblDesp.text = Core.Data.stringManager.getString (7377);
                lblDesp.color = new Color (1f,0,1f,1f);
                break;
            case DailyGiftItemClass.dailyItemType.sevenGiftType:
                spIcon.spriteName = "qirilibao";
                dinnerObj.SetActive (false);
                giftObj.SetActive (true);
                btnGet.gameObject.SetActive (true);
     
                string sNum =string.Format(Core.Data.stringManager.getString (7382), Core.Data.stringManager.getString (40000+myData.id*100));
                lblName.text = sNum;

                if(myData.canGet == true){    
                    sp_Btn.spriteName = s_canGet;
                    btnGet.isEnabled = true;
                }else{
                    lblCanGet.gameObject.SetActive (true);
                    lblCanGet.text = Core.Data.stringManager.getString (7384);
                    sp_Btn.gameObject.SetActive (false);

                }
                btnDinner.gameObject.SetActive (false);
                break;
			case DailyGiftItemClass.dailyItemType.monthGiftType:
				spIcon.spriteName = "shop-1012";
				dinnerObj.SetActive (false);
				giftObj.SetActive (true);
				btnGet.gameObject.SetActive (true);
				lblName.text = Core.Data.stringManager.getString (7379);
				btnDinner.gameObject.SetActive (false);

				if (myData.id == 1) {//购买过 
					if (myData.canGet) {
						sp_Btn.gameObject.SetActive (true);
						sp_Btn.spriteName = s_canGet;
						lblCanGet.gameObject.SetActive (true);
						lblCanGet.text = string.Format( Core.Data.stringManager.getString (7408),myData.otherPara.ToString());
						getVipBtn.gameObject.SetActive (false);
					} else {
						sp_Btn.gameObject.SetActive (true);
						sp_Btn.spriteName = s_canNotGet;
						lblCanGet.gameObject.SetActive (true);
						lblCanGet.text = string.Format( Core.Data.stringManager.getString (7408),myData.otherPara.ToString());
						getVipBtn.gameObject.SetActive (false);
					}
				} else if (myData.id == 2) { //未购买
					lblCanGet.gameObject.SetActive (true);
					lblCanGet.text = Core.Data.stringManager.getString (7384);
					getVipBtn.gameObject.SetActive (true);
					sp_Btn.gameObject.SetActive (false);
				}
					
                break;
            case DailyGiftItemClass.dailyItemType.vipGiftType:
                spIcon.spriteName = "viplongzhu";
                dinnerObj.SetActive (false);
                giftObj.SetActive (true);
                lblName.text = Core.Data.stringManager.getString (7381);
              
                btnGet.gameObject.SetActive (true);
                if (myData.canGet) {
                    sp_Btn.spriteName = s_canGet;
                } else {
                    sp_Btn.spriteName = s_canNotGet;
                }
                btnDinner.gameObject.SetActive (false);
                break;

            case DailyGiftItemClass.dailyItemType.levelGiftType:
                spIcon.spriteName = "dengjijiangli";
                dinnerObj.SetActive (false);
                giftObj.SetActive (true);
                lblName.text = string.Format( Core.Data.stringManager.getString (7380),myData.id.ToString()+ Core.Data.stringManager.getString(7147));
                btnGet.gameObject.SetActive (true);
                if (myData.canGet) {
                    sp_Btn.spriteName = s_canGet;
                    btnGet.isEnabled = true;
                } else {
                    lblCanGet.gameObject.SetActive (true);
                    lblCanGet.text = Core.Data.stringManager.getString (7384);
                    sp_Btn.gameObject.SetActive (false);
                }
                btnDinner.gameObject.SetActive (false);
                break;
            default:
                break;
            }
            if (myData.giftReward != null) {
                SetGift (myData.giftReward.ToArray ());
            } else {
                giftObj.SetActive (false);
            }
        }

    }


    void OnClickGet(){
		if (myData.canGet == true) {
			switch (curType) {
			case DailyGiftItemClass.dailyItemType.dinnerType:
				ActivityNetController.GetInstance ().EatDinnerRequest ();
				btnDinner.gameObject.SetActive (false);
				btnGet.isEnabled = false;
				break;
			case DailyGiftItemClass.dailyItemType.levelGiftType:
				ActivityNetController.GetInstance ().GotLevelGiftRequest (myData.id);
				btnGet.isEnabled = false;
				break;
			case DailyGiftItemClass.dailyItemType.monthGiftType:
                //要判断   是否是第一次充值  
				ActivityNetController.GetInstance ().GetMonthGiftRequest ();
				Core.Data.rechargeDataMgr._RechargeCallback = CallBack;
				//DailyGiftController.Instance.OnClickBack ();
				break;
			case DailyGiftItemClass.dailyItemType.vipGiftType:
				ActivityNetController.GetInstance ().GetVipGift ();
				break;
			default:
				break;
			}
		} else {
			if (curType == DailyGiftItemClass.dailyItemType.monthGiftType) {
				if (myData.id == 1) {//购买过
					MonthGiftData  tMonthD =  Core.Data.ActivityManager.GetMonthStateData ();
					if (tMonthD!= null && tMonthD.canGain != 1 && tMonthD.lastDay >0) {
						ActivityNetController.ShowDebug (Core.Data.stringManager.getString(32202));
					}
				}else if(myData.id ==2){//未购买
					UIDragonMallMgr.GetInstance ().SetRechargeMainPanelActive ();
					Core.Data.rechargeDataMgr._RechargeCallback = CallBack;
					GetGiftPanelController.Instance.BtnBack ();
				}
			}
		}
    }
	void CallBack(){
		ActivityManager.curGetGiftType = 3;
		GetGiftPanelController.CreateUIRewardPanel(DBUIController.mDBUIInstance._bottomRoot);
	}


    void SetGift(ItemOfReward[] gifts){
        if (gifts != null) {
            if (giftObj.activeSelf == true) {
                for (int i = 0; i < giftItemList.Count; i++) {
                    if (i < gifts.Length) {
                        giftItemList [i].gameObject.SetActive (true);
                        giftItemList [i].SetItemValue (gifts [i]);
                    } else {
                        giftItemList [i].gameObject.SetActive (false);
                    }
                }
            }
        } else {
            giftObj.SetActive (false);
        }
    }

}
