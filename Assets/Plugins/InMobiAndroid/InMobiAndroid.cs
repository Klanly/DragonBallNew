using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;


#if UNITY_ANDROID

public enum InMobiAdSize
{
	Unit_300x250 = 10,
	Unit_728x90 = 11,
	Unit_468x60 = 12,
	Unit_320x50 = 15
}

public enum InMobiLogLevel
{
    None      = 0,
    Debug     = 1,
    Verbose   = 2,
}

public enum IMLTVPPingType
{
	IMPRESSION,
	CLICK,
	GOAL_SUCCESS,
	GOAL_FAILURE
}


public enum IMItemType
{
    CONSUMABLE,
    DURABLE,
    PERSONALIZATION
}


public enum InMobiAdPosition
{
	TopLeft,
	TopCenter,
	TopRight,
	Centered,
	BottomLeft,
	BottomCenter,
	BottomRight
}

public enum IMErrorCode
{
	INVALID_REQUEST,
	INTERNAL_ERROR,
	NO_FILL,
	DO_MONETIZE,
	DO_NOTHING,
	NETWORK_ERROR
}


public class InMobiAndroid
{
	private static AndroidJavaObject _plugin;

	static InMobiAndroid()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		// find the plugin instance
		using( var pluginClass = new AndroidJavaClass( "com.prime31.InMobiPlugin" ) )
			_plugin = pluginClass.CallStatic<AndroidJavaObject>( "instance" );
	}


	// Sets the log level for the InMobi SDK
	public static void setLogLevel( InMobiLogLevel logLevel )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "setLogLevel", (int)logLevel );
	}
	
	
	public static void init( string appId,Dictionary<string,string> optionalInMobiParams = null )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "initialize", appId,optionalInMobiParams != null ? optionalInMobiParams.toJson() : string.Empty);
	}
	

	
	
	#region Events/Analytics

	// Starts a session
	public static void startSessionManually( Dictionary<string,string> parameters = null )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "startSessionManually", parameters != null ? parameters.toJson() : string.Empty );
	}


	// Ends the current session
	public static void endSessionManually( Dictionary<string,string> parameters = null )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "endSessionManually", parameters != null ? parameters.toJson() : string.Empty );
	}


	// Logs the start of a section
	public static void beginSection( string sectionName, Dictionary<string,string> parameters = null )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "beginSection", sectionName, parameters != null ? parameters.toJson() : string.Empty );
	}


	// Logs the end of a section
	public static void endSection( string sectionName, Dictionary<string,string> parameters = null )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "endSection", sectionName, parameters != null ? parameters.toJson() : string.Empty );
	}
	
	
	// Tracks a custom event with optional parameters
	public static void tagEvent( string eventName, Dictionary<string,string> parameters = null )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "tagEvent", eventName, parameters != null ? parameters.toJson() : string.Empty );
	}
	

	// Reports a custom goal
	public static void reportCustomGoal( string goalName )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "reportCustomGoal", goalName );
	}

	// setting User Attribute
	public static void setUserAttribute( string key,string value )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "setUserAttribute", key, value);
	}
	
	
	#endregion
	
	
	#region interstitials

	// Preloads an interstitial
	public static void loadInterstitial( string appId )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "loadInterstitial", appId );
	}
	
	
	// Preloads an interstitial via the slotId
	public static void loadInterstitialWithSlotId( long slotId )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "loadInterstitialWithSlotId", slotId );
	}
	
	
	// Gets the current state of the interstitial. State will be None, Init, Active, Loading, Ready or Unknown
	public static string getInterstitialState()
	{
		if( Application.platform != RuntimePlatform.Android )
			return "None";
		return _plugin.Call<string>( "getInterstitialState" );
	}
	
	
	// Presents the interstitial if it is loaded and ready
	public static void showInterstitial()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "showInterstitial" );
	}
	
	#endregion
	
	
	#region banners

	// Creates a banner
	public static void createBanner( InMobiAdPosition alignment, string appId, InMobiAdSize adSize, int refreshInterval )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		if( refreshInterval > 0 && refreshInterval < 20 )
		{
			Debug.LogError( "refreshInterval cannot be less than 20 seconds. resettting it to 20." );
			refreshInterval = 20;
		}
		_plugin.Call( "createBanner", (int)alignment, (int)adSize, appId, refreshInterval );
	}
	
	
	// Reloads the banner
	public static void loadBanner()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "loadBanner");
	}


	// Destroys the banner
	public static void destroyBanner()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "destroyBanner");
	}
	
	#endregion

}

#endif
