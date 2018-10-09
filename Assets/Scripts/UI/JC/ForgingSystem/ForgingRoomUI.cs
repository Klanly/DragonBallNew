using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace FrogingSystem
{

	public enum ForgingPage
	{
		Forging_Main,
		Forging_Synthetic,
		Forging_Mosaic,
		Forging_Recasting,
	}


	//本类是整个锻造系统的控制类
	public class ForgingRoomUI : MonoBehaviour {
	
		//宝石系统所有功能模块(对应ForgingRoomUI_Logic)
		public List<GameObject> All_Pages=new List<GameObject>();
		//当前停留的界面
		public ForgingPage CurPage{get;set;}
	
		public FrogingTopUI frogingTopUI;
		
		public System.Action Exit = null;
		
		static ForgingRoomUI m_Instance = null;
		
		public  static ForgingRoomUI  Instance 
		{
			get
			{
				return m_Instance;
			}
		}
		
		void OnDestroy()
		{
		    m_Instance = null;
		}
		
		
		/*合成系统指针
		 * */
		GemSyntheticSystemUI_Logic _syntheticSystemInstance;
		public GemSyntheticSystemUI_Logic SyntheticSystem
		{
			get
			{
				if(_syntheticSystemInstance==null)
					_syntheticSystemInstance=All_Pages[1].GetComponent<GemSyntheticSystemUI_Logic>();
				return _syntheticSystemInstance;
			}
		}
		/*镶嵌系统指针
		 * */
		GemInlaySystemUI_Logic _inlaySystemInstance;
		public GemInlaySystemUI_Logic InlaySystem
		{
			get
			{
				if(_inlaySystemInstance==null)
					_inlaySystemInstance=All_Pages[2].GetComponent<GemInlaySystemUI_Logic>();
				return _inlaySystemInstance;
			}
		}
		
		/*重铸系统指针
		 * */
		GemRecastingSystemUI_Logic _gemRecastingSystemInstance;
		public GemRecastingSystemUI_Logic RecastingSystem
		{
			get
			{
				if(_gemRecastingSystemInstance==null)
					_gemRecastingSystemInstance=All_Pages[3].GetComponent<GemRecastingSystemUI_Logic>();
				return _gemRecastingSystemInstance;
			}
		}		
		
		public static void OpenUI(params System.Action [] action)
		{
			if (Instance == null)
			{		    
				Object prefab = PrefabLoader.loadFromPack ("JC/ForgingRoomUI");
				if (prefab != null)
				{
					GameObject obj = Instantiate (prefab) as GameObject;
					RED.AddChild (obj, DBUIController.mDBUIInstance._bottomRoot);
					m_Instance = obj.GetComponent<ForgingRoomUI> ();
				    m_Instance.CurPage=ForgingPage.Forging_Main;
					if(action.Length > 0)
					m_Instance.Exit = action[0];
				}
				
			}
		}
	
		//销毁锻造系统
		public 	void DestoryForgingRoomUI()
		{
			Destroy(gameObject);
			DBUIController.mDBUIInstance.ShowFor2D_UI ();
		    m_Instance=null;
		}
	
		//显示隐藏锻造系统
		private bool _visible=true;
	    public bool Visible
		{
			get
			{
				return _visible;
			}
			set
			{
				if(gameObject.activeSelf==!value)
				{
					_visible=value;
					RED.SetActive(_visible,gameObject);
				}					
			}
		}
	
	    
		//跳转到某个子界面
		public void GoTo(ForgingPage page)
		{			
			CurPage=page;			
            //  wxl change
            //frogingTopUI.ShowBg = CurPage!=ForgingPage.Forging_Main;
            if (CurPage != ForgingPage.Forging_Main && CurPage != ForgingPage.Forging_Synthetic)
            {
                frogingTopUI.ShowBg = true;
            }
            else
            {
                frogingTopUI.ShowBg = false;
            }
			int index=(int)page;
		   		
			for(int i=0;i<All_Pages.Count;i++)
			{
				if(i==index)
			    {
				    All_Pages[i].gameObject.SetActive(true);
			     }
				else
			    {	
				     All_Pages[i].gameObject.SetActive(false);
			    }
			}
		}

		public void GoToAndSetDia(ForgingPage page, Gems gem){

			this.GoTo (page);
			AsyncTask.QueueOnMainThread (() => {
				SyntheticSystem.SelectGem (gem);
			}, 0.1f);
			ComLoading.Close ();

		}
	
	    public void ReturnLastPage()
	    {
			if(ForgingRoomUI.Instance.CurPage!=ForgingPage.Forging_Main)
			{
					if(Exit == null)
					{
						switch(CurPage)
						{
						case ForgingPage.Forging_Synthetic:
							SyntheticSystem.Quit();
							break;
						case ForgingPage.Forging_Mosaic:
							InlaySystem.Quit();
							break;
						case ForgingPage.Forging_Recasting:
							RecastingSystem.Quit();
							break;
						}
						ForgingRoomUI.Instance.GoTo(ForgingPage.Forging_Main);
					}
					else
					{
						Exit();
					}
				
			}			  
		    else
			{
				if(Exit == null)
				{
					ForgingRoomUI.Instance.DestoryForgingRoomUI();
					Core.Data.playerManager.RTData.curTeam.upgradeMember();
					//Debug.Log("jc------RefreshUserInfo------");
					//DBUIController.mDBUIInstance.RefreshUserInfo();
					DBUIController.mDBUIInstance._mainViewCtl.RefreshUserInfo();
				}
				else
					Exit();
			}
	    }
	}
}
