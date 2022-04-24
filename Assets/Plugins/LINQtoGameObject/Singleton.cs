using UnityEngine;

/// <summary>
/// Make Singleton Class
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    //Destroy Check
    private static bool _shuttingdown = false;
    private static object _lock = new object();
    private static T _instance;

    public static T Instance
    {
        get
        {
            //게임종료 시 object보다 싱글턴의 OnDestroy가 먼저 실행될 수 있다
            //해당 싱글턴을 gameObject.OnDestroy 메서드에서는 사용하지 않거나 사용한다면 null체크
            if (_shuttingdown)
            {
                CatLog.WLog("[Singleton] Instance '" + typeof(T) + "' already Destroyed. Returning Null.");
                return null;
            }

            lock (_lock) //Thread Safe
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    //아직 생성되지 않았다면 인스턴스를 생성함
                    if (_instance == null)
                    {
                        //새로운 싱글턴 게임오브젝트를 만들어서 싱글턴 Attach
                        var singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " [Singleton]";

                        //Make Instance Persistence
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return _instance;
            }
        }
    }

    private void Awake()
    {
#if UNITY_EDITOR
        //InitMessage Output :: Unity Debug
        CatLog.Log(StringColor.BLACK, "Init Singleton Object :: " + typeof(T));
#endif
    }

    private void OnApplicationQuit()
    {
        _shuttingdown = true;
    }

    private void OnDestroy()
    {
        _shuttingdown = true;
    }
}

