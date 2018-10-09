//
//  HTPayInfo.h
//  HTGameProxy
//
//  Created by zbflying on 14-9-25.
//  Copyright (c) 2014年 上海黑桃互动网络科技有限公司. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface HTPayInfo : NSObject

@property (nonatomic) float price;						//单价
@property (nonatomic) int rate;							//兑换比例
@property (nonatomic) int count;						//个数
@property (nonatomic) BOOL fixedMoney;					//是否定额
@property (nonatomic, strong) NSString *unitName;		//货币单位
@property (nonatomic, strong) NSString *productId;		//产品ID
@property (nonatomic, strong) NSString *serverId;		//服务器ID
@property (nonatomic, strong) NSString *name;			//商品名称
@property (nonatomic, strong) NSString *callbackUrl;	//回调地址
@property (nonatomic, strong) NSString *description;	//商品描述
@property (nonatomic, strong) NSString *cpExtendInfo;	//CP扩展信息
@property (nonatomic, strong) NSDictionary *custom;		//自定义信息

/**
 *  转换为字符串
 *  @return 字符串
 * */
- (NSString *)stringValue;

@end
