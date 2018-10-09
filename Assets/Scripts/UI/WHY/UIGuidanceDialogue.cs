using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIGuidanceDialogue : MonoBehaviour 
{
	#region 基本变量

	public UILabel continueLabel;

	public UISpriteAnimation eyeSpriteAnimation;
	public UISprite eyeSprite;

	public UILabel messageLabel;
	public List<string> messageList = new List<string>();
	int messageListIndex;
	string currentMessage;
	char[] currentMessageArray;
	int currentMessageArrayIndex;

	private static UIGuidanceDialogue instance = null;

	static public UIGuidanceDialogue Instance
	{
		get
		{
			if (instance == null)
			{
				instance = UnityEngine.Object.FindObjectOfType(typeof(UIGuidanceDialogue)) as UIGuidanceDialogue;

				if (instance == null)
				{
					GameObject prb = PrefabLoader.loadFromPack("WHY/pbGuidanceDialogue") as GameObject;
					GameObject prbObj = GameObject.Instantiate(prb) as GameObject;


					instance = prbObj.GetComponent<UIGuidanceDialogue>();
				}
			}
			return instance;
		}
	}
	#endregion


	#region 基本方法
	public void setRoot(GameObject root)
	{
		gameObject.transform.parent = root.transform;
		gameObject.transform.localPosition = Vector3.back * 10;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;

	}

	// Use this for initialization
	void Start ()
	{
		TweenScale ts = TweenScale.Begin(this.continueLabel.gameObject, 0.5f, new Vector3(1.1f, 1.1f, 1));
		ts.style = UITweener.Style.PingPong;

		this.eyeSpriteAnimation.AnimationEndDelegate = eyeSpriteAnimationEnd;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}


	IEnumerator eyeSpriteAnimationStart()
	{
		yield return new WaitForSeconds(1.0f);
		this.eyeSpriteAnimation.ResetToBeginning();
	}

	void eyeSpriteAnimationEnd(UISpriteAnimation spriteAnimation)
	{
		this.eyeSprite.spriteName = "eye-10090001";
		this.eyeSprite.MakePixelPerfect();
		StartCoroutine(eyeSpriteAnimationStart());
	}

	public void setMessageList(List<string> messageList)
	{
		this.messageList.Clear();
		this.messageList.AddRange(messageList);
		this.messageListIndex = 0;
	}

	public bool setCurrentMessageFromMessageList()
	{
		if(this.messageListIndex >= this.messageList.Count)
		{
			return false;
		}
		this.setMessage(this.messageList[this.messageListIndex]);
		this.messageListIndex++;
		return true;
	}

	public void setMessage(string message)
	{
		this.currentMessage = message;
		this.currentMessageArray = this.currentMessage.ToCharArray();
	}



	public void messageAnimationStart()
	{
		if(this.currentMessageArrayIndex >= this.currentMessageArray.Length)
		{
			return;
		}
		StartCoroutine(setMessageLabel(this.currentMessageArray[this.currentMessageArrayIndex]));
	}

	IEnumerator setMessageLabel(char c)
	{
		yield return new WaitForSeconds(0.01f);
		this.messageLabel.text += new string(c, 1);
		this.currentMessageArrayIndex++;
		if(this.currentMessageArrayIndex >= this.currentMessageArray.Length)
		{
			yield break;
		}
		StartCoroutine(setMessageLabel(this.currentMessageArray[this.currentMessageArrayIndex]));
	}

	void onMessageContinueClick()
	{

		if(!setCurrentMessageFromMessageList())
		{
			return;
		}
		StopAllCoroutines();
		this.currentMessageArrayIndex = 0;
		this.messageLabel.text = "";
		this.messageAnimationStart();
	}

	void test()
	{
		UIGuidanceDialogue.Instance.setRoot(DBUIController.mDBUIInstance._TopRoot);

		List<string> l = new List<string>();
		for(int i = 0; i < 10; i++)
		{
			l.Add("   " +i + "." + Core.Data.stringManager.getString(6108));
		}
		UIGuidanceDialogue.Instance.setMessageList(l);
		UIGuidanceDialogue.Instance.setCurrentMessageFromMessageList();
		UIGuidanceDialogue.Instance.messageAnimationStart();
	}
	#endregion
}
