using UnityEngine;
using System.Collections;

public class HardwareInfo : MonoBehaviour {

	private static HardwareInfo _instance;
	public static HardwareInfo mInstace
	{
		get
		{
			return _instance;
		}
	}
	[HideInInspector]
	public string deviceName;
	[HideInInspector]
	public string graphicsDeviceName;
	[HideInInspector]
	public string operatingSystem;
	[HideInInspector]
	public string systemMemorySize;
	[HideInInspector]
	public string graphicsMemorySize;
	[HideInInspector]
	public string deviceId;


	void Awake(){
		_instance = this;
	}


	void Start () {
		deviceId =  SystemInfo.deviceUniqueIdentifier;
		deviceName = SystemInfo.deviceName;
		graphicsDeviceName = SystemInfo.graphicsDeviceName;
		operatingSystem = SystemInfo.operatingSystem;
		systemMemorySize = SystemInfo.systemMemorySize.ToString();
		graphicsMemorySize = SystemInfo.graphicsMemorySize.ToString();
	}
	
}
