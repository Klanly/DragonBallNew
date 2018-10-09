using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AutoAdapterFullScene : MonoBehaviour {

	
	public enum AdapterType
	{
		Bottom,
	}
	
	public AdapterType type = AdapterType.Bottom;
	
	
	public List<GameObject> List_AdapterObject = new List<GameObject>();
	
	void Awake ()
	{
		float ratio = (float)Screen.width/(float)Screen.height;
		float y = 0;
		if( Mathf.Abs( ratio - (16f/9f) ) > 0.001f )
		{
			float h = 1136f /(float) Screen.width * (float) Screen.height;
		     y = Mathf.Abs((640f - h))/2;
			
			foreach(GameObject g in List_AdapterObject)
			{
				Vector3 pos = g.transform.localPosition;
				if(type == AdapterType.Bottom)
				{
					pos.y -= y;
		            g.transform.localPosition = pos;
				}
			}
		}		
	}
	
}
