using UnityEngine;
using System.Collections;

//训练屋UI显示方式
public enum ENUM_TRAIN_TYPE
{
	None,				//默认主界面
	HeCheng,			//潜力合成
	AttrSwap,			//属性转换
	QianLiXunLian,		//潜力训练
	MonsterEvolve,		//宠物进化
}

public class TrainingRoomUI : MonoBehaviour 
{
	public MonHeChengUI m_hechengUI;
	public AttrSwapUI m_attrSwapUI;
	public QianLiXunLianUI m_qianLiUI;
	public MonEvolveUI m_monEvolveUI;

	//描述
	public DescriptionUI m_despUI;

	public GameObject m_mainTraining;
	public UILabel m_txtTitle;
	public GameObject[] m_types;
	public UILabel[] m_txtNames;

	public delegate void ExitCallback();
	public ExitCallback m_callBack;

	private ENUM_TRAIN_TYPE m_nType;
	private Monster m_monster;

	private static TrainingRoomUI _mInstanece;
	public static TrainingRoomUI mInstance
	{
		get 
		{
			return _mInstanece;
		}
	}

	public static void OpenUI(ENUM_TRAIN_TYPE type = ENUM_TRAIN_TYPE.None, Monster mon = null, ExitCallback callback = null)
	{
		if (_mInstanece == null)
		{
			Object prefab = PrefabLoader.loadFromPack ("ZQ/TrainingRoomUI");
			if (prefab != null)
			{
				GameObject obj = Instantiate (prefab) as GameObject;
				RED.AddChild (obj, DBUIController.mDBUIInstance._bottomRoot);
				_mInstanece = obj.GetComponent<TrainingRoomUI> ();
				_mInstanece.m_nType = type;
				_mInstanece.m_monster = mon;

			}
		}
		else
		{
			_mInstanece.InitUI (type, mon);
			_mInstanece.SetShow (true);
		}

		_mInstanece.m_callBack = callback;
       
	}

	public static bool IsTrainingRoomUnLock()
	{
		if(Core.Data.playerManager.RTData.curVipLevel < 10)
		{
			BaseBuildingData bd = Core.Data.BuildingManager.GetConfigByBuildLv (BaseBuildingData.BUILD_XUNLIAN, 1);
			if (Core.Data.playerManager.RTData.curLevel < bd.limitLevel)
			{
				string strText = Core.Data.stringManager.getString (6054);
				strText = strText.Replace ("#", bd.limitLevel.ToString());
				SQYAlertViewMove.CreateAlertViewMove (strText);
				return false;
			}
			return true;
		}
		return true;
	}

	public void DestroyUI()
	{
		Destroy (gameObject);
		_mInstanece = null;
	}

	public void OnClickExit()
	{
		if (m_callBack != null)
		{
			m_callBack ();
		}
		else
		{
			DBUIController.mDBUIInstance.ShowFor2D_UI ();
		}
		DestroyUI ();
	}

	void Start()
	{
		BaseBuildingData build = Core.Data.BuildingManager.GetConfigByBuildLv (BaseBuildingData.BUILD_XUNLIAN, 1);
		m_txtTitle.text = build.name;
		for (int i = 0; i < m_txtNames.Length; i++)
		{
			m_txtNames [i].text = Core.Data.stringManager.getString (5049 + i);
		}
		InitUI (m_nType, m_monster);
		StartCoroutine("InitMonData");
	}

	void InitUI(ENUM_TRAIN_TYPE type, Monster mon)
	{
		RED.SetActive (false, m_despUI.gameObject);
		if (type == ENUM_TRAIN_TYPE.None)
		{
			m_hechengUI.SetShow (false);
			m_attrSwapUI.SetShow (false);
			m_qianLiUI.SetShow (false);
			m_monEvolveUI.SetShow (false);
			RED.SetActive (true, m_mainTraining);
		}
		else if (type == ENUM_TRAIN_TYPE.HeCheng)
		{
			m_hechengUI.SetShow (true);
			m_attrSwapUI.SetShow (false);
			m_qianLiUI.SetShow (false);
			m_monEvolveUI.SetShow (false);
			RED.SetActive (false, m_mainTraining);
            _mInstanece.HCStarMove();

		}
		else if (type == ENUM_TRAIN_TYPE.AttrSwap)
		{
			m_hechengUI.SetShow (false);
			m_attrSwapUI.SetShow (true);
			m_qianLiUI.SetShow (false);
			m_monEvolveUI.SetShow (false);
			RED.SetActive (false, m_mainTraining);
		}
		else if (type == ENUM_TRAIN_TYPE.QianLiXunLian)
		{

			m_hechengUI.SetShow (false);
			m_attrSwapUI.SetShow (false);
			m_qianLiUI.SetShow (true);
			m_monEvolveUI.SetShow (false);
			RED.SetActive (false, m_mainTraining);

            StarMove sm3  =m_qianLiUI.m_Star.gameObject.GetComponent<StarMove>();
            sm3.setBtnXing();
		}
		else if (type == ENUM_TRAIN_TYPE.MonsterEvolve)
		{
			m_hechengUI.SetShow (false);
			m_attrSwapUI.SetShow (false);
			m_qianLiUI.SetShow (false);
			m_monEvolveUI.SetShow (true);

			RED.SetActive (false, m_mainTraining);
		}
	}

    //按钮星星移动效果
    public void SetStar(UISprite uisp)
    {
       // int h = uisp.height;
        //int w = uisp.width;

    }
        

	IEnumerator InitMonData()
	{
		yield return new WaitForSeconds (0.1f);
		if (m_nType == ENUM_TRAIN_TYPE.HeCheng)
		{
			if(m_monster != null)
			{
				m_hechengUI.SetData (m_monster);
			}
		}
		else if (m_nType == ENUM_TRAIN_TYPE.AttrSwap)
		{
			if(m_monster != null)
			{
				m_attrSwapUI.SetData (m_monster);
			}
		}
		else if (m_nType == ENUM_TRAIN_TYPE.QianLiXunLian)
		{
			if(m_monster != null)
			{
				m_qianLiUI.SetData (m_monster);
			}
		}
		else if (m_nType == ENUM_TRAIN_TYPE.MonsterEvolve)
		{
			if (m_monster != null)
			{
				m_monEvolveUI.SetSelData (m_monster);
			}
		}
	}
		

	public void SetShow(bool bShow)
	{
		RED.SetActive (bShow, this.gameObject);
	}

	public void OnClickTypes(GameObject obj)
	{
		int index = int.Parse (obj.name);

		switch (index)
		{
			case 1:
				m_hechengUI.SetShow (true);
//                StarMove sm  =m_hechengUI.star_1.gameObject.GetComponent<StarMove>();
//                StarMove sm2 =m_hechengUI.star_2.gameObject.GetComponent<StarMove>();
//                sm.setBtnXing();
//                sm2.setBtnXing();
                HCStarMove();
				break;
			case 2:
				m_qianLiUI.SetShow (true);
                StarMove sm3  =m_qianLiUI.m_Star.gameObject.GetComponent<StarMove>();
                sm3.setBtnXing();
				break;
			case 3:
				m_attrSwapUI.SetShow (true);
				break;
			case 4:
				m_monEvolveUI.SetShow (true);
				break;
		}
		RED.SetActive (false, m_mainTraining);
	}
    //合成界面 初始化星星
    private void HCStarMove()
    {
        StarMove sm  =m_hechengUI.star_1.gameObject.GetComponent<StarMove>();
        StarMove sm2 =m_hechengUI.star_2.gameObject.GetComponent<StarMove>();
        sm.setBtnXing();
        sm2.setBtnXing();
    }
}
