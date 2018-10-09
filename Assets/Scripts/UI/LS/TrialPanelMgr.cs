using UnityEngine;
using System.Collections;

public class TrialAttributePanel : RUIMonoBehaviour 
{
    public static UITrailAddAttribute CreateShangChengPanel()
    {
        UnityEngine.Object obj = PrefabLoader.loadFromPack("LS/pbLSPrepareEnter_AttributeAdd");
        if(obj != null)
        {
            GameObject go = Instantiate(obj) as GameObject;
            UITrailAddAttribute cc = go.GetComponent<UITrailAddAttribute>();
            return cc;
        }
        return null;
    }
}

public class TrialEnterPanel : RUIMonoBehaviour 
{
    public static UITrailEnter CreateShangChengPanel()
    {
        UnityEngine.Object obj = PrefabLoader.loadFromPack("LS/pbLSPrepareEnter");
        if(obj != null)
        {
            GameObject go = Instantiate(obj) as GameObject;
            UITrailEnter cc = go.GetComponent<UITrailEnter>();
            return cc;
        }
        return null;
    }
}

public class TrialMapPanel : RUIMonoBehaviour 
{
    public static UITrialMap CreateShangChengPanel()
    {
        UnityEngine.Object obj = PrefabLoader.loadFromPack("LS/pbLSPrepareEnterMap");
        if(obj != null)
        {
            GameObject go = Instantiate(obj) as GameObject;
            UITrialMap cc = go.GetComponent<UITrialMap>();
            return cc;
        }
        return null;
    }
}

public class TrialMapNotAttrPanel : RUIMonoBehaviour 
{
	public static UITrialMapNotAdd CreateShangChengPanel()
	{
		UnityEngine.Object obj = PrefabLoader.loadFromPack("LS/pbLSPrepareMapAddAttr");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			UITrialMapNotAdd cc = go.GetComponent<UITrialMapNotAdd>();
			return cc;
		}
		return null;
	}
}

public class FinalTrialMap3D : RUIMonoBehaviour
{
	public static FinalTrial3D Create3DModal()
	{
		UnityEngine.Object obj = PrefabLoader.loadFromPack("LS/LSshow3d");
		if(obj != null)
		{
			GameObject go = UnityEngine.MonoBehaviour.Instantiate(obj) as GameObject;
			FinalTrial3D mStage = go.GetComponent<FinalTrial3D>();
			return mStage;
		}
		return null;
	}
}

