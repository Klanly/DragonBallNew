using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CRLuo_RedD_AlphaAnim : MonoBehaviour {

	public GameObject myObj;
	public float DefaultAlpha;
	public CRLuo_RedD_AlphaAnim_Element[] AlphaElement;
	Color myColor;
	// Use this for initialization
	void Start () {
		myColor = myObj.renderer.material.color;
		myObj.renderer.material.color = new Color(myColor.r, myColor.g, myColor.b, DefaultAlpha);
		StartCoroutine("PlayAlphaAnim");
	}

	public void ReplayAlphaAnim()
	{
		Start();
	}

	IEnumerator PlayAlphaAnim()
	{
		for (int i = 0; i < AlphaElement.Length; i++)
		{
			if (AlphaElement[i].OnOff)
			{
				yield return new WaitForSeconds(AlphaElement[i].StarTime);
				MiniItween.ColorTo(myObj, new V4(myColor.r, myColor.g, myColor.b, AlphaElement[i].GoAlpha), AlphaElement[i].LongTime);
			}
		}
	}
}

[System.Serializable]
public class CRLuo_RedD_AlphaAnim_Element
{
	public bool OnOff;
	public float GoAlpha;
	public float StarTime;
	public float LongTime;
}
