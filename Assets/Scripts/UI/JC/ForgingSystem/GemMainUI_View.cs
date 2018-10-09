using UnityEngine;
using System.Collections;
using FrogingSystem;
//宝石系统主界面显示层
public class GemMainUI_View : MonoBehaviour 
{
	public  FrogingTopUI  frogingTopUI;
	#region 代理
	public System.Action <GameObject> ButtonClick;
	#endregion
	
    //宝石系统逻辑层实例
	public  static  GemMainUI_View  m_Instance = null;
	
	public UILabel Lab1;
	public UILabel Lab2;
	public UILabel Lab3;
	
	void Start()
	{
		Lab1.text = TEXT(5015)+TEXT(5055);
		Lab2.text = TEXT(5015)+TEXT(9003);
		Lab3.text = TEXT(9052)+TEXT(9005);
	}
	
	
	public  GemMainUI_View  Instance 
	{
		get
		{

			return m_Instance;
		}
	}
	
	
	public void GemMainUI_Btn(GameObject Btn)
	{
		if(ButtonClick!=null)
			ButtonClick(Btn);
	}
	
	void OnEnable()
	{
		//BaseBuildingData build = 
		Core.Data.BuildingManager.GetConfigByBuildLv (BaseBuildingData.BUILD_YELIAN, 1);
		frogingTopUI.SetTitle("bsxt_zbdzw");
	}
	
	public string TEXT(int num_text)
	{
		return	Core.Data.stringManager.getString(num_text);
	}
}
