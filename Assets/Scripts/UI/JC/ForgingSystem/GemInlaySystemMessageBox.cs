using UnityEngine;
using System.Collections;

public class GemInlaySystemMessageBox : MonoBehaviour {

	public System.Action<GameObject> ButtonClick = null;
	void Start () {
	
	}
	
	public void GemInlaySystemMessageBoxClick(GameObject btn)
	{
		if(btn!=null)
			ButtonClick(btn);
	}
	
	public void Close()
	{
		gameObject.SetActive(false);
	}
	
	public void Open()
	{
		gameObject.SetActive(true);
	}
}
