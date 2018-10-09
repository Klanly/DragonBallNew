using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SelUserHeadUI : MonoBehaviour 
{
	//public GameObject ScrollBar;
	static SelUserHeadUI _this;	
	public UIScrollView _scrollView;
	
	bool OpenOrClose = false;
	
	public List<UIButton> Btn_Backs = new List<UIButton>();
	
	public static SelUserHeadUI OpenUI()
	{
		if(_this == null)
		{
			Object prefab = PrefabLoader.loadFromPack ("ZQ/SelUserHeadUI");		
			if (prefab != null)
			{
				GameObject obj = Instantiate (prefab) as GameObject;
				RED.AddChild (obj, UIOptionController.Instance.gameObject);
				_this = obj.GetComponent<SelUserHeadUI> ();
				//RED.TweenShowDialog(obj);
			}
		}
		else
		{
			_this.gameObject.SetActive(true);
		}
		return _this;
	}

	public UILabel m_txtTitle;		//标题
	public UIButton m_btnOK;
	public UITable m_grid;

	[HideInInspector]
	public UserHeadCell m_selHead;
	
	List<UserHeadCell> list_headcell = new List<UserHeadCell>();
	Dictionary<PlayerHeadType,Dictionary<int,UserHeadData>>  HeadData = null;
	public TweenScale _tweenScale;
	
	void Start()
	{
		//m_btnOK.TextID = 5030;	
		m_txtTitle.text = Core.Data.stringManager.getString(5079);		
		_tweenScale.onFinished.Add(new EventDelegate(OnAnimationFinished));
	}
	
	
	void OnAnimationFinished()
	{
		OpenOrClose = !OpenOrClose;
		if(OpenOrClose)
		   CreateElement();
		else
		   Destroy(gameObject);
	}
	
	
	//创建对象池
	void CreateElement()
	{	
		HeadData = Core.Data.playerManager.HeadConfigData;
		int MaxCount = Mathf.Max(HeadData[PlayerHeadType.Vip ].Count,HeadData[PlayerHeadType.General ].Count,HeadData[PlayerHeadType.Reward ].Count );
		Object prefab = PrefabLoader.loadFromPack ("ZQ/UserHeadCell");		
		for (int i = 0; i < MaxCount; i++)
		{
			GameObject obj = Instantiate (prefab) as GameObject;
			RED.AddChild (obj, m_grid.gameObject);
			obj.name = (10 + i).ToString ();
			UserHeadCell cell = obj.GetComponent<UserHeadCell> ();
			list_headcell.Add(cell);
		}
		m_grid.Reposition ();

		_scrollView.ResetPosition();
		OnBtnClick("0");
	}
	

	public void OnClickExit()
	{
		_tweenScale.PlayReverse();
		//Destroy (this.gameObject);
	}

	void ErrorInformation()
	{
		if(m_selHead.Headdata.type == 1)
		{
			SQYAlertViewMove.CreateAlertViewMove(string.Format(Core.Data.stringManager.getString(20061), m_selHead.Headdata.VIPLv));
		}
		else if(m_selHead.Headdata.type == 2)
		{
			SQYAlertViewMove.CreateAlertViewMove(string.Format(Core.Data.stringManager.getString(20062), m_selHead.Headdata.Batter));
		}
		else if(m_selHead.Headdata.type == 3)
		{
			SQYAlertViewMove.CreateAlertViewMove(string.Format(Core.Data.stringManager.getString(25131), m_selHead.Headdata.Gamble));
        }
    }

	void OnClickOK()
	{
		if(m_selHead == null)
		{
			OnClickExit ();
			return ;
		}
		if(!m_selHead.IsChoose)
		{
			ErrorInformation();
			return;
		}
		if(m_selHead != null)
		{
			UIOptionController.Instance.SetSelHead (m_selHead.Headdata.id);
		}
		OnClickExit ();
	}
	
	
	private int prevIndex = -1;
	UIButton OldClickBtn = null;
	void OnBtnClick(string btnName)
	{
		int Btn_index = System.Convert.ToInt32( btnName.Substring(btnName.Length-1) );
		if(prevIndex == Btn_index) {
			return;
		} else {
			prevIndex = Btn_index;
		}

		if(Btn_index>=0 && Btn_index< Btn_Backs.Count) {
		    Btn_Backs[Btn_index].normalSprite = "Symbol 31";
			if(OldClickBtn != null)
				OldClickBtn.normalSprite = "Symbol 32";
			OldClickBtn = Btn_Backs[Btn_index];
		}
		PlayerHeadType headtype =(PlayerHeadType) System.Convert.ToInt32( btnName.Substring(btnName.Length-1) );
		
		Dictionary<int,UserHeadData> head_list = HeadData[headtype];
		int index = -1;
		foreach(UserHeadData data in head_list.Values)
		{
			index++;
			if(index<list_headcell.Count)
			{
				if(!list_headcell[index].gameObject.activeSelf)list_headcell[index].gameObject.SetActive(true);
				list_headcell[index].InitUI(data);
			}
		}
		index++;
		for(;index<list_headcell.Count;index++)
		{
			list_headcell[index].gameObject.SetActive(false);
		}

		if(list_headcell.Count > 0)
		{
			m_selHead = list_headcell[0];
			m_selHead.m_objSel.alpha = (float)System.Convert.ToUInt32(true);
		}
			
		m_grid.Reposition ();
		_scrollView.ResetPosition();
	}

	void OnBtnClick(GameObject btn)
	{
		OnBtnClick(btn.name);
	}

}
