using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// ゲーム全体を管理するクラス
/// </summary>
public class GameManager : SingletonMonoBehaviour<GameManager>
{

    [SerializeField]
    bool isStageUpdated = false;

    [SerializeField]
    bool isBakeleValleyCleared = false;

    [SerializeField]
    bool isRaindyCloudsCleared = false;
    [SerializeField]
    bool isDesertResortCleared = false;
    [SerializeField]
    bool isGlaseSnowFieldCleared = false;

    /// <summary>
    /// ステージクリア時のイベント
    /// </summary>
    public static Action GameEnd;

    /// <summary>
    /// ゲームを開始フラグ
    /// </summary>
    public bool GameStarted { get; set; }

    /// <summary>
    /// ステージの更新がある時のフラグ
    /// </summary>
    public bool IsStageUpdated { get => isStageUpdated; set => isStageUpdated = value; }

    /// <summary>
    /// ベイクルバレーのクリアフラグ
    /// </summary>
    public bool IsBakeleValleyCleared { get => isBakeleValleyCleared; set => isBakeleValleyCleared = value; }

    public bool IsRaindyCloudsCleared { get => isRaindyCloudsCleared; set=> isRaindyCloudsCleared = value; }

    public bool IsDesertResortCleared { get => isDesertResortCleared; set => isDesertResortCleared = value; }

    public bool IsGlaseSnowFieldCleared { get => isGlaseSnowFieldCleared; set => isGlaseSnowFieldCleared = value; }


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
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (SceneManager.GetActiveScene().name == "Title")
        {
        }
        else if (SceneManager.GetActiveScene().name == "Base")
        {
        }
        else if (SceneManager.GetActiveScene().name == "BakedValley")
        {
        }
    }

    public void OnGameEnd()
    {
        GameEnd?.Invoke();
    }

    void OnSceneLoaded(Scene nextScene, LoadSceneMode mode)
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Title":
                Cursor.visible = true;
                break;
            case "BakedValley":
                Cursor.visible = false;
                break;
            case "Base":
                Cursor.visible = true;
                break;
        }
    }
}
