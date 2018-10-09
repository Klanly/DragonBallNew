using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FinalTrialDataMgr : Manager {
	
	private List<MapFinalTrialData> MapFinalTrialDataConfig;
    
    public FinalTrialDataMgr()
    {
		MapFinalTrialDataConfig = new List<MapFinalTrialData>();
    }
    
    public override bool loadFromConfig () {
		return base.readFromLocalConfigFile<MapFinalTrialData> (ConfigType.MapOfFinalTrial, MapFinalTrialDataConfig);
    }
    
	public List<MapFinalTrialData> GetMessageInfoDataConfig()
    {
		return MapFinalTrialDataConfig;
    }
    
    
	public MapFinalTrialData GetMapFinalTrialDataById(int mId)
    {
		if(MapFinalTrialDataConfig.Count != 0)
        {
			for(int i=0; i<MapFinalTrialDataConfig.Count; i++)
            {
				if(MapFinalTrialDataConfig[i].ID == mId)
                {
					return MapFinalTrialDataConfig[i];
                }
            }
        }
        return null;
    }

	public List<MapFinalTrialData> GetShaluOrBuouList(int m_Type)
	{
		List<MapFinalTrialData> m_tempShalu = new List<MapFinalTrialData>();
		List<MapFinalTrialData> m_tempBuou = new List<MapFinalTrialData>();
		foreach(MapFinalTrialData data in MapFinalTrialDataConfig)
		{
			if(data.FightType == 0)m_tempShalu.Add(data);
			else if(data.FightType == 1)m_tempBuou.Add(data);
		}
		if(m_Type == 0)return m_tempShalu;
		else return m_tempBuou;
	}

	public override void fullfillByNetwork(BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR)
		{
			LoginResponse loginResp = response as LoginResponse;
			if(loginResp != null && loginResp.data != null && loginResp.data.buliding != null) 
			{
				//沙鲁布欧手动同步
//				NewFinalTrialStateRequest();
			}
		}
	}

	void NewFinalTrialStateRequest()
	{
//		ComLoading.Open();
		NewFinalTrialStateParam param = new NewFinalTrialStateParam(int.Parse(Core.Data.playerManager.PlayerID));
		HttpTask task = new HttpTask(ThreadType.MainThread,TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.NEW_FINALTRIAL_STATE, param);
		
		task.afterCompleted += SetNewFinalTrialData;
		task.DispatchToRealHandler();
	}

	void SetNewFinalTrialData(BaseHttpRequest request, BaseResponse response)
	{
//		ComLoading.Close();
		if(response != null && response.status != BaseResponse.ERROR)
		{
			HttpRequest httprequest = request as HttpRequest;
			if(httprequest.Act == HttpRequestFactory.ACTION_NEW_FINALTRIAL_STATE)
			{
				GetFinalTrialStateResponse res = response as GetFinalTrialStateResponse;
				if(res != null && res.data != null && res.data.shalu != null && res.data.buou != null)
				{
					if(FinalTrialMgr.GetInstance()._PvpShaluBuouRoot != null)
					{
						FinalTrialMgr.GetInstance()._FinalTrialData.getFinalTrialStateDataShalu = res.data.shalu;
						FinalTrialMgr.GetInstance()._FinalTrialData.getFinalTrialStateDataBuou = res.data.buou;
						if(FinalTrialMgr.GetInstance().NowEnum == TrialEnum.TrialType_ShaLuChoose)
						{
							FinalTrialMgr.GetInstance().OpenNewMap(1);
						}
						else if(FinalTrialMgr.GetInstance().NowEnum == TrialEnum.TrialType_PuWuChoose)
						{
							FinalTrialMgr.GetInstance().OpenNewMap(2);
						}
					}

				}
				
			}
		}
	}

}
