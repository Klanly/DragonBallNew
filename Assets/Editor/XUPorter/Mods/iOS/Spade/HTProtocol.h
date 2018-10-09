//
//  HTProtocol.h
//  HTGameProxy
//
//  Created by zbflying on 14-9-25.
//  Copyright (c) 2014年 上海黑桃互动网络科技有限公司. All rights reserved.
//

#ifndef HTGameProxy_HTProtocol_h
#define HTGameProxy_HTProtocol_h

#import <Foundation/Foundation.h>
@class HTUser, HTError;

@protocol HTLoginProtocol <NSObject>

@required
- (void)onHTLoginCompleted:(HTUser *)user custom:(NSDictionary *)customDictionary;
- (void)onHTLoginFailed:(HTError *)error;

@end


@protocol HTLogoutProtocol <NSObject>

@required
- (void)onHTLogoutCompleted:(HTUser *)user custom:(NSDictionary *)customDictionary;
- (void)onHTLogoutFailed:(HTError *)error;

@end

@protocol HTPayProtocol <NSObject>

@required
- (void)onHTPayCompleted:(NSDictionary *)customDictionary;
- (void)onHTPayFailed:(HTError *)error;

@end

#endif
