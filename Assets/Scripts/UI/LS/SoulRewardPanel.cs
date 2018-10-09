using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoulRewardPanel : MonoBehaviour
{

	private List<Vector3> FinishPos = new List<Vector3>()
	{
		new Vector3(-200f,104f,0f),
		new Vector3(0,100f,0f),
		new Vector3(200f,104f,0f),
		new Vector3(-200f,-63f,0f),
		new Vector3(0f,-63,0f),
		new Vector3(200f,-63f,0f),
	};
	
	public List<UISecretSoulHero> _SoulHeroList = new List<UISecretSoulHero>();
	public UILabel _Title;
	[HideInInspector]
	public ItemOfReward[] p;

	void InitCell()
	{
		int m_count = _SoulHeroList.Count;
		for(int i=0; i<m_count; i++)
		{
			_SoulHeroList[i].gameObject.SetActive(false);
		}

		if(p != null)
		{
			for(int i=0; i<p.Length; i++)
			{
				_SoulHeroList[i].gameObject.SetActive(true);
				_SoulHeroList[i].SetDetail(p[i]);
				_SoulHeroList[i].SetInitPos(i+1,FinishPos[i]);
			}
		}


	}

	void Ok_Btn()
	{
		gameObject.SetActive(false);

	}

	void ResetCell()
	{
		int m_count = _SoulHeroList.Count;
		for(int i=0; i<m_count; i++)
		{
			_SoulHeroList[i].ResetGameobj();
		}
	}

	void ReBuy_OnClick()
	{
		SecretShopMgr.GetInstance().SecretSoulHeroRequest();
	}

	void OnEnable()
	{
		if(p != null)
		{
			SoulData data = Core.Data.soulManager.GetSoulConfigByNum(p[0].pid);
			if(data != null)
			{
				_Title.text = string.Format(Core.Data.stringManager.getString(25138), data.name);
			}

		}
	

		ResetCell();
		InitCell();
	}

	public void Reset()
	{
		OnEnable();
	}

	public void SetHeroDetail(int[] Soul)
	{
		if(Soul != null && Soul.Length == 4)
		{
			for(int i=0; i<Soul.Length; i++)
			{
				_SoulHeroList[i].SetDetail(Soul[i], 0);
			}

		}
	}

}
