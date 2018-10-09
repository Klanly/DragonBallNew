using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CombinationSkillPanelScript : RUIMonoBehaviour {

	/// <summary>
	/// 人物介绍内容
	/// </summary>
	public UILabel CharacterLabel;
	/// <summary>
	/// 四个组合技能技能描述
	/// </summary>
	public UILabel[] SkillDesLabelArray;
	/// <summary>
	/// 四个组合技能名称
	/// </summary>
	public UILabel[] SkillTitleLabelArray;
	/// <summary>
	/// 技能标题的背景（是否需要隐藏？）
	/// </summary>
	public UISprite[] SkillTitleBackgroundArray;
	/// <summary>
	/// 游戏界面的根节点 用来移动和缩放
	/// </summary>
	public GameObject PanelRoot;

    public UIButton[] btnFateTarget;

    public List<FateData> fateList ;

    /// <summary>
    /// 第五项  说明
    /// </summary>
    public UILabel specialLblDesp;
    public UILabel specialLblTitle;
    public UISprite specialBg;

    Vector3 specialDespPos = new Vector3 (-180,-5,0);
    Vector3 bgTitleBgPos = new Vector3 (-265,0,0);
    Vector3 titlePos = new Vector3 (-265,-5,0);

	//   Color lightColor = new Color (1f,155f/255f,0,1f);
    string addAng ;
    string strColor = "[FFDF00]";

	/// <summary>
	/// 关闭按钮点下
	/// </summary>
	void OnXBtnClick()
	{
		TweenScale tween = PanelRoot.GetComponent<TweenScale>();
		tween.delay = 0;
		tween.duration = 0.25f;
		tween.from =  Vector3.one;
		tween.to = new Vector3(0.01f,0.01f,0.01f);
		tween.onFinished.Clear();
		tween.onFinished.Add(new EventDelegate(this,"DestroyPanel"));
		tween.ResetToBeginning();
		tween.PlayForward();
	}

	public void DestroyPanel()
	{
		Destroy(gameObject);
	}

	/// <summary>
	/// 初始化界面信息
	/// </summary>
	public  void InitPanel(Monster mt, MonsterTeam m_Team = null)
	{
		CharacterLabel.text = mt.config.description;
		//SetShowLabelAndSprite(,,,);
		List<FateData> o = mt.getMyFate(Core.Data.fateManager);
        fateList = o;
		List<AoYi> aoyiList = Core.Data.dragonManager.usedToList ();
 		MonsterTeam myteam = null;
		if(m_Team == null)myteam =  Core.Data.playerManager.RTData.curTeam;
		else 
		{
			myteam = m_Team;
		}
		int count = o.Count;

		for (int i = 0; i < SkillTitleLabelArray.Length; i++) {
			SkillTitleBackgroundArray[i].gameObject.SetActive  (false);
			SkillTitleLabelArray [i].gameObject.SetActive(false);
			SkillDesLabelArray [i].gameObject.SetActive(false);
            btnFateTarget [i].gameObject.SetActive (false);
		}
        specialLblTitle.text = Core.Data.stringManager.getString (7335);
        specialLblDesp.text =string.Format( Core.Data.stringManager.getString(7336),mt.config.nuqi1);

        int AllSetUp = 0;
        specialLblDesp.transform.localPosition = specialDespPos + Vector3.down* count *60;
        specialLblTitle.transform.localPosition = titlePos + Vector3.down * count * 60;
        specialBg.transform.localPosition = bgTitleBgPos + Vector3.down * count * 60;

		for (int i = 0; i < count; i++) {
			SkillTitleBackgroundArray[i].gameObject.SetActive(true);
			SkillTitleLabelArray [i].gameObject.SetActive(true);
			SkillDesLabelArray [i].gameObject.SetActive(true);
            btnFateTarget [i].gameObject.SetActive (true);
			SkillTitleLabelArray [i].text = o [i].name;
            string tStrColor = "";
			if (mt.checkMyFate (o [i], myteam, aoyiList)) {
                SkillTitleBackgroundArray[i].spriteName = "battle-0010";
                SkillTitleLabelArray [i].color = Color.white;
                //  SkillDesLabelArray [i].color = lightColor;
                tStrColor = strColor;
                AllSetUp++;
			} else {
                SkillTitleBackgroundArray[i].spriteName = "common-0028";
                SkillTitleLabelArray [i].color =Color.white;
                SkillDesLabelArray [i].color = Color.white;
			}
            addAng =string.Format( Core.Data.stringManager.getString(1008),o[i].nuqi.ToString());
            SkillDesLabelArray [i].text = tStrColor + o [i].description + addAng;

		}
        if (count != 0) {

            if (AllSetUp == count) {
                //specialLblDesp.color = lightColor;
                specialLblDesp.text = strColor + specialLblDesp.text;
                specialLblTitle.color = Color.white;
                specialBg.spriteName = "battle-0010";
            } else {
                specialLblDesp.color = Color.white;
                specialLblTitle.color = Color.white;
                specialBg.spriteName = "common-0028";
            }
        } else {
            specialLblDesp.gameObject.SetActive(false);
            specialLblTitle.gameObject.SetActive(false);
            specialBg.gameObject.SetActive(false);
        }
	}

    void FateBtnMethord(FateData fd){
		ShowFatePanelController.CreatShowFatePanel (fd.ID,ShowFatePanelController.FateInPanelType.isInSkillInfoPanel, this);
    }

    void FateBtnOne(){
        if(fateList.Count>0)
        FateBtnMethord (fateList[0]);
    }
    void FateBtnTwo(){
        if(fateList.Count>1)
             FateBtnMethord (fateList[1]);
    }
    void FateBtnThree(){
        if(fateList.Count>2)
            FateBtnMethord (fateList[2]);
    }
    void FateBtnFour(){
        if(fateList.Count>3)
            FateBtnMethord (fateList[3]);
    }

	void DisableBtnFateTarget()
	{
		for(int i=0; i<btnFateTarget.Length; i++)
		{
			btnFateTarget[i].isEnabled = false;
		}
	}

	/// <summary>
	/// 显示和隐藏不需要显示的组合技部分
	/// </summary>
	/// <value>The set show label and sprite.</value>
//	void SetShowLabelAndSprite(int showNum,string[] skillDes,string[] skillTitle)
//	{
//		int i = 0;
//		for (; i < showNum; i++) {
//			SkillDesLabelArray[i].gameObject.SetActive (true);
//			SkillDesLabelArray[i].text = skillDes[i];
//			SkillTitleBackgroundArray[i].gameObject.SetActive (true);
//			SkillTitleLabelArray[i].gameObject.SetActive (true);
//			SkillTitleLabelArray[i].text = skillTitle[i];
//		
//		}
//		for (; i < 4; i++) {
//			SkillDesLabelArray [i].gameObject.SetActive (false);
//			SkillTitleBackgroundArray [i].gameObject.SetActive (false);
//			SkillTitleLabelArray [i].gameObject.SetActive (false);
//		}
//	}


	/// <summary>
	/// 显示界面并初始化信息，需要传一个数据参数
	/// </summary>
	/// <returns>The combination skill panel.</returns>
	/// <param name="root">Root.</param>
	public static CombinationSkillPanelScript ShowpbCombinationSkillPanel( GameObject root,Monster o, bool m_EnableClick = true, MonsterTeam m_Team = null){
		
		GameObject obj = PrefabLoader.loadFromPack("GX/pbCombinationSkillPanel")as GameObject ;
		if(obj !=null)
		{
			GameObject go = NGUITools.AddChild(root,obj);
			CombinationSkillPanelScript script = go.GetComponent<CombinationSkillPanelScript>();
			script.InitPanel(o, m_Team);
			Instance = script;
			if(!m_EnableClick)
			{
				script.DisableBtnFateTarget();
			}
			return script;
		}
		return null;
		
	}
	
	
	public static CombinationSkillPanelScript Instance;
	
	void OnDestroy()
	{
		Instance = null;
	}
	
}
