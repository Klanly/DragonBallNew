using UnityEngine;
using System.Collections;

public class SQYGizmosSplitCirle : MonoBehaviour {

	public Color boundsColor = Color.red;
	public float radiou = 6;
	public int splitNum = 6;
	public bool b_OnDrawGizmos = true;
	void OnDrawGizmos() {
		if(b_OnDrawGizmos)
		{
			Gizmos.color = boundsColor;

			Gizmos.DrawLine(Pos(0,0),Pos(0,radiou*Mathf.Sin(Mathf.PI/2)));

			for(int i=1;i<splitNum;i++)
			{
				float rr = Mathf.PI/2+ i*2*Mathf.PI/splitNum;
				Gizmos.DrawLine(Pos(0,0),Pos(radiou*Mathf.Cos(rr),radiou*Mathf.Sin(rr)));
			}
		}
	}
	Vector3 Pos(float x,float y){
		return this.transform.position + x * Vector3.right + y * Vector3.up;
	}
}
