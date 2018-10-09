using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmptyLoad : MonoBehaviour {
	private static bool b_NeedReleaseResource = true;
	private static List<Object> list_NotRelease = new List<Object>();
	private static List<Object> list_Release = new List<Object>();
	private static List<GameObject> list_ResumedObj = new List<GameObject>();
	private static string _loadedLevelName;
	public static void Load(string loadedLevelName){
		_loadedLevelName = loadedLevelName;
		Application.LoadLevel("EmptyLoad");
	}

	public static void StateReleaseTextures(){
		Debug.Log("----StateReleaseTexture Start----");
		foreach(Texture2D aTexture2D in list_Release){
			Debug.Log(aTexture2D.name);
		}
		Debug.Log("----StateReleaseTexture End----");
	}


	// 不需要释放资源的在这里声明下

	public static void NeedNotRelease(Object aObject){
		if(b_NeedReleaseResource){
			if(!list_NotRelease.Contains(aObject)){
				list_NotRelease.Add(aObject);
			}
		}
	}

	//增加一个需要释放的图片
	public static void AddReleaseTexture(Texture2D aTexture){
		if(!list_Release.Contains(aTexture)){
			list_Release.Add(aTexture);
		}
	}
	
	public static GameObject SimpleCreate(Object go){
		return (GameObject)GameObject.Instantiate(go);
	}

	public static GameObject SimpleCreate(Object go,Vector3 v3,Quaternion q4){
		return (GameObject)GameObject.Instantiate(go,v3,q4);
	}

	//等同 Instantiate
	public static GameObject CreateObj(Object go){
		ResumeTextures(go as GameObject);
		return (GameObject)GameObject.Instantiate(go);
	}

	//等同 Instantiate
	public static GameObject CreateObj(Object go,Vector3 v3,Quaternion q4){
		ResumeTextures(go as GameObject);
		return (GameObject)GameObject.Instantiate(go,v3,q4);
	}

	//修正图片
	public static void ResumeTextures(GameObject aGo){
		if(b_NeedReleaseResource){
			if(!list_ResumedObj.Contains(aGo)){
				list_ResumedObj.Add(aGo);
				DoResumeTextures(aGo);
				Check(aGo);
			}
		}
	}

	//马上修正图片
	public static void DoResumeTextures(GameObject aGo){
		if(aGo) {
			if(aGo.renderer != null ){
				if(aGo.renderer.sharedMaterials != null && aGo.renderer.sharedMaterials.Length > 0 ){
					for(int i = 0;i<aGo.renderer.sharedMaterials.Length;i++){
						if(aGo.renderer.sharedMaterials[i] != null){
							aGo.renderer.sharedMaterials[i].mainTexture = aGo.renderer.sharedMaterials[i].mainTexture;
						}
					}
				}
			}
			Renderer [] renders = GetRendersInChildren(aGo,false);
			foreach(Renderer aRender in renders){
                if(aRender != null && aRender.sharedMaterial != null){
                    if(aRender.sharedMaterial.HasProperty("_MainTex"))
    					aRender.sharedMaterial.mainTexture = aRender.sharedMaterial.mainTexture;
				}
			}
		}
	}

	public static Renderer [] GetRendersInChildren(GameObject aGo,bool b_IncludeSelf){
		List<Renderer> list_Renders = new List<Renderer>();
		List<Transform> list_Open = new List<Transform>();
		List<Transform> list_Close = new List<Transform>();
		list_Open.Add(aGo.transform);
		while(list_Open.Count > 0){
			Transform firstTransform = list_Open[0];
			int length = firstTransform.childCount;
			if(length > 0) {
				for(int i = 0;i<length;i++){
					list_Open.Add(firstTransform.GetChild(i));
				}
			}
			list_Close.Add(firstTransform);
			list_Open.RemoveAt(0);
		}
		if(!b_IncludeSelf){
			list_Close.Remove(aGo.transform);
		}

		foreach(Transform aTransform in list_Close){
			if(aTransform.renderer != null){
				list_Renders.Add(aTransform.renderer);
			}
		}
		return list_Renders.ToArray();
	}

	public static void Check(GameObject go){
		if(b_NeedReleaseResource){
			List<Texture2D> list_textures= GetTextures(go);
			foreach(Texture2D aTexture2D in list_textures){
				if(!list_Release.Contains(aTexture2D) && !list_NotRelease.Contains(aTexture2D) ){
					list_Release.Add(aTexture2D);
				}
			}
		}
	}

	public static void DoRelease(){
		if(b_NeedReleaseResource){
			if(list_Release.Count > 0){
				foreach(Object aObject in list_Release){
					Resources.UnloadAsset(aObject);
				}
				list_Release.Clear();
			}
			if(list_ResumedObj.Count > 0){
				list_ResumedObj.Clear();
			}
		}
	}

	public static List<Texture2D> GetTextures(GameObject go){
		List<Renderer> list_Render = new List<Renderer>();
		//Check each
		List<Texture2D> list_Textures = new List<Texture2D>();

		if(go == null) return list_Textures;
		//Main Render
		if(go.renderer != null){
			list_Render.Add(go.renderer);
		}
		//Child Render
		Renderer [] childRenders = go.GetComponentsInChildren<Renderer>();
		if(childRenders.Length > 0){
			foreach(Renderer aRender in childRenders){
				if(aRender != null){
					list_Render.Add(aRender);
				}
			}
		}

		if(list_Render != null && list_Render.Count > 0 ){
			foreach(Renderer aRender in list_Render){
				if(aRender != null){
					Material [] materials = aRender.sharedMaterials;
					if(materials != null && materials.Length > 0){
						foreach(Material aMaterial in materials){
							if(aMaterial!=null&& aMaterial.mainTexture != null ){
								if(!list_Textures.Contains(aMaterial.mainTexture as Texture2D)){
									list_Textures.Add(aMaterial.mainTexture as Texture2D);
								}
							}
						}
					}
				}
			}
		}

		return list_Textures;
	}

	void Awake () {
		if(b_NeedReleaseResource){
			DoRelease();
		}
		Application.LoadLevel(_loadedLevelName);
	}

}
