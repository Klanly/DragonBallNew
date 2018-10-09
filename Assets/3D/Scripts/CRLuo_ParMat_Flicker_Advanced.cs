using UnityEngine;
using System.Collections;

//������:CRomputer-��
//����Ⱥ:121463857

public class CRLuo_ParMat_Flicker_Advanced : MonoBehaviour
{
	//��ǰ͸��������ֵ
	private float Intensity = 0;
	//��ʾ���λ
	private float State = 0;
	//��˸����
	private float speed = 0;
	//�������ֵ
	private float Min = 0;
	private float Max = 0;
	//��������ɫ�ݴ�
	private float ColorR = 0;
	private float ColorG = 0;
	private float ColorB = 0;

	private Color clr;

	//�رչ��̱��λ
	private bool MainKeyTF;
	//�ر�ʱ�ݴ�����
	private float NowIntensity = 0;

	public string _ = "-=<��㼶���Ӳ�����˸����_�߼���>=-";
	public string __ = "��˸����ģ��(��MeshRenderer����)";
	public Renderer myRender;
	public string ___ = "������ID";
	public int ElementID = 0;
	public string ____ = "������ɫ����";
	public bool SetColor_Key;
	public string _____ = "��ɫ";
	public Color myColor;
	public string ______ = "��˸��ʼʱ��";
	public float StartTime = 0;
	public string _______ = "ÿ����˸ʱ��";
	public float OneFlickerTime = 1;
	public string ________ = "�������";
	public float FlickerMax = 1;
	public string _________ = "-------�߼������˸����--------";
	public bool OnAdvancedFlicker;
	public string __________ = "�ر�ʱ��";
	public float OffTimeMin = 0;
	public float OffTimeMax = 10;
	public string ___________ = "����ʱ��";
	public float UpTimeMin = 0;
	public float UpTimeMax = 0.5f;
	public string ____________ = "����ʱ��";
	public float OnTimeMin = 0;
	public float OnTimeMax = 1;
	public string _____________ = "�䰵ʱ��";
	public float DownTimeMin = 0;
	public float DownTimeMax = 1;
	public string ______________ = "��������";
	public float IntensityMin = 0;
	public string _______________ = "�������";
	public bool IntensityMinRand;
	public string ________________ = "��������";
	public float IntensityMax = 1;
	public string _________________ = "�������";
	public bool IntensityMaxRand;

	public string __________________ = "�ⲿ�ƹ�����";
	public Light light;
	public string ___________________ = "�ƹ�����΢��";
	public float lightIntensityScle = 1;
	public string ____________________ = "��˸������";
	public bool MainKey = true;


	private Material selMat;

	void Start () {
		if (SetColor_Key)
		{
			ColorR = myColor.r;
			ColorG = myColor.g;
			ColorB = myColor.b;
		}
		else
		{
			//������ɫ
			ColorR = myRender.materials[ElementID].GetColor("_TintColor").r;
			ColorG = myRender.materials[ElementID].GetColor("_TintColor").g;
			ColorB = myRender.materials[ElementID].GetColor("_TintColor").b;

		}

		clr = new Color (ColorR, ColorG, ColorB, 0);

		selMat = myRender.materials [ElementID];
//		myRender.materials[ElementID].SetColor("_TintColor", clr);
		selMat.SetColor("_TintColor", clr);
	}

	void Update () {

		//������
		if (MainKey)
		{
			//��ʼ��ʱ
			if (StartTime > 0)
			{
				StartTime -= Time.deltaTime;
				return;
			}


			//�߼�����
			if (OnAdvancedFlicker)
			{
				//���ø߼���˸
				StartCoroutine(AdvancedFlicker());
			}
			else
			{
//				������˸
				BasisFlicker();
			}

			//�رս������
			MainKeyTF = true;

//			NowIntensity = myRender.materials[ElementID].GetColor("_TintColor").a;
			NowIntensity = selMat.GetColor("_TintColor").a;
		}
		else
		{ 
			//�����رս���
			if(MainKeyTF)
			{
				//�ر�ʱ���ȼ�С
				NowIntensity -= Time.deltaTime;
				//������׹ر�
				if (NowIntensity <= 0)
				{
					//���ȹ���
					NowIntensity = 0;
					//�رս���
					MainKeyTF = false;
					//�߼���˸״̬����
					State = 0;
					//���ȹ���
					Intensity = 0;
				}


//				myRender.materials[ElementID].SetColor("_TintColor", new Color(ColorR, ColorG, ColorB, NowIntensity));
//				clr.a = NowIntensity;
//				myRender.materials[ElementID].SetColor("_TintColor", clr);
				selMat.SetColor("_TintColor", clr);
			
			}
		
		
		}

	}

	void BasisFlicker()
	{//������˸

		//״̬0
		if (State == 0)
		{
			//���Ϊ����״̬
			State = 1;
			//������˸Ƶ��
			speed = FlickerMax / OneFlickerTime;
		};
		if (State == 1)
		{
			//�����ۼ�
			if (Intensity <= FlickerMax)
			{
				//�����ۼ�
				Intensity += Time.deltaTime * speed;
				//���ò�����ƹ�����
				setLinght();
			}
			else
			{
				//������������������
				Intensity = FlickerMax;
				//���ò�����ƹ�����
				setLinght();
				//���ñ��λΪ�䰵
				State = -1;
				//�������ȵݼ�����
				speed = -FlickerMax / OneFlickerTime * 0.5f;
			}
		};

		//�䰵����
		if (State == -1)
		{
			//�䰵�ۼ�
			if (Intensity > 0)
			{
				//�����ۼ�
				Intensity += Time.deltaTime * speed;
				//���ò�����ƹ�����
				setLinght();

			}
			else
			{
				//��ɱ䰵������С����
				Intensity = 0;
				//��������
				setLinght();
				//����Ϊ�ر�״̬
				State = 0;

			}
		};
	
	}

	IEnumerator AdvancedFlicker()
	{//�߼���˸
		//yield return null;IEnumerator�޷���ֵʱʹ��
		
		//״̬0
		if (State == 0)
		{
			//�ر�ʱ����ֹͣ������ر�ʱ�䣩
			yield return new WaitForSeconds(Random.Range(OffTimeMin, OffTimeMax));
			//���Ϊ����״̬
			State = 1;
			//���ñ�������
			speed = 1 / Random.Range(UpTimeMin, UpTimeMax);
			//��ȡ�������������
			rand_MinMax();

		};

		//��������
		if (State == 1)
		{
			//�����ۼ�
			if (Intensity <= Max)
			{
				//�����ۼ�
				Intensity += Time.deltaTime * speed;
				//���ò�����ƹ�����
				setLinght();
			}
			else
			{
				//������������������
				Intensity = Max;
				//���ò�����ƹ�����
				setLinght();
				//���ȱ���ʱ��
				yield return new WaitForSeconds(Random.Range(OnTimeMin, OnTimeMax));
				//���ñ��λΪ�䰵
				State = -1;
				//�������ȵݼ�����
				speed = -1 / Random.Range(DownTimeMin, DownTimeMax);
				//��ȡ�������������
				rand_MinMax();
			}
		};

		//�䰵����
		if (State == -1)
		{
			//�䰵�ۼ�
			if (Intensity > Min)
			{
				//�����ۼ�
				Intensity += Time.deltaTime * speed;
				//���ò�����ƹ�����
				setLinght();

			}
			else
			{
				//��ɱ䰵������С����
				Intensity = Min;
				//��������
				setLinght();
				//�ر�״̬����ʱ��
				yield return new WaitForSeconds(Random.Range(OffTimeMin, OffTimeMax));
				//����Ϊ����״̬
				State = 1;
				//���ñ�������
				speed = 1 / Random.Range(UpTimeMin, UpTimeMax);
				//��ȡ�������������
				rand_MinMax();
			}
		};


	}

	//�������������
	void rand_MinMax()
	{
		//����������޿���
		if (IntensityMinRand)
		{
			//����������ޣ�0~Min��
			Min = Random.Range(0, IntensityMin);
		}
		else
		{
			//ֱ��������Сֵ
			Min = IntensityMin; 
		};
		//����������޿���
	      if(IntensityMaxRand)
		{
			//����������ޣ�Min~Max��
			Max = Random.Range(Min, IntensityMax); }
	      else
		{
			//ֱ���������ֵ
			Max = IntensityMax; 
		};

	}

	//��������
	void setLinght()
	{
		
		//����͸���� = ������0~1
		float Alp = Mathf.Clamp(Intensity,0,1);

		clr.a = Alp;
//		myRender.materials[ElementID].SetColor("_TintColor", new Color(ColorR, ColorG, ColorB, Alp));
//		myRender.materials[ElementID].SetColor("_TintColor", clr);

		selMat.SetColor("_TintColor", clr);
		//���õƹ�����
		if (light != null)
		{
			light.intensity = Alp * lightIntensityScle;
		
		}
	
	}

}
