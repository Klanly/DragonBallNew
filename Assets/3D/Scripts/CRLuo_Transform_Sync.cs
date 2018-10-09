using UnityEngine;
using System.Collections;

public class CRLuo_Transform_Sync : MonoBehaviour {

	public GameObject InputOBJ;
	public bool Pos_Key;
	public Vector3 Offset_Pos = Vector3.zero;
	public bool Rot_Key;
	public Vector3 Offset_Rot = Vector3.zero;
	public bool Sca_Key;
	public Vector3 Offset_Sca = Vector3.zero;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Pos_Key) {
			this.gameObject.transform.localPosition = InputOBJ.transform.localPosition + Offset_Pos;
		}

		if (Rot_Key) {
			this.gameObject.transform.localRotation = InputOBJ.transform.localRotation;
			if(Offset_Rot != Vector3.zero)
			this.gameObject.transform.Rotate(Offset_Rot);
		}

		if (Sca_Key) {
			this.gameObject.transform.localScale = InputOBJ.transform.localScale + Offset_Sca;
		}

	}
}
