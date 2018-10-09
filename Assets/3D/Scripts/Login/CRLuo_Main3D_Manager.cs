using UnityEngine;
using System.Collections;

public class CRLuo_Main3D_Manager : MonoBehaviour {

    private CRLuo_Main3D_DataLoad loader;
    public GameObject go_Main3D;
    private Main3DManager _Main3DManager;

    private int P1 = -1, P2 = -1, ScreenID = -1;
    int CommandID;
    int ran;
	public bool Try_Key;
	public int ListID;
	bool Swop;
	bool Close_Key = false;
	float myWaitTime = 0.5f;
	float NowTime;
	bool myShowKey = false;
	Action3D[] defaultActionList;
	// Use this for initialization
	void Start () {
		if (1 == Random.Range (0, 2)) {
			Swop = true;
        } else {
			Swop = false;
		}
        loader = new CRLuo_Main3D_DataLoad();
        loader.load();
		do{
			P1 = -1;
			P2 = -1;
			ScreenID = -1;
			Loading_Mode();
        }while( (P1 == -1 || P2 == -1 || ScreenID == -1) && !Try_Key);

		if (Swop) {

			int tempSwopID = P1;
			P1 = P2;
			P2 = tempSwopID;
		}


        GameObject main3D = Instantiate(go_Main3D, transform.localPosition, transform.localRotation) as GameObject;
        _Main3DManager = main3D.GetComponent<Main3DManager>();
		_Main3DManager.showButtons = false;
        _Main3DManager.ShowScreen(ScreenID);
		CommandID = 0;
        if (loader.configs [ran].ActionList != null && loader.configs [ran].ActionList.Length != 0 ) {
			defaultActionList = loader.configs [ran].ActionList;
		} else {
            int rand = Random.Range(0, loader.DefaultActionList.Count);
            defaultActionList = loader.DefaultActionList[rand].ActionList;
		}

		ShowCommand();
		ShowCommand();
	}


	void Loading_Mode()
	{
		if (Try_Key) {
			ran = ListID;
		} else {
			ran = Random.Range (0, loader.configs.Count);
		}
        int temp_ScreenID = loader.configs[ran].Scene;

		Object temp_screen = PrefabLoader.loadFromPack("CRLuo/Screen/pbScreen_" + temp_ScreenID, false);
		if (temp_screen != null) {
			ScreenID = temp_ScreenID;
		} else {
            return;
		}

        int temp_P1ID = loader.configs[ran].AttackID;
		P1 = temp_P1ID;

        int temp_P2ID = loader.configs[ran].DefenseID;
		P2 = temp_P2ID;
    }

	void ShowCommand()
	{
		string Temp_Who = defaultActionList [CommandID].Who;

		if (Swop) {
			if (Temp_Who == "P1") {
				Temp_Who = "P2";
			} else {
				Temp_Who = "P1";
			}
		}

		if (Temp_Who == "P1") {

			switch (defaultActionList [CommandID].Act) {

			case "Appear":
				_Main3DManager.Show_LAction (P1);
				break;
			case "Anger":
				_Main3DManager.Free1_Ban (true);
				break;
			case "Provoke":
				_Main3DManager.Man_L.HandleTypeAnim (CRLuoAnim_Main.Type.Free2);
				break;
			case "Attack":
				_Main3DManager.Man_L.Show_RivalAmin_Key = true;
				_Main3DManager.AttackAction_Ban (true);
				break;
			case "Attack_Kill":
				_Main3DManager.KillYou = true;
				_Main3DManager.Man_L.Show_RivalAmin_Key = true;
				_Main3DManager.AttackAction_Ban (true);
				break;
			case "Skill":
				_Main3DManager.Man_L.Show_RivalAmin_Key = true;
				_Main3DManager.SkillAction_Ban (true);
				break;
			case "Skill_Kill":
				_Main3DManager.KillYou = true;
				_Main3DManager.Man_L.Show_RivalAmin_Key = true;
				_Main3DManager.SkillAction_Ban (true);
				break;
			case "GroupSkill":
				_Main3DManager.Man_L.Show_RivalAmin_Key = true;
				_Main3DManager.GroupSkill_Ban (true);
				break;
			case "GroupSkill_Kill":
				_Main3DManager.KillYou = true;
				_Main3DManager.Man_L.Show_RivalAmin_Key = true;
				_Main3DManager.GroupSkill_Ban (true);
				break;
			case "PowerSkill":
				_Main3DManager.Man_L.Show_RivalAmin_Key = true;
				_Main3DManager.PowerSkillAction_Ban (true);
				break;
			case "PowerSkill_Kill":
				_Main3DManager.KillYou = true;
				_Main3DManager.Man_L.Show_RivalAmin_Key = true;
				_Main3DManager.PowerSkillAction_Ban (true);
				break;
			case "OverSkill":
				if (_Main3DManager.Man_L.CameraKey_OverSkill) {
					_Main3DManager.KillYou = false;
					_Main3DManager.Man_L.Show_RivalAmin_Key = true;
					_Main3DManager.OverSkillAction_Ban (true);
				} else {
					_Main3DManager.KillYou = false;
					_Main3DManager.Man_L.Show_RivalAmin_Key = true;
					_Main3DManager.SkillAction_Ban (true);
				}
				break;
			case "OverSkill_Kill":
				if (_Main3DManager.Man_L.CameraKey_OverSkill) {
					_Main3DManager.KillYou = true;
					_Main3DManager.Man_L.Show_RivalAmin_Key = true;
					_Main3DManager.OverSkillAction_Ban (true);
				} else {
					_Main3DManager.KillYou = true;
					_Main3DManager.Man_L.Show_RivalAmin_Key = true;
					_Main3DManager.SkillAction_Ban (true);
				}
				break;
			case "Draw":
				_Main3DManager.Man_L.RivalOBJ = null;
				_Main3DManager.Man_R.RivalOBJ = null;
				_Main3DManager.Draw_Ban ();
				break;
			case "Draw_Trip":
				_Main3DManager.Man_L.RivalOBJ = null;
				_Main3DManager.Man_R.RivalOBJ = null;
				_Main3DManager.DoubleKill_Ban ();
				break;
			case "Draw_Kill":
				_Main3DManager.Man_L.RivalOBJ = null;
				_Main3DManager.Man_R.RivalOBJ = null;
				_Main3DManager.KillYou = true;
				_Main3DManager.DoubleKill_Ban ();
				break;

			case "Skill_Self":
				_Main3DManager.Man_L.CameraKey_Skill = false;
				_Main3DManager.Man_L.Show_RivalAmin_Key = false;
				_Main3DManager.Man_L.RivalOBJ = _Main3DManager.Man_R;
				_Main3DManager.Man_L.HandleTypeAnim (CRLuoAnim_Main.Type.Skill);
				break;
			case "Injure_0":
				_Main3DManager.Man_L.HandleTypeAnim (CRLuoAnim_Main.Type.Injure_0);
				break;
			case "Injure_1":
				_Main3DManager.Man_L.HandleTypeAnim (CRLuoAnim_Main.Type.Injure_1);
				break;
			case "Injure_2":
				_Main3DManager.Man_L.HandleTypeAnim (CRLuoAnim_Main.Type.Injure_2);
				break;
			case "Defend":
				_Main3DManager.Man_L.HandleTypeAnim (CRLuoAnim_Main.Type.Defend);
				break;
			case "Injure_Fly_Go":
				_Main3DManager.Man_L.HandleTypeAnim (CRLuoAnim_Main.Type.Injure_Fly_Go);
				break;
			case "Injure_Fly_Go_Kill":
				_Main3DManager.Man_L.SaveLife = false;
				_Main3DManager.Man_L.Injure_Key = false;
				_Main3DManager.Man_L.HandleTypeAnim (CRLuoAnim_Main.Type.Injure_Fly_Go);
				break;
			}


		} else {
			switch (defaultActionList [CommandID].Act) {
				
			case "Appear":
				_Main3DManager.Show_RAction (P2);
				break;
			case "Anger":
				_Main3DManager.Free1_Ban (false);
				break;
			case "Provoke":
				_Main3DManager.Man_R.HandleTypeAnim (CRLuoAnim_Main.Type.Free2);
				break;
			case "Attack":
				_Main3DManager.Man_R.Show_RivalAmin_Key = true;
				_Main3DManager.AttackAction_Ban (false);
				break;
			case "Attack_Kill":
				_Main3DManager.KillYou = true;
				_Main3DManager.Man_R.Show_RivalAmin_Key = true;
				_Main3DManager.AttackAction_Ban (false);
				break;
			case "Skill":
				_Main3DManager.Man_R.CameraKey_Skill = true;
				_Main3DManager.Man_R.Show_RivalAmin_Key = true;
				_Main3DManager.SkillAction_Ban (false);
				break;
			case "Skill_Kill":
				_Main3DManager.KillYou = true;
				_Main3DManager.Man_R.CameraKey_Skill = true;
				_Main3DManager.Man_R.Show_RivalAmin_Key = true;
				_Main3DManager.SkillAction_Ban (false);
				break;
			case "GroupSkill":
				_Main3DManager.Man_R.Show_RivalAmin_Key = true;
				_Main3DManager.GroupSkill_Ban (false);
				break;
			case "GroupSkill_Kill":
				_Main3DManager.KillYou = true;
				_Main3DManager.Man_R.Show_RivalAmin_Key = true;
				_Main3DManager.GroupSkill_Ban (false);
				break;

			case "PowerSkill":
				_Main3DManager.Man_R.Show_RivalAmin_Key = true;
				_Main3DManager.PowerSkillAction_Ban (false);
				break;
			case "PowerSkill_Kill":
				_Main3DManager.KillYou = true;
				_Main3DManager.Man_R.Show_RivalAmin_Key = true;
				_Main3DManager.PowerSkillAction_Ban (false);
				break;

			case "OverSkill":
				if (_Main3DManager.Man_R.CameraKey_OverSkill) {
					_Main3DManager.KillYou = false;
					_Main3DManager.Man_R.Show_RivalAmin_Key = true;
					_Main3DManager.OverSkillAction_Ban (false);
				} else {
					_Main3DManager.KillYou = false;
					_Main3DManager.Man_R.Show_RivalAmin_Key = true;
					_Main3DManager.SkillAction_Ban (false);
				}
				break;
			case "OverSkill_Kill":
				if (_Main3DManager.Man_R.CameraKey_OverSkill) {
					_Main3DManager.KillYou = true;
					_Main3DManager.Man_R.Show_RivalAmin_Key = true;
					_Main3DManager.OverSkillAction_Ban (false);
				} else {
					_Main3DManager.KillYou = true;
					_Main3DManager.Man_R.Show_RivalAmin_Key = true;
					_Main3DManager.SkillAction_Ban (false);
				}
				break;
			case "Draw":
				_Main3DManager.Man_L.RivalOBJ = null;
				_Main3DManager.Man_R.RivalOBJ = null;
				_Main3DManager.Draw_Ban ();
				break;
			case "Draw_Trip":
				_Main3DManager.Man_L.RivalOBJ = null;
				_Main3DManager.Man_R.RivalOBJ = null;
				_Main3DManager.DoubleKill_Ban ();
				break;
			case "Draw_Kill":
				_Main3DManager.Man_L.RivalOBJ = null;
				_Main3DManager.Man_R.RivalOBJ = null;
				_Main3DManager.KillYou = true;
				_Main3DManager.DoubleKill_Ban ();
				break;

			case "Skill_Self":
				_Main3DManager.Man_R.CameraKey_Skill = false;
				_Main3DManager.Man_R.Show_RivalAmin_Key = false;
				_Main3DManager.Man_R.RivalOBJ = _Main3DManager.Man_L;
				_Main3DManager.Man_R.HandleTypeAnim (CRLuoAnim_Main.Type.Skill);
				break;
			case "Injure_0":
				_Main3DManager.Man_R.HandleTypeAnim (CRLuoAnim_Main.Type.Injure_0);
				break;
			case "Injure_1":
				_Main3DManager.Man_R.HandleTypeAnim (CRLuoAnim_Main.Type.Injure_1);
				break;
			case "Injure_2":
				_Main3DManager.Man_R.HandleTypeAnim (CRLuoAnim_Main.Type.Injure_2);
				break;
			case "Defend":
				_Main3DManager.Man_R.HandleTypeAnim (CRLuoAnim_Main.Type.Defend);
				break;
			case "Injure_Fly_Go":
				_Main3DManager.Man_R.HandleTypeAnim (CRLuoAnim_Main.Type.Injure_Fly_Go);
				break;
			case "Injure_Fly_Go_Kill":
				_Main3DManager.Man_R.SaveLife = false;
				_Main3DManager.Man_R.Injure_Key = false;
				_Main3DManager.Man_R.HandleTypeAnim (CRLuoAnim_Main.Type.Injure_Fly_Go);
				break;
			}
		}

		CommandID++;
		if (CommandID < defaultActionList.Length) {
			myShowKey = true;
		} else {
			Close_Key = true;
		}
	}

	void Update()
	{
		if(_Main3DManager.Man_L != null && _Main3DManager.Man_R != null) {
			if(_Main3DManager.Man_L.NowAnimType == CRLuoAnim_Main.Type.Idle && _Main3DManager.Man_R.NowAnimType == CRLuoAnim_Main.Type.Idle && myWaitTime < (Time.time - NowTime) && myShowKey) {
				Invoke ("ShowCommand",1f);
				myShowKey = false;
				NowTime = Time.time;
			}
			
			if(Close_Key) {

				if(Swop) {
					if(_Main3DManager.Man_R.GetCurAnim() == "Idle") {
						Close_Key = false;
						_Main3DManager.Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.Free2);
						Invoke ("CloseSystem",3f);
					}
				}
				else {
					if(_Main3DManager.Man_L.GetCurAnim() == "Idle") {
						Close_Key = false;
						_Main3DManager.Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.Free2);
						Invoke ("CloseSystem",3f);
					}
				}
			}
		}
	}

	void CloseSystem()
	{
		if(Swop)
			_Main3DManager.Man_R.DelayDestroy(2.1f);
		else 
			_Main3DManager.Man_L.DelayDestroy(2.1f);
		
		LoginAnimation.Instance.CloseDoor();
		
		GameObject.Destroy(_Main3DManager.temp_Screen, 2.1f);
		GameObject.Destroy(_Main3DManager.gameObject, 2.1f);
		Destroy(this.gameObject, 2.1f);

	}


}
