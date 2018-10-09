using UnityEngine;
using System.Collections;

public class GetGemSucUI : MonoBehaviour 
{
	public UILabel m_txtTitle;
	public UILabel m_txtName;
	public UILabel m_txtLevel;
	public UIButton m_btnOK;
    public UISprite m_gemSprite;
	
	public TweenScale m_scale;
	public GameObject Stars;
	
    private GemData m_data;
	private string m_strTitle;

	private static GetGemSucUI _mInstance;
    public static void OpenUI(GemData data, string strTitle)
	{
		if(_mInstance == null)
		{
			Object  prefab = PrefabLoader.loadFromPack("JC/GetGemSucUI");
			if(prefab != null)
			{
				GameObject obj = Instantiate(prefab) as GameObject;
				RED.AddChild(obj, DBUIController.mDBUIInstance._TopRoot);
								RED.TweenShowDialog (obj);
				_mInstance = obj.GetComponent<GetGemSucUI>();
				_mInstance.m_data = data;
				_mInstance.m_strTitle = strTitle;
			}
		}
		else
		{
			RED.SetActive(true, _mInstance.gameObject);
			_mInstance.m_data = data;
			_mInstance.m_strTitle = strTitle;
			_mInstance.InitUI();
		}

	}

	void Awake()
	{
		_mInstance = this;
	}

	void Start()
	{
		InitUI();
	}

	public void InitUI()
	{
		SetShow (true);
        GemData data = m_data;
		m_txtTitle.text = m_strTitle;
       // Debug.Log(" idddddddddddd "+data.ID );
		m_txtName.text = data.name;
		Debug.Log("star:"+data.star.ToString());
		m_txtLevel.text="Lv."+data.level;
		ShowLevelStars((int)data.star);
		m_gemSprite.spriteName=data.anime2D;
		m_gemSprite.MakePixelPerfect();
		m_btnOK.TextID = 5030;
	}
		
	public void SetShow(bool bShow)
	{
		if (bShow)
		{
			RED.SetActive (true, gameObject);
			m_scale.from = Vector3.one * 0.1f;
			m_scale.to = Vector3.one;
			m_scale.duration = 0.4f;
			m_scale.onFinished.Clear ();
			m_scale.PlayForward ();
		}
		else
		{
			m_scale.from = Vector3.one ;
			m_scale.to = Vector3.one * 0.1f;
			m_scale.duration = 0.4f;
			m_scale.onFinished.Clear ();
			m_scale.onFinished.Add(new EventDelegate(this, "HideUI"));
			m_scale.PlayForward ();
		}
	}

	void HideUI()
	{
		Destroy(this.gameObject);
		_mInstance = null;
	}
	
	void OnMessageBoxClick()
	{
		HideUI();
	}
	
	void ShowLevelStars(int starlevel)
	{
		Transform[] stars=Stars.GetComponentsInChildren<Transform>();
		Debug.Log(stars.Length);
		int index=0;
		foreach(Transform s in stars)
		{
			if(s.name!=Stars.name)
			{
				index++;
				if(index<=starlevel)
					s.gameObject.SetActive(true);					
				else
					s.gameObject.SetActive(false);				
			}
		}
		Stars.transform.localPosition=new Vector3(64-16*(starlevel-1),-122f,0);
	}
	
}
