//
//  UMBCrossPromo.h
//  UMBCrossPromo
//
//  Created by Ivan Isaev on 07/09/16.
//  Copyright © 2016 umbrella. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UMBCrossPromo/UMBTypes.h>

@class UMBCrossPromo;

@protocol UMBCrossPromoDelegate <NSObject>

-(void)umbCrosspromoDidLoad;
-(void)umbCrosspromoDidFailToLoadWithError:(NSError*)error;
-(void)umbCrosspromoDidClose;

-(void)umbCrosspromoDidOpenStoreForAppWithId:(NSString*)appid;
-(void)umbCrosspromoDidCloseStore;

@optional
-(void)umbCrosspromoDidCallAction:(NSString*)action withParameters:(NSDictionary*)parameters;

@end

@interface UMBCrossPromo : NSObject

@property (weak) id<UMBCrossPromoDelegate> delegate;

+ (UMBCrossPromo *)instance;

-(void)trackInstallWithBundleId:(NSString*)bundleId andCompletionBlock:(UMBSimpleNetworkReturnBlock)completionBlock;
-(void)trackInstallWithCompletionBlock:(UMBSimpleNetworkReturnBlock)completionBlock;

-(void)showCrossPromo;
-(void)showCrossPromoWithBundleId:(NSString*)bundleId;;
-(void)hideCrossPromo;

@end
