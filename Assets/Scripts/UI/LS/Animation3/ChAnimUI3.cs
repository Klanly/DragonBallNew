using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChAnimUI3 : RUIMonoBehaviour
{
	public System.Action OnFinished;

	public List<UISprite> m_AnimatinSp;
	public UISprite m_Bg;

	static ChAnimUI3 mInstance = null;
	public static ChAnimUI3 GetInstance()
	{
		return  mInstance;
	}

	public static ChAnimUI3 OpenUI(Transform tfParent)
	{
		Object obj = PrefabLoader.loadFromPack("LS/pbChAnim3");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			if(go != null)
			{
				RED.AddChild(go, tfParent.gameObject);
				mInstance = go.GetComponent<ChAnimUI3>();
			}
		}
		return mInstance;
	}

	void Start()
	{
		Step1();
		Invoke("Step2", 0.6f);
		Invoke("Step3", 1.2f);
		Invoke("Step4", 1.8f);
		Invoke("Step5", 2.7f);
		Invoke("Step6", 3.5f);
		Invoke("Step7", 4.3f);
		Invoke("Step8", 6.5f);
	}

	void Step1()
	{
		MiniItween.ColorTo(m_AnimatinSp[0].gameObject, new V4(Color.white), 0.5f, MiniItween.Type.ColorWidget);
	}

	void Step2()
	{
		MiniItween.ColorTo(m_AnimatinSp[1].gameObject, new V4(Color.white), 0.5f, MiniItween.Type.ColorWidget);
	}

	void Step3()
	{
		MiniItween.MoveTo(m_AnimatinSp[2].gameObject, new Vector3(182f,201f,0f), 0.3f);
	}

	void Step4()
	{
		MiniItween.ColorTo(m_AnimatinSp[3].gameObject, new V4(Color.white), 0.2f, MiniItween.Type.ColorWidget);
		MiniItween _tween = MiniItween.ScaleTo(m_AnimatinSp[3].gameObject, new Vector3(0.8f,0.8f,0.8f), 0.2f);
		_tween.myDelegateFunc += Step4_subsidiary;
	}

	void Step4_subsidiary()
	{
		MiniItween.ScaleTo(m_AnimatinSp[3].gameObject, new Vector3(1f,1f,1f), 0.1f);
		MiniItween.Shake(m_AnimatinSp[0].gameObject, new Vector3(0.1f,0.1f,0.1f), 0.1f, MiniItween.EasingType.EaseInCubic);
		MiniItween.Shake(m_AnimatinSp[1].gameObject, new Vector3(0.1f,0.1f,0.1f), 0.1f, MiniItween.EasingType.EaseInCubic);
	}

	void Step5()
	{
		MiniItween.MoveTo(m_AnimatinSp[4].gameObject, new Vector3(181f,-11.4f,0f), 0.3f);
	}

	void Step6()
	{
		MiniItween.MoveTo(m_AnimatinSp[5].gameObject, new Vector3(181f,-206f,0f), 0.3f);
	}

	void Step7()
	{
		MiniItween.MoveTo(m_AnimatinSp[6].gameObject, new Vector3(464f,4f,0f), 1.1f);
		MiniItween.ColorTo(m_AnimatinSp[6].gameObject, new V4(Color.white), 1.1f, MiniItween.Type.ColorWidget);
	}

	void Step8()
	{
		MiniItween _tween = MiniItween.ColorTo(m_Bg.gameObject, new V4(Color.white), 0.6f, MiniItween.Type.ColorWidget);
		_tween.myDelegateFunc += AtLastCancel;
	}

	void AtLastCancel()
	{
		if(OnFinished != null)
		{
			OnFinished();
			OnFinished = null;
		}
		this.dealloc();
	}

	void OnDestroy()
	{
		mInstance = null;
	}

}
