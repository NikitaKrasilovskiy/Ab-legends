//
//  ActionButton.m
//  devtodev
//
//  Created by Aleksey Kornienko on 07/09/16.
//  Copyright © 2016 devtodev. All rights reserved.
//

#import "DTDActionButton.h"
#import "DTDActionButton+Internal.h"

#define ACTION_URL                  @"_u"
#define ACTION_SHARE                @"_s"
#define ACTION_DEEPLINK             @"_d"

@interface DTDActionButton() <NSCoding>

@property (nonatomic) NSDictionary * payload;

@end

@implementation DTDActionButton

- (void)encodeWithCoder:(NSCoder *)aCoder {
    [aCoder encodeObject:self.payload forKey:@"payload"];
}

- (id)initWithCoder:(NSCoder *)aDecoder
{
    if (self = [super init])
    {
        self.payload = [aDecoder decodeObjectForKey:@"payload"];
    }
    
    return self;
}

- (id) initWithPayload:(NSDictionary *)payload {
    if (self = [super init]) {
        self.payload = payload;
    }
    
    return self;
}

- (DTDActionType) actionType {
    if ([self.payload objectForKey:ACTION_URL]) {
        return Url;
    } else if ([self.payload objectForKey:ACTION_SHARE]) {
        return Share;
    } else if ([self.payload objectForKey:ACTION_DEEPLINK]) {
        return Deeplink;
    }
    
    return App;
}

- (NSString *) actionString {
    return [self actionStringByType:[self actionType]];
}

- (NSString *) actionStringByType: (DTDActionType) actionType {
    switch (actionType) {
        case Url: return [self.payload objectForKey:ACTION_URL];
        case Share: return [self.payload objectForKey:ACTION_SHARE];
        case Deeplink: return [self.payload objectForKey:ACTION_DEEPLINK];
        default: return nil;
    }
}

- (BOOL) destructive {
    return [self.payload objectForKey:@"destructive"] != nil && [[self.payload objectForKey:@"destructive"] boolValue] == YES;
}

- (NSString *) buttonId {
    return [self.payload objectForKey:@"id"];
}

- (NSString *) text {
    return [self.payload objectForKey:@"text"];
}

- (UIUserNotificationActivationMode) activationMode {
    id mode = [self.payload objectForKey:@"activationMode"];
    if ([mode isEqualToString:@"background"]) {
        return UIUserNotificationActivationModeBackground;
    }
    
    return UIUserNotificationActivationModeForeground;
}

- (UIUserNotificationAction *) uiNotificationAction {
    UIMutableUserNotificationAction * action = [[UIMutableUserNotificationAction alloc] init];
    [action setActivationMode:self.activationMode];
    [action setTitle:self.text];
    [action setIdentifier:self.buttonId];
    [action setDestructive:self.destructive];
    [action setAuthenticationRequired:NO];
    
    return action;
}

- (UNNotificationAction *) unNotificationAction {
    UNNotificationActionOptions options = UNNotificationActionOptionNone;
    if (self.activationMode == UIUserNotificationActivationModeForeground) {
        options = UNNotificationActionOptionForeground;
    }
    
    return [UNNotificationAction actionWithIdentifier:self.buttonId title:self.text options:options];
}

- (NSDictionary*) json {
    NSDictionary* dict = [NSMutableDictionary new];
    [dict setValue:[NSNumber numberWithInteger:(int)[self actionType]] forKey:@"actionType"];
    if ([self actionString]) {
        [dict setValue:[self actionString] forKey:@"actionString"];
    }
    [dict setValue:[self buttonId] forKey:@"id"];
    NSString* encodedText = [[self text] stringByAddingPercentEscapesUsingEncoding:
                            NSUTF8StringEncoding];
    [dict setValue:encodedText forKey:@"text"];
    return dict;
}

@end
