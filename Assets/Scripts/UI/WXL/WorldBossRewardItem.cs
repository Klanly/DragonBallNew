using UnityEngine;
using System.Collections;

public class WorldBossRewardItem : RUIMonoBehaviour,IItem {
    #region IItem implementation
    private int id;
    private WorldBossRewardData myData;
    public UILabel lblName;
    //  public UILabel lblRank;
    public UILabel lblForewardNum;
    public UILabel lblBackwardNum;


    public  void SetItemValue (object obj)
    {
        myData = obj as WorldBossRewardData;
        this.Refresh ();
    }

    public object ReturnValue ()
    {
        return myData;
    }

    public  void Refresh ()
    {
        if (myData != null) {
            id = myData.id;
            lblName.text =  id +"."+ myData.rank;
            //      lblRank.text = myData.id.ToString ();
            lblForewardNum.text = myData.credit.ToString ();
            lblBackwardNum.text = myData.credit1.ToString ();
        }
    }

    #endregion



}
