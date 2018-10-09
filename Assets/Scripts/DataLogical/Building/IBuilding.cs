using System;

/// <summary>
/// Interface of Building.
/// </summary>
public interface IBuilding {

	/// <summary>
	/// Constructs a new building.
	/// </summary>
	/// <returns>1 = means success. -1 = means player level limitation. -2 means Count limitation. -3 Coin not enough. -4 Stone not enough </returns>
	int constructNewBuilding ();

	/// <summary>
	/// How long is the construction going to complete.
	/// </summary>
	/// <returns>left timing, unix-time. -1 means no upgrade action.</returns>
	int getConstructCounting ();


	/// <summary>
	/// Upgrade the building level
	/// And we should start counting down
	/// </summary>
	/// <returns>1 = means success. -1 = means player level limitation. -2 means Count limitation. -3 Coin not enough. -4 Stone not enough </returns>
	int upgradeLevel ();

	/// <summary>
	/// How long is the upgrading going to complete.
	/// </summary>
	/// <returns>left timing, unix-time. -1 means no upgrade action.</returns>
	long getUpgradeCounting ();


	/// <summary>
	/// Can we enter this building?
	/// </summary>
	/// <returns><c>true</c>, if enter building was ok, <c>false</c> otherwise.</returns>
	bool enterBuilding ();


	/// <summary>
	/// Reconstruct building.
	/// </summary>
	/// <returns> 1 if we can construct a new building , 0 if building has goods, -1 if construct a new building is denied.</returns>
	int reconstructBuilding ();
}
	
public class Building {

	//这个不能为空
	public BaseBuildingData config;
	public BuildingTeamInfo RTData;
	public DateTime fTime = new DateTime();	//到期时间

	public Building() {}
}