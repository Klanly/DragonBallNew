using UnityEngine;
using System.Collections;

public class ChAnimUI5 : MonoBehaviour {

	public TweenScale Animation_Scale;
	public  System.Action OnFinished;

	void Start ()
	{
		Animation_Scale.PlayForward();
		Invoke("ScaleFinished",Animation_Scale.duration);
	}

	void ScaleFinished()
	{
		if(OnFinished != null)
			Invoke("onFinished",1f);
		else
		{
			Destroy(gameObject);
		}
	}

	void onFinished()
	{
		if(OnFinished != null)
		{
			OnFinished();
			Destroy(gameObject);
		}
	}

	public static ChAnimUI5 OpenUI(GameObject objParent)
	{
		Object prefab = PrefabLoader.loadFromPack ("JC/ChAnimUI5");
		ChAnimUI5 anim = null;
		if (prefab != null) 
		{
			GameObject obj = Instantiate (prefab) as GameObject;
			if (obj != null) 
			{
				RED.AddChild (obj, objParent);
			}
			anim = obj.GetComponent<ChAnimUI5>();
			
		}
		return anim;
	}
}
