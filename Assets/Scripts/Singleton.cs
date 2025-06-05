using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private static object lockObject = new object();
    private static bool applicationIsQuitting = false;

    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning("[Singleton] 이미 종료된 애플리케이션에서 " + typeof(T) + " 인스턴스를 요청했습니다.");
                return null;
            }

            lock (lockObject)
            {
                if (instance == null)
                {
                    instance = Object.FindFirstObjectByType<T>();

                    if (Object.FindObjectsByType<T>(FindObjectsSortMode.None).Length > 1)
                        return instance;

                    if (instance == null)
                    {
                        GameObject singleton = new GameObject();
                        instance = singleton.AddComponent<T>();
                        singleton.name = typeof(T).ToString();
                        DontDestroyOnLoad(singleton);
                    }
                }

                return instance;
            }
        }
    }

    protected virtual void Awake()
    {
        lock (lockObject)
        {
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    protected virtual void OnDestroy()
    {
        applicationIsQuitting = true;
    }
}
