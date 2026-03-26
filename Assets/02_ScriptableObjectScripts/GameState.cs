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

    [SerializeField] private int defaultGold = 500;
    [SerializeField] private int defaultLives = 20;
    [SerializeField] private int defaultStageLevel = 1;

    public int maxEXP => expTable ? expTable.GetMaxEXP(Level - 1) : 0;

    public void Reset()
    {
        Gold = defaultGold;
        Lives = defaultLives;
        StageLevel = defaultStageLevel;
        Level = 1;
        currentEXP = 0;
        IsPaused = false;
    }
}