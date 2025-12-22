using System;
using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private StageManager stageManager;
    [field: SerializeField] private StageEnemyData StageEnemyData;
    [SerializeField] private GameState gameState;

    public int MaxStageLevel => StageEnemyData.StageCount;
    public int StageLevel => gameState.StageLevel;
    public int Gold => gameState.Gold;

    public Action OnNextStage;
    public event Action<int> OnGoldChanged;

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
        // - GameState.Gold에 (스테이지 레벨 * 기본 보상 * 골드 추가 업그레이드 배율) 추가

        gameState.StageLevel++;
        stageManager.StartStage();
    }

    public StageEnemyData GetStageEnemyData() => StageEnemyData;

    public void AddGoldOnEnemyDie(int amount)
    {
        //TODO: 업그레이드에 따른 보상 배율 적용
        AddGold(amount /* *upgradeValue */);
    }

    private void AddGold(int amount)
    {
        gameState.Gold += amount;
        OnGoldChanged?.Invoke(gameState.Gold);
    }

    public bool TrySpendGold(int amount)
    {
        if (gameState.Gold < amount) return false;

        gameState.Gold -= amount;
        OnGoldChanged?.Invoke(gameState.Gold);
        return true;
    }
}