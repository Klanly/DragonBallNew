using UnityEngine;

/// <summary>
/// 时间的管理模块，目前应该只有战斗在使用
/// </summary>

public class TimeMgr {

	/// <summary>
	/// 战斗是否暂停
	/// </summary>
	private bool mWarPause = false;

	public bool WarPause {
		get {
			return mWarPause;
		} set {
			mWarPause = value;
		}
	}


	/// <summary>
	/// 记录下基础的时间线
	/// </summary>
	private float BaseFactor;

	/// <summary>
	/// 记录下额外的影响值
	/// </summary>
	private float extFactor;

	private static TimeMgr instance = null;

	public static TimeMgr getInstance() {
		return instance ?? (instance = new TimeMgr());
	}
	//默认构造函数
	public TimeMgr () { 
		BaseFactor = 1.0f; 
		extFactor  = 1.0f;
	}

	//重置变化速率
	public void Reset() {
		BaseFactor = 1.0f;
		extFactor  = 1.0f;
	}

	/// <summary>
	/// 设定基础的值
	/// </summary>
	/// <param name="scale">Scale.</param>
	public void setBaseLine(float scale) {
		BaseFactor     = scale;
		Time.timeScale = BaseFactor;
	}

	/// <summary>
	/// 设定额外的值
	/// </summary>
	/// <param name="ext">Ext.</param>
	public void setExtLine(float ext) {
		extFactor      = ext;
		//如果没有暂停功能
		if(!mWarPause)
			Time.timeScale = BaseFactor * extFactor;
	}

	/// <summary>
	/// 直接返回，而不设定Time
	/// </summary>
	/// <returns>The ext line.</returns>
	/// <param name="ext">Ext.</param>
	public float getExtLine(float ext) {
		return BaseFactor * ext;
	}

	/// <summary>
	/// 回归基线
	/// </summary>
	public void revertToBaseLine() {
		//如果没有暂停功能
		Time.timeScale = BaseFactor;
	}
}
