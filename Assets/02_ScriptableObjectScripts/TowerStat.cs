using UnityEngine;

[CreateAssetMenu(menuName = "Game/TowerStat/BasicTower")]
public class TowerStat : ScriptableObject
{
    public TowerType towerType;
    public AttackStats attackStats;
}