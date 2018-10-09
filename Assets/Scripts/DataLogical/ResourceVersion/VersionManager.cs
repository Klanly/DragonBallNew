using System;
using System.Collections.Generic;

public class VersionManager : Manager {
    private Dictionary<string, int> dicVers = null;
    private List<VersionNumItem> Config = null;

    public VersionManager() {
        dicVers = new Dictionary<string, int>();
        Config = new List<VersionNumItem>();
    }

    public override bool loadFromConfig () {
        bool success = base.readFromLocalConfigFile<VersionNumItem>(ConfigType.VersionConfig, Config);
        anaylize();
        return success;
    }

    private void anaylize() {
        foreach(VersionNumItem item in Config) {
            dicVers.Add(item.FileName, item.num);
        }
    }

    public int GetVersionNum(string AssetName) {
        int result = 0;
        dicVers.TryGetValue(AssetName, out result);
        return result;
    }

	//获取下载资源列表
	public List<VersionNumItem> GetVersionList()
	{
		return Config;
	}
}
