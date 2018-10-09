using System;
using System.Collections.Generic;

public class DataCore : ICore {
	#region ICore implementation
	void ICore.Dispose ()
	{
		throw new NotImplementedException ();
	}
	void ICore.Reset ()
	{
		throw new NotImplementedException ();
	}
	void ICore.OnLogin (object obj)
	{
		throw new NotImplementedException ();
	}
	#endregion

	//All Data 
	private List<Manager> managerList = null;
	//临时数据保存的地方
	public TemporyData temper {
		get;
		set;
	}

	//招募
	public ZhaomuMgr zhaomuMgr
	{
		get;
		private set;
	}

	#region 获取各种Manager

	public SkillManager skillManager {
		get {
			return managerList[0] as SkillManager;
		}
	}

	public MonsterManager monManager {
		get {
			return managerList[1] as MonsterManager;
		}
	}
    
	public DungeonsManager dungeonsManager  {
		get {return managerList[2] as DungeonsManager;}
    }

	public PlayerManager playerManager {
		get {
			return managerList[3] as PlayerManager;
		}
	}

	public StringManager stringManager {
		get {
			return managerList[4] as StringManager;
		}
	}

	public FateManager fateManager {
		get {
			return managerList[5] as FateManager;
		}
	}

	public EquipManager EquipManager {
		get {
			return managerList[6] as EquipManager;
		}
	}

	public GemsManager gemsManager {
		get {
			return managerList[7] as GemsManager;
		}
	}

	public DragonManager dragonManager {
		get {
			return managerList[8] as DragonManager;
		}
	}

	public ItemManager itemManager {
		get {
			return managerList[9] as ItemManager;
		}
	}

	public BuildingManager BuildingManager {
		get {
			return managerList[10] as BuildingManager;
		}
	}
	public ActivityManager ActivityManager{
		get{ 
			return managerList [11] as ActivityManager;
		}
	}

	public DuiHuanManager DuiHuanManager{
		get{ 
			return managerList [12] as DuiHuanManager;
		}
	}

	public VipManager vipManager{
        get{
			return managerList [13] as VipManager;
        }
    }

//	public HonorItemManager honorItemManager{
//		get{ 
//			return managerList [14] as HonorItemManager;
//		}
//	}

	public FriendManager FriendManager{
		get{
            return managerList [14] as FriendManager;
		}
	}

    public MessageDataMgr MessageDataMgr{
        get{
            return managerList [15] as MessageDataMgr;
        }
    }

    public FinalTrialDataMgr FinalTrialDataMgr{
        get{
            return managerList[16] as FinalTrialDataMgr;
        }
    }

	public SoulManager soulManager {
		get {
            return managerList [17] as SoulManager;
		}
	}

	public UserConfigManager usrManager {
		get {
            return managerList[18] as UserConfigManager;
		}
	}
    
    public SoundManager soundManager {
		get {
            return managerList[19] as SoundManager;
		}
	}

	public SecretShopDataMgr secretShopDataMgr{
		get{
            return managerList[20] as SecretShopDataMgr;
		}
	}

    /*不在使用
    public VersionManager VersionMgr {
        get {
            return managerList[22] as VersionManager;
        }
    }*/
	
	public GuideManager guideManger
	{
		get
		{
            return managerList[21] as GuideManager;
		}
	}
	
	public ExtensionManager extensionManager
	{
		get
		{
            return managerList[22] as ExtensionManager;
		}
	}
	
	public SourceManager sourceManager
	{
		get
		{
            return managerList[23] as SourceManager;
		}
	}
    public NoticeManager noticManager{
        get{ 
            return managerList[24] as NoticeManager;
        }
    }
	
	public TaskManager taskManager
	{
		get
		{
			return managerList[25] as TaskManager;
		}
	}

	public GPSWarManager gpsWarManager
	{
		get
		{
			return managerList[26] as GPSWarManager;
		}
	}

	public GuaGuaLeMgr guaGuaLeMgr
	{
		get
		{
			return managerList[27] as GuaGuaLeMgr;
        }
    }

	public RechargeDataMgr rechargeDataMgr
	{
		get
		{
			return managerList[28] as RechargeDataMgr;
		}
	}
	
    public NewDungeonsManager newDungeonsManager
	{
		get
		{
			return managerList[29] as NewDungeonsManager;
		}
	}

	public DeblockingBuildMgr deblockingBuildMgr
	{
		get
		{
			return managerList[30] as DeblockingBuildMgr;
		}
	}

	public BattleTeamMgr battleTeamMgr
	{
		get
		{
			return managerList[31] as BattleTeamMgr;
		}
	}


    /// 
    /// 活动的数据层
    /// 
    private HolidayActivityData _holiday;
    public HolidayActivityData HolidayActivityManager
    {
        get
        {
            return _holiday ?? (_holiday = new HolidayActivityData());
        }
    }
    
    //这个AccountManager 很特殊必须在游戏选完区，登陆之后才能决定加载什么文件
    //所以必须在EventCenter里面的Login之后才初始化
    private AccountConfigManager _accinfo;
    public AccountConfigManager AccountMgr {
        get {
            return _accinfo ?? ( _accinfo = new AccountConfigManager() );
        }
    }

	///这个MovableNoticeManager必须等到服务返回信息
	private MovableNoticeManager _mNoticeMgr;
	public MovableNoticeManager MainNoticeMgr {
		get {
			return _mNoticeMgr ?? ( _mNoticeMgr = new MovableNoticeManager() );
		}
	}

	#endregion

	 public DataCore() {
		managerList = new List<Manager>();
		//.. to do list
		//add all managers here.

		managerList.Add(new SkillManager());
		managerList.Add(new MonsterManager());
		managerList.Add(new DungeonsManager());
		managerList.Add(new PlayerManager());
		managerList.Add(new StringManager());
		managerList.Add(new FateManager());
		managerList.Add(new EquipManager());
		managerList.Add(new GemsManager());
		managerList.Add(new DragonManager());
		managerList.Add(new ItemManager());
		managerList.Add(new BuildingManager());
		managerList.Add(new ActivityManager());
		managerList.Add(new DuiHuanManager());
		managerList.Add(new VipManager());
		managerList.Add(new FriendManager());
        managerList.Add(new MessageDataMgr());
        managerList.Add(new FinalTrialDataMgr());
		managerList.Add(new SoulManager ());
		managerList.Add(new UserConfigManager());
        managerList.Add(new SoundManager(usrManager, new AudioLoader()));
		managerList.Add(new SecretShopDataMgr());
	    managerList.Add(new GuideManager());
		managerList.Add(new ExtensionManager());
		managerList.Add(new SourceManager());
        managerList.Add(new NoticeManager());
		managerList.Add(new TaskManager());
		managerList.Add(new GPSWarManager());
		managerList.Add(new GuaGuaLeMgr());
		managerList.Add(new RechargeDataMgr());
		managerList.Add(new NewDungeonsManager());
		managerList.Add(new DeblockingBuildMgr());
		managerList.Add(new BattleTeamMgr());

		
		temper = new TemporyData();
		zhaomuMgr = new ZhaomuMgr ();
	}

	public void readLocalConfig() {
		if(managerList != null && managerList.Count > 0) 
		{
			try
			{
				foreach(Manager man in managerList) {
					bool success = man.loadFromConfig();
					if(!success) {
						ConsoleEx.DebugLog( man.GetType().ToString() + ".User may delete local config file.");
					}
				}
			} catch(Exception ex) {
				ConsoleEx.DebugLog(ex.ToString());
			}

		}
	}

	/// <summary>
	/// Gets the type of the data.根据ID来获得Data属于哪种类型的数据
	/// </summary>
	/// <returns>The data type.</returns>
	/// <param name="ID">I.</param>
	public static ConfigDataType getDataType(int ID){
		int Prefix = ID / 10000;

		if(Enum.IsDefined(typeof(ConfigDataType), Prefix)) {
			return (ConfigDataType)Prefix;
		} else {
			return ConfigDataType.Default_No;
		}
	}


	/// 
	/// 注销的时候，消除所有和账号相关的数据
	/// 
	public void Unregister() {
		temper   = new TemporyData();
		// wxl 
		zhaomuMgr = new ZhaomuMgr ();

		_holiday = new HolidayActivityData ();

		_accinfo = null;
	}
}
