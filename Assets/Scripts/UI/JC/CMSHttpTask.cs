using UnityEngine;
using System.Collections;
using System;
using fastJSON;
public class CMSHttpTask 
{
	/* The code will be excute after we get response
	 */ 
	public Action<BaseHttpRequest, BaseResponse> afterCompleted;

	/* When Error Occured, it will notify user
	 */ 
	public Action<BaseHttpRequest, string> ErrorOccured;
	
	
	HttpTask task = null;
	public HttpRequest request;
	
	public CMSHttpTask (ThreadType threadType, TaskResponse respType = TaskResponse.Default_Response) 
	{
		task = new HttpTask(threadType, respType);	
	}
	
	public void AppendCommonParam (RequestType requestType, BaseRequestParam param , string url)
	{
		if(task != null)
		{
//			Debug.Log(">>>>>>>>>>>>>>>>>>>>>");
			this.request = HttpRequestFactory.createHttpRequestInstance(requestType, param);
			request.Url = url;
			
//			Debug.Log(param.generatePara());
			string data = param.generatePara();
			//{"gameid":1,"version":"1.0","md5":"FC9387A064E004309239851A9E5864E3"}
			url = url +"?" +GetFormatData(data);
			task.AppendThirdParam(ThirdPartyRequestType.CMS,url);

		    task.ErrorOccured += testHttpResp_Error;
		    task.afterCompleted += testHttpResp_Sucess;
		}
	}
	
	
	public void testHttpResp_Error(BaseHttpRequest b, string s)
	{
		if(ErrorOccured != null)
			ErrorOccured(b,s);
	}
	
	public void testHttpResp_Sucess(BaseHttpRequest request, BaseResponse response)
	{
		Debug.Log(  "^^^^^^--->"+ ((ThirdPartyResponse)response).rawData);
		if(afterCompleted != null)
		{
			Debug.Log(  "^^^^^^--->"+ ((ThirdPartyResponse)response).rawData);
			System.Type type = HttpRequestFactory.PreDefined[RequestType.UPDATE_RESOURCES].respType;
		    BaseResponse  res = fastJSON.JSON.Instance.ToObject( ((ThirdPartyResponse)response).rawData,  type   ) as BaseResponse;
		    afterCompleted(this.request,res);
		}
	}
	
	public  void DispatchToRealHandler() 
	{
		if(task != null)
		task.DispatchToRealHandler();
	}
	
	string GetFormatData(string jsondata)
	{
		string result = "";
		foreach(char a in jsondata)
		{
			char c = a;
			if(c!='{' && c!='}' && c !='\"')
			{
				if(c==':')
					c='=';
				else if(c==',')
					c='&';
				result+= c;
			}
		}
		return result;
	}
	
	
}
