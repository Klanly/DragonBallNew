using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SkillUpUI : RUIMonoBehaviour 
{
	public TweenScale m_scale;
	public UIGrid m_grid;
	public UILabel m_txtCardTitle;
	public UILabel m_txtCardCnt;

	public UILabel m_txtCoinTitle;
	public UILabel m_txtCoinCnt;

	private Dictionary<int, SkillUpCell> m_dicSkillCells = new Dictionary<int, SkillUpCell>();

	private static SkillUpUI _mInstance;
	public static SkillUpUI mInstance
	{
		get
		{
			return _mInstance;
		}
	}


	private Monster m_monster;
	private Object m_skillPrefab;

	void Awake()
	{
		_mInstance = this;
	}

	void Start()
	{
		m_skillPrefab = PrefabLoader.loadFromPack ("ZQ/SkillUpCell");
		if (m_skillPrefab != null)
		{
			InitSkills ();
		}

		m_txtCardTitle.text = Core.Data.stringManager.getString (5232);
		m_txtCardCnt.text = Core.Data.itemManager.GetBagItemCount (ItemManager.SKILL_CARD).ToString();

		m_txtCoinTitle.text = Core.Data.stringManager.getString (5235);
		m_txtCoinCnt.text = Core.Data.playerManager.RTData.curCoin.ToString ();
	}


	void InitSkills()
	{
		m_dicSkillCells.Clear ();
		m_grid.sorted = true;
//		m_grid.sorting = UIGrid.Sorting;
		List<Skill> list = m_monster.getSkill;
		for (int i = 0; i < list.Count; i++)
		{
			GameObject obj = Instantiate (m_skillPrefab) as GameObject;
			RED.AddChild (obj, m_grid.gameObject);
			SkillUpCell cell = obj.GetComponent<SkillUpCell>();
			cell.InitUI (list[i],  i / 2);
			int index = (i + 1) % 3; 
			cell.name = index.ToString();
			m_dicSkillCells.Add (list [i].sdConfig.ID, cell);
		}
	}

	/// <summary>
	/// 关闭按钮
	/// </summary>
	void OnXBtnClick()
	{
		m_scale.delay = 0;
		m_scale.duration = 0.25f;
		m_scale.from =  Vector3.one;
		m_scale.to = new Vector3(0.01f,0.01f,0.01f);
		m_scale.onFinished.Clear();
		m_scale.onFinished.Add(new EventDelegate(this,"DestroyPanel"));
		m_scale.ResetToBeginning();
		m_scale.PlayForward();
	}

	void DestroyPanel()
	{
		if (TeamUI.mInstance != null)
		{
			TeamUI.mInstance.RefreshMonster (m_monster);
		}
		else
		{
			MonsterInfoUI.OpenUI (m_monster, ShowFatePanelController.FateInPanelType.isInMonsterInfoPanel);
		}
		Destroy(gameObject);
	}

	/// <summary>
	/// 显示界面并初始化
	/// </summary>
	/// <returns>The skill panel.</returns>
	/// <param name="root">Root.</param>
	public static SkillUpUI CreateSkillUpUI(Monster mon)
	{	
		GameObject obj = PrefabLoader.loadFromPack("ZQ/SkillUpUI")as GameObject ;
		if(obj !=null)
		{
			GameObject go = Instantiate (obj) as GameObject;
			RED.AddChild (go, DBUIController.mDBUIInstance._TopRoot);
			SkillUpUI script = go.GetComponent<SkillUpUI>();
			script.m_monster = mon;
			return script;
		}
		return null;
	}

	public void OnClickSkillUpgrade(int skillId)
	{
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);

		SkillUpgradeParam param = new SkillUpgradeParam(Core.Data.playerManager.PlayerID, m_monster.pid, skillId);
		task.AppendCommonParam(RequestType.SKILL_UPGRADE, param);

		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;

		task.DispatchToRealHandler ();
		ComLoading.Open ();
	}


	#region 网络返回

	void testHttpResp_UI (BaseHttpRequest request, BaseResponse response)
	{
		ComLoading.Close ();
		if (response.status != BaseResponse.ERROR) 
		{
			HttpRequest req = request as HttpRequest;
			SkillUpgradeParam param = req.ParamMem as SkillUpgradeParam;

			m_txtCardCnt.text = Core.Data.itemManager.GetBagItemCount (ItemManager.SKILL_CARD).ToString ();
			m_txtCoinCnt.text = Core.Data.playerManager.RTData.curCoin.ToString ();

			List<Skill> list = m_monster.getSkill;
			for (int i = 0; i < list.Count; i++)
			{
				if (list [i].sdConfig.ID == param.skillNum)
				{
					m_dicSkillCells [param.skillNum].InitUI (list [i], i / 2);
				
					List<string> upParamList = list[i].skillLvConfig.GetUpParam(list[i].level);
					m_dicSkillCells [param.skillNum].ShowUpParam(upParamList);
					break;
				}
			}
			DBUIController.mDBUIInstance.RefreshUserInfoWithoutShow ();
		} 
		else 
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
		}
	}

	void testHttpResp_Error (BaseHttpRequest request, string error)
	{
		ComLoading.Close();
		ConsoleEx.DebugLog ("---- Http Resp - Error has ocurred." + error);
		SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(36000));
	}

	#endregion

}
