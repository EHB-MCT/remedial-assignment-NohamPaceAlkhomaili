using UnityEngine;
using Firebase.Auth;
using System;
using TMPro;

public class AuthManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject loginPanel;
    public TMP_InputField nameInputField;
    public static FirebaseUser User;
    public Action<string> OnUserConnected;

    public void OnLoginButtonClicked()
    {
        string playerName = nameInputField.text.Trim();
        Debug.Log($"[AuthManager] OnLoginButtonClicked called! playerName={playerName}");

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("[AuthManager] playerName is empty, aborting login.");
            return;
        }

        FirebaseAuth.DefaultInstance.SignInAnonymouslyAsync().ContinueWith(authTask =>
        {
            Debug.Log("[AuthManager] Firebase callback reached.");
            if (authTask.IsCompleted && !authTask.IsFaulted && !authTask.IsCanceled)
            {
                var authResult = authTask.Result;
                User = authResult.User;

                Debug.Log("[AuthManager] User connected: " + User.UserId);

                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    if (loginPanel != null)
                    {
                        loginPanel.SetActive(false);
                        Debug.Log("[AuthManager] loginPanel.SetActive(false) success");
                    }
                    else
                    {
                        Debug.LogError("[AuthManager] loginPanel reference is null!");
                    }
                });

                OnUserConnected?.Invoke(playerName);
            }
            else
            {
                Debug.LogError("[AuthManager] Firebase sign-in failed: " + authTask.Exception?.Message);
            }
        });
    }
}
