using UnityEngine;
using System.Collections;

public class PVEDownloadCartoonController : MonoBehaviour
{
    private static PVEDownloadCartoonController instance;

    public static PVEDownloadCartoonController Instance
    {
        get
        {
            return instance;
        }
    }

    public static PVEDownloadCartoonController CreateCartoonPanel(NewFloorData tNewFD,bool isShow = false)
    {
        if (instance == null)
        {
            UnityEngine.Object obj = WXLLoadPrefab.GetPrefab(WXLPrefabsName.UIDownloadCartoonPanel);
            if (obj != null)
            {
                GameObject go = Instantiate(obj) as GameObject;
				DBUIController.mDBUIInstance._PVERoot.AddPage(go.name,go);
                PVEDownloadCartoonController fc = go.GetComponent<PVEDownloadCartoonController>();
                instance = fc;
                Transform goTrans = go.transform;
				go.transform.parent = DBUIController.mDBUIInstance._PVERoot.transform;
                go.transform.localPosition = new Vector3(0, -580, 0);
                goTrans.localScale = Vector3.one;
                fc.curFD = tNewFD;
                if (isShow == true)
                    fc.OpenCartoonPanel();
                return fc;
            }
            return null; 

        }
        else
        {
            RED.SetActive(true,instance.gameObject);
			instance.transform.parent = DBUIController.mDBUIInstance._PVERoot.transform;
            instance.transform.localPosition = new Vector3(0, -580, 0);
            instance.transform.localScale = Vector3.one;
            instance.curFD = tNewFD;
            instance.Start();
            if (isShow == true)
                instance.OpenCartoonPanel();
            return instance;
        }

    }



    Vector3 outPos = new Vector3(0, -580, 0);
    float outTime = 0.5f;
    public float switchTexTime = 0.2f;

    public Texture l_Tex{ get; set; }

    public Texture r_Tex{ get; set; }

    public UITexture L_texture;
    public UITexture R_texture;
    public UIScrollView _scrollView;
    public UISprite LoadImgProgress;
    public UILabel lbl_floorName;

    public UIButton btn_Hide;
    string wantTexName_L = "";
    string wantTexName_R = "";
    float progressValue = 1f;
    bool isRunProgress = false;

    public bool isImgDone_L { get; set; }

    public bool isImgDone_R { get; set; }

    private bool isShow = false;
    public bool curShowStatus{ get{ return isShow;}}
    float finishValue = 0;
    NewFloorData curFD = null;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
//		Debug.Log (" cartoonnnnn   === " + LuaTest.Instance.OpenCartoon);
		if (LuaTest.Instance.OpenCartoon == true || Core.Data.guideManger.isGuiding) {
			if (Core.Data.usrManager.UserConfig.cartoon == 1)
				this.PrepareTex ();
		} 
    }




    public void OpenCartoonPanel(bool isDirectly = false)
    {
		if (LuaTest.Instance.OpenCartoon || Core.Data.guideManger.isGuiding) {
			if (instance != null) {
				MoveToTarget (true, isDirectly);
				isShow = true;
				if (Core.Data.usrManager.UserConfig.cartoon == 1)
					ShowPicFromA ();
				else
					PrepareTex ();
			}
		}
    }

   
    public void HideDownBtn(bool isHide){
        btn_Hide.gameObject.SetActive(isHide);
    }


    public void CloseCartoonPanel(bool isDirectly = false)
    {
        if (instance != null)
        {
            MoveToTarget(false,isDirectly);
            isShow = false;
            HideDownBtn(true);

            Invoke("ShowAlpha",outTime);

			Resources.UnloadUnusedAssets ();

            if(JCPVEPlotDes.Instance == null) 
				if(JCPVEPlotController.Instance != null)
                	JCPVEPlotController.Instance.Lab_Title.transform.parent.gameObject.SetActive(true);
        }
    }

    void HideBtn(){
        this.CloseCartoonPanel();
    }

    void Update()
    {
        if(isRunProgress)
        {
            finishValue = (isImgDone_R && isImgDone_L)?0:0.1f; 

            if(progressValue > finishValue)
            {
                progressValue -= 0.03f;
                L_texture.alpha = progressValue;
                R_texture.alpha = progressValue;
            }
            if (progressValue <= 0)
            {

                //   Debug.Log( "progress value " + progressValue  + " L = " +isImgDone_L + " R = " + isImgDone_R   );
                if (isImgDone_L && isImgDone_R)
                {
                    SetPic();
                    isRunProgress = false;
                    TweenAlpha.Begin(L_texture.gameObject, 0.3f, 1f);
                    TweenAlpha.Begin(R_texture.gameObject, 0.3f, 1f);
                    progressValue = 0;
                }
            }

            LoadImgProgress.fillAmount = progressValue;
        }
    }

	bool isRunSecond = true;

    void MoveToTarget(bool tMoveOut,bool isDirectly = false)
    {
        if (tMoveOut)
        {
			if (isDirectly == false){
				if (Core.Data.guideManger.isGuiding) {
					if (isRunSecond == true) {
						MiniItween.MoveTo (this.gameObject, Vector3.zero, outTime, MiniItween.EasingType.Pow);//.myDelegateFunc += RunGuide;
						AsyncTask.QueueOnMainThread (RunGuide, 2f);
						isRunSecond =false;
					}
				}else{
					MiniItween.MoveTo(this.gameObject, Vector3.zero, outTime, MiniItween.EasingType.Pow);
				}
			}else 
                gameObject.transform.localPosition = Vector3.zero;


//				if (Core.Data.guideManger.isGuiding) {
//					UIGuide.Instance.DelayAutoRun (1f);
//					NGUIDebug.Log (" delay  auto  run  in  moveTarget");
//				}

        }
        else
        {
            if (isDirectly == false)
                MiniItween.MoveTo(this.gameObject, outPos, outTime, MiniItween.EasingType.Pow);
            else
            {
                gameObject.transform.localPosition = outPos;
            }

            if(Core.Data.guideManger.isGuiding){
                Core.Data.guideManger.DelayAutoRun(1f);
            }
        }
    }

	void RunGuide(){
		if (Core.Data.guideManger.isGuiding) {
			//UIGuide.Instance.DelayAutoRun (1f);
			Core.Data.guideManger.AutoRUN ();
		}
	}
  

    void ShowAlpha( ){
        if (isShow == false)
        {
            TweenAlpha.Begin(L_texture.gameObject, switchTexTime, 0f);
            TweenAlpha.Begin(R_texture.gameObject, switchTexTime, 0f);
        }
    }

    void ShowPicFromA(){
		if (isShow == true)
        {
            TweenAlpha.Begin(L_texture.gameObject, switchTexTime, 1f);
            TweenAlpha.Begin(R_texture.gameObject, switchTexTime, 1f);
        }
    }
        

    void SetPic()
    {
        if (curFD != null)
        {
            lbl_floorName.text = curFD.name;
            if (L_texture.mainTexture != l_Tex && R_texture.mainTexture != r_Tex)
            {

//                Texture old_L_Texture = L_texture.mainTexture;
//                Texture old_R_Texture = R_texture.mainTexture;

                L_texture.mainTexture = l_Tex;
                R_texture.mainTexture = r_Tex;

                L_texture.width = 530;
                L_texture.height = 776;
                R_texture.width = 530;
                R_texture.height = 776;

                _scrollView.ResetPosition();

                TweenAlpha.Begin(L_texture.gameObject, switchTexTime, 1f);
                TweenAlpha.Begin(R_texture.gameObject, switchTexTime, 1f);

//                old_L_Texture = null;
//                old_R_Texture = null;
            }
        }
    }
    //准备展示图片
    void PrepareTex()
    {

        string[] tempPic = curFD.cartoon;
        this.isImgDone_L = false;
        this.isImgDone_R = false;

        if (tempPic.Length > 0)
        {
            if (curFD != null)
            {
                this.ShowTex(1, tempPic[0]+ ".jpg");
                this.ShowTex(2, tempPic[1] + ".jpg");
            }
        }
       
    }


    public void ClosePanel()
	{
        Destroy(gameObject);
		instance = null;
    }

    #region 下载 或者 加载本地

    // type = 1   L    type =2 R
    public void ShowTex(int type, string _name)
    {

        _name = FloorTextureManager.GetFileName(_name);
        if (type == 1)
        {
            if (string.IsNullOrEmpty(_name) || wantTexName_L == _name)
            {
                TweenAlpha.Begin(L_texture.gameObject, switchTexTime, 1f);

                return;
            }
            wantTexName_L = _name;
        }
        else
        {
            if (string.IsNullOrEmpty(_name) || wantTexName_R == _name)
            {
                TweenAlpha.Begin(R_texture.gameObject, switchTexTime, 1f);
                return;
            }
            wantTexName_R = _name;
        }

        UIDownloadTexture dt = new UIDownloadTexture();
        //检查本地是存否在
        if (FloorTextureManager.CheckExist(_name))
        {
            this.RunProgress();        
            if (type == 1)
                dt.EndLoadLocal = EndLoadLeft;
            else
                dt.EndLoadLocal = EndLoadRight;

            StartCoroutine(dt.LoadLocal(_name));
        }
        else
        {
            //从网络下载
//            if (Core.Data.usrManager.UserConfig.cartoon == 1)
//            {
                this.RunProgress();
                if (type == 1)
                {

                    dt.EndDownload = EndDownloadLeft;
                    dt.DownloadError = DownloadError_L;
                }
                else
                {
                    dt.EndDownload = EndDownloadRight;
                    dt.DownloadError = DownloadError_R;
                }
                StartCoroutine(dt.Download(_name));
//            }
//            else
//            {
//                if (type == 1)
//                    EndDownloadLeft(null, "");
//                else
//                    EndDownloadRight(null, "");
//            }
        }

		Resources.UnloadUnusedAssets ();
    }
    //加载本地图片完成   L
    void EndLoadLeft(Texture _tex)
    {
        this.l_Tex = _tex;
        this.isImgDone_L = true;
    }
    //下载网络图片完成   L
    void EndDownloadLeft(Texture _tex, string _name)
    {

        this.l_Tex = _tex;
        this.isImgDone_L = true;
    }
    //加载本地图片完成  R
    void EndLoadRight(Texture _tex)
    {
        this.r_Tex = _tex;
        this.isImgDone_R = true;
    }
    //下载网络图片完成  R
    void EndDownloadRight(Texture _tex, string _name)
    {
        //下载网络图片完成
        this.r_Tex = _tex;
        this.isImgDone_R = true;
    }
    //下载失败
    void DownloadError_L(string name)
    {

        //原来 处理是展示上一张漫画
        //现在处理是 一直下加载
        
        ShowTex(1,name);
//        int index = name.IndexOf('-');
//        if (index > 0)
//        {
//            int num = System.Convert.ToInt32(name.Substring(index + 1, 1));
//            if (num > 1)
//            {
//                name = name.Replace("-" + num.ToString(), "-" + (--num).ToString());
//                ShowTex(1, name);
//            }           
//        }
    }

    void DownloadError_R(string name)
    {
        ShowTex(1,name);
//        int index = name.IndexOf('-');
//        if (index > 0)
//        {
//            int num = System.Convert.ToInt32(name.Substring(index + 1, 1));
//            if (num > 1)
//            {
//                name = name.Replace("-" + num.ToString(), "-" + (--num).ToString());
//                ShowTex(2, name);
//            }           
//        }
    }

	bool isRunFrist = true;
    public void RunToBottom(){

		if (!isRunFrist)
			return;
		isRunFrist = false;
        SpringPanel.Begin(_scrollView.gameObject, new Vector3(0, 156, -101), 8).onFinished = () =>
		{
			if(Core.Data.guideManger.isGuiding){
				Core.Data.guideManger.DelayAutoRun(0.2f);
//				Debug.Log(" is finish  ????????????    ");
		    }
		};

    }
    public void RunProgress()
    {
        progressValue = 1f;
        LoadImgProgress.fillAmount = progressValue;
        isRunProgress = true;
    }

    #endregion


}
