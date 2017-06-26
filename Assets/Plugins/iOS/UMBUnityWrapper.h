//
//  UMBUnityWrapper.h
//  UMBCrossPromo
//
//  Created by Ivan Isaev on 08/09/16.
//  Copyright © 2016 umbrella. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface UMBUnityWrapper : NSObject

+ (UMBUnityWrapper *)instance;

-(void)setListenerGameObjectName:(NSString*)objectName;
-(void)showCrossPromoWithBundleId:(NSString*)bundleId;
-(void)showCrossPromo;
-(void)hideCrossPromo;

@end