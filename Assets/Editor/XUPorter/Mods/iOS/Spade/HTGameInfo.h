//
//  HTGameInfo.h
//  HTGameProxy
//
//  Created by zbflying on 14-9-26.
//  Copyright (c) 2014年 上海黑桃互动网络科技有限公司. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface HTGameInfo : NSObject

enum HTDirection
{
    Landscape,								//横屏
    Portrait								//竖屏
};

typedef enum HTDirection HTDirection;

@property (nonatomic, strong) NSString *name;			//游戏名称
@property (nonatomic, strong) NSString *shortName;		//游戏简称 e.g. MHJ
@property (nonatomic) HTDirection direction;			//游戏屏幕方向

/**
 *  转换为字符串
 *  @return 字符串
 * */
- (NSString *)stringValue;

@end
