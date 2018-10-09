using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UISevenDayRewardCell : RUIMonoBehaviour 
{

	public UISprite mIconUp;
	public UISprite mIconDown;

	public UILabel mNameUp;
	public UILabel  mNameDown;

	public UISprite mBgUp;
	public UISprite mBgDown;

	public UISprite mCircleUp;
	public UISprite mCircleDown;

	public UISprite mBg2Up;
	public UISprite mBg2Down;

	public UILabel mNumUp;
	public UILabel mNumDown;

	public UISprite mTitleIcon;

	public UILabel mTitle;

	public UISprite mReceiveIcon;

	public UISprite[] mStar1;
	public UISprite[] mStar2;
	public UISprite[] mStar3;
	public UISprite[] mStar4;




	public UISprite mIconUp1;
	public UISprite mIconDown1;
	
	public UILabel mNameUp1;
	public UILabel  mNameDown1;
	
	public UISprite mBgUp1;
	public UISprite mBgDown1;
	
	public UISprite mCircleUp1;
	public UISprite mCircleDown1;
	
	public UISprite mBg2Up1;
	public UISprite mBg2Down1;
	
	public UILabel mNumUp1;
	public UILabel mNumDown1;

    public UISprite mNumUpBg;
    public UISprite mNumDownBg;
    public UISprite mNumUp1Bg;
    public UISprite mNumDown1Bg;


	Vector3 m1 = new Vector3(0,-23,0);
	float mm = 11.5f;

	Color grayC = new Color (0,0,0,1f);
	
    SevenDayCellType _type;

	public void Show(SevenDayCellType type, SevenDaysAwardList[] awads, int day)
	{
		List<int[]> _AwardData = AwardData(day,awads);

		mTitle.text = Core.Data.stringManager.getString(20054).Replace("#",day.ToString());


		if(_AwardData.Count != 4)
		{
			ConsoleEx.DebugLog("Serve's back is wrong" + "------- index = " + day.ToString());
			return;
		}

		SetItemDetail(_AwardData);

		_type = type;
		switch(type)
		{
			case SevenDayCellType.SevenDayCellType_NOTOPEN:
				ShowNotOpen();
				break;
			case SevenDayCellType.SevenDayCellType_HAVETAKE:
				ShowHaveTake();
				break;
			case SevenDayCellType.SevenDayCellType_WAITTAKE:
				ShowWaitTake();
				break;
		}
	}

	void SetStar(UISprite[] star, int starcount)
	{
		int mCount = 0;
		while(mCount < starcount)
		{
			if(star[mCount] != null)star[mCount].gameObject.SetActive(true);
			star[mCount].transform.localPosition = new Vector3(m1.x+(mCount+1)*2*mm,m1.y,m1.z);
			mCount++;
		}
		for(int j=0; j<mCount; j++)
		{
			star[j].transform.localPosition = new Vector3(star[j].transform.localPosition.x-(mCount+1)*mm,m1.y,m1.z); ;
		}
		for(;mCount<5; mCount++)
		{
			if(star[mCount] != null)star[mCount].gameObject.SetActive(false);
		}
	}
    
    List<int[]> AwardData(int day, SevenDaysAwardList[] awads)
    {
        for(int i=0; i<awads.Length; i++)
		{
			if(awads[i].day == day)return awads[i].reward;
		}
		return new List<int[]>();
	}

	void ShowNotOpen()
	{
		mIconUp.color = Color.gray;
		mIconDown.color = Color.gray;
		mBgUp.color = grayC;
		mBgDown.color = grayC;
//		mBgUp.spriteName = "common-0004_gray";
//		mBgDown.spriteName = "common-0004_gray";
		mCircleUp.spriteName = "star6";
		mCircleDown.spriteName = "star6";
		mBg2Up.color = grayC;
		mBg2Down.color = grayC;
//		mBg2Up.spriteName = "common-0022_gray";
//		mBg2Down.spriteName = "common-0022_gray";

		mIconUp1.color = Color.gray;
		mIconDown1.color = Color.gray;
//		mBgUp1.spriteName = "common-0004_gray";
//		mBgDown1.spriteName = "common-0004_gray";
		mBgUp1.color = grayC;
		mBgDown1.color = grayC;
		mCircleUp1.spriteName = "star6";
		mCircleDown1.spriteName = "star6";
//		mBg2Up1.spriteName = "common-0022_gray";
//		mBg2Down1.spriteName = "common-0022_gray";
		mBg2Up1.color = grayC;
		mBg2Down1.color = grayC;

		mTitleIcon.spriteName = "common-0015";
		mTitle.color = Color.gray;
		mReceiveIcon.gameObject.SetActive(false);
	}

	void ShowHaveTake()
	{
		mIconUp.color = Color.white;
		mIconDown.color = Color.white;
//		mBgUp.spriteName = "common-0004";
//		mBgDown.spriteName = "common-0004";
		mBgUp.color =  Color.white;
		mBgDown.color =  Color.white;
		mCircleUp.spriteName = "star3";
		mCircleDown.spriteName = "star3";
		mBg2Up.color = Color.white;//.spriteName = "common-0022";
		mBg2Down.color = Color.white;//spriteName = "common-0022";

		mIconUp1.color = Color.white;
		mIconDown1.color = Color.white;
		mBgUp1.color = Color.white;//spriteName = "common-0004";
		mBgDown1.color = Color.white;//spriteName = "common-0004";
		mCircleUp1.spriteName = "star3";
		mCircleDown1.spriteName = "star3";
		mBg2Up1.color = Color.white;//spriteName = "common-0022";
		mBg2Down1.color = Color.white;//spriteName = "common-0022";

		mTitleIcon.spriteName = "common-0018";
		mTitle.color = Color.white;
		mReceiveIcon.gameObject.SetActive(true);
		mReceiveIcon.spriteName = "deng-002";
	}

	void ShowWaitTake()
	{
		mIconUp.color = Color.white;
		mIconDown.color = Color.white;
		mBgUp.color = Color.white;//.spriteName = "common-0004";
		mBgDown.color = Color.white;//.spriteName = "common-0004";
		mCircleUp.spriteName = "star3";
		mCircleDown.spriteName = "star3";
		mBg2Up.color = Color.white;//.spriteName = "common-0022";
		mBg2Down.color = Color.white;//.spriteName = "common-0022";

		mIconUp1.color = Color.white;
		mIconDown1.color = Color.white;
		mBgUp1.color = Color.white;//.spriteName = "common-0004";
		mBgDown1.color = Color.white;//.spriteName = "common-0004";
		mCircleUp1.spriteName = "star3";
		mCircleDown1.spriteName = "star3";
		mBg2Up1.color = Color.white;//.spriteName = "common-0022";
		mBg2Down1.color = Color.white;//.spriteName = "common-0022";

		mTitleIcon.spriteName = "common-0019";
		mTitle.color = Color.white;
		mReceiveIcon.gameObject.SetActive(true);
		mReceiveIcon.spriteName = "deng-001";
	}

	void SetItemDetail(List<int[]> _award)
	{
		if(_award.Count == 0)return;
		if(_award[0].Length < 2)return;


		SetIconDetail(mIconUp, _award[0][0], mNameUp, mStar1);
        mNumUp.SafeText( ItemNumLogic.setItemNum( _award[0][1],mNumUp , mNumUpBg) );

		if(_award.Count >= 2)
		{
			if(_award[1].Length < 2)return;
			SetIconDetail(mIconDown, _award[1][0], mNameDown, mStar2);
            mNumDown.SafeText( ItemNumLogic.setItemNum( _award[1][1],mNumDown , mNumDownBg) );

			SetIconDetail(mIconUp1, _award[2][0], mNameUp1, mStar3);
            mNumUp1.SafeText(ItemNumLogic.setItemNum( _award[2][1],mNumUp1 , mNumUp1Bg));

			SetIconDetail(mIconDown1, _award[3][0], mNameDown1, mStar4);
            mNumDown1.SafeText(ItemNumLogic.setItemNum( _award[3][1],mNumDown1 , mNumDown1Bg) );
		}
	}

	void SetIconDetail(UISprite _sprite, int id, UILabel _label, UISprite[] star)
	{
		int starcount = 0;
		if (DataCore.getDataType(id) == ConfigDataType.Monster)
		{

			AtlasMgr.mInstance.SetHeadSprite (_sprite, id.ToString ());
			MonsterData data = Core.Data.monManager.getMonsterByNum(id);
			_label.text = data.name;

			starcount = Core.Data.monManager.getMonsterByNum(id).star;
			SetStar(star, starcount);
			return;
		}
		else if (DataCore.getDataType(id) == ConfigDataType.Equip)
		{
			EquipData data = Core.Data.EquipManager.getEquipConfig(id);
			_sprite.atlas = AtlasMgr.mInstance.equipAtlas;
			_label.text = data.name;
			starcount = data.star;
			_sprite.spriteName = id.ToString();
        }
		else if (DataCore.getDataType(id) == ConfigDataType.Gems)
		{
			GemData data = Core.Data.gemsManager.getGemData(id);
			_sprite.atlas = AtlasMgr.mInstance.commonAtlas;
			_label.text = data.name;
			starcount = data.star;
            _sprite.spriteName = data.anime2D.ToString();
            _sprite.MakePixelPerfect();
            SetStar(star, starcount);
            return;
        }
		else if (DataCore.getDataType(id) == ConfigDataType.Item)
		{
			ItemData data = Core.Data.itemManager.getItemData(id);
			_sprite.atlas = AtlasMgr.mInstance.itemAtlas;
			_sprite.spriteName = data.iconID.ToString ();
			_label.text = data.name;
			starcount = data.star;
        }
		else if(DataCore.getDataType(id) == ConfigDataType.Frag)
        {
			SoulData data = Core.Data.soulManager.GetSoulConfigByNum(id);
            AtlasMgr.mInstance.SetHeadSprite (_sprite, data.updateId.ToString());
			_label.text = data.name;
			starcount = data.star;
            _sprite.spriteName = data.updateId.ToString();
            SetStar(star, starcount);
            return;
        }
        else
        {
            RED.LogWarning("unknow reward type");
        }
 
		SetStar(star, starcount);
	
    }

	public void SendMsg()
	{
		ReceiveSevenRewardParam param = new ReceiveSevenRewardParam(int.Parse(Core.Data.playerManager.PlayerID));
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);

		task.AppendCommonParam(RequestType.SEVENDAYREWARD_BUY, param);
		
		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;
		
		task.DispatchToRealHandler ();

		ComLoading.Open();
	}

	void testHttpResp_UI (BaseHttpRequest request, BaseResponse response)
	{
		if(response != null)
		{
            if (response.status != BaseResponse.ERROR)
            {
                SevenDaysBuyResponse mbuyres = response as SevenDaysBuyResponse;

                SevenDaysListData sData = Core.Data.ActivityManager.GetSevenData ();
                if (sData != null)
                {
                    sData.canGain = false;
                    if (sData.index < 7)
                        sData.index++;
                }
                Core.Data.ActivityManager.SaveSevenDayData(sData);
                GetRewardSucUI.OpenUI(mbuyres.data.p, Core.Data.stringManager.getString(5047));
                _type = SevenDayCellType.SevenDayCellType_HAVETAKE;
                ShowHaveTake();

				if (sData.index >= 7) {
					Core.Data.ActivityManager.SetDailyGiftState (ActivityManager.sevenDayType, "3");
					GetGiftPanelController.Instance.SevenDayRewardRefresh ();
				} else {
					Core.Data.ActivityManager.SetDailyGiftState (ActivityManager.sevenDayType, "2");
				}
				GetGiftPanelController.Instance.CheckNewPos ();

            }
            else
            {
                SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
            }
		}
        ComLoading.Close();
	}

	void testHttpResp_Error (BaseHttpRequest request, string error)
	{
		ConsoleEx.DebugLog ("---- Http Resp - Error has ocurred." + error);
	}



	void OnClick()
	{

        // yangchenguang  修改七日领取点击提示 7月16日
        if(_type  == SevenDayCellType.SevenDayCellType_NOTOPEN) //奖励还没有开放
		{
           
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(20087));
			return;
		}
        else if (_type ==SevenDayCellType.SevenDayCellType_HAVETAKE) //奖励已经领取过了
        {
            return ; 

        }


		SendMsg();
	}

	void OnDestroy()
	{
		mIconUp = null;
		mIconDown = null;
		mNameUp = null;
		mNameDown = null;
		mBgUp = null;
		mBgDown = null;
		mCircleUp = null;
		mCircleDown = null;
		mBg2Up = null;
		mBg2Down = null;
		mTitleIcon = null;
		mTitle = null;

		mIconUp1 = null;
		mIconDown1 = null;
		mNameUp1 = null;
		mNameDown1 = null;
		mBgUp1 = null;
		mBgDown1 = null;
		mCircleUp1 = null;
		mCircleDown1 = null;
		mBg2Up1 = null;
		mBg2Down1 = null;

	}
}
