using UnityEngine;
using System.Collections;

public class UIGuidanceMessageDialogue : MonoBehaviour
{
	public UILabel messageLabel;

	private static UIGuidanceMessageDialogue instance = null;

	static public UIGuidanceMessageDialogue Instance
	{
		get
		{
			if (instance == null)
			{
				instance = UnityEngine.Object.FindObjectOfType(typeof(UIGuidanceMessageDialogue)) as UIGuidanceMessageDialogue;

				if (instance == null)
				{
					GameObject prb = PrefabLoader.loadFromPack("WHY/pbGuidanceMessageDialogue") as GameObject;
					GameObject prbObj = GameObject.Instantiate(prb) as GameObject;


					instance = prbObj.GetComponent<UIGuidanceMessageDialogue>();
				}
			}
			return instance;
		}
	}

	public void setRoot(GameObject root)
	{
		gameObject.transform.parent = root.transform;
		gameObject.transform.localPosition = Vector3.back * 10;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
