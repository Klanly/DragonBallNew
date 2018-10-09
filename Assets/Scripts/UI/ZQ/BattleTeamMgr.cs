using System;
using fastJSON;
using System.IO;
using System.Collections.Generic;

public class BattleTeamMgr : Manager , ICore
{
	#region ICore implementation

	void ICore.Dispose ()
	{
		throw new NotImplementedException ();
	}

	void ICore.Reset ()
	{
		throw new NotImplementedException ();
	}

	void ICore.OnLogin (object obj)
	{
		throw new NotImplementedException ();
	}

	#endregion


	public readonly int SKILL_BATTLE = 1;			//技能副本
	public readonly int SOUL_BATTLE = 2;			//战魂副本
	public readonly int EXP_BATTLE = 3;				//经验副本
	public readonly int GEM_BATTLE = 4;				//宝石副本
	public readonly int STORY_BATTLE = 5;			//剧情副本
	public readonly int SHALU_BATTLE = 6;			//沙鲁副本
	public readonly int BUOU_BATTLE = 7;			//布欧副本

	private Dictionary<int, BattleTeam> dicBattleTeam = new Dictionary<int, BattleTeam> ();		//各种battle的出战队伍数据


	public override bool loadFromConfig() {
		return true;
	}
	///增加东西
	public override void addItem(BaseResponse response) {

	}
	///花费材料
	public override void spendItem(BaseResponse response) {

	}

	public override void fullfillByNetwork (BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) 
		{
			LoginResponse resp = response as LoginResponse;
			if (resp != null && resp.data != null && resp.data.teamSeqs != null) 
			{
				Unregister();
				for (int i = 0; i < resp.data.teamSeqs.Length; i++)
				{
					BattleTeam battleTeam;
					if (dicBattleTeam.ContainsKey (resp.data.teamSeqs [i].battleType))
					{
					 	battleTeam = dicBattleTeam [resp.data.teamSeqs [i].battleType];
					}
					else
					{
						battleTeam = new BattleTeam ();
						dicBattleTeam.Add (resp.data.teamSeqs [i].battleType, battleTeam);
					}
					GetTeamOrder (resp.data.teamSeqs [i].teamSeq1, ref battleTeam.m_arryAtkTeam);
					GetTeamOrder (resp.data.teamSeqs [i].teamSeq2, ref battleTeam.m_arryDefTeam);
				}
			}
		}
	}


	public BattleTeamMgr()
	{
		Unregister();
	}

	public void Unregister()
	{
		dicBattleTeam.Clear ();
		for (int i = 1; i <= 7; i++)
		{
			dicBattleTeam.Add (i, new BattleTeam ());
		}
	}


	public BattleTeam GetBattleTeam(int type)
	{
		if (dicBattleTeam.ContainsKey (type))
		{
			return dicBattleTeam [type];
		}
		RED.LogWarning ("not find battle team   " + type);
		return null;
	}
		
	public int GetBattleType(int floorID)
	{
		int type = 0;
		if (floorID / 10000 == 6)
		{
			type = 5;
		}
		else if (floorID / 10000 == 3)
		{
			type = floorID % 10000 / 100;
		}
		else
		{
			RED.LogWarning ("floorid is not validate!!!!!");
		}
		return type;
	}

	void GetTeamOrder(string strTeam, ref int[] arryTeam)
	{
		if (string.IsNullOrEmpty (strTeam))
		{
			return;
		}
		
		//btw: 此函数无法解析负数  例 7_8_2_-1_-1_5_6
		//string[] strTemp = strTeam.Split ('_');

		if (strTeam == null || strTeam.Length <= 0)
			return;
		
		List<int> tempNum = new List<int>();
		string num = "";
	    for (int i = 0; i < strTeam.Length; i++)
		{
			if(strTeam[i]!='_')
				num+=strTeam[i];
			else
			{
				tempNum.Add(int.Parse(num));
				num = "";
			}
		}
		tempNum.Add(int.Parse(num));
		arryTeam = tempNum.ToArray();

	}
}
	
public class BattleTeam
{
	//保存的是下面的选择,下面的选择最多七个人(上方是背包,不变)
	public int[] m_arryAtkTeam = new int[7];				//出战攻击队伍
	public int[] m_arryDefTeam = new int[7];				//出战防御队伍

	public BattleTeam()
	{
		for (int i = 0; i < 7; i++)
		{
			m_arryAtkTeam [i] = -1;
			m_arryDefTeam [i] = -1;
		}
	}

	public string atkTeam
	{
		get
		{
//			if(Core.Data.guideManger.isGuiding)
//				return null;
			
			string strAtk = "";
			if (m_arryAtkTeam != null)
			{
				for (int i = 0; i < m_arryAtkTeam.Length; i++)
				{
					if (i < m_arryAtkTeam.Length - 1)
					{
						strAtk += m_arryAtkTeam [i].ToString () + "_";
					}
					else
					{
						strAtk += m_arryAtkTeam [i].ToString ();
					}
				}
			}
			return strAtk;
		}
	}

	public string defTeam
	{
		get
		{
//			if(Core.Data.guideManger.isGuiding)
//				return null;
			
			string strDef = "";
			if (m_arryDefTeam != null)
			{
				for (int i = 0; i < m_arryDefTeam.Length; i++)
				{
					if (i < m_arryDefTeam.Length - 1)
					{
						strDef += m_arryDefTeam [i].ToString () + "_";
					}
					else
					{
						strDef += m_arryDefTeam [i].ToString ();
					}
				}
			}
			return strDef;
		}
	}
}
