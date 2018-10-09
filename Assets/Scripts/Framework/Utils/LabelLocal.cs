using UnityEngine;
using System.Collections;

public class LabelLocal : MonoBehaviour {

	public int stringId;
	// Use this for initialization
	void Start () {
		UILabel label = gameObject.GetComponent<UILabel>();
		if(label != null)
		{
			label.text = Core.Data.stringManager.getString(stringId);
		}
	}
}
