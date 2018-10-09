using UnityEngine;
using System.Collections;

public class SQYSkillView : MonoBehaviour {

//	// Use this for initialization
//	void Start () {
//	
//	}
//	
	public static SQYSkillView CreateSkillView()
	{
		Object obj = PrefabLoader.loadFromPack("SQY/pbSQYSkillView",false);
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			SQYSkillView sv = go.GetComponent<SQYSkillView>();
			return sv;
		}
		
		return null;
	}
}
