using System;

public interface ITowerUpgradeNotifier
{
    event System.Action<TowerType> OnTowerUpgraded;
    void NotifyTowerUpgraded(TowerType towerType);
}
