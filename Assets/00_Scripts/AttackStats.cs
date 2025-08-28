[System.Serializable]
public struct AttackStats
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
}
