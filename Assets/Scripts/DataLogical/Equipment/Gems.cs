using System;

public class Gems {
	//Unique id
	public int id;
	public GemData configData;
	//装备上了吗
	public bool equipped;
	public bool isNew;

	public Gems () 
	{
		isNew = true;
		equipped = false;
	}

	public Gems (GemData config) { 
		configData = config;
		equipped = false;
		isNew = true;
	}

	public SlotOrGemsColor getColor {
		get {
			if(configData != null) 
				return configData.getColor;
			else 
				return SlotOrGemsColor.Default_No;
		}
	}

}

