using System.IO;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

class ProjectBuilder : Editor {

	public static string MacroSymbol = "AOT0;China;CHECKCONFIG;SPLIT_MODEL;NEWPVE;TEST;InMobi";

	public static string MacroSymbol_QiHoo = "AOT0;China;CHECKCONFIG;SPLIT_MODEL;QiHo360";

	public static string MacroSymbol_Spade = "AOT0;China;SPLIT_MODEL;NEWPVE;TEST";//"AOT0;China;CHECKCONFIG;SPLIT_MODEL;NEWPVE;TEST";

	public static string MacroSymbol_Inmobi = "AOT0;China;CHECKCONFIG;SPLIT_MODEL;Spade;NEWPVE;InMobi";

	public static string MacroSymbol_Google = "AOT0;China;SPLIT_MODEL;NEWPVE;Google;TEST";

	/// <summary>
	/// 给Android提供编译的方法
	/// </summary>
	static void BuildForAndroid() {
		FileUtility.DeleteFolder(Application.dataPath + "/Plugins/Android");

		//不管什么平台先拷贝JPush
		FileUtility.CopyDirectory(Application.dataPath + "/Android_Platform/JPush", Application.dataPath + "/Plugins/Android");

		//是Release模式还是Debug模式
		string buildMode = BuildUtils.buildMode;
		if(buildMode.ToLower() == "debug")
		{
			MacroSymbol += ";DEBUG";
			MacroSymbol_QiHoo += ";DEBUG";
			MacroSymbol_Spade += ";DEBUG";
			MacroSymbol_Inmobi += ";DEBUG";
		}

		string fileName = "";
		string curDate = DateHelper.getShortDate();

		//是工程包还是apk包
		string apkorprojection = BuildUtils.apkorprojection;
		BuildOptions buildOptions = BuildOptions.None;
		if(apkorprojection == "projection")
		{
			fileName = BuildUtils.projectName + "_" + curDate + "_" + buildMode + "_projection";
			buildOptions = BuildOptions.AcceptExternalModificationsToPlayer;
		}
		else
		{
			fileName = BuildUtils.projectName + "_" + curDate + "_" + buildMode + ".apk";
		}

		string projectName = BuildUtils.projectName;
		if (projectName == "Qihoo360") {
			FileUtility.CopyDirectory (Application.dataPath + "/Android_Platform/Qihoo360", Application.dataPath + "/Plugins/Android");
			PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.Android, MacroSymbol_QiHoo);
		} else if (projectName == "Oringinal") {
			FileUtility.CopyDirectory (Application.dataPath + "/Android_Platform/Oringinal", Application.dataPath + "/Plugins/Android");
			PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.Android, MacroSymbol);
		} else if (projectName == "Spade") {
			FileUtility.CopyDirectory (Application.dataPath + "/Android_Platform/Spade", Application.dataPath + "/Plugins/Android");
			PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.Android, MacroSymbol_Spade);
		} else if (projectName == "Inmobi") {
			FileUtility.CopyDirectory (Application.dataPath + "/Android_Platform/Spade_Inmobi", Application.dataPath + "/Plugins/Android");
			PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.Android, MacroSymbol_Inmobi);
		} else if (projectName == "Google") {
			FileUtility.CopyDirectory (Application.dataPath + "/Plugins/Android/StansAssets/Android", Application.dataPath + "/Plugins/Android");
			PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.Android, MacroSymbol_Google);
		}

		string path = DeviceInfo.PersisitFullPath(fileName);

		try {
			if(File.Exists(path)) File.Delete(path);
		} catch(IOException ex) {
			ConsoleEx.DebugLog(ex.ToString());
		} catch(Exception ex) {
			ConsoleEx.DebugLog(ex.ToString());
		}

		///
		/// ----- 这里的代码 releasekey.keystore放在了当前Project的Android_Platform里面 目的是Signed APK ---- 
		///
		string keystoreName = Application.dataPath + "/Android_Platform/releasekey.keystore", keystorePassword = "redinfinity56", keyAliasName = "android_release", keyAliasPassword = "redinfinity56";

		PlayerSettings.Android.keystoreName = keystoreName;
		PlayerSettings.Android.keystorePass = keystorePassword;
		PlayerSettings.Android.keyaliasName = keyAliasName;
		PlayerSettings.Android.keyaliasPass = keyAliasPassword;

		/// --------- 必须使用 Automatic的选项，否则有些 adreno graphic card will parently crash.
		/// NORMAL  AUTO
		/// XIAOMI  OPENGL ES 2.0
		PlayerSettings.targetGlesGraphics = TargetGlesGraphics.OpenGLES_2_0;
		BuildPipeline.BuildPlayer(BuildUtils.GetBuildScenes(), path, BuildTarget.Android, buildOptions);
	}




	public static string MacroSymbol_IOS_Normal = "AOT0;China;CHECKCONFIG;SPLIT_MODEL;NEWPVE;TEST";
	public static string MacroSymbol_IOS_Spade = "AOT0;China;SPLIT_MODEL;NEWPVE;Spade;CHECKCONFIG;TEST";


	/// <summary>
	/// 给Ios提供编译的方法
	/// </summary>
	static void BuildForIos() {
		//不管什么平台先拷贝JPush
		FileUtility.CopyDirectory(Application.dataPath + "/AIOS_Platform/JPush", Application.dataPath + "/Plugins/iOS");

		//是Release模式还是Debug模式
		string buildMode = BuildUtils.buildMode;

		if(buildMode.ToLower() == "debug") {
			MacroSymbol_IOS_Spade += ";DEBUG";
			MacroSymbol_IOS_Normal += ";DEBUG";
		}



		string projectName = BuildUtils.projectName;

		if(projectName.StartsWith("Spade"))
			PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iPhone, MacroSymbol_IOS_Spade);
		else if(projectName.StartsWith("Oringinal"))
			PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iPhone, MacroSymbol_IOS_Normal);

		PlayerSettings.aotOptions = "nimt-trampolines=1024";

		string path     = DeviceInfo.PersisitFullPath(BuildUtils.projectName);
		//删除之前的工程目录
		if(Directory.Exists(path))
			FileUtility.deleteSubFolder(path);

		BuildPipeline.BuildPlayer(BuildUtils.GetBuildScenes(), path, BuildTarget.iPhone, BuildOptions.Development);
	}

}