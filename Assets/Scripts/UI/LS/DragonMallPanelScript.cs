using UnityEngine;
using System.Collections;

public class DragonMallPanelScript : RUIMonoBehaviour 
{
    public static UIDragonMallMain  CreateShangChengPanel()
    {
        UnityEngine.Object obj = PrefabLoader.loadFromPack("LS/pbLSDragonBallMall");
        if(obj != null)
        {
            GameObject go = Instantiate(obj) as GameObject;
			UIDragonMallMain cc = go.GetComponent<UIDragonMallMain>();
			UIDragonMallMgr.GetInstance().mDragonMallScrollViewControl = go.GetComponentInChildren<DragonMallScrollViewControl>();
            //   RED.TweenShowDialog(go);
            return cc;
        }
        return null;
    }
}

public class DragonMallVipPanelScript : RUIMonoBehaviour 
{
    public static DragonMallVipPanelScript CreateShangChengPanel()
    {
        UnityEngine.Object obj = PrefabLoader.loadFromPack("LS/pbLSDragonBallMallVip");
        if(obj != null)
        {
            GameObject go = Instantiate(obj) as GameObject;
            DragonMallVipPanelScript cc = go.GetComponent<DragonMallVipPanelScript>();
            RED.TweenShowDialog(go);
            return cc;
        }
        return null;
    }
}

public class DragonMallVipTequanPanelScript : RUIMonoBehaviour 
{
      
    public static UIDragonVipTequan CreateShangChengPanel()
    {
        UnityEngine.Object obj = PrefabLoader.loadFromPack("LS/pbLSDragonBallMallVipTequan");
        if(obj != null)
        {
            GameObject go = Instantiate(obj) as GameObject;
            UIDragonVipTequan cc = go.GetComponent<UIDragonVipTequan>();
            RED.TweenShowDialog(go);
            return cc;
        }
        return null;
    }
}

public class  DragonMallVipLibaoPanelScript: RUIMonoBehaviour 
{
    
    public static UIDragonVipLibao CreateShangChengPanel()
    {
        UnityEngine.Object obj = PrefabLoader.loadFromPack("LS/pbLSDragonBallMallVipLibao");
        if(obj != null)
        {
            GameObject go = Instantiate(obj) as GameObject;
            UIDragonVipLibao cc = go.GetComponent<UIDragonVipLibao>();
            RED.TweenShowDialog(go);
            return cc;
        }
        return null;
    }
}


public class  DragonMallVipRechargePanelScript: RUIMonoBehaviour 
{
    
    public static UIDragonVipRecharge CreateShangChengPanel()
    {
        UnityEngine.Object obj = PrefabLoader.loadFromPack("LS/pbLSDragonBallMallRecharge");
        if(obj != null)
        {
            GameObject go = Instantiate(obj) as GameObject;
            UIDragonVipRecharge cc = go.GetComponent<UIDragonVipRecharge>();
            RED.TweenShowDialog(go);
            return cc;
        }
        return null;
    }
}


 public class  DragonMallVipRechargeMainPanelScript: RUIMonoBehaviour 
{
    
    public static UIDragonRechargeMain CreateShangChengPanel()
    {
        UnityEngine.Object obj = PrefabLoader.loadFromPack("LS/pbLSDragonBallMallFanbei");
        if(obj != null)
        {
            GameObject go = Instantiate(obj) as GameObject;
            UIDragonRechargeMain cc = go.GetComponent<UIDragonRechargeMain>();
            RED.TweenShowDialog(go);
            return cc;
        }
        return null;
    }
}



