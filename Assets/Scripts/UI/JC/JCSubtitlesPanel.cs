using UnityEngine;
using System.Collections;

public class JCSubtitlesPanel : MonoBehaviour {

	
	string Content;
	int ContentLength = 0;
	int index = 0;
	public UILabel LabSubtitles;
	public System.Action OnClose;
	
	public static JCSubtitlesPanel _instance;
	
	void Start ()
	{
		Content = Core.Data.stringManager.getString(9112);
		//这将是你在今后的冒险中，首个对你造成严峻威胁的敌人“比克大魔王”！让我们开始冒险之旅吧！
		ContentLength = Content.Length;
		if(Content.Length > 0)
	    InvokeRepeating("RepeatingSubtitles",0.2f,0.2f);
	}
	
	void RepeatingSubtitles()
	{
		index++;
		if(index > ContentLength)
		{
		    CancelInvoke("RepeatingSubtitles");
			Invoke("DestroyUI",1f);
		}
		else
		   LabSubtitles.text = Content.Substring(0,index);
	}
	
	
	void DestroyUI()
	{
		if(OnClose != null)
				OnClose();
		Destroy(gameObject);			
	}
	
	
	static JCSubtitlesPanel _this;
	public static JCSubtitlesPanel OpenUI()
	{	
		Object prefab = PrefabLoader.loadFromPack("JC/JCSubtitlesPanel");
		if(prefab != null)
		{
			GameObject obj = Instantiate(prefab) as GameObject;
			RED.AddChild(obj, DBUIController.mDBUIInstance._bottomRoot);
			_this = obj.GetComponent<JCSubtitlesPanel>();
		}
		return _this;
	}
	
	
	
	
	
	
}
