using UnityEngine;
using System.Collections;

public class EmotionUI : MonoBehaviour 
{
	private bool m_bshow = true;
	public UIGrid m_grid;
	public UIAtlas m_emotionAtlas;

	public static EmotionUI OpenUI()
	{
		Object prefab = PrefabLoader.loadFromPack("ZQ/EmotionUI");
		if(prefab != null)
		{
			GameObject obj = Instantiate(prefab) as GameObject;
			RED.AddChild(obj, UIMessageMain.Instance.gameObject);
			EmotionUI emotion = obj.GetComponent<EmotionUI>();
			obj.transform.localPosition = new Vector3(62f,0f,0f);
			return emotion;
		}
		return null;
	}

	public void SetShow(bool bShow)
	{
		m_bshow = bShow;
		RED.SetActive(bShow, this.gameObject);
	}
		
	public bool IsShow
	{
		get
		{
			return m_bshow;
		}
	}


	void Start()
	{
		InitUI();
	}

	void InitUI()
	{
		for(int i = 0; i < m_emotionAtlas.spriteList.Count; i++)
		{
			string strName = m_emotionAtlas.spriteList[i].name;
			if(strName.Contains("-01"))
			{
				EmotionItem.CreateEmotion(m_grid.gameObject, strName, SelEmotionCallback);
			}
		}
		SetShow(true);
	}

	void SelEmotionCallback(string strEmotion)
	{
		UIMessageMain.Instance.AppendEmotion(strEmotion);
	}
}
