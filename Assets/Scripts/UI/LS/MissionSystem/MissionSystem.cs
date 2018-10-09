using UnityEngine;
using System.Collections;

public class MissionSystem : RUIMonoBehaviour {

	static MissionSystem _Instance;
	public static MissionSystem GetInstance()
	{
		return _Instance;
	}

	public MissionSystemMain _MissionSystemMain;

	public static void OpenUI()
	{
		if(_Instance == null)
		{
			Object prefab = PrefabLoader.loadFromPack ("LS/MissionSysRoot");
			if (prefab != null)
			{
				GameObject obj = Instantiate (prefab) as GameObject;
				RED.AddChild (obj, DBUIController.mDBUIInstance._TopRoot);
				_Instance = obj.GetComponent<MissionSystem> ();
			}
		}
	}

	void Back_OnClick()
	{
		_MissionSystemMain.DeleteCell();
		this.dealloc();
	}

	void Start () 
	{
		_MissionSystemMain.SendMissionSysRequest();
	}

	void OnDestroy()
	{
		_MissionSystemMain = null;
	}


	

}
