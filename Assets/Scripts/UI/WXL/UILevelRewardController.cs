using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 等级奖励的界面
/// </summary>
public class UILevelRewardController : RUIMonoBehaviour
{
	private static UILevelRewardController instance;

	public static UILevelRewardController Instance {
		get {
			return instance;
		}
	}
		
    public List<LevelRewardCollection> RewardColList;
	public GameObject itemGroupObj;
	public const int colNum = 24;
    private int tempTargetNum = 27;
	protected int maxLvLimit;
    public List<ItemOfReward> temperRewardList;
	System.Action CallBack = null;
    int offsetX = -412;
	

	public static UILevelRewardController CreateUILevelRewardPanel(ActivityItemType type, GameObject tObj, System.Action callback = null)
	{
		UnityEngine.Object obj = WXLLoadPrefab.GetPrefab(WXLPrefabsName.UILevelRewardPanel);
		if (obj != null) {
			GameObject go = Instantiate(obj) as GameObject;
			UILevelRewardController fc = go.GetComponent<UILevelRewardController>();
			Transform goTrans = go.transform;
			go.transform.parent = tObj.transform;
			go.transform.localPosition = Vector3.zero;
			goTrans.localScale = Vector3.one;
			if(fc !=null )
				fc.CallBack = callback;
			
			return fc;
		}
		return null;		
	}


	void Awake()
	{
        instance = this;
        RewardColList = new List<LevelRewardCollection>();
        temperRewardList = new List<ItemOfReward> ();
        DBUIController.mDBUIInstance.HiddenFor3D_UI();
        //请求签到状态  0  
        ActivityNetController.GetInstance().LevelGiftStateRequest(0);
        this.InitData ();
		itemGroupObj.GetComponent<UIGrid>().repositionNow = true;
	}
 


	/// <summary>
	/// 初始化数据
	/// </summary>
	public void  InitData()	
	{
        maxLvLimit = (int)Core.Data.playerManager.Lv / 5; 
        for (int i = 0; i < colNum; i++){
            if (RewardColList.Count == 0)
            {
                SpawnItem();
                this.RefreshItemState(i, maxLvLimit);
            }
            else
            {
                this.RefreshItemState(i,maxLvLimit);
            }
            RewardColList[i].SetItemValue(Core.Data.ActivityManager.GetLeveRewardDataList()[i]);
        }
        Invoke ("AutoMoveToCanGet",0.2f);
       
	}

    public void  RefreshItemState(int num ,int tLimit){   
		List<int> levelRewardList = Core.Data.ActivityManager.GetGotLvReward ();
        for (int j = 0; j < colNum; j++)
        {
            if (levelRewardList != null)//领过奖励 
            { 
                if (num < tLimit)  //解锁 可以领
                {
                    if (levelRewardList.Contains (RewardColList [num].levelNum))
                        RewardColList [num].curLvColState = LevelRewardCollection.CollectionState.GotReward;
                    else {
                        RewardColList [num].curLvColState = LevelRewardCollection.CollectionState.UnlockReward;
                    
                    }
                }
                else
                {
                    RewardColList[num].curLvColState = LevelRewardCollection.CollectionState.LockReward;
                }
            }
            else
            {// 从未领过  可以开启 
                if (num < tLimit) {
                    RewardColList [num].curLvColState = LevelRewardCollection.CollectionState.UnlockReward;
             
                }
                else
                    RewardColList[num].curLvColState = LevelRewardCollection.CollectionState.LockReward;
            }
        }

    }


	public void SpawnItem()
	{
        if (RewardColList != null && RewardColList.Count == colNum) {
			return;
		}
		UnityEngine.Object ItemObj = WXLLoadPrefab.GetPrefab(WXLPrefabsName.UIColItem);
		for (int i = 0; i < colNum; i++) {
			GameObject tObj = Instantiate(ItemObj)as GameObject;
			tObj.transform.localScale = Vector3.one;
			tObj.transform.parent = itemGroupObj.transform;
            int tX = i * 250 - 400;
			tObj.transform.localPosition = new Vector3(tX, -25, 0);
            RewardColList.Add(tObj.GetComponent<LevelRewardCollection>());
		}
	}

	public void Refresh()
	{
		this.InitData();
	}

	public void OnBtnBack()
	{
		if(CallBack != null) 
		{
			CallBack();
		}
		else
		{
	        DBUIController.mDBUIInstance.ShowFor2D_UI();
			if (UIWXLActivityMainController.Instance != null)
	        {
	
				if (UIWXLActivityMainController.Instance.gameObject.activeInHierarchy == false)
					UIWXLActivityMainController.Instance.SetActive(true);
	
	            UIWXLActivityMainController.Instance.Refresh ();
	        }
		}
        Destroy(gameObject);
	}

   
    /// <summary>
    /// 返回领取等级奖励 
    /// </summary>
    public void BackOnGotReward (ItemOfReward[] tItemList,int level){
        //刷新 状态
        maxLvLimit = (int)Core.Data.playerManager.Lv / 5; 
       
        for (int i = 0; i < colNum; i++)
        {
            this.RefreshItemState(i, maxLvLimit);
        }
        //显示奖励
        ShowGetGift(tItemList,level);
        this.Refresh();
    }

    public void ShowGetGift(ItemOfReward[] Reward,int lv){
        string tTitle = string.Format (Core.Data.stringManager.getString(7318),lv.ToString());
        GetRewardSucUI.OpenUI(Reward, tTitle);
    }

    //自动转到 可领取
    public void AutoMoveToCanGet(){
        tempTargetNum = this.CheckNew ();

        if (tempTargetNum > 0 || tempTargetNum < 20) {
            Vector3 targetPos = new Vector3 (-250 * tempTargetNum, 0, 0);
            SpringPanel.Begin (itemGroupObj.transform.parent.gameObject, targetPos + Vector3.right * offsetX, 8);
            itemGroupObj.transform.parent.GetComponent<SpringPanel> ().enabled = true;
        }
    }

    int CheckNew(){
        tempTargetNum = 27;
		List<int> gotList = Core.Data.ActivityManager.GetGotLvReward ();
        int temp = Core.Data.playerManager.Lv / 5;
        if (gotList != null) {
            for (int i = 0; i < temp; i++) {
                if (!gotList.Contains ((i + 1) * 5)) {
                    if (i < tempTargetNum) {
                        tempTargetNum = i;
                    }
                }
            }
        } else {
            tempTargetNum = 0;
        }
        if (tempTargetNum == 27) {
            int tNum = Core.Data.playerManager.Lv % 5;
            if (tNum != 0) {
                if (Core.Data.playerManager.Lv - tNum <= 0)
                    return 0;
                else {
                    return (Core.Data.playerManager.Lv - tNum) / 5;
                }
            } else {
                return temp;
            }
        }
        else return tempTargetNum;
        

    }
}
