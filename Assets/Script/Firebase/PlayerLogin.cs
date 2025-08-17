using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Extensions;

public class PlayerLogin : MonoBehaviour
{
    public TMP_InputField pseudoInput;
    public Button confirmButton;
    public GameObject loginPanel;
    public static string PlayerName { get; private set; }

    void Start()
    {
        confirmButton.onClick.AddListener(OnConfirm);
        loginPanel.SetActive(true);
    }

    void OnConfirm()
    {
        string pseudo = pseudoInput.text.Trim();
        if (!string.IsNullOrEmpty(pseudo))
        {
            PlayerName = pseudo;
            loginPanel.SetActive(false);

            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
            var docRef = db.Collection("players").Document(PlayerName);

            docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    if (task.Result.Exists)
                    {
                        FirebaseSaver.Instance.RestoreSessionFromSnapshot(task.Result);
                    }
                    else
                    {
                        if (MoneyManager.Instance != null)
                        {
                            MoneyManager.Instance.currentMoney = 100;
                            MoneyManager.Instance.UpdateUI();
                        }

                        Dictionary<string, object> data = new Dictionary<string, object>();

                        docRef.SetAsync(data).ContinueWithOnMainThread(setTask =>
                        {
                            if (setTask.IsCompleted)
                                Debug.Log("[Firestore] New player created!");
                            else
                                Debug.LogError("[Firestore] Error creating player: " + setTask.Exception);
                        });
                    }
                }
                else
                {
                    Debug.LogError("[Firestore] Error reading Firestore: " + task.Exception);
                }
            });
        }
        else
        {
            Debug.LogWarning("Please enter a valid name.");
        }
    }
}
