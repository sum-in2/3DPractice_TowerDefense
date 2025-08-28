using UnityEngine;
using System.Collections.Generic;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    private Dictionary<Component, object> pools = new Dictionary<Component, object>();

    public void CreatePool<T>(T prefab, int initialSize, Transform parent = null) where T : Component
    {
        if (pools.ContainsKey(prefab))
            return;

        ObjectPool<T> newPool = new ObjectPool<T>(prefab, initialSize, parent);
        pools.Add(prefab, newPool);
    }

    public T GetObject<T>(T prefab) where T : Component
    {
        if (!pools.ContainsKey(prefab))
        {
            Debug.LogWarning($"풀에 없는 프리팹: {prefab.name} - 자동 생성");
            CreatePool(prefab, 10);
        }

        ObjectPool<T> pool = pools[prefab] as ObjectPool<T>;
        return pool.GetObject();
    }

    public void ReturnObject<T>(T obj) where T : Component
    {
        foreach (var kvp in pools)
        {
            if (kvp.Key == null) continue;
            if (obj.gameObject.name.Contains(kvp.Key.gameObject.name))
            {
                var pool = kvp.Value as ObjectPool<T>;
                pool?.ReturnObject(obj);
                return;
            }
        }
        GameObject.Destroy(obj.gameObject);
    }
}