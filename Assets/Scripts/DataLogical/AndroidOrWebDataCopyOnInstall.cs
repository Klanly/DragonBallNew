using System;
using System.IO;
using System.Collections;
using UnityEngine;

namespace AW.Data
{
	/*Contains the path to the StreamingAssets folder (Read Only).

    If you have a "StreamingAssets" folder in the Assets folder of your project, it will be copied to your player builds and be present in the path given by Application.streamingAssetsPath.
    Note that on some platforms it is not possible to directly access the StreamingAssets folder because there is no file system access in the web platforms,
    and because it is compressed into the .apk file on Android. On those platforms, a url will be returned, which can be used using the WWW class.
    */

	//因为当Android和Web的情况下时，StreamingAssets的路径不是一个普通的路径，而是一个JAR的文件
	//必须在UI线程
	public class AndroidOrWebDataCopyOnInstall
	{
		/// 
		/// 拷贝配表信息到可以读取的位置
		/// 

		public void ApplicationOn(MonoBehaviour worker, Action CopyFinished, Action IfCoied) {
			if(check()) {
				worker.StartCoroutine(Copy(CopyFinished, IfCoied));
			} else {
				if(CopyFinished != null) CopyFinished();
			}
		}

		private bool check() {
			bool copy = false;
			string hasDownloaded = Path.Combine(DeviceInfo.PersistRootPath, "Config");
			if (Directory.Exists(hasDownloaded)) {
				string[] fileName = Directory.GetFiles(hasDownloaded);
				if(fileName == null || fileName.Length == 0) {
					//Copy data from streaming assets to 
					copy = true;
				}
			} else {
				Directory.CreateDirectory(hasDownloaded);
				copy = true;
			}

			return copy;
		}


		private IEnumerator Copy(Action CopyFinished, Action IfCoied) {
			WWW www = null;

			foreach(HowToRead read in Config.LocalConfigs.Values) {
				string srcPath = Path.Combine(DeviceInfo.StreamingPath, read.path);
				string targetPath = Path.Combine(DeviceInfo.PersistRootPath, read.path);

				www = new WWW(srcPath);
				yield return www;
				byte[] data = www.bytes;
				using(FileStream fs = File.Create(targetPath)){
					fs.Write(data, 0, data.Length);
				}

				www.Dispose();
			}

			if(IfCoied != null) IfCoied();
			if(CopyFinished != null) CopyFinished();

		}


		///
		/// ---------------------  拷贝lua到可以读取的位置  -----------------------
		///
		public void LuaOn(MonoBehaviour worker, Action CopyFinished, Action IfCoied) {
			if(checkLua()) {
				worker.StartCoroutine(CopyLua(CopyFinished, IfCoied));
			} else {
				if(CopyFinished != null) CopyFinished();
			}
		}


		bool checkLua() {
			bool copy = false;
			string hasDownloaded = Path.Combine(DeviceInfo.PersistRootPath, "LuaRoot");
			if (Directory.Exists(hasDownloaded)) {
				string[] fileName = Directory.GetFiles(hasDownloaded);
				if(fileName == null || fileName.Length == 0) {
					//Copy data from streaming assets to 
					copy = true;
				}
			} else {
				Directory.CreateDirectory(hasDownloaded);
				copy = true;
			}

			return copy;
		}

		private IEnumerator CopyLua(Action CopyFinished, Action IfCoied) {
			WWW www = null;

			string srcPath = Path.Combine(DeviceInfo.StreamingPath, "LuaRoot/main.lua");
			string targetPath = Path.Combine(DeviceInfo.PersistRootPath, "LuaRoot/main.lua");

			www = new WWW(srcPath);
			yield return www;
			byte[] data = www.bytes;
			using(FileStream fs = File.Create(targetPath)){
				fs.Write(data, 0, data.Length);
			}

			www.Dispose();

			if(IfCoied != null) IfCoied();
			if(CopyFinished != null) CopyFinished();

		}

	}
}

