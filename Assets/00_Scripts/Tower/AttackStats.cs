[System.Serializable]
public class AttackStats
{
    public float attackPower;
    public float range;
    public float attackSpeed;
    public float ignoreDefense;
    public float criticalChance;
    public float criticalDamage;

    public AttackStats(float attackPower, float range, float attackSpeed, float ignoreDefense, float criticalChance, float criticalDamage)
    {
        this.attackPower = attackPower;
        this.range = range;
        this.attackSpeed = attackSpeed;
        this.ignoreDefense = ignoreDefense;
        this.criticalChance = criticalChance;
        this.criticalDamage = criticalDamage;
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
