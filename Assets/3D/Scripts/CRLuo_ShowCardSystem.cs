using UnityEngine;
using System.Collections;

public class CRLuo_ShowCardSystem : MonoBehaviour {

	public GameObject CarkObj;
	public float RotAngle;
	public CRLuo_ShowStage ShowID;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		CarkObj.transform.localRotation = Quaternion.Euler (new Vector3(0,RotAngle,0));
	
	}
	void Show(int ID)
	{

		ShowID.ShowCharactor (ID);
	}
}
