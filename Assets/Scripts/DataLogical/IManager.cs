using System;
#if DEBUG
using System.Diagnostics;
#endif
using fastJSON;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// Manager interface. 下面方法二者选其一
/// </summary>
public interface IManager
{
	/// <summary>
	/// Reads from local config file.
	/// </summary>
	/// <returns><c>true</c>, if from local config file was read, <c>false</c> otherwise.</returns>
	bool readFromLocalFile<T>(string path, IList<T> container, Type type) where T : class;
	bool readFromLocalConfigFile<T>(ConfigType configType, IList<T> container) where T : class;


	/// <summary>
	/// Reads from local config file.
	/// </summary>
	/// <returns><c>true</c>, if from local config file was read, <c>false</c> otherwise.</returns>
	bool readFromLocalFile<TValue>(string path, IDictionary<int, TValue> container, Type type) 
		where TValue : UniqueBaseData;
	bool readFromLocalConfigFile<TValue>(ConfigType configType, IDictionary<int, TValue> container) 
		where TValue : UniqueBaseData;

	/// <summary>
	/// Write to local config file.
	/// </summary>
	/// <returns><c>true</c>, if from local config file was read, <c>false</c> otherwise.</returns>

	bool writeToLocalFile<T>(string path, IList<T> container) where T : class;
	bool writeToLocalConfigFile<T>(ConfigType configType, IList<T> container) where T : class;

	/// <summary>
	/// Write to local config file.
	/// </summary>
	/// <returns><c>true</c>, if from local config file was read, <c>false</c> otherwise.</returns>
	bool writeToLocalFile<TValue>(string path, IDictionary<int, TValue> container) 
		where TValue : UniqueBaseData;
	bool writeToLocalConfigFile<TValue>(ConfigType configType, IDictionary<int, TValue> container) 
		where TValue : UniqueBaseData;

	/// <summary>
	/// Decrypts the line. If we need encrypt or decrypt.
	/// </summary>
	/// <returns>The line.</returns>
	/// <param name="decrypt">Decrypt.</param>
	string decryptLine(string decrypt);
}


public class Manager : IManager {

	private ConfigType type;
	#if DEBUG
	private Stopwatch stopwatch = new Stopwatch();
	#endif

	string IManager.decryptLine(string decrypt) {
		return decrypt;
	}

	public virtual void fullfillByNetwork(BaseResponse response) {
		throw new NotImplementedException();
	}

	public virtual bool loadFromConfig() {
		throw new NotImplementedException();
	}

	///增加东西
	public virtual void addItem(BaseResponse response) {
		throw new NotImplementedException();
	}
	///花费材料
	public virtual void spendItem(BaseResponse response) {
		throw new NotImplementedException();
	}

	public string getBasePath()
	{
#if CHECKCONFIG		
		string BasePath = DeviceInfo.PersistRootPath;
		string hasDownloaded = Path.Combine(DeviceInfo.PersistRootPath, "Config");
		if(Directory.Exists(hasDownloaded))
		{
			 string[] fileName = Directory.GetFiles(hasDownloaded);
			 if ( !(fileName != null && fileName.Length >= 1) )
				BasePath = DeviceInfo.StreamingPath;
		}
		else
			BasePath = DeviceInfo.StreamingPath;
#else
		string BasePath = DeviceInfo.StreamingPath;
		if(Core.SM.rtPlat == UnityEngine.RuntimePlatform.IPhonePlayer || Core.SM.rtPlat == UnityEngine.RuntimePlatform.Android || Core.SM.rtPlat == UnityEngine.RuntimePlatform.WP8Player){
			string hasDownloaded = Path.Combine(DeviceInfo.PersistRootPath, "Config");
			if (Directory.Exists(hasDownloaded)) {
				string[] fileName = Directory.GetFiles(hasDownloaded);
				if(fileName != null && fileName.Length >= 1) 
					BasePath = DeviceInfo.PersistRootPath;
			}
		}

#endif			
		return BasePath;
	}

	public virtual bool readFromLocalFile<T>(string path, IList<T> container, Type type) where T : class
	{
		bool success = false;

		string localpath = Path.Combine(getBasePath(), path);

		success = File.Exists(localpath);
		if(success) {
			success = readFile<T>(localpath, container, type);
		}

		return success;
	}

	public virtual bool readFromLocalConfigFile<T>(ConfigType configType, IList<T> container) where T : class
	{
		type = configType;

		Utils.Assert(container == null, "Read Config file :" + configType.ToString() + " null.");

		return this.readFromLocalFile<T>(Config.LocalConfigs[configType].path, container, Config.LocalConfigs[configType].format);
	}

	public virtual bool readFromLocalFile<TValue>(string path, IDictionary<int, TValue> container, Type type) where TValue : UniqueBaseData
	{
		bool success = false;

		string localpath = Path.Combine(getBasePath(), path);

		success = File.Exists(localpath);
		if(success) {
			success = readFile<TValue>(localpath, container, type);
		}

		return success;
	}

	public virtual bool readFromLocalConfigFile<TValue>(ConfigType configType, IDictionary<int, TValue> container) where TValue : UniqueBaseData {
		type = configType;

		Utils.Assert(container == null, "Read Config file :" + configType.ToString() + " null.");

		return this.readFromLocalFile<TValue>(Config.LocalConfigs[configType].path, container, Config.LocalConfigs[configType].format);
	}

	/// <summary>
	/// Reads the config files.
	/// </summary>
	/// <param name="path">Path.</param>

	private bool readConfig<T>(string path, IList<T> container) where T : class 
	{
		Utils.Assert( string.IsNullOrEmpty(path), "Path is empty When reads " + type.ToString() + " Config Data.");
		return this.readFile<T>(path, container, Config.LocalConfigs[type].format);
	}

	private bool readFile<T>(string path, IList<T> container, Type type) where T : class
	{
		bool success = true;

		#if DEBUG
		stopwatch.Start();
		#endif

		StreamReader sr = null;
        FileStream fs = File.OpenRead(path);
		string line = null;
		try {
			sr = new StreamReader(fs);
			if(sr != null) {
				while( !string.IsNullOrEmpty(line = sr.ReadLine()) ) {
					if(!line.StartsWith("#")) {
						T t = JSON.Instance.ToObject(line, type) as T;
						container.Add(t);
					}
				}
			}
		} catch(Exception ex) {
			ConsoleEx.DebugLog(ex.ToString() + "\nError Line = " + line);
			success = false;
		} finally {
			if(sr != null) { sr.Close(); sr = null; }
			if(fs != null) { fs.Close(); fs = null; }
			#if DEBUG
			ConsoleEx.DebugLog(type.ToString() + " costs " + stopwatch.ElapsedMilliseconds + " miliseconds to be done!");
			stopwatch.Reset();
			#endif
		}
		return success;
	}



	/// <summary>
	/// Reads the config files.
	/// </summary>
	/// <param name="path">Path.</param>

	private bool readFile<TValue>(string path, IDictionary<int, TValue> container, Type type) where TValue : UniqueBaseData 
	{
		bool success = true;

		#if DEBUG
		stopwatch.Start();
		#endif

		StreamReader sr = null;
        FileStream fs = File.OpenRead(path);
		string line = null;
		try {
			sr = new StreamReader(fs);
			if(sr != null) {
				while( !string.IsNullOrEmpty(line = sr.ReadLine()) ) {
					if(!line.StartsWith ("#")) {
						TValue t = JSON.Instance.ToObject(line, type) as TValue;
						container[t.ID] = t;
					}
				}
			}
		} catch(Exception ex) {
			ConsoleEx.DebugLog(ex.ToString() + "\nError Line = " + line);
			success = false;
		} finally {
			if(sr != null) { sr.Close(); sr = null; }
			if(fs != null) { fs.Close(); fs = null; }

			#if DEBUG
			ConsoleEx.DebugLog(type.ToString() + " costs " + stopwatch.ElapsedMilliseconds + " miliseconds to be done!");
			stopwatch.Reset();
			#endif
		}
		return success;
	}

	private bool readConfig<TValue>(string path, IDictionary<int, TValue> container) where TValue : UniqueBaseData 
	{

		Utils.Assert( string.IsNullOrEmpty(path), "Path is empty When reads " + type.ToString() + " Config Data.");
		return this.readFile<TValue>(path, container, Config.LocalConfigs[type].format);
	}

	public virtual bool writeToLocalFile<T>(string path, IList<T> container) where T : class
	{
		bool success = false;
		string localpath = Path.Combine(getBasePath(), path);

		success = File.Exists(localpath);
		if(success) {
			File.Delete(localpath);
		}
		success = writeFile<T>(localpath, container);
		return success;
	}

	public virtual bool writeToLocalConfigFile<T>(ConfigType configType, IList<T> container) where T : class {
		type = configType;

		Utils.Assert(container == null, "Read Config file :" + configType.ToString() + " null.");

		return this.writeToLocalFile<T>(Config.LocalConfigs[configType].path, container);
	}

	public virtual bool writeToLocalFile<TValue>(string path, IDictionary<int, TValue> container) where TValue : UniqueBaseData
	{
		bool success = false;

		string localpath = Path.Combine(getBasePath(), path);

		success = File.Exists(localpath);
		if(success) {
			File.Delete(localpath);
		}
		success = writeFile<TValue>(localpath, container);

		return success;
	}

	public virtual bool writeToLocalConfigFile<TValue>(ConfigType configType, IDictionary<int, TValue> container) where TValue : UniqueBaseData {
		type = configType;

		Utils.Assert(container == null, "Read Config file :" + configType.ToString() + " null.");

		return this.writeToLocalFile<TValue>(Config.LocalConfigs[configType].path, container);
	}

	private bool writeFile<T>(string path, IList<T> container) where T : class
	{
		bool success = true;
		#if DEBUG
		stopwatch.Start();
		#endif

		StreamWriter sw = null;
		FileStream fs = File.Open(path, FileMode.OpenOrCreate);

		string line = null;
		try {
			sw = new StreamWriter(fs);
			if(sw != null)
			{
				foreach(T t in container)
				{
					string s = JSON.Instance.ToJSON(t);

					sw.WriteLine(s);
				}
			}
		} catch(Exception ex) {
			ConsoleEx.DebugLog(ex.ToString() + "\nError Line = " + line);
			success = false;
		} finally {
			if(sw != null) { sw.Close(); sw = null; }
			if(fs != null) { fs.Close(); fs = null; }
			#if DEBUG
			ConsoleEx.DebugLog(type.ToString() + " costs " + stopwatch.ElapsedMilliseconds + " miliseconds to be done!");
			stopwatch.Reset();
			#endif
		}
		return success;
	}
	
	private bool writeFile<TValue>(string path, IDictionary<int, TValue> container) where TValue : UniqueBaseData {
		bool success = true;
		Utils.Assert( string.IsNullOrEmpty(path), "Path is empty When reads " + container.ToString() + " File.");

		#if DEBUG
		stopwatch.Start();
		#endif
		File.Delete(path);
		StreamWriter sw = null;
		FileStream fs = File.Open(path, FileMode.OpenOrCreate);
		string line = null;
		try {
			sw = new StreamWriter(fs);
			if(sw != null) {
				foreach(TValue value in container.Values)
				{
					string s = JSON.Instance.ToJSON(value);

					sw.WriteLine(s);
				}
			}
		} catch(Exception ex) {
			ConsoleEx.DebugLog(ex.ToString() + "\nError Line = " + line);
			success = false;
		} finally {
			if(sw != null) { sw.Close(); sw = null; }
			if(fs != null) { fs.Close(); fs = null; }

			#if DEBUG
			ConsoleEx.DebugLog(type.ToString() + " costs " + stopwatch.ElapsedMilliseconds + " miliseconds to be done!");
			stopwatch.Reset();
			#endif
		}
		return success;
	}
}