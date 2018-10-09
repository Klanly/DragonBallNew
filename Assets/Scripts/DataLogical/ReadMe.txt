如何添加读取配置文件的方法：
1.在ConfigData.cs里面的ConfigType里面添加新的类型，
2.在ConfigData.cs里面Config里添加新的定义


背包数据：
1. 宠物背包- DataCore.MonsterManager.mBagOfMonster 
2. 装备背包- DataCore.EquipManager.BagOfEquips
3. 宝石背包- Datacore.GemsManager.BagOfGems
4. 宠物编队信息 - Player.myTeams
5. 物品背包- DataCore.ItemManager.BagOfItem

队伍里装备信息-都是在MonsterTeam里面读取

如何读取文本
DataCore.StringManager.getString()



DungeonseManager里面OnFinishFloor，当打完一个关卡成功后要调用。
DungeonseManager里面的FailToWar，当打完一个boss关卡失败后调用.