using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShowFatePanelController : RUIMonoBehaviour {
    private static ShowFatePanelController instance;
    public static ShowFatePanelController Instance
    {
        get
        {
            return instance;
        }
    }

	//缘分 界面 
	public static ShowFatePanelController CreatShowFatePanel(int tId,FateInPanelType type,CombinationSkillPanelScript cSkillPanel = null){
        UnityEngine.Object obj = WXLLoadPrefab.GetPrefab (WXLPrefabsName.UIShowFatePanel);
        if (obj != null) {
            GameObject go = Instantiate (obj) as GameObject;
            go.layer =LayerMask.NameToLayer ("UITop");
            ShowFatePanelController fc = go.GetComponent<ShowFatePanelController> ();
			go.transform.parent = DBUIController.mDBUIInstance._TopRoot.transform;
			go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.back*11;
            RED.TweenShowDialog(go);
			fc.myId = tId;
			//fc.fateData = fateD;
            fc.cSkillPanel = cSkillPanel;
            fc.curInPanelType  = type;
            return fc;
        }
        return null;
    }

	[HideInInspector]
	private FateData fateD = null;
	private MonsterData monsterD = null;
	private EquipData equipD = null;
	public int myId;

    public List<FateObjItem> headItem;
    private List<MonsterData> monData;
    private List<EquipData> equipData;
    public UISprite spArrow;
    public GameObject headItemGrid;
	public GameObject btnItemGrid;
    Vector3 startPos = new Vector3(-124,140,0);
	FateObjItem defaultItem;


    string normalName = "common-0028";
    string slectName = "common-0029";
    public List<GameObject> btnItemList;
    [HideInInspector]
    public CombinationSkillPanelScript cSkillPanel = null;
    public UILabel lblTitle;

//    public UISprite topWhiteBg;
//    public UISprite midBlackBg;
//    public UISprite bottomBg;
//    int threeMoodTopWhiteBg = 170;
//    int twoMoodTopWhiteBg = 90;
//    int threeMoodMidBlackBg = 240;
//    int twoMoodMidBlackBg = 155;
//    int threeMoodBBg = 550;
//    int twoMoodBBg = 460;

    public enum FateInPanelType
    {
        isInMonsterInfoPanel,
        isInSkillInfoPanel,
        isInRecruitPanel,
        isInTeamModifyPanel,
		isInBagPanel,
		isInInfoPanel,
    }

    public FateInPanelType curInPanelType;

    void Awake(){
        instance = this;
        headItem = new List<FateObjItem> ();
        monData = new List<MonsterData> ();
        equipData = new List<EquipData> ();
    }

    void Start(){
		//		Debug.Log (" id = " + myId);



		if (myId != 0) {
			if ((int)(myId / 1000) == 9) {
				fateD = Core.Data.fateManager.getFateDataFromID (myId);
				equipD = null;
				monsterD = null;
			} else {
				switch (DataCore.getDataType (myId)) {
				case ConfigDataType.Fate:
					fateD = Core.Data.fateManager.getFateDataFromID (myId);
					equipD = null;
					monsterD = null;
					Debug.Log (" fate   case    = " + fateD);
					break;
				case ConfigDataType.Equip:
					equipD = Core.Data.EquipManager.getEquipConfig (myId);
					monsterD = null;
					fateD = null;
					break;
				case ConfigDataType.Monster:
					monsterD = Core.Data.monManager.getMonsterByNum (myId);
					equipD = null;
					fateD = null;
					break;
				default:
					fateD = null;
					monsterD = null;
					equipD = null;
					break;
				}
			}
		}

		if (fateD != null) {
			lblTitle.text = fateD.name;
			InitHeadItem ();
			defaultItem = headItem [0]; 
			defaultItem.mainBg.spriteName = slectName;
			InitFateItem (defaultItem);
			spArrow.transform.localPosition = new Vector3 (-125, spArrow.transform.localPosition.y, 0);
		} else if (equipD != null) {
			lblTitle.text = equipD.name;
			InitHeadItem ();
			defaultItem = headItem [0]; 
			defaultItem.mainBg.spriteName = slectName;
			InitFateItem (defaultItem);
			spArrow.transform.localPosition = new Vector3 (-125, spArrow.transform.localPosition.y, 0);
		} else if (monsterD != null) {
			lblTitle.text = monsterD.name;
			InitHeadItem ();
			defaultItem = headItem [0]; 
			defaultItem.mainBg.spriteName = slectName;
			InitFateItem (defaultItem);
			spArrow.transform.localPosition = new Vector3 (-125, spArrow.transform.localPosition.y, 0);
		}
    }

    void InitHeadItem(){
		if (fateD != null) {
			monData = Core.Data.fateManager.GetMonsterByFateNum (fateD.ID);
			equipData = Core.Data.fateManager.GetEquipByFateNum (fateD.ID);
		} else if (equipD != null) {
			equipData.Add (equipD);
		} else if (monsterD != null) {
			monData.Add (monsterD);
		}

		for (int i = 0; i < monData.Count + equipData.Count; i++) {
           
			UnityEngine.Object obj = WXLLoadPrefab.GetPrefab (WXLPrefabsName.UIFateHeadItem);
			if (obj != null) {
				GameObject go = Instantiate (obj)as GameObject;
				FateObjItem hi = go.GetComponent<FateObjItem> ();
                  
				if (monData.Count != 0 && equipData.Count == 0) {
					hi.HeadID = monData [i].ID;
					go.transform.parent = headItemGrid.transform;
					go.transform.localScale = Vector3.one;
					go.transform.localPosition = startPos + i * Vector3.right * 160;
					hi.id = i;
              
				} else if (equipData.Count != 0 && monData.Count == 0) {
					hi.EquipID = equipData [i].ID;
					go.gameObject.transform.parent = headItemGrid.transform;
					go.transform.localScale = Vector3.one;
					go.transform.localPosition = startPos + i * Vector3.right * 160;
					hi.id = i;
				}
				headItem.Add (hi);
			}
		}
		
    }

	//初始化底部入口item
    void InitFateItem(FateObjItem item){
		
        List<int> idList = null;
		if(item != null){
			if(item.HeadID != 0 || item.EquipID != 0){
                idList = Core.Data.newDungeonsManager.GetFloorIdByGiftId(item.HeadID);
			}
		}
        //可能没有配缘的  也可能 有 很多缘分
        if (idList != null) {
            for (int i = 0; i <= idList.Count; i++) {
                GameObject obj = null;
  
                if (i == idList.Count) {
                    obj = this.CreatFateBtnObj (btnItemGrid.transform, null, Vector3.down * i * 80);
                    btnItemList.Add (obj);
                    break;
                }

                obj = this.CreatFateBtnObj (btnItemGrid.transform, Core.Data.newDungeonsManager.FloorList[ idList [i] ], Vector3.down * i * 80);
                btnItemList.Add (obj);
            }

        } else {
            GameObject tobj = this.CreatFateBtnObj (btnItemGrid.transform, null, Vector3.zero);
            btnItemList.Add (tobj);
        }
            
	}

    public GameObject CreatFateBtnObj(Transform root,NewFloor data,Vector3 pos){
		UnityEngine.Object obj =WXLLoadPrefab.GetPrefab(WXLPrefabsName.UIFateBtnItem);
		if (obj != null) {
			GameObject go = Instantiate(obj) as GameObject;
			FateBtnItem uiH = go.GetComponent<FateBtnItem> ();
            uiH.fData = data;
			go.transform.parent = root;
			go.transform.localScale = Vector3.one;
			go.transform.localPosition = pos;
			return go; 
		}
		return null;
	}


    public void OnGoFateItem(int index){
        if (index == defaultItem.id)
            return;
        FateObjItem item = headItem [index];
        if (item != null) {
            for (int i = 0; i < btnItemList.Count; i++) {
                Destroy (btnItemList [i].gameObject);
              
            }
            btnItemList.Clear ();
            btnItemGrid.GetComponent<UIGrid> ().Reposition ();
           
            this.InitFateItem (item);

            float tempX = item.gameObject.transform.localPosition.x - 128 + headItemGrid.transform.parent.localPosition.x;
            if (tempX < -168f) {
                tempX = -168f;
            }else if(tempX > 178){
                tempX = 178f;
            }

            Vector3 targetPos = new Vector3 (tempX, spArrow.transform.localPosition.y, 0);
            MiniItween.MoveTo (spArrow.gameObject, targetPos , 0.3f, MiniItween.EasingType.Clerp,false);
            item.mainBg.spriteName = slectName;
            defaultItem.mainBg.spriteName = normalName;
            defaultItem = item; 
        }
    }


    public  void OnClose(){
        Destroy (gameObject);
    }

}
