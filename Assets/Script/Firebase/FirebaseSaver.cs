using UnityEngine;
using Firebase.Firestore;
using System;
using System.Collections.Generic;

public class FirebaseSaver : MonoBehaviour
{
    public static FirebaseSaver Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RestoreSessionFromSnapshot(DocumentSnapshot snapshot)
    {
        Debug.Log("[FirebaseSaver] Restoring Firestore session...");

        var data = snapshot.ToDictionary();

        int money = data.ContainsKey("money") ? Convert.ToInt32(data["money"]) : 100;
        int buyCount = data.ContainsKey("buyCount") ? Convert.ToInt32(data["buyCount"]) : 0;
        int upgradeCount = data.ContainsKey("upgradeCount") ? Convert.ToInt32(data["upgradeCount"]) : 0;

        if (MoneyManager.Instance != null)
        {
            MoneyManager.Instance.currentMoney = money;
            MoneyManager.Instance.UpdateUI();
        }

        var blockManagers = FindObjectsOfType<BlockManager>();
        foreach (var bm in blockManagers)
        {
            bm.RestoreStats(buyCount, upgradeCount);
            bm.UpdateUI();
        }
    }

    public void SavePlayerData(int money, int buyCount, int upgradeCount)
    {
        if (string.IsNullOrEmpty(PlayerLogin.PlayerName))
        {
            Debug.LogWarning("No player name defined. Cannot save.");
            return;
        }

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("players").Document(PlayerLogin.PlayerName);

        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "money", money },
            { "buyCount", buyCount },
            { "upgradeCount", upgradeCount }
        };

        docRef.SetAsync(data).ContinueWith(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
                Debug.Log("<color=green> Save successful for " + PlayerLogin.PlayerName + "</color>");
            else
                Debug.LogError("Save error: " + task.Exception);
        });
    }
}
