using System;
using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private StageManager stageManager;
    [field: SerializeField] private StageData stageData;
    [SerializeField] private GameState gameState;

    public int MaxStageLevel => stageData.StageCount;
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

        gameState.StageLevel++;
        stageManager.StartStage();
    }

    public StageData GetStageData() => stageData;
}