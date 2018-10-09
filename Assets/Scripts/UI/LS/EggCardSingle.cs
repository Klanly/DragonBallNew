using UnityEngine;
using System.Collections;

public class EggCardSingle : RUIMonoBehaviour {

	static EggCardSingle _mInstance;
	public static EggCardSingle GetInstance()
	{
		return _mInstance;
	}

	public CRLuo_ShowANDelCharactor _CRLuo_ShowANDelCharactor;
	public Camera ShakeCamera;
	public int starnum;

	public static void Open3D(int m_Starnum)
	{
		if(_mInstance == null)
		{
			Object obj = PrefabLoader.loadFromPack("LS/pbLSEggCardSingle",true);
			if(obj != null)
			{
				GameObject go = Instantiate(obj) as GameObject;
				_mInstance = go.GetComponent<EggCardSingle>();
				_mInstance.starnum = m_Starnum;
				_mInstance._CRLuo_ShowANDelCharactor.StarNum = m_Starnum;
			}
		}
		else
		{
			_mInstance.starnum = m_Starnum;
			_mInstance._CRLuo_ShowANDelCharactor.StarNum = m_Starnum;
			_mInstance._CRLuo_ShowANDelCharactor.ReplayCRLuo();
		}

	}

	public static void Open3DWithoutEgg(int m_Starnum){
		Object obj = PrefabLoader.loadFromPack("LS/pbWXLEggCardSingle",true);
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			_mInstance = go.GetComponent<EggCardSingle>();
			_mInstance.starnum = m_Starnum;
			_mInstance._CRLuo_ShowANDelCharactor.StarNum = m_Starnum;
		}
	}
	

	public void Delete()
	{
		Destroy(_CRLuo_ShowANDelCharactor.goModel);
		this.dealloc();
	}
	
	void OnDestroy()
	{
		_CRLuo_ShowANDelCharactor = null;
		_mInstance = null;
	}
}
