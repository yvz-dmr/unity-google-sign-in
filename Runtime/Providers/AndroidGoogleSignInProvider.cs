#if UNITY_ANDROID
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Vuzmir.UnityGoogleSignIn
{
    internal class AndroidGoogleSignInProvider : IGoogleSignInProvider
    {
        private const string JAVA_PACKAGE_NAME = "com.vuzmir.authentication";
        public class SignInCallback : AndroidJavaProxy
        {
            private TaskCompletionSource<GoogleSignInResult> completionSource;
            public SignInCallback(TaskCompletionSource<GoogleSignInResult> completion) : base($"{JAVA_PACKAGE_NAME}.GoogleSignInManager$SignInCallback")
            {
                completionSource = completion;
            }
            public void onSignInSuccess(string userId, string idToken, string email, string fullname)
            {
                completionSource.TrySetResult(new GoogleSignInResult(userId, idToken, email, fullname));
            }
            public void onSignInFailure(string errorType, string errorMessage)
            {
                completionSource.TrySetException(new GoogleSignInException(GoogleSignInError.Unknown, "Type: " + errorType + " | " + errorMessage));
            }
        }

        public Task<GoogleSignInResult> SignIn()
        {
            string clientId = null;
            try
            {
                clientId = GoogleSignInManager.GetGoogleSignInOptions().AndroidClientId;
            }
            catch (FileNotFoundException e)
            {
                throw new GoogleSignInException(GoogleSignInError.InvalidOptions, e.Message);
            }
            if (string.IsNullOrEmpty(clientId))
                throw new GoogleSignInException(
                    GoogleSignInError.InvalidOptions,
                    $"AndroidClientId is not set in the {nameof(GoogleSignInOptions)}");

            var completion = new TaskCompletionSource<GoogleSignInResult>();

            using var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            using var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            using var googleSignInManager = new AndroidJavaClass($"{JAVA_PACKAGE_NAME}.GoogleSignInManager");
            googleSignInManager.CallStatic("signIn", activity, clientId, new SignInCallback(completion));
            return completion.Task;
        }
    }
}
#endif