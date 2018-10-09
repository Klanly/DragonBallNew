using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

public class UIActivitylimittimeMain : RUIMonoBehaviour
{
	static UIActivitylimittimeMain mInstance;
	public static UIActivitylimittimeMain GetInstance()
	{
		return mInstance;
	}

	public List<UIActivitylimittimeCell> m_ActivityCellList;

	public UILabel m_TimeLab;
	public UILabel m_DecLab;
	public UILabel m_Title;
	public UIGrid m_Grid;
	public UIPanel m_panel;
	public UIScrollView m_ScrollView;
	public UIScrollBar m_ScrollBar;
	public UIScrollBar m_ScrollBar2;
	
	private long m_Lefttime;


	NewActivityData _NewActivityData;

	public static void CreatePrefab(NewActivityData m_NewActivityData)
	{
		if(mInstance == null)
		{
			UnityEngine.Object prefab = PrefabLoader.loadFromPack ("LS/pbLSActivityLimitTime");
			if (prefab != null)
			{
				GameObject obj = Instantiate (prefab) as GameObject;
				RED.AddChild (obj, DBUIController.mDBUIInstance._TopRoot);
				mInstance = obj.GetComponent<UIActivitylimittimeMain> ();
				mInstance._NewActivityData = m_NewActivityData;
			}
		}
	}

	void Awake()
	{
		mInstance = this;

	}

	void Start()
	{
		m_Title.SafeText("");
		
		m_ActivityCellList = new List<UIActivitylimittimeCell>();
		Invoke("Init", 0.3f);
		ComLoading.Open();
	}

	void ShowTimeLab()
	{
		m_TimeLab.gameObject.SetActive(true);
    }

	void SetPanel()
	{
		m_panel.alpha = 1f;
		m_Grid.Reposition();
	}

	void CreatCell(NewActivityItem[] _Item)
	{
		if(_Item != null)
		{
			UnityEngine.Object prefab = PrefabLoader.loadFromPack ("LS/pbLSActivityLimitTimeCell");
			if (prefab != null)
			{
				for(int i=0; i<_Item.Length; i++)
				{
					GameObject obj = Instantiate (prefab) as GameObject;
					RED.AddChild (obj, m_Grid.gameObject);
					UIActivitylimittimeCell mScript = obj.GetComponent<UIActivitylimittimeCell> ();
					mScript.Init(_Item[i], _NewActivityData.activityId, i);
					m_ActivityCellList.Add(mScript);
				}
			}
		}
	}

	void Init()
	{
		ComLoading.Close();
		if(_NewActivityData != null)
		{
			m_Title.SafeText(_NewActivityData.activityName);
			StringBuilder builder = new StringBuilder();
			builder.Append(Core.Data.stringManager.getString(25177));
			builder.Append(_NewActivityData.activityDesc);
			m_DecLab.SafeText(builder.ToString());

			BoxCollider _box = m_DecLab.GetComponent<BoxCollider>();
			_box.size = new Vector3(m_DecLab.width,  (float)m_DecLab.height, _box.size.z);
			_box.center = new Vector3((float)m_DecLab.width/2, (float)-m_DecLab.height/2, _box.center.z);


			CreatCell(_NewActivityData.items);
			SetTimer(_NewActivityData.activityStartTime, _NewActivityData.activityEndTime);
			m_Grid.Reposition();
			m_ScrollBar.value = 0f;
			m_ScrollBar2.value = 0f;

			m_ScrollView.disableDragIfFits = !(m_DecLab.height >28);
		}

	}

	void SetTimer(long _start, long _end)
	{
		if(m_TimeLab == null)return;
		m_Lefttime = _end - Core.TimerEng.curTime;
		TimerTask task = new TimerTask(Core.TimerEng.curTime, _end, 1);
		task.taskId = TaskID.ActivityLimitTime;
		task.onEvent = TimerEvent;
		task.DispatchToRealHandler();
		InvokeRepeating("SetTimerText", 0f, 1f);
	}

	void TimerEvent(TimerTask task)
	{
		m_Lefttime = task.leftTime;

	}

	void SetTimerText()
	{
		int l = 0;
		long T = 0;
		string output = "";
//		Debug.Log(m_Lefttime.ToString());
		output += (m_Lefttime/(3600*24)).ToString("d2") + Core.Data.stringManager.getString(25183);
		T = m_Lefttime%(3600*24);
		output += (T/3600).ToString("d2")+ Core.Data.stringManager.getString(25184);
		l = (int)T % 3600;
		output += (l / 60).ToString ("d2")+ Core.Data.stringManager.getString(25185);
		l = (int)l % 60;
		output+=l.ToString("d2")+ Core.Data.stringManager.getString(25186);
        
//        DateTime time = DateHelper.UnixTimeStampToDateTime (m_Lefttime);
		StringBuilder builder = new StringBuilder();
		builder.Append(Core.Data.stringManager.getString(25178));
		builder.Append(output);
//		builder.Append(time.Hour.ToString() + ":" + time.Minute.ToString() + ":" + time.Second.ToString());

		m_TimeLab.SafeText(builder.ToString());
    }

	public void Back_Onclick()
	{
		if (SQYMainController.mInstance != null) {
			SQYMainController.mInstance.ArrangeNewActivity ();
		}
		DeleteCell();
		Core.TimerEng.deleteTask(TaskID.ActivityLimitTime);
		this.dealloc();

	}

	void DeleteCell()
	{
		foreach(UIActivitylimittimeCell cell in m_ActivityCellList)
		{
			cell.DeleteCell();
			cell.dealloc();
		}
	}

	void OnDestroy()
	{
		mInstance = null;
	}

}
