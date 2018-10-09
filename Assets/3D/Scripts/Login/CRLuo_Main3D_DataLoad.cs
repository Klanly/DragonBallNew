using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class LoginSceneConfig {
    //攻击方的Number
    public int AttackID;
    //敌对方的Number
    public int DefenseID;

    //当前的战斗场景
    public short Scene;
    public LoginScene curScene {
        get {
            return (LoginScene)Scene;
        }
    }

    public Action3D[] ActionList;
}

[Serializable]
public class Action3D {
    //延迟的时间
    public float DelayTime;

    //谁, p1 & p2
    public string Who;

    //做了什么
    public string Act;

    public Action3D() {}

    public Action3D(string Act, string who, float delay) {
        this.Act = Act;
        this.Who = who;
        DelayTime = delay;
    }

}

[Serializable]
public class DefaultActionList {
    public Action3D[] ActionList;
}

//登陆的战斗场景
public enum LoginScene {
    LoginScene_1 = 0x0,
    LoginScene_2 = 0x1,
    LoginScene_3 = 0x2,
    LoginScene_4 = 0x3,
}


public class CRLuo_Main3D_DataLoad {
    public List<LoginSceneConfig> configs;
    public List<DefaultActionList> DefaultActionList;

    public CRLuo_Main3D_DataLoad() {

    }

    public void load() {

        string BasePath = "CRLuo/LoginBattle";
        TextAsset TA_LoginBattle = PrefabLoader.loadFromUnPack(BasePath, false, false) as TextAsset;
        byte[] config_LB = TA_LoginBattle.bytes;

        if(config_LB != null) {
            configs = SharedPrefs.loadValue<LoginSceneConfig>(config_LB);
        }
        Resources.UnloadAsset(TA_LoginBattle);
        config_LB = null;

        BasePath = "CRLuo/DefaultAction";
        TextAsset TA_Default = PrefabLoader.loadFromUnPack(BasePath, false, false) as TextAsset;
        byte[] config_DF = TA_Default.bytes;
        if(config_DF != null) {
            DefaultActionList = SharedPrefs.loadValue<DefaultActionList>(config_DF);
        }
        Resources.UnloadAsset (TA_Default);
        config_DF = null;
    }

}
