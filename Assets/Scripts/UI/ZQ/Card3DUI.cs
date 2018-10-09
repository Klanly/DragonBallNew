using UnityEngine;
using System.Collections;

public class Card3DUI : MonoBehaviour {

	public UILabel m_txtMonName;
	public UILabel m_txtAtk;
	public UILabel m_txtDef;
	public UISprite m_spAttr;
	public ParticleSystem[] m_particles;
	//牌
	public Card3DBg cardObj;

	public Animator m_Animator;
	//星级
	public StarsUI m_stars;
	public StarsUI m_extStars;
	
	CRLuo_Rot_Inversion _CRLuo_Rot_Inversion = null;
	MonsterAttribute previousAttr = MonsterAttribute.DEFAULT_NO;
	[HideInInspector]
	public CRLuo_ShowStage mShowOne=null;
	
	int previouNum = 0;
	private Monster m_data;

	public void InitUI()
	{
		m_Animator.SetFloat("aa", 20.0f);
		m_Animator.SetFloat("Time", 0.0f);
		m_Animator.SetFloat("Reset", 0.0f);

		int star = 1;
		if(m_data != null)
		{
			m_txtMonName.text = m_data.config.name;
			m_txtAtk.text = m_data.baseAttack.ToString();
			m_txtDef.text = m_data.baseDefend.ToString();
			m_spAttr.spriteName = "Attribute_" + ((int)(m_data.RTData.Attribute)).ToString();
			RED.SetActive(true, m_stars.gameObject, m_spAttr.gameObject);
//			Vector3 pos = m_stars.transform.localPosition;
//			if (m_data.config.jinjie == 1)
//			{
//				float val = (m_stars.m_listStars.Length - 1) / 2.0f;
//				pos.x = val * m_stars.m_Width * -1;
//				m_stars.m_pos = ENUM_POS.right;
//				m_extStars.m_pos = ENUM_POS.right;
//			}
//			else
//			{
//				m_stars.m_pos = ENUM_POS.left;
//			}
//			m_stars.transform.localPosition = pos;
//			m_extStars.transform.localPosition = pos;
			RED.SetActive (m_data.config.jinjie == 1, m_extStars.gameObject);
			m_stars.SetStar (m_data.Star);
			m_extStars.SetStar (6);
			star = m_data.Star;
		}
		else
		{
			m_txtMonName.text = "";
			m_txtAtk.text = "";
			m_txtDef.text = "";
			RED.SetActive(false, m_stars.gameObject, m_spAttr.gameObject, m_extStars.gameObject);
		}
		cardObj.SetMonStar (star);
		for (int i = 0; i < m_particles.Length; i++)
		{
			m_particles [i].enableEmission = false;
		}
	}
		
	public void InitAttrs()
	{
		int star = 1;
		if(m_data != null)
		{
			m_txtMonName.text = m_data.config.name;
			m_txtAtk.text = m_data.getAttack.ToString();
			m_txtDef.text = m_data.getDefend.ToString();
			m_spAttr.spriteName = "Attribute_" + ((int)(m_data.RTData.Attribute)).ToString();
			RED.SetActive(true, m_stars.gameObject, m_spAttr.gameObject);
	
			RED.SetActive (m_data.config.jinjie == 1, m_extStars.gameObject);
			m_stars.SetStar (m_data.Star);
			m_extStars.SetStar (6);
			star = m_data.Star;
		}
		else
		{
			m_txtMonName.text = "";
			m_txtAtk.text = "";
			m_txtDef.text = "";
			RED.SetActive(false, m_stars.gameObject, m_spAttr.gameObject, m_extStars.gameObject);
		}
		cardObj.SetMonStar (star);
	}

	public bool IsPosValid()
	{
		return (m_Animator.transform.localEulerAngles == Vector3.zero);
	}


	//-----------------change by jc 2014.12.12------------------
	//----要显示两张卡一样的卡牌，是不需要创建两次这个模型的，显示层显示同一个模型在不同的位置即可
	//--------------------------------------------------------------------
	void SetMonsterModel(CRLuo_ShowStage _mShowOne = null)
	{
		if(_mShowOne != null) 
			this.mShowOne = _mShowOne;
		if(mShowOne == null)
		{
			mShowOne = InitCardFx();
			mShowOne.Try_key = false;
			_CRLuo_Rot_Inversion = mShowOne.GetComponent<CRLuo_Rot_Inversion>();
			_CRLuo_Rot_Inversion.InputOBJ = cardObj.gameObject;
		}
		if(previouNum != m_data.num || m_data.RTData.Attribute != previousAttr)
		{
			mShowOne.ShowCharactor(m_data.num, m_data.RTData.Attribute);
			previouNum = m_data.num;
			previousAttr = m_data.RTData.Attribute;
		}
//		m_Animator.SetFloat ("aa", 20.0f);
	}
	
	public void Del3DModel()
	{
		if(mShowOne != null)
		{
			previouNum = 0;
			previousAttr = MonsterAttribute.DEFAULT_NO;
			mShowOne.DeleteSelf();
			mShowOne = null;
		}
		if(CradSystemFx.GetInstance()._CRLuo_Rot_Inversion != null)
		{
			CradSystemFx.GetInstance()._CRLuo_Rot_Inversion.InputOBJ = null;
		}
		
		if(CradSystemFx.GetInstance()._CRLuo_ShowStage != null)
		{
			CradSystemFx.GetInstance()._CRLuo_ShowStage.DeleteSelf();
		}
		m_data = null;

		if(m_Animator.GetFloat("Time") >= 10.0f)
		{
			m_Animator.SetFloat ("aa", 0.0f);
			m_Animator.SetFloat ("Time", 0.0f);
			m_Animator.SetFloat ("Reset", 20.0f);
		}
		else
		{
			m_Animator.SetFloat ("aa", 20.0f);
			m_Animator.SetFloat ("Time", 0.0f);
			m_Animator.SetFloat ("Reset", 0.0f);
		}
	}

	CRLuo_ShowStage InitCardFx()
	{
		UnityEngine.Object obj = PrefabLoader.loadFromPack("LS/ShowStage_Card");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			CRLuo_ShowStage mStage = go.GetComponent<CRLuo_ShowStage>();
			return mStage;
		}
		return null;
	}

	public CRLuo_ShowStage Show3DCard(Monster mon, bool bPlayAnim = true,CRLuo_ShowStage _ShowStage = null)
	{
		m_data = mon;
		SetMonsterModel(_ShowStage);
		if (bPlayAnim)
		{
			m_stars.transform.localScale = Vector3.zero;
			m_extStars.transform.localScale = Vector3.zero;
			m_txtAtk.transform.localScale = Vector3.zero;
			m_txtDef.transform.localScale = Vector3.zero;
			m_txtMonName.transform.localScale = Vector3.zero;

			RED.SetActive (true, this.gameObject);

			m_Animator.SetFloat ("aa", 20.0f);
			m_Animator.SetFloat ("Reset", 0.0f);
			m_Animator.SetFloat("Time", 10.0f);

			StartCoroutine ("PlayMoveAnim");
		}
		else
		{
			Vector3 pos = m_stars.transform.localPosition;
			pos.x = -130;
			if (m_data.config.jinjie == 1)
			{
				m_stars.m_pos = ENUM_POS.right;
				pos.x = -150;
			}
			else
			{
				m_stars.m_pos = ENUM_POS.left;
			}
			m_stars.transform.localPosition = pos;
			m_extStars.transform.localPosition = pos;
			RED.SetActive (mon.config.jinjie == 1, m_extStars.gameObject);
			m_stars.SetStar (mon.Star);
			m_extStars.SetStar (6);

			m_stars.transform.localScale = Vector3.one;
			m_extStars.transform.localScale = Vector3.one;
			m_txtAtk.transform.localScale = Vector3.one;
			m_txtDef.transform.localScale = Vector3.one;
			m_txtMonName.transform.localScale = Vector3.one;

			m_Animator.SetFloat ("aa", 0.0f);
			m_Animator.SetFloat ("Reset", 0.0f);
			m_Animator.SetFloat("Time", 10.0f);
		}

		InitAttrs ();
		return mShowOne;
	}

	IEnumerator PlayMoveAnim()
	{
		yield return  new WaitForSeconds (2);
	
		if (m_data != null)
		{
			Vector3 pos = m_stars.transform.localPosition;
			pos.x = -130;
			if (m_data.config.jinjie == 1)
			{
				pos.x = -150;
			}

			m_stars.transform.localPosition = pos;
			m_extStars.transform.localPosition = pos;

			m_stars.transform.localScale = Vector3.one * 5;
			m_extStars.transform.localScale = Vector3.one * 5;

			TweenPosition.Begin (m_stars.gameObject, 0.3f, pos);
			TweenPosition.Begin (m_extStars.gameObject, 0.3f, pos);


			TweenScale.Begin (m_stars.gameObject, 0.3f, Vector3.one);
			TweenScale.Begin (m_extStars.gameObject, 0.3f, Vector3.one);

			m_txtAtk.transform.localScale = Vector3.one * 5;
			m_txtDef.transform.localScale = Vector3.one * 5;
			m_txtMonName.transform.localScale = Vector3.one * 5;

			TweenScale.Begin (m_txtAtk.gameObject, 0.3f, Vector3.one);
			TweenScale.Begin (m_txtDef.gameObject, 0.3f, Vector3.one);
			TweenScale.Begin (m_txtMonName.gameObject, 0.3f, Vector3.one);

			StartCoroutine ("PlayShakeAnim");

			Core.Data.soundManager.SoundFxPlay (SoundFx.Fx_PickCard);
		}
	}

	IEnumerator PlayShakeAnim()
	{
		yield return  new WaitForSeconds (0.3f);
		MiniItween.Shake(this.gameObject, new Vector3(20,20,0), 0.4f, MiniItween.EasingType.Normal, false);
		for (int i = 0; i < m_particles.Length; i++)
		{
			m_particles [i].enableEmission = true;
			m_particles [i].Play ();
		}
	}
}
