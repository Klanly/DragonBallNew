using UnityEngine;
using System.Collections;

public class CRLuo_Transform_Add : MonoBehaviour
{
	public string _ = "-=<����λ�ơ���ת�����š��޸��ۼӳ���>=-";
	public string __ = "λ���ۼ�";
	public bool Move_Add_Key;
	public Vector3 Move_Add;
	public Vector3 Move_SpeedUp;
	public string ___ = "��ת�ۼ�";
	public bool Rotation_Add_Key;
	public Vector3 Rotation_Add;
	public Vector3 Rotation_SpeedUp;
	public string ____ = "�����ۼ�";
	public bool Scale_Add_Key;
	public Vector3 Scale_Add;
	public Vector3 Scale_SpeedUp;

	public string _____ = "ʱ�����";
	public bool Time_Add_Key = false;
	public float StartTime = 0;
	public float EndTime = 3;
	public string ______ = "�Զ�����";
	public bool AutoDelete ;
	void Start () {
		//�����ʱ�����
		if(Time_Add_Key){
			//��������Զ�����
			if (AutoDelete)
			{
				//��ʱִ��ɾ��
				Invoke("Del", EndTime);
			}
			else
			{
				//��ʱִ��ֹͣ
				Invoke("Stop", EndTime);
			}
		}
	}

	//�Զ���ɾ������
	void Del(){
		//ɾ����ǰ����
		Destroy(this.gameObject);
	}
	//�Զ���ֹͣ����
	void Stop()
	{
		//ɾ����δ���
		Destroy(this);
	}
	void Update () {

		//�Ƿ���ʱ�����
		if (Time_Add_Key)
		{
			//���û�е���ʼʱ��
			if(StartTime > 0){
				//��ʼʱ���ÿһ֡��϶ʱ��
				StartTime -= Time.deltaTime;
				//��һ֡������ֱ��������ǰ����
				return;
			}
		}

		//�������λ���ۼ�
		if (Move_Add_Key)
		{
			Move_SpeedUp += Move_SpeedUp * Time.deltaTime;
			Move_Add += Move_SpeedUp * Time.deltaTime;

			//λ���޸�Ϊ = �޸�ֵ*֡��϶ʱ��
			this.transform.Translate(Move_Add * Time.deltaTime);
		}

		//���������ת�ۼ�
		if (Rotation_Add_Key)
		{
			Rotation_SpeedUp += Rotation_SpeedUp * Time.deltaTime;
			Rotation_Add += Rotation_SpeedUp * Time.deltaTime;
			//λ���޸�Ϊ = �޸�ֵ*֡��϶ʱ��
			this.transform.Rotate(Rotation_Add * Time.deltaTime);
		}

		//������������ۼ�
		if (Scale_Add_Key)
		{
			Scale_SpeedUp += Scale_SpeedUp * Time.deltaTime;
			Scale_Add += Scale_SpeedUp * Time.deltaTime;
			//λ���޸�Ϊ = �޸�ֵ*֡��϶ʱ��
			this.transform.localScale += Scale_Add * Time.deltaTime;
		}

	}
}
