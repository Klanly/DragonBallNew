using UnityEngine;
using System.Collections;

public class BanCurveLine : MonoBehaviour {

	public Vector2 TrackRandomOffset_LR = new Vector2(-45f, 45f);
	public Vector2 TrackRandomOffset_UD = new Vector2(-45f, 45f);
	public Vector3 v3_TargetRandomOffset = new Vector3(0.2f,0.2f,0.2f);

	public GameObject prefab_Explosion;

	public GameObject target;

	[System.NonSerialized]
	public Vector3 targetPos;

	public Vector3 finalPos{
		get{
			if(target != null){
				return target.transform.position + v3_TargetRandomOffset;
			}else{
				return targetPos + v3_TargetRandomOffset;
			}
		}
	}

	public float f_Time = 3;

	public float f_BeiShu = 5;

	public float DeleteSelfTime = 0.5f;

	private float f_MaxSpeed = 30;

	private float f_AddSpeed = 30;

	private Vector3 v3_Speed;

	void Start () {

		v3_TargetRandomOffset = new Vector3(
			Random.Range(v3_TargetRandomOffset.x, -v3_TargetRandomOffset.x),
			Random.Range(v3_TargetRandomOffset.y, -v3_TargetRandomOffset.y),
			Random.Range(v3_TargetRandomOffset.z, -v3_TargetRandomOffset.z)
		);

		f_MaxSpeed = Vector3.Distance(finalPos,this.transform.position)/f_Time;

		f_AddSpeed = f_MaxSpeed * f_BeiShu;

		float angleX = Random.Range(TrackRandomOffset_UD.x, TrackRandomOffset_UD.y);

		float angleY = Random.Range(TrackRandomOffset_LR.x, TrackRandomOffset_LR.y);

		float angleZ = 0;


		Quaternion rotation = Quaternion.identity;
		//投掷时保证弧线向上
		if (target != null)
		{
			if (target.transform.position.z - this.gameObject.transform.position.z > 0)
			{

				rotation = Quaternion.Euler(angleX, angleY, angleZ);

			}
			else
			{

				rotation = Quaternion.Euler(-angleX, angleY, angleZ);
			}
		}
		Vector3 dir = (finalPos - this.transform.position).normalized;

		dir = rotation * dir;

		v3_Speed = dir * f_MaxSpeed;

		if (Ground_FX != null)
		{
			temp_Ground_FX = ParticleManager.CreateParticle(Ground_FX, transform.position);
			PartSystem_ONOFF(true);
		}

	}

	private bool b_WillDie = false;

	//private bool b_InUse = false;

	void Update () {

		if (temp_Ground_FX != null)
		{
			temp_Ground_FX.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
			if (this.transform.position.y < 2)
			{
				PartSystem_ONOFF(true);
			}
			else
			{
				PartSystem_ONOFF(false);
			}
		
		}

		if(b_WillDie){
			return;
		}

		this.transform.position += v3_Speed * Time.deltaTime;
		Vector3 target_Minus_This = finalPos - this.transform.position;
		if( Vector3.Dot(v3_Speed,target_Minus_This) < 0 ){

			Destroy(this.gameObject, DeleteSelfTime);
			if(prefab_Explosion != null){
				EmptyLoad.CreateObj(prefab_Explosion, transform.position, Quaternion.identity);
			}
			foreach(ParticleSystem aParticleSystem in GetComponentsInChildren<ParticleSystem>()){
				aParticleSystem.enableEmission = false;
			}

			foreach (MeshRenderer aRenderer in GetComponentsInChildren<MeshRenderer>())
			{
				aRenderer.enabled = false;
			}

			b_WillDie = true;
			return;
		}

		Vector3 dir = Vector3.Normalize(target_Minus_This);
		v3_Speed += dir * f_AddSpeed * Time.deltaTime;
		if (Vector3.SqrMagnitude(v3_Speed) > f_MaxSpeed * f_MaxSpeed)
		{
			v3_Speed = Vector3.Normalize(v3_Speed) * f_MaxSpeed;
		}
	}

	public GameObject Ground_FX;

	GameObject temp_Ground_FX;
	void OnDestroy()
	{
		PartSystem_ONOFF(false);
		if(temp_Ground_FX != null) Destroy(temp_Ground_FX);
		ParticleManager.ReleaseOne();
	}
	void PartSystem_ONOFF(bool Key)
	{
		if (temp_Ground_FX != null)
		{
			ParticleSystem[] PartTemp = temp_Ground_FX.GetComponentsInChildren<ParticleSystem>();
			foreach (ParticleSystem aPartTemp in PartTemp)
			{
				aPartTemp.enableEmission = Key;
			}

		}
	}
}
