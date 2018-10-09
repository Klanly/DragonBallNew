//
//  HTUser.h
//  HTGameProxy
//
//  Created by zbflying on 14-9-25.
//  Copyright (c) 2014年 上海黑桃互动网络科技有限公司. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface HTUser : NSObject

@property (nonatomic, strong) NSString *userId;					//黑桃用户ID
@property (nonatomic, strong) NSString *platformUserId;			//平台用户ID
@property (nonatomic, strong) NSString *platformId;				//平台ID
@property (nonatomic, strong) NSString *token;					//登录Token
@property (nonatomic, strong) NSDictionary *custom;				//自定义参数信息

/**
 *  转换为字符串
 *  @return 字符串
 * */
- (NSString *)stringValue;

@end
