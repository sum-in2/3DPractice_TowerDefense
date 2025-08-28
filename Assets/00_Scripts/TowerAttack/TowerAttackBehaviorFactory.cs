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
            // 추가 타입별 공격 방식
            default:
                return null;
        }
    }
}
