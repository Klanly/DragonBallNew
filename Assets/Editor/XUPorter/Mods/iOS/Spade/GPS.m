////
////  GPS.m
////  Unity-iPhone
////
////  Created by Ban on 14-11-11.
////
////
//
#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <CoreLocation/CoreLocation.h>

@interface GPS : NSObject<CLLocationManagerDelegate>
{
    CLLocationManager *locManager;
}

@property (retain, nonatomic) CLLocationManager *locManager;
-(void) StartGPS;
-(void) StopGPS;


@end


static GPS* instance = nil;

@implementation GPS
@synthesize locManager;



+(GPS *)sharedGPS{
    
    @synchronized(self){  //为了确保多线程情况下，仍然确保实体的唯一性
        
        if (!instance) {
            
            [[self alloc] init]; //该方法会调用 allocWithZone
            
        }
        
    }
    
    return instance;
    
}



+(id)allocWithZone:(NSZone *)zone{
    
    @synchronized(self){
        
        if (!instance) {
            
            instance = [super allocWithZone:zone]; //确保使用同一块内存地址
            
            return instance;
            
        }
        
    }
    
    return nil;
    
}



- (id)copyWithZone:(NSZone *)zone;{
    
    return self; //确保copy对象也是唯一
    
}



-(id)retain{
    
    return self; //确保计数唯一
    
}



- (unsigned)retainCount

{
    
    return UINT_MAX;  //装逼用的，这样打印出来的计数永远为-1
    
}



- (id)autorelease

{
    
    return self;//确保计数唯一
    
} 



- (oneway void)release

{
    
    //重写计数释放方法
    
}




-(void) StartGPS
{
//    NSLog(@"StartGPS start!!!!!!!!!!!!!!!!!!!!!!!!");
    
    if(locManager == nil)
    {
        //初始化位置管理器
        locManager = [[CLLocationManager alloc]init];
    }
    else
    {
         [locManager stopUpdatingLocation];
    }
  
    //设置代理
    locManager.delegate = [GPS sharedGPS];
    //设置位置经度
    locManager.desiredAccuracy = kCLLocationAccuracyKilometer;
    //设置每隔500米更新位置
    locManager.distanceFilter = 500;
    
    if([self.locManager respondsToSelector:@selector(requestAlwaysAuthorization)])
    {
        [self.locManager requestAlwaysAuthorization]; // 永久授权
 //       [self.locManager requestWhenInUseAuthorization]; //使用中授权
    }
    [self.locManager startUpdatingLocation];
}


-(void) StopGPS
{
//    NSLog(@"StopGPS start~~~~~~~~~~~~~~~~~~~~~~~~~~~");
    [locManager stopUpdatingLocation];
    
    [locManager release];
}

//
//- (void)requestWhenInUseAuthorization __OSX_AVAILABLE_STARTING(__MAC_NA, __IPHONE_8_0);
//- (void)requestAlwaysAuthorization __OSX_AVAILABLE_STARTING(__MAC_NA, __IPHONE_8_0);


//协议中的方法，作用是每当位置发生更新时会调用的委托方法
-(void)locationManager:(CLLocationManager *)manager didUpdateToLocation:(CLLocation *)newLocation fromLocation:(CLLocation *)oldLocation
{
//    NSLog(@"didUpdateToLocation~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
    //结构体，存储位置坐标
    CLLocationCoordinate2D loc = [newLocation coordinate];
    float longitude = loc.longitude;
    float latitude = loc.latitude;
    

    NSString *str1 = [NSString stringWithFormat:@"%f", longitude];
    NSString *str2 = [NSString stringWithFormat:@"%s", "|"];
    NSString * str = [NSString stringWithFormat:@"%f", latitude];
    
    NSString *strFinal = [NSString stringWithFormat:@"%@%@%@", str1, str2, str];
    UnitySendMessage("Native", "GetGPSLocation", [strFinal cStringUsingEncoding:NSASCIIStringEncoding]);
}

//当位置获取或更新失败会调用的方法
-(void)locationManager:(CLLocationManager *)manager didFailWithError:(NSError *)error
{
    NSString *errorMsg = nil;
    if ([error code] == kCLErrorDenied) {
        errorMsg = @"访问被拒绝";
    }
    if ([error code] == kCLErrorLocationUnknown) {
        errorMsg = @"获取位置信息失败";
    }
    
    UIAlertView *alertView = [[UIAlertView alloc]initWithTitle:@"Location"
                                                       message:errorMsg delegate:self cancelButtonTitle:@"Ok"otherButtonTitles:nil, nil];
    [alertView show];

}

@end




#if defined (__cplusplus)
extern "C" {
#endif
    void _StartGPS()
    {
        [[GPS sharedGPS] StartGPS];
    }
    
    void _StopGPS()
    {
        [[GPS sharedGPS]StopGPS];
    }
    
#if defined (__cplusplus)
}
#endif