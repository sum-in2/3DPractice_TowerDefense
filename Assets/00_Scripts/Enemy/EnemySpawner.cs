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
        SpawnEnemies("TestMonster");
    }

    public void SpawnEnemies(string enemyName, float spawnInterval = 1f, int enemyCount = 20)
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);

        spawnCoroutine = StartCoroutine(SpawnRoutine(enemyName, spawnInterval, enemyCount));
    }

    private IEnumerator SpawnRoutine(string enemyName, float spawnInterval, int enemyCount)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            if (enemyPrefabDict.TryGetValue(enemyName, out Enemy prefab))
            {
                Enemy enemy = ObjectPoolManager.Instance.GetObject(prefab);
                enemy.transform.position = GetSpawnPosition().position;
                enemy.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning($"적 프리팹 없음: {enemyName}");
            }
            yield return new WaitForSeconds(spawnInterval);
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
