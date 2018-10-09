using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class ChatLabel : MonoBehaviour 
{
	private UILabel m_labContent;
	private string m_txtContent;
	private List<ChatEmotion> m_listEmotionl = new List<ChatEmotion>();
	
	public int m_lineWidth = 200;
	public float m_nEmotionSize = 30f;
	public Vector2 m_offset = Vector2.zero;

	public UIAtlas m_emotionAtlas;

	void Awake()
	{
		m_labContent = GetComponent<UILabel>();
		m_labContent.overflowMethod = UILabel.Overflow.ResizeFreely;
		m_labContent.pivot = UIWidget.Pivot.TopLeft;
	}

	public void SetText(string strText)
	{
		if(m_labContent == null)
		{
			Awake();
		}
		m_labContent.text = "";
		for(int i = 0; i < m_listEmotionl.Count; i++)
		{
			Destroy(m_listEmotionl[i].gameObject);
		}
		m_listEmotionl.Clear();

		m_txtContent = strText;
		ProcessEmotionText ();
	}

	public int height
	{
		get
		{
			if(m_labContent == null)
			{
				m_labContent = GetComponent<UILabel>();
			}
			return m_labContent.height;
		}
	}

	bool CheckEmotion(string m_EmotionStr)
	{
		UIAtlas atlas = m_emotionAtlas;
		for(int i = 0; i < atlas.spriteList.Count; i++)
		{
			if(atlas.spriteList[i].name == m_EmotionStr)
			{
				return true;
			}
		}
		return false;
	}
		
	void ProcessEmotionText()
	{
		string strText = m_txtContent; 

		if (string.IsNullOrEmpty (m_txtContent))
		{
			m_labContent.text = m_txtContent;
			SwapText();
			return;
		}

		if (!strText.Contains ("<") || !strText.Contains (">"))
		{
			m_labContent.text = m_txtContent;
			SwapText();
			return; 
		}

		m_labContent.text = " ";
		float spaceWidth = m_labContent.width;
		int spaceCnt = (int)(m_nEmotionSize / spaceWidth) + 1;
		List<int> list = new List<int>();
		int m_Index = 0;
		foreach(char c in strText)
		{
			if(c == '>')
			{
				list.Add(m_Index);
			}
			m_Index++;
		}

		m_labContent.text = "";
		string[] strArry = strText.Split ('>');
		for (int i = 0; i < strArry.Length; i++)
		{
			int startIndex = strArry [i].IndexOf ('<');
			if(startIndex < 0)
			{
				m_labContent.text += strArry[i];
				continue;
			}

			string strEmotion = strArry[i].Substring(startIndex, strArry[i].Length - startIndex);
			strArry [i] = strArry [i].Replace(strEmotion, "");
			m_labContent.text += strArry[i];

			strEmotion = strEmotion.Replace("<", "");

			int lineCount = m_labContent.width / m_lineWidth;

			Vector3 pos = Vector3.zero;
			if(lineCount > 0)
			{
				pos.x = m_labContent.width % m_lineWidth;
				pos.y = m_labContent.width / m_lineWidth * (m_labContent.fontSize + m_labContent.spacingY) * -1;
				pos.y -= m_labContent.fontSize - m_nEmotionSize;
				pos.y -= m_offset.y;
			}
			else
			{
				pos.x = m_labContent.width;
				pos.y -= m_labContent.fontSize - m_nEmotionSize;
				pos.y -= m_offset.y;
			}
	
			if(!CheckEmotion(strEmotion))
			{
				m_labContent.text += "<" + strEmotion + ">";
			}
			else
			{
				ChatEmotion emotion = ChatEmotion.CreateChatEmotion(this.gameObject, pos, strEmotion, m_nEmotionSize);

				m_listEmotionl.Add(emotion);
				
				for(int n = 0; n < spaceCnt; n++)
				{
					m_labContent.text += " ";
				}
			}
		}

		SwapText();
	}

	void SwapText()
	{
		string strTemp = m_labContent.text;
		int lineCount = 0;
		List<string> listString = new List<string>();
		if(m_lineWidth > 0)
		{
			lineCount = m_labContent.width / m_lineWidth;
			if(lineCount > 0)
			{
				int curLine = 0;
				m_labContent.text = "";
				for(int i = 0; i < strTemp.Length; i++)
				{
					m_labContent.text += strTemp[i];
					if(m_labContent.width > m_lineWidth)
					{
						string strSubString = m_labContent.text + "\n";
						listString.Add(strSubString);
						m_labContent.text = "";
						curLine++;
						
						if(curLine == lineCount)
						{
							listString.Add(strTemp.Substring(i + 1, strTemp.Length - i - 1));
							break;
						}
					}
				}
			}
			else
			{
				listString.Add(strTemp);
			}
		}

		m_labContent.text = "";
		for(int i = 0; i < listString.Count; i++)
		{
			m_labContent.text += listString[i];
		}
	}
}
