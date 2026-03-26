using System;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private GameState gameState;

    public int Gold => gameState.Gold;
    public int Level => gameState.Level;
    public int CurrentEXP => gameState.currentEXP;
    public int MaxEXP => gameState.maxEXP;

    protected override void Awake()
    {
        base.Awake();
        gameState.Reset();
    }

    public event Action<int> OnGoldChanged;
    public event Action<int> OnEXPChanged;
    public event Action<int> OnLevelUp;

    public void AddGoldOnEnemyDie(int amount)
    {
        // TODO: 업그레이드에 따른 보상 배율 적용
        AddGold(amount);
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

    public void AddEXP(int amount)
    {
        gameState.currentEXP += amount;
        OnEXPChanged?.Invoke(gameState.currentEXP);

        while (gameState.currentEXP >= gameState.maxEXP)
        {
            gameState.currentEXP -= gameState.maxEXP;
            gameState.Level++;
            OnLevelUp?.Invoke(gameState.Level);
            OnEXPChanged?.Invoke(gameState.currentEXP);
        }
    }
}
