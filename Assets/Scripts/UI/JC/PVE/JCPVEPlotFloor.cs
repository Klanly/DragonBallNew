using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JCPVEPlotFloor : MonoBehaviour {


	public NewFloor floorData;
	public StarsUI stars;
	public UISprite Spr_Buliding;
	public UISprite Guang;
	
	private LockFB lockFb;
	public GameObject Obj_TreasureBox;
	public TweenRotation TRotation;
	public UISprite Spr_floating;
	public UISprite Spr_light;

	int FanZhuan = 0;
	float Oldduration = 0;
	int PlayIndex = 0;
	readonly string chestName = "xiaobaoxiang";

	void PlayBaoXiangAnimation(bool StartOrStop)
	{
		CancelInvoke("PlayBXAnimation");
		if(StartOrStop)
		{
			FanZhuan = 0;
			PlayIndex = 0;
			TRotation.PlayForward();
			Oldduration = TRotation.duration;
			Invoke("PlayBXAnimation",Oldduration);
		}
		else
		{
			TRotation.transform.localRotation = Quaternion.Euler(Vector3.zero);
		}
	}

	void PlayBXAnimation()
	{
		PlayIndex ++;
		if(FanZhuan == 0)
		{
			FanZhuan = 1;
			TRotation.PlayReverse();
		}
		else if(FanZhuan == 1)
		{
			FanZhuan = 0;
			TRotation.PlayForward();
		}

		float duration = 0;
		if(PlayIndex >=4)
		{
			PlayIndex = 0;
			duration = Oldduration+0.9f;
		}
		else
			duration = Oldduration;

		if(FanZhuan < 2)
			Invoke("PlayBXAnimation",duration);

	}


	void Start()
	{
		
	}
	
	public void SetData(NewFloor flrData)
	{
		if(flrData ==  null)
		{
			gameObject.SetActive(false);
			return;
		}
		if(!gameObject.activeSelf) gameObject.SetActive(true);
		
		
		Guang.gameObject.SetActive(false);
		floorData = flrData;

		Spr_Buliding.autoResizeBoxCollider = true;
        Spr_Buliding.pivot = UIWidget.Pivot.Bottom;
		transform.localPosition = floorData.localPosition;
		
		Spr_Buliding.spriteName = floorData.config.TextrueID[0];
		Spr_Buliding.MakePixelPerfect();
		UIButtonMessage message = gameObject.AddComponent<UIButtonMessage>();
		if(message != null)
		{
			message.target = JCPVEPlotController.Instance.gameObject;
			message.functionName = "OnBuildingClick";
		}

		stars.SetStar(floorData.star);

		SetBuildingState(floorData);

		int i = 1;
		bool needCreate = false;
		for (; i <= 5; i++)
		{
			ExploreConfigData explore = Core.Data.newDungeonsManager.GetExploreData (i);
			if (explore != null)
			{
				if (explore.openfloor == floorData.config.ID && Core.Data.newDungeonsManager.lastFloorId < explore.openfloor)
				{
					needCreate = true;
					break;
				}
			}
		}
		
		if(needCreate)
		{
			if(lockFb == null)
			{
				CreateLockFB (i);
			}
			else
				 lockFb.SetFBType(i);
		}
		else if(lockFb != null)
		{
			Destroy(lockFb.gameObject);
			lockFb = null;
		}
	}
		
	 void CreateLockFB(int type)
	{
		//to add...
		Object prefab = PrefabLoader.loadFromPack ("ZQ/LockFB");
		if (prefab != null)
		{
			GameObject obj = Instantiate (prefab) as GameObject;
			RED.AddChild (obj, this.gameObject);
			lockFb = obj.GetComponent<LockFB>();
			if (lockFb != null)
			{
				lockFb.SetFBType (type);
			}
		}
	}
	
	
	void SetBuildingState(NewFloor data)
	{
		floorData = data;
		switch(data.state)
		{
		case NewFloorState.Current:
			if(data.isBoss)
			{
				Spr_Buliding.color = new Color(1f,1f,1f,1f);
				Guang.gameObject.SetActive(true);
				Obj_TreasureBox.SetActive(false);
			}
			else
			{
				if(data.config.TextrueID[0] != chestName)
				{
					Spr_Buliding.spriteName = "xiaojianzhu-02";
					Spr_Buliding.color = new Color(1f,1f,1f,1f);
					Spr_Buliding.GetComponent<BoxCollider>().enabled = true;
					Obj_TreasureBox.SetActive(false);
				}
				else
				{
					//假设都是宝箱
					Spr_Buliding.autoResizeBoxCollider = false;
					BoxCollider box = Spr_Buliding.GetComponent<BoxCollider>();
					box.enabled = true;
					box.center =new Vector3(0,50f,0);
					box.size = new Vector3(80f,100f,0);

					Spr_Buliding.spriteName = "xiaojianzhu-04";
					Spr_Buliding.color = new Color(1f,1f,1f,1f);

					Obj_TreasureBox.SetActive(true);
					Spr_light.enabled = true;
					Spr_floating.spriteName = chestName;
					Spr_floating.MakePixelPerfect();
					Spr_floating.transform.localPosition = Vector3.zero;
					PlayBaoXiangAnimation(true);
				}
			}
			Spr_Buliding.GetComponent<BoxCollider>().enabled = true;
			break;
		case NewFloorState.Pass:
			PlayBaoXiangAnimation(false);
			if(data.isBoss)
			{
			    Spr_Buliding.color = new Color(1f,1f,1f,1f);
				Spr_Buliding.GetComponent<BoxCollider>().enabled = true;
				Guang.gameObject.SetActive(true);
				Obj_TreasureBox.SetActive(false);
			}
			else
			{
				if(data.config.TextrueID[0] != chestName)
				{
					Spr_Buliding.spriteName = "xiaojianzhu-01";
					Spr_Buliding.color = new Color(1f,1f,1f,1f);
	                Spr_Buliding.GetComponent<BoxCollider>().enabled = true;
					Obj_TreasureBox.SetActive(false);
				}
				else
				{
					//假设都是宝箱
					Spr_Buliding.autoResizeBoxCollider = false;
					BoxCollider box = Spr_Buliding.GetComponent<BoxCollider>();
					box.enabled = true;
					box.center =new Vector3(0,38f,0);
					box.size = new Vector3(80f,80f,0);

					Spr_Buliding.spriteName = "xiaojianzhu-04";
					Spr_Buliding.color = new Color(1f,1f,1f,1f);

					Obj_TreasureBox.SetActive(true);
					Spr_light.enabled = false;
					TRotation.enabled = false;
					Spr_floating.spriteName = "kulou";
					Spr_floating.MakePixelPerfect();
					Spr_floating.transform.localPosition = new Vector3(0,-10f,0);
				}
			}			
			break;
		case NewFloorState.Unlocked:
			PlayBaoXiangAnimation(false);
			if(data.isBoss)
			{
			    Spr_Buliding.color = new Color(0,0,0,1f);
				Obj_TreasureBox.SetActive(false);
			}
			else
			{
				if(data.config.TextrueID[0] != chestName)
				{
					Spr_Buliding.spriteName = "xiaojianzhu-03";
					Obj_TreasureBox.SetActive(false);
					Spr_light.enabled = false;
				}
				else
				{
					//假设都是宝箱
					Spr_Buliding.autoResizeBoxCollider = false;
					BoxCollider box = Spr_Buliding.GetComponent<BoxCollider>();
					box.enabled = true;
					box.center =new Vector3(0,50f,0);
					box.size = new Vector3(80f,100f,0);

					Spr_Buliding.spriteName = "xiaojianzhu-04";
					Spr_Buliding.color = new Color(1f,1f,1f,1f);
					
					Obj_TreasureBox.SetActive(true);
					Spr_light.enabled = true;
					Spr_floating.spriteName = chestName;
					Spr_floating.MakePixelPerfect();
					Spr_floating.transform.localPosition = Vector3.zero;
					PlayBaoXiangAnimation(true);
				}
			}
			break;
		}
		
		Spr_Buliding.MakePixelPerfect();
	}

	void OnPress(bool press)
	{

		if (floorData.state == NewFloorState.Unlocked)
		{
			if (press)
			{
				List<int[]> rewards = floorData.config.Reward;
				ItemOfReward[] arryAward = new ItemOfReward[rewards.Count];
				for(int i=0; i<rewards.Count; i++)
				{
					arryAward[i] = new ItemOfReward();
					arryAward[i].pid = rewards[i][0];
				}
				JCPVEPlotController.Instance.m_rewardTipUI.SetReward (arryAward, floorData.config.ID, this.transform);
			}
			else
			{
				JCPVEPlotController.Instance.m_rewardTipUI.HideUI ();
			}
		}
	}
}
