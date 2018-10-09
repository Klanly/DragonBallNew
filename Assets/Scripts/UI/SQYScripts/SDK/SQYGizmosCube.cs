using UnityEngine;
using System.Collections;

public class SQYGizmosCube : MonoBehaviour {

	public float width = 13.0f;
	public Color boundsColor = Color.green;
	public bool b_OnDrawGizmos = true;
	void OnDrawGizmos() {
		if(b_OnDrawGizmos)
		{
			Gizmos.color = boundsColor;
			Gizmos.DrawLine(Pos(-width/2,-width/2), Pos(width/2,-width/2));
			Gizmos.DrawLine(Pos(-width/2,width/2), Pos(width/2,width/2));
			Gizmos.DrawLine(Pos(-width/2,-width/2), Pos(-width/2,width/2));
			Gizmos.DrawLine(Pos(width/2,-width/2), Pos(width/2,width/2));
			Gizmos.DrawLine(Pos(-width/2,0),Pos(width/2,0));
			Gizmos.DrawLine(Pos(0,-width/2),Pos(0,width/2));
		}
	}
	Vector3 Pos(float x,float y){
		return this.transform.position + x * Vector3.right + y * Vector3.up;
	}
}
