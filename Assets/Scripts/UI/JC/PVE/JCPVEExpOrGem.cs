using UnityEngine;
using System.Collections;

public class JCPVEExpOrGem : MonoBehaviour {

	
	public UILabel Lab_Exp;
	public UILabel Lab_Gem;
	public UISprite Spr_Exp;
	public UISprite Spr_Gem;
	public UILabel Lab_Exp_NeedStone;
	public UILabel Lab_Gem_NeedStone;
	JCPVEMainController main;
	NewDungeonsManager dm;
	void Start () 
	{

	}
	
	private static JCPVEExpOrGem _mInstance;
	public static JCPVEExpOrGem mInstance
	{
        get {
			return _mInstance;
		}
	}
	
	
	public static void OpenUI()
	{
		if(_mInstance == null)
		{
			Object prefab = PrefabLoader.loadFromPack("JC/JCPVEExpOrGem");
			if(prefab != null)
			{
				GameObject obj = Instantiate(prefab) as GameObject;
				RED.AddChild(obj, DBUIController.mDBUIInstance._TopRoot);
//				obj.transform.localScale = Vector3.one;
//				obj.transform.localPosition = Vector3.zero;
//				obj.transform.localEulerAngles = Vector3.zero;
				DBUIController.mDBUIInstance._PVERoot.AddPage(obj.name,obj);
				_mInstance = obj.GetComponent<JCPVEExpOrGem>();		
				_mInstance.Init();
				
			}
		}
	}
	
	
	/*
	 * {"ID":9086,"txt":"每周一、三、五、日开启"}
       {"ID":9087,"txt":"每周二、四、六、日开启"}
	 * */
	public void Init()
	{
		Refresh();
		JCPVEMainController.Instance.FreshExpOrGem =() =>
		{
			Refresh();
		};
	}
	
	void Refresh()
	{
		main = JCPVEMainController.Instance;
		
		JCPVETimerManager m = JCPVETimerManager.Instance;
		
		dm = Core.Data.newDungeonsManager;
		if(dm.explorDoors != null)
		{
			if(dm.explorDoors.exp == null)
			{
				isExpFBActive = false;
				ExploreConfigData explore = dm.GetExploreData (3);
				if (explore != null)
				{
					if(Lab_Exp_NeedStone.gameObject.activeSelf)Lab_Exp_NeedStone.gameObject.SetActive(false);
					if(!Lab_Exp.gameObject.activeSelf) Lab_Exp.gameObject.SetActive(true);
					Lab_Exp.text = explore.opendayDesp;
				}
			}
			else
			{
				m.ExpFBCoding = ExpFBCoding;
				ExpFBCoding(Core.Data.temper.ExpDJS );
			}
			
			if(dm.explorDoors.gems == null)
			{
				isGemFBActive = false;
				ExploreConfigData explore = dm.GetExploreData (4);
				if (explore != null)
				{
					if(Lab_Gem_NeedStone.gameObject.activeSelf)Lab_Gem_NeedStone.gameObject.SetActive(false);
					if(!Lab_Gem.gameObject.activeSelf) Lab_Gem.gameObject.SetActive(true);
					Lab_Gem.text = explore.opendayDesp;
				}
			}
			else
			{
				m.GemFBCoding = GemFBCoding;	
				GemFBCoding(Core.Data.temper.GemDJS);
			}
		}
	}
	
	


	
	
	
	void OnBtnClick(GameObject btn)
	{
		OnBtnClick(btn.name);
		
	}
	
	void OnBtnClick(string btnName)
	{
		switch(btnName)
		{
		case "Btn_Exp":
			if(isExpFBActive)
			{
				JCPVEExpController.OpenUI(0).Exit = CallBackBtnReturn;
				gameObject.SetActive (false);
				main.gameObject.SetActive(false);
			}
			else
			{
				JCPVEMainController.Instance.AutoShowBuyBox(3);
			}
			break;
		case "Btn_Gem":
			if(isGemFBActive)
			{
				JCPVEExpController.OpenUI(1).Exit = CallBackBtnReturn;
				gameObject.SetActive (false);
				main.gameObject.SetActive(false);
			}
			else
			{
				JCPVEMainController.Instance.AutoShowBuyBox(4);
			}
			break;
		case "Btn_Close":
			OnClose();
			break;
		}
	}




	void ExpFBCoding(long time)
	{
		if(this)
		{
			StringManager sm = Core.Data.stringManager;
			int passCount = 0;
			int Count = 2;
			if(dm.explorDoors.exp != null)
			{
			    passCount = dm.explorDoors.exp.passCount;
			    Count = dm.explorDoors.exp.count;
			}
			if(passCount == Count)
			{
				//Lab_Exp.text =  sm.getString(9084)+":"+passCount.ToString()+"/"+Count.ToString();
				SetLabExpProgress(passCount,Count,dm.explorDoors.exp.needStone);
				isExpFBActive = false;
			}
			else
			{
				if(Lab_Exp_NeedStone.gameObject.activeSelf)Lab_Exp_NeedStone.gameObject.SetActive(false);
				if(!Lab_Exp.gameObject.activeSelf) Lab_Exp.gameObject.SetActive(true);
				isExpFBActive = time == 0;	
				if(time == 0)
					Lab_Exp.text =  sm.getString(9084)+":"+passCount.ToString()+"/"+Count.ToString();
				else
				    Lab_Exp.text =  sm.getString(9084)+":"+passCount.ToString()+"/"+Count.ToString()+"\n"+sm.getString(9085)+":"+DateHelper.getDateFormatFromLong(  time );		
			}
		}
	}
	
	void GemFBCoding(long time)
	{
		if(this)
		{
			StringManager sm = Core.Data.stringManager;
			int passCount = 0;
			int Count = 2;
			if(dm.explorDoors.gems != null)
			{
			    passCount = dm.explorDoors.gems.passCount;
			    Count = dm.explorDoors.gems.count;
			}
			if(passCount == Count)
			{
				//Lab_Gem.text =  sm.getString(9084)+":"+passCount.ToString()+"/"+Count.ToString();
				SetLabGemProgress(passCount,Count,dm.explorDoors.gems.needStone);
				isGemFBActive = false;
			}
			else
			{
				if(Lab_Gem_NeedStone.gameObject.activeSelf)Lab_Gem_NeedStone.gameObject.SetActive(false);
				if(!Lab_Gem.gameObject.activeSelf) Lab_Gem.gameObject.SetActive(true);
			    isGemFBActive = time == 0;
				if(time == 0)
				    Lab_Gem.text =  sm.getString(9084)+":"+passCount.ToString()+"/"+Count.ToString();
				else
					Lab_Gem.text =  sm.getString(9084)+":"+passCount.ToString()+"/"+Count.ToString()+"\n"+sm.getString(9085)+":"+DateHelper.getDateFormatFromLong(  time );
			}
		}
	}
	
	public void OnClose()
	{		
		 Destroy(gameObject);
		 JCPVEMainController.Instance.FreshExpOrGem = null;
		 _mInstance = null;
	}
	
	void OnDestroy()
	{
		 
	}
	
	void SetLabExpProgress(int passCount,int Count,int NeedStone)
	{
		if(passCount < Count)
		{
			if(Lab_Exp_NeedStone.gameObject.activeSelf)Lab_Exp_NeedStone.gameObject.SetActive(false);
			if(!Lab_Exp.gameObject.activeSelf) Lab_Exp.gameObject.SetActive(true);
			Lab_Exp.text = passCount.ToString()+"/"+Count.ToString();
		}
		else
		{
			if(!Lab_Exp_NeedStone.gameObject.activeSelf)Lab_Exp_NeedStone.gameObject.SetActive(true);
			if(Lab_Exp.gameObject.activeSelf) Lab_Exp.gameObject.SetActive(false);
			if(NeedStone > 0)
				Lab_Exp_NeedStone.text = NeedStone.ToString();
			else
			{
				if(Lab_Exp_NeedStone.gameObject.activeSelf)Lab_Exp_NeedStone.gameObject.SetActive(false);
				if(!Lab_Exp.gameObject.activeSelf) Lab_Exp.gameObject.SetActive(true);
				Lab_Exp.text = Core.Data.stringManager.getString(9121);
			}
		}
	}

	void SetLabGemProgress(int passCount,int Count,int NeedStone)
	{
		if(passCount < Count)
		{
			if(Lab_Gem_NeedStone.gameObject.activeSelf)Lab_Gem_NeedStone.gameObject.SetActive(false);
			if(!Lab_Gem.gameObject.activeSelf) Lab_Gem.gameObject.SetActive(true);
			Lab_Gem.text = passCount.ToString()+"/"+Count.ToString();
		}
		else
		{
			if(!Lab_Gem_NeedStone.gameObject.activeSelf)Lab_Gem_NeedStone.gameObject.SetActive(true);
			if(Lab_Gem.gameObject.activeSelf) Lab_Gem.gameObject.SetActive(false);
			if(NeedStone > 0)
				Lab_Gem_NeedStone.text = NeedStone.ToString();
			else
			{
				if(Lab_Gem_NeedStone.gameObject.activeSelf)Lab_Gem_NeedStone.gameObject.SetActive(false);
				if(!Lab_Gem.gameObject.activeSelf) Lab_Gem.gameObject.SetActive(true);
				Lab_Gem.text = Core.Data.stringManager.getString(9121);
			}
		}
	}



	//是否激活经验副本
	bool _isExpFBActive;
	bool isExpFBActive
	{
		set
		{
			_isExpFBActive = value;
			Spr_Exp.color = value ? new Color(1f,1f,1f,1f) : new Color(0,0,0,1f);
			if(value)
			{
				StringManager sm = Core.Data.stringManager;
				if(dm.explorDoors.exp != null)
				Lab_Exp.text = sm.getString(9084)+":"+dm.explorDoors.exp.passCount.ToString()+"/"+dm.explorDoors.exp.count.ToString();
				Spr_Exp.transform.parent.GetComponent<UIButtonScale>().tweenTarget = Spr_Exp.transform;
			}
			else
				Spr_Exp.transform.parent.GetComponent<UIButtonScale>().tweenTarget = Lab_Exp.transform;
		}
		get
		{
			return _isExpFBActive;
		}
	}
	
	//是否激活宝石副本
	bool _isGemFBActive;
	bool isGemFBActive
	{
		set
		{
			_isGemFBActive = value;
			Spr_Gem.color = value ? new Color(1f,1f,1f,1f) : new Color(0,0,0,1f);
			if(value)
			{
				Spr_Gem.transform.parent.GetComponent<UIButtonScale>().tweenTarget = Spr_Gem.transform;
			}
			else
				Spr_Gem.transform.parent.GetComponent<UIButtonScale>().tweenTarget = Lab_Gem.transform;
		}
		get
		{
			return _isGemFBActive;
		}
	}
	
	//每个子界面关闭后的回调
	void CallBackBtnReturn(string btnName)
	{
		gameObject.SetActive(true);
		main.gameObject.SetActive(true);
	}
	
	public static void SafeDelete()
	{
		if(mInstance != null)
			mInstance.OnClose();
	}
	
}
