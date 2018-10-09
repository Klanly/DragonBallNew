using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class JCPVESaoDangPanel : MonoBehaviour {

	
	public List<JCPVESaoDangRewardElement> List_Elements = new List<JCPVESaoDangRewardElement>();
	
	public UIGrid uiGrid;
	
	public UIPanel uiPanel;
	
	public TweenScale ContentScale1;
	public TweenScale ContentScale2;
	
    public TweenScale CloseScale ;
	
	public GameObject SaoDangOK;
	
	public GameObject UIComboReward;
	//锚点(子物件)
	public GameObject Anchor;
	//裁剪区域
	Vector4 panelClipRegion = new Vector4();
	//连击奖励金币
	int ComboRewardCoin = 0;
	
	//连击奖励金币Label
	public UILabel Lab_ComboRewardCoin;
	
	void Start () 
	{		
		panelClipRegion = uiPanel.baseClipRegion;
		BackgroundPlay(true);
	}
	
	private int SaoDangData = 0;
	private List<BattleSequence> listdata = null;
	
	static JCPVESaoDangPanel _this = null;
	
	public static JCPVESaoDangPanel OpenUI(List<BattleSequence> listdata)
	{
		UnityEngine.Object obj = PrefabLoader.loadFromPack("JC/JCPVESaoDangPanel");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			RED.AddChild(go, DBUIController.mDBUIInstance._TopRoot);
			_this = go.GetComponent<JCPVESaoDangPanel>();
			_this.SaoDangData = listdata.Count;
			_this.listdata = listdata;
			_this.CollectLevelUp();
		}
		return _this;
	}
	
	
	void BackgroundPlay(bool forwardOrReverse)
	{
		if(forwardOrReverse)
		{
			ContentScale1.PlayForward();
			ContentScale2.PlayForward();
		}
		else
		{
			ContentScale1.PlayReverse();
			ContentScale2.PlayReverse();
		}
	}
	
	
	
	public static void SafeDelete()
	{
		if(_this != null)
			_this.DestroyMe();
	}
	
	
	
	
	bool ForwardOrReverse = false;
	
	void OpenWindowsFinished()
	{
		ForwardOrReverse = !ForwardOrReverse;
		if(ForwardOrReverse)
			Init();
		else
            DestroyMe();
	}
	
	void Init()
	{
		CreateElement(SaoDangData);
		PlayRewardAnimation();
	}
	
	void CreateElement(int Num)
	{
			Object prefab = PrefabLoader.loadFromPack("JC/JCPVESaoDangRewardElement");
			if(prefab != null)
			{
			    int i = 0;
				for(;i<Num;i++)
				{
					GameObject obj = Instantiate(prefab) as GameObject;
				    obj.transform.parent = uiGrid.transform;
				    obj.transform.localPosition = new Vector3(0,-uiGrid.cellWidth*i ,0);
					obj.transform.localScale = Vector3.one;
				    JCPVESaoDangRewardElement element = obj.GetComponent<JCPVESaoDangRewardElement>();
				    if(element != null)		
					{
					    element.SetData(listdata[i],i);
						List_Elements.Add( element );
					}
				    obj.SetActive(false);
				}
			    SaoDangOK.transform.localPosition = new Vector3(0,-uiGrid.cellWidth*(i-1) - 140f ,0);			
			}
	}
	
	
	//播放显示奖励动画
	void PlayRewardAnimation()
	{
		for(int i=0; i< List_Elements.Count; i++)
			List_Elements[i].onFinished = OnPlayRewardAnimationFinished;

		List_Elements[0].Play();
		uiPanel.GetComponent<UIScrollView>().enabled = false;
	}
	
	

	
	int RewardAnimationIndex = 0;
	void OnPlayRewardAnimationFinished()
	{		
		#region 同步精力和钱经验
		BattleSyncData syncdata = this.listdata[RewardAnimationIndex].sync;
		PlayerManager pm = Core.Data.playerManager;

		if(syncdata != null)
		{
			pm.SetCurUserLevel(syncdata.lv);
			pm.RTData.curCoin = syncdata.coin;
	        pm.RTData.curExp = syncdata.ep;
			pm.RTData.curJingLi = syncdata.eny;
	
			DBUIController.mDBUIInstance.RefreshUserInfo();
	    }
		#endregion

		
		RewardAnimationIndex++;		
		Vector3 pos = uiPanel.transform.localPosition;
		Vector2 size =  uiPanel.GetViewSize();
		if(RewardAnimationIndex < List_Elements.Count)
		{			
			float cha = (RewardAnimationIndex+1) * uiGrid.cellWidth - size.y - pos.y;
			if(cha > 0)
			{		
				pos.y += cha;
				SpringPanel.Begin(uiPanel.gameObject,pos,16f).onFinished = () =>
				{			
				    List_Elements[RewardAnimationIndex].Play();
				};
			}
			else
			{
				List_Elements[RewardAnimationIndex].Play();
			}
		}
		else
		{
			float cha = RewardAnimationIndex * uiGrid.cellWidth+240f  - size.y - pos.y;
			if(cha > 0)
			{
				pos.y += cha;
				SpringPanel.Begin(uiPanel.gameObject,pos,16f).onFinished = () =>
				{ 
					SaoDangOK.SetActive(true);
					OnSaoDangOKFinshed();
				};
			}
			else
			{
				SaoDangOK.SetActive(true);
				OnSaoDangOKFinshed();
			}
		}
	}
	
	//扫荡模块出现以后
	void OnSaoDangOKFinshed()
	{
		Vector3 pos = uiPanel.transform.localPosition;
		Vector2 size =  uiPanel.GetViewSize();
		float cha = RewardAnimationIndex * uiGrid.cellWidth+240f+130f  - size.y - pos.y;
		if(cha > 0)
		{
			pos.y += cha;
			SpringPanel.Begin(uiPanel.gameObject,pos,16f).onFinished = () =>
			{
				UIComboReward.SetActive(true);
				CloseScale.PlayForward();
			    uiPanel.GetComponent<UIScrollView>().enabled = true;
				//扫荡结束处理升级
		        AutoLevelUp();		
			};
		}
		else
		{
			UIComboReward.SetActive(true);
			CloseScale.PlayForward();
			uiPanel.GetComponent<UIScrollView>().enabled = true;
			//扫荡结束处理升级
		     AutoLevelUp();		
		}
		
		
	}
	
	//是否存在连击奖励
	bool isHaveComboReward = true;
	
	//搜集升级情况(包括多级连升)
	List<int> Lvs = new List<int>();
	void CollectLevelUp()
	{
		PlayerManager pm = Core.Data.playerManager;
		foreach(BattleSequence  bsq in this.listdata)
		{
			BattleSyncData syncdata = bsq.sync;// this.listdata[RewardAnimationIndex].sync;
			if(syncdata != null)
			{
				
				if(pm.Lv < syncdata.lv)
				{
					if(Lvs.Count > 0)
					{
						if(Lvs[Lvs.Count-1] < syncdata.lv)
					    Lvs.Add(syncdata.lv);
					}
					else
						Lvs.Add(syncdata.lv);	
				}
			}
			ComboReward comboReward = bsq.comboReward; 
			if(comboReward != null)
				ComboRewardCoin += bsq.comboReward.award;
		}
		
		Lab_ComboRewardCoin.text ="+"+ ComboRewardCoin.ToString();
	}
	
	//自动升级(包括多级连升)
	void AutoLevelUp()
	{
		if(Lvs.Count > 0)
		{
			LevelUpUI.OpenUI(Lvs[0]).OnClose = OnCloseUpLevel;		
			Lvs.RemoveAt(0);
			DBUIController.mDBUIInstance.RefreshUserInfo();
		}
	}

	//关闭升级界面回调
	void OnCloseUpLevel()
	{
		AutoLevelUp();
	}
	
	bool isClose = false;
	void Close()
	{
		Vector3 pos = uiPanel.transform.localPosition;
		pos.y = - pos.y;
		Anchor.transform.localPosition = pos;
		uiGrid.transform.parent = Anchor.transform;
		BackgroundPlay(false);
		isClose = true;
	}
	
	void DestroyMe()
	{
		Destroy(gameObject);
		_this = null;
	}
	
	
	void Update()
	{
		if(isClose)
		{
		    float ratio = Anchor.transform.localScale.x;		
			Vector4 pos = uiPanel.baseClipRegion;
			pos.w = panelClipRegion.w*ratio;
			pos.z = panelClipRegion.z*ratio;
			uiPanel.baseClipRegion = pos;
		}
	}
	
	
}
