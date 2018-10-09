
//0、地球碎片 1、体力 2、精力3、钥匙4、金币兑换5、潜力6、龙珠7、奖励效果加成8、宠物9、兑换钻石10、扭蛋11、免战牌12、宝石13、幸运大奖14、钻石15、金币16、属性切换17、属性重置18、装备道具19、装备道具包20、碎片包21、VIP礼包22、娜美克碎片
public enum ItemType {
	Earth_Frage = 0x00,
	TiLi = 0x01,
	JingLi = 0x02,
	Key = 0x03,
	Coin_Exchange = 0x04,
	ChaKeLa = 0x5,
	Dragon_Ball = 0x06,
	Reward_Imp = 0x07,
	Monster = 0x08,
	Stone_Exchange = 0x09,
	Egg = 0x0A,
	No_War = 0x0B,
	Gems = 0x0C,
	Big_Reward = 0x0D,
	Stone = 0x0E,
	Coin = 0x0F,
	Property_Change = 0x10,
	Property_Reset = 0x11,
	Equipment = 0x12,
	Equipment_Bag = 0x13,
	Frage_Bag = 0x14,
	VIP_Bag = 0x15,
	Nameike_Frage = 0x16,
    Monster_Frage = 0x1C,
	Equip_Frage = 0x1D,
}

//在商店的位置
public enum ShopItemType {

	HotSale = 0x0,      //热卖
	Item = 0x1,     //扭蛋
	Egg = 0x2,  //道具
	Vip = 0x3,      //Vip
	Active = 0x4,      //活动
	
}

public class Item {
	//配置信息
	public ItemData configData;
	//运行的时候服务器给的信息
	public ItemInfo RtData;

	public bool isNew;
	public Item()
	{
		isNew = true;
	}

	public Item (ItemData config, ItemInfo info) {
		Utils.Assert(config == null,"ItemData can't be null when item initialized");
		Utils.Assert(info == null, "ItemInfo can't be null when item initialized");

		isNew = true;
		configData = config;
		RtData = info;
	}

}

