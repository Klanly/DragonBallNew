using System;
using System.Collections.Generic;

public class Dragon {
	//碎片
    public List<Soul> Fragment;
	public DragonInfo RTData;
	public UpDragonData Config;

    public Dragon (DragonInfo runtime, DragonManager manager, SoulManager soulManager) {

        Utils.Assert(runtime == null || manager == null || soulManager == null, "Can't initialize Dragon Object for input data is null.");

		RTData = runtime;
		Config = manager.getDragonConfig(RTData.lv);

		if(RTData.num == AoYiData.DRAGON_EARTH) {
            Fragment = soulManager.GetFramentByType(ItemType.Earth_Frage);
		} else {
            Fragment = soulManager.GetFramentByType(ItemType.Nameike_Frage);
		}

		Fragment.Sort(new SortByDraonBallNum());

		Utils.Assert(Config == null, "Dragon Up Level Config file is wrong. lv = " + RTData.lv);
	}

}


class SortByDraonBallNum : IComparer<Soul> {

    public int Compare (Soul x, Soul y) {
        return x.m_config.ID - y.m_config.ID;
	}

}
