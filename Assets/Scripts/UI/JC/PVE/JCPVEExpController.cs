using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class JCPVEExpController : MonoBehaviour {

	public static JCPVEExpController Instance = null;
	
	
	//关闭按钮回调
	public System.Action<string> Exit;
	
	private  int FristFloorID = 0;
	
	NewFloor floordata = null;
	
	//30301  30401
	public UILabel Lab_DES;
	
	public UILabel Lab_Progress;
	
	public UILabel Lab_NeedEnergy;
	//战斗类型  0:攻  1:防
	private int FightType;
	//副本类型  0:经验  1:宝石
	private int type;
	//所需精力
	private int NeedEnergy = 0;
	
	void Start ()
	{
		
		
	}
	
	
	public GameObject uiGrid ;
	
	
	//0:经验  1:宝石
	public static JCPVEExpController OpenUI(int type)
	{
		if(Instance == null)
		{
			Object prefab = PrefabLoader.loadFromPack("JC/JCPVEExpController");
			if(prefab != null)
			{
				GameObject obj = Instantiate(prefab) as GameObject;
				RED.AddChild(obj, DBUIController.mDBUIInstance._PVERoot.gameObject);
				Instance = obj.GetComponent<JCPVEExpController>();		
				DBUIController.mDBUIInstance._PVERoot.AddPage(obj.name,obj);
			}
			Instance.type = type;
			
			Instance.Init();
		}				
		return Instance;
	}
	
	
	void Init()
	{
		FristFloorID = type == 0 ? 30301:30401;
		if(Core.Data.newDungeonsManager.FloorList.TryGetValue(FristFloorID,out floordata))
			Instance.FightType = floordata.config.FightType;
		
		StringManager sm = Core.Data.stringManager;
		string str = this.FightType == 0 ? "[ff0000]"+sm.getString(26) : "[0000ff]"+sm.getString(27);	
		Lab_DES.text = sm.getString(9090).Replace("{}",str);
		string pragress =sm.getString(9084)+": 0/2"; 
		ExplorDoors explorDoors = Core.Data.newDungeonsManager.explorDoors;
		
		
		if(type == 0)
		{
			if(explorDoors!= null && explorDoors.exp != null)
				pragress = sm.getString(9084)+": "+explorDoors.exp.passCount.ToString()+"/"+explorDoors.exp.count.ToString();
		}
		else if(type == 1)
		{
			if(explorDoors!= null && explorDoors.gems != null)
				pragress = sm.getString(9084)+": "+explorDoors.gems.passCount.ToString()+"/"+explorDoors.gems.count.ToString();
		}
		

		Lab_Progress.text = pragress;
		
		//所需精力
		this.NeedEnergy = floordata.config.NeedEnergy;
		string NeedEnergy =sm.getString(9091) +": ";
		if(floordata != null)  NeedEnergy+= this.NeedEnergy.ToString();
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
			if(Core.Data.playerManager.curJingLi < NeedEnergy)
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
					SelectFightPanelType _type =  this.type == 0 ? SelectFightPanelType.EXP_BATTLE:SelectFightPanelType.GEM_BATTLE;
					FightRoleSelectPanel FPanel = FightRoleSelectPanel.OpenUI(floor.config.TeamSize,_type, floor.config.FightType);
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
				}
				else
					SQYAlertViewMove.CreateAlertViewMove("[ff0000]Can't find floor:"+floorID.ToString()+"[-]");
				
				

			}
			
			
			
		}
	}
	
	
	
	List<JCPVEDifficultyElement> list_cells = new List<JCPVEDifficultyElement>();
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
				    list_cells.Add(element);
					
				}
			}
					
		}
	}
	

	
	
	
}
