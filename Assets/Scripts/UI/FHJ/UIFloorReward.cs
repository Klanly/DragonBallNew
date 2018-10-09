using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UIFloorReward : MonoBehaviour 
{
    public UILabel nothing;
    public UILabel exp;
    public UILabel coin;
    public UILabel other;
    
    public UILabel rewardTitle;
    public UILabel rewardExp;
    public UILabel rewardCoin;
    
    public GameObject expPos;
    public GameObject coinPos;
    public GameObject nothingPos;
    public GameObject otherPos;
    public UISprite att;
    public UISprite border;
    public UISprite spr_head;
    public UILabel _name;
    
    public GameObject starRoot;
    public GameObject starTemp;
    
    public float min = 0.2f;
    public float max = 0.4f;
    public float dur = 1.2f;
//    public delegate void onClose();
//    public onClose OnClose = null;
	public System.Action Close = null;
	
	private BattleSequence m_rewardData;
	
//	public UIAtlas atlas_Equipment;
//	public UIAtlas atlas_Monster;
//	public UIAtlas atlas_Item;
	public	UISprite blackAlpha;
	
	public List<Transform> KuangPos = new List<Transform>();
	public UISprite kuang;
	
	int KuangIndex = 0;
	float timeSpeed = 0.1f;
	float timecount = 0;
	int KuangIndexMax = 0;
	bool KuangStop = false;
	int FinishedIndex = 0;
	bool isShowReward = true;
	public GameObject pointerRoot;
	public GameObject background;

	
	
	void PlayEffect()
	{
		KuangStop = true;	
		pointerRoot.SetActive(true);
		isShowReward = false;		
	}
	
	
	void Update()
	{
		timecount+=Time.deltaTime;
		if(timecount<timeSpeed)
			return;
		else
			timecount = 0;
		
		if(KuangIndex>KuangIndexMax)KuangIndex=0;
		
		if(KuangStop && KuangIndex == FinishedIndex)
		{
			if(!isShowReward)
			{
			    kuang.transform.localPosition = KuangPos[KuangIndex].localPosition;
				isShowReward = true;
				pointerRoot.SetActive(false);
				background.SetActive(true);
				StartCoroutine(PlaySoundFx());
			}
			return;
		}
		
		kuang.transform.localPosition = KuangPos[KuangIndex].localPosition;
	    KuangIndex++;			
	}
	
    void Awake()
    {
		KuangIndexMax = KuangPos.Count -1;
        nothing.text = Core.Data.stringManager.getString(10004);
        exp.text = Core.Data.stringManager.getString(10005);
        coin.text = Core.Data.stringManager.getString(10007);
        other.text = Core.Data.stringManager.getString(10006);
        rewardTitle.text = Core.Data.stringManager.getString(10008);
		
		
    }


    bool stared = false;

    public void ShowStar()
    {
        if(! stared)
        {
            stared = true;
            NGUITools.AddChild(starRoot, starTemp);
        }
        blackAlpha.gameObject.SetActive(true);
    }

    void Init(int _exp, int _coin)
    {
        rewardExp.text = _exp.ToString();
        rewardCoin.text = _coin.ToString();
    }

	public void ShowReward(BattleSequence data)
    {
		
        //StartCoroutine(PlaySoundFx());
		m_rewardData = data;
		if(data != null)
        {
			Init(data.reward.bep, data.reward.bco);

			if(data.reward.eco > 0)
            {
                NGUITools.SetActive(coinPos, true);
                //_name.text = coin.text;
                _name.text =Core.Data.stringManager.getString(9060)+data.reward.eco.ToString() ; // yangchenguang 修改 额外奖励修改成具体奖励数
				FinishedIndex = 3;
            }
			else if(data.reward.eep > 0)
            {
                NGUITools.SetActive(expPos, true);
                _name.text = Core.Data.stringManager.getString(9060)+data.reward.eep.ToString();
				FinishedIndex = 1;
            }
			else if(data.reward.p == null || data.reward.p.Length<1)
            {
                NGUITools.SetActive(nothingPos, true);
                _name.text = nothing.text;
				FinishedIndex = 0;
            }
            else
            {
				FinishedIndex = 2;
               // _name.text = other.text;
				ItemOfReward item = data.reward.p[0];
                NGUITools.SetActive(otherPos, true);
				
				att.enabled = false;
                switch(item.getCurType())
                {
                case ConfigDataType.Monster:
					att.enabled = true;
                    Monster m = item.toMonster(Core.Data.monManager);
                    if(m != null)
                    {
                        att.spriteName = "Attribute_" + ((int)m.RTData.Attribute).ToString();
                        border.spriteName = "Attribute_Border_" + ((int)m.RTData.m_nAttr).ToString();
                        _name.text = m.config.name;
						AtlasMgr.mInstance.SetHeadSprite(spr_head,item.pid.ToString());
                    }
                    break;
				case ConfigDataType.Equip:
					spr_head.atlas = AtlasMgr.mInstance.equipAtlas;
					_name.text = Core.Data.EquipManager.getEquipConfig(item.pid).name;
					spr_head.spriteName = item.pid.ToString();			
					break;
				case ConfigDataType.Frag:
					spr_head.atlas = AtlasMgr.mInstance.itemAtlas;
					Soul soul = Core.Data.soulManager.GetSoulByID(item.ppid);
					if(soul != null)_name.text = soul.m_config.name;		
                    NGUITools.SetActive(att.gameObject, false);
					spr_head.spriteName = item.pid.ToString();
					break;
                default:
					/*道具*/
					spr_head.atlas = AtlasMgr.mInstance.itemAtlas;
					_name.text = Core.Data.itemManager.getItemData(item.pid).name;
                    NGUITools.SetActive(att.gameObject, false);
					spr_head.spriteName = item.pid.ToString();
                    break;
                }
				
				spr_head.MakePixelPerfect();
            }
        }
		
		Invoke("PlayEffect",1f);
    }

    IEnumerator PlaySoundFx( ) {
        //播放声效
        Core.Data.soundManager.SoundFxPlay(SoundFx.FX_Searching);
        yield return new WaitForSeconds(0.6f);
        Core.Data.soundManager.SoundFxPlay(SoundFx.FX_SerachGet);
    }

    public void OnColse()
    {
        //UpdataPlayerInfo();
        CityFloorData.Instance.isCanClick = true;
		
		UIMiniPlayerController.Instance.freshPlayerInfoView ();
		DBUIController.mDBUIInstance.RefreshUserInfo ();
		
		if(m_rewardData != null)
		{
			if(m_rewardData.sync.lv > Core.Data.temper.mPreLevel)
			{
                Core.Data.soundManager.SoundFxPlay(SoundFx.FX_UserLevelUp);
				LevelUpUI.OpenUI();
				Core.Data.temper.mPreLevel = Core.Data.playerManager.Lv;
			}
		}

        if(Close != null)
        {
            Close();
        }
		
		Destroy(gameObject);
		
    }
}
