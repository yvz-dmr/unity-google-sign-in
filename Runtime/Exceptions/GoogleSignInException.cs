using System;

namespace Vuzmir.UnityGoogleSignIn
{
    public class GoogleSignInException : Exception
    {
        public GoogleSignInError ErrorCode { get; private set; }
        public int? NativeErrorCode { get; private set; }

        public GoogleSignInException(GoogleSignInError code, string message) : this(code, null, message)
        {
        }
        public GoogleSignInException(GoogleSignInError code, int? nativeErrorCode, string message)
            : base(nativeErrorCode + " " + message)
        {
            ErrorCode = code;
            NativeErrorCode = nativeErrorCode;
        }
    }
}