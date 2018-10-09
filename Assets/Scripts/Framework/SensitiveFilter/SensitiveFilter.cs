using System;
using System.IO;
using JsonFx.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class SensitiveFilterManager : Manager {

	private static char[] INVALIDATE_CHAR = {'@','#','!','$','%','^','&','*','(',')',',',':','?','[',']','{','}',';','.','/','\\','|','\'','\"'};
	private BadWordsFilter badWorldFilter = null;

    private static SensitiveFilterManager _filter;
    private SensitiveFilterManager() {
		badWorldFilter = new BadWordsFilter();
        loadFromConfig ();
	}
	
    public static SensitiveFilterManager getInstance() {
		if (_filter == null)
            _filter = new SensitiveFilterManager ();
		return _filter;
	}
	
    public override bool loadFromConfig () {
        bool success = false;

        string localpath = Path.Combine(getBasePath(), Config.LocalConfigs[ConfigType.SensitiveData].path);

        success = File.Exists(localpath);
        if(success) {
            loadFromFile(localpath);
        }

        return success;
    }


	/// <summary>
	/// Loads from file.
	/// </summary>
    private bool loadFromFile (string path) {
        ConsoleEx.DebugLog("Load Sensitive Filter data here..");
        bool success = false;

        StreamReader sr = null;
        FileStream fs = File.OpenRead(path);
        string line = null;
        try {
            sr = new StreamReader(fs);
            if(sr != null) {
                while( !string.IsNullOrEmpty(line = sr.ReadLine()) ) {
                    if(!line.StartsWith ("#")) {
                        string[] contain = line.Split ('@');// we define @ as split charactor
                        if (contain != null && contain.Length > 0) {
                            foreach (string name in contain) {
                                if (!string.IsNullOrEmpty (name))
                                    badWorldFilter.AddKey(name);
                            }
                        }
                    }
                }
            }
        } catch(Exception ex) {
            ConsoleEx.DebugLog(ex.ToString() + "\nError Line = " + line);
            success = false;
        } finally {
            if(sr != null) { sr.Close(); sr = null; }
            if(fs != null) { fs.Close(); fs = null; }

            #if DEBUG
            ConsoleEx.DebugLog(ConfigType.SensitiveData.ToString());
            #endif
        }
        return success;
	}		
	
	private bool checkInvalidateJson (string input) {
		bool inValidate = false;
		if(!string.IsNullOrEmpty(input)) {
			//foreach()
			char[] toBeHit = input.ToCharArray ();
			if(toBeHit != null && toBeHit.Length > 0) {
				foreach(char c in toBeHit) {
                    inValidate = Utils.inArray (c, INVALIDATE_CHAR);
					if (inValidate) break;
				}
			}
		}
		return inValidate;
	}
	
	
	/// <summary>
	/// Check the specified input.
	/// return : true means contains sensetive data
	/// </summary>
	/// <param name="input">Input.</param>
	public bool check(string input, bool SplitChar = true) {
#if China
		bool inValidate = false;
		if(SplitChar)
			inValidate = checkInvalidateJson (input);
		if (!inValidate) {
			bool flag = badWorldFilter.HasBadWord(input);
			return flag;
		}
		else
			return inValidate;
#else
		return checkInvalidateJson (input);
#endif
		
	}
}
