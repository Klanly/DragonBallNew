using UnityEngine;
using System.Collections;

public class ActItem : RUIMonoBehaviour,IItem {

	public UILabel Lbl_actName;
	public UISprite Usp_actIcon;
	public UISprite Usp;
	public UISprite StateIcon;
	//num
	int Id;
	string actName;
	string actIcon;
	bool isOpen = false;

	private  ActItemData myData;
	/// <summary>
	/// Sets the item value.    data  = {id , name, icon   ,}
	/// </summary>
	/// <param name="obj">Object.</param>
	public void SetItemValue(object obj){
		myData = obj as ActItemData;
		if (myData != null) {
//			string objStr = obj.ToString ();
//			string[] tStr = (objStr).Split (',');
			Id = myData.Id;// int.Parse (tStr [0]);
			actName = myData.actName;// tStr [1];
			actIcon = myData.actIcon;//  tStr [2];

				//Id = 
		}
		this.Refresh();
	}
	public object ReturnValue(){
		return (object)Id;
	}

	public void Refresh(){
		Lbl_actName.text = actName;
		Usp.spriteName = actIcon;
        Usp.MakePixelPerfect ();
	}

	public void ShowState(int tState){
        // 公告 
     
        if (tState == 1)
            this.isOpen = true;
		else
            this.isOpen = false;
		this.StateCtrl ();
	}

	void StateCtrl(){
        if (isOpen == false) {
            StateIcon.gameObject.SetActive (false);
        } else {
            StateIcon.gameObject.SetActive (true);
        }
    }

}

public class ActItemData{
	public int Id;
	public string actName;
	public string actIcon;
	public ActItemData(int tId,string tActName,string tActIcon){
		Id = tId;
		actName = tActName;
		actIcon = tActIcon;
	}
}
