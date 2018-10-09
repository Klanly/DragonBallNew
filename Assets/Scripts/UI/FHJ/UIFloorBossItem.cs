using UnityEngine;
using System.Collections.Generic;

public class UIFloorBossItem : UIFloorItem 
{
    public GameObject rewardRoot;
    public GameObject stoneRoot;
    public UISprite stone;
    public UILabel cost;
    public UISprite type;
		public GameObject typeParent;
    public UIGrid grid;

    public UISprite  Spr_specialReward;
	/*是否是免费的
	 * */
	public bool isFree{get;set;}
	
	public UILabel Lab_specialRewardName;

    
    public delegate void onClickedReward(Floor item);
    public onClickedReward OnClickedReward = null;

	public UIAtlas atlas_equipment;
	public StarsUI stars;
	


	//最近一次是不是免费的
	private bool LastShowIsFree{get;set;}
	
    public override bool isBoss
    {
        get
        {
            return true;
        }
    }

    public override FloorItemState state
    {
        get{return base.state;}
        protected set
        {
            if(value == FloorItemState.Pass)
            {
                value = FloorItemState.Current;
            }
            base.state = value;
        }
    }
    
    public override void OnClickItem()
    {
        if(state!=FloorItemState.Unlocked && OnClickedItem!=null)
        {
           if(!CityFloorData.Instance.isCanClick)return;
		    CityFloorData.Instance.isCanClick = false;
			data.isfree = isFree;
            OnClickedItem(data);
        }
    }
    
    public void OnClickReward(GameObject btn)
    {
		if(OnLookDesInfo != null)
		{
		    OnLookDesInfo(System.Convert.ToInt32(btn.name));
		}
//        if(OnClickedReward != null)
//        {
//            Debug.Log("OnClickReward" );
//            OnClickedReward(data);
//        }
    }

    public override void InitItem(Floor _data, int _index)
    {
        base.InitItem(_data, _index);
        if(data != null)
        {
            UpdataType(data.config.gf==1);

			SetSpecialReward(data.config.specialRewardID);
        }
    }

    public override void UpdateItem(int lastID,int currID,int index = 0)
    {
        if(data != null)
        {
			//Debug.Log("当前BOSS关:"+data.config.ID.ToString() +"    已挑战次数:"+ data.curProgress.ToString() +"  上次挑战是否成功:"+ Core.Data.dungeonsManager.isWinOfLastFloor);
            if(data.status==DungeonsData.STATUS_NEW && data.config.ID!=currID && data.curProgress==0)
            {
                state = FloorItemState.Unlocked;
            }
            else
            {
                state = FloorItemState.Current;
            }
            UpdateButton();

        }
    }

    void SetPet(List<int[]> boss)
    {
        int count = grid.transform.childCount;
        for(int i=0; i < count; ++i)
        {
            DestroyImmediate(grid.transform.GetChild(0).gameObject);
        }
        foreach(int[] item in boss)
        {
            UIHeadItem hi = FhjLoadPrefab.GetPrefabClass<UIHeadItem>();
            if(hi != null)
            {
                hi.HeadID = (((item[0]%2)==0)?10104:10140);
                hi.transform.parent = grid.transform;
                hi.transform.localScale = Vector3.one;
            }
        }
        grid.repositionNow = true;
    }
	
	
	
#region Add by jc
	/*设置特殊奖励(几率掉落)
	 * */
	void SetSpecialReward(int itemId)
	{
//		Debug.Log("------显示特殊奖励:"+itemId.ToString());
		int itemtype=itemId/10000;
		switch(itemtype)
		{
		case 1:
			/*宠物
			 * */
			Spr_specialReward.gameObject.name=itemId.ToString();
			AtlasMgr.mInstance.SetHeadSprite(Spr_specialReward,itemId.ToString());
			MonsterData monster = Core.Data.monManager.getMonsterByNum(itemId);
			if(monster != null)
			{
				stars.SetStar( monster.star );
				Spr_specialReward.MakePixelPerfect();
				Lab_specialRewardName.text = monster.name;
			}
			break;
		case 4:
			/*装备
			 * */
			ShowSpecialReward(atlas_equipment,itemId.ToString());
			EquipData equip = Core.Data.EquipManager.getEquipConfig(itemId);
			if(equip != null)
			{
			    stars.SetStar( equip.star );
				Lab_specialRewardName.text = equip.name;
			}
			break;
		case 15:
			{
				SoulData data = Core.Data.soulManager.GetSoulConfigByNum(itemId);
				if(data != null)
				{
				     stars.SetStar(data.star);
				       Spr_specialReward.gameObject.name=data.updateId.ToString();
						if (data.type == (int)ItemType.Monster_Frage) 
						{
							AtlasMgr.mInstance.SetHeadSprite(Spr_specialReward, data.updateId.ToString());
						}
						else if(data.type == (int)ItemType.Nameike_Frage || data.type == (int)ItemType.Earth_Frage)
						{
							Spr_specialReward.atlas = AtlasMgr.mInstance.itemAtlas;
					        Spr_specialReward.spriteName = data.updateId.ToString();
						}
				        Lab_specialRewardName.text = data.name;
			    }	
			     break;	
			}
		}		

	}
	
	void ShowSpecialReward(UIAtlas _atlas,string spr_item)
	{				
		Spr_specialReward.atlas=_atlas;
		Spr_specialReward.spriteName=spr_item;		
		Spr_specialReward.gameObject.name=spr_item;
		Spr_specialReward.MakePixelPerfect();
	}
	

#endregion	
	
    void UpdateButton()
    {
		//如果本Boss关已打的次数和可打的数次一样，则说明要花钱	
		bool free = data.curProgress<(data.config.wave+data.config.BossLaveTime);
		/*获取上一关的数据
		 * */
		if(!free)
		{
			Floor beforeData=Core.Data.dungeonsManager.getFloor(data.config.ID-1);
			if(beforeData!=null)
			{
				//Debug.Log("本层BOSS关已打"+data.curProgress+"次"+ "    上一次的进度:"+beforeData.curProgress+"/"+(beforeData.config.wave+beforeData.config.laveTime).ToString());

//				if(Core.Data.dungeonsManager.isFristRun)
//				{
//					free = false;
//					Core.Data.dungeonsManager.isFristRun = false;
//					cost.text = "10";
//				}
//				else 
				if( beforeData.curProgress==beforeData.config.wave+beforeData.config.laveTime && Core.Data.dungeonsManager.isWinOfLastFloor)
				{
					//if(Core.Data.dungeonsManager.isBossOfLastFloor)
					//{
					    free = LastShowIsFree;
					    LastShowIsFree = free;
					//}
					//else
					//{
					    free = true;	
						count.text =  "0/1";
					//}
				}
				else
				{
					free = false;
					cost.text = "10";
				}
			}
		}
		else
		{
			if(data.curProgress>=1)
			{
				Floor beforeData=Core.Data.dungeonsManager.getFloor(data.config.ID-1);
			    if(beforeData!=null)
			    {
					if( beforeData.curProgress==beforeData.config.wave+beforeData.config.laveTime )
					{
						count.text =  "0/1";
					}
					else
						count.text ="VIP"+Core.Data.playerManager.curVipLv+":"+(data.curProgress-1).ToString()+"/" + data.config.BossLaveTime;						
				}
			}
			else
				count.text =  "0/1";				
		}
		
			
		NGUITools.SetActive(stoneRoot, !free);
		NGUITools.SetActive(count.gameObject, free);
//		if(free)
//		{
//			count.text =  "0/1";
//		}        
//		else
//        cost.text = "10";
		
		isFree = free;
		//Debug.Log("isFree="+isFree);
    }

    void UpdataType(bool attack)
    {

        type.spriteName = (attack?"BattleUI_Attack":"BattleUI_Defense");
    }


    public void AddTweenAniTOType(){
        //Debug.Log(" tween ani    ");

        if (typeParent == null)
            typeParent = type.transform.parent.gameObject;
 
        AnimationCurve tScale = new AnimationCurve(new Keyframe(0.1f,8),new Keyframe(0.2f,1f),new Keyframe(0.25f,0.5f),new Keyframe(0.3f,1f));
	    if (typeParent.GetComponent<TweenScale>() != null)
        {
			typeParent.GetComponent<TweenScale>().enabled = true;
        }
        else
        {
           	typeParent.transform.localScale = Vector3.zero;
			typeParent.AddComponent<TweenScale>().animationCurve = tScale;
        }
        TweenScale.Begin(typeParent,1f,Vector3.one);

        AnimationCurve tAlpha = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.05f,0f), new Keyframe(0.2f, 1f));
        if (type.gameObject.GetComponent<TweenAlpha>() != null)
        {
            type.gameObject.GetComponent<TweenAlpha>().enabled = true;
        }
        else
        {
            type.alpha = 0;
            type.gameObject.AddComponent<TweenAlpha>().animationCurve = tAlpha;
        }
        TweenAlpha.Begin(type.gameObject,1f,1f);



    }





}
