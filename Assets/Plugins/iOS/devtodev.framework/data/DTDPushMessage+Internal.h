#import "DTDPushMessage.h"
#import "DTDNotificationCategory.h"

@interface DTDPushMessage ()

@property (nonatomic) NSUInteger systemId;
@property (nonatomic) BOOL apiSource;
@property (nonatomic) DTDNotificationCategory * notificationCategory;

- (id) initWithRemoteMessage:(NSDictionary *) remoteMessage;

- (BOOL) isContentAvailable;

- (NSDictionary*) json;

@end
