using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class MobilePhoneInfo
{
	//手机序列号
    public string identifierNumber;
    //手机别名： 用户定义的名称
    public string userPhoneName;
    //设备名称
    public string deviceName;
    //手机系统版本
    public string phoneVersion;
    //手机型号
    public string phoneModel;
    //地方型号
    public string localPhoneModel;
    //当前应用名称
    public string appCurName;
    // 当前应用软件版本  比如：1.0.1
    public string appCurVersion;
    // 当前应用版本号码   int类型
    public string appCurVersionNum;	
}

public class ExtensionManager : Manager {

//#if UNITY_EDITOR	
//#elif UNITY_IPHONE
//	[DllImport("__Internal")]
//    private static extern string GetEquipmentInfo();
//#endif
//	
	public MobilePhoneInfo iPhoneInfo;
//	public MobilePhoneInfo iPhoneInfo
//	{
//		get
//		{
//			if(_iPhoneInfo == null)
//			{
//#if UNITY_EDITOR	
//#elif UNITY_IPHONE
//				_iPhoneInfo = new MobilePhoneInfo();
//				string info_str = GetEquipmentInfo();
//				string[]  info_arry = info_str.Split('_');
//			    if(info_arry.Length > 8)
//				{
//					_iPhoneInfo.identifierNumber = info_arry[0];
//					_iPhoneInfo.userPhoneName = info_arry[1];
//					_iPhoneInfo.deviceName = info_arry[2];
//					_iPhoneInfo.phoneVersion = info_arry[3];
//					_iPhoneInfo.phoneModel = info_arry[4];
//					_iPhoneInfo.localPhoneModel = info_arry[5];
//					_iPhoneInfo.appCurName = info_arry[6];
//					_iPhoneInfo.appCurVersion = info_arry[7];
//					_iPhoneInfo.appCurVersionNum = info_arry[8];
//				}
//#elif UNITY_ANDROID
//				
//#endif
//				return _iPhoneInfo;
//			}
//			else
//				return _iPhoneInfo;
//		}
//	}
	
    public override bool loadFromConfig () {
        return true;
    }
}
