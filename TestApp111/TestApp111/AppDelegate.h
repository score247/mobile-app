//
//  AppDelegate.h
//  TestApp111
//
//  Created by Vivian Nguyen on 3/13/19.
//  Copyright © 2019 Vivian Nguyen. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <CoreData/CoreData.h>

@interface AppDelegate : UIResponder <UIApplicationDelegate>

@property (strong, nonatomic) UIWindow *window;

@property (readonly, strong) NSPersistentContainer *persistentContainer;

- (void)saveContext;


@end

