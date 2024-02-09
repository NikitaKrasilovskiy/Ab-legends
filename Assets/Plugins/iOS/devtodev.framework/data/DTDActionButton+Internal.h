#import "DTDActionButton.h"

@interface DTDActionButton ()

@property (nonatomic, readonly) UIUserNotificationActivationMode activationMode;

- (id) initWithPayload: (NSDictionary *) payload;

- (UIUserNotificationAction *) uiNotificationAction;

- (UNNotificationAction *) unNotificationAction;

@end
