using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class ScreenShot : MonoBehaviour {

	// «∑Ò√ø÷°∂ºΩÿ∆¡
	public bool b_PerFrame = false;

	public int f_FrameNumInSecond = 5;

	private int index = 1;

	private int defaultFPS = 30;

	private string path = "D:/ScreenShots";

	void Start () {

		if (Directory.Exists(path)) {
			Directory.Delete(path, true);
		}

		if (!Directory.Exists(path)) {
			Directory.CreateDirectory(path);
		}

		Application.targetFrameRate = defaultFPS;
		if(!b_PerFrame){
			InvokeRepeating("Func", 0.01f, 1/(float)f_FrameNumInSecond);
		}
	}

	void Update() { 
		if(b_PerFrame){
			Func();
		}
	}

	void Func() {
		Application.CaptureScreenshot("D:/ScreenShots/" + index + ".png");
		Debug.Log(index++);
	}

}
