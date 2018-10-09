using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TrucePanelScript : MonoBehaviour
{
    //1金币开启，2钻石开启，3道具开启
    //所以我们必须+1转换后发给服务器
    public enum MianZhanType
	{
        CoinMianZhan = 0,
        JewelMianZhan = 1,
        ItemMianZhan = 2,
	};

	public UILabel[] priceLabel;
	public UILabel[] buttonLabel;
	public UILabel[] nameLabel;
	public UISprite[] buttonBackground;
	public UISprite[] priceIcon;
	public UILabel[] mianZhanItemCountLabel;
    public StarsUI[] starObj;

	public List<Item> mianZhanItem = new List<Item>();

	public GameObject PanelRoot;

	void Start () 
	{
		mianZhanItem.Clear ();
		mianZhanItem = Core.Data.itemManager.getMianZhanPai();

		if (mianZhanItem [0].configData.ID == 110050) {
			Item itemTemp = mianZhanItem [0];
			mianZhanItem [0] = mianZhanItem [1];
			mianZhanItem [1] = itemTemp;
		} 

		
		InitPanel();
	}
	
	/// <summary>
	/// 关闭按钮
	/// </summary>
	public void OnXBtnClick()
	{
        //Reflash Top UI
        DBUIController.mDBUIInstance.RefreshUserInfoWithoutShow();
		Core.Data.dragonManager.buyMianZhanTimeCompletedDelegate = null;
		DestroyPanel();
	}

	void DestroyPanel()
	{
		Destroy(gameObject);
	}


	public void InitPanel()
	{
        int total = mianZhanItem.Count;
        for(int i = 0; i < total; i++)
		{
			Item item = mianZhanItem[i];
			priceLabel[i].text = item.configData.price[1].ToString();
			if (item.configData.price [0] == 0) {
				priceIcon [i].spriteName = "common-0013";
			} else {
				priceIcon [i].spriteName = "common-0014";
			}

			nameLabel[i].text = item.configData.name;
			int count = Core.Data.itemManager.GetBagItemCount (item.configData.ID);
            starObj [i].SetStar (item.configData.star);
			if(count == 0)
			{

				buttonLabel[i].text = Core.Data.stringManager.getString(6057); // 购买
				buttonBackground[i].spriteName = "Symbol 30";
			}
			else
			{
				buttonLabel[i].text = Core.Data.stringManager.getString(6060); // 使用
                buttonBackground[i].spriteName = "Symbol 30";

			}

			mianZhanItemCountLabel[i].text = "X" + count;
		}
	}

	public void coinClick()
	{
		actionClick(MianZhanType.CoinMianZhan);
	}

	public void jewelClick()
	{
		actionClick(MianZhanType.JewelMianZhan);
	}

	void actionClick(MianZhanType mianZhanType)
	{
		Item item = mianZhanItem[(int)mianZhanType];

		// 直接购买免战时间
		if(item.RtData.count == 0)
		{
			ItemData mdata = item.configData;
			int mNeedMoney = 0;
			if(mdata.price.Length != 0)
			{
				mNeedMoney = mdata.price[1];
			}
            if(mdata.price[0] == 0) {
                if(Core.Data.playerManager.Coin < mNeedMoney) {
					SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(35000));
					return;
				}
            } else {
                if(Core.Data.playerManager.Stone < mNeedMoney) {
					SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(35006));
					return;
				}
			}
			ComLoading.Open();
			Core.Data.dragonManager.buyMianZhanTimeCompletedDelegate = OnXBtnClick;
            Core.Data.dragonManager.buyMianZhanTimeRequest((int)mianZhanType + 1, item.configData.ID);
        } else{ // 有物品直接使用物品 
            ComLoading.Open();
            Core.Data.dragonManager.buyMianZhanTimeCompletedDelegate = OnXBtnClick;
            Core.Data.dragonManager.buyMianZhanTimeRequest((int)MianZhanType.ItemMianZhan + 1, item.RtData.id);
		}

	}

	static public TrucePanelScript ShowTrucePanel(GameObject root)
	{
		GameObject obj = PrefabLoader.loadFromPack("GX/pbTrucePanel")as GameObject ;
		if(obj !=null)
		{
			GameObject go = NGUITools.AddChild(root,obj);
			TrucePanelScript script = go.GetComponent<TrucePanelScript>();
			//	script.InitPanel(o);
            RED.TweenShowDialog (go);
			return script;
		}
		return null;
	}

}
