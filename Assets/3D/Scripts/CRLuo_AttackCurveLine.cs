using UnityEngine;
using System.Collections;

public class CRLuo_AttackCurveLine : MonoBehaviour
{

	//���һ���ƫ��
	public Vector2 TrackRandomOffset_LR = new Vector2(-45f, 45f);
	//���»���ƫ��
	public Vector2 TrackRandomOffset_UD = new Vector2(-45f, 45f);
	//���ƫ��
	public Vector3 v3_TargetRandomOffset = new Vector3(0.2f,0.2f,0.2f);
	//��ײ��Ч
	public GameObject FX_Boo;
	//����Ŀ��
	public GameObject target;

	//���չ���Ŀ��
	private Vector3 finalPos;

	//����ʱ��
	public float f_Time = 3;
	//����Ŀ����΢��
	public float f_BeiShu = 5;

	//ɾ���ӳ�
	public float DeleteSelfTime = 0.5f;

	//Ĭ���ٶ�����
	private float f_MaxSpeed = 50;
	//Ĭ�ϼ��ٶ�
	private float f_AddSpeed = 50;
	//��ǰ�ٶ�
	private Vector3 v3_Speed;

	void Start () {

		//�������ƫ�Ʒ�Χ
		v3_TargetRandomOffset = new Vector3(
			Random.Range(v3_TargetRandomOffset.x, -v3_TargetRandomOffset.x),
			Random.Range(v3_TargetRandomOffset.y, -v3_TargetRandomOffset.y),
			Random.Range(v3_TargetRandomOffset.z, -v3_TargetRandomOffset.z)
		);

		if (target != null)
		{
			finalPos = target.transform.position + v3_TargetRandomOffset;
		}
		else
		{
			finalPos = v3_TargetRandomOffset;
		}


		//��������ٶ�
		f_MaxSpeed = Vector3.Distance(finalPos,this.transform.position)/f_Time;
		//������ٶ�
		f_AddSpeed = f_MaxSpeed * f_BeiShu;
		//����ƫ�ƽǶ�
		float angleX = Random.Range(TrackRandomOffset_UD.x, TrackRandomOffset_UD.y);
		//����ƫ�ƽǶ�
		float angleY = Random.Range(TrackRandomOffset_LR.x, TrackRandomOffset_LR.y);
		//ZΪǰ������
		float angleZ = 0;
		//���ó�ʼ�Ƕ���λ��
		Quaternion rotation = Quaternion.Euler(angleX, angleY, angleZ);

		//������֮��ĵ�λ��������this.transform.positionΪ0��ָ��finalPos�ĵ�λ1�߶γ��ȵ�XYZ���꣩
		Vector3 dir = (finalPos - this.transform.position).normalized;
		//���սǶ� = ��ת����*ƫת��λ�����õ���ȷ��ƫ�ƺ�ĽǶ�
		dir = rotation * dir;
		//�����ٶ� = ��ʼ�Ƕ�*�ٶ�
		v3_Speed = dir * f_MaxSpeed;

		//�����̳���Ч����
		if (Ground_FX != null)
		{
			//�����̳�
			temp_Ground_FX = EmptyLoad.CreateObj(Ground_FX, new Vector3(this.transform.position.x, 0, this.transform.position.x), Quaternion.identity);
			//���ȹر���ʾ
			PartSystem_ONOFF(true);

		}

	}

	private bool b_WillDie = false;

	//private bool b_InUse = false;

	void Update () {

		//�̳���Ч�Ƿ����
		if (Ground_FX != null)
		{
			//�����̳�����λ��ƥ��
			temp_Ground_FX.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
			//����߶�С��2��ʾ
			if (this.transform.position.y < 2)
			{
				PartSystem_ONOFF(true);
			}
			else
			{
				PartSystem_ONOFF(false);
			}
		
		}

		//if (Input.GetKeyDown(KeyCode.A))
		//{
		//      b_InUse = true;
		//}

		//if (!b_InUse)
		//{
		//      return;
		//}

		//�����ǰ������ʧΪ��
		if(b_WillDie){
			//�������򣬲���������
			return;
		}

		//��ǰλ�� = ʸ���ٶ�*ʱ���ۼ�
		this.transform.position += v3_Speed * Time.deltaTime;
		//Ŀ��ʸ�� = Ŀ��λ�� - ��ǰλ��
		Vector3 target_Minus_This = finalPos - this.transform.position;
		//��������ٶ��뵱ǰĿ��ʸ������ С�� 0 ˵�����﹥��Ŀ��
		if( Vector3.Dot(v3_Speed,target_Minus_This) < 0 ){

			//׼��ɾ����ǰ����
			Destroy(this.gameObject, DeleteSelfTime);
			//�����ը��Ч����
			if(FX_Boo != null){
				//������ը��Ч ������Ч����Ч����ǰλ�ã��븸���򳡾���ת���룩
				EmptyLoad.CreateObj(FX_Boo,this.transform.position,Quaternion.identity);
			}
			//�������е�ǰ�����µ����ӹر�
			foreach(ParticleSystem aParticleSystem in GetComponentsInChildren<ParticleSystem>()){
				aParticleSystem.enableEmission = false;
			}

			foreach (MeshRenderer aRenderer in GetComponentsInChildren<MeshRenderer>())
			{
				aRenderer.enabled = false;
			}
			//��ǰ������ʧΪ��
			b_WillDie = true;
			//������ǰ�������κ󷽳���
			return;
		}

		//��׼����ǰʸ������
		Vector3 dir = Vector3.Normalize(target_Minus_This);
		//���ٶȽ�� = ��׼������*���ٶ�
		v3_Speed += dir * f_AddSpeed * Time.deltaTime;
		//�����ǰ�ٶ�ƽ�������ٱ��1ά�������ݣ� ��������ٶ�ƽ��
		if (Vector3.SqrMagnitude(v3_Speed) > f_MaxSpeed * f_MaxSpeed)
		{
			//��ǰ�ٶ� = ʸ����׼����λ*�ٶ�����
			v3_Speed = Vector3.Normalize(v3_Speed) * f_MaxSpeed;
		}
	}

	//������Ч����
	public GameObject Ground_FX;
	//������Ч��ʱ�洢����
	GameObject temp_Ground_FX;
	//������ɾ��ʱ����
	void OnDestroy()
	{
		//�ر�������Ч
		PartSystem_ONOFF(false);
		//ɾ��������Чʱ��
		Destroy(temp_Ground_FX,1f);
	}

	//�������ӿ��ؼ�
	void PartSystem_ONOFF(bool Key)
	{
		//���õ���������ʾ����
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
