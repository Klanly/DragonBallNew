using UnityEngine;
using System.Collections;

//制作人:CRomputer-罗
//发布群:121463857

public class CRLuo_ParMat_Flicker_Advanced : MonoBehaviour
{
	//当前透明度亮度值
	private float Intensity = 0;
	//显示标记位
	private float State = 0;
	//闪烁速率
	private float speed = 0;
	//随机最亮值
	private float Min = 0;
	private float Max = 0;
	//材质球颜色暂存
	private float ColorR = 0;
	private float ColorG = 0;
	private float ColorB = 0;

	private Color clr;

	//关闭过程标记位
	private bool MainKeyTF;
	//关闭时暂存亮度
	private float NowIntensity = 0;

	public string _ = "-=<多层级粒子材质闪烁程序_高级版>=-";
	public string __ = "闪烁对象模型(有MeshRenderer对象)";
	public Renderer myRender;
	public string ___ = "材质球ID";
	public int ElementID = 0;
	public string ____ = "设置颜色开关";
	public bool SetColor_Key;
	public string _____ = "颜色";
	public Color myColor;
	public string ______ = "闪烁开始时间";
	public float StartTime = 0;
	public string _______ = "每次闪烁时长";
	public float OneFlickerTime = 1;
	public string ________ = "最大亮度";
	public float FlickerMax = 1;
	public string _________ = "-------高级随机闪烁开关--------";
	public bool OnAdvancedFlicker;
	public string __________ = "关闭时间";
	public float OffTimeMin = 0;
	public float OffTimeMax = 10;
	public string ___________ = "变亮时间";
	public float UpTimeMin = 0;
	public float UpTimeMax = 0.5f;
	public string ____________ = "开启时间";
	public float OnTimeMin = 0;
	public float OnTimeMax = 1;
	public string _____________ = "变暗时间";
	public float DownTimeMin = 0;
	public float DownTimeMax = 1;
	public string ______________ = "亮度下限";
	public float IntensityMin = 0;
	public string _______________ = "随机下限";
	public bool IntensityMinRand;
	public string ________________ = "亮度上限";
	public float IntensityMax = 1;
	public string _________________ = "随机上限";
	public bool IntensityMaxRand;

	public string __________________ = "外部灯光连接";
	public Light light;
	public string ___________________ = "灯光亮度微调";
	public float lightIntensityScle = 1;
	public string ____________________ = "闪烁主开关";
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
			//保存颜色
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

		//主开关
		if (MainKey)
		{
			//开始延时
			if (StartTime > 0)
			{
				StartTime -= Time.deltaTime;
				return;
			}


			//高级开关
			if (OnAdvancedFlicker)
			{
				//调用高级闪烁
				StartCoroutine(AdvancedFlicker());
			}
			else
			{
//				基本闪烁
				BasisFlicker();
			}

			//关闭渐隐标记
			MainKeyTF = true;

//			NowIntensity = myRender.materials[ElementID].GetColor("_TintColor").a;
			NowIntensity = selMat.GetColor("_TintColor").a;
		}
		else
		{ 
			//开启关闭渐隐
			if(MainKeyTF)
			{
				//关闭时亮度减小
				NowIntensity -= Time.deltaTime;
				//如果彻底关闭
				if (NowIntensity <= 0)
				{
					//亮度归零
					NowIntensity = 0;
					//关闭渐隐
					MainKeyTF = false;
					//高级闪烁状态归零
					State = 0;
					//亮度归零
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
	{//基本闪烁

		//状态0
		if (State == 0)
		{
			//标记为变亮状态
			State = 1;
			//计算闪烁频率
			speed = FlickerMax / OneFlickerTime;
		};
		if (State == 1)
		{
			//增亮累加
			if (Intensity <= FlickerMax)
			{
				//增亮累加
				Intensity += Time.deltaTime * speed;
				//设置材质球灯光亮度
				setLinght();
			}
			else
			{
				//完成增量设置最大亮度
				Intensity = FlickerMax;
				//设置材质球灯光亮度
				setLinght();
				//设置标记位为变暗
				State = -1;
				//设置亮度递减速率
				speed = -FlickerMax / OneFlickerTime * 0.5f;
			}
		};

		//变暗过程
		if (State == -1)
		{
			//变暗累加
			if (Intensity > 0)
			{
				//增亮累加
				Intensity += Time.deltaTime * speed;
				//设置材质球灯光亮度
				setLinght();

			}
			else
			{
				//完成变暗设置最小亮度
				Intensity = 0;
				//设置亮度
				setLinght();
				//设置为关闭状态
				State = 0;

			}
		};
	
	}

	IEnumerator AdvancedFlicker()
	{//高级闪烁
		//yield return null;IEnumerator无返回值时使用
		
		//状态0
		if (State == 0)
		{
			//关闭时程序停止（随机关闭时间）
			yield return new WaitForSeconds(Random.Range(OffTimeMin, OffTimeMax));
			//标记为变亮状态
			State = 1;
			//设置变量速率
			speed = 1 / Random.Range(UpTimeMin, UpTimeMax);
			//获取随机亮度上下限
			rand_MinMax();

		};

		//增亮过程
		if (State == 1)
		{
			//增亮累加
			if (Intensity <= Max)
			{
				//增亮累加
				Intensity += Time.deltaTime * speed;
				//设置材质球灯光亮翘
				setLinght();
			}
			else
			{
				//完成增量设置最大亮度
				Intensity = Max;
				//设置材质球灯光亮度
				setLinght();
				//亮度保持时间
				yield return new WaitForSeconds(Random.Range(OnTimeMin, OnTimeMax));
				//设置标记位为变暗
				State = -1;
				//设置亮度递减速率
				speed = -1 / Random.Range(DownTimeMin, DownTimeMax);
				//获取随机亮度上下限
				rand_MinMax();
			}
		};

		//变暗过程
		if (State == -1)
		{
			//变暗累加
			if (Intensity > Min)
			{
				//增亮累加
				Intensity += Time.deltaTime * speed;
				//设置材质球灯光亮度
				setLinght();

			}
			else
			{
				//完成变暗设置最小亮度
				Intensity = Min;
				//设置亮度
				setLinght();
				//关闭状态保持时间
				yield return new WaitForSeconds(Random.Range(OffTimeMin, OffTimeMax));
				//设置为变亮状态
				State = 1;
				//设置变亮速率
				speed = 1 / Random.Range(UpTimeMin, UpTimeMax);
				//获取随机亮度上下限
				rand_MinMax();
			}
		};


	}

	//随机亮度上下限
	void rand_MinMax()
	{
		//随机亮度下限开关
		if (IntensityMinRand)
		{
			//随机亮度下限（0~Min）
			Min = Random.Range(0, IntensityMin);
		}
		else
		{
			//直接设置最小值
			Min = IntensityMin; 
		};
		//随机亮度上限开关
	      if(IntensityMaxRand)
		{
			//随机亮度下限（Min~Max）
			Max = Random.Range(Min, IntensityMax); }
	      else
		{
			//直接设置最大值
			Max = IntensityMax; 
		};

	}

	//设置亮度
	void setLinght()
	{
		
		//设置透明度 = 限制在0~1
		float Alp = Mathf.Clamp(Intensity,0,1);

		clr.a = Alp;
//		myRender.materials[ElementID].SetColor("_TintColor", new Color(ColorR, ColorG, ColorB, Alp));
//		myRender.materials[ElementID].SetColor("_TintColor", clr);

		selMat.SetColor("_TintColor", clr);
		//设置灯光亮度
		if (light != null)
		{
			light.intensity = Alp * lightIntensityScle;
		
		}
	
	}

}
