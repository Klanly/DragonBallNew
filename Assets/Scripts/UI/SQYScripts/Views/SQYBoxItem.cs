using UnityEngine;
using System.Collections;
using RUIType;
namespace RUIType{
	public enum EMItemType{
		NONE = 0,
		Charator = 1,
		Equipment =2,
		Gem = 3,
		Props = 4,
		MonFrag = 5,	// 武者碎片
		AtkFrag = 6,	// 武器碎片
		DefFrag = 7,	// 防具碎片
	}
}
/// <summary>
/// 背包的元素树,包含所有的元素节点.
/// </summary>
public class SQYNodeForBI
{
	public SQYBoxItem _boxItem = null;
	public SQYBICharator _charator = null;
	public SQYBIEquipment _equipment = null;
	public SQYBIGem _gem = null;
	public SQYBISoul _soul = null;
	public SQYBIProps _props = null;
	public void SetShowAll(bool bShow)
	{
		if(_charator != null)
		{
			_charator.gameObject.SetActive(bShow);
		}
		if(_equipment != null)
		{
			_equipment.gameObject.SetActive(bShow);
		}
		if(_gem != null)
		{
			_gem.gameObject.SetActive(bShow);
		}
		if(_props != null)
		{
			_props.gameObject.SetActive(bShow);
		}
		if (_soul != null)
		{
			_soul.gameObject.SetActive(bShow);
		}
	}
	EMItemType _itemType = EMItemType.NONE;
	/// <summary>
	/// 重新设置当前的元素
	/// </summary>
	/// <param name="iType">I type.</param>
	public void resetItemType(EMItemType iType)
	{
		_itemType = iType;
		switch(_itemType)
		{
		case EMItemType.Charator:
			_boxItem = _charator as SQYBoxItem;
			break;
		case EMItemType.Equipment:
			_boxItem = _equipment as SQYBoxItem;
			break;
		case EMItemType.Gem:
			_boxItem = _gem as SQYBoxItem;
			break;
		case EMItemType.Props:
			_boxItem = _props as SQYBoxItem;
			break;
//		case EMItemType.Soul:
		case EMItemType.AtkFrag:
		case EMItemType.DefFrag:
		case EMItemType.MonFrag:
			_boxItem = _soul as SQYBoxItem;
			break;
		}
		SetShowAll(false);
		if(_boxItem!=null)
		{
			_boxItem.gameObject.SetActive(true);
		}
	}
}
/// <summary>
/// 背包元素的父类.
/// </summary>
public class SQYBoxItem : RUIMonoBehaviour {
	
	public UILabel[] allLabels;
	
	public UISprite[] allSprites;

	public UITexture[] allTextures;

	public GameObject group_Select;

	public short star
	{
		get
		{
			short star = 0;
			switch (curItemType)
			{
				case EMItemType.Charator:
					Monster mon = curData as Monster;
					star = mon.Star;
					break;
				case EMItemType.Equipment:
					Equipment equip = curData as Equipment;
					star = equip.ConfigEquip.star;
					break;
				case EMItemType.Gem:
					Gems gem = curData as Gems;
					star = gem.configData.star;
					break;
				case EMItemType.Props:
					Item item = curData as Item;
					star = item.configData.star;
					break;
//				case EMItemType.Soul:
				case EMItemType.AtkFrag:
				case EMItemType.DefFrag:
				case EMItemType.MonFrag:
					Soul soul = curData as Soul;
					MonsterData monData = Core.Data.monManager.getMonsterByNum (soul.m_config.updateId);
					if (monData == null)
					{
						RED.LogWarning (soul.m_config.updateId + " not find");
						star = 1;
					}
					else
					{
						star = monData.star;
					}
					break;
			}
			return star;
		}
	}

	public int pid
	{
		get
		{
			int id = 0;
			switch (curItemType)
			{
				case EMItemType.Charator:
					Monster mon = curData as Monster;
					id = mon.pid;
					break;
				case EMItemType.Equipment:
					Equipment equip = curData as Equipment;
					id = equip.RtEquip.id;
					break;
				case EMItemType.Gem:
					Gems gem = curData as Gems;
					id = gem.id;
					break;
				case EMItemType.Props:
					Item item = curData as Item;
					id = item.RtData.id;
					break;
//				case EMItemType.Soul:
				case EMItemType.AtkFrag:
				case EMItemType.DefFrag:
				case EMItemType.MonFrag:
					Soul soul = curData as Soul;
					id = soul.m_RTData.id;
					break;
			}
			return id;
		}
	}

	[HideInInspector]
	/// <summary>
	/// 当前元素的星级view
	/// </summary>
	public SQYLinePetView myParent;
	[HideInInspector]
	/// <summary>
	/// 当前元素的元素树
	/// </summary>
	public SQYNodeForBI myNode;

	[HideInInspector]
	/// <summary>
	/// 当前元素的数据
	/// </summary>
	public object curData;

	[HideInInspector]
	/// <summary>
	/// 当前元素的索引值
	/// </summary>
	public int mIndex;

	[HideInInspector]
	/// <summary>
	/// 当前的元素类型(人物,道具,装备)
	/// </summary>
	public EMItemType curItemType= EMItemType.NONE;

	GameObject _gameObject;

	public System.Action<GameObject> OnClick;

	protected UISprite[] szSelects = null ;
	protected TweenPosition[] szTwPos = null;
	protected bool _bSelect = false;
	public virtual bool bSelect
	{
		set
		{
			_bSelect = value;
			NGUITools.SetActive(group_Select.gameObject, _bSelect);

		}
		get{
			return _bSelect;
		}
	}
	protected virtual void dealloc()
	{
		for(int i=0;i<allLabels.Length;i++){allLabels[i]=null;}
		for(int i=0;i<allSprites.Length;i++){allSprites[i]=null;}
		for(int i=0;i<allTextures.Length;i++){allTextures[i]=null;}
	}

	protected bool bInitBox = false;
	/// <summary>
	/// 一定在重写此方法的时候 执行 base.initBoxItem();
	/// </summary>
	protected virtual void initBoxItem()
	{
		if(bInitBox)return;
		bInitBox = true;
		_gameObject = gameObject;

		UIEventListener.Get(_gameObject).onClick +=this.On_Click;
	}


	//所有的元素的刷新UI 都继承这个借口
	public virtual void freshBoxItemWithData(object obj)
	{
		curData = obj;
	}

	protected virtual void On_Click(GameObject go)
	{
		if(OnClick != null)
		{
			OnClick(go);
		}

		Core.Data.soundManager.BtnPlay (ButtonType.Confirm);
	}
	
}
