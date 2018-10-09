using UnityEngine;
using System.Collections;
using RUIType;

public class LevelUpMsgBox : MonoBehaviour
{
    public UISprite headBack;
    public UISprite fiveElement;
    public UISprite head;
    public UILabel Lab_Name;
    public UILabel Lab_afterLevel;
    public UILabel Lab_beforeLevel;
    public UILabel Lab_Atk;
    public UILabel Lab_Def;
    public UILabel Lab_Exp;
    public UILabel lab_Title;
    public UISlider exp_slider;
    int tCountTimes = 0;
    //  int countNum = 0;
    float targetValue = 0;
    bool SetValueBar = false;
    int privateBeforeLevel = 0;
    int curObjStar = 0;
    int gAtk = 0;
    int gDef = 0;
    int tepAtk = 0;
    int tepDef = 0;
    private object m_data;
    private readonly int  MaxLevel = 60;
    private ConfigDataType m_type;
    private static LevelUpMsgBox _mInstance;

    private float growthAtkParam = 0;
    private float growthDefParam = 0;

    public static void OpenUI (object data, ConfigDataType type)
    {
        if (_mInstance == null) {
            Object prefab = PrefabLoader.loadFromPack ("JC/LevelUpMsgBox");
            if (prefab != null) {
                GameObject obj = Instantiate (prefab) as GameObject;
                RED.TweenShowDialog (obj);
                RED.AddChild (obj, DBUIController.mDBUIInstance._TopRoot);
                _mInstance = obj.GetComponent<LevelUpMsgBox> ();
            }
        } else {
            RED.SetActive (true, _mInstance.gameObject);
        }
        _mInstance.Init (data, type);
    }

    public static LevelUpMsgBox Instance {
        get{ return _mInstance; }
    }

    void Init (object data, ConfigDataType type)
    {
        m_data = data;

        m_type = type;
        growthAtkParam = 0;
        growthDefParam = 0;

        if (m_type == ConfigDataType.Monster) {
            Monster mon = data as Monster;

            growthAtkParam = Core.Data.monManager.GetMonUpAtkParam (mon.config.star,mon.Star) ;
            growthDefParam = Core.Data.monManager.GetMonUpDefParam (mon.config.star,mon.Star) ;

            headBack.spriteName = "star" + mon.RTData.m_nAttr.ToString ();
            RED.SetActive (true, fiveElement.gameObject);
            fiveElement.spriteName = "Attribute_" + ((int)(mon.RTData.Attribute)).ToString ();
            AtlasMgr.mInstance.SetHeadSprite (head, mon.num.ToString ());
            Lab_Name.text = mon.config.name;
        } else if (m_type == ConfigDataType.Equip) {
            Equipment equip = data as Equipment;
            head.atlas = AtlasMgr.mInstance.equipAtlas;
            head.spriteName = equip.Num.ToString ();
            RED.SetActive (false, fiveElement.gameObject);
            headBack.spriteName = "star6";
            Lab_Name.text = equip.ConfigEquip.name;
        }

        InitDataUI ();
    }

    void InitDataUI ()
    {
        lab_Title.text = Core.Data.stringManager.getString (5156);
        if (m_type == ConfigDataType.Monster) {
            Monster nowData = m_data as Monster;
            Monster preMon = Core.Data.temper.preMonsterData;
            curObjStar = preMon.Star;
            privateBeforeLevel = preMon.RTData.curLevel;
			
            if (nowData.RTData.curLevel > preMon.RTData.curLevel) {
                Lab_Atk.text = "[7DA7FF]"+Core.Data.stringManager.getString(5103)+":[-]   [7DA7FF]" + preMon.getAttack.ToString () + "[-][7DA7FF]+" + ((int)preMon.config.atkGrowth * growthAtkParam).ToString () + "[-]";
                Lab_Def.text = "[baff41]" +Core.Data.stringManager.getString(5104)+":[-]   [BAFF41]" + preMon.getDefend.ToString () + "[-][BAFF41]+" + ((int)preMon.config.defGrowth * growthDefParam).ToString () + "[-]";
                int nextExp = Core.Data.monManager.getMonsterNextLvExp (preMon.Star, preMon.RTData.curLevel);
                Lab_Exp.text = preMon.RTData.curExp.ToString () + "/" + nextExp.ToString ();
                exp_slider.value = (float)preMon.RTData.curExp / +(float)nextExp;
                gAtk = 0;
                gDef = 0;
	               
            } else {
                Lab_Atk.text = "[7DA7FF]"+Core.Data.stringManager.getString(5103)+":[-]   [7DA7FF]" + (preMon.getAttack * growthAtkParam).ToString ();
                Lab_Def.text = "[baff41]"+Core.Data.stringManager.getString(5104)+":[-]   [BAFF41]" + (preMon.getDefend* growthDefParam).ToString ();
                int nextExp = Core.Data.monManager.getMonsterNextLvExp (preMon.Star, preMon.RTData.curLevel);
                Lab_Exp.text = preMon.RTData.curExp.ToString () + "/" + nextExp.ToString ();
                exp_slider.value = (float)preMon.RTData.curExp / +(float)nextExp;
            } 
        } else if (m_type == ConfigDataType.Equip) {
            Equipment nowData = m_data as Equipment;
            Equipment preEquip = Core.Data.temper.preEquipData;
            curObjStar = preEquip.ConfigEquip.star;
            privateBeforeLevel = preEquip.RtEquip.lv;


            if (nowData.RtEquip.lv > privateBeforeLevel) {
                Lab_Atk.text = "[7DA7FF]" +Core.Data.stringManager.getString(5103)+ ":[-]   [7DA7FF]" + preEquip.getAttack.ToString () + "[-][7DA7FF]+" + ((int)preEquip.ConfigEquip.atkGrowth).ToString () + "[-]";
                Lab_Def.text = "[baff41]" +Core.Data.stringManager.getString(5104)+":[-]   [BAFF41]" + preEquip.getDefend.ToString () + "[-][BAFF41]+" + ((int)preEquip.ConfigEquip.defGrowth).ToString () + "[-]";
                int nextExp = Core.Data.EquipManager.GetEquipUpExp ( preEquip.RtEquip.lv,preEquip.ConfigEquip.star);
                Lab_Exp.text = preEquip.RtEquip.exp.ToString () + "/" + nextExp.ToString ();
                exp_slider.value = (float)preEquip.RtEquip.exp / +(float)nextExp;
                gAtk = 0;
                gDef = 0;
				
            } else {
                Lab_Atk.text = "[7DA7FF]"+Core.Data.stringManager.getString(5103)+":[-]   [7DA7FF]" + preEquip.getAttack.ToString ();
                Lab_Def.text = "[baff41]"+Core.Data.stringManager.getString(5104)+":[-]   [BAFF41]" + preEquip.getDefend.ToString ();
                int nextExp = Core.Data.EquipManager.GetEquipUpExp (preEquip.RtEquip.lv,preEquip.ConfigEquip.star);
                Lab_Exp.text = preEquip.RtEquip.exp.ToString () + "/" + nextExp.ToString ();
                exp_slider.value = (float)preEquip.RtEquip.exp / +(float)nextExp;
            } 
         
        }
        LevelUpEffect();
        Lab_beforeLevel.text = "Lv" + privateBeforeLevel.ToString ();
    }

    void LevelUpEffect ()
    {
        if (m_type == ConfigDataType.Monster) {
            Monster nowData = m_data as Monster;
            Monster preMon = Core.Data.temper.preMonsterData;
            tCountTimes = nowData.RTData.curLevel - preMon.RTData.curLevel;
            tepAtk = (int)(nowData.config.atkGrowth*growthAtkParam + 0.5f);
            tepDef = (int)(nowData.config.defGrowth*growthDefParam +0.5f);

            targetValue = (float)nowData.RTData.curExp / (float)Core.Data.monManager.getMonsterNextLvExp (nowData.Star, nowData.RTData.curLevel);
            if(targetValue >=1)
                targetValue = 0.99f;
            SetValueBar = true;
        } else if (m_type == ConfigDataType.Equip) {
            Equipment nowData = m_data as Equipment;
            Equipment preEquip = Core.Data.temper.preEquipData;
            tCountTimes = nowData.RtEquip.lv - preEquip.RtEquip.lv;
            tepAtk = (int)(nowData.ConfigEquip.atkGrowth + 0.5f);
            tepDef = (int)(nowData.ConfigEquip.defGrowth+0.5f);
            if (Core.Data.EquipManager.GetEquipUpExp (nowData.RtEquip.lv,nowData.ConfigEquip.star ) != 0) {
                targetValue = (float)nowData.RtEquip.exp / (float)Core.Data.EquipManager.GetEquipUpExp (nowData.RtEquip.lv,nowData.ConfigEquip.star );
                // Debug.Log("target  value eeeeee  === " + targetValue +" dinggggg   == " + (float)nowData.RtEquip.exp +   "   dishu " +(float)Core.Data.EquipManager.GetEquipUpExp (nowData.RtEquip.lv,nowData.ConfigEquip.star ) );
            }
            if (targetValue >= 1)
                targetValue = 0.99f;
            
            SetValueBar = true;
        }

    }

    void Update ()
    {
        if (SetValueBar == true) {
            // if (m_type == ConfigDataType.Monster)
            //   {
            if (exp_slider.value < 1) {
                int tempExp = 0;
                if (m_type == ConfigDataType.Monster) {
                    tempExp = Core.Data.monManager.getMonsterNextLvExp (curObjStar, privateBeforeLevel);
                } else {
                    tempExp = Core.Data.EquipManager.GetEquipUpExp (privateBeforeLevel,curObjStar);

                }
                int temB = (int)((float)exp_slider.value * (float)tempExp);
                if (tCountTimes <= 0) {

                    exp_slider.value += Time.smoothDeltaTime * 2;

                    if (exp_slider.value >= targetValue) {
                        exp_slider.value = targetValue;

                        this.SetResult ();
                        SetValueBar = false;
                    }

                } else {
                    exp_slider.value += Time.smoothDeltaTime * 2; 
                    if (tepAtk > 0) {
                        gAtk++;
                        tepAtk--;
//                        Debug.Log(" gAtk  == " + gAtk + "   ;;;;   tep Atk==  " + tepAtk);
                    }
                    if (tepDef > 0) {
                        gDef++;
                        tepDef--;
                    }
                    Lab_Exp.text = temB.ToString () + "/" + tempExp.ToString ();
                }

                if (gAtk != 0 ) {
                    if (m_type == ConfigDataType.Monster) {
                        Lab_Atk.text = "[7DA7FF]"+Core.Data.stringManager.getString(5103)+":[-]   [7DA7FF]" + Core.Data.temper.preMonsterData.getAttack.ToString () + "[-][7DA7FF]+" + (gAtk*growthAtkParam).ToString () + "[-]";
                    } else if (m_type == ConfigDataType.Equip) {
                        if (gAtk != 0) {
                            Lab_Atk.text = "[7DA7FF]"+Core.Data.stringManager.getString(5103)+":[-]   [7DA7FF]" + Core.Data.temper.preEquipData.getAttack.ToString () + "[-][7DA7FF]+" + gAtk.ToString () + "[-]";
                        } else {
							Lab_Atk.text = "[7DA7FF]"+Core.Data.stringManager.getString(5103)+":[-]   [7DA7FF]" + Core.Data.temper.preEquipData.getAttack.ToString ();
                        }
                    }

                }


                if (gDef != 0)
                { 
                    if (m_type == ConfigDataType.Monster)
                    {
                        Lab_Def.text = "[baff41]" + Core.Data.stringManager.getString(5104) + ":[-]   [BAFF41]" + Core.Data.temper.preMonsterData.getDefend.ToString() + "[-][BAFF41]+" + (gDef*growthDefParam) .ToString() + "[-]";
                    }
                    else if (m_type == ConfigDataType.Equip)
                    {
                        if (gDef != 0) {
							Lab_Def.text = "[baff41]"+Core.Data.stringManager.getString(5104)+":[-]   [BAFF41]" + Core.Data.temper.preEquipData.getDefend.ToString () + "[-][BAFF41]+" + gDef.ToString () + "[-]";
                        } else {
                            Lab_Def.text = "[baff41]"+Core.Data.stringManager.getString(5104)+":[-]   [BAFF41]" + Core.Data.temper.preEquipData.getDefend.ToString ();
                        }
                    }
                }
            } else {

                if (privateBeforeLevel < MaxLevel)
                {
                    // countNum++;
                    tCountTimes--; 
                    exp_slider.value = 0;
                    privateBeforeLevel++;
                    Lab_beforeLevel.text = "Lv" + privateBeforeLevel;

                    if (m_type == ConfigDataType.Monster) {
                        Monster mon = m_data as Monster;
                        gAtk += tepAtk;
                        gDef += tepDef;
                        tepAtk = (int)(mon.config.atkGrowth * growthAtkParam + 0.5f);
                        tepDef = (int)(mon.config.defGrowth * growthDefParam+ 0.5f);
                    } else if (m_type == ConfigDataType.Equip) {
                        Equipment equip = m_data as Equipment;
                        gAtk += tepAtk;
                        gDef += tepDef;
                        tepAtk = (int)(equip.ConfigEquip.atkGrowth+0.5f);
                        tepDef = (int)(equip.ConfigEquip.defGrowth +0.5f);
                    }

                    if (gAtk != 0) {
                        if (m_type == ConfigDataType.Monster) {
                            Lab_Atk.text = "[7DA7FF]"+Core.Data.stringManager.getString(5103)+":[-]   [7DA7FF]" + Core.Data.temper.preMonsterData.getAttack.ToString () + "[-][7DA7FF]+" +  tepAtk.ToString () + "[-]";
                            Lab_Def.text = "[baff41]"+Core.Data.stringManager.getString(5104)+":[-]   [BAFF41]" + Core.Data.temper.preMonsterData.getDefend.ToString () + "[-][BAFF41]+" + tepDef.ToString () + "[-]";
                        } else if (m_type == ConfigDataType.Equip) {
                            Lab_Atk.text = "[7DA7FF]"+Core.Data.stringManager.getString(5103)+":[-]   [7DA7FF]" + Core.Data.temper.preEquipData.getAttack.ToString () + "[-][7DA7FF]+" + tepAtk.ToString () + "[-]";
                            Lab_Def.text = "[baff41]"+Core.Data.stringManager.getString(5104)+":[-]   [BAFF41]" + Core.Data.temper.preEquipData.getDefend.ToString () + "[-][BAFF41]+" + tepDef.ToString () + "[-]";
                        }
                    }
                }
                else
                {
                    tCountTimes = 0;
                    exp_slider.value = 0.99f;
                    SetValueBar =false;
                }
               
            }
        }
    }

    public void SetResult ()
    {

        if (m_type == ConfigDataType.Monster) {
            Monster mon = m_data as Monster;
            int nextExp = Core.Data.monManager.getMonsterNextLvExp (mon.Star, mon.RTData.curLevel);
            Lab_Exp.text = mon.RTData.curExp.ToString () + "/" + nextExp.ToString (); 
            exp_slider.value = (float)mon.RTData.curExp / +(float)nextExp;

        } else if (m_type == ConfigDataType.Equip) {
            Equipment equip = m_data as Equipment;
            int nextExp = Core.Data.EquipManager.GetEquipUpExp (equip.RtEquip.lv + 1, equip.ConfigEquip.star);
            Lab_Exp.text = equip.RtEquip.exp.ToString () + "/" + nextExp.ToString ();      
            exp_slider.value = (float)equip.RtEquip.exp / +(float)nextExp;
        }
       
    }

    public void OnClose ()
    {
        _mInstance = null;
        StopCoroutine ("LevelUpEffect");
        SetValueBar = false;
        this.SetResult ();
        
		if (Core.Data.guideManger.isGuiding)
		{
			Core.Data.guideManger.AutoRUN ();
		}

        Destroy (gameObject);

        if (m_type == ConfigDataType.Monster) {
            if (MonsterInfoUI.mInstance != null) {
                MonsterInfoUI.mInstance.RefreshUI (m_data as Monster);
            } else {  
                DBUIController.mDBUIInstance._petBoxCtl.SetPetBoxType (EMBoxType.QiangHua);
            }
            Core.Data.temper.preMonsterData = m_data as Monster;
        } else {
            Equipment equip = m_data as Equipment;
            if (equip.ConfigEquip.type == 0) {
                DBUIController.mDBUIInstance._petBoxCtl.SetPetBoxType (EMBoxType.Equip_QH_ATK);
            } else {
                DBUIController.mDBUIInstance._petBoxCtl.SetPetBoxType (EMBoxType.Equip_QH_DEF);
            }
            Core.Data.temper.preEquipData = equip;
        }
    }


   
}
