using System;
using UnityEngine;
using System.Collections.Generic;

// TODO : 각 라운드 시작 시 맞는 Enemmy를 오브젝트 풀 생성하는 기능 필요
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