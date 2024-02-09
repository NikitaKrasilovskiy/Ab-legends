//
//  devtodev.h
//  devtodev
//
//  Created by Aleksey Kornienko on 19/11/15.
//  Copyright © 2015 Aleksey Kornienko. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <UserNotifications/UserNotifications.h>


//! Project version number for devtodev.
FOUNDATION_EXPORT double devtodevVersionNumber;

//! Project version string for devtodev.
FOUNDATION_EXPORT const unsigned char devtodevVersionString[];

extern "C" {
    const char* cCopy(const char* string);
    const bool dtd_lazyHelper_isInitializationStarted();
    const bool dtd_lazyHelper_isInitialized();
    const void dtd_lazyHelper_initialization();
    const char* dtd_lazyHelper_getUserAgent();
    const char * dtd_a();
    float dtd_b();
    int dtd_c();
    const char * dtd_d();
    const char * dtd_e();
    const char * dtd_f();
    const char * dtd_g();
    const char * dtd_i(const char* key);
    void dtd_j(const char* key);
    void dtd_k();                                           // Init push manger
    const char * dtd_l();
    void dtd_z(const char* appKey, const char* userId);
    void dtd_x(const char * userId, const char * url, const char * postData);
    void dtd_y(const char *userId);
    void dtd_p();
    void dtd_t(const char* categories);
    void logger_setLogEnabled(bool isEnabled);
    int getTimeZoneOffset();
    bool isPushTokenProduction();
    void setNotificationOptions(unsigned int options);
    void startNotificationService();
    void stopNotificationService();
    void registerPushNotificationSettings();
    const char * dtd_uids();
    const char * dtd_idfa();
    void dtd_savePrev(const char* prev);
    const bool dtd_adTrackingEnabled();
    void dtd_resetKeychain();
    void dtd_requestIDFAAuthorization();
}
