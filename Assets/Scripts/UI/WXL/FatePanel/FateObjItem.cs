using UnityEngine;
using System.Collections;
//只有两种  宠物 和 装备
public class FateObjItem : MonoBehaviour {

    public UISprite border;
    public UISprite icon;

    public StarsUI Sc_star;

    public UILabel lbl_name;

    public UISprite mainBg;

    public int id;

	//已拥有标志
	public GameObject obj_get;

    public int HeadID
    {
        get{
            return int.Parse(icon.spriteName);
        }
        set{
            AtlasMgr.mInstance.SetHeadSprite(icon, value.ToString());
			RED.SetActive (Core.Data.monManager.GetAllMonsterByNum (value).Count > 0, obj_get);
        }
    }

    public int EquipID
    {
        get{
            return int.Parse(icon.spriteName);
        }
        set{
            icon.atlas = AtlasMgr.mInstance.equipAtlas;
            icon.spriteName=value.ToString();
			RED.SetActive (Core.Data.EquipManager.GetAllEquipByNum(value).Count > 0, obj_get);
        }
    }

    public string myName{
        get{ 
            if (DataCore.getDataType (HeadID) == ConfigDataType.Monster) {
                MonsterData mon = Core.Data.monManager.getMonsterByNum (HeadID);
                return  mon.name;
            } else {
                EquipData equip = Core.Data.EquipManager.getEquipConfig(EquipID);
                return  equip.name;
            }
        }
        set{ 
            lbl_name.text = value;
        }

    }
        
    public int Border
    {
        set {border.spriteName = "star" + value;}
    }

    public int Star {
        set { Sc_star.SetStar(value); }
    }

    void Start(){
        if (HeadID != 0) {
            if (DataCore.getDataType (HeadID) == ConfigDataType.Monster) {
                MonsterData mon = Core.Data.monManager.getMonsterByNum (HeadID);
                myName = mon.name;
                Border = 4;
                Star = mon.star;
                AtlasMgr.mInstance.SetHeadSprite (icon,mon.ID.ToString());
            } else {
                EquipData equip = Core.Data.EquipManager.getEquipConfig(EquipID);
                myName = equip.name;
                Border = 4;
                Star = equip.star;
                icon.atlas =  AtlasMgr.mInstance.equipAtlas;
                icon.spriteName = equip.ID.ToString();
            }
        }

        icon.MakePixelPerfect ();
    }
    /// <summary>
    /// Raises the click event.
    /// </summary>
    public void OnClickToTarget(){

        ShowFatePanelController.Instance.OnGoFateItem (id);
    }

}
