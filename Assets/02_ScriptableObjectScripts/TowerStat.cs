using UnityEngine;

//TODO : 의도는 각 타워의 초기 스탯을 저장하는 SO를 만드려고 함
//       그렇다면 어디선가 타워 타입 + 해당 타워 스탯을 저장하는 딕셔너리를 가지고
//       타워 설치시에 해당 SO의 공격스탯을 대입하면 됨
[CreateAssetMenu(menuName = "Game/TowerStat/BasicTower")]
public class TowerStat : ScriptableObject
{
    public TowerType towerType;
    public AttackStats attackStats;
}