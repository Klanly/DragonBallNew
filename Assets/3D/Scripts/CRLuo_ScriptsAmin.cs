using UnityEngine;
using System.Collections;

public class CRLuo_ScriptsAmin : MonoBehaviour
{
	public bool Initial = false;
	//public Vector3 Initial_Move;
	public Vector3 Initial_Rotate;
	public Vector3 Initial_Scale = new Vector3(1, 1, 1);




	public CRLuo_ScriptsAmin_Element[] myClasses;

	private Vector3 v3_OriginPos;

	private Vector3 v3_OriginScale;

	private Vector3 v3_OriginRotation;

	void Start () {
		v3_OriginPos = this.transform.position;
		v3_OriginScale = this.transform.localScale;
		v3_OriginRotation = this.transform.eulerAngles;

		if (Initial)
		{

			//this.transform.position = Initial_Move + v3_OriginPos;

			this.transform.rotation = Quaternion.Euler(Initial_Rotate + v3_OriginRotation);

			this.transform.localScale = Initial_Scale;
		}

		Run();
	}
	

	public void Run() {
		foreach(CRLuo_ScriptsAmin_Element aMyClass in myClasses){
			HandleOneAction(aMyClass);
		}
	}

	void HandleOneAction(CRLuo_ScriptsAmin_Element myClass){

		if(myClass.b_EnableMove){
			StartCoroutine(DoMove(myClass.startTime, myClass.Move, myClass.leftTime, myClass.AnimType, myClass.PosSelf));
		}
		if (myClass.b_EnableScale)
		{
			StartCoroutine(DoScale(myClass.startTime, myClass.Scale, myClass.leftTime, myClass.AnimType));
		}
		if (myClass.b_EnableRotate)
		{
			StartCoroutine(DoRotate(myClass.startTime, myClass.Rotate, myClass.leftTime, myClass.AnimType, myClass.RotSelf));
		}
		if (myClass.b_EnableShake)
		{
			StartCoroutine(DoShake(myClass.startTime, myClass.Shake, myClass.leftTime, myClass.AnimType));
		}
	}

	IEnumerator DoMove(float delayTime, Vector3 v3, float animTime, MiniItween.EasingType animType,bool PosSelf)
	{
		yield return new WaitForSeconds(delayTime);

		if (PosSelf)
		{
			MiniItween.MoveTo(this.gameObject, v3 + v3_OriginPos, animTime, animType, false);
		}
		else
		{
			MiniItween.MoveTo(this.gameObject, v3 + v3_OriginPos, animTime, animType, true);
		}
	}

	IEnumerator DoRotate(float delayTime, Vector3 v3, float animTime, MiniItween.EasingType animType,bool RotSelf)
	{
		yield return new WaitForSeconds(delayTime);

		if (RotSelf)
		{
			MiniItween.RotateTo(this.gameObject, v3 + v3_OriginRotation, animTime, animType, false);
		}
		else
		{
			MiniItween.RotateTo(this.gameObject, v3 + v3_OriginRotation, animTime, animType, true);
		}
	}

	IEnumerator DoScale(float delayTime,Vector3 v3,float animTime,MiniItween.EasingType animType){
		yield return new WaitForSeconds(delayTime);
		 MiniItween.ScaleTo(this.gameObject, v3 , animTime, animType);
	}


	IEnumerator DoShake(float delayTime, Vector3 v3, float animTime, MiniItween.EasingType animType)
	{
		yield return new WaitForSeconds(delayTime);
		MiniItween.Shake(this.gameObject, v3, animTime, animType);
	}

}

[System.Serializable]
public class CRLuo_ScriptsAmin_Element {

	public bool b_EnableMove = false;
	public Vector3 Move;
	public bool PosSelf = false;

	public bool b_EnableRotate = false;
	public Vector3 Rotate;
	public bool RotSelf = false;

	public bool b_EnableScale = false;
	public Vector3 Scale = new Vector3(1, 1, 1);

	public bool b_EnableShake = false;
	public Vector3 Shake;

	public MiniItween.EasingType AnimType;
	public float startTime;
	public float leftTime;
}