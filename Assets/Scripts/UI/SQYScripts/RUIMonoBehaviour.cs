using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 所以的UI 暂时先继承这个类,防止未来统一修改
/// </summary>
public class RUIMonoBehaviour : MonoBehaviour {
	public virtual void SetActive(bool active)
	{
		this.gameObject.SetActive(active);
	}

	public virtual void release()
	{

	}
}
