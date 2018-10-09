using UnityEngine;
using System.Collections;

public class UIHeadItemN : MonoBehaviour 
{
    public UISprite att;
    public UISprite border;
    public UISprite _head;
    public UILabel _name;
    public UILabel _power;
    public UILabel _level;
    public UISprite _part;


    public void SetData(BanBattleRole roleInfo, bool showAttack)
    {
        AtlasMgr.mInstance.SetHeadSprite(_head, roleInfo.number.ToString());
        _head.MakePixelPerfect();
        _level.text = "Lv" + roleInfo.level;

		att.spriteName = BanTools.GetAttributeName(roleInfo.initAttri);
		border.spriteName = BanTools.GetAttributeBorderName (roleInfo.initAttri);
        _power.text = ""+(roleInfo.group == BanBattleRole.Group.Attack ? roleInfo.attackBP : roleInfo.defenseBP);
        _name.text = Core.Data.monManager.getMonsterByNum (roleInfo.number).name;

        if(showAttack) {
            _part.spriteName = "common-0008";
        } else {
            _part.spriteName = "common-0010";
        }


    }
}
