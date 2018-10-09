using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;

public class UIVersinUpgrade : RUIMonoBehaviour 
{
	static UIVersinUpgrade mInstance;
	public UIVersinUpgrade GetInstance()
	{
		return mInstance;
	}

	public UILabel m_Content;
	public GameObject root;
	private float m_StrLength;

	private string m_Url;

	readonly int MAXWIDTH = 630;
	readonly int MAXHEIGHT = 42;
	readonly int OFFSET = 10;
	readonly string COLOR = "[00FF00]";

	private string m_TotalStr = "";

	static public void OpenUI()
	{
		if(mInstance == null)
		{
			Object prefab = PrefabLoader.loadFromPack ("LS/pbLSVersionUpgrade");
			if (prefab != null)
			{
				GameObject obj = Instantiate (prefab) as GameObject;
				mInstance = obj.GetComponent<UIVersinUpgrade> ();
			}
		}
	}

	void Awake()
	{
		mInstance = this;
	}

	void Start()
	{
		m_Content.SafeText("");
		m_StrLength = (float)m_Content.width;
		m_Url = MessageMgr.GetInstance().Upgrape;
		SetContentDetail(Core.Data.stringManager.getString(25171));
	}

	void SetContentDetail(string _str)
	{
		List<Vector3> boxlist = new List<Vector3>();
		List<Vector3> boxposlist = new List<Vector3>();
		List<string> newstrlist = new List<string>();
		string _strtemp = Core.Data.stringManager.getString(25170);
		m_Content.text = " ";
		float space = m_Content.width;

		string[] _strArry = _strtemp.Split('{');
		for(int i=0; i< _strArry.Length; i++)
		{
			if(_strArry[i].Contains("}"))
			{
				int index = _strArry[i].IndexOf("}");
				string _substr = _strArry[i].Substring(0,index+1);
				_substr = _substr.Replace(_substr, _str);
				int _lastline = m_Content.width / MAXWIDTH;
				int _lastposx = m_Content.width % MAXWIDTH;
				int _lastposy = MAXHEIGHT * _lastline;

				int _split = 0;
				for(int z=0; z<_substr.Length; z++)
				{
					m_Content.text += "   ";
					if(_split == 0)
					{
						if(m_Content.width / MAXWIDTH != _lastline)
						{
							_split = z;
						}
					}
				}
				if(_split == 0)
				{
					newstrlist.Add(_substr);
				}
				else
				{
					newstrlist.Add(_substr.Substring(0, _split+1));
					newstrlist.Add(_substr.Substring(_split+1, _substr.Length - _split -1));
				}


				int _line = m_Content.width / MAXWIDTH;
				int _posx = m_Content.width % MAXWIDTH;
				int _posy = MAXHEIGHT * _line;

				if(_lastline == _line)
				{
					boxlist.Add(new Vector3(MAXWIDTH - _lastposx, MAXHEIGHT, 0));
					boxposlist.Add(new Vector3(_lastposx, -(_lastposy+MAXHEIGHT/2), 0));
				}
				else if(_line - _lastline == 1)
				{
					boxlist.Add(new Vector3(MAXWIDTH - _lastposx, MAXHEIGHT, 0));
					boxlist.Add(new Vector3(_posx, MAXHEIGHT, 0));
					boxposlist.Add(new Vector3(_lastposx, -(_lastposy+MAXHEIGHT/2), 0));
					boxposlist.Add(new Vector3(0f, -(_posy+MAXHEIGHT/2), 0));
				}
				else if(_line - _lastline > 1)
				{
					boxlist.Add(new Vector3(MAXWIDTH - _lastposx, MAXHEIGHT, 0));
					boxlist.Add(new Vector3(_posx, MAXHEIGHT, 0));
                }

				m_Content.text += _strArry[i].Substring(index+1);
            }
			else
			{
				m_Content.text += _strArry[i];
			}
		}
		SwapLabel();
		AddBox(boxlist, boxposlist, newstrlist);
	}

	void SwapLabel()
	{
		string _str = m_Content.text;
		List<string> _totalstr = new List<string>();
		int _maxline = m_Content.width/MAXWIDTH;
		m_Content.text = "";
		int _line = 0;
		if(_maxline > 0)
		{
			for(int i=0; i<_str.Length; i++)
			{
				m_Content.text += _str[i];
				if(m_Content.width >= MAXWIDTH)
				{
					m_Content.text += "\n";
					_totalstr.Add(m_Content.text);
					m_Content.text = "";
					_line ++;
					if(_line == _maxline)
					{
						_totalstr.Add(_str.Substring(i+1, _str.Length - i -1));
					}
				}
			}
		}
		else
		{
			_totalstr.Add(_str);
		}
		m_Content.text = "";
		foreach(string str in _totalstr)
		{
			m_Content.text += str;
		}
	}

	void AddBox(List<Vector3> boxlist, List<Vector3> boxposlist, List<string> newstrlist)
	{
		for(int i=0; i<boxlist.Count; i++)
		{
			Object prefab = PrefabLoader.loadFromPack ("LS/Urlobj");
			if (prefab != null)
			{
				GameObject obj = Instantiate (prefab) as GameObject;
				RED.AddChild (obj, root);

			    UILabel label = obj.GetComponentInChildren<UILabel>();
				label.text = "[00FF00]" + newstrlist[i];
				obj.transform.localPosition = boxposlist[i];

				obj.AddComponent<UIButton>().tweenTarget = label.gameObject;

				BoxCollider box =  obj.gameObject.AddComponent<BoxCollider>();
				box.size = new Vector3((float)label.width, (float)label.height, 0f);
				box.center = new Vector3((float)label.width/2, 0f,0f);

				UIButtonMessage message = obj.gameObject.AddComponent<UIButtonMessage>();
				message.target = this.gameObject;
				message.functionName = "OnClick";
				message.trigger = UIButtonMessage.Trigger.OnClick;
			}
		}

	}

	public void CloseClick()
	{
		this.dealloc();
	}

	public void OnClick()
	{
		Application.OpenURL(m_Url);
		Application.Quit();
	}

	void OnDestroy()
	{
		mInstance = null;
	}

}
