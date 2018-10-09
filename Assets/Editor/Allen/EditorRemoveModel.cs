using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class EditorRemoveModel : EditorWindow
{

    public static EditorRemoveModel m_Window;
	public static string m_SaveResourcesModels = "10107,10116,10117,10135,10140,10142,10148,10150,10165,10169,10172,10173,10174,10175,10178,10191,10193,10211,10223,19998,19999";

    public static void CleanCache()
    {
        Caching.CleanCache();
        string path = Application.dataPath.Replace("Assets", "") + "/VerRes.bytes";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("", "CleanCache", "OK");
    }

    public static void RemoveModel()
    {
        CheckWindow();
    }

    static void CheckWindow()
    {
        if (m_Window == null)
        {
            m_Window = (EditorRemoveModel)GetWindow(typeof(EditorRemoveModel));
        }
    }

    void OnGUI()
    {
        m_SaveResourcesModels = EditorGUILayout.TextField("SaveResourcesModels ", m_SaveResourcesModels);
        GUILayout.Label("SaveResourcesModels", EditorStyles.boldLabel);
		if (GUILayout.Button("Move 3D Model to AssetBundle folder"))
        {
			SetRemoveModels(true);
            AssetDatabase.Refresh();
			EditorUtility.DisplayDialog("Move 3D Models", "3D Models moved to AssetBundle folder successfully.", "OK");
            Debug.Log("SaveResourcesModels Finish!!!!!!");
        }

		if(GUILayout.Button("Move 3D Model to AssetBundleEx folder")) {
			SetRemoveModels(false);
			AssetDatabase.Refresh();
			EditorUtility.DisplayDialog("Move 3D Models", "3D Models moved to AssetBundleEx folder successfully.", "OK");
			Debug.Log("SaveResourcesModels Ex Finish!!!!!!");
		}

    }

	/// <summary>
	/// Pos == true 则保留Charactor，否则保留CharactorEx
	/// </summary>
	static void SetRemoveModels(bool Pos)
    {
        List<int> list = str2IntList(m_SaveResourcesModels);
		string path = null;
		if(Pos)
			path = Application.dataPath + "/Resources/Pack/CRLuo/Charactor";
		else
			path = Application.dataPath + "/Resources/Pack/CRLuo/CharactorEx";

        string destPath = Application.dataPath + "/AssetBundles/Charactor";
        if (!Directory.Exists(destPath))
        {
            Directory.CreateDirectory(destPath);
        } else
        {
            string[] paths = Directory.GetFiles(destPath);
            for(int i= 0;i<paths.Length;i++){
                if (File.Exists(paths [i]))
                {
                    File.Delete(paths [i]);
                }
            }
        }
        string[] assetPaths = Directory.GetFiles(path);
        foreach (string assetPath in assetPaths)
        {
            //.prefab.meta
            if (assetPath.EndsWith(".prefab") || assetPath.EndsWith(".meta"))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(assetPath);
                string assetName = dirInfo.Name;
                assetName = assetName.Replace(".prefab", "").Replace(".meta", "").Replace("pb", "");
                int ID = int.Parse(assetName);
                if (!list.Contains(ID))
                {
//                    Debug.Log(assetPath);
//                    Debug.Log(destPath + "/" + dirInfo.Name);
                    FileInfo info = new FileInfo(assetPath);
                    if (File.Exists(destPath + "/" + dirInfo.Name))
                    {
                        File.Delete(destPath + "/" + dirInfo.Name);
                    }
                    info.MoveTo(destPath + "/" + dirInfo.Name);
                }
            }
        }

		RemoveModels(Pos);
    }

	/// <summary>
	/// 删除不需要的模型. Pos == true 则保留Charactor，否则保留CharactorEx
	/// </summary>
	static void RemoveModels(bool Pos) {
		string path = null;
		if(Pos) {
			path = Application.dataPath + "/Resources/Pack/CRLuo/CharactorEx";
		} else {
			path = Application.dataPath + "/Resources/Pack/CRLuo/Charactor";
		}

		try {
			Directory.Delete(path, true);
		} catch(System.Exception ex) {
			ConsoleEx.DebugLog(ex.Message, ConsoleEx.RED);
		}
	}

    public static List<int> str2IntList(string str)
    {
        List<int> ret = new List<int>();
        string[] ss = str.Split(new char[]{ ',' }, System.StringSplitOptions.RemoveEmptyEntries);
        foreach (string s in ss)
        {
            int n = 0;
            if (int.TryParse(s, out n))
            {
                ret.Add(n);
            }
        }
        return ret;
    }
}
