using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;


public class InMobiAndroidManager : AbstractManager
{
#if UNITY_ANDROID
	// Fired when an interstitial is dismissed
	public static event Action onDismissInterstitialScreenEvent;

	// Fired when an interstitial fails to load
	public static event Action<IMErrorCode> onInterstitialFailedEvent;

	// Fired when a interstitial is interacted with. Includes the data payload.
	public static event Action<Dictionary<string,object>> onInterstitialInteractionEvent;

	// Fired when an interstitial is loaded
	public static event Action onInterstitialLoadedEvent;

	// Fired when an action will leave the application
	public static event Action onLeaveApplicationEvent;

	// Fired when an interstitial is shown
	public static event Action onShowInterstitialScreenEvent;

	// Fired when a banner is interacted with. Includes the data payload.
	public static event Action<Dictionary<string,object>> onBannerInteractionEvent;

	// Fired when a banner fails to load
	public static event Action<IMErrorCode> onBannerRequestFailedEvent;

	// Fired when a banner loads
	public static event Action onBannerRequestSucceededEvent;

	// Fired when a banners full screen action is dismissed
	public static event Action onDismissBannerScreenEvent;

	// Fired when a banner shows an action screen
	public static event Action onShowBannerScreenEvent;

	// Fired when incent callbacks completed 
	public static event Action<Dictionary<string,object>> onIncentCompletedEvent;
	
	

	static InMobiAndroidManager()
	{
		AbstractManager.initialize( typeof( InMobiAndroidManager ) );
	}


	public void onDismissInterstitialScreen( string empty )
	{
		onDismissInterstitialScreenEvent.fire();
	}


	public void onInterstitialFailed( string error )
	{
		onInterstitialFailedEvent.fire( (IMErrorCode)Enum.ToObject( typeof( IMErrorCode ), int.Parse( error ) ) );
	}


	public void onInterstitialInteraction( string json )
	{
		onInterstitialInteractionEvent.fire( json.dictionaryFromJson() );
	}


	public void onInterstitialLoaded( string empty )
	{
		onInterstitialLoadedEvent.fire();
	}


	public void onLeaveApplication( string empty )
	{
		onLeaveApplicationEvent.fire();
	}


	public void onShowInterstitialScreen( string empty )
	{
		onShowInterstitialScreenEvent.fire();
	}


	public void onBannerInteraction( string json )
	{
		onBannerInteractionEvent.fire( json.dictionaryFromJson() );
	}


	public void onBannerRequestFailed( string error )
	{
		onBannerRequestFailedEvent.fire( (IMErrorCode)Enum.ToObject( typeof( IMErrorCode ), int.Parse( error ) ) );
	}


	public void onBannerRequestSucceeded( string empty )
	{
		onBannerRequestSucceededEvent.fire();
	}


	public void onDismissBannerScreen( string empty )
	{
		onDismissBannerScreenEvent.fire();
	}


	public void onShowBannerScreen( string empty )
	{
		onShowBannerScreenEvent.fire();
	}

	public void onIncentCompleted( string json )
	{
		onIncentCompletedEvent.fire( json.dictionaryFromJson() );
	}



#endif
}

