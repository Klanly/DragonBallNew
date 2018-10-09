using UnityEngine;
using System.Collections;

public class GPSLocation : MonoBehaviour 
{
	private static string UNTIFY_CLASS = "com.unity3d.player.UnityPlayer";
	//pgs定位服务是否可用
	private bool m_bCanLocate;

	#if UNITY_ANDROID && !UNITY_EDITOR
	private AndroidJavaObject activity;
	#endif
	private bool m_blocateSuc;
	private string m_strLocation;
	public void SetGPSLocation(string strLocation)
	{
		RED.Log (" GPSLocation :: SetGPSLocation " + strLocation);
		m_blocateSuc = true;
		m_strLocation = strLocation;
	}

	//停止pgs服务
	public void StopGPS()
	{
		Input.location.Stop();
	}
		

	//开启pgs服务
	public IEnumerator StartGPS()
	{
		float jindu = 0.0f;
		float weidu = 0.0f;

		#if UNITY_ANDROID && !UNITY_EDITOR
	
		if(activity == null)
		{
			AndroidJavaClass unityClass = new AndroidJavaClass(UNTIFY_CLASS);
		 	activity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
		}

		activity.Call("runOnUiThread", new AndroidJavaRunnable(() => { 
			Native.mInstace.StartGPS();
		}));

		int waitTime = 10;
		while(!m_blocateSuc && waitTime > 0)
		{
			ComLoading.Open (Core.Data.stringManager.getString(5211));
			yield return new WaitForSeconds(1);
			waitTime--;
		}

		ComLoading.Close();
		if(!string.IsNullOrEmpty(m_strLocation))
		{
			string[] strText = m_strLocation.Split('|');
			jindu = float.Parse(strText[0]);
			weidu = float.Parse(strText[1]);
			m_bCanLocate = true;
		}
		else
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5185));
			m_bCanLocate = false;
			return false;
		}

		#elif UNITY_IPHONE && !UNITY_EDITOR

		Native.mInstace.StartGPS();
		
		int waitTime = 10;
		while(!m_blocateSuc && waitTime > 0)
		{
			ComLoading.Open (Core.Data.stringManager.getString(5211));
			yield return new WaitForSeconds(1);
			waitTime--;
		}
		
		ComLoading.Close();
		if(!string.IsNullOrEmpty(m_strLocation))
		{
			string[] strText = m_strLocation.Split('|');
			jindu = float.Parse(strText[0]);
			weidu = float.Parse(strText[1]);
			m_bCanLocate = true;
		}
		else
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5185));
			m_bCanLocate = false;
			return false;
		}
#else 

		if(!Input.location.isEnabledByUser)
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5183));
			m_bCanLocate = false;
			return false;
		}
			
		if(Input.location.status == LocationServiceStatus.Running)
		{
			m_bCanLocate = false;
			return false;
		}

		Input.location.Start(500, 500);

		int waitTime = 10;
		while(Input.location.status == LocationServiceStatus.Initializing && waitTime > 0)
		{
			ComLoading.Open (Core.Data.stringManager.getString(5211));
			yield return new WaitForSeconds(1);
			waitTime--;
		}

		ComLoading.Close ();
		if(waitTime <= 0)
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5184));
			m_bCanLocate = false;
			return false;
		}

		if(Input.location.status == LocationServiceStatus.Failed)
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5185));
			m_bCanLocate = false;
			return false;
		}

		jindu = Input.location.lastData.longitude;
		weidu = Input.location.lastData.latitude;

#endif
		RadarTeamUI.mInstace.SendSyncGPSRQ (jindu, weidu);
	}

	//得到经度
	public float GetLongitude()
	{
		if (m_bCanLocate)
		{
			return Input.location.lastData.longitude;
		}
		RED.LogWarning("GetLongitude：：GPS定位不可用");
		return 0.0f;
	}

	//得到纬度
	public float Getlatitude()
	{
		if (m_bCanLocate)
		{
			return Input.location.lastData.latitude;
		}
		RED.LogWarning("Getlatitude：：GPS定位不可用");
		return 0.0f;
	}
}



