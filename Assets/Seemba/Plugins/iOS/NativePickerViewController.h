//
//  PickerViewController.h
//  PickerTest
//
//  Created by Yevhen Paschenko on 4/13/13.
//  Copyright (c) 2013 Yevhen Paschenko. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "NativePickerViewControllerDelegate.h"

typedef enum {
    PickerViewControllerModeCustom,
    PickerViewControllerModeDate,
    PickerViewControllerModeTime,
    PickerViewControllerModeDateTime
} PickerViewControllerMode;

@interface NativePickerViewController : UIViewController<UIPickerViewDataSource, UIPickerViewDelegate>

- (id)initPickerViewController;

@property (retain, nonatomic) IBOutlet UIPickerView *picker;
@property (retain, nonatomic) IBOutlet UIDatePicker *datePicker;
@property (assign, nonatomic) PickerViewControllerMode mode;
@property (retain, nonatomic) NSObject<NativePickerViewControllerDelegate>* delegate;
@property (retain, nonatomic) NSArray* itemList;
@property (assign) int64_t selectedItem;

- (void)selectCurrentValue;

@end
