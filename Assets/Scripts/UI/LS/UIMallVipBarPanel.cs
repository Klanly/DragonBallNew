using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIMallVipBarPanel : RUIMonoBehaviour {

    private List<UIMallVipBarCell> mUIMallVipBarCell;
    public GameObject ShowRoot;
    public UIGrid mGrid;
    public UIScrollView mScrollView;
    
    void Start ()
    {
        mGrid = gameObject.GetComponentInChildren<UIGrid>();
        mScrollView = gameObject.GetComponent<UIScrollView>();
        GameObject obj = PrefabLoader.loadFromPack("LS/pbLSMallVipBar")as GameObject ;
        mUIMallVipBarCell = new List<UIMallVipBarCell>();
        if(obj != null)
        { 
			for(int i=0; i<Core.Data.vipManager.GetVipGiftDataListConfig().Count; i++)
            {
                GameObject go = NGUITools.AddChild (ShowRoot, obj);
                go.gameObject.name = "pbLSMallVipBar" + i.ToString();
                UIMallVipBarCell m_UIMallVipBarCell = go.gameObject.GetComponent<UIMallVipBarCell> ();
                mUIMallVipBarCell.Add (m_UIMallVipBarCell);

            }
        }
        ShowAllScript();
    }

    void ShowAllScript()
    {
//        int m_Temp2 = mitems.Count;
        for(int i=13,j=0; i>0; i--,j++)
        {
            mUIMallVipBarCell[i-1].OnShow(j+1);
        }
        mScrollView.ResetPosition();
    }
}
