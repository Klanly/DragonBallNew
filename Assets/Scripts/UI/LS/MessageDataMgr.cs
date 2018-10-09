using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageDataMgr : Manager 
{
    private List<MessageInfoData> MessageInfoDataConfig ;
    
    public MessageDataMgr()
    {
        MessageInfoDataConfig = new List<MessageInfoData>();

    }
    
    public override bool loadFromConfig () {
        return base.readFromLocalConfigFile<MessageInfoData> (ConfigType.Message, MessageInfoDataConfig);
    }
    
    public List<MessageInfoData> GetMessageInfoDataConfig()
    {
        return MessageInfoDataConfig;
    }

    
    public MessageInfoData GetMessageInfoData(int mIndex)
    {
        if(MessageInfoDataConfig.Count != 0)
        {
            if(mIndex <=  MessageInfoDataConfig.Count)
            {
                return MessageInfoDataConfig[mIndex-1];
            }
        }
        return null;
    }

    public MessageInfoData GetMessageInfoDataById(int mId)
    {
        if(MessageInfoDataConfig.Count != 0)
        {
            for(int i=0; i<MessageInfoDataConfig.Count; i++)
            {
                if(MessageInfoDataConfig[i].id == mId)
                {
                    return MessageInfoDataConfig[i];
                }
            }
        }
        return null;
    }
    


}
