using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SQYUIManager {

	public enum TEAM{
		NONE,
		FirstIn,
	};
	public TEAM teamState= TEAM.NONE;
	
	
	public List<Monster> allMonster;
	
	public MonsterTeam opTeam = null;
	public Monster opMonster = null;
	public int opIndex = 0;

	public Equipment targetEquip = null;

#region 类单例 实例
	private static SQYUIManager umInstance = null;
	public static SQYUIManager getInstance()
	{
		if(umInstance == null)
		{
			umInstance = new SQYUIManager();
		}
		return umInstance;
	}
	private SQYUIManager()
	{
	}
#endregion
}
