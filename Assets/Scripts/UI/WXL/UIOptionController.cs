using UnityEngine;
using System.Collections;

public class UIOptionController : RUIMonoBehaviour {

   
	private static UIOptionController instance;
	public static UIOptionController Instance
	{
        get {
			return instance;
		}
	}

    public static UIOptionController CreatOptionCtrl( GameObject tObj){
        UnityEngine.Object obj = WXLLoadPrefab.GetPrefab (WXLPrefabsName.UIOptionPanel);
        if (obj != null) {
            GameObject go = Instantiate (obj) as GameObject;
            UIOptionController fc = go.GetComponent<UIOptionController> ();
            Transform goTrans = go.transform;
            go.transform.parent = tObj.transform;
            go.transform.localPosition = Vector3.zero;
            goTrans.localScale = Vector3.one;
            //RED.TweenShowDialog(go);
            return fc;
        }
        return null;
    } 

    public GameObject tipName;
    public UILabel lbl_Lv;
    public UILabel lbl_Exp;
    public UILabel lbl_totalCombo;
    public UILabel lbl_Energe;
    public UILabel lbl_ID;
	public UILabel lal_Name;
	public UIInput m_inputName;
    public UISprite sp_Sound;
    public UILabel lbl_Sound;
    public UIToggle btnToggle;

	public GameObject VipNone;
	public GameObject VipReal;
//	public UISprite mVipimperial;
	public UISprite mVipicon;
	public UILabel mViplv;
    public GameObject checkBox;

	public GameObject btnCartoon;
	//改名字花费的钻石个数
	private const int COST_STONE = 5;


	//added by zhangqiang at 2014-4-23  
	//选择头像UI
	[HideInInspector]
	public SelUserHeadUI m_selHeadUI;
	public UISprite m_spHead;	//头像
	private int m_curHead;		//当前选中的头像

    public void Awake()
	{
        instance = this;
    }

	void Start()
	{
        m_inputName.characterLimit = 20;
		m_curHead = Core.Data.playerManager.RTData.headID;
        tipName.SetActive(false);
		AtlasMgr.mInstance.SetHeadSprite(m_spHead, Core.Data.playerManager.RTData.headID.ToString ());

		InitUI ();
	} 
		
	void InitUI()
	{
		PlayerManager player = Core.Data.playerManager;

		lal_Name.text = player.NickName;
		lbl_Lv.text = player.Lv.ToString ();
        
		lbl_ID.text = player.PlayerID;
		lbl_Exp.text = player.curExp.ToString()+"/"+ player.nextLvExp.ToString();

		lbl_totalCombo.text = player.RTData.TotalCombo.ToString();
		
		if( player.curVipLv <= 0) {
			VipNone.gameObject.SetActive(true);
			VipReal.gameObject.SetActive(false);
		}
		else
		{
			VipNone.gameObject.SetActive(false);
			VipReal.gameObject.SetActive(true);
			
			mViplv.text = player.curVipLv.ToString();
			if(player.curVipLv < 4)
			{
//				mVipimperial.spriteName = "common-2010";
				mVipicon.spriteName = "common-2008";
			}
			else if(player.curVipLv > 3 && player.curVipLv < 8)
			{
//				mVipimperial.spriteName = "common-2011";
				mVipicon.spriteName = "common-2009";
			}
			else if(player.curVipLv > 7 && player.curVipLv < 12 )
			{
//				mVipimperial.spriteName = "common-2012";
				mVipicon.spriteName = "common-2007";
			}else if(player.curVipLv > 11 && player.curVipLv < 16){
				mVipicon.spriteName = "common-2109";
			}
		}

        showSoundInfo();
        showDownloadCartoon();

		OnShowCartoonBtn ();
	}

    /// <summary>
    /// 改名
    /// </summary>
    public void OnChangeNameBtn()
	{
        tipName.SetActive(true);
//        RED.TweenShowDialog(tipName , 1);
    }

	//声音开关
    public void OnAudioSwithBtn() {
        Core.Data.soundManager.SwitchSound();
        showSoundInfo();
    }

    private void showSoundInfo() {
        if(Core.Data.soundManager.SoundMute) {
            sp_Sound.spriteName = "main-1003";
            lbl_Sound.text = Core.Data.stringManager.getString(8);
        } else {
            sp_Sound.spriteName = "main-1002";
            lbl_Sound.text = Core.Data.stringManager.getString(9);
        }
        sp_Sound.MakePixelPerfect();
    }

    //下载漫画
    private void showDownloadCartoon() {
        UserConfigManager usr = Core.Data.usrManager;
        if(usr.UserConfig.cartoon == 1) {
            checkBox.SetActive(true);
        } else {
            checkBox.SetActive(false);
        }
    }

    public void OnQuitBtn()
	{
		Application.Quit ();
    }

    public void OnBackBtn()
	{
		if (Core.Data.playerManager.RTData.headID == m_curHead) 
		{
			Destroy (this.gameObject);
			return;
		}
		SendChangeUserInfoMsg(new ChangeUserInfoParam(Core.Data.playerManager.PlayerID, 1, m_curHead.ToString()));
    }

    private void DownLoadInfo(){
        RED.Log("  ");
    }


    //下载漫画
    void OnClickDownLoadIMG() {

        UserConfigManager user = Core.Data.usrManager;
        if(user.UserConfig.cartoon == 1) {
            user.UserConfig.cartoon = 0;
            checkBox.SetActive(false);
        } else {
            user.UserConfig.cartoon = 1;
            checkBox.SetActive(true);
        }

        user.save();
    }

	
	

	void OnBtnSelHead()
	{
		if (m_selHeadUI == null) 
		{
			  m_selHeadUI = SelUserHeadUI.OpenUI ();
		}
	}


	//取消更换名字
	void OnClickCancelChangeName()
	{
		tipName.SetActive (false);

    }

	//确定更改名字
	void OnClickOKChangeName()
	{

		if (string.IsNullOrEmpty (m_inputName.value)) 
		{
			return;
		}

		if(m_inputName.value == Core.Data.playerManager.NickName)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(25128));
			return;
		}

		if (Core.Data.playerManager.Stone < COST_STONE) 
		{
		     SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(35006));
			return;
        }
        if (m_inputName.label.text.Length > 6)
        {
            SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(5138));
            return;
        }
		bool check = SensitiveFilterManager.getInstance().check(m_inputName.value);
		if(check) {
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(75003));
			return;
		}

        SendChangeUserInfoMsg(new ChangeUserInfoParam(Core.Data.playerManager.PlayerID, 2, m_inputName.value));
      
		tipName.SetActive (false);

	}
	

	public void SetSelHead(int headID)
	{
		m_curHead = headID;
		AtlasMgr.mInstance.SetHeadSprite (m_spHead, m_curHead.ToString ());

	}

	//发送修改个人信息消息
	void SendChangeUserInfoMsg(ChangeUserInfoParam param)
	{
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.CHANGE_USERINFO, param);

		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;

		//then you should dispatch to a real handler
		task.DispatchToRealHandler ();
		ComLoading.Open();
	}
		

	#region 网络返回

	void testHttpResp_UI (BaseHttpRequest request, BaseResponse response)
	{
		ComLoading.Close();
		if (response.status != BaseResponse.ERROR) 
		{
			HttpRequest rq = request as HttpRequest;
			switch(rq.Type)
			{
				case RequestType.CHANGE_USERINFO:
				{
					HttpRequest req = request as HttpRequest;
					ChangeUserInfoParam param = req.ParamMem as ChangeUserInfoParam;
					//add by  wxl talkingdata
					if(param.type == 2 ){
						Core.Data.ActivityManager.OnPurchaseVirtualCurrency(ActivityManager.ChangeInfoType,1,COST_STONE);
					}


					DBUIController.mDBUIInstance.RefreshUserInfo();
					if (param.type == 2) 
					{
						lal_Name.text = param.param;
					} 
					else if (param.type == 1)
					{
						Core.Data.playerManager.RTData.headID = int.Parse(param.param);
						Destroy (this.gameObject);
					}
					break;
				}
			}
		} 
		else 
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getNetworkErrorString(response.errorCode));
		}
	}

	void testHttpResp_Error (BaseHttpRequest request, string error)
	{

		ConsoleEx.DebugLog ("---- Http Resp - Error has ocurred." + error);
		HttpRequest rq = request as HttpRequest;
		switch(rq.Type)
		{
		case RequestType.CHANGE_USERINFO:
			{
				HttpRequest req = request as HttpRequest;
				ChangeUserInfoParam param = req.ParamMem as ChangeUserInfoParam;

				if (param.type == 1)
				{
					Destroy (this.gameObject);
				}
				break;
			}
		}
	}

    void OnBtnGonggao(){
        AnnounceMrg.GetInstance().SetInfoPanel(true);
    }

	void OnQuitGame()
	{
		#if Spade
		SpadeIOSLogin spadeSDK = Native.mInstace.m_thridParty as SpadeIOSLogin;
		spadeSDK.tryLogout();
		#else
		Application.Quit();
		#endif
	}

	#endregion


	void OnShowCartoonBtn(){
		if (LuaTest.Instance.OpenCartoon == false) {
			btnCartoon.SetActive (false);
		} else {
			btnCartoon.SetActive (true);
		}
	}

}
