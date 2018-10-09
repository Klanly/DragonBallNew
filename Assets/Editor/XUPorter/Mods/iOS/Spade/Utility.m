//
//  Utility.m
//  Unity-iPhone
//
//  Created by 阿飞的头脑壳 on 14-10-3.
//
//

#import <Foundation/Foundation.h>
#import "Utility.h"

//把NSString 转化为 char*
__strong const char * S2CString(NSString *str)
{
    const char * utf8str = [str UTF8String];
    int length = strlen(utf8str)+1;
    char *buf = (char *)malloc(length);
    memset(buf, 0, length);
    strcpy(buf, utf8str);
    return buf;
}

//把char* 转化为NSString
NSString *CS2String(const char *cString)
{
    if(cString)
        return [NSString stringWithCString:cString encoding:NSUTF8StringEncoding];
    else
        return @"";
}