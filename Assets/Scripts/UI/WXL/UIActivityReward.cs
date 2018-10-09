using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIActivityReward : RUIMonoBehaviour
{
    public UILabel giftNum;
    public GameObject giPos;
    public UISprite att;
   
    public UILabel _name;
    public GameObject starRoot;
    public GameObject starTemp;
    public bool stared = false;
    private ActivityReward m_rewardData;
    private HonorItemData honorGiftData;
    public UISprite giftIcon;
    public StarsUI starsPic;
    public GameObject numObj;
	//double
	public GameObject panelObj;
	public UISprite border;
    Vector3 BtnPos = new Vector3(120,-90,0);
    Vector3 BtnOrgPos = new Vector3(0,-90,0);
	public UIButton btnGet;

	public UILabel _name_D;
	public UISprite giftIcon_D;
	public StarsUI starsPic_D;
	public GameObject numObj_D;
	public UILabel giftNum_D;
	public UISprite att_D;


    int giftIndex=0;
    public delegate void onClose();

    public onClose OnClose = null;

    private ItemOfReward I_RewardData;

    public List<ItemOfReward> temperRewardList = new List<ItemOfReward>();

    public void ShowStar()
    {
        if (!stared)
        {
            stared = true;
            NGUITools.AddChild(starRoot, starTemp);
        }
    }
//    void Start(){
//      
//    }

//    public void ShowReward(ActivityReward reward)
//    {
//        ShowStar();
//        m_rewardData = reward;
//        if (reward != null)
//        {
//            NGUITools.SetActive(giPos, true);
//        }
//        this.ShowAtt(0)
//    }

    /// <summary>
    /// 武者的节日 礼物
    /// </summary>
    /// <param name="reward">Reward.</param>
    public void ShowFestivalReward(object reward)
    {
        ShowStar();
        I_RewardData = reward as ItemOfReward;
        if (reward != null)
        {
            NGUITools.SetActive(giPos, true);
        }
		this.ShiftNameAndIcon(giftIcon,starsPic,_name,I_RewardData.pid);
        this.ShowAtt(I_RewardData.at);
        this.ShowNum(numObj,giftNum,I_RewardData.num);
    }
    void ShowNum(GameObject Obj,UILabel lblNum ,int num){
		if (Obj == null)
            return;
        if (num <= 1)
        {
			Obj.SetActive(false);
        }
        else
        {
			Obj.SetActive(true);
			lblNum.text = num.ToString();
        }
    }

    void ShowAtt(int num){
        if (att == null)
            return;
        if (num != 0)
        {
            att.spriteName = "Attribute_" + num.ToString();
        }
        else
        {
            att.gameObject.SetActive(false);
        }
    }


    public void ShowItemReward(object obj)
    {
        ShowStar();
           HonorItemData thItem = obj as HonorItemData;
        //     honorGiftData = thItem;
        if (obj != null)
        {
            NGUITools.SetActive(giPos, true);
        }
		this.ShiftNameAndIcon(giftIcon,starsPic,_name,thItem.index);

		this.ShowNum(numObj,giftNum,thItem.count);
        //     this.ShowAtt(thItem);
    }

    /// <summary>
    /// Shifts the name and icon.
    /// </summary>
    /// <param name="rewardId">Reward identifier.</param>
    public void ShiftNameAndIcon(UISprite icon,StarsUI stars,UILabel nameLbl,int rewardId)
    {
        string myName = "";
        if (rewardId == 0)
            return;
        switch(DataCore.getDataType(rewardId)){
			case ConfigDataType.Monster:
				myName = Core.Data.monManager.getMonsterByNum (rewardId).name;
				AtlasMgr.mInstance.SetHeadSprite (giftIcon, rewardId.ToString ());
				icon.spriteName = Core.Data.monManager.getMonsterByNum (rewardId).ID.ToString ();
				icon.MakePixelPerfect ();
				stars.SetStar ((int)Core.Data.monManager.getMonsterByNum (rewardId).star);
                break;
            case ConfigDataType.Item:
                myName = Core.Data.itemManager.getItemData(rewardId).name;
                icon.atlas = AtlasMgr.mInstance.itemAtlas;
				icon.spriteName = Core.Data.itemManager.getItemData(rewardId).ID.ToString();
                stars.SetStar((int)Core.Data.itemManager.getItemData(rewardId).star);
                break;
            case ConfigDataType.Equip:
                myName = Core.Data.EquipManager.getEquipConfig(rewardId).name;
                icon.atlas = AtlasMgr.mInstance.equipAtlas;
				icon.spriteName = Core.Data.EquipManager.getEquipConfig(rewardId).ID.ToString();
                stars.SetStar((int)Core.Data.EquipManager.getEquipConfig(rewardId).star);
                break;
            case ConfigDataType.Gems:
                myName = Core.Data.gemsManager.getGemData(rewardId).name;
                icon.atlas = AtlasMgr.mInstance.commonAtlas;
                icon.spriteName = Core.Data.gemsManager.getGemData(rewardId).anime2D;
                stars.SetStar((int)Core.Data.gemsManager.getGemData(rewardId).star);
                break;
        case ConfigDataType.Frag:
            SoulData soul = Core.Data.soulManager.GetSoulConfigByNum (rewardId);

            if (soul != null) {
                myName = soul.name;
                //  icon.atlas = AtlasMgr.mInstance.itemAtlas;
                AtlasMgr.mInstance.SetHeadSprite(icon,soul.updateId.ToString());
              
                stars.SetStar (soul.star);
            } else {
                Debug.LogError (rewardId);
            }
                break;
            default:
                RED.LogError(" not found  : " + rewardId);
                break;
            }
		nameLbl.text = myName;
        icon.MakePixelPerfect ();
    }

    public void ShowDateItem(ItemOfReward[] giftItem,bool isDouble = false)
    {

        ShowStar();
		int giftID = giftItem[0].pid;
        if (isDouble == true) {
            if (panelObj != null && btnGet != null) {
                panelObj.SetActive (true);
                btnGet.gameObject.transform.localPosition = BtnPos;
                this.ShowNum (numObj_D, giftNum_D, (int)(giftItem [0].num / 2));
                this.ShiftNameAndIcon (giftIcon_D, starsPic_D, _name_D, giftID);

                this.ShiftNameAndIcon(giftIcon,starsPic,_name,giftID);
                this.ShowNum(numObj,giftNum,giftItem[0].num/2);
            }
        } else {
            panelObj.SetActive (false);
            btnGet.transform.localPosition =BtnOrgPos ;
            this.ShiftNameAndIcon(giftIcon,starsPic,_name,giftID);
            this.ShowNum(numObj,giftNum,giftItem[0].num);
        }		
    
        ShowAtt (giftItem[0].at);
    }

    public void ShowLevelItem(ItemOfReward[] tR)
    {
        ShowStar();
        if (tR != null)
        {
            NGUITools.SetActive(giPos, true);
        }
   
        ShowGift ();
//        for (int i = 0; i < tR.Length; i++)
//        {
//            yield return new  WaitForSeconds(0.5f);
//            int giftID = tR[i].pid;
//            this.ShiftNameAndIcon(giftID);
//            this.ShowAtt(tR[i].at);
//            this.ShowNum(tR[i].num);
//        }

    }

    void ShowGift(){
        temperRewardList = UILevelRewardController.Instance.temperRewardList;
        if (temperRewardList.Count > 0) {
            ItemOfReward tIOR = temperRewardList [giftIndex - 1];
            if (tIOR != null) {
                //    int giftID = temperRewardList [giftIndex - 1].pid;
                this.ShiftNameAndIcon (giftIcon, starsPic, _name,tIOR.pid);
                //       this.ShiftNameAndIcon (temperRewardList [giftIndex-1].pid);
                this.ShowAtt (temperRewardList [giftIndex - 1].at);
                this.ShowNum (numObj, giftNum, tIOR.num);
            }
            OnClose += ShowGift;
            UILevelRewardController.Instance.temperRewardList.Remove (tIOR);
        } else {
            OnClose = null;
        }
    }

    void OnColse()
    {
        if (OnClose != null)
        {
            ShowGift ();
            // OnClose();
            return;
        }

        Destroy(gameObject);
    
    }
}




