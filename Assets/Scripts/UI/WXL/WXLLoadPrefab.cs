using UnityEngine;
using System.Collections;

public class WXLLoadPrefab {


	public static GameObject GetPrefab (string _name)
	{
		return  PrefabLoader.loadFromPack(WXLPrefabsName.Path + _name) as GameObject;
	}

	public static T GetPrefabClass<T>(string path=WXLPrefabsName.Path) where T : MonoBehaviour
	{
		GameObject go = PrefabLoader.loadFromPack(path + "pb" + typeof(T).ToString()) as GameObject;
		if(go != null)
		{
			go = GameObject.Instantiate(go) as GameObject;
			if(go != null)
			{
				return go.GetComponent<T>();
			}
		}
		return null;
	}


}

public class WXLPrefabsName
{
	public const string Path = @"WXL/";

	public const string UIFestivalPanel = "pbWXLFestivalActivityController";
	public const string UIFestivalRankItem = "pbActivityRankItem";
	public const string UIMainItem = "pbActItem";
	public const string UIMonsterComePanel = "pbWXLMosterComeController";
	public const string UIFestivalReward = "pbUIActivityReward";
	public const string UIActivityMain = "pbUIActivityMain";
	public const string UITaoBaoPanel = "pbTaoBaoController";
	public const string UIDinnerPanel = "pbDinnerController";
	public const string UISignPanel = "pbActSignController";
	public const string UISignItem = "pbDateGiftItem";
	public const string UILevelRewardPanel ="pbLevelRewardController";
	public const string UIColItem ="pbCollectionItem";

    public const string UIOptionPanel ="pbOptionController";

    public const string UIBlackAlphaMask = "pbBlackAlphaMask";

    public const string UILabelEffect = "pbLabelEffect";
    public const string UIMonsterComerRank = "pbShowRank";
    public const string UITreasurePanel = "pbTreasureBoxController";
    public const string UITBDespItem = "pbGiftDespItem";
    public const string UIShowFatePanel = "pbShowFatePanel";
	public const string UIFateHeadItem = "pbFateObjItem";
	public const string UIFateBtnItem = "pbFateBtnItem";
    public const string UIGamblePanel = "pbUIGambleController";
    public const string UIGambleResultPanel = "pbUIGambleResult";
	public const string UIGambleItem = "pbGambleItemcell";
    public const string UISecretShopCellItem = "pbSecretMallItem";
    public const string UIRollGamblePanel = "pbRollGamblePanel";
    public const string UIDailyGiftPanel = "pbDailyGiftController";
    public const string UIDailyGiftItem = "pbDailyItem";
    public const string UIGetRewardPanel = "pbGetRewardController";
    public const string UICombineGemPanel = "pbCombineGemTag";
    public const string UIDownloadCartoonPanel = "pbWXLPVECartoonController";
	public const string UIAnimationFour = "pbUIAnimationFour";

	public const string UISourceBuildingPanel = "pbSourceBuildingNewUI";
	public const string UIDragonBankPanel = "pbDragonBankUI";

}
