#if APP_Store
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public partial class IOSInterface:MonoBehaviour{

	public IAPDelegate _iapDelegate;
	string _productID = null;
	string _recepit = null;
	
	void receiveIAPMsg(string msg)
	{
		string[] message = msg.Split('#');
		if(message[0].Equals("receivedProducts"))
		{
			if(_iapDelegate !=null)
			{
				_iapDelegate.didReceivedProducts();
			}
		}
		else if(message[0].Equals("buyFail"))
		{
			if(_iapDelegate != null)
			{
				_iapDelegate.didFailedBuyProduct();
			}
		}

	}
	void willDidCompleteWithRecepit(string productId)
	{
		if(_iapDelegate != null)
		{
			_productID = productId;
		}
		else{
			SQYDataManager.getInstance().lstProductID.Add(productId);
		}
	}
	void didCompleteWithRecepit(string msg)
	{
		if(_iapDelegate != null)
		{
			_recepit = msg;
			_iapDelegate.didCompleteWithRecepit(_recepit,_productID);
		}
		else{
			SQYDataManager.getInstance().lstRecepit.Add(msg);
		}
	}
	
}

public interface IAPDelegate
{
	void didCompleteWithRecepit(string recepit,string productId);
	void didFailedBuyProduct();
	void didReceivedProducts();
}

public class IAP
{
	[DllImport ("__Internal")]
	private static extern void _requestProduct();
	[DllImport ("__Internal")]
	private static extern void _payForProIdentifier(string proId);
	[DllImport ("__Internal")]
	private static extern void _didFinishLastTransaction(string proId);

	public static void RequestProducts()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer||
		   Application.platform == RuntimePlatform.OSXPlayer)
		{
			_requestProduct();
		}
	}
	public static void payForProIdentifier(string proId)
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer||
		   Application.platform == RuntimePlatform.OSXPlayer)
		{
			_payForProIdentifier(proId);
		}
	}
	public static void didFinishLastTransaction(string productId)
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer||
		   Application.platform == RuntimePlatform.OSXPlayer)
		{
			_didFinishLastTransaction(productId);
		}
	}
}

#endif