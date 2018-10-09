using UnityEngine;
using System.Collections;
using System;
using fastJSON;
using System.Collections.Generic;
using Framework;

public class GetUniqueIDFactory 
{
	public IGetUniqueID createInstance() 
	{
	#if UNITY_IPHONE
		#if APP_STORE
			return new AppStoreLogin();
		#elif Spade
			return new SpadeIOSLogin();
		#else
			return new QuickLogin();
		#endif

    #elif UNITY_STANDALONE
        return new QuickLogin();
	
    #elif UNITY_ANDROID
		#if QiHo360 && !UNITY_EDITOR
			return new Qiho360Login();
		#elif Spade
			return new SpadeIOSLogin();
		#else
        	return new QuickLogin();
		#endif
    #else
        return new QuickLogin();
	#endif
	}
}


