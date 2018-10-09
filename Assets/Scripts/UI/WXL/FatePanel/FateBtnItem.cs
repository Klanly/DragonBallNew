using UnityEngine;
using System.Collections;

public class FateBtnItem : MonoBehaviour {
    public UILabel lblShop;
	public UILabel lblTitle;
	public UILabel lblChapter;
	public UIButton targetBtn;
	[HideInInspector]
    public NewFloor fData;
	public UILabel lblLock;


	void Start(){
		
		//		Debug.LogError(fData.config.ID+"  ------->  "+ fData.BelongChapterID.ToString());
        if (fData != null) {
            //跳到章节 id
            //NewChapterData  newCD = Core.Data.newDungeonsManager.ReverseToChapter(fData.ID);
			NewChapter  newCD = null;
			 if( Core.Data.newDungeonsManager.ChapterList.TryGetValue(fData.BelongChapterID,out newCD) )
			{
				
	            int ChapterId = 0;
	            if (newCD != null)
	            {
	                ChapterId = newCD.config.ID;
	                int tNum = ChapterId + 10000;
	                lblTitle.text =  string.Format(Core.Data.stringManager.getString(7323),Core.Data.stringManager.getString (tNum));
	                lblChapter.text = fData.config.name; 
	            }
	
	            lblShop.gameObject.SetActive (false);
				lblLock.gameObject.SetActive(true);
	            if(fData.config.ID > Core.Data.newDungeonsManager.lastFloorId){
					lblLock.color = Color.red;
	                lblLock.text = Core.Data.stringManager.getString(7325);
				}else{
			
					lblLock.color = Color.yellow;
	                lblLock.text = Core.Data.dungeonsManager.GetBossShowContent(fData.config.ID);
				}
			}
        } else {
            lblLock.gameObject.SetActive (false);
            lblShop.gameObject.SetActive (true);
            lblTitle.gameObject.SetActive (false);
            lblChapter.gameObject.SetActive (false);
            lblShop.text = Core.Data.stringManager.getString (7322);
        }
	}

    //跳转 按钮 
	public void OnClick(){
     
        if (fData != null)
		{
            if (fData.config.ID <= Core.Data.newDungeonsManager.lastFloorId) {
                if (DBUIController.mDBUIInstance.OpenFVE(PVEType.PVEType_Plot, fData.config.ID))
                {

                    this.GoToFloorB();
                } else{
                    ActivityNetController.ShowDebug (Core.Data.stringManager.getString (7324));
                }
            } else {
                ActivityNetController.ShowDebug (Core.Data.stringManager.getString (7324));
            }

        } 
		else 
		{
            this.GoToShop ();
        }
    }



    void GoToFloorB()
    {
        ShowFatePanelController.Instance.OnClose ();
      
		if (ShowFatePanelController.Instance.curInPanelType == ShowFatePanelController.FateInPanelType.isInSkillInfoPanel) {
			if (ShowFatePanelController.Instance.cSkillPanel != null) {
				Destroy (ShowFatePanelController.Instance.cSkillPanel.gameObject);
			}
			if (TeamUI.mInstance != null) {
//				TeamUI.mInstance.CloseUI ();
				TeamUI.mInstance.SetShow (false);
			}

			DBUIController.mDBUIInstance.HiddenFor3D_UI ();
		} else if (ShowFatePanelController.Instance.curInPanelType == ShowFatePanelController.FateInPanelType.isInMonsterInfoPanel) {
			if (MonsterInfoUI.mInstance != null) {  
				MonsterInfoUI.mInstance.OnClickClose ();
				SQYPetBoxController.mInstance.viewWillHidden ();
			}
			DBUIController.mDBUIInstance.HiddenFor3D_UI ();
		} else if (ShowFatePanelController.Instance.curInPanelType == ShowFatePanelController.FateInPanelType.isInRecruitPanel) {
			ZhaoMuUI.mInstance.SetShow (false);
			DBUIController.mDBUIInstance.HiddenFor3D_UI ();
		} else if (ShowFatePanelController.Instance.curInPanelType == ShowFatePanelController.FateInPanelType.isInBagPanel) {
			if (ShowFatePanelController.Instance.cSkillPanel != null) {
				Destroy (ShowFatePanelController.Instance.cSkillPanel.gameObject);
			}
		
			SQYPetBoxController.mInstance.viewWillHidden ();

			DBUIController.mDBUIInstance.HiddenFor3D_UI ();
		}else if(ShowFatePanelController.Instance.curInPanelType == ShowFatePanelController.FateInPanelType.isInInfoPanel){
			if (TeamUI.mInstance != null && TeamUI.mInstance.IsShow) {
				TeamUI.mInstance.SetShow (false);
			}
			UIMiniPlayerController.Instance.SetActive (true);
			DBUIController.mDBUIInstance.HiddenFor3D_UI ();
		}
    }

    void GoToShop()
	{
		if (!Core.Data.BuildingManager.ZhaoMuUnlock) 
		{
			string strText = Core.Data.stringManager.getString (9111);
			strText = string.Format (strText, RED.GetChineseNum(4));
			SQYAlertViewMove.CreateAlertViewMove (strText);
//			string strText = Core.Data.stringManager.getString (7320);
//			strText = string.Format (strText, 5);
//			SQYAlertViewMove.CreateAlertViewMove (strText);
			return;
		}

        ShowFatePanelController.Instance.OnClose ();
        if (ShowFatePanelController.Instance.cSkillPanel != null) {
            Destroy (ShowFatePanelController.Instance.cSkillPanel.gameObject);
        }

		if (ShowFatePanelController.Instance.curInPanelType == ShowFatePanelController.FateInPanelType.isInSkillInfoPanel) {
			if (TeamUI.mInstance != null && TeamUI.mInstance.IsShow) {
				TeamUI.mInstance.SetShow (false);
			}

			//   UIDragonMallMgr.GetInstance ().OpenUI (ShopItemType.Egg, ShopCallBack);
			ZhaoMuUI.OpenUI (ShowMonsterInfo);
			UIMiniPlayerController.Instance.SetActive (true);
		} else if (ShowFatePanelController.Instance.curInPanelType == ShowFatePanelController.FateInPanelType.isInMonsterInfoPanel) {
			if (MonsterInfoUI.mInstance != null) {  
				MonsterInfoUI.mInstance.OnClickClose ();
				if (SQYPetBoxController.mInstance != null) {
					SQYPetBoxController.mInstance.viewWillHidden ();
				}
			}

			// UIDragonMallMgr.GetInstance ().OpenUI (ShopItemType.Egg, ShowBagCallBack);
			ZhaoMuUI.OpenUI (ShowBagCallBack);
			UIMiniPlayerController.Instance.SetActive (true);
		} else if (ShowFatePanelController.Instance.curInPanelType == ShowFatePanelController.FateInPanelType.isInRecruitPanel) {
			ZhaoMuUI.mInstance.SetShow (false);
			if (MonsterInfoUI.mInstance != null) {  
				MonsterInfoUI.mInstance.OnClickClose ();
			}
			//   UIDragonMallMgr.GetInstance ().OpenUI (ShopItemType.Egg, RecruitCallBack);
			UIMiniPlayerController.Instance.SetActive (true);
		} else if (ShowFatePanelController.Instance.curInPanelType == ShowFatePanelController.FateInPanelType.isInBagPanel) {
			//	UIDragonMallMgr.GetInstance ().OpenUI (ShopItemType.Egg,ShowBagCallBack );
			ZhaoMuUI.OpenUI (ShowBagCallBack);
			UIMiniPlayerController.Instance.SetActive (true);
			SQYPetBoxController.mInstance.viewWillHidden ();

			DBUIController.mDBUIInstance.HiddenFor3D_UI ();
		} else if(ShowFatePanelController.Instance.curInPanelType == ShowFatePanelController.FateInPanelType.isInInfoPanel){
			if (TeamUI.mInstance != null && TeamUI.mInstance.IsShow) {
				TeamUI.mInstance.SetShow (false);
			}
			ZhaoMuUI.OpenUI (ShowMonsterInfo);
			UIMiniPlayerController.Instance.SetActive (true);

		}
    }


	void ShowMonsterInfo(){
		TeamUI.mInstance.SetShow (true);
		UIMiniPlayerController.Instance.SetActive (false);
	}

    //商城 回调
    void ShopCallBack()
	{

        DBUIController.mDBUIInstance.SetViewState(RUIType.EMViewState.S_Team_NoSelect);       
    }

	 
        
    //背包 回调
    void ShowBagCallBack()
    {
        UIMiniPlayerController.Instance.SetActive (true);
        SQYPetBoxController.mInstance.viewWillShow ();
    
    }

    void RecruitCallBack(){
        ZhaoMuUI.mInstance.SetShow (true);

    }

    IEnumerator SetShowMiniBar(){
        yield return new WaitForSeconds (0.2f);
        UIMiniPlayerController.Instance.SetActive (true);
    }
        

    public static FateBtnItem CreatFateBtnItem(Transform root,NewFloor data,Vector3 pos){
		UnityEngine.Object obj =WXLLoadPrefab.GetPrefab(WXLPrefabsName.UIFateBtnItem);
		if (obj != null) {
			GameObject go = Instantiate(obj) as GameObject;
			FateBtnItem uiH = go.GetComponent<FateBtnItem> ();
			uiH.fData = data;
			go.transform.parent = root;
            go.transform.localScale = Vector3.one ;
			go.transform.localPosition = pos;
			return uiH; 
		}
		return null;
	}



//	void GetLeftTimes(){
//		Floor tFloor = Core.Data.dungeonsManager.getFloor(fData.ID);
//	}
}