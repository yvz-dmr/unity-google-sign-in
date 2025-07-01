#import <GoogleSignIn/GoogleSignIn.h>
#import <UIKit/UIKit.h>

@interface IOSGoogleSignInManager : NSObject

@end

@implementation IOSGoogleSignInManager

+ (void)startSignIn:(void (^)(const char *userId, const char *idToken,
                              const char *fullName, const char *email))callback
    addErrorCallback:
        (void (^)(const int code, const char *error))errorCallback {

  // Get the root view controller (since we aren't using AppController directly)
  UIWindow *window = UIApplication.sharedApplication.keyWindow;
  UIViewController *rootViewController = window.rootViewController;

  [GIDSignIn.sharedInstance
      signInWithPresentingViewController:rootViewController
                              completion:^(GIDSignInResult *_Nullable result,
                                           NSError *_Nullable error) {
                                if (error) {
                                  const char *errorMessage =
                                      [error.localizedDescription UTF8String];
                                  char *errorMessageCopy = strdup(errorMessage);
                                  int errorCode = (int)error.code;
                                  errorCallback(errorCode, errorMessageCopy);
                                } else if (callback != NULL) {
                                  const char *userId =
                                      [result.user.userID UTF8String];
                                  const char *idToken =
                                      [result.user.idToken
                                              .tokenString UTF8String];
                                  const char *fullName =
                                      [result.user.profile.name UTF8String];
                                  const char *email =
                                      [result.user.profile.email UTF8String];

                                  // Allocate memory for the strings and copy
                                  // the values
                                  char *userIdCopy = strdup(userId);
                                  char *idTokenCopy = strdup(idToken);
                                  char *fullNameCopy = strdup(fullName);
                                  char *emailCopy = strdup(email);

                                  callback(userIdCopy, idTokenCopy,
                                           fullNameCopy, emailCopy);
                                } // If sign in succeeded, display the app's
                                  // main content View.
                              }];
}

@end

extern "C" {

typedef void (*GoogleSignInCallback)(const char *, const char *, const char *,
                                     const char *);
typedef void (*GoogleSignInErrorCallback)(const int, const char *);

void _StartGoogleSignIn(GoogleSignInCallback callback,
                        GoogleSignInErrorCallback errorCallback) {
  [IOSGoogleSignInManager
      startSignIn:^(const char *userId, const char *idToken,
                    const char *fullName, const char *email) {
        if (callback != NULL) {
          callback(userId, idToken, fullName, email);
        }
      }
      addErrorCallback:^(const int code, const char *error) {
        if (errorCallback != NULL) {
          errorCallback(code, error);
        }
      }];
}
}
