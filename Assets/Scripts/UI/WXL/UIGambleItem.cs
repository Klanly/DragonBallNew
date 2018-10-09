using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIGambleItem :RUIMonoBehaviour,IItem {

    public int id {
        set;
        get;
    }
    public UILabel lblMoney;
    public UILabel lblTitle;
    public UILabel lblWinMoney;
    public UILabel lblGambleDesp;
    public UIButton GotBtn;

    GambleItem curGambleData;

    public void SetItemValue(object obj){
        curGambleData = obj as GambleItem;
        id = curGambleData.id;
        this.Refresh ();
    }


    public object ReturnValue ()
    {
        return (object)curGambleData;
    }


    public void Refresh ()
    {
        if (curGambleData != null) {
            lblMoney.text = curGambleData.cost.ToString ();

            if (Core.Data.playerManager.RTData.curCoin >= curGambleData.cost) {
                GotBtn.isEnabled = true;
            } else
                GotBtn.isEnabled = false;

            if (curGambleData.type == 1) {
                //1v 5
                lblTitle.text = string.Format (Core.Data.stringManager.getString (7337), curGambleData.condition);
                lblGambleDesp.text = string.Format (Core.Data.stringManager.getString(7355),curGambleData.condition);
            } else {
                // skill 向上取整
               
                float tnum = (float)curGambleData.condition *(float) Core.Data.playerManager.RTData.curTeam.validateMember/100.0f;
                float skillNum = Mathf.Round (tnum+0.5f);
               
                lblTitle.text = string.Format (Core.Data.stringManager.getString (7338), (int)skillNum);
            }
            lblWinMoney.text = curGambleData.win.ToString ();
        }

    }

    void OnBuyGamble(){
        if (Core.Data.playerManager.RTData.curCoin < curGambleData.cost) {
            ActivityNetController.ShowDebug (Core.Data.stringManager.getString(35000));
            return;
        }
        Core.Data.temper.gambleTypeId = id;
        UIGambleController.Instance.OnBack ();
        Core.Data.playerManager.ReduceCoin (-curGambleData.cost);
        ComLoading.Open ();
        GotBtn.isEnabled = false;
    }
}
