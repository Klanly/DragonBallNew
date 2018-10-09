using UnityEngine;
using System.Collections;

public class ConfigLoading : MonoBehaviour {

	public UILabel Lab_progress;
	public UILabel Lab_describe;
	public UISlider slider;
	public System.Action onFinished;
//	string describe;
	void Start () 
	{
	
	}
	
	public void SetDescribe(string des)
	{
		if(!gameObject.activeSelf)gameObject.SetActive(true);
		Lab_describe.text = des;
//		describe = des;
	}
	
	public void ShowLoading(float progress)
	{
		
		Lab_progress.text =((int)(progress*100)).ToString()+"%";
		if(progress>=0.03f) 
		{
			slider.value = progress;
			if(progress>=0.97f)
				slider.value = 0.97f;
		}
		
		if(progress >= 1f)
		{
			if(onFinished!=null)onFinished();
		}
	}
	
	
}
