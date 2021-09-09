using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// ゲーム全体を管理するクラス
/// </summary>
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    /// <summary>
    /// ステージクリア時のイベント
    /// </summary>
    public static Action GameEnd;

    /// <summary>
    /// ステージの更新がある時のフラグ
    /// </summary>
    public bool IsStageUpdated { get; set; } = false;

    /// <summary>
    /// ベイクルバレーのクリアフラグ
    /// </summary>
    public bool IsBakeleValleyCleared { get; set; } = false;

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
