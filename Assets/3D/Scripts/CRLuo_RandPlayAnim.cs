using UnityEngine;
using System.Collections;

public class CRLuo_RandPlayAnim : MonoBehaviour {

	public string[] AnimName;
	public float RandMinTime=1f;
	public float RandMaxTime=5f;
	public bool starPlay = false;
	public int PlayID = 0;
	public RandPlayFxElement [] ShowFX;

	// Use this for initialization
	void Start () {
		if (AnimName == null) {
			Debug.Log(this.gameObject.name + "Not Anim !!!!");
			Destroy(this);
		}

		if (starPlay) {
			animation.CrossFade(AnimName[PlayID]);
			StartCoroutine("CreateFx", PlayID);
				}
		if (AnimName.Length > 1) {
			Invoke ("RandPlayGO", Random.Range (RandMinTime, RandMaxTime));
		}
	}
	void Update()
	{

		if (!animation.isPlaying)
		{
			animation.CrossFade(AnimName[0]);
		}
	}
	void RandPlayGO()
	{
		int TempID = Random.Range(1, AnimName.Length);
		if(gameObject.activeInHierarchy)
		{
			StartCoroutine("CreateFx", TempID);
		}
		animation.CrossFade(AnimName[TempID]);


		Invoke("RandPlayGO", Random.Range(RandMinTime, RandMaxTime));

	}
	

	IEnumerator CreateFx(int ID)
	{
		if (ShowFX != null && ShowFX.Length != 0)
		{
			for (int i = 0; i < ShowFX.Length; i++)
			{
				if (ShowFX[i].ON_OFF  && ShowFX[i].Prefab_FX != null &&  ShowFX[i].PlayID == ID)
				{

					yield return new WaitForSeconds(ShowFX[i].FXtime);
					GameObject temp;
					
					temp = EmptyLoad.CreateObj(
						ShowFX[i].Prefab_FX,this.transform.position,this.transform.localRotation);

					temp.transform.parent = this.gameObject.transform;
					
					temp.transform.Translate(ShowFX[i].v3_FXPos_offset);
					temp.transform.Rotate(ShowFX[i].v3_FXRot_offset);

				}
				
				
			}
		}

	}


	

}

[System.Serializable]
public class RandPlayFxElement
{
	public int PlayID;
	public bool ON_OFF = true;
	
	public GameObject Prefab_FX;
	
	public float FXtime;
	
	public Vector3 v3_FXPos_offset;
	public Vector3 v3_FXRot_offset;
}
