using UnityEngine;
using System.Collections;

public class UITrailAddAttribute : RUIMonoBehaviour {

    public UILabel[] mLabelArray;
    public UILabel _HighestStar;
    public UILabel _Star;
    public UILabel[] _AddAtr1 ;
	public UILabel[] _AddAtr2;
	public UISprite BuOuSprite;
	public UISprite ShaLuSprite;

	public GameObject Type1;
	public GameObject Type2;

	NewFinalTrialAddBufferResponse mResponse;

    public void OnShow(BaseResponse response)
    {
        mResponse = response as NewFinalTrialAddBufferResponse;
		_Star.text = mResponse.data.ulbuffer.stars.ToString();

		bool key;
        if(FinalTrialMgr.GetInstance().NowEnum == TrialEnum.TrialType_PuWuChoose)  //防御方   0ak 1df 2sf 3eak 4edf 5esf
		{

			Type1.gameObject.SetActive(true);
			Type2.gameObject.SetActive(false);

			mLabelArray[0].text = Core.Data.stringManager.getString(6045);
			
			ShaLuSprite.gameObject.SetActive(false);
			BuOuSprite.gameObject.SetActive(true);

			SetDetail(mResponse.data.ulbuffer.bufferList[0].name,_AddAtr1[4], out key);
			SetNum(_AddAtr1[0], mResponse.data.ulbuffer.bufferList[0].buffer, key);
			
			SetDetail(mResponse.data.ulbuffer.bufferList[1].name,_AddAtr1[5], out key);
			SetNum(_AddAtr1[1], mResponse.data.ulbuffer.bufferList[1].buffer, key);
			
			SetDetail(mResponse.data.ulbuffer.bufferList[2].name,_AddAtr1[6], out key);
			SetNum(_AddAtr1[2], mResponse.data.ulbuffer.bufferList[2].buffer, key);
			
			SetDetail(mResponse.data.ulbuffer.bufferList[3].name,_AddAtr1[7], out key);
			SetNum(_AddAtr1[3], mResponse.data.ulbuffer.bufferList[3].buffer, key);
        }
		else                       //攻击方   0ak 1df 2sf 3eak 4edf 5esf
        {
			Type1.gameObject.SetActive(false);
			Type2.gameObject.SetActive(true);

			mLabelArray[0].text = Core.Data.stringManager.getString(25139);
			
			ShaLuSprite.gameObject.SetActive(true);
			BuOuSprite.gameObject.SetActive(false);

			SetDetail(mResponse.data.ulbuffer.bufferList[0].name,_AddAtr2[4], out key);
			SetNum(_AddAtr2[0], mResponse.data.ulbuffer.bufferList[0].buffer, key);
			
			SetDetail(mResponse.data.ulbuffer.bufferList[1].name,_AddAtr2[5], out key);
			SetNum(_AddAtr2[1], mResponse.data.ulbuffer.bufferList[1].buffer, key);
			
			SetDetail(mResponse.data.ulbuffer.bufferList[2].name,_AddAtr2[6], out key);
			SetNum(_AddAtr2[2], mResponse.data.ulbuffer.bufferList[2].buffer, key);
			
			SetDetail(mResponse.data.ulbuffer.bufferList[3].name,_AddAtr2[7], out key);
			SetNum(_AddAtr2[3], mResponse.data.ulbuffer.bufferList[3].buffer, key);
        }


		


        
        mLabelArray[1].text = Core.Data.stringManager.getString(25006);
        mLabelArray[2].text = Core.Data.stringManager.getString(25007);
        mLabelArray[3].text = Core.Data.stringManager.getString(25005);

        _HighestStar.text = FinalTrialMgr.GetInstance()._FinalTrialData.MostStar.ToString();
    }

	void SetNum(UILabel mm, int num, bool key)
	{
		if(key)
		{
			mm.text = "+" + num.ToString()+"%";
		}
		else
		{
			mm.text = "-" + num.ToString()+"%";
		}
	}

	void SetDetail(string str, UILabel content, out bool key)
	{
		key = false;
		switch(str)
		{
			case "ak":
				content.text = Core.Data.stringManager.getString(25001);
				key = true;
				break;
			case "df":
				content.text = Core.Data.stringManager.getString(25044);
				key = true;
				break;
			case "sf":
				content.text = Core.Data.stringManager.getString(25046);
				key = true;
				break;
			case "eak":
				content.text = Core.Data.stringManager.getString(25000);
				key = false;
				break;
			case "edf":
				content.text = Core.Data.stringManager.getString(25043);
				key = false;
				break;
			case "esf":
				content.text = Core.Data.stringManager.getString(25045);
				key = false;
            break;
        }
    }

    void BackBtn()
    {
		if(FinalTrialMgr.GetInstance()._MissionBackCallBack != null)
		{
			FinalTrialMgr.GetInstance()._MissionBackCallBack();
			FinalTrialMgr.GetInstance()._MissionBackCallBack = null;
			DBUIController.mDBUIInstance.ShowFor2D_UI();
		}
        gameObject.SetActive(false);
    }

//    void Btn1_OnClick()
//    {
//        CreateTrialPanel(0);
//    }
//
//    void Btn2_OnClick()
//    {
//        CreateTrialPanel(1);
//    }
//
//    void Btn3_OnClick()
//    {
//        CreateTrialPanel(2);
//    }
//
//    void Btn4_OnClick()
//    {
//        CreateTrialPanel(3);
//    }

//    void Btn5_OnClick()
//    {
//        CreateTrialPanel(FinalTrialAdditionType.Addition_EnemySkill);
//    }
//
//    void Btn6_OnClick()
//    {
//        CreateTrialPanel(FinalTrialAdditionType.Addition_SelfSkill);
//    }
}
