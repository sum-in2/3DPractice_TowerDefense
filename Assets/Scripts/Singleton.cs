using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static object _lock = new object();
    private static bool _applicationIsQuitting = false;

    public static T Instance
    {
        get
        {
            if (_applicationIsQuitting)
            {
                Debug.LogWarning("[Singleton] 이미 종료된 애플리케이션에서 " + typeof(T) + " 인스턴스를 요청했습니다.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = Object.FindFirstObjectByType<T>();

                    if (Object.FindObjectsByType<T>(FindObjectsSortMode.None).Length > 1)
                    {
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton) " + typeof(T).ToString();

                        DontDestroyOnLoad(singleton);
                    }
                }

                return _instance;
            }
        }
    }

    protected virtual void OnDestroy()
    {
        _applicationIsQuitting = true;
    }
}
