using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//yangcg
public class HolidayActivityLogic : MonoBehaviour
{
    public static GameObject instence; 
    public UIGrid _grid ; 
    public ActivityItem at ; 
    public ActivityExplain Ae ; 
	Dictionary<int , NewActivityData > activityInfo = new Dictionary<int, NewActivityData>();
	private NewActivityResponse har;
    List<GameObject> listAct = new List<GameObject>();
    private HolidayActivityData Data;
    string clickActId ="";
	// Use this for initialization
	void Start ()
    {
        Data = Core.Data.HolidayActivityManager;
//        har  = Data.har;
        Invoke("OneHour", 800f);
		ComLoading.Open();
		ActivityLimitTimeRequest();
	}

    void OneHour() {

//        har  = Data.har;
        closeExplainLabel();
        Invoke("OneHour", 800f);
    }
    //当前打开的活动说明界面是否还存在这个活动
    void closeExplainLabel()
    {
        if (Ae.gameObject.activeSelf == false) return ; 
		NewActivityData hai;
        bool isOK = false ;
        if (har != null&& har.data.Length > 0)
        {
            for (int i = 0 ; i < har.data.Length ; i++ ) 
            {
                hai  =  har.data[i];
                if (hai.activityId == clickActId)
                {
                    isOK= true ; 
                }
            }
        }
       
        if (isOK ==false )
        {
            Ae.gameObject.SetActive(false); //活动说明显示界面
        }
    }

    //单例
    public static HolidayActivityLogic _instence() {

        if (instence != null ) return null; 
        Object obj = PrefabLoader.loadFromPack("YCG/HolidayActivity");
        HolidayActivityLogic Hal ; 
        if (instence == null ) {
            instence = Instantiate(obj) as GameObject;
            Hal  = instence.GetComponent<HolidayActivityLogic>();
            RED.AddChild (instence , DBUIController.mDBUIInstance._TopRoot );
            return Hal;
        }
        return null; 
    }

    //初始化数据
	private void initInfo(NewActivityData[] data)
	{
        if (listAct !=  null )
        {
            for (int i = 0; i <listAct.Count ;i++)
            {
                listAct[i].SetActive(false);
            }
        }

        if(har == null || har.data == null  || har.data.Length <= 0) 
        {
           
            if (Ae != null)
            {
                Ae.gameObject.SetActive(false); //活动说明显示界面
            }

            return;
        }


        GameObject instence;
        ActivityItem ai ; 
		NewActivityData hai;
        if(activityInfo != null )
        {
            activityInfo.Clear();
        }

       
        int index =0 ;
        for (int i = 0 ; i < har.data.Length ; i++ ) 
        {
            hai  =  har.data[i];
            index   = i  ; 

            if (index<listAct.Count)
            {

                instence = listAct[index] ; 


            }else
            {
                instence = Instantiate(at.gameObject) as GameObject;


                if (listAct != null )
                {
                    listAct.Add(instence);
                }

            }

            ai  = instence.GetComponent<ActivityItem>(); 
            instence.name = hai.activityId  ; 
            //hai.id
            instence.transform.parent = _grid.transform ;
            instence.transform.localScale = Vector3.one ;
            instence.transform.localPosition = Vector3.one;
            instence.SetActive(true);


            ai.setName = hai.activityName ; 

			ai.setSpriteIcon = hai.activityIcon;
			ai.activityIndex = i;
            
            if(activityInfo != null )
            {
				activityInfo.Add(int.Parse(hai.activityId) ,hai);
            }

          
          
        }
       
       // _grid.repositionNow = true ;
        _grid.Reposition();
		//_grid.GetComponentInParent<UIScrollView> ().ResetPosition ();
//      Debug.Log(" _grid.transform.childCount == " +  _grid.transform.childCount);

    }
    //点击活动
    public void ClickActivityItem(GameObject go)
    {
        string name = go.transform.parent.gameObject.name ;
      
        clickActId= name ; //点击的ID名字

        int index = int.Parse(name);
		NewActivityData  hai ;
        if (activityInfo!=null &&activityInfo.ContainsKey(index))
        {
            hai  =      activityInfo[index];
//            showExplain(hai.description);
			closeUI();
			UIActivitylimittimeMain.CreatePrefab(hai);
			ActivityItem ai  = go.transform.parent.GetComponent<ActivityItem>(); 
			Core.Data.HolidayActivityManager.mActivityIndex = (ai != null) ? ai.activityIndex : -1;

        }else
        {
            showExplain("");

        }

    }
    //显示说明
    private void showExplain(string description)
    {

        Ae.setExplainLabel = description ; // 说明内容； 

        Ae.gameObject.SetActive(true); //显示界面

    }

    // 关闭界面
    public void closeUI()
    {
        Destroy(this.gameObject);
    }
    //
    void OnDestroy()
    {
        CancelInvoke("OneHour");
        if(activityInfo != null )
        {
            activityInfo.Clear();

        }
        instence = null ; 
        _grid    = null ; 
        if (listAct.Count >0)
        {

            for (int i = 0 ; i < listAct.Count ;i++ )
            {
                Destroy(listAct[i]) ;
            }

            listAct.Clear();
        }
        har = null ; 
        activityInfo=null ;
        listAct=null ;
    }

	void ActivityLimitTimeRequest()
	{
		ActivityLimitTimeParam param = new ActivityLimitTimeParam(int.Parse(Core.Data.playerManager.PlayerID));
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		
		task.AppendCommonParam(RequestType.ACTIVITYLIMITTIME, param);
		
		task.ErrorOccured += ActivityLimitTimeError;
		task.afterCompleted += ActivityLimitTimeReponse;
		
		task.DispatchToRealHandler ();
	}
	
	void ActivityLimitTimeReponse(BaseHttpRequest request, BaseResponse response)
	{
		ComLoading.Close();
		if (response != null && response.status != BaseResponse.ERROR) 
		{
			NewActivityResponse res = response as NewActivityResponse;
			if(res != null)
			{
				har = res;
				Core.Data.HolidayActivityManager.har = res;
				initInfo(res.data);
            }
        }
		if(response != null && response.status == BaseResponse.ERROR)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
		}
	}
	
	void ActivityLimitTimeError (BaseHttpRequest request, string error)
	{
		ComLoading.Close();
		ConsoleEx.DebugLog ("---- Http Resp - Error has ocurred." + error);
	}
	
}
