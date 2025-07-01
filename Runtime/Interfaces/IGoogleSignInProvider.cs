using System.Threading.Tasks;

namespace Vuzmir.UnityGoogleSignIn
{

    public interface IGoogleSignInProvider
    {
        public Task<GoogleSignInResult> SignIn();
    }

}
