using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour  where T : MonoBehaviour
{
    [SerializeField] bool isDontDestroy = false;
    static T _instance;

    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                if(_instance = GameObject.FindObjectOfType<T>())
                {
                    Debug.Log("Instance found. (Singleton)");
                }
                else
                {
                    GameObject singleton = new GameObject(typeof(T).Name);
                    _instance = singleton.AddComponent<T>();
                    Debug.Log("Instance not found, creating new instance. (Singleton)");
                }
            }
            return _instance;
        }
    }

    public virtual void Awake()
    {
        if(_instance == null)
        {
            _instance = this as T;

            if (isDontDestroy)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            if(_instance != null)
            {
                Destroy(gameObject);
            }
        }
    }
}
