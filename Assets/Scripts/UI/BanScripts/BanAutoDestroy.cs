using UnityEngine;
using System.Collections;

public class BanAutoDestroy : MonoBehaviour {

	public float time = 1;
	
	void Start () {
		Destroy(this.gameObject,time);
	}

}
