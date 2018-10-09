using System;
#if DEBUG
using System.Diagnostics;
#endif
using System.Collections.Generic;

public class FateManager : Manager {
	//缘分的配表， Key is Num
	private Dictionary<int, FateData> fateConfig = null;

	//反向的缘分表，
	//Key is equipment id. List<int>是宠物的Number
	private Dictionary<int, List<int>> reserveFate = null;

//
//	public List<TeamFateData> atkTeamData = null;
//	public List<TeamFateData> defTeamData = null;

	#if DEBUG
	private Stopwatch stopwatch = new Stopwatch();
	#endif

	public FateManager () {
		fateConfig = new Dictionary<int, FateData>();
		reserveFate = new Dictionary<int, List<int>>();
//		atkTeamData = new List<TeamFateData> ();
//		defTeamData = new List<TeamFateData> ();
	}

	public override bool loadFromConfig () {
		bool success = base.readFromLocalConfigFile<FateData>(ConfigType.Fate, fateConfig);
		analyze();
		return success;
	}

	/// <summary>
	/// Gets the fate data from Id.
	/// </summary>
	/// <returns>The fate data from I.</returns>
	/// <param name="Id">Identifier.</param>
	public FateData getFateDataFromID (int Id) {
		FateData fd = null;
		if(!fateConfig.TryGetValue(Id, out fd)) {
			fd = null;
		} 
		return fd;
	}

	public void GetMonsterTeam(){
		MonsterTeam tAtkTeam = Core.Data.playerManager.RTData.GetTeam (1);
		MonsterTeam tDefTeam = Core.Data.playerManager.RTData.GetTeam (2);
		List<FateData> atkFate = new List<FateData> ();
		List<FateData> defFata = new List<FateData> ();

		for (int i = 0; i < 7; i++) {
			Monster tM = tAtkTeam.getMember (i);
			if (tM != null) {
				List<FateData> tAFate = tM.getMyFate (Core.Data.fateManager);
				atkFate.AddRange (tAFate);
			}
		}

		if (tDefTeam != null) {
			for (int i = 0; i < 7; i++) {
				Monster tM = tDefTeam.getMember (i);
				if (tM != null) {
					List<FateData> tDFate = tM.getMyFate (Core.Data.fateManager);
					defFata.AddRange (tDFate);
				}
			}
		}

		//defTeamData = Core.Data.playerManager.RTData.GetTeam (2);
	}

	/// <summary>
	/// Analyze this instance. 分析缘分表的反向数据
	/// 我们只分析装备的反向数据
	/// </summary>
	private void analyze() {
		#if DEBUG
		stopwatch.Start();
		#endif
		List<int> monsterId = null;
		foreach(FateData faDa in fateConfig.Values) {
			if(faDa != null) {
				int count = faDa.CountOfCondition;
				for(int i = 0; i < count; ++ i) {
					int[] condition = faDa.itemID[i + 1];
					int type = condition.Value<int>(FateData.Type_Pos);
					int Num = condition.Value<int>(FateData.Item_ID_Pos);

					//我们只分析装备的反向数据
					if(Num != 0 && type == (int)ConfigDataType.Equip) {
						if(reserveFate.TryGetValue(Num, out monsterId)) {
							monsterId.Add(faDa.WhoesFateId);
						} else {
							monsterId = new List<int>();
							monsterId.Add(faDa.WhoesFateId);
							reserveFate[Num] = monsterId;
						}
					}
				}

			}
		}

		#if DEBUG
		ConsoleEx.DebugLog("Analyze Fate Table costs " + stopwatch.ElapsedMilliseconds + " miliseconds to be done!");
		stopwatch.Reset();
		#endif
	}


	/// <summary>
	/// Gets the monster fate by equip number.根据装备的Num获得宠物的缘分关系
	/// </summary>
	/// <returns>The monster fate by equip number.</returns>
	public List<MonsterData> getMonsterFateByEquipNum(MonsterManager monManager, int equipNum) {
		Utils.Assert(monManager == null, "Monster Manager is Null.");
		List<MonsterData> monData = new List<MonsterData> ();
		List<int> monNum = null;

		if(reserveFate.TryGetValue(equipNum, out monNum)) {
			foreach(int num in monNum) {
				MonsterData md = monManager.getMonsterByNum(num);
				if(md != null) monData.Add(md);
			}
		}



		return monData;
	}


    public List<MonsterData> GetMonsterByFateNum(int fateId){
        List<MonsterData> monData = new List<MonsterData> ();
        FateData tempData = null;
		ConsoleEx.DebugLog("fate id = "+fateId);
		if(fateId != 0){
			if (fateConfig.TryGetValue (fateId,out tempData)) {
			    for (int i = 0; i < tempData.itemID.Count-1; i++) {
			//		ConsoleEx.DebugLog(" temp date "+ tempData.itemID[1][1] +" temp  count =" + tempData.itemID.Count);
			        if (tempData.itemID [1+i][0] == 1) {// 1是宠物  4是装备
			           
						MonsterData md = Core.Data.monManager.getMonsterByNum (tempData.itemID [1 + i][1]);
			            monData.Add (md);
				
					}
			    }
			}
		}
        return monData;
    }   

    public List<EquipData> GetEquipByFateNum(int fateId){
        List<EquipData> equipData = new List<EquipData> ();
        FateData tempData = null;
        if (fateConfig.TryGetValue (fateId,out tempData)) {
            for (int i = 0; i < tempData.itemID.Count -1; i++) {
                if (tempData.itemID[1 +i][0] == 4) {// 1是宠物  4是装备
                    EquipData ed = Core.Data.EquipManager.getEquipConfig (tempData.itemID [i+1][1]);
                    equipData.Add (ed);
                }
            }
        }
        return equipData;
    }

	//检测 是否 有缘
	public bool EquipAndMonIsFate(int monsterId ,int equipId){
		//装备 找 宠物   必然 1v1 情况 

		MonsterData tMData =  Core.Data.monManager.getMonsterByNum (monsterId);
		List<MonsterData> tMonList = getMonsterFateByEquipNum (Core.Data.monManager,equipId);
		ConsoleEx.DebugLog (" mID = " + monsterId + " eID = " + equipId  );
		if (tMonList != null && tMData != null) {
			for (int i = 0; i < tMonList.Count; i++) {
				if (tMonList [i].ID == tMData.ID) {
					return true;
				}
			}
		}
		return false;

	}

//	void AnalysisFate(List<FateData> tAtkList , List<FateData> tDefList){
//		List<FateData> tAList = tAtkList;
//		if (tAtkList != null) {
//			foreach (FateData fD in tAList) {
//				for(int i=0;i< fD.itemID.Count;i++)
//				if(fD.itemID[1].Value[0] )
//			}
//		}
//	}

}

//public class TeamFateData{
//	public int fateId;
//	public bool isOpen = false;
//	public bool isPtoP = false;
//	public int[] otherId;
//}


