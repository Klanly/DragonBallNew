using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// collection: 在Item 上一级  在controller 下一级
/// </summary>
public class LevelRewardCollection : RUIMonoBehaviour,IItem
{
    public int levelNum;
    // 相当于id
    public List<RewardCollectionItem> colItemList;

    public enum CollectionState
    {
        GotReward,
        UnlockReward,
        LockReward,
    }

	public CollectionState curLvColState;
	//LevelUpRewardData collectionData;
	private LvCollection collectionData;
    public UILabel lbl_LvNum;
    public UISprite spt_Got;
    public UISprite spt_CanGet;

    public UISprite BgLevelLab;
    public UISprite Bg_main;
    // public UISprite spt_BlackAlpha;

    public bool canClick = true;
    Color unActiveColor = new Color (0,0,0,1f);

    #region IItem implementation

    void Awake()
    {
        canClick = true;
        gameObject.GetComponent<UIButton>().enabled = true;
    }

    	public void SetItemValue(object obj)
    {
		collectionData = obj as LvCollection;


        if (collectionData != null)
        {
            levelNum = collectionData.level;
			curLvColState = collectionData.CollectionState;
            for (int i = 0; i < colItemList.Count; i++)
            {
				CollectionItem tItem = new CollectionItem (collectionData.reward [i][0],collectionData.reward [i][1],curLvColState);
				colItemList[i].SetItemValue((object)tItem);

            }
        }
        else
        {
            levelNum = 5;
            for (int i = 0; i < colItemList.Count; i++)
            {
                colItemList[i].SetItemValue(null);
				colItemList[i].curItemState = CollectionState.LockReward;
                colItemList[i].Refresh();
            }
        }
   
        Refresh();
    }
		

    public object ReturnValue()
    {
        return (object)collectionData;
    }

    public void Refresh()
    {
        lbl_LvNum.text = levelNum + Core.Data.stringManager.getString(7147);
        switch (curLvColState)
        {
            case CollectionState.GotReward:
                spt_Got.gameObject.SetActive(true);
                spt_CanGet.gameObject.SetActive(false);
                canClick = false;
                BgLevelLab.color =Color.white;
                Bg_main.color = Color.white;
                break;
            case CollectionState.LockReward:
                spt_Got.gameObject.SetActive(false);
                spt_CanGet.gameObject.SetActive(false);
                canClick = false;
                BgLevelLab.color = unActiveColor;
                Bg_main.color = unActiveColor;
                BgLevelLab.color = unActiveColor;
                lbl_LvNum.color = Color.grey;
                break;
            case CollectionState.UnlockReward:
                spt_Got.gameObject.SetActive(false);
                spt_CanGet.gameObject.SetActive(true);
                canClick = true;
                BgLevelLab.color = Color.white;
                Bg_main.color = Color.white;
                lbl_LvNum.color = Color.white;
                break;
        }
		foreach (RewardCollectionItem rCollectionItem in colItemList) {
			rCollectionItem.Refresh ();
		}
    }

    #endregion

    public void OnGetThisGift()
    {
        if (curLvColState == CollectionState.UnlockReward)
        {
            if (Core.Data.playerManager.Lv >= levelNum)
            {
                if (canClick == true)
                {
                    ActivityNetController.GetInstance().GotLevelGiftRequest(levelNum);
                    canClick = false;
                }
            }
        }
    }

	public static void OnClickGiftOnGuide(){
			if (Core.Data.playerManager.Lv >= 5) {
					ActivityNetController.GetInstance ().GotLevelGiftRequest (5);
			}
	}
}

public class LvCollection{
	public int level;
	public List<LvItem[]> tLvReward;
	public List<int[]> reward;
	public LevelRewardCollection.CollectionState CollectionState;
	public LvCollection( int tlevel , List<LvItem[]> tLevelReward, List<int[]> tReward, LevelRewardCollection.CollectionState tCollectionState){
		this.level = tlevel;
		tLvReward = tLevelReward;
		reward = tReward;
		CollectionState = tCollectionState;
	}

}
