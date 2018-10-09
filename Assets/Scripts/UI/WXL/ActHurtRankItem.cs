using UnityEngine;
using System.Collections;

public class ActHurtRankItem : RUIMonoBehaviour,IItem {

	//public UILabel lblRank;
	public UILabel lblName;
	public UILabel lblHurt;

	string strName;
	public int rank;
	int hurtNum;
	UserAtkBossInfo userInfo;


    public void SetItemValue(object obj){
        userInfo = (UserAtkBossInfo)obj;
        //   Debug.Log ("user " + userInfo.userId);
		if (userInfo != null) {
			strName = userInfo.userName;
			hurtNum = userInfo.hurt;
		}
		else{
            strName = Core.Data.stringManager.getString (7354);
			hurtNum = 0;
		}
		this.Refresh ();
	}
	/// <summary>
	/// Returns the value.
	/// </summary>
	public object ReturnValue(){
		return rank;
	}

	public void Refresh(){
			if (hurtNum != -1) {
			lblName.text = rank  + ". " + strName;
			lblHurt.text = hurtNum.ToString ();
		} else {
			lblName.text = Core.Data.stringManager.getString (7110) + strName;
			lblHurt.text = "";
		}
	}


}
