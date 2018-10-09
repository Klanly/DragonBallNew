using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using AW.Resources;

public class CountryRegion {
	#if China
	public const string country = "China";
	#elif US
	public const string country = "US";
	#elif Korea
	public const string country = "Korea";
	#elif TW
	public const string country = "TW";
	#else
	public const string country = "China";
	#endif
}

/// <summary>
/// Model loader. 载入3D模型的帮助类
/// </summary>
public class ModelLoader {
	private const string ROOT = "Pack";
	//Mr Luo's root folder
	private const string RootFolder = "CRLuo";
	//普通对决位置
	private const string Charactor = "Charactor";
	//复杂对决位置
	private const string CharactorEx = "CharactorEx";

	/// <summary>
	/// Get the 3D model.全部手动释放
	/// </summary>
	/// <returns>The D model.</returns>
	/// <param name="petNum">Pet number.</param>
	public static Object get3DModel(int monsterNum) {
		Object obj = null;
		string path = get3DModelPath(monsterNum);
		obj = Resources.Load(path);
		return obj;
	}

	/// <summary>
	/// 获取3D模型的路径
	/// </summary>
	static string get3DModelPath(int monsterNum) {
		string path = Path.Combine(ROOT, RootFolder);

		#if VS
		path = Path.Combine(path, CharactorEx);
		#else
		path = Path.Combine(path, Charactor);
		#endif

		path = Path.Combine(path, PrefabLoader.RES_PREFIX_PREFAB + monsterNum);
		return path;
	}

}

/// <summary>
/// Model loader, 载入3D模型的帮助类。这个是分包载入的代码
/// </summary>
public class SplitModelLoader : IAssetManager {

    #if UNITY_IPHONE
    private const int CAPACITY = 10;
    private const int MAX = 5;

    #elif UNITY_ANDROID
    private const int CAPACITY = 20;
    private const int MAX = 10;

    #elif UNITY_WP8
    private const int CAPACITY = 30;
    private const int MAX = 10;

    #else
    private const int CAPACITY = 50;
    private const int MAX = 30;

    #endif

    public SplitModelLoader () : base(CAPACITY, MAX) { }

    public override void RefAsset (string name) {
        int cached = lstRefAsset.Get(name);
        if(cached == 0) {
            string[] toBeRm = lstRefAsset.Add(name, 1);
            if(toBeRm != null && toBeRm.Length > 0) {
                foreach(string assetName in toBeRm) {
                    Core.ResEng.UnrefAsset(assetName);
                }

				///释放了资源的引用之后应该释放内存
				Resources.UnloadUnusedAssets();
            }
        } 
    }

}

public enum LoadType {
	GAMEOBJECT,
	TEXTURE_2D,
	OTHER_RES,
}

/// <summary>
/// Prefab loader.
/// 载入预设体模型的帮助类
/// </summary>
public class PrefabLoader {

	private const string PACKROOT = "Pack";
	private const string UNPACKROOT = "UnPack";

	public const string RES_PREFIX_PREFAB = @"pb";
	public const string RES_PREFIX_TEXTURE = @"tex2D";

	public static Dictionary<string, Object> cachedAsset = new Dictionary<string, Object>();

	/// <summary>
	/// Loads from unpack folder. country参数决定是否要读取 国家分类
	/// </summary>
	/// <returns>The Object from unack.</returns>
	/// <param name="name">Name.</param>
	public static Object loadFromUnPack (string name, bool country = true, bool cached = false) {
		Object obj = null;

		if(!string.IsNullOrEmpty(name)) {

			#if VS
			if(name == "CRLuo/pbXXX") {
				name += "Ex";
			}
			#endif

			string path = string.Empty;
			if(country) {
				path = Path.Combine (CountryRegion.country, UNPACKROOT);
				path = Path.Combine (path, name);
			} else {
				path = Path.Combine(UNPACKROOT, name);
			}

			if(cachedAsset.ContainsKey(path)) {
				obj = cachedAsset[path];
			} else {
				obj = Resources.Load (path);
				if(cached) {
					cachedAsset.Add(path, obj);
				}
			}

		}

		return obj;
	}

	/// <summary>
	/// Loads from pack fold. country参数决定是否要读取 国家分类
	/// </summary>
	/// <returns>The from pack.</returns>
	/// <param name="name">Name.</param>
	/// <param name="country">If set to <c>true</c> country.</param>
	public static Object loadFromPack (string name, bool country = true, bool cached = false) {
		Object obj = null;

		if( !string.IsNullOrEmpty(name) ) {

			#if Split
			throw new System.NotImplementedException();
			#else
			if(country) {
				name = Path.Combine(Path.Combine (CountryRegion.country, PACKROOT), name);
			} else {
				name = Path.Combine(PACKROOT, name);
			}

			if(cachedAsset.ContainsKey(name)) {
				obj = cachedAsset[name];
			} else {
				obj = Resources.Load (name);
				if(cached) {
					cachedAsset.Add(name, obj);
				}
			}
			#endif		

		}

		return obj;
	}

	/// <summary>
	/// Cleans all Assets
	/// </summary>
	public static void CleanAll() {
		if(cachedAsset.Count > 0) {
			#if Split
            throw new System.NotImplementedException();
			#else
			foreach(Object o in cachedAsset.Values) {
				Object.Destroy(o);
			}
			#endif

			cachedAsset.Clear();
			Resources.UnloadUnusedAssets();
			System.GC.Collect();
		}
	}

	/// <summary>
	/// Cleans the Special One.
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="country">If set to <c>true</c> country.</param>
    public static void CleanOneSpecial(string name, bool Pack = false,  bool country = true) {
		#if Split
		throw new System.NotImplementedException();
		#else
		if(country) {
            if(Pack) {
                name = Path.Combine(Path.Combine (CountryRegion.country, PACKROOT), name);
            } else {
                name = Path.Combine(Path.Combine (CountryRegion.country, UNPACKROOT), name);
            }
		} else {
            if(Pack) {
                name = Path.Combine(PACKROOT, name);
            } else {
                name = Path.Combine(UNPACKROOT, name);
            }
		}

		if(cachedAsset.ContainsKey(name)) {
			Object obj = cachedAsset[name];
			cachedAsset.Remove(name);
			obj = null;
		}
		
		#endif
	}
}