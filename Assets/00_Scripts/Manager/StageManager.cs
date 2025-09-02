using System;
using System.Collections.Generic;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    Dictionary<int, String> levelMonsterDict = new Dictionary<int, string>();
    [SerializeField] EnemySpawner enemySpawner;
    private Coroutine stageCoroutine;

    private int StageLevel;

    protected override void Awake()
    {
        base.Awake();
        levelMonsterDict[1] = "TestMonster";
    }

    public void StartStage(int stageLevel = 1, float spawnInterval = 1f, int enemyCount = 20)
    {
        if (stageCoroutine != null)
        {
            Debug.LogWarning("스테이지가 진행 중 입니다");
            return;
        }

        enemySpawner.SpawnEnemies(levelMonsterDict[stageLevel], 1f, 20);
        stageCoroutine = StartCoroutine(StageCoroutine(spawnInterval * enemyCount + 10f));
        this.StageLevel = stageLevel;
    }

    private IEnumerator StageCoroutine(float StageTimer)
    {
        yield return new WaitForSeconds(StageTimer);
        StopStage();
    }

    void StopStage()
    {
        // TODO : NextStage든 끝나고 UI를 띄우든 스테이지 종료 코드  
        stageCoroutine = null;
        Debug.Log(StageLevel + "Stage 종료");
    }
}
