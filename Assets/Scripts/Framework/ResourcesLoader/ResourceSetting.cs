using System;

namespace AW.Resources
{
    public class ResourceSetting
    {

        #if UNITY_IPHONE
        public static string m_RemoteFolder = "ios/";
        #elif UNITY_ANDROID
        public static string m_RemoteFolder = "android/";
        #else
        public static string m_RemoteFolder = "editor/";
        #endif
        public const string FULL_PATH = @"Assets.Resources.Pack.CRLuo.Charactor.";
        public const string EXTENSION_FILENAME = @".unity3d";

        //public static string URL = @"http://114.215.183.29/down/resource/dragonball/";
		public static string URL = @"http://cdn.myappblog.net/resource/dragonball/";
        
        //传入Prefab的文件名，不带有路径
        public static string ConvertToAssetBundleName(string name) {
            return name + EXTENSION_FILENAME;
        }

        public static string ConverToFtpPath(string name) {
			return URL + m_RemoteFolder + SoftwareInfo.VersionCode.ToString() + "/" + name;
        }
    }
}

