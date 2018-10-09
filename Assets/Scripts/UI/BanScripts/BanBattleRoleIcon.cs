using UnityEngine;
using System.Collections;

public class BanBattleRoleIcon : MonoBehaviour {

	public UISprite sprite_Role;

	public UISprite sprite_Attribute_Border;

	public UILabel text_Level;

	public BanBattleRole banBattleRole;

    public UISprite sprite_Attribute;

    public UILabel RoleName;

    public GameObject go_RoleName;

    private bool isDead = false;
    public bool isDie {
        get {
            return isDead;
        }
    }

    public void ReadData (BanBattleRole aBanBattleRole, bool isCurrent = false) {
        AtlasMgr.mInstance.SetHeadSprite(sprite_Role, aBanBattleRole.number.ToString());
		sprite_Attribute_Border.spriteName = BanTools.GetAttributeBorderName(aBanBattleRole.attribute);
        if(!isDie)
            sprite_Attribute.spriteName = BanTools.GetAttributeName(aBanBattleRole.attribute);
        else 
            sprite_Attribute.spriteName = BanTools.GetAttributeName(BanBattleRole.Attribute.Unknown);
        text_Level.text = "Lv" + aBanBattleRole.level;

        if(Core.Data.monManager != null) {
            RoleName.text =  Core.Data.monManager.getMonsterByNum(aBanBattleRole.number).name;
        }

        banBattleRole = aBanBattleRole;
	}

    //死亡
    public void toDie() {
        isDead = true;
        sprite_Attribute.spriteName = BanTools.GetAttributeName(BanBattleRole.Attribute.Unknown);
    }

    //复活
    public void toLive() {
        isDead = false;
		banBattleRole.attribute = banBattleRole.initAttri;
		sprite_Attribute.spriteName = BanTools.GetAttributeName(banBattleRole.initAttri);
		sprite_Attribute_Border.spriteName = BanTools.GetAttributeBorderName(banBattleRole.initAttri);
    }


    //改变为全属性
    public void changeToAllAttribute() {
        sprite_Attribute_Border.spriteName = BanTools.GetAttributeBorderName(BanBattleRole.Attribute.Quan);
        sprite_Attribute.spriteName = BanTools.GetAttributeName(BanBattleRole.Attribute.Quan);
    }

}
