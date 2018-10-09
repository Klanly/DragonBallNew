using UnityEngine;
using System.Collections;

public class EmotionItem : MonoBehaviour 
{
	public UISprite m_spEmotion;
	private string m_txtEmotion;


	public delegate void SelEmotionCallback(string strEmotion);
	private SelEmotionCallback m_onClick;

	public static EmotionItem CreateEmotion(GameObject parent, string strText, SelEmotionCallback callback)
	{
		Object prefab = PrefabLoader.loadFromPack("ZQ/EmotionItem");
		if(prefab != null)
		{
			GameObject obj = Instantiate(prefab) as GameObject;
			RED.AddChild(obj, parent);
			EmotionItem emotion = obj.GetComponent<EmotionItem>();
			emotion.m_txtEmotion = strText;
			emotion.m_onClick = callback;
			return emotion;
		}
		return null;
	}

	void Start()
	{
		m_spEmotion.spriteName = m_txtEmotion;
	}


	void OnClick()
	{
		string strText = "<" + m_txtEmotion + ">";
		m_onClick(strText);
	}
}
