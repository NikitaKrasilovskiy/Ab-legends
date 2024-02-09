#import "Metric.h"

extern NSString * const DTD_PUSH_ID;
extern NSString * const DTD_ACTION_ID;

@interface PushOpened : Metric

-(id) initWithPushId: (NSInteger) pushId;

-(id) initWithPushId: (NSInteger) pushId andActionId:(NSString *) actionId;

-(void) putPushData: (NSString*) pushData;

@end
