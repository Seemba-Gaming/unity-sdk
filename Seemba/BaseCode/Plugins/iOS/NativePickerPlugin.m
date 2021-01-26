//
//  iOSPlugin.m
//  UnityIOSPlugin
//
//  Created by Yevhen Paschenko on 8/17/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "NativePickerPlugin.h"
#import "NativePickerViewController.h"

extern void UnitySendMessage(const char *, const char *, const char *);

@interface NativePickerPlugin () {
    NativePickerViewController* _pickerViewController;
    UIPopoverController* _pickerPopover;
    UIView* _glView;
    UILabel* _debugLabel;
    NSString* _selectedItem;
    UIButton* _closeButton;
}
@end

@implementation NativePickerPlugin

- (id)init {
    self = [super init];
    
    if (self) {
        UIWindow* window = [UIApplication sharedApplication].keyWindow;
        NSArray* views = window.subviews;
        
        for (UIView* view in views) {
            NSString* classString = NSStringFromClass([view class]);
            if ([classString isEqualToString:@"EAGLView"] || [classString isEqualToString:@"MainGLView"] || [classString isEqualToString:@"UnityView"]) {
                NSLog(@"found %@", classString);
                _glView = view;
                break;
            }
        }
        
        _pickerViewController = [[NativePickerViewController alloc] initPickerViewController];
        _pickerViewController.delegate = self;
        
        _closeButton = [[UIButton alloc] init];
        _closeButton.backgroundColor = [UIColor colorWithRed:1 green:0 blue:0 alpha:0];
        [_closeButton setTitle:@"" forState:UIControlStateNormal];
        [_closeButton addTarget:self action:@selector(onClosePickerButton:) forControlEvents:UIControlEventTouchUpInside];
    }
    
    return self;
}

- (void)showPicker:(int)type withArray:(NSArray*)array andSelectedItem:(int64_t)selectedItem atRect:(CGRect)rect andGameObject:(NSString*)gameObject{
    
    gameObject_ = [gameObject copy];
    
//    if (_debugLabel == nil) {
//        _debugLabel = [[UILabel alloc] initWithFrame:rect];
//        [_debugLabel setBackgroundColor:[UIColor whiteColor]];
//        [_glView addSubview:_debugLabel];
//    } else {
//        [_debugLabel setFrame:rect];
//    }
    
    if ([[UIDevice currentDevice] userInterfaceIdiom] == UIUserInterfaceIdiomPhone) {
        __block CGRect pickerFrame = _pickerViewController.view.frame;
        int thisHeight = _glView.frame.size.height;
        int thisWidth = _glView.frame.size.width;
        int pickerHeight = _pickerViewController.view.frame.size.height;
        int pickerWidth = thisWidth;
        
        _closeButton.frame = _glView.frame;
        
        pickerFrame.origin.x = (thisWidth - pickerWidth)/2;
        pickerFrame.size.width = pickerWidth;
        
        if ([_glView.subviews containsObject:_pickerViewController.view] == NO) {
            _pickerViewController.mode = type;
            _pickerViewController.itemList = array;
            _pickerViewController.selectedItem = selectedItem;
            
            pickerFrame.origin.y = thisHeight;
            _pickerViewController.view.frame = pickerFrame;
            [_glView addSubview:_closeButton];
            [_glView addSubview:_pickerViewController.view];
        }
        
        
        [UIView animateWithDuration:0.3 delay:0 options:UIViewAnimationOptionCurveEaseOut animations:^{
            pickerFrame.origin.y = thisHeight - pickerHeight;
            _pickerViewController.view.frame = pickerFrame;
        } completion:^(BOOL finished) {
            
        }];

    } else {
        if (_pickerPopover == nil) {
            _pickerPopover = [[UIPopoverController alloc] initWithContentViewController:_pickerViewController];
            _pickerPopover.popoverContentSize = _pickerViewController.view.frame.size;
            _pickerPopover.delegate = self;
        }
        
        _selectedItem = nil;
        
        _pickerViewController.mode = type;
        _pickerViewController.itemList = array;
        _pickerViewController.selectedItem = selectedItem;
        
        [_pickerPopover presentPopoverFromRect:rect inView:_glView permittedArrowDirections:UIPopoverArrowDirectionAny animated:YES];
    }
}

- (void)hidePicker {
    if ([[UIDevice currentDevice] userInterfaceIdiom] == UIUserInterfaceIdiomPhone) {
        __block CGRect frame = _pickerViewController.view.frame;
        CGRect thisFrame = _glView.frame;
        
        [_closeButton removeFromSuperview];
        
        [UIView animateWithDuration:0.3 delay:0 options:UIViewAnimationOptionCurveEaseIn animations:^{
            frame.origin.y = thisFrame.size.height;
            _pickerViewController.view.frame = frame;
        } completion:^(BOOL finished) {
            [_pickerViewController.view removeFromSuperview];
        }];
    }
}

+ (BOOL)isIphone {
    return [[UIDevice currentDevice] userInterfaceIdiom] == UIUserInterfaceIdiomPhone;
}

- (void)didSelectedValue:(NSString*)value atIndex:(NSInteger)index {
    _selectedItem = [NSString stringWithFormat:@"%d", index];
    UnitySendMessage([gameObject_ cStringUsingEncoding:NSUTF8StringEncoding], "ItemPicked", [_selectedItem cStringUsingEncoding:NSUTF8StringEncoding]);
}

- (void)onDoneWithIndex:(NSInteger)index {
    _selectedItem = [NSString stringWithFormat:@"%d", index];
    UnitySendMessage([gameObject_ cStringUsingEncoding:NSUTF8StringEncoding], "ItemPicked", [_selectedItem cStringUsingEncoding:NSUTF8StringEncoding]);
    
    if (_pickerPopover != nil) {
        [_pickerPopover dismissPopoverAnimated:true];
    }
    [self hidePicker];
}

- (BOOL)popoverControllerShouldDismissPopover:(UIPopoverController *)popoverController {
    if (_selectedItem == nil)
        UnitySendMessage([gameObject_ cStringUsingEncoding:NSUTF8StringEncoding], "PickerCanceled", "");
    
    return YES;
}

- (IBAction)onClosePickerButton:(id)sender {
    //[_pickerViewController selectCurrentValue];
    if (_selectedItem == nil)
        UnitySendMessage([gameObject_ cStringUsingEncoding:NSUTF8StringEncoding], "PickerCanceled", "");
    
    [self hidePicker];
}

@end

NSString* nativePickerCreateNSString (const char* string);
NSArray* nativePickerCreateArray(const char** array, int num);
void nativePickerCreatePluginIfNeeded();


static NativePickerPlugin* g_nativePickerPlugin = nil;

// Converts C style string to NSString
NSString* nativePickerCreateNSString (const char* string)
{
	if (string)
		return [NSString stringWithUTF8String: string];
	else
		return [NSString stringWithUTF8String: ""];
}

NSArray* nativePickerCreateArray(const char** array, int num)
{
    NSMutableArray* retval = [[NSMutableArray alloc] init];
    
    for (int i = 0 ; i < num ; i++)
    {
        [retval addObject:nativePickerCreateNSString(array[i])];
    }
    
    return retval;
}

void nativePickerCreatePluginIfNeeded()
{
    if (g_nativePickerPlugin == nil)
        g_nativePickerPlugin = [[NativePickerPlugin alloc] init];
}


void showPicker(int type, const char** items, int numItems, int64_t selectedItem, int x, int y, int w, int h, const char* gameObject)
{
    nativePickerCreatePluginIfNeeded();
    
    NSLog(@"selected item: %lli", selectedItem);
    
    float scale = [[UIScreen mainScreen] scale];
    CGRect rect = CGRectMake(x/scale, y/scale, w/scale, h/scale);
    [g_nativePickerPlugin showPicker:type withArray:nativePickerCreateArray(items, numItems) andSelectedItem:selectedItem atRect:rect andGameObject:nativePickerCreateNSString(gameObject)];
}

void hidePicker()
{
    nativePickerCreatePluginIfNeeded();
    
    [g_nativePickerPlugin hidePicker];
}

bool isIphone() {
    //nativePickerCreatePluginIfNeeded();
    
    return [NativePickerPlugin isIphone];
}
