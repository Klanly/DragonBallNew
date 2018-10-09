using UnityEngine;
using System;
using fastJSON;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class FriendManager : Manager 
{
    public List<OtherUserInfo> friendList = new List<OtherUserInfo>();
    public List<OtherUserInfo> suDiList = new List<OtherUserInfo>();


    public const int pageShowListItemCount = 10;
    #region 好友列表分页
    // 好友列表总数
    public int friendListCount;
    // 好友列表总页数
    public int friendListPageCount;
    // 好友列表当前页数
    public int friendListCurrentPage;
    #endregion

    #region 好友申请分页
    // 好友申请列表总数
    public int requestFriendListCount;
    // 好友申请列表总页数
    public int requestFriendListPageCount;
    // 好友申请列表当前页数
    public int requestFriendListCurrentPage;
    #endregion

    #region 宿敌列表分页
    // 宿敌列表总数
    public int suDiListCount;
    // 宿敌列表总页数
    public int suDiListPageCount;
    // 宿敌列表当前页数
    public int suDiListCurrentPage;
    #endregion

    public FriendManager()
    {
       
    }

    public override bool loadFromConfig () 
    {
        return true;
    }
    #region 好友列表
    public Action getFriendListRequestCompletedDelegate;

    public void initFriendListInfo()
    {
        Core.Data.FriendManager.friendListCount = 0;
        Core.Data.FriendManager.friendListCurrentPage = 1;
        Core.Data.FriendManager.friendListPageCount = 0;
        Core.Data.FriendManager.friendList.Clear();
    }

    public void getFriendListRequest()
    {
        HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
        task.AppendCommonParam(RequestType.GET_FRIEND_LIST, new GetFriendListParam(Core.Data.playerManager.PlayerID, this.friendListCurrentPage, 0));

        task.afterCompleted = getFriendListRequestCompleted;
        task.ErrorOccured = getFriendListRequestError;

        task.DispatchToRealHandler();
    }

    public void getFriendListRequestCompleted(BaseHttpRequest request, BaseResponse response)
    {
        if(response != null && response.status != BaseResponse.ERROR)
        {
            //            GetFriendListResponse getFriendListResponse = response as GetFriendListResponse;
            //            if(getFriendListResponse.data != null)
            //            {
            //                GetOtherUserListInfo getFriendListInfo = getFriendListResponse.data;
            //                this.friendList.AddRange(getFriendListInfo.friends);
            //                this.friendListCount = getFriendListInfo.count;
            //                this.friendListPageCount = this.getPageCount(this.friendListCount);
            //                if(getFriendListRequestCompletedDelegate != null)
            //                {
            //                    getFriendListRequestCompletedDelegate();
            //                }
            //            }
            //
            //
            //            if(!UIMainFriend.Instance.gameObject.activeSelf)
            //            {
            //                UIMainFriend.Instance.gameObject.SetActive(true);
            //            }
        }
        // 测试用 start
        if(!UIMainFriend.Instance.gameObject.activeSelf)
        {
            UIMainFriend.Instance.gameObject.SetActive(true);
        }
        // end
        ComLoading.Close();
    }

    public void getFriendListRequestError(BaseHttpRequest request, string error)
    {

        ComLoading.Close();
    }
    #endregion



    #region 获取别人申请添加好友请求列表

    public Action<List<OtherUserInfo>> getFriendRequestListRequestCompletedDelegate;

    public List<OtherUserInfo> requestFriendList = new List<OtherUserInfo>();

    public void initFriendRequestListInfo()
    {
        Core.Data.FriendManager.requestFriendListCount = 0;
        Core.Data.FriendManager.requestFriendListCurrentPage = 1;
        Core.Data.FriendManager.requestFriendListPageCount = 0;
        Core.Data.FriendManager.requestFriendList.Clear();
    }

    public void getFriendRequestListRequest()
    {
        HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
        task.AppendCommonParam(RequestType.GET_FRIEND_REQUEST_LIST, new GetRequestFriendListParam(Core.Data.playerManager.PlayerID, this.requestFriendListCurrentPage, 0));

        task.afterCompleted = getFriendRequestListRequestCompleted;
        task.ErrorOccured = getFriendRequestListRequestError;

        task.DispatchToRealHandler();
    }

    public void getFriendRequestListRequestCompleted(BaseHttpRequest request, BaseResponse response)
    {
        if(response != null && response.status != BaseResponse.ERROR)
        {
            GetFriendRequestListResponse getFriendRequestListResponse = response as GetFriendRequestListResponse;
            if(getFriendRequestListResponse.data != null)
            {
                //                GetOtherUserListInfo getFriendListInfo = getFriendRequestListResponse.data;
                //                this.requestFriendListCount = getFriendListInfo.count;
                //                this.requestFriendList.AddRange(getFriendListInfo.friends);
                //                this.requestFriendListPageCount = this.getPageCount(this.requestFriendListCount);
                //
                //                if(getFriendRequestListRequestCompletedDelegate != null)
                //                {
                //                    getFriendRequestListRequestCompletedDelegate(this.requestFriendList);
                //                }
            }

        }
        ComLoading.Close();
    }

    public void getFriendRequestListRequestError(BaseHttpRequest request, string error)
    {
        ComLoading.Close();
    }

    #endregion


    #region 宿敌列表

   // public Action getSuDiListRequestCompletedDelegate;

    public void initSuDiListInfo()
    {
        Core.Data.FriendManager.suDiListCount = 0;
        Core.Data.FriendManager.suDiListCurrentPage = 1;
        Core.Data.FriendManager.suDiListPageCount = 0;
        Core.Data.FriendManager.suDiList.Clear();
    }
    #endregion

    //放到FinalTrialMgr类中
    //    public void getSuDiRequest()
    //    {
    //        this.getSuDiRequest(getSuDiRequestCompleted, getSuDiRequestError);
    //    }
    //
    //    public void getSuDiRequest(Action<BaseHttpRequest, BaseResponse> afterCompleted, Action<BaseHttpRequest, string> ErrorOccured)
    //    {
    //        HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
    //        task.AppendCommonParam(RequestType.GET_SU_DI_LIST, new GetSuDiParam(Core.Data.playerManager.PlayerID, this.suDiListCurrentPage, 0));
    //
    //        task.afterCompleted = afterCompleted;
    //        task.ErrorOccured = ErrorOccured;
    //
    //        task.DispatchToRealHandler ();
    //    }
    //
    //   

    //     添加宿敌
    //
    //
    //    public Action addSuDiCompletedDelegate;
    //
    //    public void addSuDiRequest(int oid)
    //    {
    //        HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
    //        task.AppendCommonParam(RequestType.ADD_SU_DI, new AddSuDiParam(Core.Data.playerManager.PlayerID, oid));
    //
    //        task.afterCompleted += addSuDiCompleted;
    //
    //        task.ErrorOccured += addSuDiError;
    //
    //
    //        task.DispatchToRealHandler ();
    //    }
    //
    //    public void addSuDiCompleted(BaseHttpRequest request, BaseResponse response)
    //    {
    //        if(response != null && response.status != BaseResponse.ERROR)
    //        {
    //            AddSuDiResponse addSuDiResponse = response as AddSuDiResponse;
    //            if(addSuDiResponse.data == 0)
    //            {
    //                if(addSuDiCompletedDelegate != null)
    //                {
    //                    addSuDiCompletedDelegate();
    //                }
    //            }
    //
    //        }
    //    }
    //
    //    public void addSuDiError(BaseHttpRequest request, string error)
    //    {
    //    }
    //    

    //HENRY EDIT
    //    删除宿敌
    //
    //    public Action deleteSuDiCompletedDelegate;
    //
    //    public OtherUserInfo deleteSuDiData;
    //
    //    public void deleteSuDiRequest(int eyid)
    //    {
    //        HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
    //        task.AppendCommonParam(RequestType.DELETE_SU_DI, new DeleteSuDiParam(Core.Data.playerManager.PlayerID, eyid.ToString()));
    //
    //        task.afterCompleted += deleteSuDiCompleted;
    //
    //        task.ErrorOccured += deleteSuDiError;
    //
    //        task.DispatchToRealHandler ();
    //
    //    }
    //
    //
    //    public void deleteSuDiCompleted(BaseHttpRequest request, BaseResponse response)
    //    {
    //        if(response != null && response.status != BaseResponse.ERROR)
    //        {
    //            DeleteSuDiResponse deleteSuDiResponse = response as DeleteSuDiResponse;
    //            if(deleteSuDiResponse.data)
    //            {
    //                this.suDiList.Remove(deleteSuDiData);
    //                this.suDiListCount--;
    //                this.suDiListPageCount = this.getPageCount(this.suDiListCount);
    //                if(deleteSuDiCompletedDelegate != null)
    //                {
    //                    deleteSuDiCompletedDelegate();
    //                }
    //            }
    //        }
    //
    //        ComLoading.Close();
    //    }
    //
    //    public void deleteSuDiError(BaseHttpRequest request, string error)
    //    {
    //        ComLoading.Close();
    //    }


    #region 同意/拒绝 添加好友

    // 1为 同意 2为 不同意
    public int agreeOrRefusedFriend = -1;
    public OtherUserInfo agreeOrRefusedFriendData = null;

    public Action<int, int> agreeOrRefusedFriendRequestCompletedDelegate;

    // 1为 同意 2为 不同意
    public void agreeOrRefusedFriendRequest(OtherUserInfo otherUserInfo, int agreeOrRefusedFriend)
    {
        this.agreeOrRefusedFriend = agreeOrRefusedFriend;
        this.agreeOrRefusedFriendData = otherUserInfo;

        HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
        task.AppendCommonParam(RequestType.AGREE_OR_REFUSED_FRIEND, new AgreeOrRefusedFriendParam(Core.Data.playerManager.PlayerID, otherUserInfo.g.ToString(), this.agreeOrRefusedFriend));

        task.afterCompleted += agreeOrRefusedFriendRequestCompleted;

        task.ErrorOccured += agreeOrRefusedFriendRequestError;

        task.DispatchToRealHandler ();
    }

    public void agreeOrRefusedFriendRequestCompleted(BaseHttpRequest request, BaseResponse response)
    {
        if(response != null && response.status != BaseResponse.ERROR)
        {
            AgreeOrRefusedFriendResponse agreeOrRefusedFriendResponse = response as AgreeOrRefusedFriendResponse;
            this.requestFriendList.Remove(this.agreeOrRefusedFriendData);
            this.requestFriendListCount--;
            this.requestFriendListPageCount = this.getPageCount(this.requestFriendListCount);

            if(agreeOrRefusedFriend == 1)
            {
                this.friendListCount++;
                this.friendList.Add(this.agreeOrRefusedFriendData);
                this.friendListPageCount = this.getPageCount(this.friendListCount);
            }

            if(this.agreeOrRefusedFriendRequestCompletedDelegate != null)
            {
                agreeOrRefusedFriendRequestCompletedDelegate(this.agreeOrRefusedFriend, agreeOrRefusedFriendResponse.data);
            }
        }
        ComLoading.Close();
    }

    public void agreeOrRefusedFriendRequestError(BaseHttpRequest request, string error)
    {
        ComLoading.Close();
    }

    #endregion

    #region 申请添加/删除好友

    public Action<int, int> addOrDeleteFriendRequestCompletedDelegate;

    public int addOrDeleteFriendOP = 0;

    public OtherUserInfo deleteFriendInfo;

    // op 1申请添加好友,2删除好友
    public void addOrDeleteFriendRequest(OtherUserInfo deleteFriendInfo, int op)
    {
        this.addOrDeleteFriendOP = op;

        this.deleteFriendInfo = deleteFriendInfo;

        HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);

        task.AppendCommonParam(RequestType.ADD_OR_DELETE_FRIEND, new AddOrDeleteFriendParam(Core.Data.playerManager.PlayerID, deleteFriendInfo.g.ToString(), 1));

        task.afterCompleted += addOrDeleteFriendRequestCompleted;

        task.ErrorOccured += addOrDeleteFriendRequestError;

        task.DispatchToRealHandler();
    }

    public void addOrDeleteFriendRequestCompleted(BaseHttpRequest request, BaseResponse response)
    {
        if(response != null && response.status != BaseResponse.ERROR)
        {
            AddOrDeleteFriendResponse addOrDeleteFriendResponse = response as AddOrDeleteFriendResponse;

            if(addOrDeleteFriendOP == 2 && addOrDeleteFriendResponse.data == 1)
            {
                this.friendList.Remove(this.deleteFriendInfo);
                this.friendListCount--;
                this.friendListPageCount = this.getPageCount(this.friendListCount);
            }

            if(addOrDeleteFriendRequestCompletedDelegate != null)
            {
                addOrDeleteFriendRequestCompletedDelegate(addOrDeleteFriendOP, addOrDeleteFriendResponse.data);
            }
        }
        ComLoading.Close();
    }

    public void addOrDeleteFriendRequestError(BaseHttpRequest request, string error)
    {
        ComLoading.Close();
    }
    #endregion

    #region 搜索
    public List<OtherUserInfo> searchUserResultList = new List<OtherUserInfo>();
    public Action<List<OtherUserInfo>> searchFriendRequestCompletedDelegate;

    public void searchUserRequest(string msg, int op)
    {
        HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
        task.AppendCommonParam(RequestType.SEARCH_FRIEND, new SearchUserParam(Core.Data.playerManager.PlayerID, msg, op));

        task.afterCompleted += searchFriendRequestCompleted;

        task.ErrorOccured += searchFriendRequestError;

        task.DispatchToRealHandler ();
    }

    public void searchFriendRequestCompleted(BaseHttpRequest request, BaseResponse response)
    {
        if(response != null && response.status != BaseResponse.ERROR)
        {
            SearchUserResponse searchUserResponse = response as SearchUserResponse;
            this.searchUserResultList.Clear();
            if(searchUserResponse.data != null)
            {
                this.searchUserResultList.AddRange(searchUserResponse.data);
            }
            if(searchFriendRequestCompletedDelegate != null)
            {
                this.searchFriendRequestCompletedDelegate(this.searchUserResultList);
            }
        }
        ComLoading.Close();
    }

    public void searchFriendRequestError(BaseHttpRequest request, string error)
    {
        ComLoading.Close();
    }
    #endregion

    #region 留言

    public Action<bool> sendMessageRequestCompletedDelegate;

    public void sendMessageRequest(int sRodeId, string msg)
    {
        HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
        task.AppendCommonParam(RequestType.SEND_MESSAGE, new SendMessageParam(Core.Data.playerManager.PlayerID, sRodeId, msg));

        task.afterCompleted += searchFriendRequestCompleted;

        task.ErrorOccured += searchFriendRequestError;

        task.DispatchToRealHandler ();
    }

    public void sendMessageRequestCompleted(BaseHttpRequest request, BaseResponse response)
    {
        if(response != null && response.status != BaseResponse.ERROR)
        {
            SendMessageResponse sendMessageResponse = response as SendMessageResponse;
            
            if(sendMessageResponse.data)
            {

            }
            else
            {

            }

            if(sendMessageRequestCompletedDelegate != null)
            {
                sendMessageRequestCompletedDelegate(sendMessageResponse.data);
            }
        
        }
        ComLoading.Close();
    }
    public void sendMessageRequestError(BaseHttpRequest request, string error)
    {
        ComLoading.Close();
    }
    #endregion

    // 获得总页码
    public int getPageCount(int listCount)
    {
        int pageCount = 1;
        if(listCount % FriendManager.pageShowListItemCount == 0)
        {
            pageCount = listCount / FriendManager.pageShowListItemCount;
        }
        else
        {
            pageCount = listCount / FriendManager.pageShowListItemCount + 1;
        }

        return pageCount;
    }
    //    protected List<OtherUserInfo> otherUserInfosToOtherUserDataList(OtherUserInfo[] otherUserInfos)
    //    {
    //        List<OtherUserInfo> otherUserInfoConfig = new List<OtherUserInfo>();
    //        foreach(OtherUserInfo otherUserInfo in otherUserInfos)
    //        {
    //            OtherUserData otherUserData = otherUserInfoToOtherUserData(otherUserInfo);
    //            otherUserInfoConfig.Add(otherUserData);
    //        }
    //        return otherUserInfoConfig;
    //    }

    public List<FightOpponentInfo> otherUserInfoListToFightOpponentInfoList(List<OtherUserInfo> otherUserInfoList)
    {
        List<FightOpponentInfo> fightOpponentInfoList = new List<FightOpponentInfo>();

        if(otherUserInfoList != null && otherUserInfoList.Count > 0)
        {
            foreach(OtherUserInfo otherUserData in otherUserInfoList)
            {
                FightOpponentInfo fightOpponentInfo = new FightOpponentInfo();
                fightOpponentInfo.g = otherUserData.g;
                fightOpponentInfo.r = otherUserData.r;
                fightOpponentInfo.n = otherUserData.n;
                fightOpponentInfo.lv = otherUserData.lv;
                fightOpponentInfo.vipLv = otherUserData.vipLv;
                fightOpponentInfo.p = otherUserData.p;
                fightOpponentInfo.hi = otherUserData.hi;
                fightOpponentInfo.eyid = otherUserData.eyid;
                fightOpponentInfoList.Add(fightOpponentInfo);
            }
        }
        return fightOpponentInfoList;
    }

    //    public void saveOtherUserToLocalFile(FightOpponentInfo[] fightOpponentInfos, ConfigType configType)
    //    {
    //        this.saveOtherUserToLocalFile(new List<FightOpponentInfo>(fightOpponentInfos), configType);
    //    }

    //    public void saveOtherUserToLocalFile(List<FightOpponentInfo> fightOpponentInfoList, ConfigType configType)
    //    {
    //        List<OtherUserData> otherUserDataList = getSuDiConfig();
    //
    //        if(otherUserDataList != null && fightOpponentInfoList.Count > 0)
    //        {
    //            otherUserDataList.Clear();
    //            foreach(FightOpponentInfo fightOpponentInfo in fightOpponentInfoList)
    //            {
    //                OtherUserData otherUserData = this.fightOpponentInfoToOtherUserData(fightOpponentInfo);
    //                otherUserDataList.Add(otherUserData);
    //            }
    //
    //            this.saveOtherUserToLocalFile(otherUserDataList, configType);
    //        }
    //    }

    //    protected void saveOtherUserToLocalFile(OtherUserInfo[] OtherUserInfos, ConfigType configType)
    //    {
    //        List<OtherUserData> otherUserDataList = getSuDiConfig();
    //        if(otherUserDataList != null && OtherUserInfos.Length > 0)
    //        {
    //            otherUserDataList.Clear();
    //            foreach(OtherUserInfo otherUserInfo in OtherUserInfos)
    //            {
    //                OtherUserData otherUserData = this.otherUserInfoToOtherUserData(otherUserInfo);
    //                otherUserDataList.Add(otherUserData);
    //            }
    //
    //            this.saveOtherUserToLocalFile(otherUserDataList, configType);
    //        }
    //    }

    //    public OtherUserData fightOpponentInfoToOtherUserData(FightOpponentInfo fightOpponentInfo)
    //    {
    //        OtherUserData otherUserData = new OtherUserData();
    //        otherUserData.g = fightOpponentInfo.g;
    //        otherUserData.r = fightOpponentInfo.r;
    //        otherUserData.n = fightOpponentInfo.n;
    //        otherUserData.lv = fightOpponentInfo.lv;
    //        otherUserData.vipLv = fightOpponentInfo.vipLv;
    //        otherUserData.p = fightOpponentInfo.p;
    //        otherUserData.hi = fightOpponentInfo.hi;
    //        otherUserData.eyid = fightOpponentInfo.eyid;
    //        return otherUserData;
    //    }

    //    public OtherUserData otherUserInfoToOtherUserData(OtherUserInfo otherUserInfo)
    //    {
    //        OtherUserData otherUserData = new OtherUserData();
    //        otherUserData.g = otherUserInfo.g;
    //        otherUserData.r = otherUserInfo.r;
    //        otherUserData.n = otherUserInfo.n;
    //        otherUserData.lv = otherUserInfo.lv;
    //        otherUserData.vipLv = otherUserInfo.vipLv;
    //        otherUserData.p = otherUserInfo.p;
    //        otherUserData.hi = otherUserInfo.hi;
    //        otherUserData.eyid = otherUserInfo.eyid;
    //        return otherUserData;
    //    }

    //    protected void saveOtherUserToLocalFile(List<OtherUserData> otherUserDataList, ConfigType configType)
    //    {
    //        this.writeToLocalFile<OtherUserData>(Core.Data.playerManager.PlayerID +"_"+ configType.ToString() + ".bytes", otherUserDataList);
    //    }

}