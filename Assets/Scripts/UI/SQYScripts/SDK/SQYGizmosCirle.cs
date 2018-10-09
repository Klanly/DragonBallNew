using UnityEngine;
using System.Collections;

public class SQYGizmosCirle : MonoBehaviour {

	public Color boundsColor = Color.red;
	public float radiou = 6;
	public bool b_OnDrawGizmos = true;

	void OnDrawGizmos() {
		if(b_OnDrawGizmos)
		{
			Gizmos.color = boundsColor;

			Gizmos.DrawWireSphere(Pos(0,0),radiou);
		}
	}
	Vector3 Pos(float x,float y){
		return this.transform.position + x * Vector3.right + y * Vector3.up;
	}

}
