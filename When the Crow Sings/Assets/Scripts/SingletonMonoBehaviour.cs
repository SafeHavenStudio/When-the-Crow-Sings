using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour : MonoBehaviour
{
    private static SingletonMonoBehaviour instance;

    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
