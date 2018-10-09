using UnityEngine;
using System.Collections;

public class CRLuo_LookAt_BanCurveLine : MonoBehaviour
{

	private GameObject _MyLookAtPos;
	public GameObject MyLookAtPos
	{
	      get
	      {
	            if (_MyLookAtPos == null)
	            {
	                  _MyLookAtPos = this.transform.parent.GetComponent<BanCurveLine>().target;

	            }
	            return _MyLookAtPos;
	      }
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (MyLookAtPos != null)
		{
			this.gameObject.transform.LookAt(MyLookAtPos.transform.position);
		
		}
	}


}
