#if APP_STORE
using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;


public partial class IOSInterface : MonoBehaviour {

	public event Action<string> myReachabilityChanged;
	
	public  static IOSInterface self = null;

	public static IOSInterface CreateIOSInterfaceObject()
	{
		if(self == null)
		{
			GameObject go = new GameObject("IOSInterface");
			self = go.AddComponent<IOSInterface>();
		}
		return self;
	}

	void Awake()
	{
		self = this;
        #if UNITY_IPHONE
		IOS.applicationDidFinishLaunching();
        #endif
	}
	void OnDestroy()
	{
		self = null;
	}
	
	void reachabilityChanged(string msg)
	{
		if(myReachabilityChanged != null)
		{
			myReachabilityChanged(msg);
		}
	}

}

#if UNITY_IPHONE
public class IOS{

	public const string URL_HEAD_iTunes = "itms://";
	public const string URL_HEAD_SMS = "sms://";//"sms:555-1234"
	public const string URL_HEAD_TEL = "tel://";//"tel://555-1234"
	public const string URL_HEAD_MAIL = "mailto:";//mailto:apple@mac.com?Subject=hello" 

	[DllImport ("__Internal")]
	private static extern bool _canOpenURL(string url);
	[DllImport ("__Internal")]
	private static extern bool _openURL(string url);
	[DllImport ("__Internal")]
	private static extern string _getExecutablePath();
	[DllImport ("__Internal")]
	private static extern string _getMacAddress();
	[DllImport ("__Internal")]
	private static extern void _Exit(int status);
	[DllImport ("__Internal")]
	private static extern bool _Debug();
	[DllImport ("__Internal")]
	private static extern void _ReachabilityWithHost(string host);
	[DllImport ("__Internal")]
	private static extern string _systemType();
	[DllImport ("__Internal")]
	private static extern string _systemVersion();
	[DllImport ("__Internal")]
	private static extern string _timeZone();
    [DllImport ("__Internal")]
    private static extern string _countryName();
    [DllImport ("__Internal")]
    private static extern string _IDFA();
     [DllImport ("__Internal")]
    private static extern string _macAddress();
    [DllImport ("__Internal")]
	private static extern void _applicationDidFinishLaunching();

	public static void applicationDidFinishLaunching()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer||
		   Application.platform == RuntimePlatform.OSXPlayer)
		{
			_applicationDidFinishLaunching();
		}
		
	}
    public static string reallyMacAddress()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer||
		   Application.platform == RuntimePlatform.OSXPlayer)
		{
			return _macAddress();
		}
		return "000000";
	}
    
    public static string systemIDFA()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer||
		   Application.platform == RuntimePlatform.OSXPlayer)
		{
			return _IDFA();
		}
		return "IDFA_Test";
	}

	public static void ReachabilityWithHost(string host){
		if(Application.platform == RuntimePlatform.IPhonePlayer||
		   Application.platform == RuntimePlatform.OSXPlayer)
		{
			_ReachabilityWithHost(host);
		}
	}
	public static string systemType()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer||
		   Application.platform == RuntimePlatform.OSXPlayer)
		{
			return _systemType();
		}
		return "Unity Simulator";
	}
	public static string systemVersion()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer||
		   Application.platform == RuntimePlatform.OSXPlayer)
		{
			return _systemVersion();
		}
		return "unknown";
	}
	public static string timeZone()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer||
		   Application.platform == RuntimePlatform.OSXPlayer)
		{
			return _timeZone();
		}
		return string.Empty;
	}
	public static string countryName()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer||
		   Application.platform == RuntimePlatform.OSXPlayer)
		{
			return _countryName();
		}
		return string.Empty;
	}
	public static bool Debug()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer||
		   Application.platform == RuntimePlatform.OSXPlayer)
		{
			return _Debug();
		}
		return true;
	}

	public static bool openURL(string url){
		if(Application.platform == RuntimePlatform.IPhonePlayer||
		   Application.platform == RuntimePlatform.OSXPlayer)
		{
			return _openURL(url);
		}
		return false;
	}
	public static bool canOpenURL(string url){
		if(Application.platform == RuntimePlatform.IPhonePlayer||
		   Application.platform == RuntimePlatform.OSXPlayer)
		{
			return _canOpenURL(url);
		}
		return false;
	}

	public static string getExecutablePath()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer||
		   Application.platform == RuntimePlatform.OSXPlayer)
		{
			return _getExecutablePath();
		}
		return null;

	}

	public static string getMacAddress()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer||
		   Application.platform == RuntimePlatform.OSXPlayer)
		{
			return _getMacAddress();
		}
		return null;
	}
	public static void Exit(int status)
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer||
		   Application.platform == RuntimePlatform.OSXPlayer)
		{
			_Exit(status);
		}
		
	}
}
#endif

#endif

