using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//yangchengaung 
public class RouletteLogic : MonoBehaviour 
{

    public static RouletteLogic _self ;

    public UIButton startBtn; //开始按钮

    public UISprite activityAward;
    public Spin sp ; // 旋转的速度
    public CRLuo_DeleteOBJ CRLuo_DeleteOBJ_1;
	// Use this for initialization
    //private int _activity = 0 ;
    static UnityEngine.Object obj;
    public UIButton closeBtn;
    private string strSM= "yaoyaole#90021#90027,xingyunzhuanlun#9071#90028,caicaikan#90030#90029" ; //说明配置 Act_11是雷达Act_11#7363#90026,
    public UIGrid uiGrid;

    public List<UISprite> listSP  = new List<UISprite>(); 
    public List<RouletteItem> listPosi  ;
    int count =0 ;
    float time  =1.5f ;
    float timeDelay = 0.03f;

    void Awake()
    {
        _self = this ; 
    }
	void Start ()
    {
        sp.rotationsPerSecond =new Vector3 (0,0,0);
        initSM();

        initItemSpeed();

      

	}
    private void  initItemSpeed()
    {
        listPosi = new List<RouletteItem>();

        //string strXY = "" ;

        count = (int)(time / timeDelay) ;
        float valuex  =0 ; 
        float valuey  =0 ; 

        for(int i =0  ; i < listSP.Count ; i++)
        {

            valuex =  -(listSP[i].transform.localPosition.x /count ); 

            valuey = -(listSP[i].transform.localPosition.y /count) ; 
           
            RouletteItem ri = new RouletteItem() ;
            ri.valueX = valuex ; 
            ri.valueY = valuey ;
            listPosi.Add(ri);
        }

    }
    // 初始化说明
    private void initSM()
    {
        string[] str = strSM.Split(',');
        for (int i =0 ;  i < str.Length; i++)
        {
            string[] strPro = str[i].Split('#');
            obj  = PrefabLoader.loadFromPack("YCG/RouletteShuoming");
            if(obj != null)
            {
                GameObject go = Instantiate(obj) as GameObject;
                ExplainLogic cc = go.GetComponent<ExplainLogic>();
                cc.Init(strPro[0],strPro[1],strPro[2]);
                RED.AddChild (go , uiGrid.gameObject );
            }

        }
       // uiGrid.repositionNow = true ; 
        uiGrid.gameObject.SetActive(true);
    }
    public static RouletteLogic CreateRouleView()
    {
    

        if (obj !=  null ) return null;
         obj  = PrefabLoader.loadFromPack("YCG/Roulette");

        if(obj != null)
        {
            GameObject go = Instantiate(obj) as GameObject;
            RouletteLogic cc = go.GetComponent<RouletteLogic>();

            RED.AddChild (go , DBUIController.mDBUIInstance._bottomRoot );
          
            return cc;
        }
        return null;
    }

   


    //打开活动  1 :超级大奖 (幸运转盘)，2：天神恩赐，（摇摇乐）3：刮刮乐，（猜猜看）4，组队副本(雷达)
    public  void openActivity(int acType  )
    {
        switch(acType)
        {
        case 1:
			DBUIController.mDBUIInstance.HiddenFor3D_UI();
            UIBigWheel.OpenUI();
			Destroy(gameObject);
			CancelInvoke("showActivityAward");
			return; 
        case 2:
			RollGambleController.CreatRollGamblePanel ();
            break ;
        case 3:
			UIGuaGuaLeMain.OpenUI();
			DBUIController.mDBUIInstance.HiddenFor3D_UI();
			Destroy(gameObject);
			CancelInvoke("showActivityAward");
			return;
		case 4:
			OpenRadarTeamUI ();
            break ;
        }

		OnCloseBtnClick();
    }



	void OpenRadarTeamUI()
	{
		DBUIController.mDBUIInstance.SetViewState (RUIType.EMViewState.S_HuoDong);
		ActivityNetController.curWaitState = 3;
	}



    //旋转逻辑
    IEnumerator startPosition()
    {

        while(count > 0)
        {
            yield return new WaitForSeconds(timeDelay) ;
            count--  ;

            for(int i =0  ; i < listSP.Count ; i++)
            {
               
                listSP[i].transform.localPosition += new Vector3( listPosi[i].valueX, listPosi[i].valueY,0)  ;
            }

        }

        if (count <=0)
        {
            for(int i =0  ; i < listSP.Count ; i++)
            {
                listSP[i].gameObject.SetActive(false);
            }

            sp.rotationsPerSecond =new Vector3 (0,0,0);
            Core.Data.soundManager.SoundFxPlay(SoundFx.FX_Building_Done,null);
            CRLuo_DeleteOBJ_1.gameObject.SetActive(true);
            Invoke("showActivityAward" ,2.5f);

        }
    }
    // 设置奖品类型  活动编号，1:超级大奖 (幸运转盘)，2：天神恩赐，（摇摇乐）3：刮刮乐，（猜猜看）4，组队副本(雷达)
    public void setActivityType(int type)
    {
        if (activityAward == null  ||  activityAward.gameObject == null ) {
        
            return ;
        }
        UISprite  name =  activityAward.gameObject.transform.FindChild("name").gameObject.GetComponent<UISprite>();
        switch(type)
        {
        case 1:
            activityAward.spriteName = "xingyunzhuanlun" ; 
            name.spriteName = "xingyunzhuanlun" ;
            name.height  =  33;
            name.width   = 113 ; 
            break ; 
        case 2:
            activityAward.spriteName = "yaoyaole" ;
            name.spriteName = "yaoyaole" ;
            name.height = 33 ; 
            name.width = 87 ; 

            break ; 
        case 3:
            activityAward.spriteName = "caicaikan" ;
            name.spriteName = "caicaikan" ;
            name.height = 32 ; 
            name .width = 87 ; 
            break ;
        case 4:
            activityAward.spriteName = "Act_11" ;
            name.spriteName = "leidazudui" ;
            name.height = 32 ; 
            name .width = 113; 
            break; 
        }
      
    }
    // 白光粒子特效消失 -- 显示活动奖励
    public void showActivityAward()
    {
         activityAward.gameObject.SetActive(true);
        closeBtn.enabled = true;
		//SQYMainController.mInstance.UpdateWheelState(ActivityManager.activityZPID);

		UIWXLActivityMainController.Instance.UpdateWheelState (ActivityManager.activityZPID);
    }

    //开始旋转
    public void OnBtnClick()
    {
        Core.Data.soundManager.SoundFxPlay(SoundFx.FX_Dragon,null);
        ComLoading.Open();
        sendSer(activityAward_UI,activityAward_Error,1);
        closeBtn.enabled = false;
    }
    // 点击随机出来的图标跳转过去
    public void OnClickActivityBtn()
    {
      
        //活动转盘ID   ActivityManager.activityZPID
        openActivity(ActivityManager.activityZPID );
    }
  
    //派发消息   roll type =1 则 请求转盘   roll type =0   则请求直接看结果
    public static void sendSer(System.Action<BaseHttpRequest, BaseResponse> CallBack,System.Action<BaseHttpRequest,string> CallBackError,int rollType)
    {
        RollActParam param = new RollActParam(int.Parse(Core.Data.playerManager.PlayerID),rollType);
        HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);

        task.AppendCommonParam(RequestType.GET_AWARDACTIVITY,param);

        task.ErrorOccured   +=  CallBackError ;
        task.afterCompleted +=  CallBack;

        task.DispatchToRealHandler ();
       
    }
    //消息异常
    void activityAward_Error(BaseHttpRequest request, string response)
    {
        // Debug.Log("11");
    }
    //消息正方返回
    void activityAward_UI(BaseHttpRequest request, BaseResponse response)
    {
     
        if(response != null && response.status != BaseResponse.ERROR)
        {
            GetAwardActivity re = response as GetAwardActivity;
            if ( re.data.status.id== 0)
            {
                SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(90016));

            }else
            {

                ActivityManager.activityZPID = re.data.status.id ; 
                setActivityType( re.data.status.id);
                StartCoroutine("startPosition");

                startBtn.gameObject.SetActive(false);
                sp.rotationsPerSecond = new Vector3 (0,0,-1.25f);

            }
           
        } 
        else if(response != null && response.status == BaseResponse.ERROR)
        {

            SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(90016));

         
        }

        ComLoading.Close();
    }
  

    public void OnCloseBtnClick()
    {
        DBUIController.mDBUIInstance.ShowFor2D_UI();

        Destroy(gameObject);
        CancelInvoke("showActivityAward");
    }


   

    void OnDestroy()
    {
        if (listSP != null)
        {
            listSP.Clear();
            listSP=null ; 
        }

        if (listPosi != null)
        {
            listPosi.Clear();
            listPosi= null;
        }

        startBtn = null ; //开始按钮
        activityAward= null;
        sp = null; // 旋转的速度
        CRLuo_DeleteOBJ_1= null;
        obj= null ;
        _self =null;

    }
	
}
public class RouletteItem
{
    public float valueX; 
    public float valueY;
}