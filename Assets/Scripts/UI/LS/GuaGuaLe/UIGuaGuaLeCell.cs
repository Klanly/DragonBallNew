using UnityEngine;
using System.Collections;

public class UIGuaGuaLeCell : RUIMonoBehaviour 
{
	public UISprite _ReverseIcon;
	public UISprite _Icon;
	public UILabel _Stone;
	public UISprite _Choose;
	public GameObject obj1;
	public GameObject obj2;

	[HideInInspector]
	public int _StoneNum;
	[HideInInspector]
	public bool isAround;

	private int _HeadId;
	private bool IsOpen;
	private bool IsSetHead;
	private bool isAlphsUp;
	private bool IsChooseAni;

	private float mAngle = 0.0f;
	private float mAlpha = 1.0f;


	public void OnReset()
	{
		gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0f,0f,0f));
		if(obj1 != null && obj2 != null)
		{
			obj1.gameObject.SetActive(true);
			obj2.gameObject.SetActive(false);
			_Choose.gameObject.SetActive(false);
		}

		SetGuaGuaColor(true);

		IsOpen = false;
		isAround = false;
		IsSetHead = false;
		IsChooseAni = false;
		isAlphsUp = false;
		mAngle = 0.0f;
		mAlpha = 1.0f;
	}

	public void SetGuaGuaColor(bool isgray)
	{
		if(isgray)_ReverseIcon.color = Color.black;
		else _ReverseIcon.color = Color.white;
	}

	public void SetHead(int m_HeadId)
	{
		_HeadId = m_HeadId;
		_StoneNum = Core.Data.guaGuaLeMgr.GetStone(m_HeadId);
    }
    
    void OpenHead()
	{
		if(obj1 != null)
		{
			obj1.gameObject.SetActive(false);
			obj2.gameObject.SetActive(true);
			_Stone.text = _StoneNum.ToString();
			AtlasMgr.mInstance.SetHeadSprite(_Icon, _HeadId.ToString());
		}
	}

	public void BeginAnimat()
	{
		IsChooseAni = true;
		_Choose.gameObject.SetActive(true);
		Invoke("HideChooseAni", 1.8f);
	}

	void HideChooseAni()
	{
		IsChooseAni = false;
		_Choose.gameObject.SetActive(false);
	}
	
	void Update()
	{
		if(isAround)
		{
			mAngle += 5.0f;
            if(mAngle >=90)
			{
				if(!IsSetHead)
				{
					mAngle += 180f;
					OpenHead();
					IsSetHead = true;
                }
            }
			if(mAngle >= 360.0f)
			{
				mAngle = 360.0f;
				isAround = false;
				UIGuaGuaLeMain.GetInstance().BeginChooseAnimation();
			}

			gameObject.transform.localRotation = Quaternion.Euler (new Vector3(gameObject.transform.localRotation.x,mAngle,gameObject.transform.localRotation.z));
        }
		if(IsChooseAni)
		{
			if(!isAlphsUp)mAlpha-=0.06f;
			else mAlpha+=0.06f;
			if(mAlpha >= 1.0f)
			{
				mAlpha = 1.0f;
				isAlphsUp = false;
			}
			else if(mAlpha <= 0.0f)
			{
				mAlpha = 0.0f;
				isAlphsUp = true;
            }
			_Choose.color = new Color(1f,1f,1f,mAlpha);
        }
    }

	void OnPress()
	{
		if(UIGuaGuaLeMain.GetInstance().m_IsBeginClick)
		{
			if(!IsOpen)
			{
				Core.Data.soundManager.SoundFxPlay(SoundFx.FX_Diamonds);
				UIGuaGuaLeMain.GetInstance().GuaGuaTotalNum++; 
				isAround = true;
				IsOpen = true;
			}
		}

    }
}
