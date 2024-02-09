#import "PushTokenSend.h"

NSString * const TOKEN_KEY = @"token";

@implementation PushTokenSend

-(id) initWithToken:(NSString *) token
{
	self = [super initWithName:@"Token Send" andCode:@"pt"];
	
	if (self) {
		[self putToken:token];
	}
	
	return self;
}

-(void) putToken: (NSString *) token {
	[self addParameterWithKey:TOKEN_KEY andValue:token];
}

@end
