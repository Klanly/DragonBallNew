using UnityEngine;
using System.Collections;

public class DelegateTestIfDestory : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AlwaysRun.instance.register( () => { doTest(); } );

        Destroy(this.gameObject);
	}

    void doTest() {
        Debug.Log("Show -- Even if this gameobject is destoryed.");
        GameObject go = new GameObject("test");
        go.transform.parent = Camera.main.transform;
    }

}
