using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    [SerializeField] private int startingMoney = 10;
    [SerializeField] private TextMeshProUGUI moneyText;
    public int currentMoney;

    private List<IEconomy> incomeSources = new List<IEconomy>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        currentMoney = startingMoney;
        UpdateUI();
        StartCoroutine(GenerateIncome());
    }

    public void RegisterEconomy(IEconomy economy)
    {
        if (!incomeSources.Contains(economy))
            incomeSources.Add(economy);
    }

    private IEnumerator GenerateIncome()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            int totalIncome = 0;
            foreach (var source in incomeSources)
                totalIncome += source.GetIncomePerTick();
            AddMoney(totalIncome);
        }
    }

    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            UpdateUI();
            return true;
        }
        return false;
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (moneyText != null)
            moneyText.text = $"MONEY: {currentMoney}";
    }

    public int GetCurrentMoney() => currentMoney;

    public void RestoreMoney(int restoredMoney)
    {
        currentMoney = restoredMoney;
        UpdateUI();
    }
}
