using System;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    private StageManager stageManager;

    protected override void Awake()
    {
        base.Awake();
        stageManager = StageManager.Instance;
    }

    public void StartStage(int stageLevel)
    {
        stageManager.StartStage(stageLevel);
    }
}