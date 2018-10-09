using UnityEngine;
using System.Collections;
using System;

public class FhjPrefabsName
{
	public const string Path = @"FHJ/";

	public const string UICityFloor = "pbUICityFloor";
#if FB2
	public const string UICityItem = "pbUICityItem2";
	public const string UIFloorItem = "pbUIFloorItem2";
	public const string UIFloorBossItem = "pbUIFloorBossItem2";
#else
	public const string UICityItem = "pbUICityItem";
	public const string UIFloorItem = "pbUIFloorItem";
	public const string UIFloorBossItem = "pbUIFloorBossItem";
#endif
	
	
	
}

public class FhjLoadPrefab
{
	public static GameObject GetPrefab (string _name)
	{
		return  PrefabLoader.loadFromPack(FhjPrefabsName.Path + _name) as GameObject;
	}

    public static T GetPrefabClass<T>(string path=FhjPrefabsName.Path) where T : MonoBehaviour
    {
		GameObject go = PrefabLoader.loadFromPack(path + "pb" + typeof(T).ToString()) as GameObject;
        if(go != null)
        {
            go = GameObject.Instantiate(go) as GameObject;
            if(go != null)
            {
                return go.GetComponent<T>();
            }
        }
        return null;
    }
	
}

public class FhjUtility
{
    public static void EaseEffect (GameObject inObj, GameObject outObj)
    {
        if (outObj != null)
        {
            MiniItween.MoveTo(outObj, Vector3.right*100, 0.3f, MiniItween.EasingType.EaseOutQuart, false);
        }
        if (inObj != null)
        {
            MiniItween.MoveTo(inObj, Vector3.zero, 0.5f, MiniItween.EasingType.EaseOutQuart, false);
        }
    }

    public static string GetHead(int id)
    {
        return id.ToString();
    }
    
    public static string GetEquip(int id)
    {
        return "equip" + id.ToString();
    }
    
    public static string GetProps(int id)
    {
        return "props" + id.ToString();
    }
}