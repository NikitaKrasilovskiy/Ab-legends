#import "Metric.h"

extern NSString * const TOKEN_KEY;

@interface PushTokenSend : Metric

-(id) initWithToken: (NSString *) token;

@end
