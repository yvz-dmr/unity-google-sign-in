using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Vuzmir.UnityGoogleSignIn
{
    public class GoogleSignInManager : IGoogleSignInProvider
    {
        public Task<GoogleSignInResult> SignIn()
        {
#if UNITY_EDITOR
            return new EditorGoogleSignInProvider().SignIn();
#elif UNITY_IOS
            return new IOSGoogleSignInProvider().SignIn();
#elif UNITY_ANDROID
            return new AndroidGoogleSignInProvider().SignIn();
#else
            throw new GoogleSignInException(
                            GoogleSignInError.NotSupported,
                            $"Google sign in is not supported on platform {Application.platform}");
#endif
        }

        public static GoogleSignInOptions GetGoogleSignInOptions()
        {
            return Resources.Load<GoogleSignInOptions>(nameof(GoogleSignInOptions))
                ?? throw new FileNotFoundException($"No {nameof(GoogleSignInOptions)} found in resources");
        }
    }
}
