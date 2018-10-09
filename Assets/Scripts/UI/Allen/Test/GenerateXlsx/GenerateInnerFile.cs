using System;
using System.IO;
using System.Text;
using System.Collections.Generic;


/// <summary>
/// 把缘分的配表导成xlsx，需要将monMgr.mConfig改为Public，第二把下面注释的代码，反注释. 把生产的fate.txt放到python 工具XlsxWriter-0.5.7目录下
/// </summary>
public class GenerateInnerFile {

	/*private MonsterManager monMgr;
	private FateManager fateMgr;
	private EquipManager EquipMgr;


	public GenerateInnerFile() {
		monMgr = Core.Data.monManager;
		fateMgr = Core.Data.fateManager;
		EquipMgr = Core.Data.EquipManager;
	}
    */
	public void saveToFile() {
		/*
		List<MonsterData> FateMon = null;
		List<EquipData> FateEquip = null;

		string filePath = Path.Combine(DeviceInfo.PersistRootPath, "Fate.txt");

		using (StreamWriter sw = new StreamWriter(File.Open(filePath, FileMode.Create))) {

			foreach(MonsterData data in monMgr.mConfig.Values) {
				if(data != null && data.fateID != null) {

					StringBuilder sb = new StringBuilder();

					sb.Append(data.name).Append("(").Append(data.star.ToString()).Append(" star)");

					sb.Append( data.atk > data.def ? "--Att" : "--Def");

					foreach(int fateId in data.fateID) {
						FateMon = fateMgr.GetMonsterByFateNum(fateId);
						FateEquip = fateMgr.GetEquipByFateNum(fateId);

						if( (FateMon != null && FateMon.Count > 0) || (FateEquip != null && FateEquip.Count > 0) ) {
							sb.Append("#");
						}

						if(FateMon != null && FateMon.Count > 0) {
							foreach(MonsterData mon in FateMon) {
								sb.Append(mon.name).Append("(").Append(mon.star.ToString()).Append(" star Monster)");
							}
						}

						if(FateEquip != null && FateEquip.Count > 0) {
							foreach(EquipData equip in FateEquip) {
								sb.Append(equip.name).Append("(").Append(equip.star.ToString()).Append(" star Equip)");
							}
						}
					}



					if(sb.Length > 0)
						sb.Append("\n");

					sw.Write(sb.ToString());
				}
			}


		}*/



	}


}
