using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class JCPanelMoveWidget : MonoBehaviour {

	public UIGrid uiGrid;
	public UIPanel  uiPanel;
	public List<GameObject> list_items = new List<GameObject>();
	int PageCount = 0;
	public UIScrollView _uiScrollView;
	void Start () 
	{
	     if(uiPanel == null) uiPanel = GetComponentInChildren<UIPanel>();
		if(uiGrid == null) uiGrid = GetComponentInChildren<UIGrid>();
		if(_uiScrollView == null) _uiScrollView = GetComponentInChildren<UIScrollView>();
		PanelInitY = uiPanel.transform.localPosition.y;
		Debug.Log(PanelInitY.ToString());
	}
	
	//创建元素
	public void CreateElement(int num)
	{
		PageCount = num;
		uiPanel.alpha = 0;
		Object prefab = PrefabLoader.loadFromPack("JC/JCLabelCeshi");
		if(prefab != null)
		{
			for(int i = 0;i<num ;i++)
			{
				GameObject obj = Instantiate(prefab) as GameObject;
				RED.AddChild(obj, uiGrid.gameObject);
				obj.name = i.ToString();
				list_items.Add(obj);
			}
		}
		
		uiGrid.repositionNow = true;
		uiPanel.alpha = 1;
	}
	
	
	void BtnClick(GameObject btn)
	{
		switch(btn.name)
		{
		case "ButtonLeft":
			if(PageIndex > 0)
			{
			     PageIndex --;
				 GoToPage(PageIndex);
			}
			break;
		case "ButtonRight":
			if(PageIndex < PageCount-1)
			{
			     PageIndex ++;
				 GoToPage(PageIndex);
			}
			break;
		}
	}
	
	
	private int PageIndex = 0;
	float PanelInitY = -75f;
	
	void GoToPage(int index)
	{
		_uiScrollView.enabled = false;
		Vector3 pos = list_items[index].transform.localPosition;
		pos.y = -(uiPanel.transform.localPosition.y -PanelInitY);
		list_items[index].transform.localPosition = pos;
		SpringPosition.Begin(uiGrid.gameObject,new Vector3(-PageIndex*uiGrid.cellWidth,0,0),10).onFinished = OnSpringPositionFinished;
	}
	
	public  void OnSpringPositionFinished ()
	{
		Vector3 pos = list_items[PageIndex].transform.localPosition;
		pos.y = 0;
		list_items[PageIndex].transform.localPosition = pos;
	    //_uiScrollView.r
		uiPanel.GetComponent<UIScrollView>().ResetPosition();
		_uiScrollView.enabled = true;		
	}
}
