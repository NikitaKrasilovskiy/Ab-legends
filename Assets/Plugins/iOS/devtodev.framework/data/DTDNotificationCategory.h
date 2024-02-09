//
//  PushCategory.h
//  devtodev
//
//  Created by Aleksey Kornienko on 09/09/16.
//  Copyright © 2016 devtodev. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <UserNotifications/UserNotifications.h>

@interface DTDNotificationCategory : NSObject

@property (nonatomic) NSString * categoryName;
@property (nonatomic) NSMutableArray * actionButtons;

- (id) initWithPayload: (NSDictionary *) payload;

- (UIUserNotificationCategory *) uiNotificationCategory;

- (UNNotificationCategory *) unNotificationCategory;

@end
