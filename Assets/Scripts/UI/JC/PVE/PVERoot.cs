using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PVERoot : MonoBehaviour {
	[HideInInspector]
	public Dictionary<string,GameObject> Dic_Pages = new Dictionary<string, GameObject>();
	
	void Start () 
	{
	
	}
	

	
	public void AddPage(string PageName,GameObject Page)
	{
		if(Dic_Pages.ContainsKey(PageName))
		   Dic_Pages[PageName] = Page;
		else
		   Dic_Pages.Add(PageName,Page);
	}
	
	//初始化PVE系统
	public void ResetPVESystem()
	{
		foreach(GameObject page in Dic_Pages.Values)
		{
			if(page != null)
				Destroy(page);
		}
		Dic_Pages.Clear();
		DBUIController.mDBUIInstance.ShowFor2D_UI(false);
		DBUIController.mDBUIInstance.pveView.SetActive(false);
		Resources.UnloadUnusedAssets();
	}	
	
	
	public void SetActive(bool _value)
	{
		gameObject.SetActive(_value);
	}
}
