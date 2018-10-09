using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIBossReward : MonoBehaviour
{
    //对比属性
    public UILabel myLevel;
    public UILabel myName;
    public UILabel myAttack;
    public UILabel otherLevel;
    public UILabel otherName;
    public UILabel otherAttack;
    public UISprite myAttBack;
    public UISprite otherAttBack;
    //
  
    private string qiangDuoDragonBallInfo;
    public GameObject FinalTrialSucRoot;
    public GameObject FinalTrialFailRoot;
    public UILabel mStarNum;
    public UISprite box1;
    public UISprite box2;
    public UISprite box3;
    public UISprite box4 ;
    public UISprite box5 ;

    public UIHeadItem[] rewardList;
    public UILabel[] rewardName;

  
    public GameObject buttonClose;
    public GameObject buttonReplay;
    public GameObject buttonConflict;
    public GameObject buttonExit;
    public BoxCollider mask;

    //战斗回放
    public delegate void onReplay();
    public onReplay OnReplay = null;

    //
    BattleReward reward;
    public GameObject _fail;
    public GameObject _suc;
  //  public GameObject _plReward;
    //public UISprite[] _titleSprite;
    public GameObject backButton;
    public GameObject _conflict;
    public GameObject _resultObj;
    private bool Status_Win = false;
   // public UISpriteAnimation winAni;
   // public UISprite[] spWinFlower;
    public GameObject[] starAni;
    public UISprite spBird;
    //public UISpriteAnimation mindLine;
    //public UISprite[] spLine;
    //public UISprite[] spTitle;
   // public UISprite[] spSide;
    public UISprite[] rotateLight;


	public UILabel ShaLu_Star;
	public UILabel ShaLu_Round1;
	public UILabel Shalu_Round2;
	//重整旗鼓，再接再厉的文字
	public UILabel FailWord;
	//输了，也可以获得经验的文字
	//public UILabel ExpWhenFail;

	//世界boss赢了，给出的提示
	public GameObject WorldBossWin;
	public UILabel WorldBossWinPoint;

	//当前声效的层级
	private int curSoundLayer = -1;

    //最新结算界面添加的属性 yangchenguang 
    public GameObject CompetesLabel; //竞彩奖励
    public GameObject qiangDuoLabel; //抢夺
    private Vector3 CompetesV3;//竞彩奖励初始化坐标
    private Vector3 qiangDuoV3;//抢夺初始化坐标
  

    //战斗收益对象
    public UILabel coinlabel; //金币
    public UILabel explabel;  //经验
    public UILabel jllabel; // 精力label
    public GameObject jlGame;// 精力对象
    public GameObject coinGame;//金钱对象
    public GameObject expGame;//经验对象
    public GameObject backGroundScroll;//阵容对比背景
    public  GameObject labelFailGame;// 沙鲁，失败以后出现的文字

	//返回主界面
	public GameObject BackToHomeScreen;

    public UIButton shareUIBtn; // 分享按钮
    public UIButton niuDan; //扭蛋
    public UIButton qiangHua;// 强化
    public GameObject vsPro; //对比信息
    //副本星级

    public BossStar bossStar;
    //副本胜利和失败的艺术字的交换
    public UISprite vFontChange;
    //战斗 在来一个
    public UIButton againBat;
    //胜利和失败的红色低
    public UISprite redBackGround;
    //星星背景
    public GameObject starBackGround;
    //PVP 状态有的按钮在PVP才显示 -1是默认 1 代表PVP
    public int pvpStatus = -1; 

    bool isExit  = true ;  //  退出按钮防止多次点击
    bool isAgain = true ;  // 再来一次按钮防止多次点击

    //添加combe
    public List<UISprite> combeList = new List<UISprite>();
    public UILabel combeLabel ; 
    public GameObject combeGame ; 
    public UILabel getRewardLab  ; 

    void Start()
    {

		int lv = Core.Data.playerManager.RTData.curLevel;
		if(lv <= 15) { //15级别不开方这个功能
			if(BackToHomeScreen != null) BackToHomeScreen.SetActive(false);
		}

        backButton.SetActive(false);
        _conflict.SetActive(false);
        _resultObj.SetActive(true);
        hideRewardFirst();
        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);

        box1.transform.localScale = new Vector3(0.01f,0.01f,0.01f) ;
        //box2.transform.localScale = new Vector3(0.01f,0.01f,0.01f) ;
        //box3.transform.localScale = new Vector3(0.01f,0.01f,0.01f) ;
        box1.gameObject.SetActive(false);
        box2.gameObject.SetActive(false);
        box3.gameObject.SetActive(false);



       

        setGrabInfoPosi(Core.Data.temper.currentBattleType);
        this.ShowEffect();
	
        GuideManager mgr = Core.Data.guideManger;
        if(mgr.isGuiding) {
            mgr.listener.battleReceiver.Exit = BackToGame;
        }

		//一段时间后展示出来，任务系统
		AsyncTask.QueueOnMainThread( ()=>{ Core.Data.taskManager.AutoShowPromptWord(BanBattleManager.Instance.go_uiPanel); }, 0.4F);
        Open1();
    }
    private void setGrabInfoPosi(TemporyData.BattleType Battype)
    {
        CompetesLabel.gameObject.SetActive(false ); 
		CompetesLabel.GetComponent<UILabel>().text = Core.Data.stringManager.getString(7501);
        qiangDuoLabel.gameObject.SetActive(false ); 
        if(Battype == TemporyData.BattleType.QiangDuoDragonBallBattle) 
        {

          
            if (Core.Data.temper.warBattle.gambleResult != null )
            {
                qiangDuoLabel.transform.localPosition = new Vector3(-495 , 219 , 0 ) ; 

            }else
            {
                qiangDuoLabel.transform.localPosition = new Vector3(-495 , 194 ,0) ; 
            }
            CompetesLabel.transform.localPosition = new Vector3(-495 , 156 ,0) ; 
         

          
     

        }else if (Battype ==  TemporyData.BattleType.TianXiaDiYiBattle) 
        {



            CompetesLabel.transform.localPosition =  new Vector3(-495 , 210 , 0 ) ; 
         
        }

        CompetesV3 = CompetesLabel.transform.localPosition ; 
        qiangDuoV3 = qiangDuoLabel.transform.localPosition;

        CompetesLabel.transform.localPosition += new Vector3(0,0,-500) ; 
        qiangDuoLabel.transform.localPosition += new Vector3(0,0,-500);
       
    }

    // 竞彩  抢夺 连击次数
    private void JSinfo(TemporyData.BattleType type)
    {

        CompetesLabel.SetActive(false ) ; 
        qiangDuoLabel.SetActive(false ) ; 
      //  AttackNum.SetActive(false ) ; 

        if (Core.Data.temper.warBattle.gambleResult != null )
        {
           
            CompetesLabel.SetActive(true);
            StartCoroutine(JingcaiT());
        }

        switch(type)
        {
        case TemporyData.BattleType.None: //普通战斗无竞彩和抢夺
         
            break ; 
        case TemporyData.BattleType.QiangDuoDragonBallBattle :// 抢夺龙珠
        case TemporyData.BattleType.QiangDuoGoldBattle :
            qiangDuoLabel.SetActive(true);

            StartCoroutine(qiangDuoT());
            break ; 
        case TemporyData.BattleType.TianXiaDiYiBattle:// 天下第一
          

            break ;
        }
    }
    //
    public void FailRewardPosi()
    {
        jlGame.transform.localPosition  = new Vector3(76,283, 0)  ; // 精力对象
        coinGame.transform.localPosition  = new Vector3(-172,283,0)  ;//金钱对象
        expGame.transform.localPosition  = new Vector3(-424,283,0)  ;//经验对象
    }
    //奖励是否显示
    public void RewardVisible(bool isValue ,TemporyData.BattleType Battype)
    {
       // TemporyData tempData = Core.Data.temper;
        if(Battype == TemporyData.BattleType.QiangDuoGoldBattle || Battype  == TemporyData.BattleType.BossBattle) 
        {
            jlGame.SetActive(isValue);
            coinGame.SetActive(isValue);
            expGame.SetActive(isValue);
        }
    }
    //竞彩字样动画
    IEnumerator JingcaiT() 
    {

        //string strText = Core.Data.stringManager.getString(10018);
       // strText = string.Format(strText,Core.Data.temper.warBattle.gambleResult.win);
        CompetesLabel.GetComponent<GrabDragonballAndCoin>()._labCoin.text = "[ffff00]" + Core.Data.temper.warBattle.gambleResult.win.ToString()+"[-]"; 
        CompetesLabel.transform.localScale += new Vector3(3,3,3) ;

        yield return new WaitForSeconds(0.2f);
        MiniItween.MoveTo(CompetesLabel , CompetesV3 ,0.2f);
        //MiniItween.ScaleTo(CompetesLabel, new Vector3(0.95f, 0.95f, 0.95f), 0.2f, MiniItween.EasingType.EaseOutCubic);
        yield return new WaitForSeconds(0.2f);
        MiniItween.ScaleTo(CompetesLabel, new Vector3(1f, 1f, 1f), 0.2f, MiniItween.EasingType.EaseOutCubic);

      
    }
    //抢夺字样动画
    IEnumerator qiangDuoT() 
    {
      
        qiangDuoLabel.transform.localScale += new Vector3(3,3,3);
  
        yield return new WaitForSeconds(0.05f);

        MiniItween.MoveTo(qiangDuoLabel , qiangDuoV3 ,0.2f);
        yield return new WaitForSeconds(0.2f);
        MiniItween.ScaleTo(qiangDuoLabel, new Vector3(1f, 1f, 1f), 0.2f, MiniItween.EasingType.EaseOutCubic);

    }


    // 结算界面等待的循序 yangchenguang
    IEnumerator orderpPlay(TemporyData.BattleType type) 
    {
        //金币
        yield return new WaitForSeconds(0.3f);

        float times = 0.8f ; 

        if (type == TemporyData.BattleType.TianXiaDiYiBattle ||type ==TemporyData.BattleType.QiangDuoGoldBattle || type ==TemporyData.BattleType.QiangDuoDragonBallBattle)
        {
           
            //奖励显示
            JSinfo(type);  
            yield return new WaitForSeconds(0.5f);
        }else
        {
           
            times= 0.3f ;
        }


        yield return new WaitForSeconds(times);

    }

    //结算奖励界面 yangchenguang  金币 经验 精力
    public void setCoinJLEXPEffect(int coin, int exp ,int jl)
    {

       
        coinlabel.text="0";
        explabel .text ="0";
        jllabel.text ="0";
        //字体滚动
        if (coin != -1 )
        {
            StartCoroutine( ShiftCoinEffect(coinlabel,coin));
        }else
        {
            coinGame.SetActive(false);
        }
        if (exp != -1) 
        {
            StartCoroutine( ShiftCoinEffect(explabel,exp));
        }else
        {
            expGame.SetActive(false);
        }
        if (jl != -1 ) 
        {
            StartCoroutine( ShiftCoinEffect(jllabel,jl));
        }else
        {
            jlGame.SetActive(false) ;
        }


    }

    //通用代码 数字滚动
    public IEnumerator ShiftCoinEffect(UILabel lab ,int endNum){


        yield return  new WaitForSeconds(0.4f);
        int startNum = 0;

       // StartCoroutine( shifCoin(lab,endNum,0));

        int ten =100 ; 
        int aNum =0;
        int tenNum =0 ;

        if (endNum> 0)
        {

            aNum   = endNum % ten ;
            tenNum = endNum/ten ;
            startNum += aNum;
            if (tenNum == 0 ) 
            {
                lab.text ="+"+ startNum.ToString();

            }

            for (int i = 0 ; i < tenNum ; i++)
            {
                startNum+=ten;

                lab.text ="+"+ startNum.ToString() ;
                yield return new WaitForEndOfFrame();
            }


        }else if (endNum == 0 )
        {
            lab.text ="+"+ endNum.ToString();
        }
        else
        {
            int value =Mathf.Abs(endNum);
            aNum   = value % ten ;
            tenNum = value/ten ;
            startNum += aNum;
            if (tenNum == 0 ) 
            {
                lab.text ="-"+ startNum.ToString() ;
            }
            for (int i = 0 ; i < tenNum ; i++)
            {
                startNum+=ten ;
                lab.text ="-"+ startNum.ToString() ;
                yield return  new WaitForEndOfFrame();
            }
        }
     
    }
  
    /// <summary>
    /// 0: 输   1： 赢
    /// </summary>
    /// <param name="type">Type.</param>
    void ShowTitle(int type)
    {

        if (type == 0 )
        {

            vFontChange.spriteName ="zhandoushibai";
            redBackGround.color= Color.white;
            starBackGround.SetActive(false);
            bossStar.gameObject.SetActive(false);
          
        }else 
        {
            vFontChange.spriteName ="zhandoushengli";
            starBackGround.SetActive(true);
            starBackGround.SetActive(true);

            bossStar.initStar(Core.Data.temper.warBattle.battleData.stars);
            bossStar.gameObject.SetActive(true);
      
        }


  
    }

    #region  动画

    //弹窗
    void ShowEffect()
    {
        gameObject.transform.localScale = Vector3.zero;
        AnimationCurve anim = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(0.4f, 1.2f, 0.5f, 0.5f), new Keyframe(1f, 1f, 1f, 0f));
        if (gameObject.GetComponent<TweenScale>() != null)
            gameObject.GetComponent<TweenScale>().animationCurve = anim;
        else
        {
            gameObject.AddComponent<TweenScale>().animationCurve = anim;
        }

        TweenScale.Begin(gameObject, 0.4f, Vector3.one);
//        this.PlayLoseAni();
    }

 

    void PlayStarAni(UISpriteAnimation spriteAnimation)
    {

        for (int i = 0; i < starAni.Length; i++)
        {
            starAni[i].SetActive(true);
            starAni[i].GetComponent<UISpriteAnimation>().namePrefix = "Star-";
            starAni[i].GetComponent<UISpriteAnimation>().framesPerSecond = 10;
            starAni[i].GetComponent<UISpriteAnimation>().enabled = true;
        }
        // 边上的星星
        starAni[1].GetComponent<UISpriteAnimation>().AnimationEndDelegate = ShowWinStar;
    }

    public void ShowWinStar(UISpriteAnimation spriteAnimation)
    {
        for (int i = 0; i < starAni.Length; i++)
        {
            starAni[i].SetActive(false);
            starAni[i].GetComponent<UISpriteAnimation>().enabled = false;
        }
//        winAni.AnimationEndDelegate = null;
//        winAni.loop = true;
//        winAni.namePrefix = "Win_";
//        winAni.Reset();
    }

    public void PlayLoseAni()
    {
       
        StartCoroutine("LoseBirdMove");
        //mindLine.gameObject.SetActive(false);
    }

    IEnumerator LoseBirdMove()
    {

        spBird.transform.localPosition = new Vector3(-608, 80, 0);
        spBird.gameObject.SetActive(true);
        spBird.gameObject.transform.localScale = Vector3.one;
        MiniItween.MoveTo(spBird.gameObject, spBird.gameObject.transform.localPosition + new Vector3(1400, 0, 0), 5f);
        yield return new WaitForSeconds(13f);
        spBird.gameObject.transform.localScale = new Vector3(-1, 0, 0);
        MiniItween.MoveTo(spBird.gameObject, spBird.gameObject.transform.localPosition + new Vector3(-1400, 0, 0), 5f);
    }

    #endregion

    void hideRewardFirst()
    {
      //  NGUITools.SetActive(coin1.gameObject, false);
       // NGUITools.SetActive(coin2.gameObject, false);
       // NGUITools.SetActive(coin3.gameObject, false);
    }

    void hideRewardNameFirst()
    {
        for (int i = 0; i < 5; ++i)
        {
            NGUITools.SetActive(rewardName[i].gameObject, false);
        }
    }

	//展示录像-和自己有关系的录像， 也可能没关系
	public void ShowVideo(bool win, int attack, int def, int attLv, int defLv, int whichside, bool myBussiness) {





		if(myBussiness) {
			//title.text = Core.Data.stringManager.getString(win ? 10015 : 10016);
            if (win)
            {
                ShowTitle(1);
            }else
            {
                ShowTitle(0);
            }
		} else {

            ShowTitle(1);
			//title.text = Core.Data.stringManager.getString(14);
		}
        //录像不显示星星
        bossStar.gameObject.SetActive(false);
        starBackGround.SetActive(false);
		TemporyData temp = Core.Data.temper;

		myLevel.text = "Lv" + attLv.ToString();
		myName.text = temp.self_name;
		otherName.text = temp.enemy_name;

        VsUI(whichside);

        //{"ID":10012,"txt":"[ff0000]总攻击力: [-]"}
		if(whichside == 1)
			myAttack.text =  attack.ToString();
		else 
			myAttack.text =  attack.ToString();

		otherLevel.text = "Lv" + defLv.ToString();

		if(whichside == 1)
			otherAttack.text =  def.ToString ();
		else 
			otherAttack.text =  def.ToString ();

       
        setCoinJLEXPEffect(-1 , -1 , -1);
		if(!myBussiness) 
        {

            musicPlay(SoundFx.FX_Win);
		} 
        else
        {
			if(win) 
            {
				
                musicPlay(SoundFx.FX_Win);
			} 
            else
            {
				PlayLoseAni(); //失败以后飞翔的鸟
				//播放失败的声音
                musicPlay(SoundFx.FX_Fail);
			}
		}

		//关闭背景的声音
		Core.Data.soundManager.BGMStop();

		//隐藏所有的输赢的奖励和扭蛋的提示信息
		FinalTrialSucRoot.gameObject.SetActive(false);
		FinalTrialFailRoot.gameObject.SetActive(false);
	}
    // 对比界面攻防图标更换
    private void VsUI(int whichside)
    {
        if(whichside == 1)
        {
            myAttBack.spriteName = "common-0008";
            otherAttBack.spriteName ="common-0010";
        }else
        {
            myAttBack.spriteName = "common-0010";
            otherAttBack.spriteName ="common-0008";
        }
    }
	// whichside 己方是哪一方（攻击还是防守）1攻击  2防御
	// 沙鲁布欧
	public void Show(bool win,int whichside)
    {
       // title.text = Core.Data.stringManager.getString(win ? 10015 : 10016);
        PlayerManager pm = Core.Data.playerManager;
        myLevel.text = "Lv" + pm.Lv;
        myName.text = pm.NickName;
	//	TemporyData temp = Core.Data.temper;
       // int attshalu ;

        int attOrdef = (whichside == 1)  ? pm.RTData.curTeam.teamAttack : pm.RTData.curTeam.teamDefend;

        VsUI(whichside);


        BattleReward reward = Core.Data.temper.warBattle.reward; // 战斗奖励 yangchenguang
		if(whichside == 1) {//己方是攻击
            myAttack.text =  attOrdef.ToString();
            otherAttack.text =   getBattleRightTeamDef().ToString() ;
		} else {
            myAttack.text =  attOrdef.ToString() ;
            otherAttack.text =  getBattleRightTeamDef().ToString() ;
		}

		otherLevel.text = "Lv ???";
        if (FinalTrialMgr.GetInstance().NowEnum == TrialEnum.TrialType_ShaLuChoose)
            otherName.text = Core.Data.stringManager.getString(25003);
        else if (FinalTrialMgr.GetInstance().NowEnum == TrialEnum.TrialType_PuWuChoose)
            otherName.text = Core.Data.stringManager.getString(25002);


        //int count = FinalTrialMgr.GetInstance().EnemyIndex;
//        if(Core.Data.temper.currentBattleType != TemporyData.BattleType.WorldBossWar)
//            mStarNum.text = FinalTrialMgr.GetInstance()._FinalTrialData.PetListCount[count].ToString();

		if (win) {
			
            combeGame.SetActive(true); // yangcg combe数开启

            ShaLu_Star.gameObject.SetActive(false);
            ShaLu_Round1.gameObject.SetActive(false);
            Shalu_Round2.gameObject.SetActive(false);
            FailWord.gameObject.SetActive(false);

			ShowSuccessTrial(FinalTrialSucRoot);
		}
        else
        {
            labelFailGame.SetActive(true);
            ShowFailTrial(FinalTrialFailRoot);
        }
            
      
        StartCoroutine(orderpPlay(TemporyData.BattleType.None));
        if (reward != null )
        {
            setCoinJLEXPEffect((reward.bco + reward.eco),(reward.bep + reward.eep),-1);
        }else
        {
            setCoinJLEXPEffect(-1,-1,-1);
        }
        starBackGround.SetActive(false);
        bossStar.gameObject.SetActive(false);
    }

	//天下第一，抢夺金币，抢夺龙珠，宿敌
    public void Show(bool win, FightBattleReward ext, BattleReward br, int def)
    {
        pvpStatus = 1;
        shareUIBtn.gameObject.SetActive(true);
        this.reward = br;
       // title.text = Core.Data.stringManager.getString(win ? 10015 : 10016);
        int zhang =-1 ; 
		if (ext != null)
		{
			//tianXiaDiYiExp.text = Core.Data.playerManager.RTData.curExp.ToString ();
			//tianXiaDiYiCoin.text = ext.coin.ToString ();
			//tianXiaDiYizhanGong.text = ext.zg.ToString ();
            zhang =ext.zg;
		}

		//临时数据存放地
		TemporyData temp = Core.Data.temper;

		PlayerManager pm = Core.Data.playerManager;

		myLevel.text = "Lv" + pm.Lv;
		myName.text = pm.NickName;

        myAttack.text =  pm.RTData.curTeam.teamAttack.ToString();
        VsUI(1);

		if(string.IsNullOrEmpty(temp.Revenge_Name)) {
			otherLevel.text = "Lv" + FinalTrialMgr.GetInstance ().currentFightOpponentInfo.lv;
			otherName.text = FinalTrialMgr.GetInstance ().currentFightOpponentInfo.n;
            otherAttack.text =    temp.warBattle.battleData.df.ToString();

		} else {
			otherLevel.text = "Lv" + temp.Revenge_Lv;
			otherName.text = temp.Revenge_Name;
			otherAttack.text =   def.ToString ();
			temp.Revenge_Name = null;
		}


		if (temp.currentBattleType == TemporyData.BattleType.QiangDuoGoldBattle ||
			temp.currentBattleType == TemporyData.BattleType.SuDiBattle )
        {
            if (temp.currentBattleType == TemporyData.BattleType.QiangDuoGoldBattle )
            {

                qiangDuoLabel.GetComponent<GrabDragonballAndCoin>().GrodCoin(  true ); 


                 //string strText=  Core.Data.stringManager.getString(6128);



               // strText = string.Format(strText,ext.coin.ToString(),ext.stone.ToString());
                qiangDuoLabel.GetComponent<GrabDragonballAndCoin>()._labCoin.text = "[ffff00]" + ext.coin.ToString() +"[-]";
                qiangDuoLabel.GetComponent<GrabDragonballAndCoin>()._labStone.text = "[ffff00]" + ext.stone.ToString() +"[-]";




            }
			//_plReward.gameObject.SetActive(true);
			//exp.text = (br.bep + br.eep).ToString();
            //coin.text = ext.coin.ToString();
          
		} else if(temp.currentBattleType == TemporyData.BattleType.TianXiaDiYiBattle) {


            //Debug.Log("tianxiadiyi ");
			//_plReward.SetActive(true);
			//exp.text = (br.bep + br.eep).ToString();
			//coin.text = (br.bco + br.eco).ToString();
		}
        else if (Core.Data.temper.currentBattleType == TemporyData.BattleType.QiangDuoDragonBallBattle)
        {
            qiangDuoLabel.GetComponent<GrabDragonballAndCoin>().GrodCoin( false ); 
			//RED.SetActive (true, qiangDuoDragonBallRoot);
			//_plReward.SetActive(true); // 暂时去掉
			//exp.text = (br.bep + br.eep).ToString();
			//coin.text = (br.bco + br.eco).ToString();
			if (ext != null && ext.p != null && ext.p.Length != 0)
            {
                this.qiangDuoDragonBallInfo ="";
                string strText=  Core.Data.stringManager.getString(6116);
              
                for (int i = 0; i < ext.p.Length; ++i)
                {
                    ItemOfReward ior = ext.p[i];

                    if(ior.getCurType() == ConfigDataType.Frag ) {
                        Soul soul = ior.toSoul(Core.Data.soulManager);
                        this.qiangDuoDragonBallInfo += soul.m_config.name;
                    } else {
                        Item item = ior.toItem(Core.Data.itemManager);
                        this.qiangDuoDragonBallInfo += item.configData.name;
                    }

                    if (i != ext.p.Length - 1)
                    {
                        this.qiangDuoDragonBallInfo += ",";
                    }
                }
               
                strText = string.Format(strText,this.qiangDuoDragonBallInfo);
                qiangDuoLabel.GetComponent<UILabel>().text =  strText ;
            }
            else
            {
                this.qiangDuoDragonBallInfo = Core.Data.stringManager.getString(6115);

                qiangDuoLabel.GetComponent<UILabel>().text =  Core.Data.stringManager.getString(6115);
            }
        }

       // JSinfo(Core.Data.temper.currentBattleType);    // 竞彩  抢夺 连击次数 yangchenguang 
        //setLianJiShu(); //设置连击数

        shareUIBtn.gameObject.SetActive(true);
       
        StartCoroutine(orderpPlay(Core.Data.temper.currentBattleType));
		if (win)
		{
            if (ext != null)
            {
                //金币 经验 精力 转动效果
                setCoinJLEXPEffect((br.bco + br.eco+ext.coin),(br.bep + br.eep),zhang);
            }else
            {
                //金币 经验 精力 转动效果
                setCoinJLEXPEffect((br.bco + br.eco),(br.bep + br.eep),zhang);
            }
           
           
			ShowSuc();
		}
		else
		{
          
            if (ext != null)
            {
                setCoinJLEXPEffect((br.bco + br.eco+ext.coin),(br.bep + br.eep),zhang);
            }else
            {
                setCoinJLEXPEffect((br.bco + br.eco),(br.bep + br.eep),zhang);
            }
          
			ShowFail(FinalTrialFailRoot);
            //判断是否显示金币 经验 精力
            RewardVisible(false ,temp.currentBattleType);
		}

        bossStar.gameObject.SetActive(false);
        starBackGround.SetActive(false);

    }

	//boss战斗 
    public void Show(bool win, BattleReward br, int def)
    {
       



        shareUIBtn.gameObject.SetActive(false);
        reward = br;
        //City city = CityFloorData.Instance.currCity;



#if NEWPVE
        NewFloor city = Core.Data.newDungeonsManager.curFightingFloor;
        bool att = city.config.FightType == 0;
       
        if (city.config.isBoss==1)
        {


            if(Core.Data.guideManger.SpecialGuideID >0 )
            {
                againBat.gameObject.SetActive(false);

            }else
            {
                againBat.gameObject.SetActive(true);
            }

          

        }
#else
        City city = CityFloorData.Instance.currCity;
		Floor floor = CityFloorData.Instance.currFloor;  
		bool att = floor.config.gf == 1;
#endif
		PlayerManager pm = Core.Data.playerManager;
       // title.text = Core.Data.stringManager.getString(win ? 10015 : 10016); // 10015 胜利
    
        myLevel.text = "Lv" + pm.Lv;
        myName.text = pm.NickName;
        int attOrdef = att ? pm.RTData.curTeam.teamAttack : pm.RTData.curTeam.teamDefend;
        myAttack.text =  attOrdef.ToString();


        otherLevel.text = "Lv??";
        otherName.text = city.config.name;
		otherAttack.text =  def.ToString();
        if (att)
        {
            VsUI(1);

        }else
        {
            VsUI(0);

        }


        StartCoroutine(orderpPlay(TemporyData.BattleType.BossBattle));
        if (win)
        {

            combeGame.SetActive(true);//yangcg combe数开启

            if (reward != null)
            {
                setCoinJLEXPEffect(reward.bco,reward.bep, -1);//战斗结算收益显示 yangchenguang

            }else
            {
                setCoinJLEXPEffect(0 , 0 , -1); //战斗结算收益显示
            }
           
            ShowSuc();
        }
        else
        {


            if(!TeamUI.secondPosUnLock  &&  city.config.ID == 60104)
            {

                niuDan.gameObject.SetActive(false) ;
                qiangHua.gameObject.SetActive(false);
              
            }



            RewardVisible(false ,TemporyData.BattleType.BossBattle);
            if(reward != null)
                setCoinJLEXPEffect(reward.bco, reward.bep, -1);

            ShowFail();
        }
			
    }

	//世界boss
	public void Show(bool win) {
		//title.text = Core.Data.stringManager.getString(win ? 10015 : 10016);

		TemporyData temp = Core.Data.temper;
		PlayerManager pm = Core.Data.playerManager;
		//己方的信息
		myLevel.text = "Lv" + pm.Lv;
		myName.text = pm.NickName;
        myAttack.text =   pm.RTData.curTeam.teamAttack.ToString();
        VsUI(1);
		//敌人的信息
		otherLevel.text = "Lv" + temp.WorldBoss_lv;
		otherName.text = Core.Data.stringManager.getString(15);
		otherAttack.text =  temp.WorldBoss_Att.ToString();
        setCoinJLEXPEffect(-1,-1,-1);
		if (win) {
			ShowSuc();
		} else {
            labelFailGame.SetActive(true);
			ShowFail();
		}
        bossStar.gameObject.SetActive(false);
        starBackGround.SetActive(false);
	}


	//boss战斗会使用 天下第一，抢夺金币，抢夺龙珠，宿敌
    void ShowFail(GameObject rewardRoot = null)
    { 

        //播放失败的声音
        musicPlay(SoundFx.FX_Fail);
        //关闭背景的声音
        Core.Data.soundManager.BGMStop();
        
        //失败动画
        this.PlayLoseAni();
        this.ShowTitle(0);

        _fail.SetActive(true);
        _suc.SetActive(false);
		FinalTrialFailRoot.SetActive(true);
        FinalTrialSucRoot.SetActive(false);
		//qiangDuoDragonBallRoot.SetActive(false);

		TemporyData temp = Core.Data.temper;

		//如果是抢夺金币，则不应该显示出“重整旗鼓，再接再厉”
		if(temp.currentBattleType == TemporyData.BattleType.QiangDuoGoldBattle || 
			temp.currentBattleType == TemporyData.BattleType.TianXiaDiYiBattle) {
			FailWord.gameObject.SetActive(false);
		} else if(temp.currentBattleType == TemporyData.BattleType.QiangDuoDragonBallBattle) {
			FailWord.gameObject.SetActive(false);
		} else if(temp.currentBattleType == TemporyData.BattleType.WorldBossWar) {
			//ExpWhenFail.gameObject.SetActive(false);
		}


        /*****yangchenguang ******/
        //设定用户的等级
        if (Core.Data.temper.AfterBattleLv > Core.Data.playerManager.RTData.curLevel) {
            Core.Data.playerManager.SetCurUserLevel (Core.Data.temper.AfterBattleLv);
        }

        if(Core.Data.guideManger.isGuiding)
        {
            backButton.SetActive(false);
            shareUIBtn.gameObject.SetActive(false);
            niuDan.gameObject.SetActive(false);
            qiangHua.gameObject.SetActive(false);
        }

    }

    SoundFx musicType;
    //战斗胜利音效间隔3秒播放 yangchenguang
    public void musicPlay(SoundFx Type001 )
    {

        musicType = Type001;
        curSoundLayer = Core.Data.soundManager.SoundFxPlay(musicType, 
            backMusicFun, 
            false);

        //CancelInvoke("ReplayMusic");
    }
    public void backMusicFun()
    {

       Invoke("ReplayMusic",3.0f);

    }

    //背景音乐
    public void ReplayMusic()
    {
        musicPlay(musicType);
    }


	/// <summary>
	/// boss战斗会使用 天下第一，抢夺金币，抢夺龙珠，宿敌
	/// </summary>
	/// <param name="rewardRoot">Reward root. 天下第一武道大会的奖励</param>
	/// <param name="promptTemp">Prompt temp. 天下第一武道大会的标签 </param>
    void ShowSuc(GameObject rewardRoot =null )
    {
        //播放胜利的声音
        musicPlay(SoundFx.FX_Win);
        //关闭背景的声音
        Core.Data.soundManager.BGMStop();

        //胜利
        Status_Win = true;
        //界面胜负标题显示
        this.ShowTitle(1);

		if(Core.Data.temper.currentBattleType == TemporyData.BattleType.WorldBossWar) {
			_suc.SetActive(false);
		} else {
			_suc.SetActive(true);
        
		}
        
        _fail.SetActive(false);
        FinalTrialFailRoot.SetActive(false);
        FinalTrialSucRoot.SetActive(false);

		if(Core.Data.temper.currentBattleType == TemporyData.BattleType.TianXiaDiYiBattle) {
			//rewardRoot.SetActive(false);
		} else if(Core.Data.temper.currentBattleType == TemporyData.BattleType.WorldBossWar) {
			WorldBossWin.SetActive(true);
			WorldBossWinPoint.text = WorldBossWinPoint.text + " : " + Core.Data.temper.WorldPoint;
		} 

        hideRewardNameFirst();
	
    

    }

	//试练
    void ShowSuccessTrial(GameObject rewardRoot)
    {    
        //播放胜利的声音
        musicPlay(SoundFx.FX_Win);
        //关闭背景的声音
        Core.Data.soundManager.BGMStop();
        //胜利
        Status_Win = true;
        //界面胜负标题显示
        this.ShowTitle(1);

        _suc.SetActive(true);
        _fail.SetActive(false);
		
        FinalTrialSucRoot.gameObject.SetActive(true);
        FinalTrialFailRoot.gameObject.SetActive(false);
        hideRewardNameFirst();


    }
	//试练
    void ShowFailTrial(GameObject rewardRoot)
    {    
        //播放失败的声音
        musicPlay(SoundFx.FX_Fail);
        //关闭背景的声音
        Core.Data.soundManager.BGMStop();

        //失败动画
        this.PlayLoseAni();
        this.ShowTitle(0);

        _suc.SetActive(false);
        _fail.SetActive(true);
		
        FinalTrialSucRoot.gameObject.SetActive(false);
        FinalTrialFailRoot.gameObject.SetActive(true);

    }

    void ShowConflict()
    {
        // yangchenguang 
        buttonReplay.SetActive(true);
        vsPro.SetActive(true);


        backButton.SetActive(true);
        _conflict.SetActive(true);
        buttonConflict.SetActive(false);
        _resultObj.SetActive(false);
        buttonExit.SetActive(false);





        if (Core.Data.temper.currentBattleType == TemporyData.BattleType.BossBattle)
        {
            againBat.gameObject.SetActive(false);
        }
        if (pvpStatus == 1 )
        {
            shareUIBtn.gameObject.SetActive(false);// 分享按钮
        }
        // yangchenguang 添加战斗结算阵容对比界面
        if (backGroundScroll.activeSelf == false  )
        {
            backGroundScroll.SetActive(true) ;
        }

    }

    void CloseConflict()
    {
        // yangchenguang 
        vsPro.SetActive(false);
        buttonReplay.SetActive(false);


        backButton.SetActive(false);
        _conflict.SetActive(false);
        buttonConflict.SetActive(true);
        _resultObj.SetActive(true);
        buttonExit.SetActive(true);


        NewFloor city = Core.Data.newDungeonsManager.curFightingFloor;
       

        if (Core.Data.temper.currentBattleType == TemporyData.BattleType.BossBattle)
        {
            if (city.config.isBoss==1)
            {
               // againBat.gameObject.SetActive(true);

                if(Core.Data.guideManger.SpecialGuideID >0 )
                {
                    againBat.gameObject.SetActive(false);

                }else
                {
                    againBat.gameObject.SetActive(true);
                }


            }
        }
        if (pvpStatus == 1 )
        {
            shareUIBtn.gameObject.SetActive(true);// 分享按钮
        }
        // yangchenguang 添加战斗结算阵容对比界面
        if (backGroundScroll.activeSelf == true ) 
        {
            backGroundScroll.SetActive(false) ;
        }
       
    }

    private bool opened = false;

    void Open1()
    {
        if(!opened) {
            pos = 0;
            ShowReward();
            opened = true;
        }
    }

    void Open2()
    {
        if(!opened) {
            pos = 1;
            ShowReward();
            opened = true;
        }
    }

    void Open3()
    {
        if(!opened) {
            pos = 2;
            ShowReward();
            opened = true;
        }
    }

    void OpenAll()
    {
        mask.enabled = true;
        StartCoroutine(OpenEnd());
        if (pos == 0)
        {
            StartCoroutine(Open1(true));
        }

    }

	/// <summary>
	/// 重播战斗
	/// </summary>
    void Replay()
    {
        if (OnReplay != null)
        {
            OnReplay();
            if(GameObject.Find("DropBox(Clone)"))
            {
                Destroy(GameObject.Find("DropBox(Clone)"));

            }

			//关闭循环声效
			Core.SoundEng.StopChannel(curSoundLayer);
            AsyncTask.RemoveQueueOnMainThread(backMusicFun);
            CancelInvoke("ReplayMusic");

            //再次播放战斗的声音
           // Debug.Log("backMusic");
            Core.Data.soundManager.BGMPlay(SceneBGM.BGM_BATTLE);
        }
    }

    int pos = -1;
    public void ShowReward()
    {
        reward = Core.Data.temper.warBattle.reward;
        if (reward != null && reward.p != null )
        {
            if (reward.p.Length<0) return ;
			sortByStar(reward.p);
            reward.p = AnalysisReward(reward.p);
            int length =reward.p.Length;
            for (int i = 0; i < length; i++)
            {

                ItemOfReward item = null;
                if (reward.p != null)
                {
                    item = reward.p[i];// 打开箱子的奖励
                }

                if (item != null)
                {
                    showReward(item, i);
                }
                else
                {
                    if (reward.eep > 0)
                    {
                        showCoinOrExp(pos, false);
                    }
                    else if (reward.eco > 0)
                    {
                        showCoinOrExp(pos);
                    }
                }


            }
            //OpenAll();
        }
    }
    //分析奖励 -- yangchenguang 筛选奖励
    private  ItemOfReward[] AnalysisReward(ItemOfReward[] m_tempData)
    {
       
        Dictionary<int, ItemOfReward> dicTemp = new Dictionary<int, ItemOfReward> ();
        for (int i = 0; i < m_tempData.Length; i++) {
            if (dicTemp.ContainsKey (m_tempData [i].pid)) {
                dicTemp [m_tempData [i].pid].num++;
            } else {
                dicTemp.Add (m_tempData [i].pid, m_tempData [i]);
            }
        }
        ItemOfReward[] m_data  = new ItemOfReward[dicTemp.Count];
//        if (m_data == null) {
//            m_data = new ItemOfReward[dicTemp.Count];
//        }
        m_data.safeFree ();
        int count = 0;
        foreach (KeyValuePair<int, ItemOfReward> itor in dicTemp) {
            m_data [count] = itor.Value;
            count++;
        }

        return m_data;
     
    }


	void lightEx(GameObject father, int star) {

		if(father == null) return;

		Object obj = null;

		if(star == 4) {
			obj = PrefabLoader.loadFromUnPack("Ban/UI_fangsheguang_lan", false);
		} else if(star == 5) {
			obj = PrefabLoader.loadFromUnPack("Ban/UI_fangsheguang_zi", false);
		}

		if(obj != null) {
			GameObject go = Instantiate(obj) as GameObject;
			RED.AddChild(go, father);
		}
	}

	void sortByStar(ItemOfReward[] allStuff) {
		if(allStuff == null || allStuff.Length == 0) return;

		int length = allStuff.Length;
		for (int i = 0; i < length; i++) {
			ItemOfReward item = allStuff[i];
				
			switch (item.getCurType()) { 
			case ConfigDataType.Monster:
				Monster m = item.toMonster(Core.Data.monManager);
				if (m != null) { item.mStar = m.Star;}
				break;
			case ConfigDataType.Equip:
				Equipment e = item.toEquipment(Core.Data.EquipManager, Core.Data.gemsManager);
				if (e != null) { item.mStar = e.ConfigEquip.star; }
				break;
			case ConfigDataType.Item:
				Item it = item.toItem (Core.Data.itemManager);
				if (it != null) { item.mStar = it.configData.star; }
				break;
			case ConfigDataType.Frag:
				Soul soul = item.toSoul(Core.Data.soulManager);
				if( soul != null) { item.mStar = soul.m_config.star; }
				break;
			case ConfigDataType.Gems:
				Gems gems = item.toGem(Core.Data.gemsManager);
				if (gems != null) { item.mStar = gems.configData.star;}
				break;
			default:
				item.mStar = 0;
				break;
			}
		}

		List<ItemOfReward> temp = new List<ItemOfReward>(allStuff);
		temp.Sort(Compare);
		reward.p = temp.ToArray();
	}

	static int Compare(ItemOfReward x, ItemOfReward y) {
		return y.mStar - x.mStar;
	}

    void showReward(ItemOfReward item, int i)
    {
        if (item == null ) return ;
        if (item.num > 1)
        {
            rewardList[i].pNum = ItemNumLogic.setItemNum(item.num ,rewardList[i]._pNum , rewardList[i]._pNumBG);
        } else {
            rewardList[i]._pNum.text="";
            rewardList[i]._pNumBG.gameObject.SetActive(false);
        }

        switch (item.getCurType())
        {
            case ConfigDataType.Monster:
                Monster m = item.toMonster(Core.Data.monManager);
                if (m != null)
                {

                    NGUITools.SetActive(rewardList[i].att.gameObject, true );

                    rewardList[i].HeadID = item.pid;
                    NGUITools.SetActive(rewardList[i].gameObject ,true ) ;
                    NGUITools.SetActive(rewardName[i].gameObject, true);
                    rewardName[i].text = m.config.name;

                    NGUITools.SetActive(rewardList[i].Sc_star.gameObject, true);
                    rewardList[i].Star = m.Star;

                    if (Core.Data.monManager.IsExpMon(m.num))
                    {
                        rewardList[i].setAttAltasExp();
                        rewardList[i].BorderAtlas = 6;

                    }else
                    {
                      rewardList[i].Att    = m.RTData.m_nAttr;
                      rewardList[i].Border = m.RTData.m_nAttr;

                    }
					lightEx(rewardList[i].gameObject, m.Star);
                }
                break;
            case ConfigDataType.Equip:
                Equipment e = item.toEquipment(Core.Data.EquipManager, Core.Data.gemsManager);
                if (e != null)
                {
                    
                    rewardList[i].EquipID = item.pid;
                    NGUITools.SetActive(rewardList[i].gameObject ,true ) ;

                    NGUITools.SetActive(rewardName[i].gameObject, true);
                    rewardName[i].text = e.name;

                    NGUITools.SetActive(rewardList[i].Sc_star.gameObject, true);
                    rewardList[i].Star = e.ConfigEquip.star;
                    NGUITools.SetActive(rewardList[i].att.gameObject, false);

                    rewardList[i].BorderAtlas = 6;
                    if (Core.Data.EquipManager.IsExpEquipment(e.Num))
                    {
                      NGUITools.SetActive(rewardList[i].att.gameObject, true );
                      rewardList[i].setAttAltasExp();
                    }
				    
					lightEx(rewardList[i].gameObject, e.ConfigEquip.star);
                }
                break;
			case ConfigDataType.Item:


				Item it = item.toItem (Core.Data.itemManager);
				if (it != null)
				{
//					rewardList [i].PropsID = it.RtData.num;
					rewardList [i].PropsID = it.configData.iconID;
					NGUITools.SetActive (rewardName [i].gameObject, true);
					rewardName [i].text = it.configData.name;

					NGUITools.SetActive (rewardList [i].Sc_star.gameObject, true);
					rewardList [i].Star = it.configData.star;
					NGUITools.SetActive (rewardList [i].att.gameObject, false);

                     NGUITools.SetActive(rewardList[i].gameObject ,true ) ;

                     rewardList[i].BorderAtlas = 6;


				}

				lightEx(rewardList[i].gameObject, it.configData.star);

                break;
            case ConfigDataType.Frag:

                bool  isEquip = false ; 
               //魂
                Soul soul = item.toSoul(Core.Data.soulManager);
                EquipData equipData  =Core.Data.EquipManager.getEquipConfig (soul.m_config.updateId);

                if (soul != null)
                {

                    //魂魄
                    if(soul.m_config.type == (int)ItemType.Monster_Frage)
                    {
                        rewardList[i].HeadID = soul.m_config.updateId;
                        NGUITools.SetActive(rewardList[i].hun.gameObject, true);

                    } else if(soul.m_config.type == (int)ItemType.Nameike_Frage || soul.m_config.type == (int)ItemType.Earth_Frage) 
                    {
                      
                        rewardList[i].PropsID = item.pid;
                    }else if (soul.m_config.type == (int)ItemType.Equip_Frage)
                    {
                        if (equipData != null)
                        {
                           rewardList[i].EquipID =       soul.m_config.updateId;
                           rewardList[i].hun.atlas = rewardList[i].commonAtlas;
                           NGUITools.SetActive(rewardList[i].hun.gameObject, true);
                           rewardList[i].hun.spriteName = "sui" ;
                          //rewardList[i].hun.atlas
                          isEquip = true  ;

                        }else
                        {
                            break ;
                        }
                    }
                    
                    NGUITools.SetActive(rewardList[i].gameObject ,true ) ;

					//rewardList[i].HeadID = soul.m_config.updateId;
                    NGUITools.SetActive(rewardName[i].gameObject, true);

                    NGUITools.SetActive(rewardList[i].Sc_star.gameObject, true);
                    NGUITools.SetActive(rewardList[i].att.gameObject, false);

                    if (isEquip )
                    {
                        
                        rewardName[i].text = soul.m_config.name;
                        rewardList[i].Star = equipData.star ;
                        rewardList[i].BorderAtlas = 6;

                    }else
                    {
                        rewardName[i].text = soul.m_config.name;
                        rewardList[i].Star =  soul.m_config.star;
                        rewardList[i].BorderAtlas = 6;
                    }
                   
					lightEx(rewardList[i].gameObject, soul.m_config.star);

                }
                break;
            case ConfigDataType.Gems:
            Gems  gems = item.toGem(Core.Data.gemsManager);
            if (gems != null)
            {

                rewardList[i].headIDCommonAtlas = gems.configData.anime2D;
                NGUITools.SetActive(rewardList[i].gameObject ,true ) ;
                NGUITools.SetActive(rewardName[i].gameObject, true);
                rewardName[i].text = gems.configData.name;

                NGUITools.SetActive(rewardList[i].Sc_star.gameObject, true);
                rewardList[i].Star = gems.configData.star;

                rewardList[i].Border = 5;

				lightEx(rewardList[i].gameObject, gems.configData.star);
            }
                break ;
            default:
                ConsoleEx.DebugLog("Can't get this kinds of things");
                break;
        }
    }

    void showCoinOrExp(int pos, bool coin = true)
    {

        if (coin)
        {
            switch (pos)
            {
                case 0:
                   // coin1.spriteName = "EUB0-55";
                  //  NGUITools.SetActive(coin1.gameObject, true);
                    NGUITools.SetActive(rewardName[pos].gameObject, true);
                    rewardName[pos].text = Core.Data.stringManager.getString(5037);
                    break;
                case 1:
                    //coin2.spriteName = "EUB0-55";
                   // NGUITools.SetActive(coin2.gameObject, true);
                    NGUITools.SetActive(rewardName[pos].gameObject, true);
                    rewardName[pos].text = Core.Data.stringManager.getString(5037);
                    break;
                case 2:
                   // coin3.spriteName = "EUB0-55";
                  //  NGUITools.SetActive(coin3.gameObject, true);
                    NGUITools.SetActive(rewardName[pos].gameObject, true);
                    rewardName[pos].text = Core.Data.stringManager.getString(5037);
                    break;
            }
        }
        else
        { 
            switch (pos)
            {
                case 0:
                    //coin1.spriteName = "EUB0-55";
                  //  NGUITools.SetActive(coin1.gameObject, true);
                    NGUITools.SetActive(rewardName[pos].gameObject, true);
                    rewardName[pos].text = Core.Data.stringManager.getString(5037);
                    break;
                case 1:
                  //  coin2.spriteName = "EUB0-55";
                   // NGUITools.SetActive(coin2.gameObject, true);
                    NGUITools.SetActive(rewardName[pos].gameObject, true);
                    rewardName[pos].text = Core.Data.stringManager.getString(5037);
                    break;
                case 2:
                   // coin3.spriteName = "EUB0-55";
                   // NGUITools.SetActive(coin3.gameObject, true);
                    NGUITools.SetActive(rewardName[pos].gameObject, true);
                    rewardName[pos].text = Core.Data.stringManager.getString(5037);
                    break;
            }
        }


    }

	// combo 
	void SendComboMsg()
	{
		//累计Combo的连击数
		Core.Data.playerManager.RTData.TotalCombo += Core.Data.temper.FingerTotalCombo;
        if(Core.Data.temper.warBattle.gambleResult != null)
            if(Core.Data.temper.warBattle.gambleResult.winFlag == true)
                Core.Data.playerManager.RTData.TotalGambleWin ++;
//		if(!Core.Data.guideManger.isGuiding) {
//			HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Igonre_Response);
//			task.AppendCommonParam(RequestType.BATTLE_COMBO, new BattleComboParam(Core.Data.playerManager.PlayerID, Core.Data.temper.MaxCombo, Core.Data.temper.TotalCombo,Core.Data.temper.FingerMaxCombo,Core.Data.temper.FingerTotalCombo));
//			task.DispatchToRealHandler();
//		}
	}

    void BackToGame()
    {
		if(Core.Data.temper.currentBattleType == TemporyData.BattleType.WorldBossWar)
			pos = 1;
		if (pos == -1 && Status_Win && FinalTrialMgr.GetInstance().NowEnum != TrialEnum.TrialType_PuWuChoose && FinalTrialMgr.GetInstance().NowEnum != TrialEnum.TrialType_ShaLuChoose)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(10), BanBattleManager.Instance.go_uiPanel);
        }
        else
        {


           // Debug.Log("isExit == " + isExit);
			if(BanBattleProcess.Instance == null) 
            {
                RED.Log("isExit 11111111111");

                isAgain= true ; 
                return;
            }
           // Debug.Log("isExit 2222222222");

            if (isExit == true )
            {
                isAgain= false ; 

                isExit = false ; 
            }else 
            {
                return ; 
            }
            //发送连接数
            SendComboMsg ();
            RED.Log("isExit == "  + isExit);
			TemporyData.BattleType curbattle = Core.Data.temper.currentBattleType;

			//关闭循环声效
			Core.SoundEng.StopChannel(curSoundLayer);
			AsyncTask.RemoveAllDelayedWork();

            CancelInvoke("ReplayMusic");

			if(BanBattleManager.Instance != null)
				BanBattleManager.Instance.ReleaseADestory();


            buttonConflict.GetComponent<UIButton>().enabled =false ;
            shareUIBtn.enabled =false ; 
			//更新用户等级
			if(curbattle == TemporyData.BattleType.BossBattle || 
				curbattle == TemporyData.BattleType.QiangDuoDragonBallBattle ||
				curbattle == TemporyData.BattleType.QiangDuoGoldBattle || 
				curbattle == TemporyData.BattleType.Revenge || 
				curbattle == TemporyData.BattleType.TianXiaDiYiBattle ||
				curbattle == TemporyData.BattleType.SuDiBattle ) {
				//设定用户的等级
				if (Core.Data.temper.AfterBattleLv > Core.Data.playerManager.RTData.curLevel) {
					Core.Data.playerManager.SetCurUserLevel (Core.Data.temper.AfterBattleLv);
				}
			}

            Core.SM.beforeLoadLevel(SceneName.GAME_BATTLE, SceneName.MAINUI);
            AsyncLoadScene.m_Instance.LoadScene(SceneName.MAINUI);
			Core.Data.temper.WarErrorCode = -1;
        }
    }
    //回到主界面
    void backMainScene()
    {
        if(Core.Data.temper.currentBattleType == TemporyData.BattleType.WorldBossWar)
            pos = 1;
        if (pos == -1 && Status_Win && FinalTrialMgr.GetInstance().NowEnum != TrialEnum.TrialType_PuWuChoose && FinalTrialMgr.GetInstance().NowEnum != TrialEnum.TrialType_ShaLuChoose)
        {
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(10), BanBattleManager.Instance.go_uiPanel);
        }else
        {
            //发送连接数
            SendComboMsg ();

			TemporyData.BattleType curbattle = Core.Data.temper.currentBattleType;

			if(curbattle == TemporyData.BattleType.BossBattle || 
				curbattle == TemporyData.BattleType.QiangDuoDragonBallBattle ||
				curbattle == TemporyData.BattleType.QiangDuoGoldBattle || 
				curbattle == TemporyData.BattleType.Revenge || 
			    curbattle == TemporyData.BattleType.TianXiaDiYiBattle ||
				curbattle == TemporyData.BattleType.SuDiBattle ) {
                //设定用户的等级
                if (Core.Data.temper.AfterBattleLv > Core.Data.playerManager.RTData.curLevel) {
                    Core.Data.playerManager.SetCurUserLevel (Core.Data.temper.AfterBattleLv);
                }
            }

            //关闭循环声效
            Core.SoundEng.StopChannel(curSoundLayer);
            AsyncTask.RemoveQueueOnMainThread(backMusicFun);
            CancelInvoke("ReplayMusic");
            //AsyncTask.RemoveQueueOnMainThread(playMusicFin);
            Core.SM.beforeLoadLevel(SceneName.GAME_BATTLE, SceneName.MAINUI);
            AsyncLoadScene.m_Instance.LoadScene(SceneName.MAINUI);
			FinalTrialMgr.GetInstance().jumpTo = TrialEnum.None;
            BattleToUIInfo.From = RUIType.EMViewState.MainView;
        }
    }
    //战斗分享功能
    void ShareBatt()
    {

        string value =  Core.Data.temper.warBattle.battleData.videoId.ToString();
        string Attackname =  BanTools.GetAttackPlayerName();
        string  Defensename = BanTools.GetDefensePlayerName();

        FinalTrialMgr.GetInstance().OpenMessageTag(value  ,Attackname ,Defensename, BattleVideoTagType.Type_Share);
    }
    // 打开 整容调整界面
    void openTeamChange()
    {

    }

    public float rotationTime = 0.3f;
    bool first = true;

    IEnumerator Open1(bool Immediately)
    {
        if (!Immediately)
        {
            yield return new WaitForSeconds(rotationTime / 2);
           // this.ShowOpenBox(box1, 2);
            rotateLight[0].gameObject.SetActive(false);
        }
        else
        {
           // this.ShowOpenBox(box1, 1);
            rotateLight[0].gameObject.SetActive(false);
        }
       
        yield return new WaitForSeconds(rotationTime / 2);
        //  box1.gameObject.SetActive(false);

        if ((reward.eep > 0 || reward.eco > 0) && (pos == 0))
        {
            ShowBoxReward(rewardList[0].gameObject,false);
        }
        else
        {
			ShowBoxReward(rewardList[0].gameObject,true);
        }

        if (first)
        {
            first = false;
  
        }
    }

    IEnumerator Open2(bool Immediately)
    {
        if (!Immediately)
        {
            yield return new WaitForSeconds(rotationTime / 2);
           // this.ShowOpenBox(box2, 2);
            rotateLight[1].gameObject.SetActive(false);
        }
        else
        {
         //   this.ShowOpenBox(box2, 1);
            rotateLight[1].gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(rotationTime / 2);
        if ((reward.eep > 0 || reward.eco > 0) && (pos == 1))
        {
            ShowBoxReward(rewardList[1].gameObject,false);
        }
        else
        {
            ShowBoxReward(rewardList[1].gameObject,true);
        }
        if (first)
        {
            first = false;
            StartCoroutine(Open1(false));
            StartCoroutine(Open3(false));
        }
    }

    IEnumerator Open3(bool Immediately)
    {
        if (!Immediately)
        {
            yield return new WaitForSeconds(rotationTime / 2);
           // this.ShowOpenBox(box3, 2);
            rotateLight[2].gameObject.SetActive(false);
        }
        else
        {
            //this.ShowOpenBox(box3, 1); 
            rotateLight[2].gameObject.SetActive(false);

        }
        yield return new WaitForSeconds(rotationTime / 2);
        if ((reward.eep > 0 || reward.eco > 0) && (pos == 2))
        {
            ShowBoxReward(rewardList[2].gameObject,false);
        }
        else
        {
            ShowBoxReward(rewardList[2].gameObject,true);
        }
        if (first)
        {
            first = false;
            StartCoroutine(Open2(false));
            StartCoroutine(Open1(false));
        }
    }
    //1：普通  2：延迟
    void ShowOpenBox(UISprite sp, int type)
    {
        if (type == 1)
        {
            sp.spriteName = "ActiveBox-01";
            sp.GetComponent<UISpriteAnimation>().namePrefix = "ActiveBox-";
			sp.GetComponent<UISpriteAnimation>().ResetToBeginning();
            sp.GetComponent<UISpriteAnimation>().enabled = true;
       

        }
        else
        {
            sp.spriteName = "ActiveBox-01";
            sp.GetComponent<UISpriteAnimation>().namePrefix = "UnActiveBox-";
			sp.GetComponent<UISpriteAnimation>().ResetToBeginning();
            sp.GetComponent<UISpriteAnimation>().enabled = true;
        }

    }

    void ShowBoxReward(GameObject obj,bool isOut )
    {
        AnimationCurve anim = null;
        obj.transform.localScale = Vector3.zero; 
        if (isOut == true)
        {
            obj.SetActive(true);
            anim = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(0.4f, 1f, 1f, 1f));
        }
        else
        { 
             anim = new AnimationCurve( new Keyframe(0.4f, 1f, 1f, 1f),new Keyframe(0f, 0f, 0f, 1f));
        }

        if (obj.GetComponent<TweenScale>() != null)
            obj.GetComponent<TweenScale>().animationCurve = anim;
        else
        {
            obj.AddComponent<TweenScale>().animationCurve = anim;
        }

        TweenScale.Begin(obj, 0.3f, Vector3.one);
    }

    

    IEnumerator OpenEnd()
    {
        yield return new WaitForSeconds(rotationTime * 2);
        mask.enabled = false;
        NGUITools.SetActive(buttonClose, true);
        //NGUITools.SetActive(buttonReplay, true);
    }

	//武者强化
	void OnStrengthMon()
    {
       

        FinalTrialMgr.GetInstance().jumpTo = TrialEnum.None;
		BattleToUIInfo.From = RUIType.EMViewState.S_Team_NoSelect;
        BackToGame();
    }
    //扭蛋 要改成武者装备强化
    void onNiuDan()
    {
        if (Core.Data.itemManager.GetBagItemCount(110024) == 0 && Core.Data.itemManager.GetBagItemCount(110025) == 0 && Core.Data.itemManager.GetBagItemCount(110026) == 0)
        {
            BattleToUIInfo.From = RUIType.EMViewState.S_ShangCheng;
        }
        else
        {
            BattleToUIInfo.From = RUIType.EMViewState.S_Bag;
        }
        FinalTrialMgr.GetInstance().jumpTo = TrialEnum.None;
        BackToGame();
    }
    // 再来一次 战斗
    void againBattle()
    {
        #if NEWPVE
        NewFloor city = Core.Data.newDungeonsManager.curFightingFloor;
       // bool att = city.config.FightType == 1;
        #else
        City city = CityFloorData.Instance.currCity;
        Floor floor = CityFloorData.Instance.currFloor;  
        bool att = floor.config.gf == 1;
        #endif


        if (isAgain == true )
        {
            isAgain = false ; 
        }else
        {
            return ; 
        }
        RED.Log("isAgain == " + isAgain);

        BackToGame();

        //city.config.ID;  再来一次跳转副本ID
       // Debug.Log("city.config.name == " + city.config.name);
        DBUIController.battleAgain = true ; 
        DBUIController.battleAgainID = city.config.ID;
       // DBUIController.mDBUIInstance.OpenFVE(PVEType.PVEType_Plot,city.config.ID);


    }
    // 获取战斗右边队伍的所有防御值
    public int  getBattleRightTeamDef()
    {
        TemporyData tempData =   Core.Data.temper;
		int length =  tempData.clientDataResp.data.warInfo.defTeam.team.Length;

        int def = 0; 
        Battle_Monster monster; 
        for (int i =0 ;i <length ; i++)
        {
			monster = tempData.clientDataResp.data.warInfo.defTeam.team[i];
            def+=monster.finalAtt;
        }
        return def ;
    }
    //combe奖励显示
    public void SetCombe(string combeStr , string combeCoin)
    {

        for(int i =0  ; i <combeStr.Length ; i++)
        {
            combeList[i].gameObject.SetActive(true);
            RED.Log("combeStr[i] == "  +combeStr[i]);
            combeList[i].spriteName = "hit_" + combeStr[i].ToString();
        }

        combeLabel.text ="+" + combeCoin ; 
    }
}
