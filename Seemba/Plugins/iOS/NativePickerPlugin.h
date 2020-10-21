//
//  iOSPlugin.h
//  UnityIOSPlugin
//
//  Created by Yevhen Paschenko on 8/17/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "NativePickerViewControllerDelegate.h"

@interface NativePickerPlugin : NSObject<UIAlertViewDelegate, NativePickerViewControllerDelegate, UIPopoverControllerDelegate>
{
    int counter_;
    NSString* gameObject_;
}

@end
