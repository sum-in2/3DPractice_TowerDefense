using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private StageManager stageManager;
    [field: SerializeField] private StageEnemyData StageEnemyData;
    [SerializeField] private GameState gameState;

    public int MaxStageLevel => StageEnemyData.StageCount;
    public int StageLevel => gameState.StageLevel;

    public Action OnNextStage;

    protected override void Awake()
    {
        base.Awake();
        OnNextStage += NextStage;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        OnNextStage -= NextStage;
    }

    public void NextStage()
    {
        if (StageLevel >= MaxStageLevel)
        {
            Debug.Log("최종 스테이지 완료");
            return;
        }

        // TODO: 스테이지 클리어 보상 처리

        gameState.IncrementStageLevel();
        stageManager.StartStage();
    }

    public StageEnemyData GetStageEnemyData() => StageEnemyData;
}