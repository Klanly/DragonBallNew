using System;

/**
 * All Const vars. should be defined here !!
 */ 
public sealed class Consts {

	public const bool FAILURE = false;
	public const bool OK = true;

    //User Data should be encrpty
    // password for AES
    public const string sharedSecret = "IWLLDOGOD";

    public const float _16Bi9 = 16.0f / 9.0f;
    public const float _4Bi3 = 4.0f / 3.0f;

	//get sell monster and equip price
	public static int GetSellPrice(int star)
	{
		int price = 0;
		switch(star)
		{
		case 1:
			price = 500;
			break;
		case 2:
			price =3000;
			break;
		case 3:
			price = 20000;
			break;
		case 4:
			price = 100000;
			break;
		}
		
		return price;
	}

    public const float oneHundred = 0.01f;
}

//宠物的属性定义
public enum MonsterAttribute {
	//无
	DEFAULT_NO = 0x0,
	//金
	GOLDEN = 0x1,
	//木
	WOOD = 0x2,
	//土
	SOIL = 0x3,
	//水
	WATER = 0x4,
	//火
	FIRE = 0x5,
	//全
	ALL = 0xA,

	//超金
	GOLDEN_S = 0xB,
	//超木
	WOOD_S = 0xC,
	//超土
	SOIL_S = 0xD,
	//超水
	WATER_S = 0xE,
	//超火
	FIRE_S = 0xF,
}