using UnityEngine;
using System.Collections;

public class JCPromptBox : MonoBehaviour {

	public UILabel Lab_title;
	public UILabel Lab_DesContent;
	private static JCPromptBox _mInstance;
	public System.Action OnBtnBuyClick;

	void Start () {
	
	}
	
	public static JCPromptBox OpenUI(string title, string Content)
	{
		if(_mInstance == null)
		{
			Object prefab = PrefabLoader.loadFromPack("JC/JCPromptBox");
			if(prefab != null)
			{
				GameObject obj = Instantiate(prefab) as GameObject;
				RED.AddChild(obj, DBUIController.mDBUIInstance._TopRoot);
				_mInstance = obj.GetComponent<JCPromptBox>();
				_mInstance.Lab_title.SafeText(title);
				_mInstance.Lab_DesContent.SafeText(Content);
			}
		}
		return _mInstance;
	}

    public static void Close()
	{
		if(_mInstance != null)
		{
			Destroy(_mInstance.gameObject);
			_mInstance = null;
		}
	}

	void OnClick_Close()
	{
		Close();
	}


	void OnClick_BtnBuy()
	{
		if(OnBtnBuyClick != null)
			OnBtnBuyClick();
	}
}
