[System.Serializable]
public class AttackStats
{
    [UnityEngine.SerializeField] private float attackPower;
    [UnityEngine.SerializeField] private float range;
    [UnityEngine.SerializeField] private float attackSpeed;
    [UnityEngine.SerializeField] private float ignoreDefense;
    [UnityEngine.SerializeField] private float critChance;
    [UnityEngine.SerializeField] private float critDamage;

    public float AttackPower => attackPower;
    public float Range => range;
    public float AttackSpeed => attackSpeed;
    public float IgnoreDefense => ignoreDefense;
    public float CritChance => critChance;
    public float CritDamage => critDamage;

    public AttackStats() { }

    public AttackStats(AttackStats other)
    {
        this.attackPower = other.attackPower;
        this.range = other.range;
        this.attackSpeed = other.attackSpeed;
        this.ignoreDefense = other.ignoreDefense;
        this.critChance = other.critChance;
        this.critDamage = other.critDamage;
    }

    /// <summary>
    /// 업그레이드 종류에 Value만큼 증가 시킵니다
    /// </summary>
    /// <param name="upgradeType"></param>
    /// <param name="value"></param>
    public void UpgradeStat(UpgradeType upgradeType, float value)
    {
        switch (upgradeType)
        {
            case UpgradeType.DamageUp:
                attackPower += value;
                break;
            case UpgradeType.RangeUp:
                range += value;
                break;
            case UpgradeType.AttackSpeedUp:
                attackSpeed += value;
                break;
            case UpgradeType.CritChanceUp:
                critChance += value;
                break;
            case UpgradeType.CritDamageUp:
                critDamage += value;
                break;
            case UpgradeType.IgnoreDefenseUp:
                ignoreDefense += value;
                break;
        }
    }
}
