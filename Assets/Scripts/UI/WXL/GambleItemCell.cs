using UnityEngine;
using System.Collections;

public class GambleItemCell : MonoBehaviour {

	public UISprite spHead;
	public UISprite spBorder;
	public UILabel lblName;
	public UILabel lblLevel;
    public UISprite spAtt;
    public int lv;
    public int att;
	ConfigDataType curType;
    public UISprite spKill;

	public int id;
	
    public static GambleItemCell CreatGambleItemCell(GameObject objParent,int id,Vector3 pos){
		UnityEngine.Object obj = WXLLoadPrefab.GetPrefab (WXLPrefabsName.UIGambleItem);
		if (obj != null) {
			GameObject go = Instantiate (obj) as GameObject;
			GambleItemCell fc = go.GetComponent<GambleItemCell> ();
			Transform goTrans = go.transform;
			go.transform.parent = objParent.transform;
            go.transform.localPosition = pos;
			goTrans.localScale = Vector3.one;
			fc.Init(id);
			return fc;
		}
		return null; 
	}


	public void Init(int id){
		curType = DataCore.getDataType(id);
		if(curType !=  ConfigDataType.Default_No)
		{
			if(curType == ConfigDataType.Monster){
                spAtt.gameObject.SetActive (true);
				AtlasMgr.mInstance.SetHeadSprite(spHead,id.ToString());
				MonsterData mData = Core.Data.monManager.getMonsterByNum(id);
                if (mData != null) {
                    lblName.text = mData.name;
                }
                spAtt.spriteName = "Attribute_" + att.ToString ();
                if (lv == 0)
                    lblLevel.gameObject.SetActive (false);
                else
                    lblLevel.text = "Lv" + lv; 
                if (att == 0) {
                    spBorder.spriteName = "star1";
                } else {
                    spBorder.spriteName = "star" + att.ToString ();
                }
                if (spKill != null) {
                    spKill.gameObject.SetActive (true);
                }
            }else if(curType == ConfigDataType.Skill){
                spAtt.gameObject.SetActive (false);
                spKill.gameObject.SetActive (false);
				SkillData skData = Core.Data.skillManager.getSkillDataConfig(id);
                if (skData != null) {
                    spHead.atlas = AtlasMgr.mInstance.skillAtlas;
                    spHead.spriteName = id.ToString ();
                    spHead.MakePixelPerfect ();
                    lblName.text = skData.name;
                    lblLevel.text = skData.level;
                }
                spAtt.gameObject.SetActive (false);
                spBorder.spriteName = "star6";

			}
            spHead.MakePixelPerfect ();

		}

	}
}
