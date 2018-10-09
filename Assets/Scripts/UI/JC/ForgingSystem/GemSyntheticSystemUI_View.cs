using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GemSyntheticSystemUI_View : MonoBehaviour {
	
	public System.Action<GameObject> ButtonClick;
	public UISprite Spr_LGem;
	public UISprite Spr_RGem;
	//public UILabel Lab_Title;
	/*当前的宝石精华的数量
	 * */
	//public UILabel Lab_GemDebrisNum;
	/*选择的宝石模具数量(最多选3个)
	 * */
	public UILabel Lab_SelectGemMouldNum;
    /*显示成功率
     * */
	public UILabel Lab_ScuessRate;
	
	public FrogingTopUI forgingTopUI;
	
	public UILabel Lab_stone;
	public UILabel Lab_coin;
	
	//public UILabel Lab_SucessWord;
	//public UILabel Lab_Describe;
	//public UILabel Lab_Recast;
	//public UILabel Lab_Recast_BD;

	public GemHoleViewInfo LGem;
	public GemHoleViewInfo RGem;
	
	public UILabel LabCreateGem;
	
	public UILabel GemInfoDes;
	
	public UISprite Spr_Gem;
		
	public UISprite Spr_ButtonAdd;
	
	public UISprite Spr_ButtonReduction;
	
	public UILabel Spr_FixSucessRate;

    public UILabel lbl_CombineResult;

    public UILabel lbl_TargetNum;

    public UIButton btn_plus;
    public UIButton btn_reduce;
    public UILabel lbl_LeftModelNum;
    public GameObject detailsObj;
    public GameObject blackPic;
	
    public UILabel lbl_desp;

	void OnEnable()
	{
		forgingTopUI.SetTitle("bsxt_bshc");
		forgingTopUI.SetDes(TEXT(9015));
        SetDesp(TEXT(9015));
	}
	
	public string TEXT(int num_text)
	{
		return	Core.Data.stringManager.getString(num_text);
	}
	
	void Start () 
	{
		InitInterface();
	}
	
	void GemSyntheticSystemUIClick(GameObject button)
	{
		if(ButtonClick!=null)
			ButtonClick(button);
	}
	
	public void SetLGem(string GemSpriteName)
	{
		LGem.SetGem (GemSpriteName);
	}
	
	public void SetRGem(string GemSpriteName)
	{
		RGem.SetGem (GemSpriteName);
	}

	public void SetLGemLv(int lv){
		LGem.SetGemLv (lv);
	}
	public void SetRGemLv(int lv){
		RGem.SetGemLv (lv);
	}
	
//	public void SetTitle(string text)
//	{
//		Lab_Title.text=text;
//	}
	
//	public void SetGemDebrisNum(int num)
//	{
//		Lab_GemDebrisNum.text=Core.Data.stringManager.getString(9001)+" "+num.ToString();
//	}
	
    public void SetSelectedGemNumMouldNum(int num ,int remaining,int bNum = 1)
	{
        if (Lab_SelectGemMouldNum != null)
        {
            /*
			 * {"ID":9056,"txt":"使用模具"}
                {"ID":9057,"txt":"背包剩余数"}
			 * */
            // Lab_SelectGemMouldNum.text="[ffe265]"+TEXT(9056)+"："+ num.ToString()+ "[-][00ff00]  ("+TEXT(9057)+":"+remaining.ToString()+")[-]";

            Lab_SelectGemMouldNum.text = num.ToString();
            lbl_LeftModelNum.text = "(" + TEXT(9057) + ":" + remaining.ToString() + ")[-]";
        }
        else
        {
            lbl_LeftModelNum.text = "("+TEXT(9057) + ":" + remaining.ToString()+")[-]"; 
        }
        if(num /bNum > 0)
		{
			Spr_ButtonReduction.color = new Color(1f,1f,1f,1f);
			Spr_ButtonReduction.transform.parent.GetComponent<BoxCollider>().enabled = true;
		}
        if(num/bNum < 3)
		{
			Spr_ButtonAdd.color = new Color(1f,1f,1f,1f);
			Spr_ButtonAdd.transform.parent.GetComponent<BoxCollider>().enabled = true;
		}
		
		if(num == 0)
		{
			Spr_ButtonReduction.color = new Color(0,0,0,1f);
			Spr_ButtonReduction.transform.parent.GetComponent<BoxCollider>().enabled = false;
		}
        else if(num /bNum== 2)
		{
			 Spr_ButtonAdd.color = new Color(0,0,0,1f);
			 Spr_ButtonAdd.transform.parent.GetComponent<BoxCollider>().enabled = false;
		}


	}
	
	public void SetScuessRate(float scuessRate)
	{
		Lab_ScuessRate.text=((int)scuessRate).ToString();
	}
	
    public void SetNeedCoin(int coin)
	{
        string strColor = "[FFFFFF]";
        if (coin > Core.Data.playerManager.Coin)
            strColor = "[FF0000]";

        Lab_coin.text = strColor +  coin.ToString();
	}
	
	public void SetNeedStone(int stone)
	{
        string strColor = "[FFFFFF]";
        if (stone > Core.Data.playerManager.Stone)
            strColor = "[FF0000]";

        Lab_stone.text = strColor + stone.ToString();
	}
	
	public void ShowTargetGemInfo(GemData gem)
	{


        if (gem != null)
        {
            LabCreateGem.enabled = false;
            string str_des = "";

            if (gem.atk > 0)
                str_des = TEXT(9010) +": "+ gem.atk.ToString();
            else if (gem.def > 0)
                str_des = TEXT(9011) +": "+ gem.def.ToString();
            else if (gem.skillEffect > 0)
                str_des = TEXT(9009) +": "+ gem.skillEffect.ToString();
		
            //GemInfoDes.text = gem.name +"\n"+"[ffe265]"+str_des+"[-]";   wxl  
            GemInfoDes.text = str_des; 
            Spr_Gem.enabled = true;
            Spr_Gem.spriteName = gem.anime2D;   
            Spr_Gem.MakePixelPerfect();		
            Spr_Gem.transform.localScale = new Vector3(1.2f, 1.2f, 1f);	
			Spr_Gem.enabled = true;
        }
        else
        {
            LabCreateGem.enabled = true;
            string str_des = "";

            str_des = TEXT(9010) +":";

            GemInfoDes.text = str_des; 
            Spr_Gem.enabled = false;

        }
	}
	
	//初始化界面
	public void InitInterface()
	{
		Spr_FixSucessRate.text = TEXT(9002);//+"：               %";
		Spr_Gem.enabled = false;
		LabCreateGem.enabled = true;
		GemInfoDes.text = "";
        //add by wxl
        int GemMouldNumInBag=Core.Data.itemManager.GetBagItemCount(110064); 
        lbl_LeftModelNum.text = "(" + TEXT(9057) + ":" + GemMouldNumInBag.ToString() + ")[-]";

		Spr_ButtonReduction.color = new Color(0,0,0,1f);
		Spr_ButtonReduction.transform.parent.GetComponent<BoxCollider>().enabled = false;
		Spr_ButtonAdd.color = new Color(0,0,0,1f);
		Spr_ButtonAdd.transform.parent.GetComponent<BoxCollider>().enabled = false;
	}

    #region  add by wxl 
    public void SetCombineGemNum(int tNum){
		lbl_TargetNum.text = tNum.ToString();
    }
    public void SetCombineGemsResult(int cLv,int times,int sucTimes){
        if (cLv != 0 )
        {
            string tStr = string.Format(Core.Data.stringManager.getString(7393), cLv.ToString(), times.ToString(), sucTimes.ToString(), sucTimes.ToString(), (cLv + 1).ToString(), cLv.ToString(), (times - sucTimes).ToString());
            lbl_CombineResult.text = tStr;
        }
        else
        {
            lbl_CombineResult.text = "";
        }
    }

    public void SetNumBtn(bool tPbool, bool tRbool){
        btn_plus.isEnabled = tPbool;
        btn_reduce.isEnabled = tRbool;
    }

    public  void ShowDesp(){
        TweenScale.Begin (detailsObj,0.3f,Vector3.one);
        blackPic.gameObject.SetActive (true);
    }

    public void CloseDesp(){
        TweenScale.Begin (detailsObj,0.3f,Vector3.zero);
        blackPic.gameObject.SetActive (false);
    }

    public void SetDesp(string text){
        lbl_desp.text = text;
    }


    #endregion
}
