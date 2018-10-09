using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
public class YcgEquipInfo : MonoBehaviour
{

    public List<UISprite> ListStart = new List<UISprite>() ;
    public UILabel EquipName ; 
    public UISprite EquipIcon ; 
    //public UISprite EquipIconBack; 
    public UILabel attack ; 
    public UILabel attackMax ; 
    public UILabel defense ; 
    public UILabel defenseMax ; 
    public UILabel EquipInfoText ; 
   // static UnityEngine.Object obj;
    static EquipData _equip ; 
    public static GameObject _self ; 
	// Use this for initialization
	void Start ()
	{
        Init() ; 
	}
    public void Init() 
    {
        // 如果数据为空就直接return；
        if (_equip == null ){

            return  ;
        }

        //装备名字
        EquipName.text  = _equip.name ; 
        //装备ID
        EquipIcon.spriteName = _equip.ID.ToString() ; 
        EquipIcon.MakePixelPerfect();
        //装备低框
//        EquipIconBack.spriteName = _equip.ID.ToString() ;
//        EquipIconBack.MakePixelPerfect();
        // 装备星级
        for ( int  i = 0 ; i < _equip.star ; i++ )
        {
            ListStart[i].gameObject.SetActive(true);
        }
        //装备普通攻击力
       
        attack.text = _equip.atk.ToString()  ; 
        //装备最大攻击力
        string attackMaxStr = Core.Data.stringManager.getString(7703) ; 
        attackMaxStr = string.Format(attackMaxStr,  getEquipMaxValue (_equip.atk , _equip.atkGrowth) ); 
        attackMax.text = attackMaxStr  ; 
        //装备普通防御力

        defense.text = _equip.def.ToString() ; 
        //装备最大防御力
        string defenseMaxStr = Core.Data.stringManager.getString(7704); 
        defenseMaxStr = string .Format(defenseMaxStr ,getEquipMaxValue (_equip.def , _equip.defGrowth)  ) ; 
        defenseMax.text = defenseMaxStr  ; 
        //装备说明
        EquipInfoText.text = _equip.description;
    }
    //获取装备最大值
    private string  getEquipMaxValue(float atk , float atkGrowth)
    {
        int maxAtk = (int)(atk + 59 * atkGrowth);

        return maxAtk.ToString();
    }

    public void Close()
    {
        Destroy(this.gameObject);
    }
    public static YcgEquipInfo  openUI(EquipData eq = null ) 
    {
        if (_self !=  null ) return null;
        _equip = eq;
        UnityEngine.Object  obj  = PrefabLoader.loadFromPack("YCG/EquipInfo");
        if(obj != null &&_self == null )
        {
            _self  = Instantiate(obj) as GameObject;
            YcgEquipInfo cc = _self.GetComponent<YcgEquipInfo>();
            RED.AddChild (_self , DBUIController.mDBUIInstance._TopRoot );
            return cc;
        }
        return null;
    }
    void OnDestroy()
    {
        if (ListStart != null )
        {
            ListStart.Clear()  ; 
            ListStart = null ; 
        }
        if (_self != null )
        {
            _self = null ;
        }
        EquipName  = null ;
        EquipIcon = null  ;
        //EquipIconBack = null ; 
        attack= null ; 
        attackMax = null ; 
        defense = null ; 
        defenseMax = null ; 
        EquipInfoText = null ;
    }
//     void OnGUI ()
//    {
//        if ( GUI.Button ( new Rect (10,10,150,100), "I am a button"))
//        {
//
//            YcgEquipInfo.openUI();
//        }
//    }
}
