#import "PilloFramework.h"

extern "C" {
  PilloFramework * pilloFramework = nil;

  void PilloInitialize() {
    pilloFramework = [PilloFramework new];
    [pilloFramework initialize];
  }
}

@implementation PilloFramework

- (void)initialize {
  NSLog(@"PilloFramework Initialized!");
}

@end
