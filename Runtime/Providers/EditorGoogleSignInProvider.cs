using System.Threading.Tasks;

namespace Vuzmir.UnityGoogleSignIn
{
    internal class EditorGoogleSignInProvider : IGoogleSignInProvider
    {
        public Task<GoogleSignInResult> SignIn()
        {
            return Task.FromResult(new GoogleSignInResult("test-user-id", "test-id-token", "test user", "test-user@gmail.com"));
        }
    }
}