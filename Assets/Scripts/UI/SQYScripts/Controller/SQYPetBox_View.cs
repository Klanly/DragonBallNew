using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RUIType;

public partial class SQYPetBoxController
{
    public UIScrollView sv_BagPackage;
    /// <summary>
    /// 售价 前面标题
    /// </summary>
    public UILabel lab_CoinTitle;
    /// <summary>
    /// 强化信息
    /// </summary>
    public UILabel lblStrength;
    //强化 背景 可扩展 所有信息
    public UISprite spStrengthBG;
    //售价背景
    public GameObject sp_coinBg;
    //售价
    public UILabel lab_Coin;
    //售价obj
    public GameObject go_CoinPanel;
    public GameObject strengthObj;

	//分解标题
	public UILabel[] m_labDecompose;
	//战魂
	public UILabel[] m_labBattleSoul;
	//分解提示obj
	public GameObject m_objDecompose;

	public BagItemOprtUI m_bagItemOprtUI;

    public UIButton btn_Ok;
    public UIButton btn_Sell;
	public UIButton btn_Decompose;

    public UIButton m_btnChar;
    public UIButton m_btnEquip;
    public UIButton m_btnGem;
    public UIButton m_btnProp;
	public UIButton m_btnShop;

	public UIButton m_btnMonFrag;			//武者碎片
	public UIButton m_btnAtkFrag;			//武器碎片
	public UIButton m_btnDefFrag;			//防具碎片

	public GameObject m_selBtnTip;
	public Transform m_selBtnTipParent;

	public GameObject m_monTip;
	public GameObject m_equipTip;
	public GameObject m_gemTip;
	public GameObject m_itemTip;

	public GameObject m_monFragTip;
	public GameObject m_atkFragTip;
	public GameObject m_defFragTip;


    public UISprite m_Star ; // yangchenguang 星星


	//是否需要保存本地文件
	private bool m_bNeedSave = false;

	public static Vector3 longLblPos = new Vector3(240,-5,0);
	public static Vector3 shortLblPos = new Vector3(240,-5,0);
	
	public static int longWidth =  700;
	public static int shortWidth = 700;
    //强化入口数据   1：背包进入  2：阵容进入
    public static int enterStrengthIndex = 0;
    private const string SEL_SPNAME = "Symbol 31";
    private const string UNSEL_SPNAME = "Symbol 32";

    Color SEL_COLOR = Color.white; //new Color(255f / 255f, 215f / 255f, 0);
    Color UNSEL_COLOR = Color.white;
    string GColor = "[00FF00]";
    string RColor = "[FF0000]";
    string YColor = "[FFD700]";

//    public UISprite spAtkArrow;
//    public UISprite spDefArrow;
//    public UILabel lblAtkDesp;
//    public UILabel lblDefDesp;
//    public GameObject changeObj;

    //public int  petAtt = 0 ;  // yangchenguang 当先选中人物的攻击值
    //public int  petDef = 0 ;  // yangchenguang 当先选中人物的防御值

	//设置按钮是否可用状态
	private void SetBtnsEnable(bool val, params UIButton[] btn)
	{
		for(int i = 0; i < btn.Length; i++)
		{
//			btn [i].isEnabled = val;
			RED.SetActive (val, btn [i].gameObject);
		}
	}

    //初始化星星
    public void initStar()
    {
        if(m_Star ==null || m_Star.gameObject == null ) return  ; 
        m_Star.gameObject.SetActive(true) ;
        StarMove sm =  m_Star.gameObject.GetComponent<StarMove>();
        sm.ClearS();
        sm.setBtnXing();
    }
    //删除星星
    public void ClearStar()
    {
        if(m_Star ==null || m_Star.gameObject == null ) return  ; 
        m_Star.gameObject.SetActive(false) ;
        StarMove sm =  m_Star.gameObject.GetComponent<StarMove>();
        sm.ClearS();
    }


	void UpdateBtnEnable()
	{
		switch (_boxType)
		{
			case EMBoxType.LOOK_Charator:
			case EMBoxType.LOOK_Equipment:
			case EMBoxType.LOOK_Gem:
			case EMBoxType.LOOK_Props:
				SetBtnsEnable (true, m_btnChar, m_btnEquip, m_btnGem, m_btnProp);
				SetBtnsEnable(false, m_btnMonFrag, m_btnAtkFrag, m_btnDefFrag);
				break;
			case EMBoxType.QiangHua:
			case EMBoxType.QIANLI_XUNLIAN:
			case EMBoxType.ATTR_SWAP:
			case EMBoxType.DECOMPOSE_MONSTER:
			case EMBoxType.HECHENG_SHENREN_MAIN:
			case EMBoxType.HECHENG_SHENREN_SUB:
			case EMBoxType.HECHENG_ZHENREN_MAIN:
			case EMBoxType.HECHENG_ZHENREN_SUB:
			case EMBoxType.CHANGE:
			case EMBoxType.EVOLVE_MONSTER:
			case EMBoxType.SELL_Charator:
			case EMBoxType.ZHENREN_HE_SHENREN_MAIN:
			case EMBoxType.ZHENREN_HE_SHENREN_SUB:
				SetBtnsEnable (true, m_btnChar);
				SetBtnsEnable (false, m_btnEquip, m_btnGem, m_btnProp, m_btnMonFrag, m_btnAtkFrag, m_btnDefFrag);
				break;
			case EMBoxType.Equipment_SWAP_DEF:
			case EMBoxType.Equipment_SWAP_ATK:
			case EMBoxType.Equip_ADD_ATK:
			case EMBoxType.Equip_ADD_DEF:
			case EMBoxType.Equip_QH_ATK:
			case EMBoxType.Equip_QH_DEF:
			case EMBoxType.SELL_Equiement:
			case EMBoxType.SELECT_EQUIPMENT_INLAY:
			case EMBoxType.SELECT_EQUIPMENT_RECAST:
				SetBtnsEnable (true, m_btnEquip);
				SetBtnsEnable (false, m_btnChar, m_btnGem, m_btnProp, m_btnMonFrag, m_btnAtkFrag, m_btnDefFrag);
				break;
			case EMBoxType.GEM_HECHENG_MAIN:
			case EMBoxType.GEM_HECHENG_SUB:
			case EMBoxType.SELECT_GEM_INLAY:
			case EMBoxType.SELL_GEM:
				SetBtnsEnable (true, m_btnGem);
				SetBtnsEnable (false, m_btnChar, m_btnProp, m_btnEquip, m_btnMonFrag, m_btnAtkFrag, m_btnDefFrag);
				break;
		}

		if (_boxType == EMBoxType.LOOK_AtkFrag || _boxType == EMBoxType.LOOK_DefFrag || _boxType == EMBoxType.LOOK_MonFrag)
		{
			RED.SetActive (false, m_btnChar.gameObject, m_btnEquip.gameObject, m_btnGem.gameObject, m_btnProp.gameObject);
			RED.SetActive (true, m_btnAtkFrag.gameObject, m_btnDefFrag.gameObject, m_btnMonFrag.gameObject);
		}
		else
		{
			RED.SetActive (false, m_btnAtkFrag.gameObject, m_btnDefFrag.gameObject, m_btnMonFrag.gameObject);
		}
	}


    /// <summary>
    /// 根据背包类型 刷新UI
    /// </summary>
    /// <param name="bt">Bt.</param>
    void freshViewWithBoxType()
    {
        strengthObj.gameObject.SetActive(true);

        spStrengthBG.width = shortWidth;
        lblStrength.width = shortWidth;
        lblStrength.transform.localPosition = shortLblPos;
		lblStrength.text = "";
        //        changeObj.SetActive (false);

		RED.SetActive (false, m_btnShop.gameObject);
		RED.SetActive(true, btn_Decompose.gameObject, btn_Ok.gameObject, btn_Sell.gameObject);
//        if (star != null )
//        {
//
//
//            Debug.LogError("star move ");
//            star.ClearS();// yangcg
//            star.gameObject.SetActive(true);
//          
//            star.setBtnXing(); // yangcg
//        }


        switch (_boxType)
        {
			case EMBoxType.LOOK_Charator:
				RED.SetActive (false, go_CoinPanel, m_objDecompose);
				btn_Sell.TextID = 5001;
				btn_Decompose.TextID = 5175;
				btn_Decompose.isEnabled = true;
                break;

			case EMBoxType.CHANGE:
			case EMBoxType.Equipment_SWAP_ATK:
			case EMBoxType.Equipment_SWAP_DEF:
                btn_Ok.TextID = 5002;
                break;

			case EMBoxType.SELL_Charator:
				btn_Ok.TextID = 5003;
				lab_CoinTitle.textID (5100);
				go_CoinPanel.SetActive (true);
				RED.SetActive (false, strengthObj, m_objDecompose);
                break;

			case EMBoxType.SELL_Equiement:
				btn_Ok.TextID = 5007;
				lab_CoinTitle.textID (5100);
				go_CoinPanel.SetActive (true);
				RED.SetActive (false, strengthObj, m_objDecompose);
                break;

            case EMBoxType.Equip_ADD_ATK:
            case EMBoxType.Equip_ADD_DEF:
                btn_Ok.TextID = 5006;
                break;

			case EMBoxType.QiangHua:
			case EMBoxType.Equip_QH_ATK: //强化
			case EMBoxType.Equip_QH_DEF:
				btn_Ok.TextID = 5009;
				RED.SetActive (false, go_CoinPanel, m_objDecompose);
				spStrengthBG.gameObject.SetActive (true);
				spStrengthBG.width = longWidth;
				lblStrength.width = longWidth;
				lblStrength.transform.localPosition = longLblPos;
                

                initStar();
				if(LuaTest.Instance!= null)
					RED.SetActive (LuaTest.Instance.OpenStore, m_btnShop.gameObject);
				Transform tfTip = m_btnShop.transform.FindChild ("tip");
				UISprite spTip = tfTip.GetComponent<UISprite>();
				if (_boxType == EMBoxType.QiangHua)
				{
					spTip.spriteName = "chaozhijingyanzhu";
				}
				else
				{
					spTip.spriteName = "chaozhijingyandao";
				}
                lab_Coin.text = "";
                UpdateSellMoney();
                break;

            case EMBoxType.LOOK_Equipment:
				RED.SetActive (false, go_CoinPanel, m_objDecompose);
                btn_Ok.TextID = 5001;

                break;
			case EMBoxType.LOOK_Props:
				RED.SetActive (false, go_CoinPanel, m_objDecompose);
				btn_Decompose.TextID = 5171;
                break;
            case EMBoxType.LOOK_Gem:
				RED.SetActive (false, go_CoinPanel, m_objDecompose);
                btn_Ok.TextID = 5001;
                break;
            case EMBoxType.GEM_HECHENG_MAIN:
            case EMBoxType.GEM_HECHENG_SUB:
                btn_Ok.TextID = 5030;
                break;
//            case EMBoxType.LOOK_Soul:
			case EMBoxType.LOOK_AtkFrag:
			case EMBoxType.LOOK_DefFrag:
			case EMBoxType.LOOK_MonFrag:
				RED.SetActive (false, go_CoinPanel, m_objDecompose);
                break;
            case EMBoxType.SELL_GEM:
                btn_Ok.TextID = 5088;
                lab_CoinTitle.textID(5100);
				RED.SetActive (false, strengthObj, m_objDecompose);
                go_CoinPanel.gameObject.SetActive(true);
                break;
			case EMBoxType.DECOMPOSE_MONSTER:
				RED.SetActive (true, m_objDecompose);
				RED.SetActive (false, go_CoinPanel, strengthObj);
				m_labDecompose [0].text = Core.Data.stringManager.getString (5124);
				m_labDecompose [1].text = Core.Data.stringManager.getString (5005);
				m_labBattleSoul [0].text = "0";
				m_labBattleSoul [1].text = "0";
				btn_Ok.TextID = 5123;
				break;
            default:
                btn_Ok.TextID = 5030;
                break;
        }

        UpdateBtnSprite();

		if (_boxType == EMBoxType.LOOK_Charator)
		{
			RED.SetActive (true, btn_Sell.gameObject, btn_Decompose.gameObject);
			RED.SetActive (false, btn_Ok.gameObject);
            if (star != null )
            {
                star.gameObject.SetActive(true);
                star.ClearS();// yangcg
                star.setBtnXing(); // yangcg
            }

		}
		else if (_boxType == EMBoxType.LOOK_Equipment)
		{
			RED.SetActive (true, btn_Sell.gameObject);
			RED.SetActive (false, btn_Ok.gameObject, btn_Decompose.gameObject);
            if (star != null )
            {
                star.ClearS();// yangcg
                star.gameObject.SetActive(false);
            }
		}
        else if (_boxType == EMBoxType.LOOK_Gem)
		{
			RED.SetActive (false, btn_Decompose.gameObject, btn_Ok.gameObject);
			RED.SetActive (true, btn_Sell.gameObject);
            if (star != null )
            {
                star.ClearS();// yangcg
                star.gameObject.SetActive(false);
               
            }
		}
		else if (_boxType == EMBoxType.Equip_ADD_ATK ||
		               _boxType == EMBoxType.Equip_ADD_DEF ||
		               _boxType == EMBoxType.Equip_QH_ATK ||
		               _boxType == EMBoxType.Equip_QH_DEF ||
		               _boxType == EMBoxType.Equipment_SWAP_ATK ||
		               _boxType == EMBoxType.CHANGE ||
		               _boxType == EMBoxType.Equipment_SWAP_DEF ||
		               _boxType == EMBoxType.QiangHua ||
		               _boxType == EMBoxType.HECHENG_ZHENREN_MAIN ||
		               _boxType == EMBoxType.HECHENG_ZHENREN_SUB ||
		               _boxType == EMBoxType.ZHENREN_HE_SHENREN_MAIN ||
		               _boxType == EMBoxType.ZHENREN_HE_SHENREN_SUB ||
		               _boxType == EMBoxType.HECHENG_SHENREN_MAIN ||
		               _boxType == EMBoxType.HECHENG_SHENREN_SUB ||
		               _boxType == EMBoxType.ATTR_SWAP ||
		               _boxType == EMBoxType.QIANLI_XUNLIAN ||
		               _boxType == EMBoxType.GEM_HECHENG_MAIN ||
		               _boxType == EMBoxType.GEM_HECHENG_SUB ||
		               _boxType == EMBoxType.SELECT_EQUIPMENT_INLAY ||
		               _boxType == EMBoxType.SELECT_GEM_INLAY ||
		               _boxType == EMBoxType.SELECT_EQUIPMENT_RECAST ||
		               _boxType == EMBoxType.EVOLVE_MONSTER)
		{
			RED.SetActive (false, btn_Sell.gameObject, btn_Decompose.gameObject);
			RED.SetActive (true, btn_Ok.gameObject);
            if (star != null )
            {
                star.ClearS();// yangcg
                star.gameObject.SetActive(false);

            }
           
		}
		else if (_boxType == EMBoxType.DECOMPOSE_MONSTER)
		{
			RED.SetActive (false, btn_Decompose.gameObject,  btn_Sell.gameObject);
			RED.SetActive (true, btn_Ok.gameObject);
            if (star != null )
            {
                star.ClearS();// yangcg
                star.gameObject.SetActive(false);
               
            }

		}
		else if (_boxType == EMBoxType.LOOK_Props || _boxType == EMBoxType.LOOK_AtkFrag || _boxType == EMBoxType.LOOK_DefFrag || _boxType == EMBoxType.LOOK_MonFrag)
		{
			RED.SetActive (false, btn_Sell.gameObject, btn_Ok.gameObject, btn_Decompose.gameObject);
            if (star != null )
            {
                star.ClearS();// yangcg
                star.gameObject.SetActive(false);

            }
		}

		else if (_boxType == EMBoxType.SELL_Charator || _boxType == EMBoxType.SELL_Equiement || _boxType == EMBoxType.SELL_GEM
			|| _boxType == EMBoxType.CHANGE || _boxType == EMBoxType.Equipment_SWAP_ATK || _boxType == EMBoxType.Equipment_SWAP_DEF)
		{
			RED.SetActive (true, btn_Ok.gameObject);
			RED.SetActive (false, btn_Decompose.gameObject, btn_Sell.gameObject);
            if (star != null )
            {
                star.ClearS();// yangcg
                star.gameObject.SetActive(false);
               
            }
		}

		btn_Ok.isEnabled = szSelectCharator.Count > 0;

        lab_Coin.text = "";

        m_btnChar.TextID = 5013;
        m_btnEquip.TextID = 5014;
        m_btnGem.TextID = 5015;
//        m_btnSoul.TextID = 5016;
        m_btnProp.TextID = 5017;
		m_btnShop.TextID = 5149;

		m_btnAtkFrag.TextID = 5256;
		m_btnDefFrag.TextID = 5257;
		m_btnMonFrag.TextID = 5258;

		UpdateBtnEnable ();
    }

    void UpdateBtnSprite()
    {
        switch (_itemType)
        {
            case EMItemType.Charator:
                SetBtnSpriteName(SEL_SPNAME, m_btnChar);
				SetBtnSpriteName(UNSEL_SPNAME, m_btnEquip, m_btnGem, m_btnProp, m_btnAtkFrag, m_btnDefFrag, m_btnMonFrag);
				RED.AddChild(m_selBtnTip, m_btnChar.gameObject);
				m_selBtnTip.transform.localPosition = new Vector3(-60,0,0);
                break;
            case EMItemType.Equipment:
                SetBtnSpriteName(SEL_SPNAME, m_btnEquip);
				SetBtnSpriteName(UNSEL_SPNAME, m_btnChar, m_btnGem, m_btnProp, m_btnAtkFrag, m_btnDefFrag, m_btnMonFrag);
				RED.AddChild(m_selBtnTip, m_btnEquip.gameObject);
				m_selBtnTip.transform.localPosition = new Vector3(-60,0,0);
                break;
            case EMItemType.Gem:
                SetBtnSpriteName(SEL_SPNAME, m_btnGem);
				SetBtnSpriteName(UNSEL_SPNAME, m_btnEquip, m_btnChar, m_btnProp, m_btnAtkFrag, m_btnDefFrag, m_btnMonFrag);
				RED.AddChild(m_selBtnTip, m_btnGem.gameObject);
				m_selBtnTip.transform.localPosition = new Vector3(-60,0,0);
                break;
            case EMItemType.Props:
                SetBtnSpriteName(SEL_SPNAME, m_btnProp);
				SetBtnSpriteName(UNSEL_SPNAME, m_btnEquip, m_btnGem, m_btnChar, m_btnAtkFrag, m_btnDefFrag, m_btnMonFrag);
				RED.AddChild(m_selBtnTip, m_btnProp.gameObject);
				m_selBtnTip.transform.localPosition = new Vector3(-60,0,0);
                break;
			case EMItemType.AtkFrag:
				SetBtnSpriteName(SEL_SPNAME, m_btnAtkFrag);
				SetBtnSpriteName(UNSEL_SPNAME, m_btnEquip, m_btnGem, m_btnProp, m_btnChar, m_btnDefFrag, m_btnMonFrag);
				RED.AddChild(m_selBtnTip, m_btnAtkFrag.gameObject);
				m_selBtnTip.transform.localPosition = new Vector3(-60,0,0);
                break;
			case EMItemType.DefFrag:
				SetBtnSpriteName(SEL_SPNAME, m_btnDefFrag);
				SetBtnSpriteName(UNSEL_SPNAME, m_btnEquip, m_btnGem, m_btnProp, m_btnChar, m_btnAtkFrag, m_btnMonFrag);
				RED.AddChild(m_selBtnTip, m_btnDefFrag.gameObject);
				m_selBtnTip.transform.localPosition = new Vector3(-60,0,0);
				break;
			case EMItemType.MonFrag:
				SetBtnSpriteName(SEL_SPNAME, m_btnMonFrag);
				SetBtnSpriteName(UNSEL_SPNAME, m_btnEquip, m_btnGem, m_btnProp, m_btnChar, m_btnAtkFrag, m_btnDefFrag);
				RED.AddChild(m_selBtnTip, m_btnMonFrag.gameObject);
				m_selBtnTip.transform.localPosition = new Vector3(-60,0,0);
				break;
        }
		RED.SetActive(false, m_selBtnTip);
		RED.SetActive(true, m_selBtnTip);
    }


    int GetStrengthCoin()
    {
        int addcoin = 0;
        int star = 0;
        if (_boxType == EMBoxType.QiangHua)
        {
			star = SQYUIManager.getInstance().opMonster.Star;
        }
        else if (_boxType == EMBoxType.Equip_QH_ATK || _boxType == EMBoxType.Equip_QH_DEF)
        {
            star = SQYUIManager.getInstance().targetEquip.ConfigEquip.star;
        }
			
        switch (star)
        {
            case 1:
                addcoin = 200;
                break;
            case 2:
                addcoin = 300;
                break;
            case 3:
                addcoin = 400;
                break;
            case 4:
                addcoin = 800;
                break;
            case 5:
                addcoin = 1500;
                break;
			case 6:
				addcoin = 2000;
				break;
        }
        int totalCoin = szSelectCharator.Count * addcoin;
        return totalCoin;
    }

    int GetStrengthEqipTotalExp()
    {
		float totalExp = 0;
		if (szSelectCharator.Count == 0)
		{
			return 0;
		}
		for (int i = 0; i < szSelectCharator.Count; i++)
		{
			Equipment equip = szSelectCharator[i]._boxItem.curData as Equipment;
			float rate = 1;
			switch(equip.ConfigEquip.star)
			{
				case 1:
					rate = 0.2f;
					break;
				case 2:
					rate = 0.3333f;
					break;
				case 3:
					rate = 0.5f;
					break;
				case 4:
					rate = 0.8f;
					break;
				case 5:
					rate = 0.9f;
					break;
				case 6:
					rate = 1;
					break;
			}
			totalExp += equip.RtEquip.exp;
			totalExp += equip.ConfigEquip.exp;
			totalExp += (Core.Data.EquipManager.GetEquipTotalExp(equip.RtEquip.lv, equip.ConfigEquip.star) * rate);
		}
		return (int)totalExp;
    }

	int GetStrengthMonTotalExp()
    {
        float totalExp = 0;
        if (szSelectCharator.Count == 0)
        {
            return 0;
        }
        for (int i = 0; i < szSelectCharator.Count; i++)
        {
            Monster mon = szSelectCharator[i]._boxItem.curData as Monster;
			totalExp += GetStrengthMonExp (mon);
        }
        return (int)totalExp;
    }

	int GetStrengthMonExp(Monster mon)
	{
		float exp = 0.0f;
		float rate = 1;
		switch(mon.config.star)
		{
			case 1:
				rate = 0.2f;
				break;
			case 2:
				rate = 0.3333f;
				break;
			case 3:
				rate = 0.5f;
				break;
			case 4:
				rate = 0.8f;
				break;
			case 5:
				rate = 0.9f;
				break;
			case 6:
				rate = 1;
				break;
		}
		exp += mon.RTData.curExp;
		exp += mon.config.exp;
		exp += (Core.Data.monManager.GetMonTotalExp(mon.RTData.curLevel, mon.Star) * rate);

		return (int)exp;
	}

	int GetStrengthMonFinalLv(int eatExp)
    {
        Monster mon = SQYUIManager.getInstance().opMonster;
		int totalexp = mon.RTData.curExp + eatExp;
		totalexp += Core.Data.monManager.GetMonTotalExp(mon.RTData.curLevel, mon.Star); 
		int lv = Core.Data.monManager.GetMonLevel(totalexp, mon.Star);
        return lv;
    }

    int GetStrengthEquipFinalLv()
    {
        Equipment equip = SQYUIManager.getInstance().targetEquip;
        int totalexp = equip.RtEquip.exp + GetStrengthEqipTotalExp();
        totalexp += Core.Data.EquipManager.GetEquipTotalExp(equip.RtEquip.lv, equip.ConfigEquip.star);
        int lv = Core.Data.EquipManager.GetEquipLv(totalexp, equip.ConfigEquip.star);
        return lv;
    }

    int GetSellCoin()
    {
        int addcoin = 0;
        int totalCoin = 0;
        for (int i = 0; i < szSelectCharator.Count; i++)
        {
            switch (szSelectCharator[i]._boxItem.star)
            {
                case 1:
                    addcoin = 500;
                    break;
                case 2:
                    addcoin = 3000;
                    break;
                case 3:
                    addcoin = 20000;
                    break;
                case 4:
                    addcoin = 100000;
                    break;
            }
            totalCoin += addcoin;
        }

        return totalCoin;
    }

	int GetSellGemCoin()
	{
		int addcoin = 0;
		int totalCoin = 0;
		for (int i = 0; i < szSelectCharator.Count; i++)
		{
			addcoin = 1000;//szSelectCharator[i]._boxItem.star * 10000;
			totalCoin += addcoin;
		}
		
		return totalCoin;
	}


	int GetLowBattleSoulCnt()
	{
		int totalCoin = 0;
		for (int i = 0; i < szSelectCharator.Count; i++)
		{
			switch (szSelectCharator[i]._boxItem.star)
			{
				case 4:
					totalCoin++;
					break;
			}
		}

		return totalCoin;
	}
		
	//得到战魂个数
	int GetBattleSoulCnt()
	{
		int count = 0;
		for (int i = 0; i < szSelectCharator.Count; i++)
		{
			Monster mon = szSelectCharator [i]._boxItem.curData as Monster;
			count += Core.Data.monManager.GetBattleSoulByResolve (mon.config.star, mon.Star);
		}
		return count;
	}
		
	int GetDecomposeCoin()
	{
		int count = 0;
		for (int i = 0; i < szSelectCharator.Count; i++)
		{
			Monster mon = szSelectCharator [i]._boxItem.curData as Monster;
			count += Core.Data.monManager.GetResolveCostCoin (mon.config.star, mon.Star);
		}
		return count;
	}
	
	void UpdateSellMoney()
	{
		string strText = "";
		if (_boxType == EMBoxType.QiangHua || _boxType == EMBoxType.Equip_QH_ATK || _boxType == EMBoxType.Equip_QH_DEF)
		{
			strText = Core.Data.stringManager.getString (5105);
			RED.SetActive (false, sp_coinBg.gameObject);
			RED.SetActive (true, spStrengthBG.gameObject);
			if (_boxType == EMBoxType.QiangHua)
			{
				int totalExp = GetStrengthMonTotalExp ();
				strText = string.Format (strText, totalExp, GetStrengthMonFinalLv (totalExp), GetStrengthCoin ());
			}
			else
			{
				strText = string.Format (strText, GetStrengthEqipTotalExp (), GetStrengthEquipFinalLv(), GetStrengthCoin ());
			}
			lblStrength.text = strText;
			// lab_CoinTitle.text = strText;
		}
		else if (_boxType == EMBoxType.SELL_Charator || _boxType == EMBoxType.SELL_Equiement)
		{
			RED.SetActive (true, sp_coinBg.gameObject);
			strText = GetSellCoin ().ToString ();
			lab_Coin.text = strText;
		}
		else if(_boxType == EMBoxType.SELL_GEM)
		{
			RED.SetActive (true, sp_coinBg.gameObject);
			strText = GetSellGemCoin ().ToString ();
			lab_Coin.text = strText;
		}
		else if (_boxType == EMBoxType.DECOMPOSE_MONSTER)
		{
			m_labBattleSoul [0].text = GetBattleSoulCnt ().ToString();
			m_labBattleSoul [1].text = GetDecomposeCoin ().ToString ();
		}
	}
	
	void UpdateSimpleDetail(){
		
		string strText = "";
		lblStrength.text = strText;
     
        if (_boxType == EMBoxType.LOOK_Charator || _boxType == EMBoxType.LOOK_Equipment || _boxType == EMBoxType.CHANGE||
            _boxType == EMBoxType.Equip_ADD_ATK || _boxType== EMBoxType.Equip_ADD_DEF||_boxType == EMBoxType.Equipment_SWAP_ATK ||
            _boxType == EMBoxType.Equipment_SWAP_DEF||_boxType == EMBoxType.SELECT_EQUIPMENT_INLAY||_boxType == EMBoxType.SELECT_EQUIPMENT_RECAST)
        {
            strText = Core.Data.stringManager.getString(5121);
            RED.SetActive(false, sp_coinBg.gameObject);
            RED.SetActive(true, strengthObj);

            if (_boxType == EMBoxType.LOOK_Charator) {
				strText ="[ffff00]"+ Core.Data.stringManager.getString(5013) + Core.Data.stringManager.getString(9004) + "：[-]" + Core.Data.stringManager.getString(5121);
                strText = string.Format (strText, GetAttackNum (0), GetDefenceNum (0), GetExpNum (0));
            } else if (_boxType == EMBoxType.CHANGE) {
               
				strText ="[ffff00]"+ Core.Data.stringManager.getString(5013)+ Core.Data.stringManager.getString(9004)+"：[-]"+ Core.Data.stringManager.getString (7312);         //7312
               
                string colorATKStr = this.GetColor (Core.Data.temper.infoPetAtk, int.Parse (GetAttackNum (0)),true,true);
                string colorDEFStr = this.GetColor (Core.Data.temper.infoPetDef, int.Parse (GetDefenceNum (0)),true,false);

                strText = string.Format (strText, GetAttackNum (0), GetDefenceNum (0), colorATKStr + Core.Data.temper.tempTeamAtk+ "[-]", colorDEFStr + Core.Data.temper.tempTeamDef+"[-]");
             	strText = string.Format (strText,GetAttackNum(0),colorATKStr + Core.Data.temper.tempTeamAtk + "[-]", GetDefenceNum(0),colorDEFStr+ Core.Data.temper.tempTeamDef + "[-]");

            } else if (_boxType == EMBoxType.Equip_ADD_ATK || _boxType == EMBoxType.Equip_ADD_DEF) {

				strText ="[ffff00]"+Core.Data.stringManager.getString(5014)+ Core.Data.stringManager.getString(9004)+"：[-]"+ string.Format (strText, GetAttackNum (1), GetDefenceNum (1), GetExpNum (1));
            } else if (_boxType == EMBoxType.Equipment_SWAP_ATK || _boxType == EMBoxType.Equipment_SWAP_DEF) {
               
				strText ="[ffff00]"+ Core.Data.stringManager.getString(5014)+ Core.Data.stringManager.getString(9004)+"：[-]"+Core.Data.stringManager.getString (7312);
                string colorATKStr = this.GetColor (Core.Data.temper.equipAtk, int.Parse (GetAttackNum (1)));
                string colorDEFStr = this.GetColor (Core.Data.temper.equipDef, int.Parse (GetDefenceNum (1)));
                strText = string.Format (strText, GetAttackNum (1), GetDefenceNum (1), colorATKStr + Core.Data.temper.tempTeamAtk + "[-]", colorDEFStr + Core.Data.temper.tempTeamDef + "[-]");

            } else if (_boxType == EMBoxType.SELECT_EQUIPMENT_INLAY) {
               
				strText = "[ffff00]"+Core.Data.stringManager.getString(5014)+ Core.Data.stringManager.getString(9004)+"：[-]"+Core.Data.stringManager.getString(5121);
                strText = string.Format (strText, GetAttackNum (1), GetDefenceNum (1), GetExpNum (1));
            } else {
				strText ="[ffff00]"+ Core.Data.stringManager.getString(5014)+ Core.Data.stringManager.getString(9004)+"：[-]"+Core.Data.stringManager.getString(5121);
                strText = string.Format (strText, GetAttackNum (1), GetDefenceNum (1), GetExpNum (1));
            
            }
        }
        else if (_boxType == EMBoxType.LOOK_Gem || boxType == EMBoxType.SELECT_GEM_INLAY||_boxType == EMBoxType.SELECT_EQUIPMENT_RECAST)
        {
			strText = "[ffff00]"+Core.Data.stringManager.getString(5015)+ Core.Data.stringManager.getString(9004)+"：[-]"+Core.Data.stringManager.getString(5122);
            RED.SetActive(false, sp_coinBg.gameObject);
            RED.SetActive(true, strengthObj);
            strText = string.Format(strText, GetAttackNum(2), GetDefenceNum(2), GetExpNum(2));

        }
        else if(_boxType == EMBoxType.LOOK_Props)
        {
            if (szSelectCharator.Count == 0 || szSelectCharator.Count >2)
                return;
            Item it = szSelectCharator[0]._props.curData as Item;
			strText ="[ffff00]"+ Core.Data.stringManager.getString(5017)+ Core.Data.stringManager.getString(9004)+"：[-]"+it.configData.description;


        }
//		else if(_boxType == EMBoxType.LOOK_Soul)
		else if(_boxType == EMBoxType.LOOK_AtkFrag || _boxType == EMBoxType.LOOK_DefFrag || _boxType == EMBoxType.LOOK_MonFrag)
		{
            if (szSelectCharator.Count == 0 || szSelectCharator.Count >2)
                return;
            Soul so = szSelectCharator[0]._boxItem.curData as Soul;
			strText ="[ffff00]"+ Core.Data.stringManager.getString(5016)+ Core.Data.stringManager.getString(9004)+"：[-]"+so.m_config.description;
        }
			lblStrength.text = strText;
    }

    string GetColor(int leftNum, int rightNum,bool isChangeType = false,bool isAtk = true){

        string colorStr = "";
//        if (isChangeType == true) {
//            spAtkArrow.gameObject.SetActive (true);
//            spDefArrow.gameObject.SetActive (true);
//        } else {
//            spAtkArrow.gameObject.SetActive (false);
//            spDefArrow.gameObject.SetActive (false);
//        }

        if (leftNum > rightNum) {
            colorStr = RColor;
//            if (isAtk)
//                spAtkArrow.spriteName = "Down";
//            else
//                spDefArrow.spriteName = "Down";

        } else if (leftNum < rightNum) {
            colorStr = GColor;
//            if (isAtk)
//                spAtkArrow.spriteName = "Up";
//            else
//                spDefArrow.spriteName = "Up";
        } else {
            colorStr = YColor;
//            if (isAtk) spAtkArrow.gameObject.SetActive (false);
//            else spDefArrow.gameObject.SetActive (false);
        }
     
        return colorStr;
    }
    // 加减 yangchenguang 
    private string addAndDec(int leftNum, int rightNum)
    {
        string strV = "" ;
        int value = 0 ; 
        if (leftNum > rightNum)
        {
            value = leftNum - rightNum ; 
            strV = "↓" + value.ToString() ; 
        }
        else if (leftNum < rightNum)
        {
            value =rightNum -  leftNum ; 
            //↑下
            strV = "↑" + value.ToString() ; 
        }
        else 
        {
            strV = "↑" + "0" ; 
        }
        return strV ; 
    }

    //0 人物  1 装备
    string GetAttackNum(int id){
        int atkNum =0;
        if (szSelectCharator.Count == 0)
        {
            return atkNum.ToString();
        }

        for (int i = 0; i < szSelectCharator.Count; i++)
        {
            switch (id)
            {
                case 0:
                    Monster mon = szSelectCharator[i]._boxItem.curData as Monster;
                    atkNum += mon.getAttack;
                    break;
                case 1:
                    Equipment equip = szSelectCharator[i]._equipment.curData as Equipment;
                    atkNum += equip.getAttack;
                    break;
                case 2:
                    Gems gem = szSelectCharator[i]._gem.curData as Gems;
                    atkNum += gem.configData.atk;
                    break;
            }

           
        }
        return atkNum.ToString();
    }

    string GetDefenceNum(int id ){
        int defNum =0;
        if (szSelectCharator.Count == 0)
        {
            return defNum.ToString();
        }

        for (int i = 0; i < szSelectCharator.Count; i++)
        {
            switch (id)
            {
                case 0:
                    Monster mon = szSelectCharator[i]._boxItem.curData as Monster;
                    defNum += mon.getDefend;
                    break;
                case 1:
                    Equipment equip = szSelectCharator[i]._equipment.curData as Equipment;
                    defNum += equip.getDefend;
                    break;
                case 2:
                    Gems gem = szSelectCharator[i]._gem.curData as Gems;
                    defNum += gem.configData.def;
                    break;
            }


        }
        return defNum.ToString();
    }
    string GetExpNum(int id){
		string expNum ="";
   
        if (szSelectCharator.Count == 0)
        {
            return expNum.ToString();
        }

        for (int i = 0; i < szSelectCharator.Count; i++)
        {
            switch (id)
            {
                case 0:
                    Monster mon = szSelectCharator[i]._boxItem.curData as Monster;
					expNum += mon.RTData.curExp + "/" + Core.Data.monManager.getMonsterNextLvExp(mon.Star, mon.RTData.curLevel).ToString();
                    break;
                case 1:
                    Equipment equip = szSelectCharator[i]._equipment.curData as Equipment;
					expNum += equip.RtEquip.exp + "/" + Core.Data.EquipManager.GetEquipUpExp(equip.RtEquip.lv, equip.ConfigEquip.star).ToString();
                    break; 
                case 2:
                    Gems gem = szSelectCharator[i]._gem.curData as Gems;
					expNum += gem.configData.skillEffect.ToString();
                    break;
            }
        }
		return expNum;
    }


    void SetBtnSpriteName(string strSpName, params UIButton[] btns)
    {
        for (int i = 0; i < btns.Length; i++)
        {
			btns[i].normalSprite = strSpName;
            if (strSpName == SEL_SPNAME)
            {
				btns[i].transform.parent = m_selBtnTipParent;
            }
            else
            {
				btns[i].transform.parent = this.transform;
            }
 
			RED.SetActive(false, btns[i].gameObject);
			RED.SetActive(true, btns[i].gameObject);
        }
    }

	void SetBtnLabColor(Color clr, params UIButton[] btns)
	{
		for (int i = 0; i < btns.Length; i++)
		{
			UILabel lal = btns[i].gameObject.GetComponentInChildren<UILabel>();
			if (lal != null)
			{
				lal.color = SEL_COLOR;
			}
		}
	}

	void UpdateBtnTips()
	{
		RED.SetActive (Core.Data.monManager.GetNewMonList().Count > 0, m_monTip);
		RED.SetActive (Core.Data.EquipManager.GetNewEquip().Count > 0, m_equipTip);
		RED.SetActive (Core.Data.gemsManager.GetNewGem().Count > 0, m_gemTip);
		RED.SetActive (Core.Data.itemManager.GetNewItem().Count > 0, m_itemTip);

		RED.SetActive (Core.Data.soulManager.IsMonFragOK(), m_monFragTip);
		RED.SetActive (Core.Data.soulManager.IsAtkEquipOk(), m_atkFragTip);
		RED.SetActive (Core.Data.soulManager.IsDefEquipOK(), m_defFragTip);
	}

	void Start()
	{
		m_bagItemOprtUI.SetShow (false);
	}
}
