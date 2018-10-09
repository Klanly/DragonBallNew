using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChapterShowCellScript : MonoBehaviour {

	/// <summary>
	/// 不显示时候的蒙版
	/// </summary>
	public UISprite MarkSprite;//
	/// <summary>
	/// 章节的名字
	/// </summary>
	public UILabel NameLabel;//
	/// <summary>
	/// 第几章
	/// </summary>
	public UILabel TitleLabel;//

	/// <summary>
	/// 章节的图片
	/// </summary>
	public UISprite ChapterSprite;//
	[HideInInspector]


	/// <summary>
	/// 本关卡是否可以进入
	/// </summary>
	private bool _CanEnter;


	private Chapter _MyChapter;
	
	public UISprite uinew;
	
	public BoxCollider Btn_Drop;
	
	/// <summary>
	/// 显示章节信息
	/// </summary>
	/// <param name="">.</param>
	/// <param name="chapterID">Chapter I.</param>
	public void Show(Chapter c,string title,string iconName)
	{
//		Debug.Log("----------------Show---------------");
		
		if (c.status == DungeonsData.STATUS_NEW) 
		{
			MarkSprite.gameObject.SetActive (true);
			Btn_Drop.enabled = false;
			
			_CanEnter = false;
		} 
		else if (c.status == DungeonsData.STATUS_CLEAR) 
		{
			_CanEnter = true;
			MarkSprite.gameObject.SetActive (false);
			Btn_Drop.enabled = true;
		} 
		else if (c.status == DungeonsData.STATUS_ONGOING) 
		{
			_CanEnter = true;
			MarkSprite.gameObject.SetActive (false);
			Btn_Drop.enabled = true;
		}
		_MyChapter = c;
		
		TitleLabel.text = title;
		NameLabel.text = c.config.name;
		ChapterSprite.spriteName = iconName;
	}
	
	void OnClick()
	{
		if (_CanEnter) 
		{
			//	Debug.Log (_CanEnter);
			//Debug.Log(_MyChapter.config.ID.ToString()+"OnClick<<<<<<<<<<<<"+"_MyChapter.toFightCityID:"+_MyChapter.toFightCityId.ToString());

//			if(_MyChapter != null){
//				string tName = _MyChapter.config.name.ToString ();
//				ControllerEventData ctrl = new ControllerEventData (name,"ChapterShowCellScript");
//				ActivityNetController.GetInstance ().SendCurrentUserState (ctrl);
//			}
			
			DBUIController.mDBUIInstance.SetViewStateWithData(RUIType.EMViewState.S_CityFloor, _MyChapter);
            UIMiniPlayerController.Instance.SetActive (false);

			#if SPLIT_MODEL
            if(_MyChapter.config.ID >= 30300 && Core.Data.temper.BattleDownloadCnt == 0 && Core.Data.playerManager.RTData.downloadReawrd == 0)
			{
                List<SouceData> list = Core.Data.sourceManager.GetUpdateModes();
                if(list != null || list.Count > 0)
                {
                    UIDownloadPacksWindow.OpenDownLoadUI(null);
                }
                Core.Data.temper.BattleDownloadCnt++;
			}
			#endif
		}
	}

	public void Refresh()
	{
		_CanEnter = true;
		MarkSprite.gameObject.SetActive (false);
		Btn_Drop.enabled = true;
		
		DungeonsManager dm = Core.Data.dungeonsManager;
		int lastfloorid=  dm.lastFloorId;
		

	    int currchapter = System.Convert.ToInt32(gameObject.name);
		ChapterData chapter = dm.reverseToChapterMore(lastfloorid);
		if(chapter != null)
		{
			//Debug.Log(" chapter ="+chapter.ID.ToString() +"  currchapter ="+ currchapter.ToString());
			if(currchapter - chapter.ID == 100)
			{
				uinew.enabled = true;
			}
			else
			{
				uinew.enabled = false;
			}
		}
	}
	
	
	//查看掉落
	public void BtnClick_LookDropReward(GameObject btn)
	{
		DropRewardPanel.OpenUI(System.Convert.ToInt32(name));
	}
	
	
}
