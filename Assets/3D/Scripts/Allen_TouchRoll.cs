using UnityEngine;
using System.Collections;


public enum AllenTouchState
{
	Up,
	Donw,
	Continue,
}

public class Allen_TouchRoll : MonoBehaviour {

    private Vector3 Touch_O;
    private Vector3 Touch_Old;
    //private Vector3 NowRotation;

    public float RotSheepScale_Z = 0.01f;
    public float RotSheepSclae_X = 0.01f;

    //回退的时间
    private float BACKTIME = 0.3f;

    public MiniItween.EasingType type;
    //  --- 旋转范围 ---
    public float MIN_X = 19.1f;
    public float MAX_X = 33.6f;
    public float MIN_Z = 17.3f;
    public float MAX_Z = 42.5f;

    public float MAX_NEAR_Z = 39.4f;
	public float MIN_NEAR_Z = 22.5f;

    public float MAX_NEAR_X = 30f;
    public float MIN_NEAR_X = 21.5f;

    //多点触摸的缩放比例
    public float Multi_Zoom = 1f;
    //记录是否UICamera来控制，而不是3DCamera来控制
    private bool hitted;
    //记录上一次手机触摸位置判断用户是在左放大还是缩小手势   
    private Vector2 oldPosition1;   
    private Vector2 oldPosition2;

    private float LastRotateZ = 0;
	
	private Camera uiCamera;
	
	public static System.Action<AllenTouchState> TouchState ;
	
	
	public static bool isMoving{get;set;}
	public Vector3 OldPos;
	
	
	static Allen_TouchRoll _this = null;
	void Awake()
	{
		isMoving = false;
		if(uiCamera == null)
		uiCamera = NGUITools.FindCameraForLayer( GameObject.Find("UI Root_bottom").layer );
		OldPos = transform.localRotation.eulerAngles;
		_this = this;
	}
	
	public static Allen_TouchRoll Instance
	{
		get
		{
			return _this;
		}
	}
	
	//Vector3 last=new Vector3();
    void Update ()  
	{
		
		if(!Core.Data.temper.TempTouch)return;
		
		if(transform.localRotation.eulerAngles==OldPos )
			isMoving = false;
		else
			isMoving = true;
		OldPos = transform.localRotation.eulerAngles;
		
		
        if(Core.SM.rtPlat == RuntimePlatform.OSXEditor || Core.SM.rtPlat == RuntimePlatform.WindowsEditor)
            SingleTouch();
        else if(Core.SM.rtPlat == RuntimePlatform.IPhonePlayer || Core.SM.rtPlat == RuntimePlatform.Android || Core.SM.rtPlat == RuntimePlatform.WP8Player) {
            if(Input.touchCount == 1) 
			{
                SingleTouch();
            } 
			else if(Input.touchCount == 2) 
			{
                MultiTouch();
            }
        }
    
        //if(Auto) AutoSwip();
		ContinueMove();
		
		
	}

	private void ContinueMove()
	{
		if(isTouchEnd)
		{
			if(Speed.x > 0.01f || Speed.y > 0.01f)
			{
				float targetX = transform.localRotation.eulerAngles.x;
                float targetZ = transform.localRotation.eulerAngles.z;
				Speed *= 0.99f;
				
				float tempSpeedX = Speed.x * MoveDir.x;
				float tempSpeedY = Speed.y * MoveDir.y;
				targetZ += tempSpeedX;
				targetX += tempSpeedY;
				
				transform.rotation = Quaternion.Euler(new Vector3(targetX, 0, targetZ ));
				//Debug.Log("tempSpeedX="+tempSpeedX.ToString()+"   tempSpeedY="+tempSpeedY.ToString());
				
			    if(targetX < MIN_X) {targetX = MIN_X;FinishContinue();}
	            if(targetX > MAX_X) {targetX = MAX_X;FinishContinue();}
	            if(targetZ < MIN_Z)  {targetZ = MIN_Z;FinishContinue();}
	            if(targetZ > MAX_Z) {targetZ = MAX_Z;FinishContinue();}
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
	
	
	/*
	 * 	Vector3 ResetPos1 = new Vector3(28.07034f,0,20.4f);
	Vector3 ResetPos2 = new Vector3(25.13108f,0,30.87951f);
	Vector3 ResetPos3 =new Vector3(30.11301f,0,41.37781f);
	Vector3 ResetPos4 = new Vector3(30f,0,30f);
	 * */
	private void FinishContinue()
	{
		isTouchEnd = false;
		Speed = Vector2.zero;

//		bool result = SwipToMin();
//		if(!result)
//		{
//			 Vector3 curAngle = transform.rotation.eulerAngles;
//			float a = Vector3.Distance(curAngle,ResetPos1);
//			float b = Vector3.Distance(curAngle,ResetPos2);
//			float c = Vector3.Distance(curAngle,ResetPos3);
//			float d = Vector3.Distance(curAngle,ResetPos4);
//			float e = Mathf.Min(a,b,c,d);
//			if(e == a)
//				MiniItween.RotateTo(gameObject, ResetPos1, BACKTIME);
//			else if(e == b)
//				MiniItween.RotateTo(gameObject, ResetPos2, BACKTIME);
//			else if(e == c)
//				MiniItween.RotateTo(gameObject, ResetPos3, BACKTIME);
//			else if(e == d)
//				MiniItween.RotateTo(gameObject, ResetPos4, BACKTIME);
//		}
	}
	
    bool isTouchEnd = false;
	int TouchFrameCount = 0;
	Vector2 Distance;
	Vector2 Speed;
	/*手指移动的方向 -1->右或下  1->左或上
	 * */
	Vector2 MoveDir ;
	
    private void SingleTouch() 
	{
        //single touch
        if (Input.GetMouseButtonDown(0))
        {
			isTouchEnd = false;
            //RaycastHit hit;
            hitted = UICamera.Raycast(Input.mousePosition);
            if(hitted)
                return;

			if(TouchState!=null)TouchState(AllenTouchState.Donw);

            clearAutoStatus();
            MiniItween.DeleteType(gameObject, MiniItween.Type.Rotate);
            Touch_O = Input.mousePosition;
            //NowRotation = transform.rotation.eulerAngles;
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
			float differenceY = NewPos.y - Touch_Old.y;

            float targetX = transform.localRotation.eulerAngles.x;
            float targetZ = transform.localRotation.eulerAngles.z;
			
			
			targetZ +=  -differenceX * 6.0f ;
			targetX +=  differenceY * 6.0f ;
			
			//if(differenceX == 0) return;
			
			TouchFrameCount++;
			Distance.x += Mathf.Abs( differenceX );
			Distance.y += Mathf.Abs( differenceY );
			
			MoveDir.x = differenceX>0 ? -1:1;
			MoveDir.y = differenceY>0 ? 1:-1;
			
            if(targetX < MIN_X) targetX = MIN_X;
            if(targetX > MAX_X) targetX = MAX_X;
            if(targetZ < MIN_Z) targetZ = MIN_Z;
            if(targetZ > MAX_Z) targetZ = MAX_Z;

            transform.rotation = Quaternion.Euler(new Vector3(targetX, 0, targetZ ));

            Touch_Old = NewPos;
        } 
		else if(Input.GetMouseButtonUp(0))
		{
			if(TouchState!=null)TouchState(AllenTouchState.Up);
			if(TouchFrameCount>0)
			{
				Speed.x = Distance.x / (float) TouchFrameCount;
				Speed.y = Distance.y / (float) TouchFrameCount;
				//isTouchEnd = true;
			}
			else
				Speed = Vector2.zero;
			
			TouchFrameCount = 0;
			Distance = Vector2.zero;
//            hitted = false;	
//            Auto = true;
			
//            if( SwipToMin() )
//			{
//				NGUIDebug.Log(" I CAN MOVE IT!!!!");
				isTouchEnd = true;
			//}
        }
    }

	private Vector3 GetWorldPos(Vector3 SceenPos)
	{
			SceenPos.x = Mathf.Clamp01(SceenPos.x / Screen.width);
			SceenPos.y = Mathf.Clamp01(SceenPos.y / Screen.height);
			SceenPos = uiCamera.ViewportToWorldPoint(SceenPos);
		    return SceenPos;
	}
	
	
    private void MultiTouch() {
        //multi - touches
        if(Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began ) {

            //RaycastHit hit;
            hitted = UICamera.Raycast(Input.mousePosition);
            if(hitted)
                return;

			if(TouchState!=null)TouchState(AllenTouchState.Donw);
        }

        //前两只手指触摸类型都为移动触摸   
        if(Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved) {

            if(hitted)
                return;

			if(TouchState!=null)TouchState(AllenTouchState.Continue);

            Vector3 curRotation = transform.rotation.eulerAngles;

            //计算出当前两点触摸点的位置   
            var curPos1 = Input.GetTouch(0).position;   
            var curPos2 = Input.GetTouch(1).position;   

            float targetX= 0f;
            //函数返回真为放大，返回假为缩小   
            if(isEnlarge(oldPosition1, oldPosition2, curPos1, curPos2))
			{
                 targetX = curRotation.x - Multi_Zoom;
            } 
			else
			{   
				 targetX = curRotation.x + Multi_Zoom;
            }   
            if(targetX < MIN_X) targetX = MIN_X;
            if(targetX > MAX_X) targetX = MAX_X;

            transform.rotation = Quaternion.Euler(new Vector3(targetX, 0, curRotation.z));
            //备份上一次触摸点的位置，用于对比   
            oldPosition1 = curPos1;   
            oldPosition2 = curPos2;   
        }

        // 多点触摸结束的时候
        if(Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled || Input.GetTouch(1).phase == TouchPhase.Ended || Input.GetTouch(1).phase == TouchPhase.Canceled) {
            hitted = false;
			if(TouchState!=null)TouchState(AllenTouchState.Up);
            SwipToMin();
        }

    }

    float duration;
    //bool Auto;
    //快速滑动的时候，停手依然会向前滑动一段
    private void AutoSwip() {
        duration += Time.deltaTime * 2;
        //if(duration >= 1) Auto = false;
        float Z = transform.rotation.eulerAngles.z;
        float X = transform.rotation.eulerAngles.x;

        float targetZ = Z - LastRotateZ * RotSheepScale_Z * 0.3f;
        if(targetZ < MIN_Z) targetZ = MIN_Z;
        if(targetZ > MAX_Z) targetZ = MAX_Z;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(X, 0, targetZ), Mathf.Clamp01(duration));
    }

    void clearAutoStatus() {
        //Auto = false;
        duration = 0;
    }

    //如果多出来的区域，要滑动回去
	
	//Vector3 ResetPos1 = new Vector3(28.07034f,0,20.4f);
//	Vector3 ResetPos2 = new Vector3(25.13108f,0,30.87951f);
//	Vector3 ResetPos3 =new Vector3(30.11301f,0,41.37781f);
//	Vector3 ResetPos4 = new Vector3(30f,0,30f);
    private bool SwipToMin() {
        Vector3 curAngle = transform.rotation.eulerAngles;
		bool result=false;
		
		if(curAngle.x > MAX_NEAR_X && curAngle.z > MAX_NEAR_Z)
		{
			MiniItween.RotateTo(gameObject, new Vector3(MAX_NEAR_X, 0, MAX_NEAR_Z), BACKTIME);
			//MiniItween.RotateTo(gameObject, ResetPos3, BACKTIME);
			result = true;
		}
		else if(curAngle.x > MAX_NEAR_X && curAngle.z < MIN_NEAR_Z)
		{
			//MiniItween.RotateTo(gameObject, ResetPos1, BACKTIME);
			MiniItween.RotateTo(gameObject, new Vector3(MAX_NEAR_X, 0, MIN_NEAR_Z), BACKTIME);
			result = true;
		}
		else if(curAngle.x < MIN_NEAR_X && curAngle.z < MIN_NEAR_Z )
		{
			//MiniItween.RotateTo(gameObject, ResetPos1, BACKTIME);
			MiniItween.RotateTo(gameObject, new Vector3(MIN_NEAR_X, 0, MIN_NEAR_Z), BACKTIME);
			result = true;
		}
		else if(curAngle.x < MIN_NEAR_X && curAngle.z > MAX_NEAR_Z)
		{
			//MiniItween.RotateTo(gameObject, ResetPos3, BACKTIME);
			MiniItween.RotateTo(gameObject, new Vector3(MIN_NEAR_X, 0, MAX_NEAR_Z), BACKTIME);
			result = true;
		}
        else if(curAngle.x > MAX_NEAR_X)
		{
            MiniItween.RotateTo(gameObject, new Vector3(MAX_NEAR_X, 0, curAngle.z), BACKTIME);
			//MiniItween.RotateTo(gameObject, ResetPos4, BACKTIME);
			result = true;
        }
        else if(curAngle.x < MIN_NEAR_X)
		{
            MiniItween.RotateTo(gameObject, new Vector3(MIN_NEAR_X, 0, curAngle.z), BACKTIME);
			//MiniItween.RotateTo(gameObject, ResetPos2, BACKTIME);
			result = true;
        }
        else if(curAngle.z > MAX_NEAR_Z)
		{
			//MiniItween.RotateTo(gameObject, ResetPos3, BACKTIME);
            MiniItween.RotateTo(gameObject, new Vector3(curAngle.x, 0, MAX_NEAR_Z), BACKTIME);
			result = true;
        }
        else if(curAngle.z < MIN_NEAR_Z) 
		{
			//MiniItween.RotateTo(gameObject, ResetPos1, BACKTIME);
            MiniItween.RotateTo(gameObject, new Vector3(curAngle.x, 0, MIN_NEAR_Z), BACKTIME);
			result = true;
        }
		return result;
    }

    //函数返回真为放大，返回假为缩小   
    bool isEnlarge(Vector2 oP1, Vector2 oP2, Vector2 nP1, Vector2 nP2)
    {   
        //函数传入上一次触摸两点的位置与本次触摸两点的位置计算出用户的手势   
        var leng1 = Mathf.Sqrt((oP1.x-oP2.x)*(oP1.x-oP2.x)+(oP1.y-oP2.y)*(oP1.y-oP2.y));   
        var leng2 = Mathf.Sqrt((nP1.x-nP2.x)*(nP1.x-nP2.x)+(nP1.y-nP2.y)*(nP1.y-nP2.y));   
        if(leng1 < leng2)//放大手势   
            return true;    
        else //缩小手势      
            return false;    
    }   

}
