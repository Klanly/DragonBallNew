using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SelectFightPanelType
{
	 SKILL_BATTLE = 1,			//技能副本
	 SOUL_BATTLE = 2,			//战魂副本
	 EXP_BATTLE = 3,				//经验副本
	 GEM_BATTLE = 4,			    //宝石副本
	 STORY_BATTLE = 5,		    //剧情副本
	 SHALU_BATTLE = 6,			//沙鲁副本
	 BUOU_BATTLE = 7,			//布欧副本
}

public class FightRoleSelectPanel : MonoBehaviour {
	
	public class MonsterSelected
	{
		public int key;
		public Monster data;
		public MonsterSelected(int key,Monster data)
		{
			this.key = key;
			this.data = data;
		}
	}

	public List<FightRoleAllElement> list_cells = new List<FightRoleAllElement>();
	public GameObject uiGrid;
	public static FightRoleSelectPanel Instance;
	
	public UIGrid uiGrid2;
	
	private	List<Monster> list_att = null;
	private	List<Monster> list_def = null;
	private List<Monster> cur_allRolelist = null;

	public UILabel Lab_DesAttribute;
	
	public UIButton[] Btn_back;
	public UISprite[] Btn_sgin;
	
	int FightType = -1;
	
	int TeamCount = 0;
	
	public UILabel Lab_Title;
	
	//下方七个插槽<UI物件>
	public List<FightRoleSelecteElement> list_SelectedRoles = new List<FightRoleSelecteElement>();
	
	public UILabel Lab_TeamAtkOrDef;
	
	//已选择的数据<默认7个null,null则显示为空>
	public List<MonsterSelected> list_selected = new List<MonsterSelected>(){null,null,null,null,null,null,null};
	
	public UISprite Spr_fightType;
	
	//点击战斗的回调
	public System.Action<int[],int> CallBack_Fight;
	
	public System.Action OnClose;
	
	
	//开启防御阵的等级
	public const int OpenDefListLv = 20;
	
	//PVE副本类型
	private SelectFightPanelType PVEType;
	//移动元件
	public RewardCell moveCell;
	//透明动画
	public TweenAlpha TMovealpaha;
	//位移动画
	public TweenPosition TMovepostion;

	public TweenScale _scale;

	void Start ()
	{
		UIMiniPlayerController.Instance.SetActive (false);
		
		if(TopMenuUI.mInstance != null)
		{
			TopMenuUI.mInstance.freshCurTeamCallback = () =>
			{
				//Debug.LogError("Refresh jiemian");
				//刷新界面
				Init();
			};
		}
	}
	

	
	//初始化数据
	void Init()
	{
		Lab_DesAttribute.text = Core.Data.stringManager.getString(9114);
		CreateElement();

		MonsterTeam atk_team = Core.Data.playerManager.RTData.GetTeam(1);
		MonsterTeam def_team = Core.Data.playerManager.RTData.GetTeam(2);
		list_att =   atk_team== null ? new List<Monster>(){null,null,null,null,null,null,null, null,null,null,null,null,null,null} :  atk_team.TeamMember;
		list_def =  def_team== null ? new List<Monster>(){null,null,null,null,null,null,null, null,null,null,null,null,null,null} :  def_team.TeamMember;

//		for(int i=0;i<list_att.Count;i++)
//		{
//			if(list_att[i]==null)
//				Debug.LogError(i.ToString()+":null");
//			else
//				Debug.LogError(i.ToString()+":"+list_att[i].pid);
//		}

		//20级之前没有防御队
		if(Core.Data.playerManager.Lv < OpenDefListLv)
		{
			SetButtonStatus(0,0);
			SetButtonStatus(1,2);
			cur_allRolelist = list_att;		
		}
		else
		{
			if(FightType == 0)
			{
				SetButtonStatus(0,0);
			    SetButtonStatus(1,1);
				cur_allRolelist = list_att;			
			}
			else
			{
				SetButtonStatus(0,1);
			    SetButtonStatus(1,0);
				cur_allRolelist = list_def;				
			}
		}
		
		RefreshPanel(cur_allRolelist);

		Spr_fightType.spriteName = this.FightType == 0 ? "BattleUI_Attack":"BattleUI_Defense";
		

		CreateElement2();

		DefalutSelect();

	}
	
	//默认选队伍,获取最近的一次战斗保存记录
	public void DefalutSelect()
	{
		/*{"ID":9088,"txt":"攻击阵容"}
          {"ID":9089,"txt":"防御阵容"} */

		int[] last_save_pos = null;
		
		if(cur_allRolelist == list_att)
		{
			Lab_Title.text = Core.Data.stringManager.getString(9088);
			//获得记忆位置
			last_save_pos = Core.Data.battleTeamMgr.GetBattleTeam((int)this.PVEType).m_arryAtkTeam;
		}
		else if(cur_allRolelist == list_def)
		{
			Lab_Title.text = Core.Data.stringManager.getString(9089);
			//获得记忆位置
			last_save_pos = Core.Data.battleTeamMgr.GetBattleTeam((int)this.PVEType).m_arryDefTeam;
		}
		
		//初始化列表
		for(int i=0;i<list_selected.Count;i++)
			list_selected[i] = null;
		//初始化待选人列表
		for(int i = 0;i<list_cells.Count;i++)
			list_cells[i].isSelected = false;
		
		for(int i=0;i<this.TeamCount;i++)
		{		
			if(cur_allRolelist !=null && i<cur_allRolelist.Count)
			{
				//记忆下标
				int key = last_save_pos[i];
				if(key >= 0)
				{
					list_cells[key].isSelected = true;
					list_selected[i] = new MonsterSelected(key,cur_allRolelist[key])  ;
				}  
				else
				{
					//-1:空位
					list_selected[i] = null;
				}
			}
		}
		RefreshSelectedPanel();
	}
	
	
	public static FightRoleSelectPanel OpenUI(int TeamCount  ,SelectFightPanelType PVEType,   int FightType = -1)
	{
		if(Instance == null)
		{
			Object prefab = PrefabLoader.loadFromPack("JC/FightRoleSelectPanel");
			if(prefab != null)
			{
				GameObject obj = Instantiate(prefab) as GameObject;
				RED.AddChild(obj, DBUIController.mDBUIInstance._PVERoot.gameObject);
				Instance = obj.GetComponent<FightRoleSelectPanel>();		
				DBUIController.mDBUIInstance._PVERoot.AddPage(obj.name,obj);
				Instance.FightType = FightType;
				
				Instance.TeamCount = TeamCount;
				Instance.PVEType = PVEType;
			}
			Instance.Init();
		}				
		return Instance;
	}
	
	
	
	void CreateElement()
	{
		//Debug.LogError("CreateElement -> list_cells.cout="+list_cells.Count.ToString());
		if(list_cells.Count == 1)
		{
			float x = -360;
			float y = 115;
			float w = 120;
			float z =125;
			int index = 0;
			for(int i=0;i<2;i++)
			{
				for(int j= 0;j<7;j++)
				{
					index++;
					if(list_cells.Count < index)
					{
						GameObject cell = Instantiate(list_cells[0].gameObject) as GameObject;
						cell.transform.parent = uiGrid.transform;
						cell.transform.localScale = Vector3.one;
						list_cells.Add(cell.GetComponent<FightRoleAllElement>());			
					}
					list_cells[index-1].transform.localPosition = new Vector3(x+j*w,y-z*i,0);
					list_cells[index-1].gameObject.name ="all_"+(index-1).ToString();
					list_cells[index-1].gameObject.SetActive(false);
				}
			}
		}
	}
	
	
	void CreateElement2()
	{
		//Debug.LogError("list_SelectedRoles -> list_SelectedRoles.cout="+list_SelectedRoles.Count.ToString());
		if(list_SelectedRoles.Count == 1)
		{
//			float x = -390;
//			float w = 130;
			for(int i=0;i<7;i++)
			{
				if(i > 0)
				{
					GameObject obj= Instantiate(list_SelectedRoles[0].gameObject) as GameObject;

					RED.AddChild(obj,uiGrid2.gameObject,new Vector3(uiGrid2.cellWidth*i,14f,0));		
					FightRoleSelecteElement element = obj.GetComponent<FightRoleSelecteElement>();
					list_SelectedRoles.Add(element);				
				}
				list_SelectedRoles[i].gameObject.name = "sel_"+i.ToString();
			}
			//uiGrid2.repositionNow = true;
		}
	}
	
	
	void OnBtnClick(GameObject Btn)
	{
		OnBtnClick(Btn.name);
	}

	//获取移动层坐标
	Vector3 GetPosAtMovePanel(GameObject G)
	{
		Transform Parent = moveCell.gameObject.transform.parent;
		Transform OldParent = G.transform.parent;
		G.transform.parent = Parent;
		Vector3 pos = G.transform.localPosition;
		G.transform.parent = OldParent;
		return pos;
	}

	//播放移动动画
	void PlayMoveAnimation(FightRoleAllElement Allelement,FightRoleSelecteElement Selectelement,bool VtoHOrHtoV)
	{
		if(!moveCell.gameObject.activeSelf)moveCell.gameObject.SetActive(true);
		Vector3? startPos = null;
		Vector3? endPos = null; 

		TMovealpaha.ResetToBeginning();
		TMovepostion.ResetToBeginning();

		if(VtoHOrHtoV)
		{
			startPos = GetPosAtMovePanel(Allelement.gameObject);
			endPos = GetPosAtMovePanel(Selectelement.gameObject);

			UISprite s = TMovealpaha.GetComponent<UISprite>();
			if(s != null) s.alpha = 0;
			TMovealpaha.from = 0;
			TMovealpaha.to = 1f;
			TMovealpaha.PlayForward();

		}
		else
		{
			startPos = GetPosAtMovePanel(Selectelement.gameObject);
			endPos = GetPosAtMovePanel(Allelement.gameObject);

			UISprite s = TMovealpaha.GetComponent<UISprite>();
			if(s != null) s.alpha = 1f;
			TMovealpaha.from = 1f;
			TMovealpaha.to = 0;
			TMovealpaha.PlayForward();
		}

		TMovepostion.transform.localPosition = (Vector3)startPos;
		TMovepostion.from = (Vector3)startPos;
		TMovepostion.to = (Vector3)endPos;

		TMovepostion.PlayForward();
		StartCoroutine(OnPlayMoveAnimationFinished(Allelement,Selectelement,VtoHOrHtoV));
	}

	//播放位移动画结束
	IEnumerator OnPlayMoveAnimationFinished(FightRoleAllElement Allelement,FightRoleSelecteElement Selectelement,bool VtoHOrHtoV)
	{
		yield return new WaitForSeconds(0.25f);

		if(VtoHOrHtoV)
		{
			RefreshSelectedPanel();
		}
		else
		{
			Allelement.isSelected = false;
		}

		yield return new WaitForSeconds(0.1f);
		moveCell.gameObject.SetActive(false);
	}

	
	public void OnBtnClick(string BtnName)
	{
		if(moveCell.gameObject.activeSelf)  
		{
			//Debug.LogError("NO Click");
			return;
		}
		if(BtnName.Contains("all_"))
		{			
			int index = int.Parse(BtnName.Substring(4,BtnName.Length-4));
			if(!list_cells[index].isSelected)
			{
				
				if(CurSelectedCount < this.TeamCount  )
				{
					MonsterSelected MS = new MonsterSelected(index, cur_allRolelist[index]);
					int insertPos = AutoAddDataToList_Selected(MS);
					if( insertPos > -1 )
					{
						list_cells[index].isSelected = true;
						moveCell.ShowMonster(MS.data);
						PlayMoveAnimation(list_cells[index],list_SelectedRoles[insertPos],true);

					}
				}
			}
			else
			{
				if(index < list_cells.Count )
				{
					list_cells[index].isSelected = false;
					int key = RemoveListSelectAtListAllRollIndex(index);	
					if(key >-1)
					{
						PlayMoveAnimation(list_cells[index], list_SelectedRoles[key], false);
						RefreshSelectedPanel();
					}
					else
					{
						Debug.LogError("Not find all in select list , the all id is"+index.ToString());
					}
				}
			}

		}
		else if(BtnName.Contains("sel_"))
		{
			int index = int.Parse(BtnName.Substring(4,BtnName.Length-4));
			if(index < list_selected.Count)
			{
				if(index>=0 && index<list_selected.Count &&  list_selected[index]!= null)
				{
					int key = list_selected[index].key;
					list_selected[index] = null;
					//list_cells[key].isSelected = false;
					PlayMoveAnimation(list_cells[key], list_SelectedRoles[index], false);
					RefreshSelectedPanel();
				}
			}
		}
		else if(BtnName == "Btn_Close")
		{
			UIMiniPlayerController.Instance.SetActive(true);
			if(OnClose != null) OnClose();
			Destroy(gameObject);
			if(TopMenuUI.mInstance != null)
			    TopMenuUI.mInstance.freshCurTeamCallback = null;
		}
		else if(BtnName == "Btn_ATT")
		{	
			if(Btn_ATK_Status != 2)
			{
				SetButtonStatus(0,0);
				if(Btn_DEF_Status != 2)
				{

					cur_allRolelist = list_att;
					 RefreshPanel(cur_allRolelist);
					DefalutSelect();
					SetButtonStatus(1,1);
				}
			}
		}
		else if(BtnName == "Btn_DEF")
		{
			if(Btn_DEF_Status != 2)
			{
				SetButtonStatus(1,0);
				if(Btn_ATK_Status != 2)
				{
				    
					 cur_allRolelist = list_def;
					 RefreshPanel(cur_allRolelist);
					 DefalutSelect();
					SetButtonStatus(0,1);
				}
			}
		}
		else if(BtnName == "Btn_Fight")
		{
			List<int> result = new List<int>();
			for(int i=0;i<list_selected.Count;i++)
			{
				if(list_selected[i] != null && list_selected[i].data !=null)
				{
					if(list_selected[i].data.pid < 0)
					{
						//Debug.LogError("Get realy data at pos :"+ list_selected[i].key.ToString());
						list_selected[i].data = Core.Data.playerManager.RTData.curTeam.getMember(list_selected[i].key);
						if(list_selected[i].data.pid < 0)
						{
							SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(17));
							return;
						}
					}
					result.Add(list_selected[i].data.pid);
				}
				
			}
			
			List<int> pos = new List<int>();
			for(int i=0;i<list_selected.Count;i++)
			{
				if(list_selected[i] != null)
				   pos.Add( list_selected[i].key);
				else
				   pos.Add( -1);
			}
			
			int teamID = 1;
			if(Btn_ATK_Status == 0)
			{
				teamID = 1;
				Core.Data.battleTeamMgr.GetBattleTeam((int)this.PVEType).m_arryAtkTeam = pos.ToArray();
			}
			else if(Btn_DEF_Status == 0)
			{
				teamID = 2;
				Core.Data.battleTeamMgr.GetBattleTeam((int)this.PVEType).m_arryDefTeam = pos.ToArray();
			}
			
			CallBack_Fight(result.ToArray(),teamID);
		}
		
	}

	void OnBtn_DesPressFalse()
	{
		_scale.PlayForward();
	}
	void OnBtn_DesPressTrue()
	{
		_scale.PlayReverse();
	}


	//通过所有人物队列下标,删除选择列表里面的数据(如果有的话)
	int RemoveListSelectAtListAllRollIndex(int index)
	{
		for(int i=0;i<list_selected.Count;i++)
		{
			if(list_selected[i] !=null && list_selected[i].key == index)
			{
				list_selected[i] = null;
				return i;
			}
		}
		return -1;
	}

	
	//根据传入的列表显示对应的队伍
	void RefreshPanel(List<Monster> list)
	{
//		int count = 0;
//		if(list != null )
//		{
//			foreach(Monster m in list)
//			{
//				if(m != null)
//			    count++;
//			}
//		}		
//		Debug.LogError("list.cout="+list.Count.ToString()+"   "+list_cells.Count.ToString());

		int count = list.Count;

		for(int i=0;i<list_cells.Count;i++)
		{
			if(i<count && list[i]!= null)
			{
				list_cells[i].gameObject.SetActive(true);
				list_cells[i].ShowMonster(list[i]);
			}
			else
			{
				list_cells[i].gameObject.SetActive(false);
			}
		}
	}
	
	
	
	//设置按钮状态 ButtonID: 0:第一个按钮   1:第二个按钮    Status  0:选中状态 1.正常状态 2.不可用状态
	int Btn_ATK_Status;
	int Btn_DEF_Status;
	void SetButtonStatus(int ButtonID,int Status)
	{
		if(ButtonID ==0) Btn_ATK_Status = Status;
		if(ButtonID ==1) Btn_DEF_Status = Status;
		UISprite buttonSprite = Btn_back[ButtonID].GetComponentInChildren<UISprite>();
		switch(Status)
		{
		case 0:
			buttonSprite.depth = 20;
			Btn_back[ButtonID].normalSprite = "Symbol 31";
			Btn_sgin[ButtonID].enabled = true;
			break;
		case 1:
			buttonSprite.depth = 6;
			Btn_back[ButtonID].normalSprite = "Symbol 32";
			Btn_sgin[ButtonID].enabled = false;
			break;
		case 2:
			buttonSprite.depth = 6;
			Btn_back[ButtonID].normalSprite = "Symbol 29";
			Btn_sgin[ButtonID].enabled = false;
			Btn_back[ButtonID].GetComponent<BoxCollider>().enabled = false;
			break;
		}
	}
	
	
	
	
	//刷新选择列表
	void RefreshSelectedPanel()
	{
		//int count = list_selected.Count;
		int AllCount = list_SelectedRoles.Count; //   (7)
		for(int i= 0;i<AllCount;i++)
		{
			if(i < this.TeamCount)		
			{
				if(list_selected[i] != null && list_selected[i].data != null)
				{
					//显示人
				   list_SelectedRoles[i].SetState(FightRoleSelecteElement.Status.Normal);
					list_SelectedRoles[i].ShowMonster( list_selected[i].data );
				}
				else
				{
					//显示空
				   list_SelectedRoles[i].SetState(FightRoleSelecteElement.Status.None);
				}
			}
			else
			{			
				//显示锁
					list_SelectedRoles[i].SetState(FightRoleSelecteElement.Status.Locked);
			}
		}

		#region 计算当前攻或防值
	    List<int> templist = new List<int>();
		for(int i=0;i<list_selected.Count;i++)
		{
			if(list_selected[i] != null)
			templist.Add(list_selected[i].key);
		}
		
		MonsterTeam team = null;
		if(cur_allRolelist == list_att)
			team = Core.Data.playerManager.RTData.getTeam(1);
		else if(cur_allRolelist == list_def)
			team = Core.Data.playerManager.RTData.getTeam(2);
		
		int showValue = 0;
		if(this.FightType == 0)
		{
			if( team != null)
		    showValue = team.GetTeamMeberAtk(templist);
		}
		else if(this.FightType == 1)
		{
			if(team != null)
			showValue = team.GetTeamMeberDef(templist);
		}
			
		Lab_TeamAtkOrDef.text = showValue.ToString() ;
		#endregion

	}
	
	//自动写入选择列表,(插入空白处)
	int AutoAddDataToList_Selected(MonsterSelected Newdata)
	{
		for(int i= 0;i< list_selected.Count;i++)
		{
			if(list_selected[i] == null)
			{
				list_selected[i] = Newdata;
				return i;
			}
		}	
		return -1;
	}
		
	//当前已选择的人数
	int CurSelectedCount
	{
		get
		{
			int count = 0;
			for(int i= 0;i< list_selected.Count;i++)
			{
				if(list_selected[i] != null)
					count++;
			}
			return count;
		}
	}
	
}
