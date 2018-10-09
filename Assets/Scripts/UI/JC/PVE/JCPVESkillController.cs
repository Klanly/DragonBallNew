using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class JCPVESkillController : MonoBehaviour {

	public static JCPVESkillController Instance = null;
	
	//关闭按钮回调
	public System.Action<string> Exit;
	
	public GameObject uiGrid ;
	
	private  int FristFloorID = 30101;
	
	private int FightType = 0;
	
	public UILabel Lab_DES;
	
	public UILabel Lab_Progress;
	
	public UILabel Lab_NeedEnergy;
	
	NewFloor floordata = null;
	
	void Start ()
	{
		
	}
	

	public static JCPVESkillController OpenUI()
	{
		if(Instance == null)
		{
			Object prefab = PrefabLoader.loadFromPack("JC/JCPVESkillController");
			if(prefab != null)
			{
				GameObject obj = Instantiate(prefab) as GameObject;
				RED.AddChild(obj, DBUIController.mDBUIInstance._PVERoot.gameObject);
				Instance = obj.GetComponent<JCPVESkillController>();		
				DBUIController.mDBUIInstance._PVERoot.AddPage(obj.name,obj);
			}
			Instance.Init();
		}		
		
		return Instance;
	}
	
	void Init()
	{
		
		if(Core.Data.newDungeonsManager.FloorList.TryGetValue(FristFloorID,out floordata))
			this.FightType = floordata.config.FightType;
		
		/*
		 *  {"ID":26,"txt":"攻击"}
            {"ID":27,"txt":"防御"}
            {"ID":9090,"txt":"小提示：进入副本使用当前的[ff0000]{}编队[-]"}
		 * */
		StringManager sm = Core.Data.stringManager;
		string str = this.FightType == 0 ? "[ff0000]"+sm.getString(26) : "[0000ff]"+sm.getString(27);	
		Lab_DES.text = sm.getString(9090).Replace("{}",str);
		string pragress =sm.getString(9084)+":0/2"; 
		ExplorDoors explorDoors = Core.Data.newDungeonsManager.explorDoors;
		if(explorDoors!= null && explorDoors.skill != null)
			pragress = sm.getString(9084)+":"+explorDoors.skill.passCount.ToString()+"/"+explorDoors.skill.count.ToString();
		Lab_Progress.text = pragress;
		
		string NeedEnergy =sm.getString(9091) +": ";
		if(floordata != null)  NeedEnergy+= floordata.config.NeedEnergy;
		Lab_NeedEnergy.text = NeedEnergy;
		
		CreateElement();
		
	}
	


	public void Close()
	{
		Destroy(gameObject);
		Instance = null;
		if(Exit != null) Exit(gameObject.name);
	}

	void OnBtnClick(GameObject Btn)
	{
		OnBtnClick(Btn.name);
	}
	
	void OnBtnClick(string BtnName)
	{
		if(BtnName == "Btn_Close")
		{
			Close();
		}
		else
		{
			if(Core.Data.playerManager.curJingLi < floordata.config.NeedEnergy)
			{
				JCRestoreEnergyMsg.OpenUI(110015,110016);
			}
			else
			{
				int floorID = this.FristFloorID + int.Parse(BtnName);
				NewFloor floor = null;
				//如果关卡存在
				if(Core.Data.newDungeonsManager.FloorList.TryGetValue(floorID,out floor))
				{			
					FightRoleSelectPanel FPanel = FightRoleSelectPanel.OpenUI(floor.config.TeamSize, SelectFightPanelType.SKILL_BATTLE, floor.config.FightType);
					if(FPanel != null)
					{
						gameObject.SetActive(false);
						FPanel.CallBack_Fight = (int[] array,int teamID) => 
						{
							if(array.Length == 0)
							    SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(9092));
							else
							{
								JCPVEFightLogic.Instance.Fight(floor.config.ID,array,teamID);
							}
						};
						
						FPanel.OnClose = () =>
						{
							gameObject.SetActive(true);
						};
					}
					else
						SQYAlertViewMove.CreateAlertViewMove("[ff0000]Can't find floor:"+floorID.ToString()+"[-]");
				}
				
			}
			
		}
	}
	
	
	//List<JCPVEDifficultyElement> list_cells = new List<JCPVEDifficultyElement>();
	void CreateElement()
	{
		Object prefab = PrefabLoader.loadFromPack("JC/JCPVEDifficultyElement");
		if(prefab != null)
		{
		    float x =  -320;
		    float y = 96f;
		    float w = 320;
		    float z = 196;
			int index = -1;
		    for(int i=0;i<2;i++)
			{
				for(int j = 0;j<3;j++)
				{
					index++;
					GameObject obj = Instantiate(prefab) as GameObject;
					obj.name = index.ToString();
				    obj.transform.parent = uiGrid.transform;
				    obj.transform.localScale = Vector3.one;
				    obj.transform.localPosition = new Vector3(x+w*j , y-z*i,0);				
					JCPVEDifficultyElement element = obj.GetComponent<JCPVEDifficultyElement>();
					
					UIButtonMessage  message = obj.AddComponent<UIButtonMessage>();
					if(message != null)
					{
						message.target = gameObject;
						message.functionName = "OnBtnClick";
					}

					int floorID = this.FristFloorID + index;
					NewFloor floor = null;
					if(Core.Data.newDungeonsManager.FloorList.TryGetValue(floorID,out floor))
					{
						element.SetTeamSize(floor.config.TeamSize);
						//设置解锁等级
						element.SetUnlockLevel(floor.config.Unlock);
					}

					
				}
			}
					
		}
	}
	
}
