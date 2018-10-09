using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UICityFloorManager : UIBaseWindow
{
    public UICityFloor cityFloor;
	public UIGrid cityRoot;
    public UITable floorRoot;
    
    GameObject cityItem;
    GameObject floorItem;


    List<UICityItem> cityList = new List<UICityItem>();
    List<UIFloorItem> floorList = new List<UIFloorItem>();
    public UIFloorBossItem bossItem{get;set;}
    SQYPlayerController player;

    public delegate void onClickedCityItem(City item);
    public onClickedCityItem OnClickedCityItem = null;
    
    public delegate void onClickedFloorItem(Floor item,bool isGuideClick=false);
    public onClickedFloorItem OnClickedFloorItem = null;
	
	/*Boss关卡特殊掉落点击以后可以查看详细信息
	 * */
	public System.Action<int> onBossDesInfoClick=null;
	
    protected static UICityFloorManager _instance;
	
	public System.Action onCityJumpFinished = null;
	
    public Dictionary<int,int> floorAndRewardDic = null;
	
	public UIScrollView _uiScrollView;
	
    public static UICityFloorManager Instance
    {
        get{return _instance;}
    }

    protected override void InitWidget()
    {
        _instance = this;
        base.InitWidget();
        player = GetComponent<SQYPlayerController>();
        UpdatePlayerInfo();
        _cityScroll = NGUITools.FindInParents<UIScrollView>(cityRoot.transform);
        _floorScroll = NGUITools.FindInParents<UIScrollView>(floorRoot.transform);
    }
    
    UIScrollView _cityScroll;
    UIScrollView _floorScroll;
    //bool _cityJump = false;
    //bool _floorJump = false;

    //bool first = true;
    //bool jump = true;
    int jumpFloorIndex = 0;
    //int jumpCityIndex = 0;
	
    

	void Start()
	{
        DBUIController.mDBUIInstance.HiddenFor3D_UI(true);
 
	}
	
   
	
    void Update()
    {

//        if(jump)
//        {
//            if(first)
//            {
//                first = false;
//                cityRoot.Reposition();
//                floorRoot.Reposition();
//            }
//            JumpToCity(jumpCityIndex, 0);
//            JumpToFloor(jumpFloorIndex, true);
//            jump = false;
//        }

//        if(_cityJump)
//        {
//            if(_cityScroll.RestrictWithinBounds(true, true, true))
//            {
//                SpringPanel sp = _cityScroll.GetComponent<SpringPanel>();
//                if (sp != null)
//                {
//                    sp.enabled = false;
//                    if(sp.onFinished != null)
//                    {
//                        sp.onFinished();
//                    }
//                }
//            }
//        }

//        if(_floorJump)
//        {
//            if(_floorScroll.RestrictWithinBounds(true, true, true))
//            {
//                SpringPanel sp = _floorScroll.GetComponent<SpringPanel>();
//                if (sp != null)
//                {
//                    sp.enabled = false;
//                    if(sp.onFinished != null)
//                    {
//                        sp.onFinished();
//                    }
//                }
//            }
//        }
    }
    
    IEnumerator NextFrame()
    {
        yield return new WaitForEndOfFrame();
		//UI切换小关
		JumpToFloor(jumpFloorIndex, true);
    }

    public void InitCityItems(int[] citys)
    {

		if(citys != null && citys.Length > 0)
        {
            if(cityItem == null)
            {
                cityItem = FhjLoadPrefab.GetPrefab(FhjPrefabsName.UICityItem);
            }
			
			DungeonsManager dm = Core.Data.dungeonsManager;
            int index = 0;
            foreach(int cityId in citys)
            {
				City itemData = dm.CityList[cityId];
				
                if(itemData != null)
                {
                    UICityItem _item = null;
                    if(cityList.Count > index)
                    {
                        _item = cityList[index];
                        _item.gameObject.SetActive(true);
                    }
                    else
                    {
                        _item = NGUITools.AddChild(cityRoot.gameObject, cityItem).GetComponent<UICityItem>();
                        _item.gameObject.name = _item.gameObject.name.Substring(0, _item.gameObject.name.IndexOf("(")) + ((char)(index + 'A')).ToString();
                        cityList.Add(_item);
                    }

                    if(_item != null)
                    {
                        _item.InitItem(itemData);
                        _item.OnClickedItem = ClickCityItem;
                        ++index;
                    }
#if FB2
					_item.SetImage(itemData.config.ID);
#endif
                }
            }
            
            for(int i=index; i < cityList.Count; ++i)
            {
                cityList[i].gameObject.SetActive(false);
            }
            cityRoot.repositionNow = true;
            _cityScroll.ResetPosition();
        }
		
		
		
		
		
		
		
		
		
    }

    public void InitFloorItems(IEnumerable<Floor> list, int floorID)
    {
        if(list != null)
        {
            if(floorItem == null)
            {
                floorItem = FhjLoadPrefab.GetPrefab(FhjPrefabsName.UIFloorItem);
            }
            if(bossItem == null)
            {
                bossItem = NGUITools.AddChild(floorRoot.gameObject, FhjLoadPrefab.GetPrefab(FhjPrefabsName.UIFloorBossItem)).GetComponent<UIFloorBossItem>();
                bossItem.gameObject.name = "z" + bossItem.gameObject.name;
                bossItem.OnClickedItem = ClickFloorItem;
                bossItem.OnClickedReward = ClickFloorReward;
                bossItem.OnOpenContent = ShowFloor;
				bossItem.OnLookDesInfo = ShowDesInfo;
            }
            
            int index = 0;
            foreach(Floor itemData in list)
            {
                if(itemData != null)
                {
                    UIFloorItem _item = null;
                    bool isBoos = itemData.config.isBoss>0;
                    if(isBoos)
                    {
                        _item = bossItem;
                    }
                    else
                    {
                        if(floorList.Count > index)
                        {
                            _item = floorList[index];
                            _item.gameObject.SetActive(true);
                        }
                        else
                        {
                            _item = NGUITools.AddChild(floorRoot.gameObject, floorItem).GetComponent<UIFloorItem>();
                            _item.OnClickedItem = ClickFloorItem;
                            _item.OnOpenContent = ShowFloor;
							_item.OnLookDesInfo += ShowDesInfo;
                            _item.gameObject.name = _item.gameObject.name.Substring(0, _item.gameObject.name.IndexOf("(")) + ((char)(index + 'A')).ToString();
                            floorList.Add(_item);
							
                        }
                    }
                    if(_item != null)
                    {
                        _item.InitItem(itemData, ++index);
                        _item.FoldContent(true);
                        if(itemData.config.ID == floorID)
                        {
 //                           jump = true;
                            jumpFloorIndex = index-1;
                        }
                    }
                }
            }

           for(int i=index-1; i < floorList.Count; ++i)
            {
               floorList[i].gameObject.SetActive(false);
            }
            floorRoot.repositionNow = true;
           _floorScroll.ResetPosition();
            StartCoroutine(NextFrame());
        }
    }


	
	//UI切换节
    public void JumpToCity(int _index, int count)
    {
        if(cityList!=null && cityList.Count>_index)
        {			 
			Vector3 pos = _cityScroll.panel.transform.localPosition ;
			float length = 46f;
			if(_index == 0)
			{
				 length =46f;
			}
			else if(_index == count -1)
			{
				 length =15f - cityRoot.cellWidth *(_index - 2) - 51f;
			}
			else
			{
				 length =15f - cityRoot.cellWidth *(_index - 1);
			}
			pos.x =  length;
			SpringPanel.Begin(_cityScroll.panel.cachedGameObject,pos, 10f);
			
					
        }
    }


    public UICityFloorManager JumpToFloor(int _index, bool reset=false)
    {
        if(floorList != null)
        {
            UIFloorItem _item = null;
            if(floorList.Count > _index)
            {
                _item = floorList[_index];
            }
            else
            {
                _item = bossItem;
				
                     
            }
            _item.FoldContent(false);
            cityFloor.SetFloorName(_item.data.config.name);
            StartCoroutine(JumpToFloor(_item.transform, reset) );
        }
		return this;
    }
	
	public void ShowDesInfo(int num)
	{
		if(onBossDesInfoClick!=null)
		{
			onBossDesInfoClick(num);
		}
	}
	
    public void ShowFloor(Transform target)
    {
		if(target != null)
            StartCoroutine( JumpToFloor(target, false) );
		else
		{
			//如果传过来null说明是收起来的操作
			//Debug.Log("how to deal it!");
		}
    }

    IEnumerator  JumpToFloor(Transform target, bool reset=false)
    {
		yield return new WaitForSeconds(0.1f);
        Jump(NGUITools.FindInParents<UIScrollView>(floorRoot.transform), target, reset);
    }

    public void Jump(UIScrollView scroll, Transform target, bool reset)
    {
		
		
		float pianyi =152f - (target.localPosition.y + target.parent.localPosition.y) ;
		
		Vector3 pos = scroll.transform.localPosition;
	
		if(target.name.Contains("Boss"))
		{
			
			//float height = pianyi;
		    UIFloorBossItem uiibossitem = target.GetComponent<UIFloorBossItem>();
			if(uiibossitem != null) 
			{
				if(uiibossitem.background.height < 342f)
				{
					   pianyi -=  (342f - uiibossitem.background.height-10f );
				}
			}
		}
		
		pos.y = -8f + pianyi;
		
		SpringPanel.Begin(scroll.panel.cachedGameObject, pos, 10f).onFinished = FloorJumpFinished;


    }

	public void FloorJumpFinished()
	{
		isPlaying = false;
		Resources.UnloadUnusedAssets();
	}

	bool _isPlaying = false;
	public bool isPlaying
	{
		get
		{
			return _isPlaying;
		}
		set
		{
			_isPlaying= value;
			if(_uiScrollView != null)
				_uiScrollView.enabled = !_isPlaying;			
		}
	}
	
	
    public void UpdateCityItems(int currID, int maxID)
    {
        foreach(UICityItem item in cityList)
        {
            if(item != null)
            {
                item.UpdateItem(currID, maxID);
            }
        }
    }

    public void UpdateFloorItems(int lastID,int currID)
    {
		int index = 0;
        foreach(UIFloorItem item in floorList)
        {
            if(item != null)
            {
                item.UpdateItem(lastID,currID,index++);
            }
        }
        bossItem.UpdateItem(lastID,currID);
    }

    
    string wantTexName = "";
    string textureName = "";
    public void ShowTex(string _name)
    {
        _name = FloorTextureManager.GetFileName(_name);
        if(string.IsNullOrEmpty(_name) || textureName==_name || wantTexName == _name)
            return;
		
        wantTexName = _name;
        UIDownloadTexture dt = new UIDownloadTexture();
		
		//检查本地是存否在
        if(FloorTextureManager.CheckExist(wantTexName))
        {
			cityFloor.RunProgress();		
            dt.EndLoadLocal = EndLoad;
            StartCoroutine(dt.LoadLocal(wantTexName));
        }
        else
        {
			//从网络下载
			if( Core.Data.usrManager.UserConfig.cartoon == 1)
			{
				cityFloor.RunProgress();		
	            dt.EndDownload = EndDownload;
	            dt.DownloadError = DownloadError;
	            StartCoroutine(dt.Download(wantTexName));
			}
			else
			{
				EndDownload(null,"");
				// cityFloor.SetTexture(null);
			}
        }
    }
    
    void EndLoad(Texture _tex)
    {
//		//加载本地图片完成
		 cityFloor.mTex = _tex;
		 cityFloor.isImgDone = true;
//        textureName = wantTexName;
//        cityFloor.SetTexture(_tex);
    }
    
    void EndDownload(Texture _tex, string _name)
    {
//		//下载网络图片完成
		  cityFloor.mTex = _tex;
	      cityFloor.isImgDone = true;

//        if(FloorTextureManager.GetFileName(_name) == wantTexName)
//        {
//            textureName = wantTexName;
//            cityFloor.SetTexture(_tex);
//        }

    }
    
	
	
	
    void DownloadError(string name)
    {
        //cityFloor.ShowDownloadProgress(false);
		int index=name.IndexOf('-');
		if(index > 0)
		{
			Debug.Log(index);
			int num =System.Convert.ToInt32( name.Substring(index+1,1) );
			if(num>1)
			{
				name=name.Replace("-"+num.ToString(),"-"+(--num).ToString());
				ShowTex(name);
			}			
		}
        //cityFloor.SetDefTexture();
    }

//    void Progress(float progress)
//    {
//        cityFloor.SetDownloadProgress(progress);
//    }


    void DestoryChlidren(Transform root)
    {
        int count = root.childCount;
        for(int i=0; i < count; ++i)
        {
            Transform item = root.GetChild(0);
            item.parent = null;
            Destroy(item.gameObject);
        }
    }
    
    public void UpdateChapterName(string _name)
    {
        cityFloor.SetChapterName(_name);
    }
    
    public void UpdateFloorName(string _name)
    {
        cityFloor.SetFloorName(_name);
    }

    public void ClickBack()
    {
		if(!CityFloorData.Instance.isCanClick)return;
		
		CityFloorData.Instance.isCanClick = true;
		
		UICityFloor floor= GetComponent<UICityFloor>();
		if(floor != null) floor.texture.mainTexture = null;
       Core.Data.dungeonsManager.isLastPlayFloor = 0;		
        UIMiniPlayerController.Instance.HideFunc();
		
		DBUIController.mDBUIInstance.chapterView.SetActive(true);
		
        Destroy(gameObject);
        _instance = null;
		
		Resources.UnloadUnusedAssets();
        //Destroy(gameObject);
    }
  

    void ClickCityItem(City _data)
    {
        if(OnClickedCityItem != null)
        {
            OnClickedCityItem(_data);
        }
    }
    
    void ClickFloorItem(Floor _data)
    {
        if(OnClickedFloorItem != null)
        {
            OnClickedFloorItem(_data);
        }
    }
    
    void ClickFloorReward(Floor _data)
    {
        
    }


	public void ShowReward(BattleSequence data, System.Action callBack)
    {
        UIFloorReward fr = FhjLoadPrefab.GetPrefabClass<UIFloorReward>();
        if(fr != null)
        {
            Transform t = fr.transform;
            t.parent = transform;
            t.localPosition = Vector3.back;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            fr.Close = callBack;
            fr.ShowReward(data);
        }
    }

    public void UpdatePlayerInfo()
    {
        if(player != null)
        {Debug.Log (" update player info");
            player.freshPlayerInfoView();
        }
    }
	

	
	
}
