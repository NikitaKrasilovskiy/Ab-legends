#import "DTDNotificationCategory.h"
#import "DTDActionButton.h"
#import "DTDActionButton+Internal.h"

@interface DTDNotificationCategory() <NSCoding>

@end

@implementation DTDNotificationCategory

- (void)encodeWithCoder:(NSCoder *)aCoder {
    [aCoder encodeObject:self.categoryName forKey:@"id"];
    [aCoder encodeObject:self.actionButtons forKey:@"buttons"];
}

- (id)initWithCoder:(NSCoder *)aDecoder
{
    if (self = [super init])
    {
        self.categoryName = [aDecoder decodeObjectForKey:@"id"];
        self.actionButtons = [[NSMutableArray alloc] init];
        [self.actionButtons setArray:[aDecoder decodeObjectForKey:@"buttons"]];
    }
    
    return self;
}

- (id) initWithPayload:(NSDictionary *)payload {
    if (self = [super init]) {
        self.categoryName = [payload objectForKey:@"id"];
        self.actionButtons = [[NSMutableArray alloc] init];
        
        for (id button in [payload objectForKey:@"buttons"]) {
            [self.actionButtons addObject:[[DTDActionButton alloc] initWithPayload:button]];
        }
    }
    
    return self;
}

- (UIMutableUserNotificationCategory *) uiNotificationCategory {
    NSMutableArray * buttons = [[NSMutableArray alloc] init];
    for (DTDActionButton * button in self.actionButtons) {
        [buttons addObject:[button uiNotificationAction]];
    }
    
    UIMutableUserNotificationCategory *category = [[UIMutableUserNotificationCategory alloc] init];
    [category setIdentifier:self.categoryName];
    [category setActions:buttons forContext:UIUserNotificationActionContextDefault];
    [category setActions:buttons forContext:UIUserNotificationActionContextMinimal];
    
    return category;
}

- (UNNotificationCategory *) unNotificationCategory {
    NSMutableArray *actions = [NSMutableArray array];
    for (DTDActionButton *action in self.actionButtons) {
        [actions addObject:[action unNotificationAction]];
    }
    
    return [UNNotificationCategory categoryWithIdentifier:self.categoryName
                                                  actions:actions
                                        intentIdentifiers:@[]
                                                  options:UNNotificationCategoryOptionNone];
}

@end
