//
//  HTError.h
//  HTGameProxy
//
//  Created by zbflying on 14-9-25.
//  Copyright (c) 2014年 上海黑桃互动网络科技有限公司. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface HTError : NSObject

@property (nonatomic, strong) NSString *code;
@property (nonatomic, strong) NSString *message;

/**
 *  转换为字符串
 *  @return 字符串
 * */
- (NSString *)stringValue;

@end
