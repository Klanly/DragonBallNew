using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonHeChengUI : MonoBehaviour 
{
	public UIButton m_btnTab1;
	public UIButton m_btnTab2;
	public UIButton m_btnTab3;

	private UILabel m_txtTab1;
	private UILabel m_txtTab2;
	private UILabel m_txtTab3;

	public UIButton[] m_btnSub;
	public MonsterHeadCell[] m_subMons;
	public UIButton m_btnHeCheng;

	public UILabel m_txtTip;
	public UILabel m_txtTitle;
	public UILabel m_txtDesp;


	public Card3DUI m_3dCard;

	[HideInInspector]
	public int m_nTempIndex = -1;

	private int m_nType = 0;
	private  Monster m_mainData;
	[HideInInspector]
	public Monster[] m_subData = new Monster[4];

	private int[] TEXT_DESP = new int[] {5059,5060,5061};

    private const string BTN_SEL   = "Symbol 31"; // 选中
    private const string BTN_UNSEL = "Symbol 32";


    private Color COLOR_SEL = Color.white; //new Color(1f,215f/255f,0);
	private Color COLOR_UNSEL = Color.white;
    // true 主卡  随便选  ,false  副卡  不能选在阵容的卡
    public static bool isInCurTeam = false;

    public UISprite ProSp_1 ;//  主卡属性
    public UISprite ProSp_2 ; //  合成以后主卡属性

    public UISprite star_1 ;// 
    public UISprite star_2 ;//


	public Monster MainMon
	{
		get 
		{
			return m_mainData;
		}
	}

	public Monster[] SubData
	{
		get
		{
			return m_subData;
		}
	}

	void Start()
	{
		InitUI ();
     
	}
    //初始化星星
    public void initXX()
    {
        if(star_1 ==null || star_1.gameObject == null ) return  ; 

        StarMove sm =  star_1.gameObject.GetComponent<StarMove>();
        sm.setBtnXing();

        if(star_2 ==null || star_2.gameObject == null ) return  ; 


        StarMove sm1=  star_2.gameObject.GetComponent<StarMove>();
        sm1.setBtnXing();
    }
    //清除星星
    public void ClearXX()
    {
        if(star_1 ==null || star_1.gameObject == null ) return  ; 

        StarMove sm =  star_1.gameObject.GetComponent<StarMove>();
        sm.ClearS();

        if(star_2 ==null || star_2.gameObject == null ) return  ; 


        StarMove sm1=  star_2.gameObject.GetComponent<StarMove>();
        sm1.ClearS();

    }



	public void SetShow(bool bShow)
	{
		RED.SetActive (bShow, this.gameObject);
		if(bShow)
		{
			InitUI();
		}
	}

	public void RepositionCard()
	{
		if(!m_3dCard.IsPosValid())
		{
			if(m_mainData != null)
			{
				m_3dCard.Show3DCard(m_mainData);
			}
		}
	}
    IEnumerator scaleProSp(UISprite ProSp)
    {
        ProSp_2.gameObject.SetActive(false);
        ProSp.gameObject.SetActive(true);
        ProSp.spriteName = "Attribute_"+ proType.ToString();
		ProSp.MakePixelPerfect ();
        ProSp.gameObject.transform.localScale  = new Vector3(5,5,5) ;
        yield return  null;
		MiniItween.ScaleTo(ProSp.gameObject, new Vector3(1f, 1f, 1f), 0.2f, MiniItween.EasingType.EaseOutCubic).myDelegateFunc += ()=>{
			ProSp.MakePixelPerfect ();
		};
    }

    IEnumerator scaleProSp1(UISprite ProSp)
    {
        ProSp.gameObject.SetActive(true);
        ProSp.spriteName = "Attribute_"+ proType.ToString();
		ProSp.MakePixelPerfect ();
        ProSp.gameObject.transform.localScale  = new Vector3(5,5,5) ;
		MiniItween.ScaleTo(ProSp.gameObject, new Vector3(1f, 1f, 1f), 0.2f, MiniItween.EasingType.EaseOutCubic).myDelegateFunc +=() =>{
			ProSp.MakePixelPerfect ();
		};
        yield return new WaitForSeconds(0.2f) ;
        ProSp.gameObject.SetActive(false);
        ProSp_1.spriteName = "Attribute_"+ proType.ToString();
		ProSp_1.MakePixelPerfect ();
    }
    int proType =0;
	public void SetData(Monster mon)
	{
		if (m_nTempIndex == -1)
		{
            proType = (int )mon.RTData.Attribute;
           
            StartCoroutine ("scaleProSp",ProSp_1);

			m_mainData = mon;
			m_3dCard.Show3DCard(mon);
			RED.SetActive (false, m_txtTip.gameObject);
			for(int i = 0; i < GetNeedSubCount (); i++)
			{
				m_subData[i] = null;
				RED.SetActive (false,  m_subMons[i].gameObject);
				RED.SetActive (true, m_btnSub[i].gameObject);
			}

			AutoSelSub ();
		}
		else
		{
			m_subData [m_nTempIndex] = mon;
			RED.SetActive (true,  m_subMons[m_nTempIndex].gameObject);
			m_subMons[m_nTempIndex].InitUI (mon);
			if(!m_3dCard.IsPosValid())
			{
				m_3dCard.Show3DCard(m_mainData);
			}
		}
	}


	void AutoSelSub()
	{
		List<Monster> totalList = new List<Monster>();
		List<Monster> list = new List<Monster>();
		int count = 0;
		switch(m_nType)
		{
			case 0:
				for (short i = 6; i >= 3; i--)
				{
					list.Clear ();
					list = Core.Data.monManager.GetZhenRenHeChSubMon (i);
					totalList.AddRange (list.ToArray ());
				}
				count = 3;
				break;
			case 1:
				for (short i = 6; i >= 3; i--)
				{
					list.Clear ();
					list = Core.Data.monManager.GetZhenRenHeShenRenSub (i);
					totalList.AddRange (list.ToArray ());
				}
				count = 2;
				break;
			case 2:
				count = 5;
				for(int i = 0; i < count; i++)
				{
					for (short j = 6; j >= 3; j--)
					{
						list.Clear ();
						list = Core.Data.monManager.GetShenRenHeChSubMon (j);
						if(list != null && list.Count > 0)
						{
							totalList.Add(list[0]);
							m_subData [i] = list[0];
							RED.SetActive (true,  m_subMons[i].gameObject);
							m_subMons[i].InitUI (list[0]);
						}
					}
				}
				break;
		}

		if(m_nType == 0 || m_nType == 1)
		{
			for (int i = 0; i < totalList.Count; i++)
			{
				if (totalList [i] != null && i < count - 1)
				{
					m_subData [i] = totalList [i];
					RED.SetActive (true,  m_subMons[i].gameObject);
					m_subMons[i].InitUI (totalList [i]);
				}
			}
		}

		if (totalList.Count < count - 1)
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(5255));
		}
	}
		
	public void InitUI()
	{
		for (int i = 0; i < m_btnSub.Length; i++)
		{
			m_btnSub [i].TextID = 5056;
		}
		m_btnHeCheng.TextID = 5055;
        m_txtTip.gameObject.SetActive(true);
		m_txtTip.text = Core.Data.stringManager.getString(5057);

		m_btnTab1.TextID = 5125;
		m_btnTab2.TextID = 5053;
		m_btnTab3.TextID = 5054;

		m_3dCard.Del3DModel ();
		m_3dCard.InitUI();
		UpdateUI ();
	}

	void UpdateUI()
	{
		m_txtTitle.text = Core.Data.stringManager.getString (5049);
		m_txtDesp.text = Core.Data.stringManager.getString (TEXT_DESP [m_nType]);

		int cnt = GetNeedSubCount ();
		for (int i = 0; i < m_btnSub.Length; i++)
		{
			RED.SetActive (i < cnt, m_btnSub [i].gameObject);
		}
		for (int i = 0; i < m_subMons.Length; i++)
		{
			RED.SetActive (false, m_subMons [i].gameObject);
			m_subData [i] = null;
		}

		m_mainData = null;

		if(m_txtTab1 == null)
		{
			m_txtTab1 = m_btnTab1.transform.GetComponentInChildren<UILabel>();
		}
		if(m_txtTab2 == null)
		{
			m_txtTab2 = m_btnTab2.transform.GetComponentInChildren<UILabel>();
		}
		if(m_txtTab3 == null)
		{
			m_txtTab3 = m_btnTab3.transform.GetComponentInChildren<UILabel>();
		}

		if (m_nType == 0)
		{
			RED.SetBtnSprite (m_btnTab1, BTN_SEL);
            RED.SetBtnSprite (m_btnTab2, BTN_UNSEL);
			RED.SetBtnSprite (m_btnTab3, BTN_UNSEL);

			m_txtTab1.color = COLOR_SEL;
			m_txtTab2.color = COLOR_UNSEL;
			m_txtTab3.color = COLOR_UNSEL;
		}
		else if (m_nType == 1)
		{
			RED.SetBtnSprite (m_btnTab2, BTN_SEL);
            RED.SetBtnSprite (m_btnTab1, BTN_UNSEL);
			RED.SetBtnSprite (m_btnTab3, BTN_UNSEL);

			m_txtTab2.color = COLOR_SEL;
			m_txtTab1.color = COLOR_UNSEL;
			m_txtTab3.color = COLOR_UNSEL;
		}
		else if (m_nType == 2)
		{
            RED.SetBtnSprite (m_btnTab3, BTN_SEL);
            RED.SetBtnSprite (m_btnTab1, BTN_UNSEL);
            RED.SetBtnSprite (m_btnTab2, BTN_UNSEL);

			m_txtTab3.color = COLOR_SEL;
			m_txtTab2.color = COLOR_UNSEL;
			m_txtTab1.color = COLOR_UNSEL;
		}
	}

	public void OnClickTab(GameObject obj)
	{

		int type = m_nType;
		if (obj == m_btnTab1.gameObject)
		{
			type = 0;
		}
		else if (obj == m_btnTab2.gameObject)
		{
			type = 1;
		}
		else if (obj == m_btnTab3.gameObject)
		{
			type = 2;
		}
		if (type == m_nType)
		{
			return;
		}

        ProSp_1.gameObject.SetActive(false) ; 
        ProSp_2.gameObject.SetActive(false) ; 


		m_nType = type;

		m_3dCard.Del3DModel ();
		m_3dCard.InitAttrs();

		UpdateUI ();
	}

	void OnClickSub(GameObject obj)
	{
       
        //配卡 不能再当前队伍中
		int index = int.Parse (obj.name);
		if (!RED.IsActive (m_btnSub [index].gameObject) && !RED.IsActive (m_subMons [index].gameObject))
		{
			return;
		}

		if (m_mainData == null)
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(5094));
			return;
		}
			
		m_nTempIndex = index;
		RUIType.EMBoxType type = RUIType.EMBoxType.NONE;
		switch(m_nType)
		{
			case 0:
				type = RUIType.EMBoxType.HECHENG_ZHENREN_SUB;
				break;
			case 1:
				type = RUIType.EMBoxType.ZHENREN_HE_SHENREN_SUB;
				break;
			case 2:
				type = RUIType.EMBoxType.HECHENG_SHENREN_SUB;
				break;
		}

		List<Monster> list = new List<Monster> ();

		switch (type)
		{
			case RUIType.EMBoxType.HECHENG_ZHENREN_SUB:
				for(short i = 6; i > 0; i--)
				{
					List<Monster> temp = Core.Data.monManager.GetZhenRenHeChSubMon (i);	
					if(temp != null && temp.Count > 0)
					{
						list.AddRange(temp.ToArray());
					}

					if (list != null && list.Count > 0)
						break;
				} 
				break;
			case RUIType.EMBoxType.ZHENREN_HE_SHENREN_SUB:
				for(short i = 6; i > 0; i--)
				{
					List<Monster> temp = Core.Data.monManager.GetZhenRenHeShenRenSub (i);

					if(temp != null && temp.Count > 0)
					{
						list.AddRange(temp.ToArray());
					}
					if (list != null && list.Count > 0)
						break;
				}
				break;
			case RUIType.EMBoxType.HECHENG_SHENREN_SUB:
				for(short i = 6; i > 0; i--)
				{
					List<Monster> temp = null;
					if(m_subData[index] == null)
					{
						temp = Core.Data.monManager.GetShenRenHeChSubMon (i);
					}
					else
					{
						temp = Core.Data.monManager.GetHechengMon(i, m_subData[index].num, m_subData[index].RTData.Attribute);
						if(temp != null && temp.Count > 0)
						{
							for(int m = 0; m < temp.Count; m++)
							{
								if(temp[m].pid == m_subData[index].pid)
									temp.Remove(temp[m]);
							}
						}
					}
					
					if(temp != null && temp.Count > 0)
					{
						list.AddRange(temp.ToArray());
					}
					if (list != null && list.Count > 0)
						break;
				}
				break;
		}

		if (list == null || list.Count == 0)
		{
//			UIInformation.GetInstance ().SetInformation (Core.Data.stringManager.getString (5095), Core.Data.stringManager.getString (5030), Core.Data.stringManager.getString (5066), OpenZhaoMuUI);
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5095));
		}
		else
		{
            ClearXX();
			DBUIController.mDBUIInstance.SetViewState (RUIType.EMViewState.S_Bag, type);
			TrainingRoomUI.mInstance.SetShow (false);

		}
	}


//	private void OpenZhaoMuUI()
//	{
//		TrainingRoomUI.mInstance.SetShow (false);
//		UIDragonMallMgr.GetInstance ().OpenUI (ShopItemType.Egg ,ShopCallBack);
//	}

	void ShopCallBack()
	{
		TrainingRoomUI.mInstance.SetShow (true);
	}

	void OnClickMain()
	{
		m_nTempIndex = -1;
		List<Monster> list = new List<Monster>();
		RUIType.EMBoxType type = RUIType.EMBoxType.NONE;
		switch(m_nType)
		{
			case 0:
				type = RUIType.EMBoxType.HECHENG_ZHENREN_MAIN;
				break;
			case 1:
				type = RUIType.EMBoxType.ZHENREN_HE_SHENREN_MAIN;
				break;
			case 2:
				type = RUIType.EMBoxType.HECHENG_SHENREN_MAIN;
				break;
		}

		int stage = RuntimeMonster.NORMAL_MONSTER;
		if (m_nType == 1)
		{
			stage = RuntimeMonster.ZHEN_MONSTER;
		}
			
		for (int i = 1; i <= 5; i++)
		{
			List<Monster> stageList = Core.Data.monManager.GetMonListByStage (i, stage);
			if (stageList != null && stageList.Count > 0)
			{
				list.AddRange (stageList.ToArray ());
			}
		}

		if (list.Count == 0)
		{
//			UIInformation.GetInstance ().SetInformation (Core.Data.stringManager.getString (5095), Core.Data.stringManager.getString (5030), Core.Data.stringManager.getString (5066), OpenZhaoMuUI);
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5095));
			return;
		}
        ClearXX();
		DBUIController.mDBUIInstance.SetViewState (RUIType.EMViewState.S_Bag, type);
		TrainingRoomUI.mInstance.SetShow (false);
	}

	void OnClickHeCheng()
	{
		if (m_mainData == null)
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5058));
			return;
		}

		int count = GetNeedSubCount ();
		for (int i = 0; i < count; i++)
		{
			if (m_subData [i] == null)
			{
				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(5255));
				return;
			}
		}

		SendHeChengMsg();
	}

	void OnClickBack()
	{
		m_mainData = null;
		for (int i = 0; i < m_subData.Length; i++)
		{
			m_subData [i] = null;
		}
		ProSp_1.gameObject.SetActive (false); 
		ProSp_2.gameObject.SetActive (false); 

		InitUI ();
		m_3dCard.Del3DModel ();
		SetShow (false);
		star_1.gameObject.GetComponent<StarMove> ().ClearS ();
		star_2.gameObject.GetComponent<StarMove> ().ClearS ();

		RED.SetActive (true, TrainingRoomUI.mInstance.m_mainTraining);

		if (TrainingRoomUI.mInstance.m_callBack != null)
		{
			TrainingRoomUI.mInstance.OnClickExit ();
		}
	}
		
	void SendHeChengMsg()
	{
		HeChengParam param = new HeChengParam ();
		param.gid = Core.Data.playerManager.PlayerID;
		param.mr = m_mainData.pid;
		List<int> list = new List<int> ();
		for(int i = 0 ; i < m_subData.Length; i++)
		{
			if (m_subData [i] != null)
			{
				list.Add (m_subData [i].pid);
			}
		}
		param.el = list.ToArray ();
		param.ty = m_nType + 1;
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.HECHENG, param);

		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;

		//then you should dispatch to a real handler
		task.DispatchToRealHandler ();
	}

	int GetNeedSubCount()
	{
		int cnt = 0;
		switch (m_nType)
		{
			case 0:
				cnt = 2;
				break;
			case 1:
				cnt = 1;
				break;
			case 2:
				cnt = 4;
				break;
		}

		return cnt;
	}
	

	#region 网络返回

	void testHttpResp_UI (BaseHttpRequest request, BaseResponse response)
	{
		if (response.status != BaseResponse.ERROR) 
		{
           

			HttpRequest rq = request as HttpRequest;
			if (rq.Type == RequestType.HECHENG)
			{
				UpdateUI ();
				HeChengResponse resp = response as HeChengResponse;
			
				Monster mon = Core.Data.monManager.getMonsterById (resp.data.ppid);

                proType =  (int )mon.RTData.Attribute; ; // yangchenguang
				if(TeamUI.mInstance != null)
				{
					TeamUI.mInstance.RefreshMonster(mon);
				}
				m_3dCard.Show3DCard (mon);

				SQYMainController.mInstance.UpdateTeamTip ();
                StartCoroutine("scaleProSp1",ProSp_2);
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
	}
	#endregion
}
