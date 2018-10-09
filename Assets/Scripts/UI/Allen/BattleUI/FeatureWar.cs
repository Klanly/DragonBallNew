using UnityEngine;
using System.Collections;

public class FeatureWar : MonoBehaviour {
	public UILabel[] words;

	//
	// ---- 动画参数 ---
	//
	public Vector3 LargeVec3 = new Vector3(2F, 2F, 2F);
	public float ScaleTime = 0.2f;

	public Vector3 ShakeVec3 = new Vector3(2F, 2F, 2F);
	public float ShakeTime = 0.2f;

	// Use this for initialization
	void Start () {
		Init();
		StartCoroutine(Hit());
	}

	void Init() {
		foreach(UILabel lbl in words) {
			lbl.gameObject.SetActive(false);
			lbl.transform.localScale = Vector3.one;
		}
	}

	IEnumerator Hit() {
		Color trans = new Color(1F, 1F, 1F, 0F);

		foreach(UILabel lbl in words) {
			lbl.gameObject.SetActive(true);

			lbl.transform.localScale = LargeVec3;
			lbl.color = trans;

			MiniItween.ScaleTo(lbl.gameObject, Vector3.one, ScaleTime, MiniItween.EasingType.EaseOutCubic);
			MiniItween.ColorTo(lbl.gameObject, new V4(Color.white), ScaleTime, MiniItween.EasingType.EaseOutCubic, MiniItween.Type.ColorWidget);

			yield return new WaitForSeconds(ScaleTime);

			MiniItween.Shake(lbl.gameObject, ShakeVec3, ShakeTime, MiniItween.EasingType.EaseInCubic);
		}
	}

	/*void OnGUI() {
		if(GUI.Button(new Rect(0,0, 100,100), "TEST")) {
			Init();
			StartCoroutine(Hit());
		}
	}*/

}
