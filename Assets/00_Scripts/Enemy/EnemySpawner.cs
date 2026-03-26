using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy[] enemyPrefabs;
    [SerializeField] private Transform spawnTransform;
    private Dictionary<string, Enemy> enemyPrefabDict;
    private Coroutine spawnCoroutine;

    void Awake()
    {
        enemyPrefabDict = new Dictionary<string, Enemy>();
        foreach (Enemy prefab in enemyPrefabs)
        {
            ObjectPoolManager.Instance.CreatePool(prefab, 20, transform);
            enemyPrefabDict.Add(prefab.name, prefab);
        }

    }

    void Start()
    {
    }

    /// <summary>
    /// 적 스폰 코루틴이 포함되어 있는 메서드
    /// </summary>
    public void SpawnEnemies(StageInfo stageInfo)
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);

        spawnCoroutine = StartCoroutine(SpawnRoutine(stageInfo));
    }

    private IEnumerator SpawnRoutine(StageInfo stageInfo)
    {
        for (int i = 0; i < stageInfo.enemyCount; i++)
        {
            if (enemyPrefabDict.TryGetValue(stageInfo.monsterName, out Enemy prefab))
            {
                Enemy enemy = ObjectPoolManager.Instance.GetObject(prefab);
                enemy.transform.position = GetSpawnPosition().position;
                enemy.Setup(stageInfo.baseReward, stageInfo.baseEXP, stageInfo.maxHP);
                enemy.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning($"적 프리팹 없음: {stageInfo.monsterName}");
            }
            yield return new WaitForSeconds(stageInfo.spawnInterval);
        }
        spawnCoroutine = null;
    }

    private Transform GetSpawnPosition()
    {
        return spawnTransform;
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }
}
