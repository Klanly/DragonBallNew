using UnityEngine;
using System.Collections;

public class UIFloorItem : MonoBehaviour
{
    public enum EFloorItemStringID
    {
        CostTitle,
        RewardTitle,
        Pass,
        Current,
        Unlocked,
    }

    public enum FloorItemState 
    {
        None,
        Pass,
        Current,
        Unlocked,
    }

    public System.Action<Floor> OnClickedItem = null;
    public System.Action<Transform> OnOpenContent = null;
	public System.Action<int> OnLookDesInfo = null;
	
    public UISprite background;
    public UISprite titlebg;
    public UISprite titleHeadbg;
    public UISprite head;
    public UILabel floorName;
    public UILabel count;
    public UILabel index;
    public UILabel describe;
    public UILabel energy;
    public UILabel coin;
    public UILabel exp;
    public UILabel button;
    public UISprite buttonbg;
    public UILabel useTitle;
    public UILabel rewardTitle;
    public TweenScale content;

    protected Floor mData;
    protected Transform mContentTrans;
    public Floor data
    {
        get{return mData;}
        protected set
        {
            if(mData != value)
            {
                mData = value;
            }
        }
    }
    
    protected FloorItemState _state = FloorItemState.None;
    public virtual FloorItemState state
    {
        get {return _state;}
        protected set
        {
            if(_state != value)
            {
                _state = value;
                background.spriteName = "common-0015";//gray
                titlebg.spriteName = "common-0015";//gray
                titleHeadbg.spriteName = "common-0043_gray";//gray
                buttonbg.spriteName = "common-0015";//gray
				if(name.Contains("Boss"))
                   button.text = Core.Data.stringManager.getString(10021);
				else
				   button.text = Core.Data.stringManager.getString(10020);
				
                switch(_state)
                {
                case FloorItemState.Pass:
                    button.text = Core.Data.stringManager.getString(10001);
                    break;
                case FloorItemState.Current:
                    background.spriteName = "common-0022";
                    titlebg.spriteName = "common-0020";
                    titleHeadbg.spriteName = "common-0043";
                    buttonbg.spriteName = "main-0013";
                    break;
                case FloorItemState.Unlocked:
                    button.text = Core.Data.stringManager.getString(10003);
                    break;
                }
            }
        }
    }

    public virtual bool isBoss
    {
        get{return false;}
    }

    void Awake()
    {
        useTitle.text = Core.Data.stringManager.getString(10010);
        rewardTitle.text = Core.Data.stringManager.getString(10011);
        mContentTrans = content.transform;
    }
    
    public virtual void InitItem(Floor _data, int _index)
    {
        data = _data;
        //content.Reset();
        if(data != null)
        {
            floorName.text = data.config.name;
            describe.text = data.config.description;
			energy.text = data.config.needeEnergy.ToString();
            coin.text = data.config.coin.ToString();
            exp.text = data.config.exp.ToString();
            index.text = _index.ToString();
        }
    }
    
    public virtual void UpdateItem(int lastID,int currID,int index = 0)
    {
        if(data != null)
        {
			Floor tempfloor = null;
			Core.Data.dungeonsManager.FloorList.TryGetValue(currID,out tempfloor);
					
			Floor curFloor = null;
			Core.Data.dungeonsManager.FloorList.TryGetValue(data.config.ID,out curFloor);
			
			Floor beforeFloor = null;
			Core.Data.dungeonsManager.FloorList.TryGetValue(data.config.ID-1,out beforeFloor);
			
			bool result =false;
			if(index>0)
			result = (tempfloor != null && tempfloor.isBoss && !Core.Data.dungeonsManager.isFloorProgressFull(data.config.ID) && Core.Data.dungeonsManager.isFloorProgressFull(data.config.ID -1)  );
			else
			result =( tempfloor != null && tempfloor.isBoss && !Core.Data.dungeonsManager.isFloorProgressFull(data.config.ID) );
							
			
			if(data.config.ID ==currID || result)
			{
				state = FloorItemState.Current;
			}
            else if(data.config.ID > lastID)
            {
                state = FloorItemState.Unlocked;
            }
            else if(data.config.ID <=  lastID)
            {
                state = FloorItemState.Pass;
            }
//            else
//            {
//                state = FloorItemState.Current;
//            }
            
            //NGUITools.SetActive(head.gameObject, data.config.ID == maxFloorID);
			//Debug.Log(data.config.ID.ToString()+"   "+currID.ToString()+"   "+state.ToString() +" index:"+index.ToString());
            count.text = data.curProgress.ToString() + "/" + data.config.wave.ToString();//???
        }
//        click = false;
    }

    public void FoldContent(bool fold)
    {
        if(fold)
        {
            content.PlayReverse();
        }
        else
        {
            content.PlayForward();
        }
    }

    public virtual void UpdateFloorInfo()
    {
        //Reward Info
    }
    
	
    public virtual void OnClickItem()
    {
        if(state==FloorItemState.Current && OnClickedItem!=null)
        {
			if(!CityFloorData.Instance.isCanClick)return;
		    CityFloorData.Instance.isCanClick = false;
            OnClickedItem(data);
        }
    }

    public void ClickTitle()
    {		
		if(UICityFloorManager.Instance.isPlaying) return ;
		
	    if(content.transform.localScale.y == 0)
		{
		    UICityFloorManager.Instance.isPlaying = true;
			content.GetComponent<TweenScale>().PlayForward();
		}
		else if(content.transform.localScale.y == 1f)
		{
		    UICityFloorManager.Instance.isPlaying = true;
			content.GetComponent<TweenScale>().PlayReverse();
		}
		else
		{
			Debug.Log("isPlaying");
		}

//		else
//           click = true;
    }
    //bool click = false;
	
    public void ClickedTitle()
    {		
		
        if(OnOpenContent!=null /*&& click */&& content.transform.localScale==Vector3.one)
        {
			//Debug.LogError("=========false==========");
			
			if(UICityFloorManager.Instance!=null)
			UICityFloorManager.Instance._uiScrollView.enabled = false;

            OnOpenContent(transform);
			MakeLabelPerfect();
        }
		else
		{
			OnOpenContent(null);
			 UICityFloorManager.Instance.isPlaying = false;
		}
    }
	
	public void MakeLabelPerfect()
	{
		useTitle.MakePixelPerfect();
		rewardTitle.MakePixelPerfect();
		button.MakePixelPerfect();
		exp.MakePixelPerfect();
		coin.MakePixelPerfect();
		energy.MakePixelPerfect();
		describe.MakePixelPerfect();
		index.MakePixelPerfect();
		count.MakePixelPerfect();
		floorName.MakePixelPerfect();
	}
}
