[System.Serializable]
public class AttackStats
{
    public float attackPower;
    public float range;
    public float attackSpeed;
    public float ignoreDefense;
    public float criticalChance;
    public float criticalDamage;

    public AttackStats(AttackStats other)
    {
        this.attackPower = other.attackPower;
        this.range = other.range;
        this.attackSpeed = other.attackSpeed;
        this.ignoreDefense = other.ignoreDefense;
        this.criticalChance = other.criticalChance;
        this.criticalDamage = other.criticalDamage;
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
        }
    }
}
