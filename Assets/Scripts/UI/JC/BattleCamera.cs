using UnityEngine;
using System.Collections;

public class BattleCamera : MonoBehaviour {

	public UICamera Camera1;
	public static BattleCamera Instance;
	void Awake()
	{
		Instance = this;
	}
	
	void Start ()
	{
		UITopAndBottomTouch = Core.Data.temper.TempTouch;
	}
	
	//是否关闭Top层和Bottom层点击
	public bool UITopAndBottomTouch
	{
		set
		{
			if(!value)
			{
                Camera1.eventReceiverMask = 1 << LayerMask.NameToLayer("NoTouch");
			}
		}
	}

	public void dealloc() {
		Instance = null;
		Destroy(gameObject);
	}

}
