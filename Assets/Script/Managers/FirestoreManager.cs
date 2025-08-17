using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;

public class FirestoreManager : MonoBehaviour
{
    private FirebaseFirestore db;
    private string userId;
    private string playerName;

    [Header("Debug")]
    public int money;
    public int buyCount;
    public int upgradeCount;

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;

        AuthManager auth = FindObjectOfType<AuthManager>();
        if (auth != null)
        {
            auth.OnUserConnected += OnUserConnected;
        }
    }

    private void OnUserConnected(string name)
    {
        userId = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        playerName = name;
        LoadPlayerData();
    }

    public void LoadPlayerData()
    {
        DocumentReference docRef = db.Collection("users").Document(userId);
        docRef.GetSnapshotAsync().ContinueWith(loadTask =>
        {
            if (loadTask.IsCompleted)
            {
                var snapshot = loadTask.Result;
                if (snapshot.Exists)
                {
                    money = snapshot.GetValue<int>("money");
                    buyCount = snapshot.GetValue<int>("buyCount");
                    upgradeCount = snapshot.GetValue<int>("upgradeCount");
                    playerName = snapshot.GetValue<string>("name");
                    Debug.Log($"[Firestore] Loaded: money {money}, buys {buyCount}, upgrades {upgradeCount}");
                }
                else
                {
                    CreateNewPlayer();
                }
            }
            else
            {
                Debug.LogError("Firestore read error: " + loadTask.Exception);
            }
        });
    }

    public void CreateNewPlayer()
    {
        DocumentReference docRef = db.Collection("users").Document(userId);
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "name", playerName },
            { "money", 0 },
            { "buyCount", 0 },
            { "upgradeCount", 0 }
        };
        docRef.SetAsync(data).ContinueWith(createTask =>
        {
            if (createTask.IsCompleted)
                Debug.Log("[Firestore] New player created!");
            else
                Debug.LogError("Firestore create error: " + createTask.Exception);
        });

        money = 0;
        buyCount = 0;
        upgradeCount = 0;
    }

    public void UpdatePlayerStats(int newMoney, int newBuy, int newUpgrade)
    {
        DocumentReference docRef = db.Collection("users").Document(userId);
        Dictionary<string, object> update = new Dictionary<string, object>
        {
            { "money", newMoney },
            { "buyCount", newBuy },
            { "upgradeCount", newUpgrade }
        };
        docRef.UpdateAsync(update).ContinueWith(updateTask =>
        {
            if (updateTask.IsCompleted)
                Debug.Log("[Firestore] Player stats updated.");
            else
                Debug.LogError("Firestore update error: " + updateTask.Exception);
        });

        money = newMoney;
        buyCount = newBuy;
        upgradeCount = newUpgrade;
    }

    public int GetMoney() => money;
    public int GetBuyCount() => buyCount;
    public int GetUpgradeCount() => upgradeCount;
}
