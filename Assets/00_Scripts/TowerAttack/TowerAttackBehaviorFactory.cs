public static class TowerAttackBehaviorFactory
{
    public static IAttackBehavior Create(TowerType type)
    {
        switch (type)
        {
            case TowerType.Basic:
                return new BasicTowerAttack();
            case TowerType.A:
                return new ATowerAttack();
            default:
                return null;
        }
    }
}
