using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
public class SQYTeamitemLstView : RUIMonoBehaviour {
    
    
	public SQYTeamController teamCtrl = null;
    public List<GameObject> lstAvatars;
    public GameObject goZhuanPan;
    
    float currentAngle = 0;
	List<Vector2> lstRoad = new List<Vector2>();
	Transform _trans;
	
	List<SQYCharatorView> szHeadViews = new List<SQYCharatorView>();
	
	float fMaxEulerAngles = 0;
	MonsterTeam currentTeam = null;
	void Awake()
	{
		
		for(int i=0;i<lstAvatars.Count;i++)
		{
			SQYCharatorView cv = SQYCharatorView.CreateCharatorView();
			cv.mIndex = i;
			RED.AddChild(cv.gameObject,lstAvatars[i]);
			cv.OnDrag +=this.OnDrag;
			cv.OnPress +=this.OnPress;
			cv.OnClick +=this.OnClick;
			szHeadViews.Add(cv);
		}
		UIEventListener.Get(goZhuanPan).onDrag = this.OnDrag;
		
	}
	// Use this for initialization
	void Start () {
		_trans = transform;
	}
	public void freshTeamItems(MonsterTeam team)
	{
		currentTeam = team;
		int count = team.capacity-3;
		
//		RED.Log("==============="+count);
		fMaxEulerAngles = count>0?count*(-45f):0;
		for(int i=0;i<lstAvatars.Count;i++)
		{
			if(i<szHeadViews.Count)
			{
				Monster mt = team.getMember(i);
				szHeadViews[i].curMonster = mt;
				if(mt==null)
				{
					
					if(i==team.capacity)
					{
						szHeadViews[i].charatorNeedUnLock(true);
					}
					else{
						szHeadViews[i].charatorNeedUnLock(false);
					}
				}
				
			}
		}
	}
	
	
	void willFreshWithNextIndex()
	{
		
		if(nextIndex >=0 && nextIndex<currentTeam.capacity)
		{
			szHeadViews[nextIndex%8].mIndex = nextIndex;
			if(lastIndex != nextIndex)
			{
				lastIndex = nextIndex;
				szHeadViews[nextIndex%8].curMonster = currentTeam.getMember(nextIndex);
				if(currentTeam.getMember(nextIndex) == null)
				{
					szHeadViews[nextIndex%8].charatorNeedUnLock(false);
				}
			}
		}
		else if(nextIndex == currentTeam.capacity)
		{
			szHeadViews[nextIndex%8].mIndex = nextIndex;
			szHeadViews[nextIndex%8].curMonster = currentTeam.getMember(nextIndex);
			szHeadViews[nextIndex%8].charatorNeedUnLock(true);
		}
	}
	bool bEffect = false;
	float mThreshold = 0f;


	Vector3 target = new Vector3(0,0,360);
	Vector3 v3from ;

	float strength = 4;
	void Update()
	{

		if(bEffect)
		{
			float delta = RealTime.deltaTime;

			if (mThreshold == 0f)
			{
				mThreshold = (target - v3from).magnitude * 0.005f;
				mThreshold = Mathf.Max(mThreshold, 0.00001f);
			}

			v3from = NGUIMath.SpringLerp(v3from, target, strength, delta);

			if (mThreshold >= Vector3.Magnitude(v3from - target))
			{
				v3from = target;
				bEffect = false;
			}
//			_trans.localEulerAngles = v3from;

			freshTeamUIWithAngle(bUp,v3from.z);

			if(isHead || isEnd)
			{
				bEffect = false;
			}

		}

	}


	int current = 0;
	int lastIndex = -1;
	int nextIndex = 0;
	bool isEnd = false;
	bool isHead = false;
	float fIncremental = 0;
	bool bUp = true;

	public void OnDrag(GameObject go, Vector2 delta)
	{
		if(go == null)return;
		
		lstRoad.Add(delta);

		if(delta.x==0&&delta.y==0)return;

		Vector2 center = new Vector2(_trans.position.x,_trans.position.y);
		                 
		Vector2 itemPos1 = new Vector2(go.transform.position.x,go.transform.position.y) - center;
		Vector2 itemPos2 = new Vector2(go.transform.position.x,go.transform.position.y)+delta - center;
		

		
		fIncremental = Vector2.Angle(itemPos2,itemPos1)/15;
		
		bUp = delta.y>=0;
		
		if(bUp)
		{
			currentAngle -=fIncremental;
//			current = -((int)currentAngle)/45;
//			nextIndex = current+4;
		}
		else{
			currentAngle +=fIncremental;
//			current = -((int)currentAngle)/45;
//			nextIndex = current-1;
		}

		freshTeamUIWithAngle(bUp,currentAngle);

		fIncremental = 0;

//		willFreshWithNextIndex();
//		
//		if(currentAngle >=0)
//		{
//			currentAngle = 0;
//			isHead = true;
//		}
//		else{
//			isHead = false;
//		}
//		
//		if(currentAngle <=fMaxEulerAngles)
//		{
//			currentAngle = fMaxEulerAngles;
//			isEnd = true;
//		}
//		else{
//			isEnd = false;
//		}
//
//		if(!float.IsNaN(currentAngle))
//		{
//			_trans.localEulerAngles = new Vector3(0, 0, currentAngle);
//		} 
//		
//		for(int i=0;i<lstAvatars.Count;i++)
//		{
//			lstAvatars[i].transform.localEulerAngles = new Vector3(0, 0, -currentAngle);
//		}
//		
		
	}
	void freshTeamUIWithAngle(bool up,float angle)
	{
		if(up)
		{
			current = -((int)angle)/45;
			nextIndex = current+4;
		}
		else{
			current = -((int)angle)/45;
			nextIndex = current-1;
		}

		willFreshWithNextIndex();

		if(angle >=0)
		{
			angle = 0;
			isHead = true;
		}
		else{
			isHead = false;
		}

		if(angle <=fMaxEulerAngles)
		{
			angle = fMaxEulerAngles;
			isEnd = true;
		}
		else{
			isEnd = false;
		}

		if(!float.IsNaN(angle))
		{
			_trans.localEulerAngles = new Vector3(0, 0, angle);
		} 

		for(int i=0;i<lstAvatars.Count;i++)
		{
			lstAvatars[i].transform.localEulerAngles = new Vector3(0, 0, -angle);
		}
	}
	float deltaTime = 0;
	float myTime = 0;
	public void OnPress(GameObject go,bool press)
	{
		
		if(press)
		{
			lstRoad.Clear();
			myTime = Time.time;
		}
		else{
		
			deltaTime = Time.time - myTime;
			
			
			float mDistance = 0;
			int mc = lstRoad.Count;
			if(lstRoad.Count>0)
			{
				mDistance = Vector2.Distance(lstRoad[mc-1],lstRoad[0]);
			}
			float perV = mDistance/deltaTime;
			if(perV <50) //click
			{
			
			}
			else{//quick drag

			}
			bEffect = true;
			v3from = new Vector3(0,0,currentAngle);
			target = v3from+new Vector3(0,0,mDistance*2*(bUp?-1:1));
			int z = ((int)target.z)/45*45;

			target = new Vector3(0,0,z);
//			rigidbody.isKinematic = false;
//			rigidbody.AddTorque (new Vector3(0,0,mDistance/2*(currentAngle>0?1:-1)));

			RED.Log("press.======"+mDistance+"------"+deltaTime+"********"+(mDistance/deltaTime));
			
			
		}
	}
	
	public void OnClick(GameObject go)
	{
//		if(teamCtrl!= null)
//		{
//			teamCtrl.ClickATeamMonster(null);
//		}
	}
	
}
