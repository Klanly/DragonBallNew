//
//  SpadeAPI.m
//  Unity-iPhone
//
//  Created by 阿飞的头脑壳 on 14-10-2.
//
//

#import <Foundation/Foundation.h>
#import "HTProtocolHandler.h"
#import "HTGameProxy.h"
#import "Utility.h"
#import "HTKeys.h"

@interface SpadeAPI : NSObject

+ (void)login;
+ (void)logout;
+ (void)purchaseInApp:(HTPayInfo *) payInfo;

@end



@implementation SpadeAPI

+ (void)login
{
    [HTGameProxy doLogin:nil delegate:[HTProtocolHandler handler]];
}

+ (void)logout
{
    [HTGameProxy doLogout:nil delegate:[HTProtocolHandler handler]];
}

+ (void)purchaseInApp:(HTPayInfo *) payInfo
{
    [HTGameProxy doPay:payInfo delegate:[HTProtocolHandler handler]];
}

@end

#if defined (__cplusplus)
extern "C" {
#endif
    void loginSpade() {
        [SpadeAPI login];
    }
    
    bool CheckGame() {
        BOOL result = [HTGameProxy onStartGame];
        NSLog(@"CheckGame = %@", result ? @"YES" : @"NO");
        return result;
    }
    
    void EnterGame(const char * serverId, const char* serverName, const char* roleId, const char * roleName, int level, bool isNew) {
        
        NSString * newRole = [NSString stringWithFormat:@"%@", isNew ? @"1" : @"0"];
        
        NSDictionary *dic = @{KEY_CP_SERVER_ID :   CS2String(serverId),
                              KEY_CP_SERVER_NAME : CS2String(serverName),
                              KEY_ROLE_ID :        CS2String(roleId),
                              KEY_ROLE_NAME :      CS2String(roleName),
                              KEY_ROLE_LEVEL :     [NSString stringWithFormat: @"%d", level],
                              KEY_IS_NEW_ROLE :    newRole};
        
        [HTGameProxy onEnterGame:dic];
    }
    
    void logoutSpade() {
        [SpadeAPI logout];
    }
    
    //price 价格， count 物品个数
    void Purchase(int price, int count, const char* productId, const char* serverId, const char* productName, const char* productDes,const char * url, const char * extend) {
        HTPayInfo *payInfo = [[HTPayInfo alloc] init];
        payInfo.price = price;
        payInfo.rate  = count/price;
        payInfo.count = count;
        payInfo.fixedMoney= 1;
        payInfo.unitName  = @"钻石";
        payInfo.productId = CS2String(productId);
        payInfo.serverId  = CS2String(serverId);
        payInfo.name = CS2String(productName);
        payInfo.callbackUrl = nil;//CS2String(url);
        payInfo.description = CS2String(productDes);
        payInfo.cpExtendInfo = CS2String(extend);
        payInfo.custom = @{KEY_USER_BALANCE : @"10",
                           KEY_VIP_LEVEL  : @"1",
                           KEY_ROLE_LEVEL : @"0",
                           KEY_USER_PARTY : @"公会",
                           KEY_IS_PAY_MONTH : @"0",
                           KEY_COIN_IMAGE_NAME : @"icon.png"};
        
        [SpadeAPI purchaseInApp:payInfo];
    }


#if defined (__cplusplus)
}
#endif