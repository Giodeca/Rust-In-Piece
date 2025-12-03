using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;
    [SerializeField] protected bool isPersistent;

    protected virtual void Awake()
    {
        SetSingleInstance();
    }

    protected virtual void OnEnable()
    {
        SetSingleInstance();
    }

    private void OnApplicationQuit() 
    {
        Instance = null;
    }

    private void SetSingleInstance()
    {
        if (Instance == this)
            return;
        else if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        //T[] instances = FindObjectsOfType<T>();

        //if (instances.Length > 1)//More than one instance is found in the scene
        //{
        //    Debug.LogError("Multiple instances of " + typeof(T).Name + " found");
        //    //Set this as instance. The others will be destroyed in their Awake
        //}

        //Anyway set the Instance with the first one that arrives here.
        Instance = this as T;

        if (isPersistent)
            DontDestroyOnLoad(gameObject);
    }
}
