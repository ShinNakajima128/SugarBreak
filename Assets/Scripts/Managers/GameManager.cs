using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public static Action GameEnd;

    void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

       if (GameEnd != null)
        {
            GameEnd = null;
        }
    }

    public void OnGameEnd()
    {
        GameEnd?.Invoke();
    }
}
