using UnityEngine;
using System.Collections;



public class JC_TouchRoll : MonoBehaviour {

    private Vector3 Touch_O;
    private Vector3 Touch_Old;

    //回退的时间
    private float BACKTIME = 0.3f;

    public MiniItween.EasingType type;
    //  --- 旋转范围 ---
	//[HideInInspector]
	private float MIN_Z = -3.2f;
	//[HideInInspector]
	private float MIN_NEAR_Z = -5.3f;
	//[HideInInspector]
	private float MAX_Z = -20.6f;
	//[HideInInspector]
	private float MAX_NEAR_Z = -17.5f;


    //记录是否UICamera来控制，而不是3DCamera来控制
    private bool hitted;
    //记录上一次手机触摸位置判断用户是在左放大还是缩小手势   
    private Vector2 oldPosition1;   
    private Vector2 oldPosition2;

    //private float LastRotateZ = 0;
	
	private Camera uiCamera;
	
	public static System.Action<AllenTouchState> TouchState ;
	
	
	public static bool isMoving{get;set;}
	[HideInInspector]
	public Vector3 OldPos;
	
	
	static JC_TouchRoll _this = null;
	void Awake()
	{
		isMoving = false;
		if(uiCamera == null)
		uiCamera = NGUITools.FindCameraForLayer( GameObject.Find("UI Root_bottom").layer );
		OldPos = transform.localRotation.eulerAngles;
		_this = this;
	}
	
	public static JC_TouchRoll Instance
	{
		get
		{
			return _this;
		}
	}
	
    void Update ()  
	{		
		if(!Core.Data.temper.TempTouch)return;		

		if(transform.localPosition ==OldPos )
			isMoving = false;
		else
			isMoving = true;
		OldPos = transform.localPosition;

        SingleTouch();
		
		ContinueMove();
	}

	private void ContinueMove()
	{
		if(isTouchEnd)
		{
			if(Speed.x > 0.01f)
			{
//				float targetX = transform.localRotation.eulerAngles.x;
//                float targetZ = transform.localRotation.eulerAngles.z;
				Speed *= 0.99f;
				
				float tempSpeedX = - Speed.x * MoveDir.x;
				//float tempSpeedY = Speed.y * MoveDir.y;
				//targetZ += tempSpeedX;

				Vector3 movepos =  transform.localPosition;
				movepos.x += tempSpeedX;

				if(movepos.x > MIN_Z)
					movepos.x = MIN_Z;
				else if(movepos.x < MAX_Z)
					movepos.x = MAX_Z;

				transform.localPosition = movepos;

				//transform.rotation = Quaternion.Euler(new Vector3(targetX, 0, targetZ ));

//	            if(targetZ < MIN_Z)  {targetZ = MIN_Z;FinishContinue();}
//	            if(targetZ > MAX_Z) {targetZ = MAX_Z;FinishContinue();}
				if(SwipToMin())
				{
					FinishContinue();
				}
			}
			else
			{
				FinishContinue();
			}
		}
	}
	
	

	private void FinishContinue()
	{
		isTouchEnd = false;
		Speed = Vector2.zero;
	}
	
    bool isTouchEnd = false;
	int TouchFrameCount = 0;
	Vector2 Distance;
	Vector2 Speed;

	Vector2 MoveDir ;
	
    private void SingleTouch() 
	{
        if (Input.GetMouseButtonDown(0))
        {
			isTouchEnd = false;
            RaycastHit hit;
            hitted = UICamera.Raycast(Input.mousePosition);
            if(hitted)
                return;

			if(TouchState!=null)TouchState(AllenTouchState.Donw);

            MiniItween.DeleteType(gameObject, MiniItween.Type.Rotate);
            Touch_O = Input.mousePosition;
			Touch_Old = GetWorldPos(Touch_O);
        } 
		else if (Input.GetMouseButton(0))
		{	
            if(hitted)
                return;

			if(TouchState!=null)TouchState(AllenTouchState.Continue);

            MiniItween.DeleteType(gameObject, MiniItween.Type.Rotate);
            Vector3 NewPos = Input.mousePosition;
			NewPos = GetWorldPos(NewPos);
			
			
			float differenceX = NewPos.x - Touch_Old.x;

//            float targetX = transform.localRotation.eulerAngles.x;
//            float targetZ = transform.localRotation.eulerAngles.z;
			Vector3 movepos =  transform.localPosition;

			movepos.x +=  differenceX * 6.0f ;


			TouchFrameCount++;
			Distance.x += Mathf.Abs( differenceX );

			
			MoveDir.x = differenceX>0 ? -1:1;

			if(movepos.x > MIN_Z)
				movepos.x = MIN_Z;
			else if(movepos.x < MAX_Z)
				movepos.x = MAX_Z;
			//Debug.LogError(movepos.x.ToString());
            //transform.rotation = Quaternion.Euler(new Vector3(targetX, 0, targetZ ));
			transform.localPosition = movepos;

            Touch_Old = NewPos;
        } 
		else if(Input.GetMouseButtonUp(0))
		{
			if(TouchState!=null)TouchState(AllenTouchState.Up);
			if(TouchFrameCount>0)
			{
				Speed.x = Distance.x / (float) TouchFrameCount;
			}
			else
				Speed = Vector2.zero;
			
			TouchFrameCount = 0;
			Distance = Vector2.zero;

			isTouchEnd = true;

        }
    }

	private Vector3 GetWorldPos(Vector3 SceenPos)
	{
			SceenPos.x = Mathf.Clamp01(SceenPos.x / Screen.width);
			SceenPos.y = Mathf.Clamp01(SceenPos.y / Screen.height);
			SceenPos = uiCamera.ViewportToWorldPoint(SceenPos);
		    return SceenPos;
	}

    //如果多出来的区域，要滑动回去
    private bool SwipToMin() {

		Vector3 movepos =  transform.localPosition;		
		//movepos.x +=  differenceX * 6.0f ;
        //Vector3 curAngle = transform.rotation.eulerAngles;

		bool result=false;

		if(movepos.x < MAX_NEAR_Z)
		{
            //MiniItween.RotateTo(gameObject, new Vector3(curAngle.x, 0, MAX_NEAR_Z), BACKTIME);
			Vector3 pos = movepos;
			pos.x = MAX_NEAR_Z;
			MiniItween.MoveTo(gameObject,pos,BACKTIME);
			result = true;
        }
		else if(movepos.x > MIN_NEAR_Z) 
		{
            //MiniItween.RotateTo(gameObject, new Vector3(curAngle.x, 0, MIN_NEAR_Z), BACKTIME);
			Vector3 pos = movepos;
			pos.x = MIN_NEAR_Z;
			MiniItween.MoveTo(gameObject,pos,BACKTIME);
			result = true;
        }
		return result;
    }

}
