//
//  HTProtocolHandler.m
//  HTGameProxyDemo
//
//  Created by zbflying on 14-10-2.
//  Copyright (c) 2014年 上海黑桃互动网络科技有限公司. All rights reserved.
//

#import "HTProtocolHandler.h"
#import "HTUser.h"
#import "HTError.h"
#import "Utility.h"

@implementation HTProtocolHandler

+ (instancetype)handler
{
    static HTProtocolHandler *handler = nil;
    static dispatch_once_t onceToken;
    
    dispatch_once(&onceToken, ^
   {
       handler = [[HTProtocolHandler alloc] init];
   });
    
    return handler;
}

#pragma mark - HTLoginProtocol

- (void)onHTLoginCompleted:(HTUser *)user custom:(NSDictionary *)customDictionary
{
    NSString *json = [NSString stringWithFormat:@"{\"platformId\":\"%@\", \"platformUserId\":\"%@\", \"token\":\"%@\", \"userId\":\"%@\"}", user.platformId, user.platformUserId, user.token, user.userId ];
    NSLog(@"Allen Debug : => %@", json);
    UnitySendMessage(U3DReceive, "LoginThridPartySuc", S2CString(json));
}

- (void)onHTLoginFailed:(HTError *)error
{
    NSLog(@"登录失败=%@", error.message);
    UnitySendMessage(U3DReceive,"LoginCacel", S2CString(error.message) );
}

#pragma mark - HTLogoutProtocol

- (void)onHTLogoutCompleted:(HTUser *)user custom:(NSDictionary *)customDictionary
{
    NSLog(@"*******登出成功*******");
    UnitySendMessage(U3DReceive,"LogoutThridParty", "OK");
}

- (void)onHTLogoutFailed:(HTError *)error
{
    NSLog(@"********登出失败*******");
}

#pragma mark - HTPayProtocol

- (void)onHTPayCompleted:(NSDictionary *)customDictionary
{
    NSLog(@"购买成功");
    UnitySendMessage(U3DReceive,"PayResultCallBack", "ok" );
}

- (void)onHTPayFailed:(HTError *)error
{
    NSLog(@"购买失败");
    UnitySendMessage(U3DReceive,"PayResultCallBack", "fail" );
}

@end
