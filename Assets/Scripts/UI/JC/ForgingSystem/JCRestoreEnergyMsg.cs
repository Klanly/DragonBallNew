using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class JCRestoreEnergyMsg : MonoBehaviour {

	public UISprite[] Spr_Selected; // 选择框
	public UISprite[] Spr_Item; //道具图标
	public UILabel[]  Lab_Name; //物品名字
	public UILabel[]  Lab_Count; //物品个数
	int cur_Selected = 0;
	public GameObject Obj_content; // 
	public UILabel Lab_BtnName; //按钮的名字
	public UILabel Lab_DataUseCount; //使用道具次数和剩余次数
	int DataUseCount = -1;
	int leftItemNum;
	int rightItemNum;
	int type = 0;
    public UISprite[] price; //   商品货币 0:left  1:right
    public UILabel[] Price_Label; //  商品价格
	public UILabel Labtitle; //选择物品的描述
    public StarsUI[] starList;// 物品星级
    public TweenScale[]  tsList ;//字的跳动
    public GameObject PhysicalObj;//精力框
    PhysicalLogic Physical;// 
    List<BuyEnergy> listBuyE ;

    public const  int JingLing   = 0 ; 
    public const  int TiLi       = 1 ; 
    public const  int Coin       = 2 ; 
    public const  int EquipExp   = 3 ;
    public const  int PetExp     = 4 ; 
    public const  int WeaponExp  = 5 ; 

    static System.Action CloseUIBack = null  ;  // 界面关闭的时候回调


	private static JCRestoreEnergyMsg _mInstance;
	public static JCRestoreEnergyMsg mInstance
	{
        get {
			return _mInstance;
		}
	}
	
	enum ButtonType
	{
		Buy,
		Use,
	}
	
	ButtonType curButtonState ;
	/*设置按钮功能状态*/
	ButtonType CurButtonState 
	{
		set
		{
			curButtonState = value;
			if(curButtonState == ButtonType.Buy)
			{
				Lab_BtnName.text = Core.Data.stringManager.getString(6057);
			}
			else if(curButtonState == ButtonType.Use)
			{
				Lab_BtnName.text = Core.Data.stringManager.getString(6060);
			}
		}
		get
		{
			return curButtonState;
		}
	}
	//创建界面
    public static void OpenUI(int leftItemNum,int rightItemNum,int type = 0, System.Action CloseUIBack = null )
	{
		if (type != 0) {
			if (LuaTest.Instance != null && !LuaTest.Instance.ConvenientBuy) {

				if (type == 2)
					SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (35000));
				else if (type == 3) {
					SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5237));
				} else if (type == 4) {
					SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5238));
				}
				return;
			}
		}


		if(_mInstance == null)
		{
			Object prefab = PrefabLoader.loadFromPack("JC/JCRestoreEnergyMsg");
			if(prefab != null)
			{
				GameObject obj = Instantiate(prefab) as GameObject;
				RED.AddChild(obj, DBUIController.mDBUIInstance._TopRoot);
				obj.transform.localScale = Vector3.one;
				obj.transform.localPosition = Vector3.zero;
				obj.transform.localEulerAngles = Vector3.zero;
				_mInstance = obj.GetComponent<JCRestoreEnergyMsg>();
                _mInstance.Init(leftItemNum,rightItemNum,type);
                CloseUIBack= CloseUIBack ; 
			}
		}
	}
	// 获取道具个数
    private string getItemCount(int ItemNum ,int index )
    {

        ItemData item=Core.Data.itemManager.getItemData(ItemNum);


        string count  ="0" ; 
        if (index == 0  ||index == 1 ||index == 2)
        {
            count  = Core.Data.itemManager.GetBagItemCount(item.ID).ToString();
        }
        else if (index == 3 )
        {
//            List<Equipment>  equipment1 =  Core.Data.EquipManager.GetAllEquipByNum(item.num2[0][0]);
//
//            if (equipment1.Count> 0) 
//            {
//                count = equipment1.Count.ToString();
//            }
			price[0].spriteName = "common-0014" ;  // 金币
			price[1].spriteName ="common-0014"  ;
        }
        else if (index == 4 )
        {
//            List<Monster>  monster1 = Core.Data.monManager.GetAllMonsterByNum(item.num2[0][0]);
//            if (monster1.Count > 0)
//            {
//                count = monster1.Count.ToString()  ;
//
//            }
			price[0].spriteName = "common-0014" ;  // 金币
			price[1].spriteName ="common-0014"  ;
        }else if ( index == 5 )
        {
			price[0].spriteName = "common-0014" ;  // 钻石
            price[1].spriteName ="common-0014"  ;
        }

        return  count;
    }
    //初始化数据
	private void Init(int leftItemNum,int rightItemNum,int type = 0)
	{
	


		/*今日剩余次数
		 * */
		Spr_Item[0].transform.parent.parent.parent.name = leftItemNum.ToString()+"_0";
		Spr_Item[1].transform.parent.parent.parent.name = rightItemNum.ToString()+"_1";
		this.leftItemNum = leftItemNum;
		this.rightItemNum = rightItemNum;
		this.type = type;

        ItemData item1=Core.Data.itemManager.getItemData(leftItemNum);
        ItemData item2=Core.Data.itemManager.getItemData(rightItemNum);
        string strtxt ="";
        Lab_DataUseCount.text = "";

		//type 0:精力  1:体力   2:金币  3：武器经验 4:人物经验 
        if(type == JingLing)
		{

           
            int eCount =Core.Data.playerManager.dayStatus.buyEnyCount;

          
            listBuyE =  Core.Data.vipManager.GetBuyEnergyDataConfig();
            BuyEnergy buyEnergy ;
            if (eCount >= listBuyE.Count  )
            {
                buyEnergy =listBuyE[listBuyE.Count - 1 ];

            }else
            {
                buyEnergy =listBuyE[eCount];

            }

            Physical = PhysicalObj.GetComponent<PhysicalLogic>();
            Physical.stoneNum = buyEnergy.cost_D ;
            Obj_content.SetActive(false);
            PhysicalObj.SetActive(true);

            DataUseCount = Core.Data.vipManager.GetVipInfoData(Core.Data.playerManager.curVipLv).buy - eCount;
            // 今日次数
            Lab_DataUseCount.text =Core.Data.stringManager.getString(9040) + eCount.ToString()+"/"+Core.Data.vipManager.GetVipInfoData(Core.Data.playerManager.curVipLv).buy.ToString();
            Physical.dayCount.text = Lab_DataUseCount.text;
            strtxt = Core.Data.stringManager.getString(6133);

            strtxt = string.Format(strtxt,buyEnergy.cost_D,buyEnergy.num );
            Physical.uilabel_1.text = strtxt;

            return ;





		}
        else if (type == TiLi )
		{


            DataUseCount = Core.Data.vipManager.GetVipInfoData(Core.Data.playerManager.curVipLv).staminaItemLimit - 	Core.Data.playerManager.dayStatus.tlUse;
            Lab_DataUseCount.text =Core.Data.stringManager.getString(9040)+ Core.Data.playerManager.dayStatus.tlUse.ToString()+"/"+Core.Data.vipManager.GetVipInfoData(Core.Data.playerManager.curVipLv).buy.ToString();
        }

        //getItemCount(leftItemNum,type);
		/*110015 火鸡腿       110017 凤梨
		 * */
        //110019 一袋金币 110020 一箱金币
        //110044 白银棍x10   110046 白银护腕x10
        //110061  银猪   110029 银猪包


        //Core.Data.itemManager.GetBagItemCount(item2.ID).ToString();

		Spr_Item[0].spriteName = item1.ID.ToString();
		Spr_Item[0].MakePixelPerfect();
		Lab_Name[0].text = item1.name;
        if (leftItemNum == 110019)//一袋金币
        {
            Lab_Count[0].text = "";
            Lab_Count[1].text = "";
            tsList[0].gameObject.SetActive(false);
            tsList[1].gameObject.SetActive(false);

        }
        else if (leftItemNum == 110041)//白银棍
        {
            Lab_Count[0].text = "";
            tsList[0].gameObject.SetActive(false);

          
            Lab_Count[1].text =    ItemNumLogic.setItemNum(item2.num2[0][3] ,Lab_Count[1] , tsList[1].gameObject.GetComponent<UISprite>());

        }
        else if (leftItemNum == 110061) //银猪
        {
            Lab_Count[0].text ="";
            tsList[0].gameObject.SetActive(false);

            Lab_Count[1].text =    ItemNumLogic.setItemNum(10 ,Lab_Count[1] , tsList[1].gameObject.GetComponent<UISprite>());


        }else if (leftItemNum == 110039) // 白银护腕
        {
            Lab_Count[0].text = "";
            tsList[0].gameObject.SetActive(false);

            Lab_Count[1].text =  ItemNumLogic.setItemNum(item2.num2[0][3] ,Lab_Count[1] , tsList[1].gameObject.GetComponent<UISprite>());
        }

        Price_Label[0].text = item1.price[1].ToString();
        starList[0].SetStar(item1.star);

		/*110016 烤全猪        110018 仙豆
		 * */
		Spr_Item[1].spriteName = item2.ID.ToString();
		Spr_Item[1].MakePixelPerfect();
		Lab_Name[1].text = item2.name;
        //Lab_Count[1].text = "x"+10;
        Price_Label[1].text = item2.price[1].ToString();
        starList[1].SetStar(item2.star);


		
		
		SelectOne(leftItemNum,cur_Selected);
	}
	
	public void OnClose()
	{
//		TweenScale tween = Obj_content.GetComponent<TweenScale>();
//		tween.delay = 0;
//		tween.duration = 0.25f;
//		tween.from =  Vector3.one;
//		tween.to = new Vector3(0.01f,0.01f,0.01f);
//		tween.onFinished.Clear();
//		tween.onFinished.Add(new EventDelegate(this,"DestroyPanel"));
//		tween.Reset();
//		tween.PlayForward();
        if(CloseUIBack != null )
        {
            CloseUIBack() ; 
        }
        Destroy(gameObject);

	}

//	void DestroyPanel()
//	{
//		Destroy(gameObject);
//	}
   
	void Start () 
	{
  //      ts.enabled = true ;
	     //SelectOne(cur_Selected);
	} 
	
	void SelectItem(GameObject item)
	{
		string[] nums = item.name.Split('_');
		//int index = System.Convert.ToInt32(item.name);
		if(nums.Length>1)
	    SelectOne(int.Parse(nums[0]),int.Parse(nums[1]));
	}
//    {"ID":6129,"txt":"[ff0000]没有体力了![-]购买使用立即获得[ffff00]{0}[-]点体力"}
//    {"ID":6130,"txt":"[ff0000]金币不足![-]购买使用立即获得[ffff00]{0}[-]万金币"}
//    {"ID":6131,"txt":"[ff0000]超值经验卡![-]购买使用立即获得[ffff00]{0}[-]只经验宠物"}
//    {"ID":6132,"txt":"[ff0000]超值经验装备![-]购买使用立即获得[ffff00]{0}[-]件经验装备"}
	void SelectOne(int ItemNum,int index)
	{
		cur_Selected = index;
		Spr_Selected[index].enabled = true;
		Spr_Selected[1-index].enabled = false;
        string count ="0" ;
       
        count =  getItemCount(ItemNum,type);


        //type == 2
        if (type == JingLing || type == TiLi )
        {
            if(int.Parse(count) > 0)
            {
                CurButtonState = ButtonType.Use;
            }
            else
            {
                CurButtonState = ButtonType.Buy;
            }
        }else
        {

            CurButtonState = ButtonType.Buy;
        }
        ItemData item=Core.Data.itemManager.getItemData(ItemNum);
    
	
        string showtxt = "";
        //type 0:精力  1:体力   2:金币  3：武器经验 4:人物经验
        if(this.type == JingLing)
        {
            showtxt = showtxt.Replace("{}",Core.Data.stringManager.getString(5038));
        }
        else if (this.type == TiLi)
        {
            showtxt = Core.Data.stringManager.getString(6129);
            if (cur_Selected == 0 )
            {
                showtxt = string.Format(showtxt,50) ;
            }else
            {
                showtxt = string.Format(showtxt,100) ;
            }
        }
        else if (this.type == Coin)
        {
            showtxt = Core.Data.stringManager.getString(6130);



//            item.num2[0][3];

            showtxt = string.Format(showtxt,item.num2[0][3]) ;

//            if (cur_Selected == 0 )
//            {
//                showtxt = string.Format(showtxt,item.num2[0][3]) ;
//            }else
//            {
//                showtxt = string.Format(showtxt,item.num2[0][3]) ;
//
//            }
        }
        else if (this.type == EquipExp )
        {
           

            if (cur_Selected == 0 )
            {
                showtxt = Core.Data.stringManager.getString(6134);

               // showtxt = string.Format(showtxt,item.num2[0][3]) ;
                showtxt = string.Format(showtxt,"1") ;

            }else 
            {
                showtxt = Core.Data.stringManager.getString(6134);

                //showtxt = string.Format(showtxt,item.num2[0][3]) ;
                showtxt = string.Format(showtxt,item.num2[0][3]) ;

            }
        }
        else if (this.type == PetExp)
        {
            showtxt = Core.Data.stringManager.getString(6131);

           // showtxt = string.Format(showtxt,item.num2[0][3]) ;


            if (cur_Selected == 0 )
            {
                showtxt = string.Format(showtxt,1) ;

            }else
            {
                showtxt = string.Format(showtxt,10) ;

            }
        }else if (this.type == WeaponExp)
        {
            if (cur_Selected == 0 )
            {
                showtxt = Core.Data.stringManager.getString(6132);

                // showtxt = string.Format(showtxt,item.num2[0][3]) ;
                showtxt = string.Format(showtxt,"1") ;

            }else 
            {
                showtxt = Core.Data.stringManager.getString(6132);

                //showtxt = string.Format(showtxt,item.num2[0][3]) ;
                showtxt = string.Format(showtxt,item.num2[0][3]) ;

            }
        }
		Labtitle.text = showtxt;
	}



    bool errorCodeTile()
    {
        ItemData item1;

        if (cur_Selected == 0 )
        {
            item1=Core.Data.itemManager.getItemData(leftItemNum);

        }else
        {
            item1=Core.Data.itemManager.getItemData(rightItemNum);

        }
        if (item1 == null ) return false  ;
        if (item1.price[0] == 0 ) // 金币
        {

            if(Core.Data.playerManager.Coin < item1.price[1])
            {
                SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(6028));

               
                return false ;
            }


        }else// 钻石
        {

            if(Core.Data.playerManager.Stone < item1.price[1])
            {
                SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(7310));

                return  false ;
            }
        }


        return true ; 

    }
	
	
	void OnUseItem()
	{
		switch(CurButtonState)
		{
		case ButtonType.Buy:

            if (errorCodeTile() == false ) return  ;

            ComLoading.Open();
            PurchaseParam param = new PurchaseParam();
            param.gid = Core.Data.playerManager.PlayerID;
            if (cur_Selected == 0 )
            {
                param.propid = leftItemNum ; 
            }else
            {
                param.propid = rightItemNum; 
            }
            param.nm = 1;
            HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
            task.AppendCommonParam(RequestType.BUY_ITEM, param);
            task.afterCompleted += testHttpResp;
            task.DispatchToRealHandler();
       
			break;
		case ButtonType.Use:
			
            if(DataUseCount > 0 ||DataUseCount == -1 )
			{
				string buttonName = Spr_Selected[cur_Selected].transform.parent.name;
			    string[] str = buttonName.Split('_');
				if(str.Length >1)
			    UseProp(int.Parse(str[0]));
			}
			else
			  SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(9061));
			break;
		}
	}
	
	/*从商城返回
	 * */
	void OnShopExit()
	{
		DBUIController.mDBUIInstance._playerViewCtl.SetActive(false);
		UIMiniPlayerController.Instance.SetActive(true);
		if(gameObject != null)
		gameObject.SetActive(true);
		freshPlayerInfoView();
	}
	
	void UseProp(int ItemNum)
	{

		ComLoading.Open();
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		UsePropParam param = new UsePropParam();
		param.gid = Core.Data.playerManager.PlayerID;
		param.propid = Core.Data.itemManager.GetBagItemPid(ItemNum);
		param.nm = 1;
		param.propNum = ItemNum;
		
		task.AppendCommonParam(RequestType.USE_PROP, param);
		
		
		
		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;
	
		task.DispatchToRealHandler ();
	}
    public int getPackBackID(int ItemNum)
    {
        if (type == JingLing || type == TiLi  || type == Coin)
        {
            return   Core.Data.itemManager.GetBagItemPid(ItemNum);
        }

        return 0 ; 

    }
	
	void testHttpResp_UI (BaseHttpRequest req, BaseResponse response)
	{
	    ComLoading.Close();
		if (response.status != BaseResponse.ERROR) 
		{	
//			UIMiniPlayerController.Instance.freshPlayerInfoView();
			DBUIController.mDBUIInstance.RefreshUserInfo();
			freshPlayerInfoView();
			
		    //type 0:精力  1:体力   
			if(type == 0)
			{
				Core.Data.playerManager.dayStatus.jlUse++;
			     DataUseCount = Core.Data.vipManager.GetVipInfoData(Core.Data.playerManager.curVipLv).energyItemLimit - Core.Data.playerManager.dayStatus.jlUse;
			     Lab_DataUseCount.text =Core.Data.stringManager.getString(9040)+ Core.Data.playerManager.dayStatus.jlUse.ToString()+"/"+Core.Data.vipManager.GetVipInfoData(Core.Data.playerManager.curVipLv).energyItemLimit.ToString();
			}
			else
			{
				Core.Data.playerManager.dayStatus.tlUse++;
			     DataUseCount = Core.Data.vipManager.GetVipInfoData(Core.Data.playerManager.curVipLv).staminaItemLimit - 	Core.Data.playerManager.dayStatus.tlUse;
				 Lab_DataUseCount.text =Core.Data.stringManager.getString(9040)+ Core.Data.playerManager.dayStatus.tlUse.ToString()+"/"+Core.Data.vipManager.GetVipInfoData(Core.Data.playerManager.curVipLv).staminaItemLimit.ToString();
			}
			
			string showtxt = "";
			if(type == 0)
				showtxt = Core.Data.stringManager.getString(5038)+Core.Data.stringManager.getString(9022);
			else
				showtxt = Core.Data.stringManager.getString(5039)+Core.Data.stringManager.getString(9022);
			SQYAlertViewMove.CreateAlertViewMove (showtxt);
		}
		else
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getNetworkErrorString(response.errorCode));
		}
	}
	
	void freshPlayerInfoView()
	{

        //Lab_Count[0].text = "x"+getItemCount(leftItemNum,type);
        //Lab_Count[1].text = "x"+getItemCount(rightItemNum,type);


		if(cur_Selected == 0)
        {
            if (type == EquipExp || type == PetExp || type == WeaponExp)
            {
//				tsList[0].Reset();
				tsList [0].ResetToBeginning ();
                tsList[0].enabled =true ; 
            }

            SelectOne(leftItemNum, cur_Selected);
        }
		else
        {
            if (type == EquipExp || type == PetExp || type == WeaponExp )
            {
				tsList [1].ResetToBeginning ();
                tsList[1].enabled =true ; 
            }
            SelectOne(rightItemNum, cur_Selected);
        }
         
	}
	
	
	void testHttpResp_Error(BaseHttpRequest request, string error)
	{
		ComLoading.Close();
		SQYAlertViewMove.CreateAlertViewMove ("---- Http Resp - Error has ocurred." + error);
	}
    public void testHttpResp(BaseHttpRequest request, BaseResponse response)
    {
        if(response != null && response.status != BaseResponse.ERROR)
        {
            BuyItemResponse resp = response as BuyItemResponse;
            string strTxt = Core.Data.stringManager.getString(5214);

            if (type == Coin)
            {

                string coinTxt = Core.Data.stringManager.getString(5037);

                strTxt = string.Format(strTxt,resp.data.Result.coin,coinTxt);
                SQYAlertViewMove.CreateAlertViewMove (strTxt);

               // GetRewardSucUI.OpenUI(resp.data.p, Core.Data.stringManager.getString (5097));


            }else if (type == EquipExp ||type == PetExp || type == WeaponExp) 
            {

                HttpRequest upp = request as HttpRequest;
                PurchaseParam pp = upp.ParamMem as PurchaseParam;

                //ItemOfReward ior ;
                ItemData item;
                if (resp.data.Result == null )
                {
                  //  GetRewardSucUI.OpenUI(resp.data.p, Core.Data.stringManager.getString (5097));


                    item = Core.Data.itemManager.getItemData(pp.propid);
                    strTxt = string.Format(strTxt,item.name ,"");
                    SQYAlertViewMove.CreateAlertViewMove (strTxt);


                }else
                {

                    //GetRewardSucUI.OpenUI(resp.data.Result.p, Core.Data.stringManager.getString (5097));


                    item = Core.Data.itemManager.getItemData(pp.propid);
                    strTxt = string.Format(strTxt,item.name,"");
                    SQYAlertViewMove.CreateAlertViewMove (strTxt);

                }
				if(SQYPetBoxController.mInstance != null)
				{
					SQYPetBoxController.mInstance.SetPetBoxType (SQYPetBoxController.mInstance._boxType);
				}
            }
            freshPlayerInfoView();
            DBUIController.mDBUIInstance.RefreshUserInfo();// 刷新金币钻石等


        }else
        {
            string strtxt =  Core.Data.stringManager.getNetworkErrorString(response.errorCode);

            RED.Log(strtxt) ;
        }
        ComLoading.Close ();

    }
	
   
}
