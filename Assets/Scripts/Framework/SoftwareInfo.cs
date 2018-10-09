using System;
using UnityEngine;

/* I hope this class is read value from local config file
 */ 

//added by zhangqiang
/// <summary>
/// 不同的渠道，不同的游戏名称
/// 根据游戏名字来确定选用不同的图片
/// </summary>
public enum GameName
{
	DragonBall,				//七龙珠			
	AllDragonBall,			//全民龙珠
	Saiyan3D,				//3d赛亚人
}

public class SoftwareInfo 
{
	/// <summary>
	/// The name of the version.
	/// </summary>
//    public const string VersionName = "3.0.1";
	public const int VersionCode = 1006;

	//每次打包前手动修改游戏名称value
	public const GameName gameName = GameName.AllDragonBall;

	//平台ID
	public const int PlatformId = -1;

	/// <summary>
	/// The APPSTONEUR.
	/// </summary>
    public static string APPSTONEURL;
    public static void JumpToAppstore()
    {
        Application.OpenURL(APPSTONEURL);
    }
}
