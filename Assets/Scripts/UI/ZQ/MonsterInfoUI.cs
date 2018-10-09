using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//created by zhangqiang at 2014-03-11
public class MonsterInfoUI : MonoBehaviour 
{
	private static MonsterInfoUI _mInstance;
	public static MonsterInfoUI mInstance
	{
		get
		{
			return _mInstance;
		}
	}

	public UILabel m_txtDesp;

	public UILabel[] m_txtFate;
	public UILabel[] m_txtFataTitle;
	public GameObject[] m_fateBg;


	private Monster m_data;
	private bool m_bOperate = true;

    public UILabel lab_Detail;
	public Card3DUI m_3dCard;

    public UIButton[]  UIFateBtn;
    List<FateData> fDateList = new List<FateData>();

    public GameObject skillInfo;
	public SkillShowCellScript[] skills;
	public UIButton m_btnSkillUp;

    Vector3  titleStartPos = new Vector3(-314,10,0);
    Vector3  DespStartPos = new Vector3(-344,10,0);
    public UILabel lblDesp;
    public UILabel lblTitle;
    public GameObject titleObj;

    string strColor = "[FFDF00]";

    ShowFatePanelController.FateInPanelType curType;

    public static void OpenUI(Monster monster,ShowFatePanelController.FateInPanelType type , bool bOperate = true)
	{
		if(mInstance == null)
		{
			Object prefab = PrefabLoader.loadFromPack("ZQ/MonsterInfoUI");
			if(prefab != null)
			{
				GameObject obj = Instantiate(prefab) as GameObject;
				if(obj != null)
				{
					_mInstance = obj.GetComponent<MonsterInfoUI>();
				
					_mInstance.transform.parent = DBUIController.mDBUIInstance._TopRoot.transform;
					_mInstance.transform.localPosition = Vector3.zero;
					_mInstance.transform.localEulerAngles = Vector3.zero;
					_mInstance.transform.localScale = Vector3.one;
				}
			}
		}
		else
		{
			mInstance.ShowUI(true);
		}

		mInstance.m_bOperate = bOperate;
		mInstance.m_data = monster;
        //用于展示缘分 的类型   by  wxl
        mInstance.curType = type;
		mInstance.InitUI();

	}

	public void Destroy()
	{
		if(mInstance != null)
		{
			Destroy(this.gameObject);
			_mInstance = null;
            UIDownModel.CloesWin();
		}
	}


	public void ShowUI(bool bShow)
	{
		RED.SetActive(bShow, this.gameObject);
	}


	public void RefreshUI(Monster mon)
	{
		m_data = mon;
		InitUI();
		ShowUI(true);
	}

    void DownLoadFinish(AssetTask aTask)
    {
        Debug.Log("LoadFinished");
        m_3dCard.Del3DModel ();
        m_3dCard.Show3DCard (m_data, false);
    }

	private void InitUI()
	{
        #if SPLIT_MODEL
        if (!Core.Data.sourceManager.IsModelExist (m_data.num))
        {
			UIDownModel.OpenDownLoadUI(m_data.num, DownLoadFinish, new Vector3(-368,-100,-30));
            UIDownModel.mInstance.gameObject.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
        }
        else
        {
            UIDownModel.CloesWin();
        }
        #endif
		if(m_data != null)
		{
            fDateList.Clear ();
			m_txtDesp.text = m_data.config.description;
          
			//init skills
			List<Skill> skillList = m_data.getSkill;
			int index = 0;
			for(; index < skillList.Count; index++)
			{
				skills[index].Show(skillList[index], index / 2);
			}
				
			for(;index < 3; index++)
			{
				skills[index].Show(null, index / 2);
			}

            //阵容界面 隐藏 
            if (curType == ShowFatePanelController.FateInPanelType.isInTeamModifyPanel) {
                for (int i = 0; i < UIFateBtn.Length; i++) {
                    UIFateBtn [i].gameObject.SetActive (false);
                }
            }
			//init fate
			List<FateData> o = m_data.getMyFate(Core.Data.fateManager);

            fDateList = o;
			List<AoYi> aoyiList = Core.Data.dragonManager.usedToList ();
			MonsterTeam myteam =  Core.Data.playerManager.RTData.curTeam;
			int count = o.Count;

            int allSet = 0;

            lblDesp.gameObject.transform.localPosition = DespStartPos + Vector3.down*count* 60;
            titleObj.transform.localPosition = titleStartPos + Vector3.down*count* 60;
            lblDesp.text =string.Format( Core.Data.stringManager.getString (7336),m_data.config.nuqi1.ToString());;
            lblTitle.text = Core.Data.stringManager.getString (7335);

            for (int i = 0; i < count; i++) 
            { 
              
				m_txtFataTitle[i].text = o[i].name;
                string tColor = "";
				if (m_data.checkMyFate (o[i], myteam, aoyiList))
				{
					m_txtFataTitle[i].color = new Color (1f,227f/255,43f/255);
                    //	m_txtFate[i].color = new Color (1f,227f/255,43f/255);
                    tColor = strColor;
                    allSet++;

                } 
				else 
				{
					m_txtFataTitle[i].color = Color.white;
					m_txtFate[i].color = Color.white;
				}
                string  addAng =  string.Format( Core.Data.stringManager.getString(1008),o[i].nuqi.ToString());
                m_txtFate[i].text = tColor +  o[i].description + addAng ;
			}
            if (count != 0) {
                if (allSet == count) {
                    // lblDesp.color = new Color (1f, 227f / 255f, 43f / 255f, 1f);
					lblDesp.text = strColor + lblDesp.text;
                    lblTitle.color = new Color (1f, 227f / 255f, 43f / 255f, 1f);
                } else {
                    lblTitle.color = Color.white;
                    lblDesp.color = Color.white;
                }
            } else {
                lblDesp .gameObject.SetActive(false);
                titleObj.gameObject.SetActive (false);

            }

			for(int i = count; i < m_txtFate.Length; i++)
			{
				RED.SetActive(false, m_txtFate[i].gameObject, m_fateBg[i]);
                // ad by wxl
                RED.SetActive (false,UIFateBtn[i].gameObject);
			}

			m_3dCard.Show3DCard (m_data, false);
			m_btnSkillUp.TextID = 5233;
			m_btnSkillUp.isEnabled = !Core.Data.monManager.IsExpMon (m_data.config.ID) && m_bOperate;
		}
	}


    public void OnClickClose()
	{
		if (m_bOperate)
		{
            if (DBUIController.mDBUIInstance._petBoxCtl !=null) {
                if (DBUIController.mDBUIInstance._petBoxCtl.boxType != RUIType.EMBoxType.LOOK_Charator) {
                    DBUIController.mDBUIInstance.SetViewState (RUIType.EMViewState.S_Bag, RUIType.EMBoxType.LOOK_Charator);
                }
            }
		}

		m_3dCard.Del3DModel ();
		this.Destroy();
	}

	void OnClickSkillUp()
	{
		SkillUpUI.CreateSkillUpUI (m_data);
		OnClickClose ();
	}

	private void HttpResp_Suc (BaseHttpRequest request, BaseResponse response)
	{
		if (response.status != BaseResponse.ERROR) 
		{
			HttpRequest rq = request as HttpRequest;
			switch(rq.Type)
			{
			case RequestType.SELL_MONSTER:
				OnClickClose();
				DBUIController.mDBUIInstance.SetViewState(RUIType.EMViewState.S_Bag,RUIType.EMBoxType.LOOK_Charator);
				break;

			case RequestType.STRENGTHEN_MONSTER:
				m_data = Core.Data.monManager.getMonsterById (m_data.pid);
				InitUI();
				break;
			}
		} 
		else 
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getNetworkErrorString(response.errorCode));
		}
	}

	void HttpResp_Error (BaseHttpRequest request, string error)
	{
	}

    public void ShowFatePanel(FateData fData){
		ShowFatePanelController.CreatShowFatePanel (fData.ID,curType,null);
    }

    public void OnClickOne(){
        if(fDateList.Count>0)
        ShowFatePanel (fDateList[0]);
    }
    public void OnClickTwo(){
        if(fDateList.Count>1)
        ShowFatePanel (fDateList[1]);
    }
    public void OnClickThree(){
        if(fDateList.Count>2)
        ShowFatePanel (fDateList[2]);
    }
    public void OnClickFour(){
        if(fDateList.Count>3)
        ShowFatePanel (fDateList[3]);
    }
}
