/*
 * Created By Allen Wu. All rights reserved.
 */

using System;


public class UserConfigData : DataObject
{
    //关闭声音 0 - 打开声音  1 - 关闭
	public short mute;
    //是否下载漫画  1- 下载 0 - 不下载
    public short cartoon;
    //小悟空缘是否第一次配齐   1 - 已经配齐过  0 没有配齐过 
    public short wukongfate;
	//add more...
    //下载总资源大小
    public float downAllSize;
    //当前下载资源大小
    public float downCurSize;
	// 是否安装 OBB文件   0 未安装  1 已安装
	public short isDownOBB;

	public UserConfigData() { }

	/// <summary>
	/// 添加默认的值
	/// </summary>
	public void DefaultValue() {
		mute = 0;
        cartoon = 1;
		isDownOBB = 0;
	}
}

/// <summary>
/// 用户的信息，只和APP有关系，和账号没有关系
/// 声音都是实例化的时候直接读取本地的配置
/// 下载漫画，小悟空的第一次满缘
/// </summary>
public class UserConfigManager : Manager {
	public const bool AES_ENCRYPT = true;

	private UserConfigData userInfo;
	private DataPersistManager persist;

	public UserConfigData UserConfig {
		get { return userInfo; } 
		set { userInfo = value; }
	}

	public UserConfigManager() {
		userInfo = null;
		persist = Core.DPM;

		userInfo = persist.ReadFromLocalFileSystem(DataType.USER_CONFIG, AES_ENCRYPT) as UserConfigData;
		if(userInfo == null) {
			userInfo = new UserConfigData();
			userInfo.DefaultValue();
		}

		userInfo.mType = DataType.USER_CONFIG;
	}

	public override bool loadFromConfig ()
	{
		return true;
	}

	public void save() {
		if (userInfo != null) {
			persist.WriteToLocalFileSystem(userInfo, AES_ENCRYPT);
		}
	}

}
