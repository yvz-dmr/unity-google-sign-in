#if UNITY_IOS
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;

namespace Vuzmir.UnityGoogleSignIn.Editor
{
    public class GoogleSignInPlistPostProcessor
    {
        [PostProcessBuild]
        public static void ChangeXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
        {
            if (buildTarget != BuildTarget.iOS) return;

            var plistPath = Path.Combine(pathToBuiltProject, "Info.plist");

            if (!File.Exists(plistPath))
                throw new System.Exception("Info.plist not found — is the build complete?");

            var plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            var options = GoogleSignInManager.GetGoogleSignInOptions();

            if (options == null || string.IsNullOrEmpty(options.IOSClientId))
                throw new System.Exception($"Missing Google Client ID for iOS — did you configure the {nameof(GoogleSignInOptions)}?");

            var urlTypes = plist.root.CreateArray("CFBundleURLTypes");
            var dict = urlTypes.AddDict();
            var urlSchemes = dict.CreateArray("CFBundleURLSchemes");
            urlSchemes.AddString(options.IOSClientIdReversed);

            plist.root.SetString("GIDClientID", options.IOSClientId);

            // Write to file
            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}
#endif
