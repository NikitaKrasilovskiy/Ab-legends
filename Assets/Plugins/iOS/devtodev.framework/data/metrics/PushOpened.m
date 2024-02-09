#import "PushOpened.h"

NSString * const DTD_PUSH_ID = @"pushId";
NSString * const DTD_ACTION_ID = @"button";
NSString * const PUSH_DATA = @"push_data";

@implementation PushOpened

-(id) initWithPushId:(NSInteger)pushId {
    return [self initWithPushId:pushId andActionId:nil];
}

-(id) initWithPushId:(NSInteger)pushId andActionId:(NSString *)actionId {
    if (self = [super initWithName:@"Push Open" andCode:@"po"])
    {
        [self putPushId: pushId];
        [self putActionId: actionId];
        [self putTimestamp];
    }
    
    return self;
}

-(void) putPushId: (NSInteger) pushId {
    [self addParameterWithKey:DTD_PUSH_ID andValue:[NSNumber numberWithInteger:pushId]];
}

-(void) putActionId: (NSString *) actionId {
    if (actionId) {
        [self addParameterWithKey:DTD_ACTION_ID andValue:actionId];
    }
}

-(void) putPushData: (NSString*) pushData
{
    [self addParameterWithKey:PUSH_DATA andValue:pushData];
}

@end
