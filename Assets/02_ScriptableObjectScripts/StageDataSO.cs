using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "StageEnemyData", menuName = "TowerDefense/StageEnemyData")]
public class StageEnemyData : ScriptableObject
{
    [SerializeField] private List<StageInfo> stages = new();

    public int StageCount => stages.Count;
    public bool IsValidLevel(int level) => 0 < level && level <= StageCount;

    public StageInfo GetStageInfo(int level)
    {
        int index = Mathf.Clamp(level - 1, 0, stages.Count - 1);
        return stages[index];
    }
}