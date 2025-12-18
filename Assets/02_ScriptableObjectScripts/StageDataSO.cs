using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "StageData", menuName = "TowerDefense/StageData")]
public class StageData : ScriptableObject
{
    [SerializeField] private List<StageInfo> stages = new();

    public string GetMonsterName(int level) => stages[level - 1].monsterName;
    public StageInfo GetStageInfo(int level) => stages[level];
    public int StageCount => stages.Count;
    public bool IsValidLevel(int level) => 0 < level && level <= StageCount;
}