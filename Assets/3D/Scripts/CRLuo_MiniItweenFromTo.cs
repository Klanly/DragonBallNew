using UnityEngine;
using System.Collections;

public class CRLuo_MiniItweenFromTo : MonoBehaviour {
	public Transform FromPosOBJ;
	public Transform TOPosOBJ;

	public bool Show_Key
	{
		set
		{
			if (value) {
				this.gameObject.transform.position = FromPosOBJ.position;
				StartCoroutine("GoMove");
			} else {
				this.gameObject.transform.position = TOPosOBJ.position;
				Destroy (this);
			}
		}
	}
	public float StartTime;
	public float LongTime;

	public MiniItween.EasingType myType;
		
	// Update is called once per frame
	IEnumerator GoMove() {

		yield return new WaitForSeconds (StartTime);

		MiniItween.MoveTo (this.gameObject, TOPosOBJ.position, LongTime, myType);
	}
}
