//
//  UMBUnityWrapper.m
//  UMBCrossPromo
//
//  Created by Ivan Isaev on 08/09/16.
//  Copyright © 2016 umbrella. All rights reserved.
//

#import "UMBUnityWrapper.h"
#import <UMBCrossPromo/UMBCrossPromo.h>

extern "C" {
    void UnitySendMessage(const char *, const char *, const char *);
    
    NSString* UMBCrossPromoCreateNSString (const char* string) {
        return string ? [NSString stringWithUTF8String: string] : [NSString stringWithUTF8String: ""];
    }
    
    char* UMBCrossPromoMakeUnityStringCopy (const char* string) {
        if (string == NULL)
            return NULL;
        char* res = (char*)malloc(strlen(string) + 1);
        strcpy(res, string);
        return res;
    }
    void _UMBCrossSetListenerObjectName(const char *objectName);
    void _UMBCrossPromoTrack(const char *bundleId);
    void _UMBCrossPromoShow(const char *bundleId);
    void _UMBCrossPromoHide();
}

@interface UMBUnityWrapper () <UMBCrossPromoDelegate>

@property (strong) NSString *gameObjectName;

@end


@implementation UMBUnityWrapper

+ (UMBUnityWrapper *)instance
{
    static UMBUnityWrapper *shared = nil;
    static dispatch_once_t onceToken = 0;
    dispatch_once(&onceToken, ^{
        shared = [[UMBUnityWrapper alloc] init];
    });
    return shared;
}

-(void)setListenerGameObjectName:(NSString*)objectName
{
    self.gameObjectName=objectName;
}

-(void)showCrossPromoWithBundleId:(NSString*)bundleId
{
    [UMBCrossPromo instance].delegate=self;
    [[UMBCrossPromo instance] showCrossPromoWithBundleId: bundleId];
}

-(void)trackInstallWithBundleId:(NSString*)bundleId
{
    [UMBCrossPromo instance].delegate=self;
    [[UMBCrossPromo instance] trackInstallWithBundleId: bundleId andCompletionBlock:^(BOOL sucessful, NSError *error) {
        if (sucessful) {
            if ([self.gameObjectName length]) {
                UnitySendMessage([self.gameObjectName UTF8String], "callbackDidTrackInstall", "");
            }
        } else {
            if ([self.gameObjectName length]) {
                UnitySendMessage([self.gameObjectName UTF8String], "callbackDidFailToTrackInstall", error?[[error localizedDescription] UTF8String]:"");
            }
        }
    }];
}

-(void)showCrossPromo
{
    [UMBCrossPromo instance].delegate=self;
    [[UMBCrossPromo instance] showCrossPromo];
}

-(void)hideCrossPromo
{
    [UMBCrossPromo instance].delegate=self;
    [[UMBCrossPromo instance] hideCrossPromo];
}

-(void)umbCrosspromoDidLoad
{
    if ([self.gameObjectName length]) {
        UnitySendMessage([self.gameObjectName UTF8String], "callbackDidLoadCrossPromo", "");
    }
}

-(void)umbCrosspromoDidFailToLoadWithError:(NSError*)error
{
    if ([self.gameObjectName length]) {
        UnitySendMessage([self.gameObjectName UTF8String], "callbackDidFailToLoadWithError", error?[[error localizedDescription] UTF8String]:"");
    }
}

-(void)umbCrosspromoDidClose
{
    if ([self.gameObjectName length]) {
        UnitySendMessage([self.gameObjectName UTF8String], "callbackDidClose", "");
    }
}

-(void)umbCrosspromoDidOpenStoreForAppWithId:(NSString*)appid
{
    if ([self.gameObjectName length]) {
        UnitySendMessage([self.gameObjectName UTF8String], "callbackDidOpenStoreForAppWithId", [appid length]?[appid UTF8String]:"");
    }
}

-(void)umbCrosspromoDidCloseStore
{
    if ([self.gameObjectName length]) {
        UnitySendMessage([self.gameObjectName UTF8String], "callbackDidCloseStore", "");
    }
}

-(void)umbCrosspromoDidCallAction:(NSString*)action withParameters:(NSDictionary*)parameters
{
    if ([self.gameObjectName length] && [action length]) {
        NSString *url=[NSString stringWithFormat:@"ucp://%@", action];
        if ([parameters count]) {
            NSMutableArray *params=[NSMutableArray array];
            for (NSString *key in parameters) {
                [params addObject:[NSString stringWithFormat:@"%@=%@", key, [parameters objectForKey:key]]];
            }
            url=[url stringByAppendingFormat:@"?%@",[params componentsJoinedByString:@"&"]];
        }
        
        UnitySendMessage([self.gameObjectName UTF8String], "callbackDidCallAction", [url UTF8String]);
    }
}

@end

void _UMBCrossSetListenerObjectName(const char *objectName) {
    [[UMBUnityWrapper instance] setListenerGameObjectName: UMBCrossPromoCreateNSString(objectName)];
}

void _UMBCrossPromoShow(const char *bundleId) {
    [[UMBUnityWrapper instance] showCrossPromoWithBundleId: UMBCrossPromoCreateNSString(bundleId) ];
}

void _UMBCrossPromoHide() {
    [[UMBUnityWrapper instance] hideCrossPromo];
}

void _UMBCrossPromoTrack(const char *bundleId) {
    [[UMBUnityWrapper instance] trackInstallWithBundleId: UMBCrossPromoCreateNSString(bundleId) ];
}


