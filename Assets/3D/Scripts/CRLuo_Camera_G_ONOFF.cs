using UnityEngine;
using System.Collections;

public class CRLuo_Camera_G_ONOFF : MonoBehaviour {

	public Camera[] Camera_G;
	public Camera_G_Key[] ShowKey;
	int NowID;
	// Use this for initialization
	void Start () {
		foreach (Camera aCamera_G in Camera_G)
		{
			aCamera_G.enabled = false;
		}
		NowID = 0;
		foreach (Camera_G_Key aShowKey in ShowKey)
			{
				Invoke("CameraONOFF", aShowKey.ON_Time);


			}
	}

	void CameraONOFF()
	{
		foreach (Camera aCamera_G in Camera_G)
		{
			aCamera_G.enabled = false;
		}

		Camera_G[ShowKey[NowID].ID].enabled = true;
		NowID++;
	}


}
[System.Serializable]
public class Camera_G_Key
{
	public int ID;
	public float ON_Time;

}