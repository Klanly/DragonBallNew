using UnityEngine;
using System.Collections.Generic;

using UObject = UnityEngine.Object;

public class MonoBehaviourEx : MonoBehaviour {

	/// <summary>
	/// 把所有PrefabLoader加载进来的Ojbect都放在这里面，然后统一释放
	/// </summary>
	protected Dictionary<short, UObject> UObjDic = new Dictionary<short, UObject>();

	/// <summary>
	/// Remove all element in UObjDic
	/// </summary>
	protected virtual void dealloc() {
		UObjDic.saftyFree();
	}

	protected virtual void destoryGo(List<GameObject> goList) {
		if(goList != null) {
			foreach(GameObject go in goList){
				Destroy(go);
			}
		}
		goList.safeFree();
	}

}


