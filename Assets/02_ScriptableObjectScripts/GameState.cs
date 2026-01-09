using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "TowerDefense/GameState")]
public class GameState : ScriptableObject
{
    [field: SerializeField] public int StageLevel { get; set; } = 1;
    [field: SerializeField] public int Level { get; set; } = 1;
    [field: SerializeField] public int currentEXP { get; set; } = 0;
    [field: SerializeField] public int Gold { get; set; } = 500;
    [field: SerializeField] public int Lives { get; set; } = 20;
    [field: SerializeField] public bool IsPaused { get; set; }

    [field: SerializeField] private EXPTable expTable;

    public int maxEXP => expTable ? expTable.GetMaxEXP(Level - 1) : 0;
}