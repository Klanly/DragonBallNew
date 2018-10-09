using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIDragonAltar : MonoBehaviour
{
	public GameObject alrarExplainObj;

	public List<AoYiSlot> aoYiSlotList = new List<AoYiSlot>();

	public GameObject selectAoYiPrb;

	public int currentEquipPos;
    public int orgAYPos = -1;

	AoYiData equipAoYiData;

	protected AoYiSlot buyAoYiSlot;

	public Transform aoYiSlotListTransform;
	
	public UISelectAoYiAlert selectAoYiAlert{get;set;}
	public void Init()
	{
		GameObject aoYiItemPrb = PrefabLoader.loadFromPack("WHY/pbUIAoYiItem") as GameObject;
        if (aoYiSlotList.Count < 5) {
            for (int i = 0; i < 5; i++) {
                GameObject aoYiItemObj = Instantiate (aoYiItemPrb) as GameObject;
                AoYiSlot aoYiSlot = aoYiItemObj.GetComponent<AoYiSlot> ();

                aoYiSlot.transform.parent = aoYiSlotListTransform;

                aoYiSlot.transform.localPosition = Vector3.zero;

                aoYiSlot.transform.localScale = Vector3.one;

                aoYiSlot.SelectedDelegate = showSelectAoYiList;

                aoYiSlot.curAoYiType = AoYiSlot.ParentType.isNormal;

                aoYiSlotList.Add (aoYiSlot);
            }
        }
	}

	void back()
	{
		Destroy(gameObject);
	}

	void alrarExplainShow()
	{
		alrarExplainObj.SetActive(true);
	}

	void alrarExplainBack()
	{
		alrarExplainObj.SetActive(false);
	}

	public void sortAoYiSlot()
	{
        float x = -370;

		List<AoYiSlot> isBuyAoYiSlot = new List<AoYiSlot>();
		List<AoYiSlot> noBuyAoYiSlot = new List<AoYiSlot>();

		for(int i = 0; i < aoYiSlotList.Count; i++)
		{
			AoYiSlot aoYiSlot = aoYiSlotList[i];

			if(aoYiSlot.lockType == AoYiSlot.LockType.NoLock)
			{
				isBuyAoYiSlot.Add(aoYiSlot);
			}
			else
			{
				noBuyAoYiSlot.Add(aoYiSlot);
			}
		}

		for(int i = 0; i < isBuyAoYiSlot.Count; i++)
		{
			AoYiSlot aoYiSlot = isBuyAoYiSlot[i];

			aoYiSlot.gameObject.transform.localPosition = new Vector3(x, -300, 0);
            x += 195;
		}

		for(int i = 0; i < noBuyAoYiSlot.Count; i++)
		{
			AoYiSlot aoYiSlot = noBuyAoYiSlot[i];

			aoYiSlot.gameObject.transform.localPosition = new Vector3(x, -300, 0);
            x += 195;
		}
	}

	public void setAoYiSlot(int index, AoYi aoYi)
	{
		if(aoYi == null)
		{
			return;
		}
		AoYiSlot aoYiSlot = aoYiSlotList[index];
        if(aoYiSlot.lockType != AoYiSlot.LockType.NoBuy )
		{
			aoYiSlot.aoYi = aoYi;
			aoYiSlot.setAoYi();
		}
	}

    //传 index 奥义槽编号  aoyi  奥义
	public void setAoYiSlot(int index, DragonLockData dragonLockData, AoYi aoYi, bool isAnimation = false)
	{
		AoYiSlot aoYiSlot = aoYiSlotList[index];

		aoYiSlot.dragonLockData = dragonLockData;

		bool isBuy = Core.Data.playerManager.RTData.aislt[dragonLockData.dragonSlot - 1] == 1;
		if(aoYi == null)
		{

			aoYiSlot.unLock();
			aoYiSlot.aoYiName.text = "";
			if(index != 0)
			{
				if(isBuy)
				{
					aoYiSlot.setAoYiSlotNoLockNoAoYi();
				}
				else
				{
					aoYiSlot.lockRoot.SetActive(true);
					aoYiSlot.priceRoot.SetActive(true);
//					aoYiSlot.lockIconAnimation(true);
					if(DragonLockData.PLAYER_LEVEL_TYPE == dragonLockData.type)
					{
						if(Core.Data.playerManager.RTData.curLevel < dragonLockData.num)
						{
							aoYiSlot.lockType = AoYiSlot.LockType.Lock;
							aoYiSlot.setInfo(Core.Data.stringManager.getString(6053) + dragonLockData.num, "common-0013", dragonLockData.price.ToString());
						}
						else
						{
							aoYiSlot.lockType = AoYiSlot.LockType.NoBuy;
							aoYiSlot.setInfo(Core.Data.stringManager.getString(6065) , "common-0013", dragonLockData.price.ToString());
						}
					}
					else if(DragonLockData.DIAMOND_TYPE == dragonLockData.type)
					{
						aoYiSlot.lockType = AoYiSlot.LockType.NoBuy;
						aoYiSlot.setInfo(Core.Data.stringManager.getString(6065), "common-0014", dragonLockData.price.ToString());
					}
				}
			}
			else if(index == 0)
			{
				aoYiSlot.setAoYiSlotNoLockNoAoYi();
			}
		}
		else
		{
            //wxl change 
            //判定此位置是否装备已装备的奥义 如果装备已装备的奥义  则自动换位；如果此位置为空，则添加当前奥义 如果这个奥义 还有存再于其他位置 这 那个位置 为空；
             if (aoYi.Pos == -1) {//未装备的奥义
                aoYiSlot.aoYi = aoYi;
                aoYiSlot.setUnLock (isAnimation);
            } else if(aoYi.Pos  != -1){//已装备的奥义
                if (aoYiSlot.aoYi != null) { //当前槽中有奥义
                    if (orgAYPos != -1) {
                        aoYiSlot.aoYi.RTAoYi.wh = orgAYPos + 1;
                        aoYiSlotList [orgAYPos].aoYi = aoYiSlot.aoYi;
                    }
                    aoYiSlot.aoYi = aoYi;
                    if (orgAYPos != -1) {
                        aoYiSlotList [orgAYPos].setUnLock (isAnimation);
                        orgAYPos = -1;
                    }
               
                    aoYiSlot.setUnLock (isAnimation);

                } 
                else {
                    //当前槽中 无奥义 
                    if(aoYi.Pos  != -1){
                        if (orgAYPos != -1) {
                            AoYi aoyi = null;
                            aoYiSlotList [orgAYPos].aoYi = aoyi;
                            //  aoYiSlotList [orgAYPos].setUnLock (false);
                            aoYiSlotList [orgAYPos].SetVoidPos ();
                            orgAYPos = -1;
                        }

                        aoYiSlot.aoYi = aoYi;
                        aoYi.RTAoYi.wh = aoYiSlot.aoYi.Pos +1;
                        aoYiSlot.setUnLock (isAnimation);

                    }
                }
            }
   

		}
	}



	void showSelectAoYiList(AoYiSlot aoYiSlot)
	{
		
		StartCoroutine(aoYiSlot.showSelectedBackground());

		int index = -1;

		for(int i = 0; i < this.aoYiSlotList.Count; i++)
		{
			AoYiSlot aoYiSlotTemp = this.aoYiSlotList[i];
			if(aoYiSlot == aoYiSlotTemp)
			{
				index = i;
				break;
			}
		}


		if(index == -1)
		{
			RED.LogError("UIDragonAltar Not AoYiSlot");
			return;
		}

		currentEquipPos = index;

		if(aoYiSlot.lockType == AoYiSlot.LockType.NoLock)
		{
			List<AoYi> noPosAoYiList = Core.Data.dragonManager.getNoPosAoYi();
			if(noPosAoYiList.Count == 0)
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(6070));
				return;
			}

			if(selectAoYiPrb == null)
			{
				selectAoYiPrb = PrefabLoader.loadFromPack("WHY/pbUISelectAoYiPanel") as GameObject;
			}

			GameObject selectAoYiAlertObj = Instantiate(selectAoYiPrb) as GameObject;
            selectAoYiAlertObj.transform.parent = DBUIController.mDBUIInstance._bottomRoot.transform;;
			selectAoYiAlertObj.transform.localPosition = Vector3.zero;
			selectAoYiAlertObj.transform.localPosition = this.transform.localPosition + Vector3.back * 10;
			selectAoYiAlertObj.transform.localScale = Vector3.one;
          
            //wxl change 
            // RED.TweenShowDialog(selectAoYiAlertObj);
			selectAoYiAlert = selectAoYiAlertObj.GetComponent<UISelectAoYiAlert>();

			selectAoYiAlert.currentSelectAoYiAlertType = UISelectAoYiAlert.SelectAoYiAlertType.SelectEquipAoYi;
			selectAoYiAlert.selectAoYiCompletedDelegate = equipAoYiCompleted;

            selectAoYiAlert.setAoYiItem(Core.Data.dragonManager.getAllAoYi(), false);

			if(aoYiSlot.aoYi != null)
			{
				selectAoYiAlert.selectedAoYi(aoYiSlot.aoYi);
			}
		}
		else if(aoYiSlot.lockType == AoYiSlot.LockType.NoBuy)
		{
			string info = "";
			if(DragonLockData.PLAYER_LEVEL_TYPE == aoYiSlot.dragonLockData.type)
			{
				info = Core.Data.stringManager.getString(6067).Replace("#", aoYiSlot.dragonLockData.price.ToString());
			}
			else if(DragonLockData.DIAMOND_TYPE == aoYiSlot.dragonLockData.type)
			{
				info = Core.Data.stringManager.getString(6068).Replace("#", aoYiSlot.dragonLockData.price.ToString());
			}

			buyAoYiSlot = aoYiSlot;

			UIInformation.GetInstance().SetInformation(
				info,
				Core.Data.stringManager.getString(6057),
				Core.Data.stringManager.getString(6069),
				buyAoYiSlotCallBack);
		}
		else if(aoYiSlot.lockType == AoYiSlot.LockType.Lock)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(6066));
		}
	}

	void buyAoYiSlotCallBack()
	{
		if(this.buyAoYiSlot == null)
		{
			return;
		}
		int mNeedMoney = this.buyAoYiSlot.dragonLockData.price;

		if(this.buyAoYiSlot.dragonLockData.type == DragonLockData.PLAYER_LEVEL_TYPE)
		{
			if(Core.Data.playerManager.Coin < mNeedMoney)
			{

				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(35000));
				return;
			}
		}
		else if(this.buyAoYiSlot.dragonLockData.type == DragonLockData.DIAMOND_TYPE)
		{
			if(Core.Data.playerManager.Stone < mNeedMoney)
			{
                SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(35006));
				return;
			}
		}
		ComLoading.Open();
		Core.Data.dragonManager.buyAoYiSlotCompletedDelegate = buyAoYiSlotCompleted;
		Core.Data.dragonManager.buyAoYiSlotRequest(this.buyAoYiSlot.dragonLockData.dragonSlot);
	}

	public void buyAoYiSlotCompleted(int slot)
	{
		Core.Data.dragonManager.buyAoYiSlotCompletedDelegate = null;
		slot = slot - 1;
		if(slot>= aoYiSlotList.Count)
		{
			return;
		}
		Core.Data.playerManager.RTData.aislt[slot] = 1;

		AoYiSlot aoYiSlot = aoYiSlotList[slot];

		aoYiSlot.lockIconAnimation(true);

		sortAoYiSlot();

        DBUIController.mDBUIInstance.RefreshUserInfoWithoutShow();
        //DBUIController.mDBUIInstance.RefreshUserInfo();
	}
    //装备奥义  changed by wxl 
	void equipAoYiCompleted(AoYiData aoYiData)
	{
		Core.Data.dragonManager.equipAoYiCompletedDelegate = equipAoYiCompleted;

		foreach(AoYi aoYi in Core.Data.dragonManager.AoYiDic.Values)
		{
			if(aoYi.AoYiDataConfig.ID == aoYiData.ID)
			{
				ComLoading.Open();
                if (aoYi.Pos != -1) {
                    orgAYPos = aoYi.Pos;
                }
				Core.Data.dragonManager.equipAoYiRequest(aoYi.ID, currentEquipPos + 1);
				break;
			}
		}
		equipAoYiData = aoYiData;
	}

	void equipAoYiCompleted(bool succ)
	{
		Core.Data.dragonManager.equipAoYiCompletedDelegate = null;

		AoYi equipAoYi = null;

		foreach(AoYi aoyiTemp in Core.Data.dragonManager.AoYiDic.Values)
		{
			if(aoyiTemp.Pos == currentEquipPos)
			{
				aoyiTemp.RTAoYi.wh = 0;
			}
			else if(aoyiTemp.AoYiDataConfig.ID == equipAoYiData.ID)
			{
				aoyiTemp.RTAoYi.wh = currentEquipPos + 1;
				equipAoYi = aoyiTemp;
			}
		}

		this.setAoYiSlot(currentEquipPos, Core.Data.dragonManager.getUnLockDragonSlotConfig(currentEquipPos), equipAoYi, true);
	}

    static public int SortByTransform (AoYiSlot a, AoYiSlot b) { 
        if (a.transform.localPosition.x >= b.transform.localPosition.x)
            return 1;
        else
            return -1;
    }

    public void ReCollectAoYiSlot(){

        aoYiSlotList = new List<AoYiSlot>();
        AoYiSlot[] aoYiSlots =  aoYiSlotListTransform.GetComponentsInChildren<AoYiSlot>();
        for(int i=0;i<aoYiSlots.Length;i++){

            aoYiSlotList.Add(aoYiSlots[i]);
        }

        aoYiSlotList.Sort(SortByTransform);

    }
}
