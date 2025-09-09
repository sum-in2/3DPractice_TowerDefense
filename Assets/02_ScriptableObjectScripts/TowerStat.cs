using UnityEngine;

[CreateAssetMenu(menuName = "Game/TowerStat/TowerStat")]
public class TowerStat : ScriptableObject
{
    public TowerType towerType;
    public AttackStats attackStats;
}