using UnityEngine;
using System.Collections;
using System.Text;

public class UICityFloor : MonoBehaviour 
{
    public UILabel chapterName;
    public UILabel floorName;

    public UITexture texture;
	
    public Texture mTex{get;set;}
    //bool first = true;
//    Texture def;
	public UIScrollView _scrollView;
	public UISprite LoadImgProgress;
	
	
//    public void SetTexture(Texture _tex)
//    {
//        if(_tex == null)
//        {
//			texture.mainTexture = null;			
//            return;
//        }
//        else if(texture.mainTexture!=_tex)
//        {
//			StartCoroutine(SetPic(_tex));
//        }
//    }

//    public void SetDefTexture()
//    {
//        SetTexture(def);
//    }

    public void SetChapterName(string _name)
    {
		if(chapterName!=null)
        chapterName.text = _name;
    }

    public void SetFloorName(string _name)
    {
		if(floorName!=null)
        floorName.text = _name;
    }

    public float switchTexTime = 0.2f;
    public float switchMinAlpha = 0.3f;
    void StartSetTexture()
    {
        TweenAlpha.Begin(texture.gameObject, switchTexTime, switchMinAlpha);
		Invoke("SwitchTextureImme", switchTexTime);
		
    }
	
	//等到假loading跑完,再去做图片赋值
//	IEnumerator SetPic(Texture tex)
//	{
//		while(!isCanSetPic)
//		{
//			yield return 0;
//		}
//		mTex = tex;
//		SwitchTextureImme();
//	}

//	void SwitchTextureImme()
//	{
//		//StartCoroutine(SwitchTexture());
//		SwitchTexture();
//	}
	
	void SetPic()
    {
//		if(mTex == null)
//        {
//			texture.mainTexture = null;			
//        }
//        else 
		if(texture.mainTexture != mTex)
        {
            Texture oldTexture = texture.mainTexture;
			
            texture.mainTexture = mTex;
			texture.MakePixelPerfect();
			_scrollView.ResetPosition();
			
//            if(first)
//            {
//                first = false;
//                def = oldTexture;
//            }

            TweenAlpha.Begin(texture.gameObject, switchTexTime, 1f);
			oldTexture = null;
        }
    }
	
	
	float progressValue = 1f;
	bool isRunProgress = false;
	public bool isImgDone {get;set;}
	float finishValue = 0;
    // isCanSetPic = false;
	
	
	void Update()
	{
		if(isRunProgress)
		{
			finishValue = isImgDone?0:0.2f;			
			if(progressValue > finishValue)
			{
				progressValue-=0.03f;
				texture.alpha = progressValue;
			}
			
			if(progressValue<=0)
			{
				progressValue = 0;
				texture.alpha = progressValue;
				
				isImgDone = false;
				//isCanSetPic = true;
				SetPic();
				isRunProgress = false;
			}
			LoadImgProgress.fillAmount = progressValue;
		}
	}
	
	public void RunProgress()
	{
		progressValue=1f;
		LoadImgProgress.fillAmount = progressValue;
		isRunProgress = true;
	}
	
	

	void releaseTexture() {
		Object[] texs = Resources.FindObjectsOfTypeAll(typeof(Texture));

		//Debug.Log("to Release Texture " + texs.Length);
		printOut(texs);

//		if(texs != null) {
//			foreach(var at in texs)
//				if(at.name == "Main")
//					Resources.UnloadAsset(at);
//		}
	}

	void printOut(Object[] objs) {
		if(objs != null) {
			StringBuilder sb = new StringBuilder();
			foreach(var item in objs) {
				sb.Append(item.ToString() + " : " + item.name + "\n");
			}
			Debug.Log(sb.ToString());
		}
	}
}

