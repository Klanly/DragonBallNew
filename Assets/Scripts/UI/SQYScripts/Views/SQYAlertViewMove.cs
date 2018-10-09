using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class SQYAlertViewMove : MDBaseAlertView {

	public static bool initialized;
    public List<GameObject> listG = new List<GameObject>();
    public List<GameObject> listTask = new List<GameObject>();

	public class MsgInfo
	{
		public string content;
		public GameObject parentObj;
        public int type ;
        public MsgInfo(string _content,GameObject _parentObj , int _type )
		{
			content = _content;
			parentObj = _parentObj;
            type = _type; 
		}
	}
	
	public static void Initialize() {
		if (!initialized) 
		{
			if(!Application.isPlaying)
				return;
			initialized = true;
			var g = new GameObject("SQYAlertViewMove");

			var gobal = GameObject.FindGameObjectWithTag("Gobal");
			if(gobal != null) g.transform.parent = gobal.transform;
			g.AddComponent<SQYAlertViewMove>();
			DontDestroyOnLoad(g);
		}

	}

	//消息队列
	static List<MsgInfo> list_msgs = new List<MsgInfo>();
    //任务消息队列
    static List<MsgInfo> list_msgTask = new List<MsgInfo>();

    public static void  CreateAlertViewMove(string msg,GameObject CustGameObj = null ,int type = 0 )
	{
		if(!string.IsNullOrEmpty(msg))
		{
			if (type ==0 )
			{
				list_msgs.Add(new MsgInfo(msg,CustGameObj,type));
			}
			else
			{
				list_msgTask.Add(new MsgInfo(msg,CustGameObj,type));
			}
		}


	}
    private  int index = 0 ;
    public  void CreateAndMove(MsgInfo msg)
	{
        Object obj = PrefabLoader.loadFromPack("SQY/pbSQYAlertViewMove");


        if(obj !=null)
        {
            GameObject go = Instantiate(obj) as GameObject;
            index++;
            go.name = "test_" + index.ToString();

          //  Debug.Log("msg.type == " + msg.type  ) ; 


            go.transform.localPosition = Vector3.zero;
            if (msg.type == 0 )
            {
                CommonMsg(msg , go ) ;
                list_msgs.Remove(msg);

            }else
            {
              //  Debug.Log("TaskMsg");
                TaskMsg(msg ,go ) ;
                list_msgTask.Remove(msg);

            }

   

        }

	} 
    private void TaskMsg(MsgInfo msg ,GameObject go)
    {
        listTask.Add(go);
        //Debug.Log("listTask.Count == " +listTask.Count) ;
        if (listTask.Count >= 2)
        {
            if (listTask[0] != null )
            {
                SQYAlertViewMoveElement alert1  =    listTask[0].GetComponent<SQYAlertViewMoveElement>();
                listTask.RemoveAt(0);
                alert1.closeDes();
                alert1.close();
            }else
            {
                listTask.RemoveAt(0);
            }

        }

      //  Debug.Log("go == " + go ) ;
        SQYAlertViewMoveElement alert = go.GetComponent<SQYAlertViewMoveElement>();
        alert.alertViewFreshUI(msg.content,msg.parentObj);

     
         go.transform.localPosition = new Vector3(0,100,0) ;


    }
    private void CommonMsg(MsgInfo msg ,GameObject go)
    {
        listG.Add(go);
        if (listG.Count >= 2)
        {
            if (listG[0] != null )
            {
                SQYAlertViewMoveElement alert1  =    listG[0].GetComponent<SQYAlertViewMoveElement>();
                listG.RemoveAt(0);
                alert1.closeDes();
                alert1.close();
            }else
            {
                listG.RemoveAt(0);
            }


        }
       // Debug.Log("go.position == " + go.transform.localPosition + "msg.parentObj == "  +msg.parentObj);
        SQYAlertViewMoveElement alert = go.GetComponent<SQYAlertViewMoveElement>();
        alert.alertViewFreshUI(msg.content,msg.parentObj);
        go.transform.localPosition =  Vector3.zero;

    }
  
	void Awake()
	{

		InvokeRepeating("UpdateCheck",0,0.02f);
	}

	void UpdateCheck()
	{
		if(list_msgs.Count > 0)
		{

            CreateAndMove(list_msgs[0]);

		}

        if (list_msgTask.Count > 0 )
        {

            CreateAndMove(list_msgTask[0]);
        }


	}

	

}
