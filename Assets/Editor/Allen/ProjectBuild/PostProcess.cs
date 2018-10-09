#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using System.Xml;
using UnityEditor.XCodeEditor_AutoPack;
#endif

using System.IO;


/// <summary>
/// 当包打完了，要处理一下
/// Add this attribute to a method to get a notification just after building the player.
/// </summary>
public static class PostProcess
{
	#if UNITY_EDITOR
	[PostProcessBuild (100)]
	public static void OnPostProcessBuild (BuildTarget target, string pathToBuiltProject) {
		if (target == BuildTarget.Android) {
			FileUtility.DeleteFolder( Application.dataPath + "/Plugins/Android");
			if(BuildUtils.projectName == "Qihoo360")
			{
				//当我们在打Qihoo360包的时候 这里面做一些 操作。
			}
		} else if(target == BuildTarget.iPhone) {


			string projectName = BuildUtils.projectName;

			try {
				File.Delete(Application.dataPath + "/Plugins/iOS/APService.h");
				File.Delete(Application.dataPath + "/Plugins/iOS/Extension.mm");
				File.Delete(Application.dataPath + "/Plugins/iOS/JPushUnityManager.h");
				File.Delete(Application.dataPath + "/Plugins/iOS/JPushUnityManager.mm");
				File.Delete(Application.dataPath + "/Plugins/iOS/libPushSDK.a");
			} catch(System.Exception ex) {
				Debug.Log(ex.Message);
			}

			if(projectName.StartsWith("Spade")) {
				//得到xcode工程的路径
				string path = Path.GetFullPath (pathToBuiltProject);
				//创建XCode项目
				XCProject_AutoPack project = createXProj(pathToBuiltProject);
				// 编辑plist 文件
				EditorPlist(path);
				//修正代码
				EditorCode(path);
				// Finally save the xcode project
				project.Save ();
			} else if(projectName.StartsWith("Oringinal")) {

			}

		}

	}

	//创建Xcode的工程
	static XCProject_AutoPack createXProj(string pathToBuiltProject) {
		// Create a new project object from build target
		XCProject_AutoPack project = new XCProject_AutoPack (pathToBuiltProject);

		// Find and run through all projmods files to patch the project.
		// Please pay attention that ALL projmods files in your project folder will be excuted!
		//在这里面把frameworks添加在你的xcode工程里面
		string[] files = Directory.GetFiles (Application.dataPath, "*.projmods", SearchOption.AllDirectories);
		foreach (string file in files) {
			project.ApplyMod (file);
		}

		//添加provisioning 
		project.overwriteBuildSetting ("PROVISIONING_PROFILE", "DisYingYou");

		//设置签名的证书， 第二个参数 你可以设置成你的证书
		project.overwriteBuildSetting ("CODE_SIGN_IDENTITY", "iPhone Distribution: Shanghai Shadow Game Network Science and Technology Co., Ltd.", "Release");
		project.overwriteBuildSetting ("CODE_SIGN_IDENTITY", "iPhone Distribution: Shanghai Shadow Game Network Science and Technology Co., Ltd.", "Debug");

		return project;
	} 

	//修正PList
	static void EditorPlist(string filePath) {
		XCPlist list   = new XCPlist(filePath);
		string bundle  = "com.afeiyingyou.dragonball";
		string appname = "七龙珠";


		string PlistAdd = @"
			<key>UIViewControllerBasedStatusBarAppearance</key>
			<false/>
			<key>NSLocationWhenInUseDescription</key>
			<string>YES</string>
			<key>NSLocationAlwaysUsageDescription</key>
			<string>YES</string>";

		//在plist里面增加一行
		list.AddKey(PlistAdd);

		list.ReplaceKey("<string>dragonball</string>", "<string>" + appname + "</string>" );

		//在plist里面替换Bundle ID
		list.ReplaceKey("<string>com.afeiyingyou.${PRODUCT_NAME}</string>","<string>" + bundle + "</string>");
		//保存
		list.Save();
	}

	//修正代码
	private static void EditorCode(string filePath) {
		//读取UnityAppController.mm文件
		XClass UnityAppController = new XClass(filePath + "/Classes/UnityAppController.mm");

		//在指定代码后面增加一行代码
		UnityAppController.WriteBelow("#include \"PluginBase/AppDelegateListener.h\"","#import \"HTGameProxy.h\"");


		//调用 SDK 初始化
		//在指定代码后面增加一行
		/*
		    HTGameInfo *gameInfo = [[HTGameInfo alloc] init];
		    gameInfo.direction = Landscape;
		    gameInfo.shortName = @"dragonball";
		    gameInfo.name = @"七龙珠";
		    
		    [HTGameProxy setLogEnable:YES];
		    [HTGameProxy setDebugEnable:YES];
		    [HTGameProxy setShowFunctionMenu:YES];
		    [HTGameProxy initWithGameInfo:gameInfo];
		 */ 

		string code = "\tHTGameInfo *gameInfo = [[HTGameInfo alloc] init];\n\tgameInfo.direction = Landscape;\n\tgameInfo.shortName = @\"qlz\";\n\tgameInfo.name = @\"七龙珠\";\n\t[HTGameProxy setLogEnable:YES];\n\t[HTGameProxy setDebugEnable:NO];\n\t[HTGameProxy setShowFunctionMenu:YES];\n\t[HTGameProxy initWithGameInfo:gameInfo];";

		UnityAppController.WriteBelow("UnityInitApplicationNoGraphics([[[NSBundle mainBundle] bundlePath]UTF8String]);",code);


		//在游戏 AppDelegate 对应的生命周期方法中调用对应的黑桃 SDK 方法
		//在指定代码后面增加一行
		string bgCode = "\t[HTGameProxy applicationDidEnterBackground];";
		UnityAppController.WriteBelow("printf_console(\"-> applicationDidEnterBackground()\\n\");}",bgCode);


		string foregroundCode = "\t[HTGameProxy applicationWillEnterForeground];";
		UnityAppController.WriteBelow("[GetAppController().unityView recreateGLESSurfaceIfNeeded];",foregroundCode);

		string HandleURLCode = "\t[HTGameProxy application:application handleOpenURL:url];";
		UnityAppController.WriteBelow("AppController_SendNotificationWithArg(kUnityOnOpenURL, notifData);",HandleURLCode);


		///
		///  在游戏主视图控制器对应的生命周期方法中调用对应的黑桃 SDK 方法
		///
	}

	#endif
}