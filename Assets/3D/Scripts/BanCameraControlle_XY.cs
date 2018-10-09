using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BanCameraControlle_XY : MonoBehaviour
{

	//----CRLuo��-----
	public Vector3 Default_Pos = new Vector3(7,1,3);
	public Vector3 Default_Rot = new Vector3( 0,270,0);
	public bool GoDefault_Key;
	//----CRLuo��-----
	//��������λ��
	public float CameraMinHeight = 1f;

	//�߶ȷ�Χ����
	public float CameraFOVAddHeight = 7f;
	public float CameraFOVAddHeight_LongKey = 5f;

	private List<GameObject> go_Objects = new List<GameObject>();

	//������������X�������
	public float distance = 10;

	//�Ƕ�ƽ��
	public float f_PingHua_Big = 10;
	public float f_PingHua_Small = 5;

	//λ��ƽ��
    public float f_PingHua_Pos_Big = 5;
	public float f_PingHua_Pos_Small = 2;

	//������Ƕȶ�Ӧ����С�������
	public float minDistance = 2;

	//����������ұ߽�ļ�϶
	public float f_Gap = 0.5f;
	//������ζ�����
	public bool MainShowKey = false;

	//bool b_Shake = false;

    private Camera MainCamera;
	//����ȫ�ִ��ڱ��
	public static BanCameraControlle_XY Instance;

	public void AddRole(CRLuo_PlayAnim_FX aCRLuo_PlayAnim_FX)
	{
        go_Objects.Add(aCRLuo_PlayAnim_FX.mainMeshRender);
	}

	//�Ƴ���ɫ�������
	public void RemoveRole(CRLuo_PlayAnim_FX aCRLuo_PlayAnim_FX)
	{
        go_Objects.Remove(aCRLuo_PlayAnim_FX.mainMeshRender);
	}

	//�����Գ���֮ǰ����
	void Awake() {
		//�����ڱ�Ǹ�ֵ
		Instance = this;

        MainCamera = GetComponent<Camera>();
	}

	//��ʼ����
	void Start()
	{
		//����Ĭ����������
		GoDefault_Key = true;
	}

	//ÿһ֡����
	void Update()
	{
		//���û�н�ɫ������λ����
		if (go_Objects.Count == 0 || GoDefault_Key)
		{
			//λ�ø�λ
			GoDefault();
		}
		else
		{
			//������ģ�������������µ�λ�ü���

			//���ΪZ���
			float left = float.MaxValue;
			//�ұ�ΪZ��С
			float right = float.MinValue;
			//����ΪY��С
			float up = float.MinValue;
			//����ΪY���
			float down = float.MaxValue;

			//������������
			foreach (GameObject aGo in go_Objects)
			{
				//������ ����  ��ǰ����ģ����������.z - ��ǰ������.z 
                if (left > aGo.renderer.bounds.center.x - aGo.renderer.bounds.extents.x)
				{
					//��߼�¼z��Сֵ
                    left = aGo.renderer.bounds.center.x - aGo.renderer.bounds.extents.x;
				}

				//������ С��  ��ǰ����ģ����������.z + ��ǰ������.z 
                if (right < aGo.renderer.bounds.center.x + aGo.renderer.bounds.extents.x)
				{
					//�ұ߼�¼z���ֵ
                    right = aGo.renderer.bounds.center.x + aGo.renderer.bounds.extents.x;
				}

				//����ϱ� ����  ��ǰ����ģ����������.y + ��ǰ����߶�.y 
                if (up < aGo.renderer.bounds.center.y + aGo.renderer.bounds.extents.y)
				{
					//�ϱ߼�¼y���ֵ
                    up = aGo.renderer.bounds.center.y + aGo.renderer.bounds.extents.y;
				}

				//����ϱ� ����  ��ǰ����ģ����������.y + ��ǰ����߶�.y 
                if (down > aGo.renderer.bounds.center.y - aGo.renderer.bounds.extents.y)
                {
					//�±߼�¼y��Сֵ
                    down = aGo.renderer.bounds.center.y - aGo.renderer.bounds.extents.y;
				}

			}


			//-------------------Fov-------------------
			//�������������
			bool b_InMinDistance;
			//ģ�Ϳ�ȷ�Χ��a����Խ�ֱ�߳���
			float a = (right - left) / 2;


			//���aС��������
			if (a < minDistance / 2)
			{
				//a�̶�Ϊ�������
				a = minDistance / 2;
				//�������������
				b_InMinDistance = true;
            } else {
				//����ر����������
				b_InMinDistance = false;
			}

			if (a > CameraFOVAddHeight_LongKey)
			{
				float h = (up - down) / 2 + CameraFOVAddHeight;

				if (a < h)
				{
					a = h;
				}
			}

			//ģ�Ϳ�ȷ�Χ ���� ����������ұ߽�ļ�϶
			a += f_Gap;

			//���㵱ǰ�Ļ����
			a = a * Screen.height / Screen.width;

			//��¼���������ɫ���� b�����ٽ�ֱ�߳�
			float b = distance;

			//�������� = tan(�Ա߱��ٱ�)*����*180������ת�Ƕ�*2���н�
			float targetFov = (Mathf.Atan(a / b) * 180 / Mathf.PI * 2);

            if (b_InMinDistance) {
                //���뼫�޾���
                MainCamera.fieldOfView = Mathf.Lerp(MainCamera.fieldOfView, targetFov, Time.deltaTime * f_PingHua_Small);
            } else {
                //������ڼ���
                //����½�����ڵ�ǰ����
                if (targetFov > MainCamera.fieldOfView) {
                    //�����ʹ����Զ�ٶȸı佹��
                    MainCamera.fieldOfView = Mathf.Lerp(MainCamera.fieldOfView, targetFov, Time.deltaTime * f_PingHua_Big);
                } else {
                    //�����ʹ�������ٶȸı佹��
                    MainCamera.fieldOfView = Mathf.Lerp(MainCamera.fieldOfView, targetFov, Time.deltaTime * f_PingHua_Small);
                }
            }
			

			//-------------------Pos-------------------

			//�����������λ�ã�Ĭ�Ͼ��룬�����м�ֵ�������м�ֵ��
			Vector3 targetPos = new Vector3((left + right) / 2, (up + down) / 2, distance);
    		//����¶�λ�߶�С����С�߶�
            if (targetPos.y < CameraMinHeight) {
				//��λΪ��С�߶�
				targetPos.y = CameraMinHeight;
			}

			//������뼫�޾���
            if (b_InMinDistance) {
                //ʹ������λ��
                this.transform.position = Vector3.Lerp(this.transform.position, targetPos, Time.deltaTime * f_PingHua_Pos_Small);
            } else {
				//ʹ�ÿ���λ��
                this.transform.position = Vector3.Lerp(this.transform.position, targetPos, Time.deltaTime * f_PingHua_Pos_Big);

                if( Mathf.Abs(targetPos.z) <= 1.2f) {
                    transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
                }

			}
		}
	}



	//����Ĭ��λ�ó���
    void GoDefault() {
		this.gameObject.transform.position = Default_Pos;
		this.gameObject.transform.rotation = Quaternion.Euler(Default_Rot);
        GoDefault_Key = false;
	}

//	void OnGUI() {
//
//		if (MainShowKey)
//		{
//
//			if (b_Shake)
//			{
//				if (GUI.Button(new Rect((Screen.width - 100) / 2, Screen.height / 6f * 5f, 100, Screen.height / 6), "no shake"))
//				{
//					b_Shake = !b_Shake;
//				}
//			}
//			else
//			{
//				if (GUI.Button(new Rect((Screen.width - 100) / 2, Screen.height / 6f * 5f, 100, Screen.height / 6), "shake"))
//				{
//					b_Shake = !b_Shake;
//				}
//			}
//		}
//		
//	}

}
