using UnityEngine;
using System.Collections;

public class ReadMe : MonoBehaviour {

	void OnGUI()
	{
        GUI.Label(new Rect(Screen.width * 0.1f, Screen.height * 0.1f + 20f, 200, 100), "程序：无限场景");
        GUI.Label(new Rect(Screen.width * 0.1f, Screen.height * 0.1f + 40f, 200, 100), "地面移动：W、A、S、D");
        GUI.Label(new Rect(Screen.width * 0.1f, Screen.height * 0.1f + 60f, 200, 100), "摄像机旋转：Q、E");
	}
}
