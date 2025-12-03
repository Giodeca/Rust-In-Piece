using System;
using UnityEngine;

public class SingletonA<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static bool IsNull => _instance == null;

    public static T Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<T>();
            if (_instance == null)
                throw new NullReferenceException("Singleton<" + typeof(T) + "> istanza non trovata");
            return _instance;
        }
    }

    public bool isDebug;

    protected virtual void Awake()
    {
        if (_instance != null && _instance != (this as T))
        {
            Destroy(_instance.gameObject);
        }
        else
        {
            _instance = this as T;
        }
    }
}