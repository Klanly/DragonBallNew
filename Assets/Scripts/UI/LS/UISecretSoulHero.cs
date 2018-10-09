using UnityEngine;
using System.Collections;

public class UISecretSoulHero : RUIMonoBehaviour 
{

	public UILabel _Name;
	public StarsUI _StarGroup;
	public UISprite _Head;
	public UISprite _Circle;
	public UISprite _Tag;
	public UILabel _Num;
	public UISprite _Numbg;
	public ParticleSystem _ParticleSystem1;
	public ParticleSystem _ParticleSystem2;

	ItemOfReward _p;

	void Start () 
	{
		if(_ParticleSystem1 != null && _ParticleSystem2 != null)
		{
			_ParticleSystem1.Stop();
			_ParticleSystem2.Stop();
        }

    }

	void PlayParticle()
	{
		if(_p != null)
		{
			if(_p.star == 4)
			{
				_ParticleSystem1.Play();
			}
			else if(_p.star == 5)_ParticleSystem2.Play();
		}

	}
	
	void FinishHead(GameObject obj)
    {
        PlayParticle();
	}

	public void SetInitPos(int m_Id, Vector3 _temp)
	{

		MiniItween m =MiniItween.MoveTo(gameObject,_temp, 0.4f, MiniItween.EasingType.Linear , false);
		m.myDelegateFuncWithObject += FinishHead;
		MiniItween.ScaleTo(gameObject, new Vector3(1f,1f,1f), 0.4f);
    }
    
	public void SetDetail(int num, int numvalue)
	{
		if(_Numbg != null && _Num != null)
		{
			_Numbg.gameObject.SetActive(false);
			_Num.SafeText("");
		}
		ConfigDataType type = DataCore.getDataType(num);
		string _str = "";
		int _starnum = 0;
		if(numvalue > 0)
		{
			if(_Numbg != null && _Num != null)
			{
				_Numbg.gameObject.SetActive(true);
				_Num.SafeText(numvalue.ToString());
			}
		}

		if(_Tag != null)_Tag.gameObject.SetActive(false);
		switch(type)
		{
			case ConfigDataType.Monster:
				AtlasMgr.mInstance.SetHeadSprite(_Head, num.ToString());
				_str = Core.Data.monManager.getMonsterByNum(num).name;
				_starnum = Core.Data.monManager.getMonsterByNum(num).star;
				break;
			case ConfigDataType.Item:
				_Head.atlas = AtlasMgr.mInstance.itemAtlas;
//				_Head.spriteName = num.ToString();
				_Head.spriteName = Core.Data.itemManager.getItemData(num).iconID.ToString();
				_starnum = Core.Data.itemManager.getItemData(num).star;
				_str = Core.Data.itemManager.getItemData(num).name;
				break;
			case ConfigDataType.Equip:
				_Head.atlas = AtlasMgr.mInstance.equipAtlas;
				_Head.spriteName = num.ToString();
				_starnum = Core.Data.EquipManager.getEquipConfig(num).star;
				_str = Core.Data.EquipManager.getEquipConfig(num).name;
				break;
			case ConfigDataType.Gems:
				_Head.atlas = AtlasMgr.mInstance.commonAtlas;
				_Head.spriteName = num.ToString();
				_starnum = Core.Data.gemsManager.getGemData(num).star;
				_str = Core.Data.gemsManager.getGemData(num).name;
            break;
			case ConfigDataType.Frag:
				SoulInfo info = new SoulInfo (0, num, 1);
				Soul soul = new Soul ();
				soul.m_config = Core.Data.soulManager.GetSoulConfigByNum (info.num);
				soul.m_RTData = info;
				if (soul.m_config.type == (int)ItemType.Monster_Frage)
				{
					MonsterData mon = Core.Data.monManager.getMonsterByNum (soul.m_config.updateId);
					if(mon != null)
					{
						AtlasMgr.mInstance.SetHeadSprite (_Head, mon.ID.ToString ());
						_Head.MakePixelPerfect ();
					}
				}
				else if (soul.m_config.type == (int)ItemType.Equip_Frage)
				{
					EquipData equip = Core.Data.EquipManager.getEquipConfig (soul.m_config.updateId);
					if (equip != null)
					{
						_Head.atlas = AtlasMgr.mInstance.equipAtlas;
						_Head.spriteName = soul.m_config.updateId.ToString ();
					}
				}
				else  
				{
					_Head.atlas = AtlasMgr.mInstance.itemAtlas;
					_Head.spriteName = soul.m_RTData.num.ToString ();
				}
				_starnum = Core.Data.soulManager.GetSoulConfigByNum(num).star;
				_str = Core.Data.soulManager.GetSoulConfigByNum(num).name;
				if(_Tag != null)_Tag.gameObject.SetActive(true);
				break;
        default:
            RED.LogError(" not found  : " +  num);
            break;
        }

		_StarGroup.SetStar(_starnum);
		_Name.text = _str;
    }

	public void SetDetail(ItemOfReward p)
	{
		SetDetail(p.pid, p.num);
	}
        
    public void ResetGameobj()
    {
        gameObject.transform.localScale = new Vector3(0.01f,0.01f,0.01f);
		gameObject.transform.localPosition = Vector3.zero;
	}
}
