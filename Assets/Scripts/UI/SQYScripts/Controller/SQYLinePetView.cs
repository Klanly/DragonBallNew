using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RUIType;


public  class SQYLinePetView : RUIMonoBehaviour {
	
	/// <summary>
	/// 选择了一个元素树
	/// </summary>
	public System.Action<SQYNodeForBI> ACT_SelectOneCharator;
	/// <summary>
	/// 选择了一大波元素树
	/// </summary>
	public System.Action<bool, List<SQYNodeForBI>> ACT_SelectMoreCharators;
	
	/// <summary>
	/// The go_ select all.
	/// </summary>
	public GameObject go_SelectAll;
	/// <summary>
	/// 当前星级的UI 数组sz
	/// </summary>
	public List<UISprite> szStars;
	/// <summary>
	/// tog按钮
	/// </summary>
	public UIToggle tg_selectAll;
	/// <summary>
	/// 上一页
	/// </summary>
	public UIButton m_btnPageUp;
	/// <summary>
	/// 下一页
	/// </summary>
	public UIButton m_btnPageDown;
	/// <summary>
	/// 所有元素的容器
	/// </summary>
	public GameObject group_Monster;
	//所有星星的容器
	public GameObject group_Star;
	/// <summary>
	/// 背景图片
	/// </summary>
	public UISprite sp_background;
	/// <summary>
	/// 页码的UI
	/// </summary>
	public UILabel lab_Page;
	/// <summary>
	/// 所有的元素的容器 现在有14个
	/// </summary>
	public List<GameObject> lstPanel;
	public BoxCollider bc_BoxCollider;
	/// <summary>
	/// 当前的所有数据
	/// </summary>
	List<object> szAllData;
	/// <summary>
	/// 当前页的数据
	/// </summary>
	List<object> szCurPageData = new List<object>();
	/// <summary>
	/// 缓存里面的元素树
	/// </summary>
	List<SQYNodeForBI> szCacheNodeBI = new List<SQYNodeForBI>();
	/// <summary>
	/// 当前页的元素树
	/// </summary>
	public List<SQYNodeForBI> szCurNodeBI = new List<SQYNodeForBI>();

	Vector3 v3_OneCenter = new Vector3(0,-25,0);
	Vector3 v3_HalfCenter = new Vector3(0,50f,0);
	
	Vector3 v3_OneSize = new Vector3(1120,450,1);
	Vector3 v3_HalfSize = new Vector3(1120,270,1);
	
	int curPage = 1;
	int totalPage = 1;
	const int mPageSize = 12;
	
	bool _bMoreThanHalf = false;

	EMItemType _itemType = EMItemType.NONE;
	EMBoxType _boxType = EMBoxType.LOOK_Charator;
	
	public bool MoreThanHalf{
		get{return _bMoreThanHalf;}
	}
	
	
		
	bool bInitView = false;
	void InitView()
	{
		if(bInitView)return;
		bInitView = true;
		
		for(int i=0;i<mPageSize;i++)
		{
			SQYNodeForBI node = new SQYNodeForBI();
			szCacheNodeBI.Add(node);
		}
		
		
		
	}

	 void OnPageUp()
	{
		if(curPage>1)
		{
			curPage --;
//			showCurPageCharator(curPage);
			StopAllCoroutines ();
			StartCoroutine ("showCurPageCharator", curPage);
		}
		SQYPetBoxController.mInstance.m_bagItemOprtUI.SetShow(false);
		SQYPetBoxController.mInstance.PageChange ();
	}
	 void OnPageDown()
	{
		if(curPage<totalPage)
		{
			curPage ++;
			StopAllCoroutines ();
			StartCoroutine ("showCurPageCharator", curPage);
//			showCurPageCharator(curPage);
		}

		SQYPetBoxController.mInstance.PageChange ();
	}
	
	IEnumerator showCurPageCharator(int page)
	{
		if(totalPage > 1)
		{
			RED.SetActive(true, m_btnPageUp.gameObject, m_btnPageDown.gameObject);

			m_btnPageDown.isEnabled = true;
			m_btnPageUp.isEnabled = true;

			if(curPage == totalPage)
			{
				m_btnPageDown.isEnabled = false;
			}

			if(curPage == 1)
			{
				m_btnPageUp.isEnabled = false;
			}
		}
		else
		{
			RED.SetActive(false, m_btnPageUp.gameObject, m_btnPageDown.gameObject);
		}

		szCurPageData.Clear();
		szCurNodeBI.Clear();
		tg_selectAll.value = false;
		lab_Page.text = Core.Data.stringManager.getString(5018) + curPage.ToString()+"/"+totalPage.ToString();//UI的这展示
		
		int beginIndex = (page-1)*mPageSize;//根据页数,用来获取当前页数的开始索引
		
		object mt = null;
		for(int i=beginIndex;i<mPageSize+beginIndex;i++)//循环当前页
		{
			if(i<szAllData.Count)
			{
				mt=szAllData[i];
				szCurPageData.Add(mt);
			}
			else{
				break;
			}
		}


		for (int i = 0; i < lstPanel.Count; i++)//循环所以的当前页元素,刷新UI
		{
			szCacheNodeBI[i].SetShowAll(false);//如果超过了数据源,则剩余的UI disable
		}

		for(int i=0;i<lstPanel.Count;i++)//循环所以的当前页元素,刷新UI
		{
			if(i<szCurPageData.Count)//如果有数据的刷新UI
			{
				SQYNodeForBI node = szCacheNodeBI[i];//取得当前的元素树
				node.SetShowAll (true);
				checkNodeBoxItem(node,_itemType,lstPanel[i]);//

				szCurNodeBI.Add(node);

				node._boxItem.bSelect = tg_selectAll.value;
				node._boxItem.gameObject.SetActive(true);
				node._boxItem.mIndex = i;
				node._boxItem.freshBoxItemWithData(szCurPageData[i]);//根据数据,刷新当前的元素(人物,道具,装备等)的UI

				yield return new WaitForSeconds (0.01f);
				
			}
//			else
//			{
//				szCacheNodeBI[i].hiddenAll();//如果超过了数据源,则剩余的UI disable
//			}
		}
	}
	/// <summary>
	/// 根据背包元素类型 来确定当前星级的元素是什么(人物,道具,装备等)
	/// </summary>
	/// <param name="node">Node.</param>
	/// <param name="iType">I type.</param>
	/// <param name="parent">Parent.</param>
	void checkNodeBoxItem(SQYNodeForBI node,EMItemType iType,GameObject parent)
	{

		switch(iType)
		{
		case EMItemType.Charator:
			if(node._charator==null)
			{
				node._charator = SQYBICharator.CreateBICharator();
				node._charator.OnClick +=this.On_Click;
			}
			break;
		case EMItemType.Equipment:
			if(node._equipment == null)
			{
				node._equipment = SQYBIEquipment.CreateBIEquipment();
				node._equipment.OnClick +=this.On_Click;
			}
			break;
		case EMItemType.Gem:
			if (node._gem == null)
			{
				node._gem = SQYBIGem.CreateBIGem ();
				node._gem.OnClick += this.On_Click;
			}
			break;
		case EMItemType.Props:
			if(node._props == null)
			{
				node._props = SQYBIProps.CreateBIProps();
				node._props.OnClick +=this.On_Click;
			}
			break;
		case EMItemType.AtkFrag:
		case EMItemType.DefFrag:
		case EMItemType.MonFrag:
			if(node._soul == null)
			{
				node._soul = SQYBISoul.CreateBISoul();
				node._soul.OnClick +=this.On_Click;
			}
			break;
		}

		node.resetItemType(iType);//根据类型 把当前的元素重设.
		RED.AddChild(node._boxItem.gameObject,parent);
		node._boxItem.myParent = this;
		node._boxItem.myNode = node;

	}
	/// <summary>
	/// 刷新当前星级的View 的入口,几乎从这里进入
	/// </summary>
	/// <param name="lstMT">所有的数据</param>
	/// <param name="star">当前星级</param>
	/// <param name="boxType">背包类型</param>
	/// <param name="st">筛选类型</param>
	/// <param name="iType">元素的类型</param>
	public void freshLinePetView(List<object> lstMT,int star,EMBoxType boxType,SplitType st,EMItemType iType)
	{
		_itemType = iType;
		_boxType = boxType;

		switch(_boxType)
		{
		case EMBoxType.CHANGE:
		case EMBoxType.LOOK_Charator:
		case EMBoxType.LOOK_Equipment:
		case EMBoxType.LOOK_Props:
		case EMBoxType.LOOK_Gem:
		case EMBoxType.LOOK_AtkFrag:
		case EMBoxType.LOOK_DefFrag:
		case EMBoxType.LOOK_MonFrag:
		case EMBoxType.Equip_ADD_ATK:
		case EMBoxType.Equip_ADD_DEF:
		case EMBoxType.Equipment_SWAP_ATK:
		case EMBoxType.Equipment_SWAP_DEF:
		case EMBoxType.HECHENG_ZHENREN_MAIN:
		case EMBoxType.HECHENG_ZHENREN_SUB:
		case EMBoxType.HECHENG_SHENREN_MAIN:
		case EMBoxType.HECHENG_SHENREN_SUB:
		case EMBoxType.ZHENREN_HE_SHENREN_MAIN:
		case EMBoxType.ZHENREN_HE_SHENREN_SUB:
		case EMBoxType.ATTR_SWAP:
		case EMBoxType.QIANLI_XUNLIAN:
		case EMBoxType.GEM_HECHENG_MAIN:
		case EMBoxType.GEM_HECHENG_SUB:
		case EMBoxType.SELECT_EQUIPMENT_INLAY:
		case EMBoxType.SELECT_EQUIPMENT_RECAST:
		case EMBoxType.SELECT_GEM_INLAY:
				go_SelectAll.SetActive(false);
			break;
		default:
			go_SelectAll.SetActive(true);
			break;
		}

		szAllData = new List<object>(lstMT);

		_bMoreThanHalf = lstMT.Count>(mPageSize/2);//是否过半
		curPage = 1;

		totalPage = (lstMT.Count/mPageSize)+(lstMT.Count%mPageSize==0?0:1);//总页数,如果没有过半,肯定只有一页

		
		for(int i=0;i<szStars.Count;i++)
		{
			if(i<star)
			{
				RED.SetActive (true, szStars [i].gameObject);
			}
			else
			{
				RED.SetActive (false, szStars [i].gameObject);
			}
		}
		
		if(_bMoreThanHalf)//根据是否过半来处理一个ui
		{
			if(lstMT.Count > mPageSize)
				sp_background.transform.localPosition = new Vector3 (-70, -230, 0);
			else
				sp_background.transform.localPosition = new Vector3 (-70, -210, 0);

			bc_BoxCollider.center = v3_OneCenter;
			bc_BoxCollider.size = v3_OneSize;
		}
		else{
			sp_background.transform.localPosition = new Vector3 (-70, -60, 0);
			bc_BoxCollider.center = v3_HalfCenter;
			bc_BoxCollider.size = v3_HalfSize;
		}

		//不管有多少页.先显示第一页的元素
//		showCurPageCharator(curPage);
		StopAllCoroutines ();
		StartCoroutine ("showCurPageCharator", curPage);
	}
	

	void OnBtnSelectAll()
	{
		if(ACT_SelectMoreCharators != null)
		{
			ACT_SelectMoreCharators(tg_selectAll.value, szCurNodeBI);
		}
	}
	
	void On_Click(GameObject go)
	{
		if(ACT_SelectOneCharator != null)
		{
			SQYBoxItem bi = go.GetComponent<SQYBoxItem>();
			if(bi !=null)
			{
				ACT_SelectOneCharator(bi.myNode);
			}
		}
	}
	
	void OnDestroy()
	{
		ACT_SelectOneCharator = null;
		ACT_SelectMoreCharators= null;
		
	}
	
	
	public static SQYLinePetView CreateLinePetView()
	{
		
		Object obj = PrefabLoader.loadFromPack("SQY/pbSQYLinePetView");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			SQYLinePetView lp = go.GetComponent<SQYLinePetView>();
			lp.InitView();
			return lp;
		}
		return null;
	}

	void OnClick()
	{
		SQYPetBoxController.mInstance.m_bagItemOprtUI.SetShow(false);
	}
	
	
	
}
