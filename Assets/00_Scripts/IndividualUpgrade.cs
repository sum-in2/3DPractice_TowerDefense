[System.Serializable]
public class IndividualUpgrade
{
    [UnityEngine.SerializeField] private UpgradeType upgradeType;
    [UnityEngine.SerializeField] private float increaseAmount;
    [UnityEngine.SerializeField] private int cost;
    [UnityEngine.SerializeField] private bool isApplied;
    [UnityEngine.SerializeField] private string upgradeName;

    public UpgradeType UpgradeType => upgradeType;
    public float IncreaseAmount => increaseAmount;
    public int Cost => cost;
    public bool IsApplied => isApplied;
    public string UpgradeName => upgradeName;

    public IndividualUpgrade(UpgradeType upgradeType, float increaseAmount, int cost, string upgradeName)
    {
        this.upgradeType = upgradeType;
        this.increaseAmount = increaseAmount;
        this.cost = cost;
        this.upgradeName = upgradeName;
        this.isApplied = false;
    }

    public void Apply() => isApplied = true;
}
