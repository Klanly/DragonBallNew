using UnityEngine;
using System.Collections;

public static class Extend {
	public static void dealloc(this RUIMonoBehaviour mb)
	{
		if(mb != null)
		{
			UnityEngine.Object.Destroy(mb.gameObject);
			UnityEngine.Object.Destroy(mb);
			mb = null;
		}
	}
}
