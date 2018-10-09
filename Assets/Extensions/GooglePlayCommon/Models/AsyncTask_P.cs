using UnityEngine;
using UnionAssets.FLE;
using System.Collections;

public class AsyncTask_P : EventDispatcher {


	void Awake() {
		DontDestroyOnLoad(gameObject);
	}

}

