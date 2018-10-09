using UnityEngine;
using System.Collections;

public class JCBuyNumberWidget : MonoBehaviour {

	public UISlider uislider;
	public UILabel  Lab_Number;
	public UILabel _Stone;
	public UILabel _Name;
	public UISprite _Icon;

	float _EveryMultiple;
	int _Maxnum;
    [HideInInspector]
	public int Maxnum
	{
		set{
			_Maxnum = value;
			_EveryMultiple = 1f/(float)(_Maxnum-1);
		}
		get{
			return _Maxnum;
		}
	}

	public bool StoneOrCoin;
	public int _unitprice;


	void Start ()
	{
		uislider.onChange.Add( new EventDelegate(OnSliderChange));	
		NumIndex = 1;
	}
	
	void OnSliderChange() 
	{
		if(!SliderOrBtn)
		{

			NumIndex = (int)(uislider.value/_EveryMultiple + 0.5f)+1;
		}

	}

	bool OnPressOrRelease = false;	
	bool SliderOrBtn = false;    //true slider    false  btn
	bool IsSlider = false;
	
	float PressTime = 0;
	
	//true:add      false:reduction
	bool AddOrReduction = true;
	
	int _buyNumber = 1;
	public int NumIndex 
	{
	   set
		{
			_buyNumber = value;
			_Stone.text = (_buyNumber*_unitprice).ToString();
			Lab_Number.text = _buyNumber.ToString();
			if(!IsSlider)uislider.value = (_buyNumber-1)*_EveryMultiple;

        }
		get
		{
			return _buyNumber;
		}   
	}
	
	public int value
	{
		get
		{
			return NumIndex;
		}
	}
	
	void Update ()
	{
	     if(OnPressOrRelease)
		{
			PressTime += Time.deltaTime;
			
			if(PressTime > 0.3f)
			{		   
				if(AddOrReduction)
				{
					if(NumIndex<Maxnum)
						NumIndex++;
				}
				else
				{
					if(NumIndex>1)
						NumIndex--;
				}

			}
		}
	}
	
	
	void OnPress (GameObject Btn)
	{		
//		Debug.Log("OnPress");
		PressTime = 0;
		if(Btn.name != "Thumb")
		{
			OnPressOrRelease = true;
			SliderOrBtn = true;
		}

		switch(Btn.name)
		{
		case "-":
			AddOrReduction = false;
			break;
		case "+":
			AddOrReduction = true;
			break;
		case "Thumb":
			IsSlider = true;
			break;
		}
	}
	
	void OnRelease(GameObject Btn)
	{
//		Debug.Log("OnRelease");
		if(Btn.name != "Thumb")OnPressOrRelease = false;
		PressTime = 0;

		switch(Btn.name)
		{
		case "-":
			AddOrReduction = false;
			break;
		case "+":
			AddOrReduction = true;
			break;
		case "Thumb":
				IsSlider = false;
				break;
		}
		if(Btn.name != "Thumb")
		{
			if(AddOrReduction)
			{
				if(NumIndex<Maxnum)
					NumIndex++;
			}
			else
			{
				if(NumIndex>1)
					NumIndex--;
			}
			SliderOrBtn = false;
		}

		
	
	}
	

}
