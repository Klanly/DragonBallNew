using UnityEngine;
using System.Collections;

public class MonFragUI : MonoBehaviour 
{
	public UISprite m_spHead;
	public UILabel m_txtName;
	public StarsUI m_star;
	public StarsUI m_subStar;

	public SkillTipItem[] m_skills;

	public UILabel m_txtAtk;
	public UILabel m_txtDef;
	public UILabel m_txtMaxAtk;
	public UILabel m_txtMaxDef;

	public FateTipItem[] m_fates;
	public UILabel m_txtDesp;
	public PressTipUI m_tipUI;

	private MonsterData m_data;

	public static MonFragUI mInstance;
		
	public static void OpenUI(MonsterData data)
	{
		Object prefab = PrefabLoader.loadFromPack ("ZQ/MonFragUI");
		if (prefab != null)
		{
			GameObject obj = Instantiate (prefab) as GameObject;
			RED.AddChild (obj, DBUIController.mDBUIInstance._TopRoot);
			mInstance = obj.GetComponent<MonFragUI> ();
			mInstance.InitUI (data);
		}
	}
		
	public void InitUI(MonsterData monData)
	{
		m_data = monData;

		AtlasMgr.mInstance.SetHeadSprite (m_spHead, m_data.ID.ToString());
		m_txtName.text = string.Format(Core.Data.stringManager.getString(5259), m_data.name);
		m_star.SetStar(m_data.star);
		m_subStar.SetStar(6);

		Monster mon = new Monster ();
		RuntimeMonster rtData = new RuntimeMonster ();
		rtData.Attribute = MonsterAttribute.FIRE;
		rtData.curLevel = 30;
		mon.RTData = rtData;
		mon.config = m_data;
		mon.num = m_data.ID;

		for (int i = 0; i < m_skills.Length; i++)
		{
			RED.SetActive (false, m_skills [i].gameObject);
		}

		for (int i = 0; i < mon.getSkill.Count; i++)
		{
			m_skills [i].InitSkill (mon.getSkill[i]);
		}

		m_txtAtk.text = ((int)(m_data.atk)).ToString();
		m_txtDef.text = ((int)m_data.def).ToString();

		int maxAtk = (int)(m_data.atk + 59 * m_data.atkGrowth);
		int maxDef = (int)(m_data.def + 59 * m_data.defGrowth);
		m_txtMaxAtk.text = string.Format (Core.Data.stringManager.getString (5262), maxAtk );
		m_txtMaxDef.text = string.Format (Core.Data.stringManager.getString (5262), maxDef );

		for (int i = 0; i < m_fates.Length; i++)
		{
			RED.SetActive (false, m_fates [i].gameObject);
		}

		FateData[] fd = new FateData[4];
		for(int i = 0; i < m_data.fateID.Length; i++)
		{
			fd[i] = Core.Data.fateManager.getFateDataFromID(m_data.fateID[i]);
			m_fates[i].InitUI(fd[i]);
		}

		m_txtDesp.text = m_data.description;
	}

	public void DestroyUI()
	{
		mInstance = null;
		Destroy (this.gameObject);
	}

	public void ShowTipUI(string strName, string strDesp, Transform tfParent)
	{
		RED.LogWarning("monfragui :: showtipui");
		m_tipUI.OpenUI (strName, strDesp, tfParent);
	}

	public void HideTipUI()
	{
		RED.LogWarning("monfragui :: hidetipui");
		m_tipUI.HideUI ();
	}
}
