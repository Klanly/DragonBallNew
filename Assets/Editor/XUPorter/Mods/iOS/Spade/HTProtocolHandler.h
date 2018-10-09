//
//  HTProtocolHandler.h
//  HTGameProxyDemo
//
//  Created by zbflying on 14-10-2.
//  Copyright (c) 2014年 上海黑桃互动网络科技有限公司. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "HTProtocol.h"

@interface HTProtocolHandler : NSObject<HTLoginProtocol, HTLogoutProtocol, HTPayProtocol>

+ (instancetype)handler;

@end
