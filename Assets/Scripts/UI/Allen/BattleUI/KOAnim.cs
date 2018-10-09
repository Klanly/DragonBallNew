using UnityEngine;
using System.Collections;

public class KOAnim : MonoBehaviour {
	public UISprite spK;
	public UISprite spO;
	public Vector3 InitScale = new Vector3(4f, 4f, 4f);
	public Vector3 ShakeVec  = new Vector3(3f, 3f, 3f);
	public float  ScaleTime  = 0.2f;
	public float  ShakeTime  = 0.1f;
	public float  HoldTime   = 1f;

	public void playAnim() {
		init();
		StartCoroutine(anim());
	}

	void init() {
		spK.gameObject.SetActive(true);
		spO.gameObject.SetActive(true);
	}

	IEnumerator anim() {
		Color trans = new Color(1f, 1f, 1f, 0f);

		//透明，变大的状态
		spK.transform.localScale = InitScale;
		spK.color = trans;
		spO.transform.localScale = InitScale;
		spO.color = trans;

		//开始显示K
		Core.Data.soundManager.SoundFxPlay(SoundFx.FX_KO);
		MiniItween.ScaleTo(spK.gameObject, Vector3.one, ScaleTime);
		MiniItween.ColorTo(spK.gameObject, new V4(Color.white), ScaleTime, MiniItween.Type.ColorWidget);
		yield return new WaitForSeconds(ScaleTime);

		//开始震动K
		MiniItween.Shake(spK.gameObject, ShakeVec, ShakeTime, MiniItween.EasingType.EaseOutExpo);

		//开始显示O
		Core.Data.soundManager.SoundFxPlay(SoundFx.FX_KO);
		MiniItween.ScaleTo(spO.gameObject, Vector3.one, ScaleTime);
		MiniItween.ColorTo(spO.gameObject, new V4(Color.white), ScaleTime, MiniItween.Type.ColorWidget);
		yield return new WaitForSeconds(ScaleTime);

		//开始震动O
		MiniItween.Shake(spO.gameObject, ShakeVec, ShakeTime, MiniItween.EasingType.EaseOutExpo);

		//保持稳定
		yield return new WaitForSeconds(HoldTime);


		//开始消失KO
		MiniItween.ScaleTo(spK.gameObject, InitScale, ScaleTime);
		MiniItween.ColorTo(spK.gameObject, new V4(trans), ScaleTime, MiniItween.EasingType.EaseOutExpo, MiniItween.Type.ColorWidget);
		MiniItween.ScaleTo(spO.gameObject, InitScale, ScaleTime);
		MiniItween.ColorTo(spO.gameObject, new V4(trans), ScaleTime, MiniItween.EasingType.EaseOutExpo, MiniItween.Type.ColorWidget);

	}

	/* Only for test
	 * void OnGUI() {
		if(GUI.Button(new Rect(0,0,100,100), "play")) {
			playAnim();
		}
	}*/

}
