using UnityEngine;
using System;
using System.Collections;

public class AoYiSlot : MonoBehaviour
{
	public enum LockType
	{
        Lock,//上锁
		NoLock,
        NoBuy,//还没买
		None
	}

	public UIAtlas aoYiAtlas;

	public UIAtlas activeAtlas;

	public LockType lockType = LockType.None;

	public UISprite aoYiIcon;

	public UILabel aoYiName;

	public UILabel info;

	public GameObject LevelRoot;
	public UILabel expLabel;
	public UILabel levelLabel;
	public UISlider levelSlider;

	public GameObject lockRoot;
	public GameObject stateRoot;
	public UILabel stateInfo;
	public UISprite stateIcon;

	public GameObject priceRoot;
	public UISprite currencyiIcon;
	public UILabel priceLabel;

	public DragonLockData dragonLockData;

	public GameObject selectedBackground;

	public GameObject background;

	public Action<AoYiSlot> SelectedDelegate;

	public AoYi aoYi;


	public GameObject lockIconObj;

	public UISpriteAnimation lightningSpriteAnimation;
	public UISpriteAnimation noLockExplodeSpriteAnimation;
    public UISprite aoyiSide;

    public enum ParentType
    {
        isNormal,
        isLearnAoYi,
        isEquipAoYi,
    }

    public ParentType curAoYiType;

    public short AoyiType {
        set{ 
            if (aoYi != null) {
                value = aoYi.AoYiDataConfig.dragonType;  
            }
        }
        get{ 
            if (aoYi != null)
                return aoYi.AoYiDataConfig.dragonType;
            else
                return 0;
        }
    }

//	public bool isAnimation = false;

    private const int MAX_LEVEL_AOYI = 10;

    void Start(){
//        if (curAoYiType != ParentType.isNormal)
//        {
//            gameObject.GetComponent<DragAndDropInDragonBall>().enabled = false;
//        }
//        else
//        {
//            gameObject.GetComponent<DragAndDropInDragonBall>().enabled = true;
//        }
        if (AoyiType == 1) {
            aoyiSide.spriteName = "dragon-1020";
        } else {
            aoyiSide.spriteName = "dragon-1021";
        }
    }

	public void unLock()
	{
		aoYiIcon.gameObject.SetActive(false);
		LevelRoot.SetActive(false);
		lockRoot.SetActive(false);
		priceRoot.SetActive(false);
		info.text = "";
        aoyiSide.gameObject.SetActive (false);
	}

	public void setAoYiSlotNoLockNoAoYi()
	{
		this.lockType = AoYiSlot.LockType.NoLock;
		this.aoYiIcon.gameObject.SetActive(true);
		this.aoYiIcon.atlas = this.activeAtlas;
		this.aoYiIcon.spriteName = "EUMO-07";
		//this.aoYiIcon.MakePixelPerfect();
        aoyiSide.gameObject.SetActive (false);
	}

	public void setAoYi()
	{
		this.aoYiIcon.spriteName = aoYi.AoYiDataConfig.ID.ToString();
		//this.aoYiIcon.MakePixelPerfect();

		this.levelLabel.text = "LV" + aoYi.RTAoYi.lv.ToString();
        float res = 0;
        if (aoYi.RTAoYi.lv == MAX_LEVEL_AOYI) {
            this.expLabel.text = "MAX";
            res = 1f;
        }
        else {
            this.expLabel.text = aoYi.RTAoYi.ep * 50 + "/" + ((aoYi.AoYiDataConfig.exp [aoYi.RTAoYi.lv]) * 50);
            res = ((float)aoYi.RTAoYi.ep) / ((float)aoYi.AoYiDataConfig.exp[aoYi.RTAoYi.lv]);
        }
        if (aoYi != null) {
            if (aoyiSide != null) {
                aoyiSide.gameObject.SetActive (true);
                if (aoYi.AoYiDataConfig.dragonType == 1) {//地球
                    aoyiSide.spriteName = "dragon-1020";
                } else if(aoYi.AoYiDataConfig.dragonType == 2) {// nmkx
                    aoyiSide.spriteName = "dragon-1021";
                }
            } else {
                aoyiSide.gameObject.SetActive (false);
            }
        }
       
        this.levelSlider.value = res;

        if(aoYi.RTAoYi.ep == 0) {
            this.levelSlider.foregroundWidget.gameObject.SetActive(false);
        } else {
            this.levelSlider.foregroundWidget.gameObject.SetActive(true);
        }
            
		this.aoYiName.text = aoYi.AoYiDataConfig.name;
	}

	public void setUnLock(bool isAnimation)
	{
		this.unLock();
		this.aoYiIcon.gameObject.SetActive(true);
		this.LevelRoot.SetActive(true);

		this.aoYiIcon.atlas = aoYiAtlas;

		this.lockType = AoYiSlot.LockType.NoLock;
        aoyiSide.gameObject.SetActive (false);
		aoYiIconAnimation(isAnimation);

	}
    //add by wxl 
    public void SetVoidPos(){
        this.lockType = AoYiSlot.LockType.NoLock;
        aoYiIcon.gameObject.SetActive (true);
        this.aoYiIcon.gameObject.SetActive(true);
        this.aoYiIcon.atlas = this.activeAtlas;
        this.aoYiIcon.spriteName = "EUMO-07";
        this.LevelRoot.SetActive (false);
        this.aoyiSide.gameObject.SetActive (false);
        this.aoYiName.text = "";

    }

	public void setInfo(string infoStr, string currencyiIconName, string price)
	{
		info.text = infoStr;
		currencyiIcon.spriteName = currencyiIconName;
		priceLabel.text = price;
	}

	public IEnumerator showSelectedBackground(bool isClose = true)
	{
		selectedBackground.SetActive(true);

		if(isClose)
		{
			yield return new WaitForSeconds(0.5f);
			selectedBackground.SetActive(false);
		}
	}

	public void OnClicked()
	{
		if(this.SelectedDelegate != null)
		{
			SelectedDelegate(this);
		}
	}

	#region 动画
	public void aoYiIconAnimation(bool isAnimation)
	{
		if(isAnimation)
		{
			this.aoYiIcon.spriteName = aoYi.AoYiDataConfig.ID.ToString();
			this.aoYiIcon.transform.localScale = new Vector3(3, 3, 1);

			MiniItween m = MiniItween.ScaleTo(this.aoYiIcon.gameObject, new Vector3(1, 1, 1), 0.1f);
			m.myDelegateFuncWithObject += aoYiIconAnimationComplete;
			MiniItween.ColorFromTo(this.aoYiIcon.gameObject, new V4(1,1,1,0), new V4(1,1,1,1), 0.1f, MiniItween.EasingType.Normal, MiniItween.Type.ColorWidget);
		}
		else
		{
			setAoYi();
		}
	}

	public void aoYiIconAnimationComplete(GameObject g)
	{
		MiniItween m = g.GetComponent<MiniItween>();
		m.myDelegateFuncWithObject -= aoYiIconAnimationComplete;

		setAoYi();

		MiniItween.Shake(this.background, new Vector3(0,2.0f,0), 0.2f, MiniItween.EasingType.Normal, false);
	}

	public void lockIconAnimation(bool isAnimation)
	{
		if(isAnimation)
		{
			this.lightningSpriteAnimation.gameObject.SetActive(true);
			this.lockIconObj.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 10));
			TweenRotation tr = TweenRotation.Begin(this.lockIconObj,  0.05f, Quaternion.Euler(new Vector3(0, 0, -10)));
			tr.style = UITweener.Style.PingPong;
			tr.method = UITweener.Method.EaseInOut;
			StartCoroutine(lockIconAnimationComplete(tr));
		}
		else
        {
			this.unLock();
			this.setAoYiSlotNoLockNoAoYi();
		}
	}

	public IEnumerator lockIconAnimationComplete(TweenRotation tr)
	{
		yield return new WaitForSeconds(0.5f);
		Destroy(tr);

		MiniItween mi = MiniItween.ColorFromTo(this.lockIconObj, new V4(1.0f, 1.0f, 1.0f, 1f), new V4(1.0f, 1.0f, 1.0f, 0f), 0.1f, MiniItween.EasingType.Normal, MiniItween.Type.ColorWidget);
		mi.myDelegateFuncWithObject += lockIconObjScaleToComplete;
//			MiniItween.ColorFromTo(g2, new V4(1.0f, 1.0f, 1.0f, 0f), new V4(1.0f, 1.0f, 1.0f, 1.0f), 0.3f, MiniItween.EasingType.Normal, MiniItween.Type.ColorWidget);

		MiniItween.ScaleTo(this.lockIconObj, new Vector3(1.5f, 1.5f, 1), 0.05f);
	}

	void lockIconObjScaleToComplete(GameObject g)
	{
		MiniItween m = g.GetComponent<MiniItween>();
		m.myDelegateFuncWithObject -= lockIconObjScaleToComplete;

		this.lockIconObj.SetActive(false);

		this.lightningSpriteAnimation.gameObject.SetActive(false);
		this.noLockExplodeSpriteAnimation.gameObject.SetActive(true);
		this.noLockExplodeSpriteAnimation.AnimationEndDelegate = noLockExplodeSpriteAnimationEnd;
	}

	public void noLockExplodeSpriteAnimationEnd(UISpriteAnimation spriteAnimation)
	{
		this.unLock();
		this.noLockExplodeSpriteAnimation.AnimationEndDelegate = null;
		this.noLockExplodeSpriteAnimation.gameObject.SetActive(false);
	
		this.setAoYiSlotNoLockNoAoYi();

		UIShenLongManager.Instance.dragonAltar.sortAoYiSlot();
	}
	#endregion

}
