using UnityEngine;
using System.Collections;

public class FragmentLighting : MonoBehaviour {
	public bool Key {get;set;}
	public GameObject mLighting;
	
	// Use this for initialization
	void Start ()
	{
		Key = false;
//		mLighting = gameObject.transform.FindChild()

	}

	void Update () 
	{
		if (Key) {
			if (Core.Data.soulManager.IsHaveFullFrag ()) {
				Key = false;
				if (mLighting != null)
					mLighting.SetActive (true);
			} else {

				if (mLighting != null)
					mLighting.SetActive (false);
			}
			Key = false;
		}

	}
}
