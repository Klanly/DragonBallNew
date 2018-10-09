using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UISelectAoYiAlert : MonoBehaviour {

	public UIGrid aoYiGrid;

	public GameObject aoYiItemPrb;

	public BoxCollider selectBoxCollider;

	public UILabel aoYiDesLabel;

    public GameObject NMKXDragon;
    public GameObject earthDragon;
    public UIButton selectBtn;

    public UIButton closeBtn;
//    Vector3 rtPos = new Vector3(523f,221f,0f);
//    Vector3 rmPos = new Vector3(523f,-150f,0f);

    public DragonManager.DragonType curType;
	public enum SelectAoYiAlertType
	{
		SelectLearnAoYi,
		SelectEquipAoYi,
		None,
	}


    public  GameObject dialogObj;
	public SelectAoYiAlertType curAlertType;
    public UILabel lblAoyiDesp;
    public UILabel lblDragonSay;
	
	public List<AoYiSlot> List_AoYiSlots = new List<AoYiSlot>();
	
	
    public void ShiftDragon(DragonManager.DragonType dragonType){
        if (curAlertType != SelectAoYiAlertType.SelectLearnAoYi || NMKXDragon == null || earthDragon == null)
				return;
	
        //        Debug.Log(dragonType);
        if (dragonType == DragonManager.DragonType.EarthDragon)
        {
            earthDragon.SetActive(true);
            NMKXDragon.SetActive(false);
        }
        else if (dragonType == DragonManager.DragonType.NMKXDragon)
        {
            earthDragon.SetActive(false);
            NMKXDragon.SetActive(true);
        }
        else
        {
            earthDragon.SetActive(true);
            NMKXDragon.SetActive(false);
        }
    }


    public IEnumerator RefreshDiaObj(bool isSelect = false){ 
        if (curAlertType == SelectAoYiAlertType.SelectLearnAoYi)
        {  
            if (lblDragonSay != null)
            {
                dialogObj.transform.localScale = Vector3.zero;
                if (isSelect == false)
                    yield return new WaitForSeconds(3f);
                else
                    yield return new WaitForSeconds(0.5f);

                if (dialogObj.GetComponent<TweenScale>() != null)
                    TweenScale.Begin(dialogObj, 0.3f, Vector3.one);
                else
                {
                    dialogObj.AddComponent<TweenScale>();
                    TweenScale.Begin(dialogObj, 0.3f, Vector3.one);
					if(Core.Data.guideManger.isGuiding)
					Core.Data.guideManger.DelayAutoRun(6.5f);
                }
                if (isSelect == false)
                {
                    lblDragonSay.text = Core.Data.stringManager.getString(6041);
                    //  selectBtn.gameObject.SetActive(false);
                    selectBtn.isEnabled = false;

                }
                else
                {
                    lblDragonSay.text = Core.Data.stringManager.getString(6123);
                    if (selectBtn != null)
                        selectBtn.isEnabled = true;
                    //selectBtn.gameObject.SetActive(true);
                }
            }
            if(closeBtn!=null)
                closeBtn.gameObject.SetActive(false);
        }
        else
        {
            if(selectBtn !=null)
            selectBtn.isEnabled = false;
        }
    }

	void Start()
	{
        ShiftDragon(curType);
        if(closeBtn != null)
            closeBtn.gameObject.SetActive(true);

        StartCoroutine(RefreshDiaObj(true));
		if(Core.Data.guideManger.isGuiding && gameObject.name == "pbUICallDragonSucceed(Clone)")
			Core.Data.guideManger.AutoRUN();
        if (List_AoYiSlots != null && List_AoYiSlots.Count != 0) {

            selectedAoYiSlot (List_AoYiSlots [0]);
        }

        
	}
	
	public Action<AoYiData> selectAoYiCompletedDelegate;

	public SelectAoYiAlertType currentSelectAoYiAlertType = SelectAoYiAlertType.None;

    public void setAoYiItem(List<AoYi> aoYiList, bool isDragonType, DragonManager.DragonType successdType = DragonManager.DragonType.None)
	{
		if(aoYiItemPrb == null)
		{
			aoYiItemPrb = PrefabLoader.loadFromPack("WHY/pbUIAoYiItem") as GameObject;
		}

		while(this.aoYiGrid.transform.childCount > 0 )
		{
			GameObject g =  this.aoYiGrid.transform.GetChild(0).gameObject;
			g.transform.parent = null;
			Destroy(g);
		}


        //------------  把未解锁的奥义放到后面 -------------
		List<AoYi> aoYiListTemp = new List<AoYi>();
        foreach(AoYi aoYi in aoYiList) {
            short dragonType = aoYi.AoYiDataConfig.dragonType;

            if(aoYi.AoYiDataConfig.unlockLevel > Core.Data.dragonManager.DragonList[dragonType - 1].RTData.lv)
			{
				aoYiListTemp.Add(aoYi);
			}
		}

        foreach(AoYi aoYi in aoYiListTemp) {
			aoYiList.Remove(aoYi);
			int index = aoYiList.Count;
			aoYiList.Insert(index, aoYi);
		}
        //------------ End 把未解锁的奥义放到后面 -------------


		foreach(AoYi aoYi in aoYiList)
        {

            short dragonType = aoYi.AoYiDataConfig.dragonType;

            if(isDragonType && dragonType!= ((short)successdType + 1) )
			{
				continue;
			}

			GameObject aoYiItemObj = Instantiate(aoYiItemPrb) as GameObject;
			AoYiSlot aoYiSlot = aoYiItemObj.GetComponent<AoYiSlot>();
			List_AoYiSlots.Add(aoYiSlot);
			
			aoYiSlot.transform.parent = aoYiGrid.transform;

			aoYiSlot.transform.localPosition = Vector3.zero;

			aoYiSlot.transform.localScale = Vector3.one;

			aoYiSlot.aoYi = aoYi;

            if (this.currentSelectAoYiAlertType == SelectAoYiAlertType.SelectLearnAoYi)
                aoYiSlot.curAoYiType = AoYiSlot.ParentType.isLearnAoYi;
            else if (this.currentSelectAoYiAlertType == SelectAoYiAlertType.SelectEquipAoYi)
                aoYiSlot.curAoYiType = AoYiSlot.ParentType.isEquipAoYi;

			aoYiSlot.SelectedDelegate = selectedAoYiSlot;

            if(/*aoYi.AoYiDataConfig.unlockLevel <= Core.Data.dragonManager.DragonList[dragonType - 1].RTData.lv &&*/ aoYi.ID != 0) // 已经学习解锁
			{
				aoYiSlot.setUnLock(false);
				if(aoYi.Pos != -1)
				{
					aoYiSlot.stateRoot.SetActive(true);
					aoYiSlot.stateIcon.gameObject.SetActive(false);
					aoYiSlot.stateInfo.text = Core.Data.stringManager.getString(6111);
				}
			}
			else
			{
				aoYiSlot.aoYiIcon.atlas = aoYiSlot.aoYiAtlas;
				aoYiSlot.aoYiIcon.spriteName = aoYi.AoYiDataConfig.ID.ToString();
				//aoYiSlot.aoYiIcon.MakePixelPerfect();
				aoYiSlot.aoYiIcon.color = Color.gray;

				aoYiSlot.aoYiName.text = aoYi.AoYiDataConfig.name;

                if(/*aoYi.AoYiDataConfig.unlockLevel <= Core.Data.dragonManager.DragonList[dragonType - 1].RTData.lv && */aoYi.ID == 0) // 没学习   add by wxl 
				{
					aoYiSlot.lockRoot.SetActive(false);
					aoYiSlot.stateRoot.SetActive(true);
					aoYiSlot.stateIcon.gameObject.SetActive(false);
					aoYiSlot.stateInfo.text = Core.Data.stringManager.getString(6112);

				}
//                else if(aoYiSlot.aoYi.AoYiDataConfig.unlockLevel > Core.Data.dragonManager.DragonList[dragonType - 1].RTData.lv && aoYiSlot.aoYi.ID == 0) // 没解锁    解锁功能  取消    by wxl
//				{
//					aoYiSlot.lockRoot.SetActive(true);
//					aoYiSlot.info.text = Core.Data.stringManager.getString(6099).Replace("#", aoYi.AoYiDataConfig.unlockLevel.ToString());
//				}
			}
		}

		aoYiGrid.Reposition();
//		aoYiGrid.GetComponentInParent<UIGrid> ().repositionNow = true;
		SpringPanel.Begin (aoYiGrid.transform.parent.gameObject,new Vector3(-451,-279,0),13);

	}

	public void selectedAoYi(AoYi aoYi)
	{
		AoYiSlot[] aoYiSlots =  aoYiGrid.GetComponentsInChildren<AoYiSlot>();
		for(int i = 0; i < aoYiSlots.Length; i++)
		{
			AoYiSlot aoYiSlotTemp = aoYiSlots[i];
			if(aoYi.AoYiDataConfig.ID == aoYiSlotTemp.aoYi.AoYiDataConfig.ID)
			{
				selectedAoYiSlot(aoYiSlotTemp);
				return;
			}
		}
	}

    public void selectedAoYiSlot(AoYiSlot aoYiSlot)
    {
        //        Debug.Log("---------------------------selectedAoYiSlot------------------------");
		AoYiSlot[] aoYiSlots =  aoYiGrid.GetComponentsInChildren<AoYiSlot>();
        string dragonType = "";
		for(int i = 0; i < aoYiSlots.Length; i++)
		{
			AoYiSlot aoYiSlotTemp = aoYiSlots[i];
            //Debug.Log(aoYiSlotTemp.aoYiName.text +"        "+aoYiSlot.aoYiName.text);
			if(aoYiSlotTemp.selectedBackground.activeSelf && aoYiSlotTemp != aoYiSlot)
			{
				aoYiSlotTemp.selectedBackground.SetActive(false);
			}
			else if(aoYiSlotTemp == aoYiSlot)
			{
				aoYiSlotTemp.selectedBackground.SetActive(true);
                StartCoroutine( RefreshDiaObj(true));
			}
		}
        if (lblAoyiDesp != null) {
            int index = 0;
            if(aoYiSlot.aoYi.RTAoYi != null)
            {
                index = aoYiSlot.aoYi.RTAoYi.lv - 1;
                if(index < 0)
                {
                    index = 0;
                }
            }

            float[] formatList = aoYiSlot.aoYi.AoYiDataConfig.full_effect(index);

            object[] format = new object[formatList.Length];
            for(int i = 0; i < formatList.Length; i++)
            {
                format[i] = formatList[i];
            }
          
            lblAoyiDesp.text =aoYiSlot.aoYi.AoYiDataConfig.name +": " + string.Format(aoYiSlot.aoYi.AoYiDataConfig.description, format);
        }
       
		if(aoYiDesLabel != null)
		{
			int index = 0;
			if(aoYiSlot.aoYi.RTAoYi != null)
			{
				index = aoYiSlot.aoYi.RTAoYi.lv - 1;
				if(index < 0)
				{
					index = 0;
				}
			}

            float[] formatList = aoYiSlot.aoYi.AoYiDataConfig.full_effect(index);

			object[] format = new object[formatList.Length];
			for(int i = 0; i < formatList.Length; i++)
			{
				format[i] = formatList[i];
			}
            if (aoYiSlot.aoYi.AoYiDataConfig.dragonType == 1) { //地球
                dragonType = Core.Data.stringManager.getString (7374);
            } else {
                dragonType = Core.Data.stringManager.getString (7375);
            }
            aoYiDesLabel.text = Core.Data.stringManager.getString(6113)+dragonType+": "+string.Format(aoYiSlot.aoYi.AoYiDataConfig.description, format);
            //             aoYiDesLabel.text = Core.Data.stringManager.getString(6113) + string.Format(aoYiSlot.aoYi.AoYiDataConfig.description, format);
		}
	}

	void OnClose()
	{
		Destroy(gameObject);
	}

	public void OnSelectAoYi()
	{
		bool noSelect = true;

		AoYiSlot[] aoYiSlots =  aoYiGrid.GetComponentsInChildren<AoYiSlot>();

		foreach(AoYiSlot aoYiSlot in aoYiSlots)
		{

			if(aoYiSlot.selectedBackground.activeSelf)
			{
                // short dragonType = aoYiSlot.aoYi.AoYiDataConfig.dragonType;

				if(this.currentSelectAoYiAlertType == SelectAoYiAlertType.SelectLearnAoYi)
				{
                    //                    if(aoYiSlot.aoYi.AoYiDataConfig.unlockLevel > Core.Data.dragonManager.DragonList[dragonType - 1].RTData.lv && aoYiSlot.aoYi.ID == 0) // 没解锁   add by wxl
//					{
//						SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(6110));
//					}
//                    else 
                    //       if(aoYiSlot.aoYi.AoYiDataConfig.unlockLevel <= Core.Data.dragonManager.DragonList[dragonType - 1].RTData.lv)
                    //	{
						ComLoading.Open();
						Core.Data.dragonManager.learnAoYiRequest(aoYiSlot.aoYi.AoYiDataConfig.ID.ToString());
						Destroy(gameObject);
                    //	}
				}
				else if(this.currentSelectAoYiAlertType == SelectAoYiAlertType.SelectEquipAoYi)
				{
                    if(/*aoYiSlot.aoYi.AoYiDataConfig.unlockLevel <= Core.Data.dragonManager.DragonList[dragonType - 1].RTData.lv && */aoYiSlot.aoYi.ID == 0) // 没学习
					{
						SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(6109));
					}
                    //                    else if(aoYiSlot.aoYi.AoYiDataConfig.unlockLevel > Core.Data.dragonManager.DragonList[dragonType - 1].RTData.lv && aoYiSlot.aoYi.ID == 0) // 没解锁    add by  wxl 
//					{
//						SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(6110));
//					}
                    //wxl changed 
//					else if(aoYiSlot.aoYi.Pos != -1)
//					{
//						SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(34016));
//					}
					else if(aoYiSlot.aoYi.ID != 0)
					{
						if(selectAoYiCompletedDelegate != null)
						{
							selectAoYiCompletedDelegate(aoYiSlot.aoYi.AoYiDataConfig);
						}
						Destroy(gameObject);
					}
				}
				noSelect = false;
				break;
			}
		}

		if(noSelect)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(6041));
		}
	}

	void OnExplainAoYiClick()
	{
		UIExplainDialog explainDialog = UIExplainDialog.getExplainDialog(DBUIController.mDBUIInstance._bottomRoot.transform);
		explainDialog.titleLabel.text = Core.Data.stringManager.getString(6107);
		explainDialog.contentLabel.text = Core.Data.stringManager.getString(6108);
	}
}
