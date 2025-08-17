using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public string name;
    public int money;

    public Dictionary<string, int> blocks;
    public Dictionary<string, int> upgrades;

    public PlayerData()
    {
        blocks = new Dictionary<string, int>();
        upgrades = new Dictionary<string, int>();
    }

    public void SetBlockData(string blockType, int count, int upgradeLevel)
    {
        blocks[blockType] = count;
        upgrades[blockType] = upgradeLevel;
    }

    public int GetBlockCount(string blockType)
    {
        return blocks.ContainsKey(blockType) ? blocks[blockType] : 0;
    }

    public int GetUpgradeLevel(string blockType)
    {
        return upgrades.ContainsKey(blockType) ? upgrades[blockType] : 0;
    }
}
