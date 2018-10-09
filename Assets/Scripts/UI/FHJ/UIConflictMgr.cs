using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UIConflictMgr : MonoBehaviour {
    public UIGrid _grid;
    string _prefabItem = "pbUIConflictItem";

	// Use this for initialization
	void Start () {
        SetItem ();
	}
	
    void SetItem() {
		TemporyData temp = Core.Data.temper;

        List<BanBattleRole> atksidelist = BanBattleManager.Instance.GetRoleList (BanBattleRole.Group.Attack);

        List<BanBattleRole> defsidelist = BanBattleManager.Instance.GetRoleList (BanBattleRole.Group.Defense);
    
        NewFloor floor = Core.Data.newDungeonsManager.curFightingFloor;
		bool side = false;
		if(floor != null)
            side = floor.config.FightType == 0;

        int itemNum = atksidelist.Count > defsidelist.Count ? atksidelist.Count : defsidelist.Count;
        RED.RemoveChildsImmediate (_grid.transform);
        for (int i = 0; i < itemNum; ++i) {
            GameObject pbitem = FhjLoadPrefab.GetPrefab (_prefabItem);
            UIConflictItem item = NGUITools.AddChild(_grid.gameObject, pbitem).GetComponent<UIConflictItem>();
            item.gameObject.name = "item" + i;
			if (atksidelist.Count > i) {
				if(temp.currentBattleType == TemporyData.BattleType.BossBattle) {
                    item._atkPart.SetData (atksidelist [i], side);
				} else if(temp.currentBattleType == TemporyData.BattleType.FinalTrialBuou) {
					item._atkPart.SetData (atksidelist [i], false);
				} else if(temp.currentBattleType == TemporyData.BattleType.PVPVideo) {
					bool att = temp.PvpVideo_AttackOrDefense == 1;
					item._atkPart.SetData (atksidelist [i], att);
				} else {
					item._atkPart.SetData (atksidelist [i], true);
				} 
			} else {
                item._atkPart.gameObject.SetActive (false);
            }

            if (defsidelist.Count > i) {
				if(temp.currentBattleType == TemporyData.BattleType.BossBattle) {
                    item._defPart.SetData (defsidelist [i], !side);
				} else if(temp.currentBattleType == TemporyData.BattleType.FinalTrialBuou) {
					item._defPart.SetData (defsidelist [i], true);
				} else if(temp.currentBattleType == TemporyData.BattleType.PVPVideo) {
					bool def = temp.PvpVideo_AttackOrDefense == 1;
					item._defPart.SetData (defsidelist [i], !def);
				} else {
					item._defPart.SetData (defsidelist [i], false);
				} 
            } else {
				item._defPart.gameObject.SetActive(false); 
			}
                
      
        }
        _grid.Reposition ();
	}

}
