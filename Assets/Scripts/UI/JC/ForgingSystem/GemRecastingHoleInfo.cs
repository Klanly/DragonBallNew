using UnityEngine;
using System.Collections;

public class GemRecastingHoleInfo : MonoBehaviour {

	public UISprite  Spr_HoleColor;
	public UISprite  Spr_HoleGem;
	public UISprite  Spr_Lock;
	public GameObject Effect;
	private bool islock=false;
	public UILabel Lab_Prompt;
    public UILabel lab_lv;
	
	public void SetHoleColor(short color)
	{
		Spr_HoleColor.spriteName="solt"+color.ToString();
	}
	
	public void SetGem(string GemSpriteName)
	{
		if(GemSpriteName==null)
		{
			Spr_HoleGem.enabled=false;
			isHaveGem=false;
			if(Effect != null && !Effect.activeSelf)Effect.SetActive(true);
//			Debug.Log(gameObject.name+"  ->  "+isHaveGem.ToString());
		}
		else
		{
			if(!Spr_HoleGem.enabled)Spr_HoleGem.enabled=true;
			Spr_HoleGem.spriteName=GemSpriteName;
			Spr_HoleGem.MakePixelPerfect();
			if(Effect != null && Effect.activeSelf)Effect.SetActive(false);			
			isHaveGem=true;
		}
	}

    public void SetLv(int lv ){
        if (lab_lv != null)
        {
            if (lv > 0)
            {
                lab_lv.gameObject.SetActive(true);
                lab_lv.text = "Lv" + lv;
            }
            else
                lab_lv.gameObject.SetActive(false);
        }
    }
	
	public bool isHaveGem{get;set;}
	
	public void AutoLockOrUnLock()
	{
		if(!islock)
		{
			if(FrogingSystem.ForgingRoomUI.Instance.RecastingSystem.IsSoltsWillAllLock)
			{
				SQYAlertViewMove.CreateAlertViewMove ("不能全部锁定!");
			}
			else
			{
				islock=!islock;
			    Spr_Lock.gameObject.SetActive(islock);
				if(Lab_Prompt != null)
				{
					Lab_Prompt.text =islock ? Core.Data.stringManager.getString(9074):Core.Data.stringManager.getString(9073);
				}
                FrogingSystem.ForgingRoomUI.Instance.RecastingSystem.LockSlotCount++;
			}
		}
		else
		{
			islock=!islock;
			Spr_Lock.gameObject.SetActive(islock);
			if(Lab_Prompt != null)
			{
				Lab_Prompt.text =islock ? Core.Data.stringManager.getString(9074):Core.Data.stringManager.getString(9073);
			}
			FrogingSystem.ForgingRoomUI.Instance.RecastingSystem.LockSlotCount--;
		}
		 FrogingSystem.ForgingRoomUI.Instance.RecastingSystem.view.SetNeedStone((FrogingSystem.ForgingRoomUI.Instance.RecastingSystem.LockSlotCount+1)*10);
	}
	
	public bool isLock
	{
		get
		{
			return islock;
		}
	}
	
	//设置未知孔
	public void SetSlotUnkown()
	{
		Spr_HoleColor.color = new Color(0,0,0,1f);
	}
	
}
