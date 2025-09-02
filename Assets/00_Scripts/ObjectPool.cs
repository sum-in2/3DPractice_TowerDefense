using UnityEngine;
using System.Collections.Generic;

public class ObjectPool<T> where T : Component
{
    private T prefab;
    private Queue<T> objects = new Queue<T>();
    private Transform parent;

    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;
        for (int i = 0; i < initialSize; i++)
        {
            T obj = GameObject.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            objects.Enqueue(obj);
        }
    }

    public T GetObject()
    {
        if (objects.Count == 0)
        {
            T obj = GameObject.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            objects.Enqueue(obj);
        }

        T dequeuedObject = objects.Dequeue();
        dequeuedObject.gameObject.SetActive(true);
        return dequeuedObject;
    }

    public void ReturnObject(T obj)
    {
        if (obj is Enemy) obj.gameObject.GetComponent<Enemy>().ResetHP();
        obj.gameObject.SetActive(false);
        objects.Enqueue(obj);
    }
}