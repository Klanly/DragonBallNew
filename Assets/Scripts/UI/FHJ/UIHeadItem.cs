using UnityEngine;
using System.Collections;

public class UIHeadItem : MonoBehaviour 
{
    public UISprite att;
    public UISprite border;
	public UISprite tex;
    
	public UIAtlas headAtlas;
	public UIAtlas equipAtlas;
	public UIAtlas itemAtlas;
    public UIAtlas commonAtlas;

    public StarsUI Sc_star;
    public UISprite hun ; 
    public UILabel _pNum; // 奖品个数
    public UISprite _pNumBG; // 奖品个数背景




    private Vector3 m_IntroducePos;
    private UIEggCellIntroduct m_UIEggCellIntroduct;
    static bool isRewardInfo= false ; 

    public int HeadID
    {
		get{
            return int.Parse(tex.spriteName.Substring(4));
        }
        set{
            AtlasMgr.mInstance.SetHeadSprite(tex, value.ToString());
            tex.MakePixelPerfect();
		}
    }
    public string  headIDCommonAtlas
    {
        set
        {
            tex.atlas = commonAtlas;
            tex.spriteName =  value;
            tex.MakePixelPerfect();

        }
    }
    
    public int EquipID
    {
		get{
            return int.Parse(tex.spriteName.Substring(5));
        }
		set{
			tex.atlas = equipAtlas;
			tex.spriteName=value.ToString();
            tex.MakePixelPerfect();

		}
    }
    
    public int PropsID
    {
		get{
            return int.Parse(tex.spriteName.Substring(5));
        }
		set{
			tex.atlas = itemAtlas;
			tex.spriteName=value.ToString();
            tex.MakePixelPerfect();

		}
    }

    public int Att
    {
        set {
            att.spriteName = "Attribute_" + value;

        }
    }

    public void setAttAltasExp ()
    {
        att.atlas = commonAtlas;
        att.spriteName ="common-1055" ;
        att.MakePixelPerfect();
    }
    public int Border
    {
       set
        {
            //border.atlas = "";
            border.spriteName = "Attribute_Border_" + value;
            border.MakePixelPerfect();
        }

//        set 
//        {
//            border.spriteName = "common-0017" ;
//        }
    }
    public int BorderAtlas
    {
        set
        {
            border.atlas = commonAtlas;
            border.spriteName = "star" + value;
            border.MakePixelPerfect();
        }
    }

    public int Star {
        set { Sc_star.SetStar(value); }
    }
    public string pNum
    {
        set{ _pNum.text =  value ;}
    }

    void OnPress( bool press)
    {

        if (press)
        {
            if (isRewardInfo ) return  ;

            if (go  == null)
            {
                ClickRewardItemInfo(gameObject) ;

            }else
            {
                go.gameObject.SetActive(true )  ;
            }
            isRewardInfo = true ;

        }else
        {
            isRewardInfo = false ;
            go.gameObject.SetActive(false)  ;

        }
    }


    public void ClickRewardItemInfo(GameObject go )
    {
       

        string str =  go.transform.parent.gameObject.name;

        string[] strList  = str.Split('m');

        int value = int.Parse(strList[1]) -1 ; 

      
        BattleReward reward = Core.Data.temper.warBattle.reward;
        Vector3 v3 = new Vector3( 0 , border.height/2 , 0);
        if (value ==  0 )
        {
            v3 = new Vector3(65 , border.height/2 , 0);
        }
        InitHeadIntroduce(reward.p[value] ,v3); 


    }
    GameObject go  ; 
    public void InitHeadIntroduce(ItemOfReward _ItemOfReward, Vector3 m_Pos)
    {
        m_IntroducePos = m_Pos;
        GameObject obj1  = PrefabLoader.loadFromPack("LS/pbLSCardHeadIntroduct") as GameObject ;
        if(obj1 != null)
        {
            go = NGUITools.AddChild (gameObject, obj1);
            go.SetActive(true);
            UIEggCellIntroduct mm = go.gameObject.GetComponent<UIEggCellIntroduct>();
            m_UIEggCellIntroduct = mm;
            mm.obj.transform.localPosition = m_IntroducePos ; 
            m_UIEggCellIntroduct.OnShow(_ItemOfReward);

        }

    }

}
