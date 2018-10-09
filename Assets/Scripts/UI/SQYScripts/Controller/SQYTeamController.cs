using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RUIType;

public class SQYTeamController : MonoBehaviour 
{
	public UIGrid m_grid;
	public UIButton m_btnSwapTeam;						//切换阵容
	public UILabel m_txtMember;							//阵容人数

	private UIProgressBar m_scrollBar;					// 滚动条
	private Dictionary<int, TeamMonsterCell> m_dicMonCell = new Dictionary<int, TeamMonsterCell>();
	private Object m_prefabMonster;						//PREFAB缓存
	Vector3 startPos = new Vector3 (5f,25,0);

	//选中的team cell
	[HideInInspector]
	public TeamMonsterCell m_selMonCell = null;			//当前选中的mon
	private GameObject m_jiantou;						//箭头

	// Use this for initialization
	void Start ()
	{
		m_btnSwapTeam.TextID = 5022;
		RefreshCurTeam();
	}
		
	//更新当前队伍
	public void RefreshCurTeam()
	{
		m_txtMember.text = TeamUI.mInstance.curTeam.validateMember.ToString() + "/14";

		while(m_grid.transform.childCount > 0)
		{
			Transform tf = m_grid.transform.GetChild(0);
			tf.parent = null;
			Destroy(tf.gameObject);
		}
		m_dicMonCell.Clear();

		if(m_prefabMonster == null)
		{
			m_prefabMonster = PrefabLoader.loadFromPack("ZQ/TeamMonsterCell");
		}

		for(int i = 0; i < TeamUI.mInstance.curTeam.capacity; i++)
		{
			GameObject obj = Instantiate(m_prefabMonster) as GameObject;
			RED.AddChild(obj, m_grid.gameObject);

			obj.name = (10 + i).ToString();
			TeamMonsterCell cell = obj.GetComponent<TeamMonsterCell>();
			cell.InitUI(i);
			m_dicMonCell.Add(i, cell);
			cell.SetSelected (false);
		}

		m_grid.enabled = true;
		m_grid.Reposition();
		m_grid.GetComponentInParent<UIScrollView> ().ResetPosition ();
        this.AutoShowVoidPos ();

		CheckEmptyGuides();

		ShowFateEffect ();

		SetMonSelected(m_dicMonCell[TeamUI.mInstance.mSelectIndex]);
	}

	void CheckEmptyGuides()
	{
		if(CheckEmptyGuide(1))
			return;
		CheckEmptyGuide(2);
	}

	bool CheckEmptyGuide(int pos)
	{
		if(pos != 1 && pos != 2)
			return false;

		if(m_dicMonCell.ContainsKey(pos) && !TeamMonsterCell.IsLock(pos))
		{
			if(Core.Data.playerManager.RTData.curTeam.getMember(pos) == null)
			{
				CreateJiantou(m_dicMonCell[pos].gameObject);
				return true;
			}
			else if(m_jiantou != null)
			{
				Destroy(m_jiantou);
				m_jiantou = null;
			}
		}
		return false;
	}


	void CreateJiantou(GameObject parent)
	{
		if(m_jiantou == null)
		{
			Object prefab = PrefabLoader.loadFromPack("JC/Jiantou");
			if(prefab != null)
			{
				m_jiantou = Instantiate(prefab) as GameObject;
				RED.AddChild(m_jiantou, parent);
			}
		}
		m_jiantou.transform.localPosition = new Vector3(-100, 0, 0);
		m_jiantou.transform.localEulerAngles = Vector3.up * 180;
	}


	public TeamMonsterCell GetMonCellByPos(int pos)
	{
		string strName = (10 + pos).ToString();
		Transform  tfcell = m_grid.transform.FindChild(strName);
		TeamMonsterCell cell = null;
		if(tfcell != null)
		{
			cell = tfcell.GetComponent<TeamMonsterCell>();
		}
		return cell;
	}

	public void RefreshMonster(Monster mon)
	{
		if(!mon.inTeam)
		{
			return;
		}

		int pos = Core.Data.playerManager.RTData.curTeam.GetMonsterPos(mon.pid);
		if(m_dicMonCell.ContainsKey(pos))
		{
			m_dicMonCell[pos].InitUI(pos);
		}

		CheckEmptyGuides();
	}

	public void RefreshSlot(int pos)
	{
		TeamMonsterCell cell = null;
		if(m_dicMonCell.ContainsKey(pos))
		{
			cell = m_dicMonCell[pos];
			cell.InitUI (pos);
		}

		CheckEmptyGuides();
	}

	public void SetMonSelected(TeamMonsterCell cell)
	{
		if (m_selMonCell != null)
		{
			m_selMonCell.SetSelected (false);
		}

		m_selMonCell = cell;
		m_selMonCell.SetSelected (true);
	}

	public void SetScrollBarPos(float val)
	{
		if (m_scrollBar == null && m_grid != null)
		{
			Transform tf = m_grid.transform.parent;
			UIScrollView scroll = tf.GetComponent<UIScrollView>();
			m_scrollBar = scroll.verticalScrollBar;
			m_scrollBar.value = val;
		}
	}


	public void SetShow(bool bShow)
	{
		RED.SetActive (bShow, this.gameObject);
	}

    //自动检测换位
    public void AutoShowVoidPos(){
        //检测 第一个 空位 
        int firstVoidPosNum = 0;
        if (Core.Data.playerManager.RTData.curTeam.validateMember >= 4) {
            //            Debug.Log (" cap = " + Core.Data.playerManager.curConfig.petSlot  + "  vali ="+ Core.Data.playerManager.RTData.curTeam.validateMember);
            if (Core.Data.playerManager.curConfig.petSlot > Core.Data.playerManager.RTData.curTeam.validateMember) {
                for (int i = 0; i < Core.Data.playerManager.RTData.curTeam.capacity; i++) {
                    if (Core.Data.playerManager.RTData.curTeam.getMember (i) == null ) {
                        firstVoidPosNum = i;
                        break;
                    }
                }
				if (TeamUI.mInstance.IsShow)
				{
                    if (firstVoidPosNum > 3)
					{
                        SpringPanel.Begin (m_grid.transform.parent.gameObject, startPos + Vector3.up * 120 * (firstVoidPosNum - 3), 8f);
                    }
                }
            }
        }
    }

	void ShowFateEffect(){
		if (TeamUI.mInstance != null) {
			TeamUI.mInstance.m_teamView.CheckEffect ();
		}
	}
}
