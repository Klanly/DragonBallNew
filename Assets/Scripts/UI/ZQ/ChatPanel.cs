using UnityEngine;
using System.Collections;

public class ChatPanel : MonoBehaviour 
{
	public UIInput m_input;
	public ChatLabel m_txtChat;

	private EmotionUI m_emotionUI;

	private static ChatPanel _instance;
	public static ChatPanel mInstance
	{
		get
		{
			return _instance;
		}
	}

	void Awake()
	{
		_instance = this;
	}

	void OnSend()
	{
		m_txtChat.SetText(m_input.label.text);
	}

	void OnEmotion()
	{
		if(m_emotionUI == null)
		{
			m_emotionUI = EmotionUI.OpenUI();
		}
		else 
		{
			m_emotionUI.SetShow(!m_emotionUI.IsShow);
		}
	}

	public void AppendEmotion(string strEmotion)
	{
		m_input.value += strEmotion;
	}
}
