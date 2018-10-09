using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DragonMallScrollViewControl : MonoBehaviour 
{

	private List<UIDragonMallCell> mDragonMallCellList;
//	private List<GameObject> mDragonMallCellParentList;
    public GameObject ShowRoot;
    public UITable mGrid;
    public UIScrollView mScrollView;
	public UIScrollBar mScrollBar;

	UIPanel _Panel;

	void Start ()
    {
        mGrid = gameObject.GetComponentInChildren<UITable>();
        mScrollView = gameObject.GetComponent<UIScrollView>();
		mScrollBar = gameObject.transform.parent.GetComponentInChildren<UIScrollBar>();
		_Panel = gameObject.GetComponent<UIPanel>();
		mDragonMallCellList = new List<UIDragonMallCell>();
		InitCell();
	}

	public void InitCell()
	{
		SetPanel(ShopItemType.HotSale);
		GameObject obj = PrefabLoader.loadFromPack("LS/pbLSDragonBallMallCell")as GameObject ;
		UIDragonMallCell  mUIDragonMallCell;
		if(UIDragonMallMgr.GetInstance().m_CurShopTaglist.Count != 0 && UIDragonMallMgr.GetInstance().m_CurShopTaglist[0] != ShopItemType.Active)
		{
			ItemManager itemmgr = Core.Data.itemManager;
			if(obj != null)
			{ 
				int index;
				index = (itemmgr.GetShopItem(UIDragonMallMgr.GetInstance().mShopItemType).Count);
				for(int i=0; i<index; i++)
				{
					GameObject go = NGUITools.AddChild (ShowRoot, obj);
					go.gameObject.name = (100 + i).ToString();
					mUIDragonMallCell = go.gameObject.GetComponentInChildren<UIDragonMallCell> ();
					mDragonMallCellList.Add (mUIDragonMallCell);
				}
			}
			ShowAllScript(itemmgr.GetShopItem(UIDragonMallMgr.GetInstance().mShopItemType), UIDragonMallMgr.GetInstance().m_CurShopTaglist[0]);
		}
		else
		{		
			List<ActiveShopItem> mList = UIDragonMallMgr.GetInstance().m_ActiveShopList;
			if(mList.Count == 0)return;
			for(int z=0;z<mList.Count; z++)
			{
				GameObject go = NGUITools.AddChild (ShowRoot, obj);
				go.gameObject.name = z.ToString();
				mUIDragonMallCell = go.gameObject.GetComponentInChildren<UIDragonMallCell> ();
				mUIDragonMallCell.OnShow(mList[z], ShopItemType.Active);
				mDragonMallCellList.Add (mUIDragonMallCell);
			}

		}

		ResetPos();
	}
	
	void ShowAllScript(List<ItemData> mitems, ShopItemType type)
    {
        if(mDragonMallCellList.Count == 0 || mitems.Count == 0)return;
		if(type == ShopItemType.Vip)mGrid.padding = new Vector2(13,14);
		else mGrid.padding = new Vector2(13,14);
		for(int i=0; i<mDragonMallCellList.Count; i++)
		{
			mDragonMallCellList[i].OnShow(mitems[i],type);
		}
    }

	void ShowAllScript(List<ActiveShopItem> mitems, ShopItemType type)
	{
		if(mDragonMallCellList.Count == 0 || mitems.Count == 0)return;
		if(type == ShopItemType.Vip)mGrid.padding = new Vector2(13,14);
		else mGrid.padding = new Vector2(13,14);
		for(int i=0; i<mDragonMallCellList.Count; i++)
		{
			mDragonMallCellList[i].OnShow(mitems[i],type);
		}
	}

    public void ReFresh(ShopItemType mType)
    {
		SetPanel(mType);
		if(mType != ShopItemType.Active)NormalShopRefresh(mType);
		else ActiveShopRefresh(mType);
		mScrollView.verticalScrollBar.value = 0.0f;
        mGrid.Reposition();
    }

	public void ResetPos()
	{
		mGrid.Reposition();
		mScrollView.ResetPosition();
		mScrollView.verticalScrollBar.value = 0.0f;
	}

	public void deleteBg()
	{
		for(int i=0; i<mDragonMallCellList.Count; i++)
		{
			if(mDragonMallCellList[i].ob != null)
			{
				Destroy(mDragonMallCellList[i].ob.gameObject);
			}
		}
	}

	void SetPanel(ShopItemType type)
	{
		if(type == ShopItemType.Vip)
		{
			_Panel.baseClipRegion = new Vector4(-1f, -115f, 1110f, 384f);
		}
		else
		{
			_Panel.baseClipRegion = new Vector4(-1f, -80, 1110f, 450f);
		}
	}

	//活动商城
	void ActiveShopRefresh(ShopItemType mType)
	{
		List<ActiveShopItem> mList = UIDragonMallMgr.GetInstance().m_ActiveShopList;
		if(mDragonMallCellList.Count == 0 || mList.Count == 0)return;
		int m_Temp2 = mList.Count;
		if(mDragonMallCellList.Count > m_Temp2)
		{
			int i=0;
			int j=0;
			while( j <  m_Temp2)
			{
				mDragonMallCellList[i].OnShow(mList[j], mType);
				j++;
				i++;
			}
			while(i < mDragonMallCellList.Count)
			{
				mDragonMallCellList[i].OnHide();
				i++;
			}
		}
		else
		{
			int i=mDragonMallCellList.Count;
			GameObject obj = PrefabLoader.loadFromPack("LS/pbLSDragonBallMallCell")as GameObject ;
			UIDragonMallCell  mUIDragonMallCell;
			int index1 = m_Temp2 - i;
			for(int z=0;z<index1; z++)
			{
				GameObject go = NGUITools.AddChild (ShowRoot, obj);
				go.gameObject.name = (100 + i + z).ToString();
				mUIDragonMallCell = go.gameObject.GetComponentInChildren<UIDragonMallCell> ();
//				if(i+z < mList.Count)
//				{
//					mUIDragonMallCell.OnShow(mList[i+z], ShopItemType.Active);
//				}
				mDragonMallCellList.Add (mUIDragonMallCell);
			}
			ShowAllScript(mList,mType);

		}
	}

	//普通商城
	void NormalShopRefresh(ShopItemType mType)
	{
		ItemManager itemmgr = Core.Data.itemManager;
		List<ItemData> mitems = itemmgr.GetShopItem(mType);
		
		if(mDragonMallCellList.Count == 0 || mitems.Count == 0)return;
		int m_Temp2 = mitems.Count;
		if(mDragonMallCellList.Count > m_Temp2)
		{
			int i=0;
			int j=0;
			while( j <  m_Temp2)
			{
				mDragonMallCellList[i].OnShow(mitems[j], mType);
				j++;
				i++;
			}
			while(i < mDragonMallCellList.Count)
			{
				mDragonMallCellList[i].OnHide();
				i++;
			}
		}
		else
		{
			int i=mDragonMallCellList.Count;
			GameObject obj = PrefabLoader.loadFromPack("LS/pbLSDragonBallMallCell")as GameObject ;
			UIDragonMallCell  mUIDragonMallCell;
			int index1 = m_Temp2 - i;
			for(int z=0;z<index1; z++)
			{
				GameObject go = NGUITools.AddChild (ShowRoot, obj);
				go.gameObject.name = (100 + i + z).ToString();
				mUIDragonMallCell = go.gameObject.GetComponentInChildren<UIDragonMallCell> ();
				mDragonMallCellList.Add (mUIDragonMallCell);
			}
			
			ShowAllScript(mitems,mType);
			
			
		}
	}
}
