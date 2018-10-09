using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SecretShopDataMgr : Manager 
{

	private List<SecretShopData> SecretShopDataConfig ;
	
	public SecretShopDataMgr()
	{
		SecretShopDataConfig = new List<SecretShopData>();
		
	}
	
	public override bool loadFromConfig () {
		return base.readFromLocalConfigFile<SecretShopData> (ConfigType.SecretShop, SecretShopDataConfig);
	}
	
	public List<SecretShopData> GetSecretShopDataConfig()
	{
		return SecretShopDataConfig;
	}
	
	
	public SecretShopData GetSecretShopData(int mIndex)
	{
		if(SecretShopDataConfig.Count != 0)
		{
			if(mIndex <=  SecretShopDataConfig.Count)
			{
				return SecretShopDataConfig[mIndex-1];
			}
		}
		return null;
	}
	
	public SecretShopData GetSecretShopDataById(int mId)
	{
		if(SecretShopDataConfig.Count != 0)
		{
			for(int i=0; i<SecretShopDataConfig.Count; i++)
			{
				if(SecretShopDataConfig[i].id2 == mId)
				{
					return SecretShopDataConfig[i];
				}
			}
		}
		return null;
	}
}
