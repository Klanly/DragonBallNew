using System;

/// <summary>
/// 奥义的定义，由服务器传过来
/// </summary>
public class RTAoYi {
	public int ppid;
	//奥义的num
	public int num;
	public int lv;
	//经验
	public int ep;
	//定义的位置，（战斗的时候有一个位置信息）从1开始
	public int wh;

	public RTAoYi()
	{

	}

	public RTAoYi(int num)
	{
		this.num = num;
	}
}

/// <summary>
/// 奥义的完成定义
/// </summary>
public class AoYi  {
	private AoYiData config;
	private RTAoYi RTData;

	public AoYi (RTAoYi runtime, DragonManager manager) {
		Utils.Assert(runtime == null || manager == null, "Invalidate argument in HugeBeast Class Constructor");

		RTData = runtime;
		config = manager.getAoYiData(RTData.num);

		Utils.Assert(config == null, "Aoyi Config data can't be initialized. AoYi Id = " + RTData.num.ToString());
	}

	public AoYi (RTAoYi runtime, AoYiData config) {

		this.RTData = runtime;
		this.config = config;
	}

	/// <summary>
	/// 奥义的Num号
	/// </summary>
	/// <value>The number.</value>
	public int Num {
		get {
			return RTData.num;
		}
	}

	/// <summary>
	/// 获取服务器的唯一ID
	/// </summary>
	/// <value>The I.</value>
	public int ID {
		get {
			return RTData.ppid;
		}
	}

	//获取位置(从0开始）
	public int Pos {
		get {
			return RTData.wh - 1;
		}
	}

	public RTAoYi RTAoYi
	{
		get
		{
			return RTData;
		}
	}

	public AoYiData AoYiDataConfig
	{
		get
		{
			return config;
		}
	}

}
