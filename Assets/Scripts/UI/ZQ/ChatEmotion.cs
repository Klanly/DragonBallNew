using UnityEngine;
using System.Collections;

//聊天表情
public class ChatEmotion : MonoBehaviour 
{
	private UISpriteAnimation m_anim;	//动画
	private UISprite m_spEmotion;
	private string m_strText;
	private float m_emotionSize = 40;

	void Start()
	{
		m_anim = GetComponent<UISpriteAnimation> ();
		m_spEmotion = GetComponent<UISprite>();
		SetEmotionText();
	}
	
	public int width
	{
		get
		{
			return m_spEmotion.width;
		}
	}


	int GetFrameCount()
	{
		UIAtlas atlas = m_spEmotion.atlas;
		int count = 0;
		int index = m_spEmotion.spriteName.IndexOf('-');
		string subName = m_spEmotion.spriteName.Substring(0, index + 1); 
		for(int i = 0; i < atlas.spriteList.Count; i++)
		{
			if(atlas.spriteList[i].name.Contains(subName))
			{
				count++;
			}
		}
		return count;
	}

	public void SetEmotionText()
	{
		m_spEmotion.spriteName = m_strText;

		float time = m_spEmotion.width / m_spEmotion.height;
		m_spEmotion.width = (int)m_emotionSize;
		m_spEmotion.height = (int)(m_spEmotion.width * time);

		if(m_strText.Contains("-"))
		{
			m_anim.enabled = true;
			int index = m_strText.IndexOf('-');
			m_anim.namePrefix = m_strText.Substring(0, index + 1); 
			m_anim.framesPerSecond = GetFrameCount();
		}
		else
		{
			m_anim.enabled = false;
		}
	}

	private void InitEmotionText(string strText, float scale)
	{
		m_strText = strText;
		m_emotionSize = scale;
	}

	public static ChatEmotion CreateChatEmotion(GameObject parent, Vector3 localPos, string strText, float scale)
	{
		Object prefab = PrefabLoader.loadFromPack("ZQ/ChatEmotion");
		if(prefab != null)
		{
			GameObject obj = Instantiate(prefab) as GameObject;
			RED.AddChild(obj, parent);
			obj.transform.localPosition = localPos;
			ChatEmotion emotion = obj.GetComponent<ChatEmotion>();
			emotion.InitEmotionText(strText, scale);
			return emotion;
		}
		return null;
	}

}
