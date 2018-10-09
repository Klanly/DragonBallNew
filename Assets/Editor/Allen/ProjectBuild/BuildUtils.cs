using System;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class BuildUtils {

	//得到项目的名称
	public static string projectName {
		get { 
			//在这里分析shell传入的参数，
			//这里遍历所有参数，找到 project开头的参数， 然后把-符号 后面的字符串返回，
			foreach(string arg in System.Environment.GetCommandLineArgs()) {
				if(arg.StartsWith("project"))
				{
					return arg.Split("-"[0])[1];
				}
			}
			return string.Empty;
		}
	}

	public static string buildMode {
		get {
			//在这里分析shell传入的参数，
			//这里遍历所有参数，找到 project开头的参数， 然后把-符号 后面的字符串返回，
			foreach(string arg in System.Environment.GetCommandLineArgs()) {
				if(arg.StartsWith("mode"))
				{
					return arg.Split("-"[0])[1];
				}
			}
			return string.Empty;
		}
	}

	public static string apkorprojection {
		get {
			//在这里分析shell传入的参数，
			//这里遍历所有参数，找到 project开头的参数， 然后把-符号 后面的字符串返回，
			foreach(string arg in System.Environment.GetCommandLineArgs()) {
				if(arg.StartsWith("apkorproject"))
				{
					return arg.Split("-"[0])[1];
				}
			}
			return string.Empty;
		}
	}

	//在这里找出你当前工程所有的场景文件，假设你只想把部分的scene文件打包 那么这里可以写你的条件判断 返回一个字符串数组。
	public static string[] GetBuildScenes() {
		List<string> names = new List<string>();
		foreach(EditorBuildSettingsScene e in EditorBuildSettings.scenes) {
			if( e != null && e.enabled)
				names.Add(e.path);
		}
		return names.ToArray();
	}

}
