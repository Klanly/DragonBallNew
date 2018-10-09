using UnityEngine;
using System.Collections;

public class RivalAnimEditor : MonoBehaviour {

	public float timeScale = 0.2f;

	private int index = 0;

	private float startTime = 0;

	private float lastTime = 0;

	public static RivalAnimEditor Instance = null;

	private string lastAnimName = "";

	public static CRLuo_PlayAnim_FX GetRivalObj(){
		if(Instance != null){
			return Instance.go_RivalObj;
		}else{
			return null;
		}
	}

	public CRLuo_PlayAnim_FX go_RivalObj;

	public CRLuo_PlayAnim_FX go_Attacker;

	void Awake(){
		Instance = this;
	}

	void Start(){
		go_RivalObj.Try_Key = false;
		go_Attacker.Try_Key = true;

		go_Attacker.RivalOBJ = go_RivalObj;
		Time.timeScale = timeScale;
	}

	void Init(){
		startTime = Time.time;
		lastTime = Time.time;
		index = 0;
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Alpha1) || 
		   Input.GetKeyDown(KeyCode.Alpha2) ||
		   Input.GetKeyDown(KeyCode.Alpha3) ||
		   Input.GetKeyDown(KeyCode.Alpha4) ||
		   Input.GetKeyDown(KeyCode.Alpha5) ||
		   Input.GetKeyDown(KeyCode.Alpha6) ||
		   Input.GetKeyDown(KeyCode.Alpha7) ||
		   Input.GetKeyDown(KeyCode.Alpha8) ||
		   Input.GetKeyDown(KeyCode.Alpha9) ||
		   Input.GetKeyDown(KeyCode.Alpha0)){
			Init();
			Debug.LogWarning("Ready");
		}
		if(Input.GetKeyDown(KeyCode.P)){
			Take();
			Debug.Break();
		}else if(Input.GetKeyDown(KeyCode.O)){
			Take();
		}

		if(lastAnimName == ""){
			lastAnimName = go_Attacker.GetCurAnim();
		}else if(lastAnimName != go_Attacker.GetCurAnim()){
			lastAnimName = go_Attacker.GetCurAnim();
			Init();
			index = 0;
		}

	}

	void Take(){
				//Debug.LogWarning(" AnimName:"+go_Attacker.GetCurAnim()+"\t\tindex:"+(index++)+"\t\t time:"+(Time.time - startTime)+"\t\t waitTime:"+(Time.time - lastTime));

		if (index == 0)
		{
			Debug.LogWarning("FXType:----------" + go_Attacker.GetCurAnim() +"----------");
		
		}


		Debug.LogWarning("Element:" + (index++) + "\t waitTime:" + "\t \t " + (Time.time - lastTime) + "\t \t time:" + (Time.time - startTime));

		lastTime = Time.time;
	}

}



























