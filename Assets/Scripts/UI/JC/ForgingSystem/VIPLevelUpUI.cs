using UnityEngine;
using System.Collections;

public class VIPLevelUpUI : MonoBehaviour {

	public GameObject All;
	public UILabel Lab_VipLv;
	public BoxCollider _box;
	void Start ()
	{
	
	}
	
	
	static VIPLevelUpUI _mInstance = null;
	
	public static VIPLevelUpUI Instance
	{
		get
		{
			if(_mInstance ==null )
			{
				Object prefab = PrefabLoader.loadFromPack("JC/VIPLevelUpUI");
				if(prefab != null)
				{
					GameObject obj = Instantiate(prefab) as GameObject;
					if(obj != null)
					{
						_mInstance = obj.GetComponent<VIPLevelUpUI>();
					
						_mInstance.transform.parent = DBUIController.mDBUIInstance._TopRoot.transform;
						_mInstance.transform.localPosition = Vector3.zero;
						_mInstance.transform.localEulerAngles = Vector3.zero;
						_mInstance.transform.localScale = Vector3.one;
					}
				}
			}
			return _mInstance;
		}
	}
	
	
	public  void Play(int VipLevel)
	{
		Lab_VipLv.text = VipLevel.ToString();
		Invoke("play",0.2f);
	}
	
	void play()
	{
		Core.Data.soundManager.SoundFxPlay(SoundFx.FX_Dragon);
		All.SetActive(true);
		Invoke("VIPFinish",0.8f);
	}
	
	void VIPFinish()
	{
		_box.enabled = true;
	}
	
	public void Close()
	{
		Destroy(gameObject);
		_mInstance = null;
	}
	
	void BtnClick(GameObject btn)
	{
		switch(btn.name)
		{
		case "mask":
			Close();
			break;
		}
	}
	
}
