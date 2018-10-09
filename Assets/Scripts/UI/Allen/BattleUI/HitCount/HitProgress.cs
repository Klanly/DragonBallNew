using UnityEngine;
using System.Collections;

public class HitProgress : MonoBehaviour {
	public UISprite curPro;

	//当Enable的时候，应该是满的状态
	private bool initStatus = false;
	private float curTime   = 0f;
	private float totalTime = 0f;

	void Start() {
		totalTime = BanTimeCenter.F_Hide_Btn - 0.1f;
	}

	void OnEnable() {
		curPro.fillAmount = 1.0f;
		curTime  = 0f;
		initStatus = true;
	}

	void OnDisable() {
		curPro.fillAmount = 0f;
		initStatus = false;
	}

	void FixedUpdate() {
		if(initStatus) {
			curTime += Time.deltaTime;
			curPro.fillAmount = 1.0f - curTime / totalTime;
		}
	}
}
