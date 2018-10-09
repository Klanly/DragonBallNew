using UnityEngine;
using System.Collections;
using System.Threading;
///
/// 用来控制粒子发射速率的一段代码,
/// 这个单独控制地面的粒子效果
///
public class ParticleManager  {
	///
	/// 每有3个的时候，就能减少发射速率。 初始情况下，发射速率为每米3个。
	/// 现在修改为：每米发射的个数小于等于3个。
	private const int PerCount = 2;

	/// 
	/// 初始情况下，发射速率为每米2个
	/// 
	private const int INIT_RATE = 2;

	/// <summary>
	/// 如果超过5个预设体，则完全不出现
	/// </summary>
	private const int MAX_COUNT = 4;

	///
	/// 当前创建的粒子系统预设提个数
	///
	private static int currentCount = 0;


	public static GameObject CreateParticle(GameObject Ground_FX, Vector3 position) {
		Interlocked.Increment(ref currentCount);

		int realRate = currentCount / PerCount;
		realRate = INIT_RATE - realRate;
		if(realRate <= 0) realRate = 1;

		if(currentCount >= MAX_COUNT) {
			Interlocked.Decrement(ref currentCount);
			return null;
		}

		GameObject go = EmptyLoad.CreateObj(Ground_FX, new Vector3(position.x, 0, position.x), Quaternion.identity);
		ParticleSystem[] PartTemp = go.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem aPartTemp in PartTemp) {
			aPartTemp.emissionRate = realRate;
		}

		return go;
	}

    public static void ReleaseOne() {
    	Interlocked.Decrement(ref currentCount);
		if(currentCount < 0) currentCount = 0;
    }

}

/// 
/// 单独用来控制爆炸的粒子效果, 目前没有使用
/// 
public class ExploreManager {
	/// <summary>
	/// 如果超过5个预设体，则完全不出现
	/// </summary>
	private const int MAX_COUNT = 8;

	///
	/// 当前创建的粒子系统预设提个数
	///
	private static int currentCount = 0;

	/// 
	/// 只考虑爆炸1， 爆炸2，爆炸3
	/// 
	public static GameObject CreateParticle(GameObject Ground_FX, Vector3 position) {
		GameObject go = null;

		string name = Ground_FX.name;
		if(name == "Baozha_1" || name == "baozha_2" || name == "Baozha_3") {
			Interlocked.Increment(ref currentCount);

			float realRate = 1f;
			if(currentCount >= MAX_COUNT) {
				realRate = MAX_COUNT * 1.0f / currentCount;
				if(realRate > 1f) realRate = 1f;
			}

			go = EmptyLoad.CreateObj(Ground_FX, position, Quaternion.identity);
			ParticleSystem[] PartTemp = go.GetComponentsInChildren<ParticleSystem>();
			foreach (ParticleSystem aPartTemp in PartTemp) {
				aPartTemp.emissionRate *= realRate;
			}

		} else {
			go = EmptyLoad.CreateObj(Ground_FX, position, Quaternion.identity);
		}

		return go;
	}

	public static void ReleaseOne() {
		Interlocked.Decrement(ref currentCount);
		if(currentCount < 0) currentCount = 0;
	}

}