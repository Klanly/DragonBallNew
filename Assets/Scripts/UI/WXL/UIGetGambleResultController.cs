using UnityEngine;
using System.Collections;

public class UIGetGambleResultController : RUIMonoBehaviour {
	private static UIGetGambleResultController instance;
	public static UIGetGambleResultController Instance
	{
		get
		{
			return instance;
		}
	}

	public enum resultWinType{
		isComboType,
		isSkillType,
	}


	public resultWinType curResultType;
    public UILabel btnLabel;

	public UILabel lblTitle;
	public UISprite bg_Title;
	public UISprite[] spStar;
	public UIPanel lightObj;
	//展示panel
   
	public UIPanel showCellPanel;
	public UISpriteAnimation roleAni;
    public UISpriteAnimation loseLine;
    public UISprite loseLineL;
    public UISprite loseLineR;
//win 
    public UILabel botTitle;
    Vector3 vComboPos = new Vector3(-230,-100,0);
    Vector3 vSkillPos = new Vector3(-230,-20,0);
    Vector3 vCellPos = new Vector3(-150,0,0);
	public GameObject winObj;
	public UILabel getRewardMoney;
	public GambleItemCell mainHeadCell;
	public GameObject girdObj;
//lose 
	public GameObject loseObj;
	public UILabel yourChoiceDesp;
	public UILabel yourNum;
	public UILabel resultDesp;
	public UILabel resultNum;

	private GambleItem gambleItemData;
    private int thisNum;
    public UILabel[] unit;
    string loseColor = "[FF8F00]";
    string winMoneyColor = "[FFD700]";

//    int[] skAry =null;
//    int[] monsterArry = null;
    MiniMonsterData MiniRole;

    //  public  delegate void CloseGambleResultDelegate();
    public System.Action onCloseResult = null;

    public int resultNumValue {
        set{ 
            thisNum = value;
            if (resultNum != null) {
                resultNum.text = thisNum.ToString ();
            }
        }
        get{ 
            return thisNum;
        }
    }


    public static UIGetGambleResultController CreateGamblePanel (GameObject tparent,System.Action onClose)
	{
		UnityEngine.Object obj = WXLLoadPrefab.GetPrefab (WXLPrefabsName.UIGambleResultPanel);
		if (obj != null) {
			GameObject go = Instantiate (obj) as GameObject;
			UIGetGambleResultController fc = go.GetComponent<UIGetGambleResultController> ();
			Transform goTrans = go.transform;
			go.transform.parent = tparent.transform;
			go.transform.localPosition = Vector3.zero;
			goTrans.localScale = Vector3.one;
            fc.onCloseResult = onClose;
			RED.TweenShowDialog (go);
			return fc;
		}
		return null;        
	}
        

	void Awake(){
		instance = this;
        //假数据
//		skAry =  new int[5]{21009,21033,21036,21014,21001};
//        monsterArry = new int[3]{ 10131, 10132, 10133 };
        MiniRole = new MiniMonsterData();
        MiniRole.attri = 3;
        MiniRole.lv = 30;
        MiniRole.num = 10100;

	}

	void Start(){
		this.InitData();
	}
	

	public void InitData(){
        BattleSequence thisBattle = Core.Data.temper.warBattle;

        if(thisBattle != null){
            if(thisBattle.gambleResult != null){
                gambleItemData = thisBattle.gambleResult;
				if(gambleItemData.winFlag == true){
					this.ShowWin();

				}else{
					this.ShowLose();
				}

				if(gambleItemData.type == 1)
					curResultType = resultWinType.isComboType;
				else
					curResultType = resultWinType.isSkillType;


                this.TypeShowLabel (gambleItemData.winFlag);

			}
		}
	}

	//展示成功
	void ShowWin(){
		winObj.SetActive(true);
		loseObj.SetActive(false);
		lightObj.gameObject.SetActive(true);
        roleAni.namePrefix = "Waiting_";
        roleAni.AnimationEndDelegate = ShowAni;
        loseLine.gameObject.SetActive(false);
		lblTitle.text = Core.Data.stringManager.getString(7339);
        btnLabel.text = Core.Data.stringManager.getString (7347);
		bg_Title.spriteName = "common-0038";
		for(int i=0;i<spStar.Length;i++){
			spStar[i].spriteName = "common-0043";
			spStar[i].MakePixelPerfect();
		}
	}
	//展示lose
	void ShowLose(){
		winObj.SetActive(false);
		loseObj.SetActive(true);
		lightObj.gameObject.SetActive(false);
        loseLineL.gameObject.SetActive (true);
        loseLineR.gameObject.SetActive (true);

        roleAni.namePrefix = "Waiting_";
        roleAni.enabled = false;
        loseLine.gameObject.SetActive(true);
		lblTitle.text = Core.Data.stringManager.getString(7340);
        btnLabel.text = Core.Data.stringManager.getString (7351);

		bg_Title.spriteName = "common-0040";
		for(int i=0;i<spStar.Length;i++){
			spStar[i].spriteName = "common-0031";
			spStar[i].MakePixelPerfect();
		}
	}

	void TypeShowLabel(bool isWin){
        if (curResultType == resultWinType.isComboType) {
            if (isWin == true) {
                mainHeadCell.gameObject.SetActive (true);
                //mainHeadCell.Init(gambleItemData.show.role.num) ;

                botTitle.text = Core.Data.stringManager.getString (7346);
                botTitle.gameObject.transform.localPosition =vComboPos;
                getRewardMoney.text = winMoneyColor+ gambleItemData.win.ToString();
            } else {
                unit[0].text = Core.Data.stringManager.getString (7349);
                unit[1].text = Core.Data.stringManager.getString (7349);
                thisNum = gambleItemData.show.killarry.Length;
                yourChoiceDesp.text = Core.Data.stringManager.getString (7343);
                yourNum.text = gambleItemData.condition.ToString();
                resultDesp.text = Core.Data.stringManager.getString (7344);
                resultNum.text = thisNum.ToString ();
                getRewardMoney.text = loseColor+ Core.Data.stringManager.getString (7348);

            }
        } else if(curResultType == resultWinType.isSkillType){
            mainHeadCell.gameObject.SetActive (false);
            if (isWin == false) {
                thisNum = gambleItemData.show.skillarr.Length;
                yourChoiceDesp.text = Core.Data.stringManager.getString (7341);

                float tnum = (float)(gambleItemData.condition) * (float)(Core.Data.playerManager.RTData.curTeam.validateMember)/100.0f;
                yourNum.text = MathHelper.MidpointRounding(tnum+0.5f).ToString();
               
//                int tnum = MathHelper.MidpointRounding( (gambleItemData.condition * Core.Data.playerManager.RTData.curTeam.validateMember/100.0f) );
//                yourNum.text =tnum.ToString();

                resultDesp.text = Core.Data.stringManager.getString (7342);
                resultNum.text = thisNum.ToString ();
                getRewardMoney.text =loseColor+ Core.Data.stringManager.getString (7348);

            } else {
                botTitle.text = Core.Data.stringManager.getString (7345);
                botTitle.gameObject.transform.localPosition = vSkillPos;      
                getRewardMoney.text =winMoneyColor+ gambleItemData.win.ToString();
                unit[0].text = Core.Data.stringManager.getString (7350);
                unit[1].text = Core.Data.stringManager.getString (7350);
            }
        }

        this. ShowList ();
           
    }

    void ShowAni(UISpriteAnimation ani){
        roleAni.enabled = false;
        roleAni.namePrefix = "Win_";
        roleAni.loop = true;
        roleAni.enabled = true;
        //  roleAni.AnimationEndDelegate = null;
    }


	void OnBack(){
        if (onCloseResult != null) {
            onCloseResult ();
        }
        Core.Data.temper.gambleTypeId = -1;
        Core.Data.playerManager.ReduceCoin (Core.Data.temper.warBattle.gambleResult.win);
		Destroy(gameObject);
	}

	void ShowList(){
		if(gambleItemData.show != null){
			if(gambleItemData.winFlag == true){
				if(curResultType == resultWinType.isComboType){     
                    mainHeadCell.att = gambleItemData.show.role.attri;
                    mainHeadCell.lv = gambleItemData.show.role.lv;
                    mainHeadCell.Init(gambleItemData.show.role.num);

					for(int i=0;i<gambleItemData.show.killarry.Length;i++){
                        GambleItemCell.CreatGambleItemCell(girdObj,gambleItemData.show.killarry[i],vCellPos+ Vector3.right*i* 140);
                    }   
				}else if(curResultType == resultWinType.isSkillType){
					for(int i=0;i<gambleItemData.show.skillarr.Length;i++){
                        GambleItemCell.CreatGambleItemCell(girdObj,gambleItemData.show.skillarr[i],vCellPos+ Vector3.right*i* 140);
					}
				}

				girdObj.GetComponent<UIGrid>().Reposition();

			}
		}
	}





//    void OnGUI(){
//        if (GUI.Button (new Rect (100, 100, 100, 60),"skill type")) {
//		    
//            curResultType = resultWinType.isSkillType;
//            this.ShowWin ();
//			for(int i=0;i<skAry.Length;i++){
//                GambleItemCell.CreatGambleItemCell(girdObj,skAry[i],vCellPos+ Vector3.right*i* 140);
//			}
//            girdObj.GetComponent<UIGrid>().Reposition();
//        }
//
//        if(GUI.Button(new Rect(50,300,100,50),"combo type")){
//            curResultType = resultWinType.isComboType;
//            this.ShowWin ();
//            mainHeadCell.att = MiniRole.attri;
//            mainHeadCell.lv = MiniRole.lv;
//            mainHeadCell.Init(MiniRole.num);
//            for(int i=0;i<monsterArry.Length;i++){
//                GambleItemCell.CreatGambleItemCell(girdObj,monsterArry[i],vCellPos+ Vector3.right*i* 140);
//            }
//            girdObj.GetComponent<UIGrid>().Reposition();
//        }
//
//    }
        
}
