using System;
using System.Reflection;
using System.Collections.Generic;

public static class GuideLocalData {

    private static Dictionary<RequestType, Type> ReflectDefined = new Dictionary<RequestType, Type>() {
		{ RequestType.SM_PVE_BATTLE,                    typeof(BattleJsonFactory) },
		{ RequestType.PVE_BATTLE   ,                    typeof(BattleJsonFactory)},
		{ RequestType.ZHAOMU,                           typeof(ZhaoMuJsonFactory) },
		{ RequestType.CHANGE_TEAM_MEMBER,               typeof(ChangeTeamJsonFactory) },
		{ RequestType.CHANGE_EQUIPMENT,                 typeof(ChangeEquipJsonFactory) },
		{ RequestType.STRENGTHEN_MONSTER,               typeof(StrengthMonJsonFactory) },
		{ RequestType.BUILD_CREATED,                    typeof(CreateBuildJsonFactory) },
		{ RequestType.BUILD_GET,                        typeof(GetBuildJsonFactory)},
        { RequestType.GET_QIANGDUO_DRAGONBALL_OPPONENTS,typeof(Rob_SixStarBallPlayerList)},
        { RequestType.GET_SU_DI_LIST ,                  typeof(NemesisList)},
        { RequestType.QIANGDUO_DRAGONBALL_BATTLE,       typeof(Rob_SixStarBall)},
        { RequestType.CALL_DRAGON,                      typeof(CallOfDragon)},
		{ RequestType.LEARN_AOYI,                       typeof(LearnAOYI)},
		{ RequestType.EQUIP_AOYI,                       typeof(EquipAoYi)},
        { RequestType.LEVELGIFTSTATE,                   typeof(GetVirtualLevelState)},
        { RequestType.LEVELGIFT_REQUEST,                typeof(GetvirtualLevelReward)},
		{ RequestType.FIGHT_FULISA,                     typeof(FightWithFulisaJsonFactory)},
		{ RequestType.GET_TIANXIADIYI_OPPONENTS,        typeof(TianxiadiyiFake)},
		{ RequestType.GET_ZHANGONG_BUY_ITEM_ID,         typeof(DuihuanListFake)},
		{ RequestType.ZHANGONG_BUY_ITEM,                typeof(DuihuanFake)},
		{ RequestType.SEVENDAYREWARD,                   typeof(SevenDayRewardFake)},
		{ RequestType.SEVENDAYREWARD_BUY,               typeof(SevenDayRewardBuyFake)},
        { RequestType.FINALTRIAL_GETFINALLOGININFO,     typeof(DuoBaoFake)},
		{ RequestType.GET_CALLDRAGONISFINISH,           typeof(GetVirtualCallDragonIsFinish)},
		{ RequestType.STRENGTHEN_EQUIPMENT,             typeof(StrengthEquipFactory)},
		{ RequestType.GEM_INLAY_REMOVE,                 typeof(InlayGemFactory)},
        { RequestType.RECHARGECOUNT,                    typeof(GetMonthData)},
        { RequestType.HAVEDINNER_STATE,                 typeof(GetDinnerStateVirtualData)},
		{ RequestType.BUY_ITEM,                         typeof(GetTenStarEggData)},
		{ RequestType.Task_Reward,                      typeof(GetTaskReward)},
		{ RequestType.Task_List,                        typeof(GetTaskList)},
    };

    public static string HandleLocalHttpTask(HttpTask task) {
        string json = string.Empty;

        if(task != null) {
            try {
				
				ConsoleEx.DebugLog(task.relation.requetType.ToString() + "    " + ReflectDefined[task.relation.requetType].ToString());
				
                JsonFactory factory = Activator.CreateInstance( ReflectDefined[task.relation.requetType] ) as JsonFactory;
                json = factory.generateLocalJson(task);
            } catch(Exception ex) {
				ConsoleEx.DebugLog("Request : " + task.relation.requetType.ToString() + "\n" + ex.ToString());
            }
        }

        return json;
    }

}


public abstract class JsonFactory {
    protected Dictionary<int, string> PreDefined;
    protected string DefaultJson;

    public abstract string generateLocalJson(HttpTask task);
}


public class BattleJsonFactory : JsonFactory {

    public BattleJsonFactory () {
		
		
		RED.Log(CityFloorData.Instance.currFloor.config.ID.ToString());
		
			
		if(CityFloorData.Instance.currFloor != null)
		   RED.Log(CityFloorData.Instance.currFloor.config.ID.ToString());
		else
			RED.Log("CityFloorData.Instance.currFloor == null");
        //Key is 小关卡ID
        PreDefined = new Dictionary<int, string>() 
		{	
			{60101, @"{""status"":1,""act"":103,""data"":{""sync"":{""eny"":59,""pwr"":""10"",""stone"":""0"",""rank"":-1,""ep"":58,""lv"":""1"",""vip"":""0""},""battleData"":{""isfirst"":0,""ak"":null,""df"":null,""rsmg"":null,""rsty"":null},""reward"":{""bep"":4,""bco"":150,""eep"":0,""eco"":150,""np"":[],""p"":[],""ext"":null}} }" },
			
			{60105, @"{""status"":1,""act"":103,""data"":{""sync"":{""eny"":60,""pwr"":""10"",""stone"":0,""rank"":-1,""ep"":0,""lv"":""2"",""vip"":""0""},""battleData"":{""isfirst"":0,""ak"":2746,""df"":2031,""rsmg"":""rVRNb5wwEP0vPhPJX0CWU1erHvbUQ3OpohwcMFsUahCYbVdR\/nvH5sNmux+ulJtnPMx78+aZdyS0fpLiF8rgqA7daac0ynCEtE0+v6OqQBnDnJMIqQFSBBNOI9R2TSs7fUJZHKFaHmUNV1Dy\/a2q4fhMCYnjlwiJJRFjlkIiH7qtBgwWM+hylHtVyD8WUtR1KbQEPPwRLcBp4oBpHARMkzNgyjxgutn4wGQNDIVdU8s9BJskJhxA9KmVlhMqZHlfLOz4plf4shVfSjY+X48qYYlPlV7UKACPh+LxRx+PXZUGz6oQUAUstG1+VGMrkMgFvRZ66FH2QMzF63DYq7IxVx9frPNE\/iY72w0+k6qX44zTxc53ylSw85WZvLs1q5i7rBMzAXwD\/6vKf1obTBBLHMxjRqEXUPqhLKcZRVHYbx6MjZdnQEHyHCQ+NIYztOjNXozIdlNT\/K2167Dnp1H5sccyai56bYCo48PxLUZjYactDQ5AZaVEvUQzwccU1i7z5ih9sKbXLmo76fOYZyGrWeB34M1Cg2bB\/izktrpuFsPYzWKi5embv9jcMQ7V5no\/fLvbZYezf51leZ07y7zFOw4nn+zwqzz+1+HEF53yT3A4ca5goQ5fucLO4dZow5limoTZgoU2vGOM35VSpp9ZAHKl6XnpXw=="",""rsty"":""i9Y11DHQMdIxMQBhQx1TIITxQWzzWAA=""},""reward"":{""bep"":20,""bco"":2250,""eep"":0,""eco"":0,""np"":[45144,10172],""p"":[{""ppid"":""-5"",""pid"":40110,""num"":1,""lv"":1,""ep"":0,""slotc"":[2,2]},],""ext"":{""zg"":null,""coin"":null,""p"":null}}}}"},
		
			{60106, @"{""status"":1,""act"":103,""data"":{""sync"":{""eny"":59,""pwr"":""10"",""stone"":""0"",""rank"":-1,""ep"":58,""lv"":""1"",""vip"":""0""},""battleData"":{""isfirst"":0,""ak"":null,""df"":null,""rsmg"":null,""rsty"":null},""reward"":{""bep"":4,""bco"":150,""eep"":0,""eco"":0,""np"":[],""p"":[],""ext"":null}} }" },		
		
			{60110, @"{""status"":1,""act"":103,""data"":{""sync"":{""eny"":60,""pwr"":""10"",""stone"":0,""rank"":-1,""ep"":0,""lv"":""3"",""vip"":""0""},""battleData"":{""isfirst"":0,""ak"":2746,""df"":2031,""rsmg"":""rVRNb5wwEP0vPhPJX0CWU1erHvbUQ3OpohwcMFsUahCYbVdR\/nvH5sNmux+ulJtnPMx78+aZdyS0fpLiF8rgqA7daac0ynCEtE0+v6OqQBnDnJMIqQFSBBNOI9R2TSs7fUJZHKFaHmUNV1Dy\/a2q4fhMCYnjlwiJJRFjlkIiH7qtBgwWM+hylHtVyD8WUtR1KbQEPPwRLcBp4oBpHARMkzNgyjxgutn4wGQNDIVdU8s9BJskJhxA9KmVlhMqZHlfLOz4plf4shVfSjY+X48qYYlPlV7UKACPh+LxRx+PXZUGz6oQUAUstG1+VGMrkMgFvRZ66FH2QMzF63DYq7IxVx9frPNE\/iY72w0+k6qX44zTxc53ylSw85WZvLs1q5i7rBMzAXwD\/6vKf1obTBBLHMxjRqEXUPqhLKcZRVHYbx6MjZdnQEHyHCQ+NIYztOjNXozIdlNT\/K2167Dnp1H5sccyai56bYCo48PxLUZjYactDQ5AZaVEvUQzwccU1i7z5ih9sKbXLmo76fOYZyGrWeB34M1Cg2bB\/izktrpuFsPYzWKi5embv9jcMQ7V5no\/fLvbZYezf51leZ07y7zFOw4nn+zwqzz+1+HEF53yT3A4ca5goQ5fucLO4dZow5limoTZgoU2vGOM35VSpp9ZAHKl6XnpXw=="",""rsty"":""i9Y11DHQMdIxMQBhQx1TIITxQWzzWAA=""},""reward"":{""bep"":20,""bco"":2250,""eep"":0,""eco"":0,""np"":[45144,10172],""p"":[{""ppid"":887955,""pid"":10174,""num"":1,""at"":5,""lv"":1,""ep"":0,""ak"":0,""df"":0}],""ext"":{""zg"":null,""coin"":null,""p"":null}}}}"},
		
			{60111, @"{""status"":1,""act"":103,""data"":{""sync"":{""eny"":59,""pwr"":""10"",""stone"":""0"",""rank"":-1,""ep"":58,""lv"":""1"",""vip"":""0""},""battleData"":{""isfirst"":0,""ak"":null,""df"":null,""rsmg"":null,""rsty"":null},""reward"":{""bep"":4,""bco"":150,""eep"":0,""eco"":150,""np"":[],""p"":[],""ext"":null}} }" },
			
			{60115, @"{""status"":1,""act"":103,""data"":{""sync"":{""eny"":60,""pwr"":""10"",""stone"":0,""rank"":-1,""ep"":0,""lv"":""4"",""vip"":""0""},""battleData"":{""isfirst"":0,""ak"":2746,""df"":2031,""rsmg"":""rVRNb5wwEP0vPhPJX0CWU1erHvbUQ3OpohwcMFsUahCYbVdR\/nvH5sNmux+ulJtnPMx78+aZdyS0fpLiF8rgqA7daac0ynCEtE0+v6OqQBnDnJMIqQFSBBNOI9R2TSs7fUJZHKFaHmUNV1Dy\/a2q4fhMCYnjlwiJJRFjlkIiH7qtBgwWM+hylHtVyD8WUtR1KbQEPPwRLcBp4oBpHARMkzNgyjxgutn4wGQNDIVdU8s9BJskJhxA9KmVlhMqZHlfLOz4plf4shVfSjY+X48qYYlPlV7UKACPh+LxRx+PXZUGz6oQUAUstG1+VGMrkMgFvRZ66FH2QMzF63DYq7IxVx9frPNE\/iY72w0+k6qX44zTxc53ylSw85WZvLs1q5i7rBMzAXwD\/6vKf1obTBBLHMxjRqEXUPqhLKcZRVHYbx6MjZdnQEHyHCQ+NIYztOjNXozIdlNT\/K2167Dnp1H5sccyai56bYCo48PxLUZjYactDQ5AZaVEvUQzwccU1i7z5ih9sKbXLmo76fOYZyGrWeB34M1Cg2bB\/izktrpuFsPYzWKi5embv9jcMQ7V5no\/fLvbZYezf51leZ07y7zFOw4nn+zwqzz+1+HEF53yT3A4ca5goQ5fucLO4dZow5limoTZgoU2vGOM35VSpp9ZAHKl6XnpXw=="",""rsty"":""i9Y11DHQMdIxMQBhQx1TIITxQWzzWAA=""},""reward"":{""bep"":20,""bco"":2250,""eep"":0,""eco"":0,""np"":[45144,10172],""p"":[{""ppid"":887955,""pid"":10174,""num"":1,""at"":5,""lv"":1,""ep"":0,""ak"":0,""df"":0}],""ext"":{""zg"":null,""coin"":null,""p"":null}}}}"},
		
		};
		
		
    }

    public override string generateLocalJson (HttpTask task) {

        string json = string.Empty;
		
	
        HttpRequest req = task.request as HttpRequest;
        if(req != null) {
            BattleParam param = req.ParamMem as BattleParam;
            if( param != null) {
                json = PreDefined[param.doorId];
            }

        }
		
	    
        return json;
    }
}


public class FightWithFulisaJsonFactory : JsonFactory , ICloneable {
    #region ICloneable implementation
    public object Clone ()
    {
        throw new NotImplementedException ();
    }
    #endregion

    public FightWithFulisaJsonFactory () {
        //Key is 小关卡ID
        PreDefined = new Dictionary<int, string>() {
			{-1,@"{""status"":1,""act"":103,""data"":{""sync"":{""eny"":60,""pwr"":""10"",""stone"":0,""rank"":-1,""ep"":0,""lv"":""1"",""vip"":""0""},""battleData"":{""isfirst"":0,""ak"":2746,""df"":2031,""rsmg"":""rZTBbtswDIbfRWevEK04iX1aEOwQYMMO7aUoelBtOQuqyYYspw0Cv3slWbblNs7cbTdRpEh+\/AmdUaWoqiuUfIEAZeyp3u9EXqDk4TFAVKlNcX9ojYzlg6E9d4z+RskZUbGXp61QKMEBUvby4YzE7fOBc3uszGmXoSQEiKKgtb+zI9NuaIJRAF6R9wGm2qVkESarC7FpLTdKNxNi3Q53jgCVsiiZVCeU6A4o5zlVLLMti1p3DBgWoY46sp3I2Kt2mFzqVDIbIwvObNFluFrjxg7jb\/jxmvyBP17O519e5ScT\/DDBHxOfHwZ+GPhx03w9G\/Fp+sykfd4ZW3\/uejxMVGzr9+JWZmPGZVVwM3QXoQ7pVhF\/2ESX0PTlNfBNpL\/01Q1Ew9vwQkvkY0sQR\/1dmwbfrKN3dU2tqs5zgwq2hFT2daQnnx8E5b3VjR0WeoySpcWRSUemi6dFpQbwUjLPpYXYF7IV5uJ2\/yy1D2Jn3XU7aSr2WX7UrcwprZQTphvIAoPPgX2OGPscMfY58HUOo9c8kOW\/gMAkyEgQEvkgxupB8Po6yEwO7+8yHOGIAj4lx2jLRM35pD5kpA\/x9QH8f7CGL+nzWDBvy6wEXeBIKOvpf6xJdWcmwDaBC9NL93IQwuQyHwJq3gA="",""rsty"":""i9Y11DHQMdIxMTBEwaZAaB4LAA==""},""reward"":{""bep"":0,""bco"":0,""eep"":0,""eco"":0,""np"":[],""p"":[]},""ext"":{""zg"":null,""coin"":null,""p"":null}}}"},
		};
    }

    public override string generateLocalJson (HttpTask task) {

        string json = string.Empty;
		
	
        HttpRequest req = task.request as HttpRequest;
        if(req != null) {
            BattleParam param = req.ParamMem as BattleParam;
            if( param != null) {
                json = PreDefined[param.doorId];
            }

        }
		
	    
        return json;
    }
}






public class ZhaoMuJsonFactory : JsonFactory
{

	public ZhaoMuJsonFactory()
	{
#if NewGuide
		DefaultJson = @" {""status"":1,""act"":502,""data"":{""p"":[{""ppid"":-1,""pid"":10142,""at"":5,""num"":1,""lv"":1,""ep"":0,""ak"":0,""df"":0}],""co"":0,""so"":0,""status"":{""pron"":51003,""state"":1,""totalcount"":1,""freecount"":""1"",""coolTime"":172800,""spGetCnt"":8}}}";
#else
		DefaultJson = @" {""status"":1,""act"":502,""data"":{""p"":[{""ppid"":-1,""pid"":10140,""at"":3,""num"":1,""lv"":1,""ep"":0,""ak"":0,""df"":0}],""co"":0,""so"":0,""status"":{""pron"":51003,""state"":1,""totalcount"":1,""freecount"":""1"",""coolTime"":172800,""spGetCnt"":8}}}";
#endif
	}

	public override string generateLocalJson (HttpTask task) 
	{
		return DefaultJson;
	}
}

public class ChangeTeamJsonFactory : JsonFactory
{
	public ChangeTeamJsonFactory()
	{
		DefaultJson = @" {""status"":1,""act"":102,""data"":true}";
	}

	public override string generateLocalJson (HttpTask task) 
	{
		return  DefaultJson;
	}
}

public class ChangeEquipJsonFactory : JsonFactory
{
	public ChangeEquipJsonFactory()
	{
		DefaultJson = @" {""status"":1,""act"":104,""data"":true}";
	}

	public override string generateLocalJson (HttpTask task) 
	{
		return  DefaultJson;
	}
}

public class StrengthMonJsonFactory : JsonFactory
{
	public StrengthMonJsonFactory()
	{
		DefaultJson = @"{""status"":1,""act"":131,""data"":{""coin"":-2400,""lv"":2,""ep"":1}}";
	}

	public override string generateLocalJson (HttpTask task) 
	{
		return  DefaultJson;
	}
}
public class CreateBuildJsonFactory : JsonFactory
{
	public CreateBuildJsonFactory()
	{
		DefaultJson = @"{""status"":1,""act"":511,""data"":{""bd"":{""num"":""830001"",""id"":""154"",""lv"":""1"",""type"":""1"",""dur"":[0,0],""robc"":""0"",""getc"":[18000,30],""openType"":""1""}}}";
	}

	public override string generateLocalJson (HttpTask task) 
	{
		return  DefaultJson;
	}
}

public class GetBuildJsonFactory : JsonFactory
{

	private  string GetBuildNormal = @"{""status"":1,""act"":512,""data"":{""num"":""830001"",""id"":""154"",""lv"":""1"",""type"":""1"",""dur"":0,""robc"":""0"",""coin"":18000,""stone"":10,""openType"":""0""}}";
//	private  string GetBuildNow =@"{""status"":1,""act"":512,""data"":{""bd"":{""num"":""830001"",""id"":""154"",""lv"":""1"",""type"":""1"",""dur"":[10800,0],""robc"":""0"",""getc"":[18000,0]}}}";


	public GetBuildJsonFactory()
	{
		DefaultJson = GetBuildNormal;
//		PreDefined = new Dictionary<int, string>()
//		{
//			{1, GetBuildNormal},
//			{2, GetBuildNow},
//		};
	}
	
	public override string generateLocalJson (HttpTask task) 
	{
//		string json = string.Empty;
//		
//		HttpRequest req = task.request as HttpRequest;
//		if(req != null) 
//		{
//			BuildGetParam param = req.ParamMem as BuildGetParam;
//			if( param != null) 
//			{
//				json = PreDefined[param.type];
//			}
//		}
//		
//		return json;

		return DefaultJson;
	}
}

//显示拥有六星龙珠的机器人
public class Rob_SixStarBallPlayerList: JsonFactory
{
	public Rob_SixStarBallPlayerList()
	{
	    DefaultJson = @"{""status"":1,""act"":153,""data"":[{""p"":[10142],""n"":""223461tony"",""lv"":3,""r"":0,""g"":223461,""htm"":0}]}";
	}
	public override string generateLocalJson (HttpTask task) 
	{
		return  DefaultJson;
	}
}


//抢夺六星龙珠
public class Rob_SixStarBall : JsonFactory
{
	public Rob_SixStarBall()
	{
		DefaultJson = @"{""status"":1,""act"":155,""data"":{""rsty"":""i9Y11DHQMdIxBULzWAA="",""rsmg"":""rZJNU4MwEIb\/y57jDEmgsZxkGA8968XpeIgQKlMMTAjVDtP\/7gb5iI62PXhjv97n3Q09SGsflXyDuAepd+aYagtxQMAOyW0PZQ5xeEtAdxjTgIaMQGPqRhl7xAqBSh1UhSVsediXFX5uGQ0EfyYg50QUcIGJrDOJRQCNsL05qI3O1QfyTmQErReQiHwQ\/QNE16EP8hhi5SPoCUumrtTGYXjEBe5kj40a6JCr4vIZKP\/vM9DQ98h8j4zxcEUnjxQ94lMl9VP5tSUaXoLWStu1EN9QV3jpdhtd1K50uuvdmMz2ygz74JjSLQoyMhdS\/1HGhtR3OP4jiTvMpPI9MRkIzvDvdfY63GdEzPHVPiYK+4XSdkUx7ohtxi5TRalltYTT7blYBKMzguxaweC83nuptdNz68LSKn62fgI="",""boss"":{""p"":[{""ppid"":-5,""pid"":110082,""num"":1,""lv"":1,""ep"":0,""ak"":0,""df"":0}],""np"":[45100,40100]},""ext"":{""p"":[{""ppid"":-6,""pid"":150006,""num"":1}],""hittms"":3,""pwr"":-1,""df"":114}}}";
	}

	public override string generateLocalJson (HttpTask task) 
	{
		return  DefaultJson;
	}
}

//请求宿敌列表
public class NemesisList :JsonFactory
{
	public NemesisList()
	{
	   DefaultJson= @"{""status"":1,""act"":141,""data"":null}";
	}
	
	public override string generateLocalJson (HttpTask task) 
	{
		return  DefaultJson;
	}
}

public class CallOfDragon:JsonFactory
{
	public CallOfDragon()
	{
		DefaultJson=@"{""status"":1,""act"":150,""data"":{""st"":1402298475,""du"":5,""lv"":1,""ep"":1}}";
	}
	
	public override string generateLocalJson (HttpTask task) 
	{
		return  DefaultJson;
	}
}

public class LearnAOYI:JsonFactory
{
	public LearnAOYI()
	{
        DefaultJson=@"{""status"":1,""act"":151,""data"":{""ppid"":159,""num"":270001,""lv"":1,""ep"":0,""wh"":0}}";
	}
	
	public override string generateLocalJson (HttpTask task) 
	{
		return  DefaultJson;
	}
}

public class EquipAoYi:JsonFactory
{
	public EquipAoYi()
	{
		DefaultJson=  @"{""status"":1,""act"":152,""data"":true}";
	}
	
	public override string generateLocalJson (HttpTask task) 
	{
		return  DefaultJson;
	}
}



public class GetVirtualLevelState:JsonFactory{
		public GetVirtualLevelState(){
				DefaultJson = @"{""status"":1,""act"":551,""data"":null}";
		}
		public override string generateLocalJson(HttpTask task){
				return DefaultJson;
		
		}
}
//{"status":1,"act":552,"data":[{"ppid":3396,"pid":45108,"num":1,"lv":1,"ep":0,"slotc":[2,2]},{"ppid":31074,"pid":10140,"num":1,"at":5,"lv":1,"ep":0,"ak":0,"df":0},{"ppid":31075,"pid":19998,"num":1,"at":1,"lv":1,"ep":0,"ak":0,"df":0},{"ppid":31076,"pid":19998,"num":1,"at":5,"lv":1,"ep":0,"ak":0,"df":0},{"ppid":31077,"pid":19998,"num":1,"at":3,"lv":1,"ep":0,"ak":0,"df":0}]}
public class GetvirtualLevelReward:JsonFactory{ 
		public GetvirtualLevelReward(){
		    DefaultJson = @"{""status"":1,""act"":552,""data"":[{""ppid"":-10,""pid"":45108,""num"":1,""lv"":1,""ep"":0,""slotc"":[2,2]},{""ppid"":-11,""pid"":10140,""num"":1,""at"":5,""lv"":1,""ep"":0,""ak"":0,""df"":0},{""ppid"":-12,""pid"":19998,""num"":1,""at"":1,""lv"":1,""ep"":0,""ak"":0,""df"":0},{""ppid"":-13,""pid"":19998,""num"":1,""at"":5,""lv"":1,""ep"":0,""ak"":0,""df"":0},{""ppid"":-14,""pid"":19998,""num"":1,""at"":3,""lv"":1,""ep"":0,""ak"":0,""df"":0}]}";
		}
		public override string generateLocalJson(HttpTask task){
				return DefaultJson;
		}
}


//四选一界面
public class DuoBaoFake : JsonFactory
{
	public DuoBaoFake()
	{
#if NewGuide
		DefaultJson = @"{""status"":1,""act"":305,""data"":{""rankStatus"":{""rank"":2391,""userzg"":21,""recozg"":20,""yetcount"":0,""totalcount"":10},""robStatus"":{""score"":0,""robcoins"":0},""pvpStatus"":{""ball"":{""type"":1,""startTime"":1415865172,""endTime"":1415865172,""count"":10,""passCount"":0,""failCount"":0,""buyTime"":0,""needStone"":10},""rank"":{""type"":2,""startTime"":1415864582,""endTime"":1415864582,""count"":5,""passCount"":0,""failCount"":0,""buyTime"":0,""needStone"":10},""robs"":null,""revenge"":{""type"":4,""startTime"":1415864582,""endTime"":1415864582,""count"":5,""passCount"":0,""failCount"":0,""buyTime"":0,""needStone"":10}},""atkStatus"":{""historyBest"":0,""todayBest"":0,""yetCount"":0,""totalCout"":0,""currRank"":0},""defStatus"":{""historyBest"":0,""todayBest"":0,""yetCount"":0,""totalCout"":0,""currRank"":0}}}";
#else
		DefaultJson = @"{""status"":1,""act"":305,""data"":{""rankStatus"":{""rank"":1194,""userzg"":42777,""recozg"":24,""yetcount"":0,""totalcount"":20},""robStatus"":{""score"":0,""robcoins"":8400},""pvpStatus"":{""ball"":{""type"":1,""startTime"":0,""endTime"":0,""count"":10,""passCount"":0,""failCount"":0,""buyTime"":0,""needStone"":10,""coolTime"":0},""rank"":{""type"":2,""startTime"":0,""endTime"":0,""count"":10,""passCount"":0,""failCount"":0,""buyTime"":0,""needStone"":10,""coolTime"":0},""robs"":{""type"":3,""startTime"":0,""endTime"":0,""count"":10,""passCount"":0,""failCount"":0,""buyTime"":0,""needStone"":10,""coolTime"":0},""revenge"":{""type"":4,""startTime"":0,""endTime"":0,""count"":10,""passCount"":0,""failCount"":0,""buyTime"":0,""needStone"":10,""coolTime"":0}},""atkStatus"":{""historyBest"":0,""todayBest"":0,""yetCount"":0,""totalCout"":0,""currRank"":0},""defStatus"":{""historyBest"":0,""todayBest"":0,""yetCount"":0,""totalCout"":0,""currRank"":0}}}";
#endif
	
	}
	
	public override string generateLocalJson (HttpTask task) 
	{
		return  DefaultJson;
	}
}

	//天下第一挑战
public class TianxiadiyiFake : JsonFactory
{
	public TianxiadiyiFake()
	{
		DefaultJson =  @"{""status"":1,""act"":300,""data"":{""rank"":268,""userzg"":9709000,""yetcount"":0,""totalcount"":19,""roles"":[{""g"":262495,""r"":135,""n"":""\u5415\u75be"",""lv"":6,""vipLv"":1,""p"":[{""num"":10142,""star"":0},{""num"":10125,""star"":0}],""zg"":null,""c"":210800,""eyid"":0,""htm"":0,""hi"":0},{""g"":280293,""r"":147,""n"":""\u8363\u5ae3"",""lv"":7,""vipLv"":1,""p"":[{""num"":10142,""star"":0},{""num"":10125,""star"":0},{""num"":10140,""star"":0}],""zg"":null,""c"":534500,""eyid"":0,""htm"":0,""hi"":0},{""g"":327866,""r"":189,""n"":""\u53f8\u9a6c\u601c\u68a6"",""lv"":50,""vipLv"":5,""p"":[{""num"":10142,""star"":0}],""zg"":null,""c"":200000,""eyid"":0,""htm"":0,""hi"":0}]}}";
	}
	
	public override string generateLocalJson (HttpTask task) 
	{
		return  DefaultJson;
	}
}

	//兑换界面
public class DuihuanListFake : JsonFactory
{
	public DuihuanListFake()
	{
		DefaultJson =  @"{""status"":1,""act"":3002,""data"":{""refreshMaxTimes"":3,""refreshMoney"":[0,100],""surplusRefreshTimes"":3,""item"":[{""id"":1,""price"":1000,""rank"":0,""num"":1,""des"":""\u6d88\u80171000\u6218\u529f\u53ef\u5151\u6362"",""pid"":110032,""canBuy"":true},{""id"":2,""price"":10000,""rank"":0,""num"":10,""des"":""\u6d88\u801710000\u6218\u529f\u53ef\u5151\u6362"",""pid"":110032,""canBuy"":true},{""id"":3,""price"":50000,""rank"":0,""num"":50,""des"":""\u6d88\u801750000\u6218\u529f\u53ef\u5151\u6362"",""pid"":110032,""canBuy"":true},{""id"":11,""price"":10000,""rank"":0,""num"":1,""des"":""\u6d88\u801710000\u6218\u529f\u53ef\u5151\u6362\u3002"",""pid"":110048,""canBuy"":true},{""id"":12,""price"":100000,""rank"":0,""num"":10,""des"":""\u6d88\u8017100000\u6218\u529f\u53ef\u5151\u6362\u3002"",""pid"":110048,""canBuy"":true}],""status"":[],""glorynum"":245456}}";
	}
	
	public override string generateLocalJson (HttpTask task) 
    {
        return  DefaultJson;
	}
}

	//兑换购买
public class DuihuanFake : JsonFactory
{
	public DuihuanFake()
	{
		DefaultJson = @"{""status"":1,""act"":3003,""data"":{""zg"":-1000,""p"":[{""ppid"":8074,""pid"":110032,""num"":1}],""canBuy"":false}}";
	}
	
	public override string generateLocalJson (HttpTask task) 
    {
        return  DefaultJson;
	}
}

//seven day reward
public class SevenDayRewardFake : JsonFactory
{
    string DefautMsg = @"{""status"":1,""act"":802,""data"":{""index"":0,""canGain"":true,""awads"":[{""day"":2,""reward"":[[10133,1],[19998,5],[110015,2],[110021,3]]},{""day"":3,""reward"":[[110056,3],[110015,2],[110021,3],[10140,1]]},{""day"":4,""reward"":[[110020,1],[110023,3],[110032,10],[10140,1]]},{""day"":5,""reward"":[[45147,1],[19998,2],[110019,2],[10140,1]]},{""day"":6,""reward"":[[110062,5],[110020,1],[110015,1],[10140,1]]},{""day"":7,""reward"":[[10107,1],[19999,3],[110032,20],[10140,1]]},{""day"":1,""reward"":[[110015,5],[19998,2],[110057,5],[110019,2]]}]}}";
    string FiveLevelMsg = @"{""status"":1,""act"":802,""data"":{""index"":1,""canGain"":false,""awads"":[{""day"":2,""reward"":[[10133,1],[19998,5],[110015,2],[110021,3]]},{""day"":3,""reward"":[[110056,3],[110015,2],[110021,3],[10140,1]]},{""day"":4,""reward"":[[110020,1],[110023,3],[110032,10],[10140,1]]},{""day"":5,""reward"":[[45147,1],[19998,2],[110019,2],[10140,1]]},{""day"":6,""reward"":[[110062,5],[110020,1],[110015,1],[10140,1]]},{""day"":7,""reward"":[[10107,1],[19999,3],[110032,20],[10140,1]]},{""day"":1,""reward"":[[110015,5],[19998,2],[110057,5],[110019,2]]}]}}";
    public SevenDayRewardFake()
    { 
        DefaultJson = DefautMsg;
    
    }
	
	public override string generateLocalJson (HttpTask task) 
    {
        if (Core.Data.playerManager.Lv == 1)
            DefaultJson = DefautMsg;
        else if (Core.Data.playerManager.Lv == 5)
            DefaultJson = FiveLevelMsg;

        return  DefaultJson;
	}
}

//seven day reward buy
public class SevenDayRewardBuyFake : JsonFactory
{
	public SevenDayRewardBuyFake()
	{
		DefaultJson = @"{""status"":1,""act"":803,""data"":{""p"":[{""ppid"":110897,""pid"":110015,""num"":5},{""ppid"":48050,""pid"":19998,""num"":1,""at"":1,""lv"":1,""ep"":0,""ak"":0,""df"":0},{""ppid"":48051,""pid"":19998,""num"":1,""at"":1,""lv"":1,""ep"":0,""ak"":0,""df"":0},{""ppid"":110898,""pid"":110057,""num"":5},{""ppid"":110899,""pid"":110019,""num"":2}]}}";	
	}
	
	public override string generateLocalJson (HttpTask task) 
    {
        return  DefaultJson;
	}
}

//call dragon is complete
public class GetVirtualCallDragonIsFinish:JsonFactory{
    public GetVirtualCallDragonIsFinish(){
        DefaultJson = @"{""status"":1,""act"":157,""data"":true}";
    }

    public override string generateLocalJson (HttpTask task) 
    {
        return  DefaultJson;
    }
}

public class StrengthEquipFactory : JsonFactory
{
	public StrengthEquipFactory()
	{
		DefaultJson = @"{""status"":1,""act"":105,""data"":{""coin"":-400,""lv"":2,""ep"":6}}";
	}

	public override string generateLocalJson (HttpTask task) 
	{
		return  DefaultJson;
	}
}

public class InlayGemFactory : JsonFactory
{
	public InlayGemFactory()
	{
		//DefaultJson = @"{""status"":1,""act"":105,""data"":{""coin"":-400,""lv"":2,""ep"":6}}";
		DefaultJson =@"{""status"":1,""act"":201,""data"":true}";
	}

	public override string generateLocalJson (HttpTask task) 
	{
		return  DefaultJson;
	}
}

public class GetDinnerStateVirtualData:JsonFactory{
    public GetDinnerStateVirtualData()
    {
        //DefaultJson = @"{""status"":1,""act"":105,""data"":{""coin"":-400,""lv"":2,""ep"":6}}";
        DefaultJson =@"{""status"":1,""act"":800,""data"":{""dinner"":{""dur"":[12,14],""isopen"":false,""vigor"":""100""},""stat"":true}}";
    }


    public override string generateLocalJson (HttpTask task) 
    {
        return  DefaultJson;
    }

}


public class GetMonthData:JsonFactory{
    public GetMonthData()
    {
        //DefaultJson = @"{""status"":1,""act"":105,""data"":{""coin"":-400,""lv"":2,""ep"":6}}";
        DefaultJson =@"{""status"":1,""act"":93,""data"":{""buyCounts"":[]}}";
    }


    public override string generateLocalJson (HttpTask task) 
    {
        return  DefaultJson;
    }

}

public class GetTenStarEggData:JsonFactory{
	public GetTenStarEggData()
	{
		DefaultJson =@"{""status"":1,""act"":120,""data"":{""p"":[{""ppid"":207752,""pid"":110025,""num"":1}],""stone"":350,""Result"":{""p"":[{""ppid"":-4,""pid"":10218,""at"":3,""num"":1,""lv"":1,""ep"":0,""ak"":0,""df"":0}]}}}";
	}
	
	
	public override string generateLocalJson (HttpTask task) 
	{
		return  DefaultJson;
	}
	
}

//获取副本第一关任务完成后的奖励
public class GetTaskReward:JsonFactory
{
	public GetTaskReward()
	{
		DefaultJson = @"{""status"":1,""act"":813,""data"":{""award"":[{""ppid"":0,""pid"":110052,""num"":10}],""nextTask"":{""id"":1002,""type"":1,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1}}}";
	}
	
	
	public override string generateLocalJson (HttpTask task) 
	{
		return  DefaultJson;
	}
}

//获取副本第一关任务完成后的奖励
public class GetTaskList:JsonFactory
{
	public static int count = 0;
	public GetTaskList()
	{
		if(count == 0)
		{
			DefaultJson = @" {""status"":1,""act"":812,""data"":{""tasks"":[{""id"":16,""type"":0,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1},{""id"":18,""type"":0,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1},{""id"":19,""type"":0,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1},{""id"":23,""type"":0,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1},{""id"":1001,""type"":1,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1},{""id"":2001,""type"":1,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1},{""id"":3001,""type"":1,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1},{""id"":4001,""type"":1,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1},{""id"":5000,""type"":1,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1},{""id"":6000,""type"":1,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1},{""id"":7000,""type"":1,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1},{""id"":8000,""type"":1,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1},{""id"":9000,""type"":1,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1},{""id"":10000,""type"":1,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1},{""id"":11000,""type"":1,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1}]}}";
		}
		else
		{
			DefaultJson = @"{""status"":1,""act"":812,""data"":{""tasks"":[{""id"":16,""type"":0,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1},{""id"":18,""type"":0,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1},{""id"":19,""type"":0,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1},{""id"":23,""type"":0,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1},{""id"":1001,""type"":1,""condition"":1,""progress"":1,""minLevel"":0,""maxLevel"":120,""isGain"":1,""openType"":1},{""id"":2001,""type"":1,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1},{""id"":3001,""type"":1,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1},{""id"":4001,""type"":1,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1},{""id"":5000,""type"":1,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1},{""id"":6000,""type"":1,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1},{""id"":7000,""type"":1,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1},{""id"":8000,""type"":1,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1},{""id"":9000,""type"":1,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1},{""id"":10000,""type"":1,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1},{""id"":11000,""type"":1,""condition"":1,""progress"":0,""minLevel"":0,""maxLevel"":120,""isGain"":0,""openType"":1}]}}";
		}
}	
	
	
	public override string generateLocalJson (HttpTask task) 
	{
		return  DefaultJson;
	}
}