using UnityEngine;
using System.Collections;

public class CRLuo_RandPlayAnim_Group : MonoBehaviour {
	public GameObject[] PlayOBJ;
	public string[] AnimName;
	public float RandMinTime=1f;
	public float RandMaxTime=5f;
	// Use this for initialization
	void Start () {
		if (AnimName == null) {
			Debug.Log(this.gameObject.name + "Not Anim !!!!");
			Destroy(this);
		}
		if (AnimName.Length > 1) {
			Invoke ("RandPlayGO", Random.Range (1f, 5f));
		}
	}
	void Update()
	{
		foreach (GameObject aOBJ in PlayOBJ) {
			if (!aOBJ.animation.isPlaying) {
				aOBJ.animation.CrossFade (AnimName [0]);
			}
		}
	}
	void RandPlayGO()
	{
		int I = Random.Range (1, AnimName.Length);

		foreach (GameObject aOBJ in PlayOBJ) {
			aOBJ.animation.CrossFade(AnimName[I]);
		}
		Invoke("RandPlayGO", Random.Range(RandMinTime, RandMaxTime));

	}
}
