using UnityEngine;
using System.Collections;

/// 
/// 专门用来收集爆炸粒子效果销毁的信息
/// 
public class BaozhaCollection : MonoBehaviour {
	void OnDestroy() {
		ExploreManager.ReleaseOne();
	}
}
