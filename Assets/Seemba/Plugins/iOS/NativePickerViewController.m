//
//  PickerViewController.m
//  PickerTest
//
//  Created by Yevhen Paschenko on 4/13/13.
//  Copyright (c) 2013 Yevhen Paschenko. All rights reserved.
//

#import "NativePickerViewController.h"

@interface NativePickerViewController () {
    NSDateFormatter* _dateFormatter;
    NSString* _dateFormatString;
    NSString* _timeFormatString;
    NSString* _dateTimeFormatString;
    
    int64_t _selectedItem;
}

@end

@implementation NativePickerViewController

- (id)initPickerViewController
{
    self = [super initWithNibName:@"NativePickerViewController" bundle:nil];
    if (self) {
         _dateFormatter = [[NSDateFormatter alloc] init];        
        
        _dateFormatString = @"yyyy-MM-dd";
        _timeFormatString = @"HH:mm";
        _dateTimeFormatString = @"yyyy-MM-dd HH:mm";
    }
    return self;
}

- (void)setMode:(PickerViewControllerMode)mode {
    _mode = mode;
    
    self.picker.hidden = mode != PickerViewControllerModeCustom;
    
    self.datePicker.hidden = mode == PickerViewControllerModeCustom;
    self.datePicker.timeZone = [NSTimeZone timeZoneWithAbbreviation:@"GMT"];
    
    if (mode == PickerViewControllerModeDate) {
        self.datePicker.datePickerMode = UIDatePickerModeDate;
        [_dateFormatter setDateFormat:_dateFormatString];
    } else if (mode == PickerViewControllerModeTime) {
        self.datePicker.datePickerMode = UIDatePickerModeTime;
        [_dateFormatter setDateFormat:_timeFormatString];
    } else if (mode == PickerViewControllerModeDateTime) {
        self.datePicker.datePickerMode = UIDatePickerModeDateAndTime;
        [_dateFormatter setDateFormat:_timeFormatString];
    }
}

- (int64_t)selectedItem {
    return _selectedItem;
}

- (void)setSelectedItem:(int64_t)selectedItem {
    if (self.mode == PickerViewControllerModeDate || self.mode == PickerViewControllerModeTime || self.mode == PickerViewControllerModeDateTime) {
        [self.datePicker setDate:[NSDate dateWithTimeIntervalSince1970:selectedItem] animated:YES];
    } else {
        [self.picker selectRow:selectedItem inComponent:0 animated:YES];
    }
    
    _selectedItem = selectedItem;
}

- (void)setItemList:(NSArray *)itemList {
    _itemList = itemList;
    
    [self.picker reloadAllComponents];
}


- (void)viewDidLoad
{
    [super viewDidLoad];
	// Do any additional setup after loading the view.
    
    self.mode = PickerViewControllerModeTime;
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

// returns the number of 'columns' to display.
- (NSInteger)numberOfComponentsInPickerView:(UIPickerView *)pickerView
{
    return 1;
}

// returns the # of rows in each component..
- (NSInteger)pickerView:(UIPickerView *)pickerView numberOfRowsInComponent:(NSInteger)component
{
    return self.itemList.count;
}

- (NSString *)pickerView:(UIPickerView *)pickerView titleForRow:(NSInteger)row forComponent:(NSInteger)component
{
    return [self.itemList objectAtIndex:row];
}

- (void)pickerView:(UIPickerView *)pickerView didSelectRow:(NSInteger)row inComponent:(NSInteger)component
{
    NSLog(@"Selected component: %i row: %i", component, row);
    
    if (self.delegate != nil) {
        [self.delegate didSelectedValue:[self.itemList objectAtIndex:row] atIndex:row];
    }
}

- (IBAction)onDatePickerValueChanged:(id)sender {
    
    NSString* value = [_dateFormatter stringFromDate:self.datePicker.date];
    NSLog(@"Selected date %@", value);
    
    if (self.delegate != nil) {
        double seconds = [self.datePicker.date timeIntervalSince1970];
        [self.delegate didSelectedValue:value atIndex:seconds];
    }
}

- (IBAction)onDone:(id)sender {
    if (self.delegate != nil) {
        //[self.delegate onDoneWithValue:];
        
        if (self.mode == PickerViewControllerModeCustom) {
            int index = [self.picker selectedRowInComponent:0];
            [self.delegate onDoneWithIndex:index];
        } else {
            double seconds = [self.datePicker.date timeIntervalSince1970];
            [self.delegate onDoneWithIndex:seconds];
        }
    }
}

- (void)viewDidUnload {
    [self setPicker:nil];
    [self setDatePicker:nil];
    [super viewDidUnload];
}

- (void)selectCurrentValue {
    if (_mode == PickerViewControllerModeCustom) {
        int row = [self.picker selectedRowInComponent:0];
        [self.delegate didSelectedValue:[self.itemList objectAtIndex:row] atIndex:row];
    } else {
        NSString* value = [_dateFormatter stringFromDate:self.datePicker.date];
        
        double seconds = [self.datePicker.date timeIntervalSince1970];
        [self.delegate didSelectedValue:value atIndex:seconds];
    }
}
@end
