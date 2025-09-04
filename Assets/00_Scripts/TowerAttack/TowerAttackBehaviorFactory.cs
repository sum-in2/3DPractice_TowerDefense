public static class TowerAttackBehaviorFactory
{
    public static IAttackBehavior Create(TowerType type)
    {
        switch (type)
        {
            case TowerType.Basic:
                return new BasicTowerAttack();
            case TowerType.Energy:
                return new EnergyTowerAttack();
            case TowerType.PlasmaChain:
                return new PlasmaChainTowerAttack();
            default:
                return null;
        }
    }
}
