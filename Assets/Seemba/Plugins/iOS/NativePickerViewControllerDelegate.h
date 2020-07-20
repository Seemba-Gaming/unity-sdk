//
//  PickerViewControllerDelegate.h
//  PickerTest
//
//  Created by Yevhen Paschenko on 4/13/13.
//  Copyright (c) 2013 Yevhen Paschenko. All rights reserved.
//

#import <Foundation/Foundation.h>

@protocol NativePickerViewControllerDelegate <NSObject>

@required
- (void)didSelectedValue:(NSString*)value atIndex:(NSInteger)index;
- (void)onDoneWithIndex:(NSInteger)index;

@end
