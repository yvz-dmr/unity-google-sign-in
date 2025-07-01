package com.vuzmir.authentication;

import android.app.Activity;
import androidx.credentials.CredentialManager;
import androidx.credentials.CredentialManagerCallback;
import androidx.credentials.CustomCredential;
import androidx.credentials.GetCredentialRequest;
import androidx.credentials.GetCredentialResponse;
import androidx.credentials.exceptions.GetCredentialException;
import com.google.android.libraries.identity.googleid.GetGoogleIdOption;
import com.google.android.libraries.identity.googleid.GoogleIdTokenCredential;

import java.security.MessageDigest;
import java.util.UUID;
import java.util.concurrent.Executor;

public class GoogleSignInManager {

    public static void signIn(Activity activity, String clientId, SignInCallback callback) {
        var nonce = generateNonce();
        if (nonce == null) {
            callback.onSignInFailure(null, "Unable to generate nonce");
            return;
        }

        // Create the Google ID option
        GetGoogleIdOption googleIdOption = new GetGoogleIdOption.Builder()
                .setFilterByAuthorizedAccounts(true)
                .setServerClientId(clientId)
                .setNonce(nonce)
                .build();

        // Create the GetCredentialRequest
        GetCredentialRequest request = new GetCredentialRequest.Builder()
                .addCredentialOption(googleIdOption)
                .build();

        CredentialManager credentialManager = CredentialManager.create(activity);

        Executor executor = command -> command.run();

        credentialManager.getCredentialAsync(
                activity,
                request,
                null,
                executor,
                new CredentialManagerCallback<GetCredentialResponse, GetCredentialException>() {
                    @Override
                    public void onResult(GetCredentialResponse result) {
                        handleSignIn(result, callback);
                    }

                    @Override
                    public void onError(GetCredentialException e) {
                        callback.onSignInFailure(e.getType(), e.getMessage());
                    }
                }
        );
    }

    private static void handleSignIn(GetCredentialResponse result, SignInCallback callback) {
        if (result.getCredential() instanceof CustomCredential) {
            CustomCredential credential = (CustomCredential) result.getCredential();
            if (credential.getType().equals(GoogleIdTokenCredential.TYPE_GOOGLE_ID_TOKEN_CREDENTIAL)) {
                try {
                    GoogleIdTokenCredential googleIdTokenCredential = GoogleIdTokenCredential.createFrom(credential.getData());

                    callback.onSignInSuccess(
                            googleIdTokenCredential.getId(),
                            googleIdTokenCredential.getIdToken(),
                            null,
                            googleIdTokenCredential.getDisplayName()
                    );
                } catch (Exception e) {
                    callback.onSignInFailure(null, e.getMessage());
                }
            } else {
                callback.onSignInFailure(null, "Unexpected type of credential");
            }
        } else {
            callback.onSignInFailure(null, "Unexpected type of credential");
        }
    }

    private static String generateNonce() {
        String rawNonce = UUID.randomUUID().toString();
        try {
            MessageDigest md = MessageDigest.getInstance("SHA-256");
            byte[] digest = md.digest(rawNonce.getBytes());
            StringBuilder sb = new StringBuilder();
            for (byte b : digest) {
                sb.append(String.format("%02x", b));
            }
            return sb.toString();
        } catch (Exception e) {
            return null;
        }
    }

    public interface SignInCallback {
        void onSignInSuccess(String userId, String idToken, String email, String fullname);
        void onSignInFailure(String errorCode, String error);
    }
}
