using UnityEngine;
using System.Collections;

public class UIAnnounceCell : RUIMonoBehaviour {

    public UILabel mUIlable;
	public UISprite mBtn;

	readonly string c_BtnChoose = "Symbol 31";
	readonly string c_BtnUnchoose = "Symbol 32";

    private int m_Index;

    public void OnShow(int index, string mTitle)
    {
        m_Index = index;
		int height = mUIlable.height;
		int width = mUIlable.width;

		mUIlable.text = mTitle;

		int cha_height = mUIlable.height - height;
		int cha_width = mUIlable.width - width;

		mBtn.width = mBtn.width + cha_width;
		mBtn.height = mBtn.height + cha_height;
		mBtn.transform.parent.gameObject.GetComponent<BoxCollider>().size = new Vector3(mBtn.width,mBtn.height,0);
		Vector3 _center = mBtn.transform.parent.gameObject.GetComponent<BoxCollider>().center;
		mBtn.transform.parent.gameObject.GetComponent<BoxCollider>().center = new Vector3(_center.x, _center.y-cha_height/2, 0);
    }

	public void SetBtnStatus(bool m_IsChoose)
	{
		if(m_IsChoose)mBtn.spriteName = c_BtnChoose;
		else mBtn.spriteName = c_BtnUnchoose;
	}

    void OnDestroy()
    {
        mUIlable = null;
    }

	void OnClick()
	{
		AnnounceMrg.GetInstance().SetAnnounceContent(m_Index,this);
		//add by wxl
//		if(gameObject != null){
//			string tName = this.name.ToString ();
//			ControllerEventData ctrl = new ControllerEventData (name,"UIAnnounceCell");
//			ActivityNetController.GetInstance ().SendCurrentUserState (ctrl);
//		}
	}

    void Btn_OnClick()
    {
		AnnounceMrg.GetInstance().SetAnnounceContent(m_Index,this);
    }
}
