using UnityEngine;
using System.Collections;

public class UICardPointFx : RUIMonoBehaviour {

	public ParticleSystem _ParticleSystem;
	bool key = false;

	static UICardPointFx _Instance;
	public static UICardPointFx GetInstance()
	{
		return _Instance;
	}

	public static void OpenUI()
	{
		Object obj = PrefabLoader.loadFromPack("LS/pbLSCardPointFx",true);
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			_Instance = go.GetComponent<UICardPointFx>();
		}
	}

	void ReplayPointFx()
	{

	}

	void Start () 
	{
		Invoke("SteKey", 1f);
	}

	void SteKey()
	{
		key = true;
	}

	void Update ()
	{
		if(key)
		{
			_ParticleSystem.startSize -= 0.6f;
			float y = _ParticleSystem.transform.localPosition.y +  0.05f;
			_ParticleSystem.transform.localPosition = new Vector3(_ParticleSystem.transform.localPosition.x, y, _ParticleSystem.transform.localPosition.z);
			if(_ParticleSystem.startSize <= 2f)
			{
				_ParticleSystem.startSize = 2f;
				key = false;
			}
		}

	}
}
