using UnityEngine;
using System.Collections;
using JPush ;
using System.Collections.Generic;
using System;


//杨晨光 7月7号 Android推送初始化 -- android 需呀把这个脚本挂载Debug 下面
public class AndroidJPushInit : MonoBehaviour
{
	#if UNITY_ANDROID && !UNITY_EDITOR
	// Use this for initialization
	void Start ()
	{
//		gameObject.name = "Debug";
//		JPushBinding.setDebug(true) ;
//		JPushBinding.initJPush(gameObject.name , "") ;	
	}
	#endif
  
    public static AndroidJPushInit _instence  ;
    void Awake()
    {
        _instence = this  ;
    }

    //ios 推送开启
    public  void addIOSJpush()
    {
        #if UNITY_IPHONE && !UNITY_EDITOR
        this.gameObject.AddComponent<JPushBinding>();
        #endif
    }
    //android 推送开启
    public  void  addAndroidJpush()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        JPushBinding.setDebug(true) ;
        JPushBinding.initJPush(gameObject.name , "") ;  
        #endif
    }

}
