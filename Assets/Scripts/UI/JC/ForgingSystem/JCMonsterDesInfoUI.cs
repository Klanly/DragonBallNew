using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JCMonsterDesInfoUI : MonoBehaviour 
{
	private static JCMonsterDesInfoUI _mInstance;
	public static JCMonsterDesInfoUI mInstance
	{
		get
		{
			return _mInstance;
		}
	}
	

	public UILabel m_txtDesp;
	public UILabel m_txtName;

	public UILabel[] m_txtSkill;
	public UISprite[] m_spSkill;

	public UILabel[] m_txtFate;
	public UILabel[] m_txtFataTitle;

	public UIButton m_btnSell;
	public UISprite head;
	
	private Monster m_data;
	//private bool m_bOperate = true;
	public StarsUI stars;
	public GameObject root;
	
	public UILabel Lab_Atk;
	public UILabel Lab_Def;
	
	public UISprite UIAttribute_Border;
	
	void Awake()
	{
	}

	public static void OpenUI(Monster monster, bool bOperate = true)
	{
		if(mInstance == null)
		{
			Object prefab = PrefabLoader.loadFromPack("JC/JCMonsterDesInfoUI");
			if(prefab != null)
			{
				GameObject obj = Instantiate(prefab) as GameObject;
				if(obj != null)
				{
					_mInstance = obj.GetComponent<JCMonsterDesInfoUI>();
				
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

		//mInstance.m_bOperate = bOperate;
		mInstance.m_data = monster;
		mInstance.InitUI();
	}

	public void Destroy()
	{
		if(mInstance != null)
		{
			Destroy(this.gameObject);
			_mInstance = null;
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

	private void InitUI()
	{
		if(m_data != null)
		{
            AtlasMgr.mInstance.SetHeadSprite(head, m_data.num.ToString() );
			stars.SetStar(m_data.config.star);
			
			m_txtName.text = m_data.config.name;
			m_txtDesp.text = m_data.config.description;

			int index = 0;
			for(; index < m_data.config.skill.Length; index++)
			{
				SkillData skill = Core.Data.skillManager.getSkillDataConfig(m_data.config.skill[index]);
				if(skill == null)
				{
					RED.SetActive(false, m_spSkill[index].transform.parent.gameObject);
					continue;
				}
				m_txtSkill[index].text = skill.name;

                // m_spSkill[index].spriteName = m_data.config.skill[index].ToString();
                m_spSkill [index].spriteName = skill.Icon.ToString ();
			}

			List<FateData> o = m_data.getMyFate(Core.Data.fateManager);
			List<AoYi> aoyiList = Core.Data.dragonManager.usedToList ();
			MonsterTeam myteam =  Core.Data.playerManager.RTData.curTeam;
			int count = o.Count;
			
			//Debug.Log("count======"+count.ToString());

			for (int i = 0; i < count; i++) 
			{
				m_txtFataTitle[i].text = o[i].name;
				//Debug.Log(o[i].name);
				m_txtFate[i].text =  o[i].description;
				//Debug.Log(o[i].description);
				if (m_data.checkMyFate (o[i], myteam, aoyiList))
				{
					m_txtFataTitle[i].color = new Color (1f,227f/255,43f/255);
					m_txtFate[i].color = new Color (1f,227f/255,43f/255);
				} 
				else 
				{
					m_txtFataTitle[i].color = Color.white;
					m_txtFate[i].color = Color.white;
				}
			}

			for(; count < m_txtFate.Length; count++)
			{
				//Debug.Log("bu ke jian");
				RED.SetActive(false, m_txtFate[count].gameObject, m_txtFataTitle[count].parent.gameObject);
			}

			m_btnSell.TextID = 5030;
			
		    Lab_Atk.text = m_data.config.atk.ToString();
			Lab_Def.text = m_data.config.def.ToString();
			
			if(m_data.RTData ==  null)
			{
				UIAttribute_Border.color = new Color(0,0,0,1f);
			}
			
			
		}
		
		
	}


	void OnClickClose()
	{
		OnXBtnClick();
	}

	void OnXBtnClick()
	{
		TweenScale tween = root.GetComponent<TweenScale>();
		tween.delay = 0;
		tween.duration = 0.25f;
		tween.from =  Vector3.one;
		tween.to = new Vector3(0.01f,0.01f,0.01f);
		tween.onFinished.Clear();
		tween.onFinished.Add(new EventDelegate(this,"DestroyPanel"));
//		tween.Reset();
		tween.ResetToBeginning ();
		tween.PlayForward();
	}

	void DestroyPanel()
	{
		Destroy(gameObject);
	}
	
	
	
}
