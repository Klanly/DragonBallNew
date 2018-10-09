using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChapterSrollViewPanelScript : MonoBehaviour {

	/// <summary>
	/// 章节图片名称
	/// </summary>
	//public string[] ChapterSpriteNameArray;

	/// <summary>
	/// 显示根节点
	/// </summary>
	public GameObject ShowRoot;


	private List<ChapterShowCellScript> _MyShowList;
	
	public UIAtlas cptAtlsv1;
	public UIAtlas cptAtlsv2;
	
	bool isRunStart = false;
	void Start()
	{
		isRunStart = true;
//		Debug.Log("---------Start---------");
		
		GameObject obj = PrefabLoader.loadFromPack("GX/pbChapterShowCell")as GameObject ;

		ChapterShowCellScript script = null;

		DungeonsManager dm = Core.Data.dungeonsManager;

		_MyShowList = new List<ChapterShowCellScript> ();

		int i = 0;

		foreach (Chapter kp in dm.ChapterList.Values)
		{
			script = (NGUITools.AddChild (ShowRoot, obj)).GetComponent<ChapterShowCellScript> ();
			_MyShowList.Add (script);
			i++;
			if(i<9)
			script.ChapterSprite.atlas=cptAtlsv1;
			else
			script.ChapterSprite.atlas=cptAtlsv2;
							
			script.Show(kp,Core.Data.stringManager.getString(20001).Replace("#",i.ToString()),"A3Cpt"+i.ToString());
			
	        script.gameObject.name = kp.config.ID.ToString();
//			if (kp.config.ID == dm.fightChapterId)
//			{
//				script.Refresh ();
//			}
		}
		
		UIGrid grid = ShowRoot.GetComponent<UIGrid>();
		if(grid!=null)
		{
			grid.repositionNow = true;
		}
		Refresh();

			
		
	}
	
	void OnEnable()
	{
		//Debug.Log("---------OnEnable---------");
		if(isRunStart)
			Refresh();
	}
	

	public void Refresh()
	{
		DungeonsManager dm = Core.Data.dungeonsManager;
		
		int i = 0;
		foreach (Chapter kp in dm.ChapterList.Values) 
		{
			if (kp.config.ID == dm.fightChapterId) 
			{
				_MyShowList [i].Refresh ();
				return;
			}
			i++;
		}
	}

}
