#import "DTDPushMessage.h"
#import "DTDPushMessage+Internal.h"
#import <UIKit/UIKit.h>

#define APS                         @"aps"
#define API                         @"_api"
#define MESSAGE_ID                  @"_k"
#define MESSAGE_INC_BADGE           @"_b"
#define MESSAGE_ACTIONS             @"_interactive"
#define MESSAGE_ALERT               @"alert"
#define MESSAGE_BADGE               @"badge"
#define MESSAGE_SOUND               @"sound"
#define MESSAGE_CONTENT_AVAILABLE   @"content-available"
#define MESSAGE_CATEGORY            @"category"
#define MESSAGE_TITLE               @"title"
#define MESSAGE_BODY                @"body"

#define ACTION_URL                  @"_u"
#define ACTION_SHARE                @"_s"
#define ACTION_DEEPLINK             @"_d"

#define MEDIA_ATTACHMENT            @"_mediaAttachments"

@interface DTDPushMessage()

@property (nonatomic) NSDictionary * payload;
@property (nonatomic) NSMutableDictionary * storedData;
@property (nonatomic) DTDActionType storedActionType;

@end

@implementation DTDPushMessage

- (id) initWithRemoteMessage:(NSDictionary *) remoteMessage {
    if (self = [super init]) {
        self.payload = remoteMessage;
    }
    return self;
}

- (NSInteger) badge {
    NSInteger currentBadge = [UIApplication sharedApplication].applicationIconBadgeNumber;
    
    id apsBadge = [[self.payload objectForKey:@"aps"] objectForKey:@"badge"];
    id badge = [self.payload objectForKey:MESSAGE_INC_BADGE];
    if (apsBadge) {
        return [apsBadge integerValue];
    } else if (badge) {
        return currentBadge + [badge integerValue];
    }
    
    return currentBadge;
}

- (NSString *) category {
    return [[self.payload objectForKey:@"aps"] objectForKey:@"category"];
}

- (NSUInteger) systemId {
    NSString * value = [self.payload objectForKey:MESSAGE_ID];
    return [value integerValue];
}

- (BOOL) apiSource {
    return [[self.payload objectForKey:API] boolValue];
}

- (DTDNotificationCategory *) notificationCategory {
    return  [[DTDNotificationCategory alloc] initWithPayload:[self.payload objectForKey:MESSAGE_ACTIONS]];
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

- (NSDictionary *) data {
    if (self.storedData) {
        return self.storedData;
    }
    
    self.storedData = [[NSMutableDictionary alloc] init];
    NSArray * excluded = @[APS, MESSAGE_ID, MESSAGE_INC_BADGE, MESSAGE_ACTIONS, MESSAGE_ALERT, MESSAGE_BADGE, MESSAGE_SOUND, MESSAGE_CONTENT_AVAILABLE, MESSAGE_CATEGORY, MESSAGE_TITLE, API, MEDIA_ATTACHMENT, MESSAGE_BODY, ACTION_URL, ACTION_SHARE, ACTION_DEEPLINK];
    for (NSString * key in self.payload) {
        if (![excluded containsObject:key]) {
            NSString* encodedKey = [[self.payload objectForKey:key] stringByAddingPercentEscapesUsingEncoding:
                                                           NSUTF8StringEncoding];
            NSString* encodedValue = [key stringByAddingPercentEscapesUsingEncoding:
                                    NSUTF8StringEncoding];
            [self.storedData setObject:encodedKey forKey:encodedValue];
        }
    }
    
    return self.storedData;
}

- (NSString *) actionStringByType: (DTDActionType) actionType {
    switch (actionType) {
        case Url: return [self.payload objectForKey:ACTION_URL];
        case Share: return [self.payload objectForKey:ACTION_SHARE];
        case Deeplink: return [self.payload objectForKey:ACTION_DEEPLINK];
        default: return nil;
    }
}

- (BOOL) isContentAvailable {
    return [[[self.payload objectForKey:APS] objectForKey:MESSAGE_CONTENT_AVAILABLE] boolValue];
}

- (NSDictionary *) dictionary {
    return @{
             
             };
}

- (NSDictionary*) json {
    NSDictionary* dict = [[self data] mutableCopy];
    [dict setValue:[NSNumber numberWithInteger:(int)[self actionType]] forKey:@"actionType"];
    if ([self actionString]) {
        [dict setValue:[self actionString] forKey:@"actionString"];
    }
    return dict;
}

@end
