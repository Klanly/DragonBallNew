using UnityEngine;
using System.Collections;

public class UICardHeadCell : RUIMonoBehaviour {

	public UILabel _Name;
	public UISprite _Head;
	public UISprite _Attr;
	public StarsUI starsObj;
	public UIButton mHeadBtn;
	public UISprite _Circle;

	public TweenScale _Scale;

	public ParticleSystem _ParticleSystem_lan;
	public ParticleSystem _ParticleSystem_zi;

	int mId;
	int mIndex;

	Vector3 mLocalPos;

	Monster _Monster;

	int m_StarNum = 0;

	ItemOfReward _ItemOfReward;

	void Start()
	{
		_ParticleSystem_lan.Stop();
		_ParticleSystem_zi.Stop();
	}

	public void SetDetail(ItemOfReward mreward)
	{
		_ItemOfReward = mreward;
		if (mreward.getCurType () == ConfigDataType.Monster)
		{
			Monster data = mreward.toMonster (Core.Data.monManager);
			ShowMonster(data);
		}
		else if (mreward.getCurType () == ConfigDataType.Equip)
		{
			Equipment data = mreward.toEquipment (Core.Data.EquipManager, Core.Data.gemsManager);
			ShowEquip(data);
		}
		else if (mreward.getCurType () == ConfigDataType.Gems)
		{
			Gems data = mreward.toGem (Core.Data.gemsManager);
			ShowGem(data);
		}
		else if (mreward.getCurType () == ConfigDataType.Frag)
		{
			Soul data = mreward.toSoul (Core.Data.soulManager);
			ShowFrag(data);
		}
		else if (mreward.getCurType () == ConfigDataType.Item)
		{
			Item item = mreward.toItem (Core.Data.itemManager);
			ShowItem(item);
		}
		else
		{
			RED.LogWarning("unknow reward type");
		}
		_Head.MakePixelPerfect();
    }

	void ShowMonster(Monster _data)
	{
		if(_data != null)
		{
			_Name.text = _data.config.name;
			AtlasMgr.mInstance.SetHeadSprite(_Head, _data.config.ID.ToString());
			_Attr.gameObject.SetActive(true);
			int attr = (int)(_data.RTData.Attribute); 
			m_StarNum = _data.Star;
			starsObj.SetStar(_data.Star);

			if (Core.Data.monManager.IsExpMon(_data.num))
			{
				_Attr.spriteName = "common-1055" ;
				_Circle.spriteName = "star6";
			}
			else
			{
				_Circle.spriteName = "star" + _data.RTData.m_nAttr.ToString();
				_Attr.spriteName = "Attribute_" + attr.ToString();
			}
			_Attr.MakePixelPerfect();

		}
	}

	void ShowGem(Gems _data)
	{
		if(_data != null)
		{
			_Name.text = _data.configData.name;
			_Head.atlas = AtlasMgr.mInstance.commonAtlas;
			_Head.spriteName = _data.configData.ID.ToString();
			_Attr.gameObject.SetActive(false);
			m_StarNum = _data.configData.star;
			starsObj.SetStar(_data.configData.star);
			_Circle.spriteName = "star6";
		}

	}

	void ShowEquip(Equipment _data)
	{
		if(_data != null)
		{
			_Name.text = _data.ConfigEquip.name;
			_Head.atlas = AtlasMgr.mInstance.equipAtlas;
			_Head.spriteName = _data.ConfigEquip.ID.ToString();
			_Attr.gameObject.SetActive(false);
			m_StarNum = _data.ConfigEquip.star;
			starsObj.SetStar(_data.ConfigEquip.star);
			_Circle.spriteName = "star6";
		}
	}

	void ShowFrag(Soul _data)
	{
		if(_data != null)
		{
			if (_data.m_config.type == (int)ItemType.Monster_Frage) 
			{
				MonsterData mon = Core.Data.monManager.getMonsterByNum(_data.m_config.updateId);
				if(mon != null)
				{
					_Name.text = _data.m_config.name;
					AtlasMgr.mInstance.SetHeadSprite(_Head, mon.ID.ToString());
					_Attr.gameObject.SetActive(true);
					_Attr.spriteName = "bag-0003";
					m_StarNum = mon.star;
					starsObj.SetStar(mon.star);
					_Circle.spriteName = "star6";
				}

			}
			else if (_data.m_config.type == (int)ItemType.Equip_Frage) 
			{
				EquipData equip = Core.Data.EquipManager.getEquipConfig (_data.m_config.updateId);
				if (equip != null)
				{
					_Name.text = _data.m_config.name;
					_Head.atlas = AtlasMgr.mInstance.equipAtlas;
					_Head.spriteName = _data.m_config.updateId.ToString ();
					_Circle.spriteName = "star6";
					m_StarNum = equip.star;
					starsObj.SetStar(equip.star);
					_Attr.gameObject.SetActive(true);
					_Attr.spriteName = "sui";
				}
				
			}
			else 
			{
				_Name.text = _data.m_config.name;
				AtlasMgr.mInstance.SetHeadSprite(_Head, _data.m_config.ID.ToString());
				_Attr.gameObject.SetActive(false);
				m_StarNum = _data.m_config.star;
				starsObj.SetStar(_data.m_config.star);
				_Circle.spriteName = "star6";
			}
		}
	}

	void ShowItem(Item _data)
	{
		//如果是金币则，只能特殊处理，金币的数量太大
		string name = string.Empty;
		if(_data.configData.type == (int)ItemType.Coin || _data.configData.type == (int)ItemType.Stone)
		{
			name = _data.configData.name + "X" +  _ItemOfReward.num.ToString();
		}
		else
		{
			name = _data.configData.name;
		}
		
		_Name.text = name;
		_Head.atlas = AtlasMgr.mInstance.itemAtlas;
//		_Head.spriteName = _data.RtData.num.ToString();
		_Head.spriteName = _data.configData.iconID.ToString ();
		_Circle.spriteName = "star6";
		_Attr.gameObject.SetActive(false);
		m_StarNum = _data.configData.star;
		starsObj.SetStar(_data.configData.star);
	}


	public void SetInitPos(int m_Id, int m_index)
	{
		mId = m_Id;
		mIndex = m_index;

        if(CradSystemFx.GetInstance().mTargetPosDic.TryGetValue(m_index, out mLocalPos))
		{
			MiniItween m =MiniItween.MoveTo(_Scale.gameObject,mLocalPos, 0.4f, MiniItween.EasingType.Linear , false);
			m.myDelegateFuncWithObject += FinishHead;
			MiniItween.ScaleTo(_Scale.gameObject, new Vector3(1f,1f,1f), 0.4f);
		}
	}

	void PlayParticle()
	{
		if(m_StarNum == 4)
		{
			_ParticleSystem_lan.Play();
		}
		else if(m_StarNum == 5)_ParticleSystem_zi.Play();
	}

	void FinishHead(GameObject obj)
	{
		PlayParticle();
        _Scale.enabled = true;
		UITweener.Begin<TweenScale>(_Scale.gameObject, 0.2f);
		mHeadBtn.enabled = true;

		if(mIndex == 9)
		{
			CradSystemFx.GetInstance().mCardSystemPanel.SetTenBtn();
			Core.Data.taskManager.AutoShowPromptWord();
		}
	}

	void DownLoadFinish(AssetTask task)
	{
		if(!CradSystemFx.GetInstance().mIsChoose)
		{
			CradSystemFx.GetInstance().InitCardCell(_ItemOfReward, mId, mIndex);
			CradSystemFx.GetInstance().mIsChoose = true;
			CradSystemFx.GetInstance().mCardSystemPanel.mAllOk.gameObject.SetActive(false);
			CradSystemFx.GetInstance().mCardSystemPanel.m_AgainBtnRoot.gameObject.SetActive(false);
        }
    }
    
    void OnClick()
	{
		if(_ItemOfReward.getCurType() == ConfigDataType.Monster)
		{
			if(CradSystemFx.GetInstance().m_UIEggCellIntroduct != null)
			{
				CradSystemFx.GetInstance().m_UIEggCellIntroduct.dealloc();
			}
			#if SPLIT_MODEL
			if (!Core.Data.sourceManager.IsModelExist (mId))
			{
				if(!CradSystemFx.GetInstance().mIsChoose)
				{
					UIDownModel.OpenDownLoadUI (mId, DownLoadFinish, null, UIDownModel.WinType.WT_Two);
				}
			}
			else
			{
				DownLoadFinish(null);
			}
			#else
			DownLoadFinish(null);
			#endif
		}
		else
		{
			if(!CradSystemFx.GetInstance().mIsChoose)
			{
				if(CradSystemFx.GetInstance().m_UIEggCellIntroduct == null)CradSystemFx.GetInstance().InitHeadIntroduce(_ItemOfReward, new Vector3(mLocalPos.x, mLocalPos.y+60, mLocalPos.z));
				else 
				{
					CradSystemFx.GetInstance().SetIntroductTrue(_ItemOfReward ,new Vector3(mLocalPos.x, mLocalPos.y+60, mLocalPos.z));
				}
			}
		}

	}

	void OnDestroy()
	{
		_Name = null;
		_Head = null;
		_Attr = null;
		_Scale = null;
		_ParticleSystem_lan = null;
		_ParticleSystem_zi = null;
	}

}
