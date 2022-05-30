using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler<T> : MonoBehaviour where T : GameHandler<T>
{
    public static GameHandler<T> Instance;

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }
}
