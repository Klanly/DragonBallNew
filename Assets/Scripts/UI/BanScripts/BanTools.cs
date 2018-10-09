using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class BanTools {

	private static Object _prefab_black;
	public static Object prefab_Black{
		get{
			if(_prefab_black != null)
				return _prefab_black;
			else {
				_prefab_black = BanTools.LoadFromUnPack("Black");
				return _prefab_black;
			} 
		}
	}

	public const bool logEnable = true;

	public static Object LoadFromUnPack(string path){
		return PrefabLoader.loadFromUnPack("Ban/"+path,false);
	}

	public static GameObject CreateObject(Object aObject){
		GameObject temp = EmptyLoad.CreateObj(aObject);
		return temp;
	}

	public static GameObject CreateObject(Object aObject,Vector3 v3){
		GameObject temp = EmptyLoad.CreateObj(aObject,v3,Quaternion.identity);
		return temp;
	}

	public static GameObject CreateNGUIOject(Object aObject,Transform parent){
		Vector3 oldScale = (aObject as GameObject).transform.localScale;
		GameObject temp = EmptyLoad.CreateObj(aObject);
		temp.transform.transform.parent = parent;
		temp.transform.localScale = oldScale;
		return temp;
	}

	public static GameObject CreateNGUIOject(Object aObject,Vector3 v3,Quaternion q4,Transform parent){
		Vector3 oldScale = (aObject as GameObject).transform.localScale;
		GameObject temp = EmptyLoad.CreateObj(aObject,v3,q4);
		temp.transform.transform.parent = parent;
		temp.transform.localScale = oldScale;
		return temp;
	}

	public static string GetAttributeBorderName(BanBattleRole.Attribute aAttribute){
		switch(aAttribute){
        case BanBattleRole.Attribute.Jin: case BanBattleRole.Attribute.SJin:
			return "Attribute_Border_1";
        case BanBattleRole.Attribute.Mu: case BanBattleRole.Attribute.SMu:
			return "Attribute_Border_2";
        case BanBattleRole.Attribute.Shui: case BanBattleRole.Attribute.SShui:
			return "Attribute_Border_3";
        case BanBattleRole.Attribute.Huo: case BanBattleRole.Attribute.SHuo:
			return "Attribute_Border_4";
        case BanBattleRole.Attribute.Tu: case BanBattleRole.Attribute.STu:
			return "Attribute_Border_5";
        case BanBattleRole.Attribute.Quan:
            return "star0";
		default:
			return "";
		}
	}

	public static int GetAttributeState(BanBattleRole.Attribute attackAttribute,BanBattleRole.Attribute defenseAttribute){
        if(attackAttribute == BanBattleRole.Attribute.Jin && (defenseAttribute == BanBattleRole.Attribute.Mu || defenseAttribute == BanBattleRole.Attribute.SMu)){
			return 1;
		}
        if(attackAttribute == BanBattleRole.Attribute.Mu && (defenseAttribute == BanBattleRole.Attribute.Tu || defenseAttribute == BanBattleRole.Attribute.STu)){
			return 1;
		}
        if(attackAttribute == BanBattleRole.Attribute.Tu && (defenseAttribute == BanBattleRole.Attribute.Shui || defenseAttribute == BanBattleRole.Attribute.SShui)){
			return 1;
		}
        if(attackAttribute == BanBattleRole.Attribute.Shui && (defenseAttribute == BanBattleRole.Attribute.Huo || defenseAttribute == BanBattleRole.Attribute.SHuo)){
			return 1;
		}
        if(attackAttribute == BanBattleRole.Attribute.Huo && (defenseAttribute == BanBattleRole.Attribute.Jin || defenseAttribute == BanBattleRole.Attribute.SJin)){
			return 1;
		}

		if(attackAttribute == BanBattleRole.Attribute.Quan && defenseAttribute != BanBattleRole.Attribute.Quan){
			return 1;
		}

        if(attackAttribute == BanBattleRole.Attribute.SJin && (defenseAttribute == BanBattleRole.Attribute.Mu || defenseAttribute == BanBattleRole.Attribute.SMu)) {
            return 1;
        }
        if(attackAttribute == BanBattleRole.Attribute.SMu && (defenseAttribute == BanBattleRole.Attribute.Tu || defenseAttribute == BanBattleRole.Attribute.STu)){
            return 1;
        }
        if(attackAttribute == BanBattleRole.Attribute.STu && (defenseAttribute == BanBattleRole.Attribute.Shui || defenseAttribute == BanBattleRole.Attribute.SShui)){
            return 1;
        }
        if(attackAttribute == BanBattleRole.Attribute.SShui && (defenseAttribute == BanBattleRole.Attribute.Huo || defenseAttribute == BanBattleRole.Attribute.SHuo)){
            return 1;
        }
        if(attackAttribute == BanBattleRole.Attribute.SHuo && (defenseAttribute == BanBattleRole.Attribute.Jin || defenseAttribute == BanBattleRole.Attribute.SJin)){
            return 1;
        }

        if(defenseAttribute == BanBattleRole.Attribute.Jin && (attackAttribute == BanBattleRole.Attribute.Mu || attackAttribute == BanBattleRole.Attribute.SMu )){
			return -1;
		}
        if(defenseAttribute == BanBattleRole.Attribute.Mu && (attackAttribute == BanBattleRole.Attribute.Tu || attackAttribute == BanBattleRole.Attribute.STu)) {
			return -1;
		}
        if(defenseAttribute == BanBattleRole.Attribute.Tu && (attackAttribute == BanBattleRole.Attribute.Shui || attackAttribute == BanBattleRole.Attribute.SShui)){
			return -1;
		}
        if(defenseAttribute == BanBattleRole.Attribute.Shui && (attackAttribute == BanBattleRole.Attribute.Huo || attackAttribute == BanBattleRole.Attribute.SHuo)){
			return -1;
		}
        if(defenseAttribute == BanBattleRole.Attribute.Huo && (attackAttribute == BanBattleRole.Attribute.Jin || attackAttribute == BanBattleRole.Attribute.SJin)){
			return -1;
		}
		if(defenseAttribute == BanBattleRole.Attribute.Quan && attackAttribute != BanBattleRole.Attribute.Quan){
			return -1;
		}

        if(defenseAttribute == BanBattleRole.Attribute.SJin && (attackAttribute == BanBattleRole.Attribute.Mu || attackAttribute == BanBattleRole.Attribute.SMu)) {
            return -1;
        }
        if(defenseAttribute == BanBattleRole.Attribute.SMu && (attackAttribute == BanBattleRole.Attribute.Tu || attackAttribute == BanBattleRole.Attribute.STu)){
            return -1;
        }
        if(defenseAttribute == BanBattleRole.Attribute.STu && (attackAttribute == BanBattleRole.Attribute.Shui || attackAttribute == BanBattleRole.Attribute.SShui)){
            return -1;
        }
        if(defenseAttribute == BanBattleRole.Attribute.SShui && (attackAttribute == BanBattleRole.Attribute.Huo || attackAttribute == BanBattleRole.Attribute.SHuo)){
            return -1;
        }
        if(defenseAttribute == BanBattleRole.Attribute.SHuo && (attackAttribute == BanBattleRole.Attribute.Jin || attackAttribute == BanBattleRole.Attribute.SJin)){
            return -1;
        }

		return 0;
	}

	public static string GetAttributeName(BanBattleRole.Attribute aAttribute){
		switch(aAttribute){
		case BanBattleRole.Attribute.Jin:
			return "Attribute_1";
		case BanBattleRole.Attribute.Mu:
			return "Attribute_2";
		case BanBattleRole.Attribute.Shui:
			return "Attribute_3";
		case BanBattleRole.Attribute.Huo:
			return "Attribute_4";
		case BanBattleRole.Attribute.Tu:
			return "Attribute_5";
        case BanBattleRole.Attribute.Quan:
            return "Attribute_10";
        case BanBattleRole.Attribute.SJin:
            return "Attribute_11";
        case BanBattleRole.Attribute.SMu:
            return "Attribute_12";
        case BanBattleRole.Attribute.SShui:
            return "Attribute_13";
        case BanBattleRole.Attribute.SHuo:
            return "Attribute_14";
        case BanBattleRole.Attribute.STu:
            return "Attribute_15";
		default:
            return "Attribute_Die";
		}
	}

	public static string GetRoleName(int number){
		string name = string.Empty;
		try{
			name = Core.Data.monManager.getMonsterByNum(number).name;
		} catch(System.Exception e){
			ConsoleEx.DebugLog(e.ToString());
		}

		return name;
	}

	public static string GetAttackPlayerName(){
		string name = string.Empty;
		try{
			TemporyData temp = Core.Data.temper;
			if(temp.currentBattleType == TemporyData.BattleType.PVPVideo) {
				name = temp.self_name;
			} else {
				name = Core.Data.playerManager.NickName;
			}
		} catch(System.Exception e){
			ConsoleEx.DebugLog(e.ToString());
		}
		return name;
	}

	public static string GetDefensePlayerName(){
		string name = string.Empty;

		TemporyData temp = Core.Data.temper;
		if(temp.currentBattleType == TemporyData.BattleType.PVPVideo) {
			name = temp.enemy_name;
		} else if(temp.currentBattleType == TemporyData.BattleType.FightWithFulisa) {
			name = Core.Data.stringManager.getString(19);
		} else if(temp.currentBattleType == TemporyData.BattleType.QiangDuoGoldBattle || 
			temp.currentBattleType == TemporyData.BattleType.QiangDuoDragonBallBattle ||
			temp.currentBattleType == TemporyData.BattleType.TianXiaDiYiBattle || 
			temp.currentBattleType == TemporyData.BattleType.SuDiBattle) {
			name = temp._PvpEnemyName;
			if(string.IsNullOrEmpty(name)) name = Core.Data.stringManager.getString(13);
		}  else {
			name = Core.Data.stringManager.getString(13);
		}

		return name;
	}

	public static string Serialize(System.Object obj){
		return fastJSON.JSON.Instance.ToJSON(obj);
	}

	public static BanBattleRole.Attribute TransAttributeType(MonsterAttribute attribute){
		switch(attribute){
		case MonsterAttribute.GOLDEN:
            return BanBattleRole.Attribute.Jin;
		case MonsterAttribute.GOLDEN_S:
            return BanBattleRole.Attribute.SJin;
		case MonsterAttribute.WOOD:
            return BanBattleRole.Attribute.Mu;
		case MonsterAttribute.WOOD_S:
            return BanBattleRole.Attribute.SMu;
		case MonsterAttribute.WATER:
            return BanBattleRole.Attribute.Shui;
		case MonsterAttribute.WATER_S:
            return BanBattleRole.Attribute.SShui;
		case MonsterAttribute.FIRE:
            return BanBattleRole.Attribute.Huo;
		case MonsterAttribute.FIRE_S:
            return BanBattleRole.Attribute.SHuo;
		case MonsterAttribute.SOIL:
            return BanBattleRole.Attribute.Tu;
		case MonsterAttribute.SOIL_S:
            return BanBattleRole.Attribute.STu;
        case MonsterAttribute.ALL:
            return BanBattleRole.Attribute.Quan;
		default:
			Debug.LogWarning("Unknown"+attribute);
			return BanBattleRole.Attribute.Unknown;
		}
	}

	public static void Log(System.Object aObject){
		if(logEnable){
			Debug.Log(aObject);
		}
	}

	public static void LogWarning(System.Object aObject){
		if(logEnable){
			Debug.LogWarning(aObject);
		}
	}

}