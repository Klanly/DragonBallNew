using UnityEngine;
using System.Collections;

public class SQYGizmosSplitRectangular : MonoBehaviour {


	public int lineNum = 1;
	public int rowNum = 1;
	public float width = 13.0f;
	public float height = 13.0f;
	public Color boundsColor = Color.green;
	public bool b_OnDrawGizmos = true;

	void OnDrawGizmos() {
		if(b_OnDrawGizmos)
		{
			Gizmos.color = boundsColor;
			
			float dW =  width/(rowNum+1);
			float dH = height/(lineNum+1);
			
			
			for(int i=1;i<lineNum+1;i++)
			{
				Gizmos.DrawLine(Pos(-width/2,-height/2+dH*i),Pos(width/2,-height/2+dH*i));
			}
			for(int j=0;j<rowNum+1;j++)
			{
				Gizmos.DrawLine(Pos(-width/2+j*dW,-height/2),Pos(-width/2+j*dW,height/2));
			}
			
			Gizmos.DrawLine(Pos(-width/2,-height/2), Pos(width/2,-height/2));
			Gizmos.DrawLine(Pos(-width/2,height/2), Pos(width/2,height/2));
			Gizmos.DrawLine(Pos(-width/2,-height/2), Pos(-width/2,height/2));
			Gizmos.DrawLine(Pos(width/2,-height/2), Pos(width/2,height/2));

		}
//		if(b_OnDrawCenterLine)
		{
//			Gizmos.color = boundsColor;
//			Gizmos.DrawLine(Pos(-width/2,0),Pos(width/2,0));
//			Gizmos.DrawLine(Pos(0,-height/2),Pos(0,height/2));
		}
	}
	Vector3 Pos(float x,float y){
		return this.transform.position + x * Vector3.right + y * Vector3.up;
	}
}
