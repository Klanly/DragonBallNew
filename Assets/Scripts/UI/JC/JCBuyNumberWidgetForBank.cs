using UnityEngine;
using System.Collections;

public class JCBuyNumberWidgetForBank : MonoBehaviour {

	public UISlider uislider;
	public UILabel  Lab_Number;
	public UILabel _Stone;
	public UILabel _Name;
	public UISprite _Icon;

	public int _unitprice;
	float _EveryMultiple;
	int _Maxnum = 0;
	int _buyNumber = 0;
	bool pressThumb = false;

	public int NumIndex 
	{
		set
		{
			_buyNumber = value;
			_Stone.text = (_buyNumber*_unitprice).ToString();
			Lab_Number.text = _buyNumber.ToString();
			uislider.value = (_buyNumber-1)*_EveryMultiple;
		}
		get
		{
			Lab_Number.text = _buyNumber.ToString();
			return _buyNumber;
		}   
	}

	public int thisValue
	{
		get
		{
			return NumIndex ;
		}
	}
    [HideInInspector]
	public int Maxnum
	{
		set{
			_Maxnum = value;
			if (_Maxnum <= 1)
				_EveryMultiple = 1f;
			else 
				_EveryMultiple = 1f/(float)(_Maxnum-1);
		}
		get{
			return _Maxnum;
		}
	}

	void Start ()
	{
		uislider.onChange.Add(new EventDelegate(OnSliderChange));	
		_buyNumber = 0;
	}


	void OnSliderChange() 
	{
		if (pressThumb) {
			if (_EveryMultiple <= 0)
				NumIndex = 0;
			else {
				NumIndex = uislider.value == 0 ? 0: (int)(uislider.value / _EveryMultiple) + 1 ;
				Debug.Log ( " value = " +uislider.value  +" / every = " + _EveryMultiple + "    =====  " + uislider.value / _EveryMultiple   +  "NumIndex" +NumIndex );
			}
		}
		if (NumIndex == Maxnum) {
			uislider.value = 1;
		} else {
			uislider.value = (float)NumIndex / (float)Maxnum;
		}
		Debug.Log ( "numindex  = " + NumIndex); 
	}

	void OnClick(GameObject Btn){
		switch(Btn.name)
		{
		case "-":
			Debug.Log ( "  -  num index "+ NumIndex);
			if(NumIndex>=1)
				NumIndex--;
			break;
		case "+":
			if (NumIndex < Maxnum) {
				NumIndex++;
				Debug.Log (" in here ");
			}
			break;
		}
		Debug.Log ( "   num index "+ NumIndex +"  max num "+ Maxnum);
		if (NumIndex == Maxnum) {
			uislider.value = 1;
		} else {
			uislider.value = (float)NumIndex / (float)Maxnum;
		}

	}

	void OnPress (GameObject Btn)
	{		
		if(Btn.name == "Thumb")
		{
			pressThumb = true;
		}


	}

	void OnRelease(GameObject Btn){
		if(Btn.name == "Thumb")
		{
			pressThumb = false;
		}

	}




	

}
