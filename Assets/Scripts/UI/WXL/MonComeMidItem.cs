using UnityEngine;
using System.Collections;

/// <summary>
/// 魔王来袭 中间的列表
/// </summary>
public class MonComeMidItem : RUIMonoBehaviour,IItem {
	string id;
    string myName;
	string TitleNum;
	string winNum;
	string loseNum;

	public UILabel lbl_name;
	public UILabel lbl_winNum;
	public UILabel lbl_loseNum;

	public UILabel lbl_Title;

	public void SetItemValue(object obj){
		id = (string)obj;
		string[] strList = id.Split (',');
        myName = strList [0];
		winNum = strList [1];
		loseNum = strList[2];
		TitleNum = strList [3];
		this.Refresh ();
	}
	public object ReturnValue(){
		return id;
	}

	public void Refresh(){
        lbl_name.text = myName; 
		lbl_winNum.text = winNum;
		lbl_loseNum.text = loseNum;
		lbl_Title.text = TitleNum;
	}

}
