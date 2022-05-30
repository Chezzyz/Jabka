using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGameHandler<T> : MonoBehaviour where T : BaseGameHandler<T>
{
    public static T Instance;

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = FindObjectOfType<T>()?.GetComponent<T>(); ;
        }

        DontDestroyOnLoad(gameObject);
    }
}
