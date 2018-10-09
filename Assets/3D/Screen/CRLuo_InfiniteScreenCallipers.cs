using UnityEngine;
using System.Collections;

public class CRLuo_InfiniteScreenCallipers : MonoBehaviour
{
	public float U_Callipers;
	public float V_Callipers;
	public Vector3 UVOffset;
	public float ShowLong;
	public int ID_U;
	public int ID_V;

    GameObject ParentOBJ;

	// Use this for initialization
	void Start () {
        ParentOBJ = this.gameObject.transform.parent.gameObject;
	
	}
	
	// Update is called once per frame
	void Update () {

        if ((this.transform.position.x - ParentOBJ.transform.position.x) > U_Callipers)
        {

            Vector3 temp = this.transform.position;

            temp.x -= U_Callipers * 2;

            this.transform.position = temp;

        }
        else if((this.transform.position.x - ParentOBJ.transform.position.x) < (-U_Callipers))
        {

            Vector3 temp = this.transform.position;

            temp.x += U_Callipers * 2;

            this.transform.position = temp;

        }





        if ((this.transform.position.z - ParentOBJ.transform.position.z) > V_Callipers)
        {

            Vector3 temp = this.transform.position;

            temp.z -= V_Callipers * 2;

            this.transform.position = temp;

        }
        else if ((this.transform.position.z - ParentOBJ.transform.position.z) < (-V_Callipers))
        {

            Vector3 temp = this.transform.position;

            temp.z += V_Callipers * 2;

            this.transform.position = temp;

        }




	
	}

	void OnDrawGizmos() {

			Gizmos.color = new Color(1, 0, 1);
			Gizmos.DrawLine(this.transform.position + new Vector3(-U_Callipers * 0.5f, 0, 0) + UVOffset, this.transform.position + new Vector3(U_Callipers * 0.5f, 0, 0) + UVOffset);
			Gizmos.DrawLine(this.transform.position + new Vector3(U_Callipers * 0.5f, -ShowLong, 0) + UVOffset, this.transform.position + new Vector3(U_Callipers * 0.5f, ShowLong, 0) + UVOffset);
			Gizmos.DrawLine(this.transform.position + new Vector3(-U_Callipers * 0.5f, -ShowLong, 0) + UVOffset, this.transform.position + new Vector3(-U_Callipers * 0.5f, ShowLong, 0) + UVOffset);
			Gizmos.DrawLine(this.transform.position + new Vector3(U_Callipers * 0.5f, 0, -ShowLong) + UVOffset, this.transform.position + new Vector3(U_Callipers * 0.5f, 0, ShowLong) + UVOffset);
			Gizmos.DrawLine(this.transform.position + new Vector3(-U_Callipers * 0.5f, 0, -ShowLong) + UVOffset, this.transform.position + new Vector3(-U_Callipers * 0.5f, 0, ShowLong) + UVOffset);



			Gizmos.DrawLine(this.transform.position + new Vector3(0, 0, -V_Callipers * 0.5f) + UVOffset, this.transform.position + new Vector3(0, 0, V_Callipers * 0.5f) + UVOffset);
			Gizmos.DrawLine(this.transform.position + new Vector3(0, -ShowLong, V_Callipers * 0.5f) + UVOffset, this.transform.position + new Vector3(0, ShowLong, V_Callipers * 0.5f) + UVOffset);
			Gizmos.DrawLine(this.transform.position + new Vector3(0, -ShowLong, -V_Callipers * 0.5f) + UVOffset, this.transform.position + new Vector3(0, ShowLong, -V_Callipers * 0.5f) + UVOffset);
			Gizmos.DrawLine(this.transform.position + new Vector3(-ShowLong,0, V_Callipers * 0.5f) + UVOffset, this.transform.position + new Vector3(ShowLong,0 , V_Callipers * 0.5f) + UVOffset);
			Gizmos.DrawLine(this.transform.position + new Vector3(-ShowLong,0 , -V_Callipers * 0.5f) + UVOffset, this.transform.position + new Vector3(ShowLong,0 , -V_Callipers * 0.5f) + UVOffset);

		}
}
