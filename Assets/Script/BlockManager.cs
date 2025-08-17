using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public interface IEconomy
{
    int GetIncomePerTick();
}

public class BlockManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int maxUnits = 6;
    [SerializeField] private int baseIncome = 10;
    [SerializeField] private float upgradeStep = 0.1f;
    [SerializeField] private int maxUpgradeLevel = 5;

    [Header("UI References")]
    [SerializeField] private Button buyButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI infoText;

    [Header("Spawn Settings")]
    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private Transform spawnArea;
    [SerializeField] private float spawnThresholdX = 2f;
    [SerializeField] private float spawnThresholdY = 2f;

    private int currentUnits = 0;
    private int upgradeLevel = 0;
    private float Multiplier => 1f + upgradeLevel * upgradeStep;
    public event Action<int, int, float> OnStatsChanged;

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
    }

    private void BuyUnit()
    {
        if (currentUnits >= maxUnits)
        {
            Debug.Log("Max units reached!");
            return;
        }

        currentUnits++;
        Vector3 spawnPos = spawnArea != null
            ? spawnArea.position + new Vector3(
                UnityEngine.Random.Range(-spawnThresholdX, spawnThresholdX),
                UnityEngine.Random.Range(-spawnThresholdY, spawnThresholdY),
                0)
            : Vector3.zero;

        Instantiate(unitPrefab, spawnPos, Quaternion.identity);
        Debug.Log($"Unit spawned! Total = {currentUnits}");

        UpdateUI();
    }

    private void Upgrade()
    {
        if (upgradeLevel >= maxUpgradeLevel)
        {
            Debug.Log("Max multiplier reached.");
            return;
        }

        upgradeLevel++;
        Debug.Log($"Upgrade applied! New multiplier = {Multiplier:F2}");

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (infoText != null)
        {
            infoText.text = $"Units: {currentUnits}/{maxUnits}\nMultiplier: x{Multiplier:F2}";
        }

        OnStatsChanged?.Invoke(currentUnits, maxUnits, Multiplier);
    }
    public int GetIncomePerTick()
    {
        return Mathf.RoundToInt(currentUnits * baseIncome * Multiplier);
    }
}
