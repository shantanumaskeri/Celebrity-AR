#import <Foundation/Foundation.h>

void _handshake();
void _postToInstagram(const char *, const char *);

@interface InstagramShare : NSObject <UIDocumentInteractionControllerDelegate>
{
	UIWindow *nativeWindow;
}

@property (nonatomic, retain) UIDocumentInteractionController *dic;

+(InstagramShare*)sharedInstance;

-(void)handshake;
-(void)postToInstagram:(NSString*)message WithImage:(NSString*)imagePath;

@end