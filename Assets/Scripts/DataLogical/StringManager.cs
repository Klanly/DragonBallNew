using System;
using System.Collections.Generic;

public class StringManager : Manager {

	private Dictionary<int, StringsData> stringsConfig = null;

	public StringManager() {
		stringsConfig = new Dictionary<int, StringsData>();
		stringsConfig.Clear();
		base.readFromLocalConfigFile<StringsData>(ConfigType.Strings, stringsConfig);
	}

	public override bool loadFromConfig() 
	{
		stringsConfig.Clear();
		return base.readFromLocalConfigFile<StringsData>(ConfigType.Strings, stringsConfig);
		
	}

	public string getString(int id) {
		StringsData found = null;
		if(stringsConfig.TryGetValue(id, out found)) {
			return found.txt;
		} else {
			return string.Empty;
		}
	}

	public string getNetworkErrorString(int id) {
		StringsData found = null;
		if(stringsConfig.TryGetValue(30000 + id, out found)) {
			return found.txt;
		} else {
            if(stringsConfig.TryGetValue(id, out found))
                return found.txt;
            else
                return string.Empty;
		}
	}
}
