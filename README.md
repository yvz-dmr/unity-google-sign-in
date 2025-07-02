# Google Sign In Plugin for Unity

## Overview
This plugin provides seamless Google authentication support for Unity applications targeting mobile platforms. It handles all native SDK setup and provides a simple C# API to sign in users, retrieve their profile info, and manage tokens.

## Requirements

This plugin requires [Google External Dependency Manager for Unity (EDM4U)](https://github.com/googlesamples/unity-jar-resolver) to be present in the project to automatically resolve and manage native dependencies for Android and iOS.

## Installation

### By Git Url
You can install this plugin via Git URL using Unity Package Manager.

```
https://github.com/yvz-dmr/unity-google-sign-in.git
```

## Configuration

1. In Unity, create a **GoogleSignInOptions** asset:
   - Right-click inside the `Assets/Resources` folder (create the folder if it doesn't exist)
   - Select **`Create → Vuzmir → Google Sign In Options`**

2. Select the newly created asset and fill in the required fields:
   - `AndroidClientId` or `IOSClientId` or both depending on your target platform

> Make sure this asset is placed **at the root of the `Resources` folder**, so it can be loaded correctly.

## Usage

Once you’ve completed the configuration steps, you can start using the plugin to authenticate users via GoogleSignInManager.

Here’s a basic example of how to use it in a MonoBehaviour:

```csharp
using System.Threading.Tasks;
using UnityEngine;
using Vuzmir.UnityGoogleSignIn;

public class LoginManager : MonoBehaviour
{
    public async void SignInWithGoogle()
    {
        try
        {
            var result = await new GoogleSignInManager().SignIn();
            // Handle result
            Debug.Log(result.IdToken);
        }
        catch (GoogleSignInException ex)
        {
            // Handle error
            Debug.LogError($"Google Sign-In failed: {ex.Message}");
        }
    }
}
```
