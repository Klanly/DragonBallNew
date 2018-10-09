using UnityEngine;
using System.Collections;

public class UIMiniPlayerController : SQYPlayerController
{
    public GameObject[] Array_GameObject;
	
	private static UIMiniPlayerController instance = null;
	
	public UIGrid Root;
	
	static public UIMiniPlayerController Instance
	{
		get
		{
			if (instance == null)
			{
				instance = UnityEngine.Object.FindObjectOfType(typeof(UIMiniPlayerController)) as UIMiniPlayerController;

				if (instance == null)
				{
					GameObject miniPlayerControllerPrb = PrefabLoader.loadFromPack("WHY/pbMiniPlayerController") as GameObject;
					GameObject miniPlayerControllerObj = GameObject.Instantiate(miniPlayerControllerPrb) as GameObject;

					RED.AddChild(miniPlayerControllerObj,DBUIController.mDBUIInstance._TopRoot);

					instance = miniPlayerControllerObj.GetComponent<UIMiniPlayerController>();
				}
			}
			return instance;
		}
	}


	public void DestroySelf()
	{
		Destroy(gameObject);
	}
	
	
	
	public static bool[] ElementShowArray = new bool[]{true,true,true,true};	
	
    public  void OnEnable()
	{
		#region Change By JC
		 transform.localPosition = new Vector3(0,362f,0);	
		 MiniItween.MoveTo(gameObject,new Vector3(0, transform.localPosition.y - 60, 0),0.5f,MiniItween.EasingType.EaseInOutBack);

		int showCount = 0;
        if(ElementShowArray.Length == 4)
		{
			for(int i=0 ;i<ElementShowArray.Length;i++)
			{
			     if(ElementShowArray[i]) showCount++;
				 Array_GameObject[i].SetActive(ElementShowArray[i]);
			}
			
			Root.cellWidth = 5f/(float)showCount * 215f;
			if(showCount != 5)Root.cellWidth*=0.9f;
			
			float x = - ((showCount-1 )* Root.cellWidth+180)/2f;
			Root.transform.localPosition = new Vector3(x,0,0);	
			
		}
		Root.repositionNow = true;		
		#endregion
    }
        
    void OnDisable()
	{
        gameObject.transform.localPosition = new Vector3(0,362,0);
		ElementShowArray = new bool[]{true,true,true,true};
    }

    public void StretchMini(){
        MiniItween.MoveTo(gameObject, new Vector3(0, instance.transform.localPosition.y + 60, 0), 0.2f, MiniItween.EasingType.EaseInOutBack).myDelegateFunc += BackMini;
    }
    void BackMini(){
        MiniItween.MoveTo(gameObject, new Vector3(0, instance.transform.localPosition.y - 60, 0), 0.5f, MiniItween.EasingType.EaseInOutBack);
    }

    public void HideFunc(){
        gameObject.SetActive(false);
        gameObject.transform.localPosition = new Vector3(0,362,0);
        gameObject.SetActive(true);
    }
		
	void RechargeStoneOnClick()
	{
        DBUIController.mDBUIInstance._playerViewCtl.OnBtnAddStone();
	}

	void RechargeCoinOnClick()
	{
        DBUIController.mDBUIInstance._playerViewCtl.OnBtnAddCoin();
	}
        
    void RechargePowerOnClick(){
        DBUIController.mDBUIInstance._playerViewCtl.OnBtnAddPower();
    }

    void RechargeEnergyOnClick(){
        DBUIController.mDBUIInstance._playerViewCtl.OnBtnAddEnergy();
    }


	public override void SetActive(bool bShow)
	{
		RED.SetActive (bShow, this.gameObject);
	}

}
