using System;
using System.Collections.Generic;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] EnemySpawner enemySpawner;

    private StageState state = StageState.Idle;
    private Coroutine stageCoroutine;
    private StageData stageData => GameManager.Instance.GetStageData();

    public bool canStartStage => state == StageState.Idle;

    public void StartStage(float spawnInterval = 1f, int enemyCount = 20)
    {
        int currentLevel = GameManager.Instance.StageLevel;

        if (!stageData.IsValidLevel(currentLevel))
        {
            Debug.LogWarning($"유효하지 않은 스테이지 레벨: {currentLevel}");
            return;
        }

        if (!canStartStage)
        {
            Debug.LogWarning("스테이지가 진행 중 입니다");
            return;
        }

        state = StageState.Playing;

        StageInfo stageInfo = stageData.GetStageInfo(currentLevel);
        enemySpawner.SpawnEnemies(
            stageInfo.monsterName,
            stageInfo.spawnInterval,
            stageInfo.enemyCount
        );
        stageCoroutine = StartCoroutine(StageCoroutine(stageInfo.spawnInterval * stageInfo.enemyCount + 10f));
    }

    private IEnumerator StageCoroutine(float StageTimer)
    {
        yield return new WaitForSeconds(StageTimer);
        StopStage();
    }

    void StopStage()
    {
        if (stageCoroutine != null)
        {
            StopCoroutine(stageCoroutine);
            stageCoroutine = null;
        }

        state = StageState.Idle;
        Debug.Log($"Stage {GameManager.Instance.StageLevel} 완료");
    }
}
