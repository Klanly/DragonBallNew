//
//  HTGameProxy.h
//  HTGameProxy
//
//  Created by zbflying on 14-9-25.
//  Copyright (c) 2014年 上海黑桃互动网络科技有限公司. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIApplication.h>
#import "HTGameInfo.h"
#import "HTProtocol.h"
#import "HTPayInfo.h"
#import "HTUser.h"
#import "HTError.h"

@interface HTGameProxy : NSObject

/**
 *  初始化SDK
 */
+ (void)initWithGameInfo:(HTGameInfo *)gameInfo;

/**
 *  登录
 *  @param dictionary 自定义参数
 */
+ (void)doLogin:(NSDictionary *)dictionary delegate:(id<HTLoginProtocol>)delegate;

/**
 *  登出
 *  @param dictionary 自定义参数
 */
+ (void)doLogout:(NSDictionary *)dictionary delegate:(id<HTLogoutProtocol>)delegate;

/**
 *  支付
 *  @param payInfo 支付信息
 */
+ (void)doPay:(HTPayInfo *)payInfo delegate:(id<HTPayProtocol>)delegate;

/**
 *  设置显示悬浮按钮
 *  @param bShow 是否显示
 */
+ (void)setShowFunctionMenu:(BOOL)bShow;

/**
 *  开始游戏
 *  @return 是否可以开始游戏
 */
+ (BOOL)onStartGame;

/**
 *  进入游戏
 *  @param dictionary 自定义参数
 */
+ (void)onEnterGame:(NSDictionary *)dictionary;

/**
 *  游戏等级发生变化
 *  @param newLevel 新等级
 */
+ (void)onGameLevelChanged:(int)newLevel;

/**
 *  游戏视图控制器将要出现
 */
+ (void)viewWillAppear;

/**
 *  游戏视图控制器将要消失
 */
+ (void)viewWillDisappear;

/**
 *  游戏进入后台
 */
+ (void)applicationDidEnterBackground;

/**
 *  游戏恢复前台
 */
+ (void)applicationWillEnterForeground;

/*
 *  客户端回调
 */
+ (void)application:(UIApplication *)application handleOpenURL:(NSURL *)url;

/**
 *  设置打印日志
 *  @param bEnable  是否打印
 */
+ (void)setLogEnable:(BOOL)bEnable;

/**
 *  设置开发模式
 *  @param bEnable  是否启用
 */
+ (void)setDebugEnable:(BOOL)bEnable;

/**
 *  获取渠道SDK版本
 *  @return 渠道SDK版本号
 */
+ (NSString *)getChannelSDKVersion;

/**
 *  获取SDK版本
 *  @return SDK版本号
 */
+ (NSString *)getSDKVersion;

@end
