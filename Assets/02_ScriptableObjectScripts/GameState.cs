using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "TowerDefense/GameState")]
public class GameState : ScriptableObject
{
    [field: SerializeField] public int StageLevel { get; private set; } = 1;
    [field: SerializeField] public int Level { get; private set; } = 1;
    [field: SerializeField] public int currentEXP { get; private set; } = 0;
    [field: SerializeField] public int Gold { get; private set; } = 500;
    [field: SerializeField] public int Lives { get; private set; } = 20;
    [field: SerializeField] public bool IsPaused { get; private set; }

    [field: SerializeField] private EXPTable expTable;

    [SerializeField] private int defaultGold = 500;
    [SerializeField] private int defaultLives = 20;
    [SerializeField] private int defaultStageLevel = 1;

    public int maxEXP => expTable ? expTable.GetMaxEXP(Level - 1) : 0;

    public void SetGold(int value) => Gold = value;
    public void SetCurrentEXP(int value) => currentEXP = value;
    public void SetLevel(int value) => Level = value;
    public void IncrementStageLevel() => StageLevel++;

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
