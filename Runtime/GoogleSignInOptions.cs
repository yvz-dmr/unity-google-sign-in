using System.Linq;
using UnityEngine;

namespace Vuzmir.UnityGoogleSignIn
{
    [CreateAssetMenu(menuName = "Vuzmir/Google Sign In Options", fileName = nameof(GoogleSignInOptions))]
    public class GoogleSignInOptions : ScriptableObject
    {
        [TextArea]
        [SerializeField] string androidClientId;

        [TextArea]
        [SerializeField] string iosClientId;

        public string AndroidClientId => androidClientId;
        public string IOSClientId => iosClientId;
        public string IOSClientIdReversed
        {
            get
            {
                if (string.IsNullOrEmpty(iosClientId)) return null;
                return string.Join('.', iosClientId.Split(".").Reverse());
            }
        }
    }

}