#if UNITY_IOS
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AOT;

namespace Vuzmir.UnityGoogleSignIn
{
    internal class IOSGoogleSignInProvider : IGoogleSignInProvider
    {
        [DllImport("__Internal")]
        private static extern void _StartGoogleSignIn(SignInCallback callback, ErrorCallback errorCallback);

        private delegate void SignInCallback(string userId, string idToken, string fullName, string email);
        private delegate void ErrorCallback(int code, string error);

        private static TaskCompletionSource<GoogleSignInResult> activeCompletion;

        [MonoPInvokeCallback(typeof(SignInCallback))]
        private static void OnSignedIn(string userId, string idToken, string fullName, string email)
        {
            activeCompletion?.TrySetResult(new GoogleSignInResult(userId, idToken, fullName, email));
            activeCompletion = null;
        }
        [MonoPInvokeCallback(typeof(ErrorCallback))]
        private static void OnSignInError(int code, string error)
        {
            var googleErrorCode = code switch
            {
                -5 => GoogleSignInError.Cancelled,
                _ => GoogleSignInError.Unknown,
            };
            activeCompletion?.TrySetException(new GoogleSignInException(googleErrorCode, code, error));
            activeCompletion = null;
        }
        public Task<GoogleSignInResult> SignIn()
        {
            activeCompletion = new TaskCompletionSource<GoogleSignInResult>();
            _StartGoogleSignIn(OnSignedIn, OnSignInError);
            return activeCompletion.Task;
        }
    }
}
#endif