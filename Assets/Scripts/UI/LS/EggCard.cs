using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EggCard : RUIMonoBehaviour
{
	public Animation _animation;
	public AnimationClip _AnimationClip;

	static EggCard _mInstance;
	public static EggCard GetInstance()
	{
		return _mInstance;
	}

	public static void Open3D()
	{
		Object obj = PrefabLoader.loadFromPack("LS/pbLSEggCard",true);
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			_mInstance = go.GetComponent<EggCard>();
        }

	}

	void Delete()
	{
		this.dealloc();
	}

	void OnDestroy()
	{
		_animation = null;
		_AnimationClip = null;
	}

	void Start()
	{
		Invoke("Delete", 7f);
	}

}
