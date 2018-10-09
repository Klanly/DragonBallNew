using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillTableScript : MonoBehaviour 
{
	/// <summary>
	/// 技能显示预制体
	/// </summary>
	private GameObject _SkillShowCellPre;
	/// <summary>
	/// 装备显示控制
	/// </summary>
	private List<SkillShowCellScript> _MySkillShowCellList; 
	/// <summary>
	/// 控件
	/// </summary>
	private UITable _MyTable;
	// Use this for initialization


	void InitUI()
	{
		if (_MySkillShowCellList == null)
		{
			_SkillShowCellPre =  PrefabLoader.loadFromPack("GX/pbSkillShowCell")as GameObject ;
			_MySkillShowCellList = new List<SkillShowCellScript> ();
			_MyTable = gameObject.GetComponent<UITable> ();
			GameObject obj = null;
			SkillShowCellScript	script = null;
			for (int i = 0; i < 3; i++) 
			{
				obj = NGUITools.AddChild (gameObject, _SkillShowCellPre);
				script = obj.GetComponent<SkillShowCellScript> ();
				_MySkillShowCellList.Add (script);
				_MySkillShowCellList [i].gameObject.SetActive (false);
			}
		}
	}


	/// <summary>
	/// 展示需要显示的技能格子数量
	/// </summary>
	public void RefreshSkillＴable(List<Skill> skillList)
	{
		InitUI ();

		int i = 0;
		int count = skillList.Count;

		for (; i < count; i++) {
			_MySkillShowCellList [i].gameObject.SetActive (true);
			_MySkillShowCellList [i].Show (skillList[i], i / 2);
		}
		for (; i < 3; i++) {
			_MySkillShowCellList [i].gameObject.SetActive (false);
		}
		_MyTable.repositionNow = true;
	}

}
