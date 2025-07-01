namespace Vuzmir.UnityGoogleSignIn
{
    public class GoogleSignInResult
    {
        public string UserId { get; private set; }
        public string IdToken { get; private set; }
        public string FullName { get; private set; }
        public string Email { get; private set; }

        public GoogleSignInResult(string userId, string idToken, string fullName, string email)
        {
            UserId = userId;
            IdToken = idToken;
            FullName = fullName;
            Email = email;
        }
    }
}