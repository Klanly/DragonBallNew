using System;
using System.IO;
using System.Collections.Generic;

public enum DataType {
	SERVER_TYPE    = 0x01,
	HTTP_COMPLETE  = 0x02,
	CRASH_TYPE     = 0x03,
	USER_CONFIG    = 0x04,
    ACCOUNT_CONFIG = 0x05,
	TIPS_CONFIG    = 0x06,
}

public class RelationData
{
    public string fileName;
    public Type typeOfData;

    public RelationData(string file, Type type){
        this.fileName = file;
        this.typeOfData = type;
    }
}


[Serializable]
public class DataObject {
    [NonSerialized]
	public DataType mType;

	[NonSerialized]
	public string Path;
}

public interface IlocaleReadWrite {
	bool WriteToLocalFileSystem(DataObject toBeSaved, bool crypto = true);
	bool AppendToLocalFileSystem(DataObject toBeSaved, bool crypto = true);
	DataObject ReadFromLocalFileSystem(DataType curType, bool decrypto = true); 
}

public class DataPersistManager : IlocaleReadWrite, ICore {
	private string baseAccountPath;
    private string baseNonAccountPath;
    //this only can be read or set to null
	private SharedPrefs prefs;

    public string AccountPath
    {
        set { baseAccountPath = value; }
    }

    public string NonAccountPath
    {
        set { baseNonAccountPath = value; }
    }

    /*
     * We define file name and typeOf(DataObject)
     */
	public static readonly Dictionary<DataType, RelationData> PreDefined = new Dictionary<DataType,RelationData>() {
		{ DataType.SERVER_TYPE,       new RelationData("Server.bin",    typeof(DataObject))     },
		{ DataType.HTTP_COMPLETE,     new RelationData("Hc.bin",        typeof(HttpData_No))    },
		{ DataType.USER_CONFIG,       new RelationData("Cf.bin",        typeof(UserConfigData)) },
        { DataType.ACCOUNT_CONFIG,    new RelationData("Af.bin",        typeof(AccountConfigData))},
		{ DataType.TIPS_CONFIG,       new RelationData("Tp.bin",        typeof(NoticeLocal))    },
	};

	/// <summary>
	/// 非账号相关的定义
	/// </summary>
    public static readonly DataType[] NonAccountDataType = {
        DataType.SERVER_TYPE, DataType.CRASH_TYPE, DataType.USER_CONFIG,
    };
	
    private DataPersistManager(string basePath) {
		prefs = new SharedPrefs();
        baseNonAccountPath = basePath;
	}

    private static DataPersistManager dsManager = null;
    public static DataPersistManager getInstance(string basePath)
    {
		if(dsManager == null)
            dsManager = new DataPersistManager(basePath);
		return dsManager;
	}

	public bool AppendToLocalFileSystem(DataObject toBeSaved, bool crypto = true) {
		bool success = true;
		if(toBeSaved == null) {
			success = false;
		} else {
			generatePath( ref toBeSaved );
			success = prefs.saveValue( toBeSaved, crypto, FileMode.Append);
		}
		return success;
	}

	public bool WriteToLocalFileSystem(DataObject toBeSaved, bool crypto = true) {
		bool success = true;
		if(toBeSaved == null) {
			success = false;
		} else {
			generatePath( ref toBeSaved );
			success = prefs.saveValue( toBeSaved, crypto, FileMode.Create );
		}
		return success;
	}

	public DataObject ReadFromLocalFileSystem(DataType curType, bool decrypto = true) {
		DataObject ob = null;
		ob = prefs.loadValue(generatePath(curType), generateType(curType), decrypto);
        if (ob != null)
        {
            ob.mType = curType;
        }
		return ob;
	}

	//we will generate storage path
	private void generatePath(ref DataObject toBeSaved) {
        string fileName = PreDefined[toBeSaved.mType].fileName;
        if (Utils.inArray<DataType>(toBeSaved.mType, NonAccountDataType)) {
            toBeSaved.Path = Path.Combine(baseNonAccountPath, fileName);
        } else {
            toBeSaved.Path = Path.Combine(baseAccountPath, fileName);
        }
	}

	private string generatePath( DataType curType) {
        string fileName = PreDefined[curType].fileName;
        string curPath = string.Empty;
        if (Utils.inArray<DataType>(curType, NonAccountDataType)){
            curPath = Path.Combine(baseNonAccountPath, fileName);
        } else {
            curPath = Path.Combine(baseAccountPath, fileName);
        }
		return curPath;
	}

	private Type generateType (DataType curType) {
        return PreDefined[curType].typeOfData;
	}

    void ICore.Dispose()
    {
        baseAccountPath = null;
        baseNonAccountPath = null;
        prefs = null;
    }

    void ICore.Reset()
    {
        baseAccountPath = string.Empty;
    }

	void ICore.OnLogin(Object path)
    {
		PathInfo info = path as PathInfo;
		if(info != null) {
			baseAccountPath = Path.Combine(baseNonAccountPath, info.UniqueId);
			if(!Directory.Exists(baseAccountPath)) 
				Directory.CreateDirectory(baseAccountPath);
			
			baseAccountPath = Path.Combine(baseAccountPath, info.curServer);
			if(!Directory.Exists(baseAccountPath))
				Directory.CreateDirectory(baseAccountPath);

		} else {
			throw new DragonException("DataPersisteManager OnLogin must get PathInfo Object.");
		}
    }
}

public class PathInfo {
	public string UniqueId;
	public string curServer;
}

