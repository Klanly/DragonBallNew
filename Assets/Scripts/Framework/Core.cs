using System;
using AW.Timer;
using AW.Resources;

public interface ICore
{
	void Dispose();
    void Reset();
	void OnLogin(Object obj);
}

public static class Core
{
    //Game States-Machine
    public static StateMachine SM;
    //Game Event-Center
    public static EventCenter EVC;
    //Persist Data Manager
    public static DataPersistManager DPM;
    //Network engine
    public static NetworkEngine NetEng;
	//Timer engine
    public static TimerMaster TimerEng;

	//AsynchTask engine
	public static AsyncTask AsyncEng;

	//Sound Manager engine;
	public static SoundEngine SoundEng;
    //Resource Download Engine
    public static ResourceManager ResEng;

	//Data core
	public static DataCore Data;

	/// <summary>
	/// Initialize this instance. 
	/// We must follow special sequnce.
	/// </summary>
	public static void Initialize() {
		//Initial sequnce

		//DataPersisteManager should be initialize first. and tell the non-account path
		Core.DPM = DataPersistManager.getInstance(DeviceInfo.PersistRootPath);
		//Timer should run.
        Core.TimerEng = new TimerMaster();
		//Sound manager.
		Core.SoundEng = SoundEngine.GetSingleton();
		//EventCenter must initialize later than Network Engine
		Core.NetEng = new NetworkEngine();
		//EventCenter also must initialize later than Aysnc Engine
		Core.AsyncEng = AsyncTask.Current;

		Core.EVC = new EventCenter();
		Core.EVC.RegisterToHttpEngine(Core.NetEng.httpEngine);
		Core.EVC.RegisterToSockEngine(Core.NetEng.SockEngine);

        ResEng = new ResourceManager();
        ResEng.RegisterAM(ManagerType.ModelManager, new SplitModelLoader());
		Core.Data = new DataCore();

		//registe some vars.
		HttpRequestFactory.swInfo = SoftwareInfo.VersionCode;
		HttpRequestFactory.platformId = SoftwareInfo.PlatformId;
	}

	/// <summary>
	/// Raises the pause event.
	/// </summary>
	public static void OnPause() {
		Core.NetEng.httpEngine.onPause();
		Core.NetEng.SockEngine.OnPause();
		Core.TimerEng.OnPause();
	}

}

