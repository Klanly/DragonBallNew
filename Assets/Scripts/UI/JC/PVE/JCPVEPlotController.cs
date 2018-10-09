using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//PVE剧情UI管理器
public class JCPVEPlotController : MonoBehaviour {
	
	private enum MoveDir
	{
		Left,
		Right,
		None,
	}

	
	public UIGrid uiGrid;
	List<JCPVEPlotMap> List_Maps = new List<JCPVEPlotMap>();
	public Dictionary<int,JCPVEPlotFloor>  Dic_Floors =  new Dictionary<int, JCPVEPlotFloor>();
	private JCUICenterOnChild uicenter;
	public UIScrollView _uiscrollview;
	[HideInInspector]
    public int CurMapIndex = 0;
	
	public static JCPVEPlotController Instance;
	
	//关闭按钮回调
	public System.Action<string> Exit;
	
	Dictionary<int,NewFloor> FloorList = null;
	
	public GameObject Btn_Left;
	
	public GameObject Btn_Right;
	
	public UILabel Lab_Title;
	
	public GameObject BuildingJianTou;
	
	public UIPanel JianTouLayer;
	
	private NewFloor CurOpenFloor = null;
	
	NewDungeonsManager NDManager = null;
	
	//箭头所在的地图ID
	private int ArrowAtMapID = 0;
	
	//临时跳转关卡[掉落追踪]
	public static int tempOpenFloorID = 0;
	
	public JCUIscrollViewPoint points;

	//pve掉落提示UI
	public PveRewardTipUI m_rewardTipUI;
	
	
	Dictionary<int, NewChapter> list_chapter = null;
	
	void Start ()
	{
		NDManager = Core.Data.newDungeonsManager;
		list_chapter = NDManager.ChapterList;
		FloorList = NDManager.FloorList;
		
	    CreateMapObjectPool();

		uicenter = uiGrid.GetComponent<JCUICenterOnChild>();
		
		//地图点数(数量)
		int MapPointCount = LastFloorBelongMapID+1;
		
		//计算开启关所在地图<章节>
		if(tempOpenFloorID > 0)
		{
			//临时跳转
		    if(FloorList.TryGetValue(tempOpenFloorID,out CurOpenFloor))
			{
				ArrowAtMapID = (CurOpenFloor.BelongChapterID - NDManager.startChapterID )/100;
			}
		}
		else
		{
			//正常跳转到最新关
			int CurOpenFloorID = NDManager.lastFloorId + 1;		
			if(CurOpenFloorID > 60548) CurOpenFloorID = 60548;
			//如果关卡存在
			 if(FloorList.TryGetValue(CurOpenFloorID,out CurOpenFloor))
			{
				//计算箭头所在地图
			    ArrowAtMapID = (CurOpenFloor.BelongChapterID - NDManager.startChapterID )/100;
				//如果箭头所在地图没有解锁
				while(!isUnlockMap(ArrowAtMapID,false) && CurOpenFloorID > NDManager.startFloorID)
				{
					 CurOpenFloorID --;
					 if(FloorList.TryGetValue(CurOpenFloorID,out CurOpenFloor))
					{
					    ArrowAtMapID = (CurOpenFloor.BelongChapterID - NDManager.startChapterID )/100;
						MapPointCount = ArrowAtMapID+1;
					}
				}
			}
		}
		
		CurMapIndex = ArrowAtMapID;
		
		//创建点
		points.Refresh(MapPointCount);
		points.SetBright(ArrowAtMapID);
		
		uicenter.onFinished += MoveFinished;
		uicenter.onStartMove += MoveStarted;
		
#region 初始化最近的三张地图		
		for(int i=-1;i<=1;i++)
		{
			NewChapter nchapter = null;
			int chapterID = 30100+(ArrowAtMapID+i)*100;
			list_chapter.TryGetValue(chapterID,out nchapter);
		    List_Maps[i+1].SetMap(nchapter);
	        if(i==0)
		    SetTitle(chapterID);
		}
		if(CurMapIndex == 0 && !isUnlockMap(1,false)) _uiscrollview.enabled = false;
#endregion	
		
#region 设置箭头的坐标		
		SetJianTouPos(CurOpenFloor);
#endregion
		
		
        //检测最新漫画 是否 下载 
        if (Core.Data.usrManager.UserConfig.cartoon == 1)
        {
            UIDownloadTexture ud = new UIDownloadTexture();
            StartCoroutine( ud.PreDownload((Core.Data.newDungeonsManager.lastFloorId + 2).ToString() + "-1.jpg"));
            StartCoroutine( ud.PreDownload((Core.Data.newDungeonsManager.lastFloorId + 2).ToString() + "-2.jpg"));
        }
	}

	//如果地图已经存在了,这个时候要跳转到某个一关卡时使用这个函数
	public void OpenPVEWhenExist(int floorID)
	{
		int MapPointCount = LastFloorBelongMapID+1;
		if(tempOpenFloorID > 0)
		{
			//临时跳转
			if(FloorList.TryGetValue(tempOpenFloorID,out CurOpenFloor))
			{
				ArrowAtMapID = (CurOpenFloor.BelongChapterID - NDManager.startChapterID )/100;
			}

			CurMapIndex = ArrowAtMapID;
			
			//创建点
			points.Refresh(MapPointCount);
			points.SetBright(ArrowAtMapID);
			
//			uicenter.onFinished += MoveFinished;
//			uicenter.onStartMove += MoveStarted;
			
			#region 初始化最近的三张地图		
			for(int i=-1;i<=1;i++)
			{
				NewChapter nchapter = null;
				int chapterID = 30100+(ArrowAtMapID+i)*100;
				list_chapter.TryGetValue(chapterID,out nchapter);
				List_Maps[i+1].SetMap(nchapter);
				if(i==0)
					SetTitle(chapterID);
			}
			if(CurMapIndex == 0 && !isUnlockMap(1,false)) _uiscrollview.enabled = false;
			#endregion	
			
			#region 设置箭头的坐标		
			SetJianTouPos(CurOpenFloor);
			#endregion

		}
	}



	void OnDestroy()
	{
		tempOpenFloorID = 0;
	}
	
	
	public static JCPVEPlotController OpenUI()
	{
		if(Instance == null)
		{
			Object prefab = PrefabLoader.loadFromPack("JC/JCPVEPlotController");
			if(prefab != null)
			{
				GameObject obj = Instantiate(prefab) as GameObject;
				RED.AddChild(obj, DBUIController.mDBUIInstance._PVERoot.gameObject);
				Instance = obj.GetComponent<JCPVEPlotController>();			
				DBUIController.mDBUIInstance._PVERoot.AddPage(obj.name,obj);
			}
		}
		return Instance;
	}
	
	//创建地图对象池
	void CreateMapObjectPool()
	{
		Object prefab = PrefabLoader.loadFromPack("JC/JCPVEPlotMap");
		for(int i=-1;i<2;i++)
		{
		   GameObject map = Instantiate(prefab) as GameObject;
			map.transform.parent = uiGrid.transform;
			map.transform.localPosition = new Vector3(uiGrid.cellWidth*i,0,0);
			map.transform.localScale = Vector3.one;
			map.name = i.ToString();
			JCPVEPlotMap jcmap = map.GetComponent<JCPVEPlotMap>();
			if(jcmap!=null)
			{
				//jcmap.SetMap(data);
			    List_Maps.Add(jcmap);
			}
			//map.SetActive(false);
		}
	}
	//创建地图
	void CreateMap ()
	{
		Object prefab = PrefabLoader.loadFromPack("JC/JCPVEPlotMap");
		if( prefab != null)
		{
			Dictionary<int, NewChapter> list_chapter = Core.Data.newDungeonsManager.ChapterList;
			int i = -1;
			foreach(NewChapter data in list_chapter.Values)
			{
				i++;
	            GameObject map = Instantiate(prefab) as GameObject;
				map.transform.parent = uiGrid.transform;
				map.transform.localPosition = new Vector3(uiGrid.cellWidth*i,0,0);
				map.transform.localScale = Vector3.one;
				map.name = i.ToString();
				JCPVEPlotMap jcmap = map.GetComponent<JCPVEPlotMap>();
				if(jcmap!=null)
				{
					jcmap.SetMap(data);
				    List_Maps.Add(jcmap);
				}
				map.SetActive(false);
			}
		}
	}
	
	void MoveMap(MoveDir dir)
	{
		if(dir == MoveDir.Right)
		{
			if(CurMapIndex + 1 < list_chapter.Count)
			{
				uicenter.CenterOn(List_Maps[List_Maps.Count-1].transform);
			}
		}
		else if(dir == MoveDir.Left)
		{
			if(CurMapIndex - 1 > -1)
			{
				uicenter.CenterOn(List_Maps[0].transform);
				
			}
		}
	}	
	
	void MoveStarted()
	{
		_uiscrollview.enabled = false;
		BuildingJianTou.SetActive(false);
	}
	
	void MoveFinished()
	{		
		#region 设置箭头的坐标		
		SetJianTouPos(CurOpenFloor);
		#endregion
		
		int MapID = System.Convert.ToInt32( uicenter.centeredObject.name );
		points.SetBright(MapID);

		CurMapIndex = MapID;
		
#region 设置标题
		SetTitle(GetChapterIDByMapID(CurMapIndex));
#endregion
		
		
		int LastMapID =int.Parse( List_Maps[List_Maps.Count-1].gameObject.name );
		if(CurMapIndex == LastMapID)
		{
			    JCPVEPlotMap map = List_Maps[0];
				//map.gameObject.name = (CurMapIndex+1).ToString();
				Vector3 pos = List_Maps[List_Maps.Count-1].transform.localPosition;
				pos.x += uiGrid.cellWidth; 
				map.transform.localPosition = pos;
				NewChapter nchapter = null;
				list_chapter.TryGetValue(30100+(CurMapIndex+1)*100,out nchapter);
			    map.SetMap(nchapter);
				List_Maps.RemoveAt(0);				
				List_Maps.Add(map);
		}
		else
	    {
			int FristMapID =int.Parse( List_Maps[0].gameObject.name );		
			if(CurMapIndex == FristMapID)
			{
				    JCPVEPlotMap map = List_Maps[List_Maps.Count-1];
					//map.gameObject.name = (CurMapIndex-1).ToString();
					Vector3 pos = List_Maps[0].transform.localPosition;
					pos.x -= uiGrid.cellWidth; 
					map.transform.localPosition = pos;
				    NewChapter nchapter = null;
					list_chapter.TryGetValue(30100+(CurMapIndex-1)*100,out nchapter);
				    map.SetMap(nchapter);
					List_Maps.RemoveAt(List_Maps.Count-1);				
					List_Maps.Insert(0,map);
			}
	     }
		_uiscrollview.enabled = true;
	}
	
	
	void SetTitle(int ChapterID)
	{
		if(ChapterID == Core.Data.newDungeonsManager.startChapterID)
		{
			if(Btn_Left.activeSelf) Btn_Left.SetActive(false);
		}
		else if(ChapterID == Core.Data.newDungeonsManager.endChapterID)
		{
			if(Btn_Right.activeSelf) Btn_Right.SetActive(false);
		}
		else
		{
			if(!Btn_Left.activeSelf)Btn_Left.SetActive(true);
			if(!Btn_Right.activeSelf)Btn_Right.SetActive(true);
		}
		
		NewChapter chapter = null;
		if(list_chapter.TryGetValue(ChapterID,out chapter))
		{
			Lab_Title.text = chapter.config.name;
		}		
	}
	
	
	
	void OnBtnClick(GameObject btn)
	{
		OnBtnClick(btn.name);
	}
	
	public void OnBtnClick(string btnName)
	{
		switch(btnName)
		{
		case "Btn_Left":
			if(_uiscrollview.enabled)
			MoveMap(MoveDir.Left);
			break;
		case "Btn_Right":			
			if(_uiscrollview.enabled || CurMapIndex == 0)
			{			
				//当前地图是否已通关
				if( !isPassCurMap(CurMapIndex))
				{
					SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(9081));
					return;
				}				
				int MapID = CurMapIndex + 1;
				//当前地图是否已解锁
				if( ! isUnlockMap(MapID)) return;
				MoveMap(MoveDir.Right);
			}		
			break;
            case "Btn_Close":
                if (PVEDownloadCartoonController.Instance != null)
                {
                    if (PVEDownloadCartoonController.Instance.curShowStatus == true)
                    {
                        PVEDownloadCartoonController.Instance.CloseCartoonPanel(true);
                        if(JCPVEPlotDes.Instance == null) 
                            Lab_Title.transform.parent.gameObject.SetActive(true);
                        return;
                    }
                }
			if(JCPVEPlotDes.Instance != null) 
			{
				JCPVEPlotDes.Instance.Close();
				Lab_Title.transform.parent.gameObject.SetActive(true);
			}
			else
		        DestoryMe();
			break;
		}
	}
	
	//销毁自身<释放内存>
	void DestoryMe()
	{
		Instance = null;
		if(Exit != null) Exit(gameObject.name);
		Destroy(gameObject);
		if (PVEDownloadCartoonController.Instance != null)
		{
			PVEDownloadCartoonController.Instance.ClosePanel ();
		}
	}
	
	
	
	//建筑物被点击了
	void OnBuildingClick(GameObject btn)
	{
		OnBuildingClick (btn.name);
	}
	
	
	//建筑物被点击了
	public void OnBuildingClick(string btnName)
	{
		int buildingID = 0;
		int.TryParse(btnName,out buildingID);
		NewFloor floorData = null;
		if(FloorList == null) FloorList = Core.Data.newDungeonsManager.FloorList;
		if(FloorList.TryGetValue(buildingID,out floorData))
		{
			if (floorData.state != NewFloorState.Unlocked)
			{
				// wxl    打过的小关卡  要能点击 看漫画
				if (buildingID <= Core.Data.newDungeonsManager.lastFloorId)
				{
					NewFloorData tNewData = Core.Data.newDungeonsManager.GetFloorData (buildingID).config;
					if (tNewData != null)
					{
						if (tNewData.isBoss != 1)
						{
							PVEDownloadCartoonController.CreateCartoonPanel (tNewData, true);
							Lab_Title.transform.parent.gameObject.SetActive (false);
							Debug.Log (" building click ");
							PVEDownloadCartoonController.Instance.HideDownBtn (false);
							return;
						}
					}
				}
				if (Core.Data.playerManager.RTData.curLevel >= floorData.config.Unlock)
				{
					Lab_Title.transform.parent.gameObject.SetActive (false);
					JCPVEPlotDes.OpenUI (floorData);
				}
				else
				{
					string strText = Core.Data.stringManager.getString (9094);
					strText = string.Format (strText, floorData.config.Unlock);
					SQYAlertViewMove.CreateAlertViewMove (strText);
				}
			}
		}
	}

	public void SetJianTouPos(NewFloor curOpenFloor)
	{		
		int TargetMapID = (curOpenFloor.BelongChapterID -30100)/100;
		if(TargetMapID < CurMapIndex-1 || TargetMapID > CurMapIndex+1)
		{
			if(BuildingJianTou.activeSelf)BuildingJianTou.SetActive(false);
			return;
		}
		
		if(Dic_Floors.ContainsKey(curOpenFloor.config.ID))
		{
			if(!BuildingJianTou.activeSelf)BuildingJianTou.SetActive(true);
			JCPVEPlotFloor _Buliding = Dic_Floors[curOpenFloor.config.ID];
			Transform parent = _Buliding.transform.parent;
			_Buliding.transform.parent = JianTouLayer.transform;
			Vector3 pos = _Buliding.transform.localPosition;		
			_Buliding.transform.parent = parent;
			pos.y += _Buliding.Spr_Buliding.height;

			if(!curOpenFloor.isBoss && curOpenFloor.config.TextrueID[0] =="common-0014" &&curOpenFloor.config.ID > Core.Data.newDungeonsManager.lastFloorId)
			{
				//如果当前是未开启的宝箱，箭头要再高一些
				pos.y+=60f;
			}
			else
			pos.y+=30f;
		    BuildingJianTou.transform.localPosition = pos;
		}
		
	}
	
	//打通的最新关所在的地图ID
	public int LastFloorBelongMapID
	{
		get
		{
			int OpenID = NDManager.lastFloorId + 1;
			NewFloor OpenData = null;
			if(NDManager.FloorList.TryGetValue(OpenID,out OpenData))
			{
				int mapid = (OpenData.BelongChapterID - NDManager.startChapterID)/100;
				//如果地图没有解锁则往前寻找
				while(!isUnlockMap(mapid,false) && mapid > 0)
				{
					mapid --;
				}
			    return mapid;
			}
			
			return 0;
		}		
	}
	
	//通过地图ID，获得地图章节ID
	public int GetChapterIDByMapID(int MapID)
	{
		return NDManager.startChapterID + MapID*100;
	}
	
	
	//查看地图是否达到解锁条件
	public bool isUnlockMap(int MapID,bool ShowPrompt = true)
	{
		int TargetChapterID = GetChapterIDByMapID(MapID);
		NewChapter chapter = null;
		NewFloor floor = null;
		if( NDManager.ChapterList.TryGetValue(TargetChapterID,out chapter))
		{
			if( chapter.config.floorID.Length > 0)
			{
				int floorID = chapter.config.floorID[0];
				if(NDManager.FloorList.TryGetValue(floorID,out floor))
				{
					if(Core.Data.playerManager.Lv >= floor.config.Unlock)
						return true;
				}
			}
	    }
		
		if(ShowPrompt)
		{
			if(chapter != null)
			{
				if(floor == null)
					SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(9109));
				else
				{					
					string temp_str = Core.Data.stringManager.getString(9110);
					temp_str = temp_str.Replace("{}",floor.config.Unlock.ToString());
					SQYAlertViewMove.CreateAlertViewMove(temp_str);				
				}
			}
		}
			return false;
	}
	
	
	//当前地图是否已通关
	public bool isPassCurMap(int MapID)
	{
		int ChapterID = GetChapterIDByMapID(MapID);
		NewChapter chapter = null;
		if(NDManager.ChapterList.TryGetValue(ChapterID, out chapter))
		{
			int[]floorID = chapter.config.floorID;
			if(floorID.Length > 0)
			{
				return !( NDManager.lastFloorId < floorID[floorID.Length-1] );
			}
		}
		return false;
	}
	
}
