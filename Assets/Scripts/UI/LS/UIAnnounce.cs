using UnityEngine;
using System.Collections;

public class UIAnnounce : RUIMonoBehaviour {

    public UILabel mUIlabel;
    public GameObject obj;

    public System.Action callBackWhenExit = null;
	public UIGrid m_Grid;
	public UIScrollBar m_ScrollBar;
	UITable mGrid2;


    public void OnShow()
    {
		if(mGrid2 == null)
		{
			if(obj != null)
			{
				mGrid2 = obj.GetComponent<UITable>();
			}

		}
		Invoke("SendMeg", 0.3f);
		ComLoading.Open();
    }

    public void SetContent(string mContent)
    {
        mUIlabel.text = mContent;
		BoxCollider _box = null;
		if(mUIlabel.gameObject.GetComponent<BoxCollider>() ==  null)
		{
			_box = mUIlabel.gameObject.AddComponent<BoxCollider>();
		}
		else
		{
			_box = mUIlabel.gameObject.GetComponent<BoxCollider>();
		}
		Vector3 _boxcollider = new Vector3(mUIlabel.width, mUIlabel.height, 0f);
		_box.size = _boxcollider;
		_box.center = new Vector3(mUIlabel.width/2, -mUIlabel.height/2, 0f);

		m_ScrollBar.value = 0f;
		m_Grid.Reposition();
    }

	void SendMeg()
	{
		AnnounceMrg.GetInstance().AnnounceRequest(obj);
	}

    public void Back_OnClick()
    {

        if (NoticeManager.openSign == true) {
//            if (NoticeManager.firstShowState == 1) {
//                UISevenDayRewardMain.OpenUI ();
//            } else 
                if(NoticeManager.firstShowState == 2 ){
                WXLAcitvityFactory.CreatActivity (ActivityItemType.qiandaoItem, (object)DBUIController.mDBUIInstance._TopRoot   );  
            }
        }
		AnnounceMrg.GetInstance().DeleteCell();
		Destroy (gameObject);
    }

	public void Reposition()
	{
		if(mGrid2 != null)
		{
			mGrid2.Reposition();
		}
	}

	void OnDestroy()
	{
		mUIlabel = null;
		obj = null;
	}
}
