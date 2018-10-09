using UnityEngine;
using System.Collections;

public class UIMallVipBarCell : RUIMonoBehaviour {

    public UILabel mName;
    public UILabel mContent;
    public UIButton mBtn;



    public void OnShow(int n)
    {
		mName.text = Core.Data.vipManager.GetVipGiftData(n).name;
		mContent.text = Core.Data.vipManager.GetVipGiftData(n).desc;
    }

    void Buy_OnClick()
    {

    }
}
