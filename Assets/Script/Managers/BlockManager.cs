using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public enum BlockType
{
    TypeA,
    TypeB,
    TypeC,
    TypeD
}

public class BlockManager : MonoBehaviour, IEconomy
{
    [Header("Settings")]
    [SerializeField] private int maxUnits = 6;
    [SerializeField] private int baseIncome = 1;
    [SerializeField] private float upgradeStep = 0.1f;
    [SerializeField] private int maxUpgradeLevel = 5;

    [Header("Costs")]
    [SerializeField] private int initialBuyCost = 10;
    [SerializeField] private int initialUpgradeCost = 50;

    [Header("UI References")]
    [SerializeField] private Button buyButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI incomeText;

    [Header("Spawn Settings")]
    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private Transform spawnArea;
    [SerializeField] private float spawnThresholdX = 2f;
    [SerializeField] private float spawnThresholdY = 2f;

    [Header("Block Type")]
    [SerializeField] public BlockType blockType;

    private int currentUnits = 0;
    private int upgradeLevel = 0;

    private float Multiplier => 1f + upgradeLevel * upgradeStep;

    private void OnEnable()
    {
        if (buyButton != null) buyButton.onClick.AddListener(BuyUnit);
        if (upgradeButton != null) upgradeButton.onClick.AddListener(Upgrade);
    }

    private void OnDisable()
    {
        if (buyButton != null) buyButton.onClick.RemoveListener(BuyUnit);
        if (upgradeButton != null) upgradeButton.onClick.RemoveListener(Upgrade);
    }

    private void Start()
    {
        UpdateUI();
        UpdateButtonsInteractable();
        MoneyManager.Instance.RegisterEconomy(this);
    }

    private int GetCurrentBuyCost() => Mathf.RoundToInt(initialBuyCost * Mathf.Pow(2, currentUnits));
    private int GetCurrentUpgradeCost() => Mathf.RoundToInt(initialUpgradeCost * Mathf.Pow(2, upgradeLevel));

    private void BuyUnit()
    {
        int cost = GetCurrentBuyCost();
        if (currentUnits >= maxUnits) return;
        if (!MoneyManager.Instance.SpendMoney(cost)) return;

        if (unitPrefab == null)
        {
            Debug.LogError("unitPrefab is not assigned in the inspector!");
            return;
        }
        
        currentUnits++;
        Vector3 spawnPos = spawnArea != null
            ? spawnArea.position + new Vector3(
                Random.Range(-spawnThresholdX, spawnThresholdX),
                Random.Range(-spawnThresholdY, spawnThresholdY),
                0)
            : Vector3.zero;

        Instantiate(unitPrefab, spawnPos, Quaternion.identity);

        UpdateUI();
        UpdateButtonsInteractable();

        FirestoreManager fm = FindObjectOfType<FirestoreManager>();
        if (fm != null)
        {
            fm.UpdatePlayerStats(MoneyManager.Instance.currentMoney, currentUnits, upgradeLevel);
        }
    }

    private void Upgrade()
    {
        int cost = GetCurrentUpgradeCost();
        if (upgradeLevel >= maxUpgradeLevel) return;
        if (!MoneyManager.Instance.SpendMoney(cost)) return;

        upgradeLevel++;

        UpdateUI();
        UpdateButtonsInteractable();

        FirestoreManager fm = FindObjectOfType<FirestoreManager>();
        if (fm != null)
        {
            fm.UpdatePlayerStats(MoneyManager.Instance.currentMoney, currentUnits, upgradeLevel);
        }
    }

    public void UpdateUI()
    {
        if (infoText != null)
            infoText.text = $"Units: {currentUnits}/{maxUnits}\nMultiplier: x{Multiplier:F2}";

        if (incomeText != null)
            incomeText.text = $"Income: {GetIncomePerTick()} / sec";

        if (buyButton != null)
        {
            TextMeshProUGUI buyBtnText = buyButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buyBtnText != null)
                buyBtnText.text = currentUnits >= maxUnits ? "Buy (MAX)" : $"Buy ({GetCurrentBuyCost()})";
        }

        if (upgradeButton != null)
        {
            TextMeshProUGUI upgradeBtnText = upgradeButton.GetComponentInChildren<TextMeshProUGUI>();
            if (upgradeBtnText != null)
                upgradeBtnText.text = upgradeLevel >= maxUpgradeLevel ? "Upgrade (MAX)" : $"Upgrade ({GetCurrentUpgradeCost()})";
        }
    }

    private void UpdateButtonsInteractable()
    {
        if (buyButton != null)
            buyButton.interactable = MoneyManager.Instance.GetCurrentMoney() >= GetCurrentBuyCost() && currentUnits < maxUnits;
        if (upgradeButton != null)
            upgradeButton.interactable = MoneyManager.Instance.GetCurrentMoney() >= GetCurrentUpgradeCost() && upgradeLevel < maxUpgradeLevel;
    }

    public int GetIncomePerTick()
    {
        return Mathf.RoundToInt(currentUnits * baseIncome * Multiplier);
    }

    private void Update()
    {
        UpdateButtonsInteractable();
    }

    public void RestoreStats(int savedUnits, int savedUpgrade)
    {
        currentUnits = savedUnits;
        upgradeLevel = savedUpgrade;
        UpdateUI();
    }
}
