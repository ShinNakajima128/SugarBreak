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

    [SerializeField]
    List<WeaponBase> m_currentWeaponList = new List<WeaponBase>();

    public List<WeaponBase> CurrentWeaponList { get => m_currentWeaponList; set => m_currentWeaponList = value; }
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

    public void OnGameEndClearedStage()
    {
        StartCoroutine(LoadBase());
    }

    void OnSceneLoaded(Scene nextScene, LoadSceneMode mode)
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Title":
                if (GameEnd != null)
                {
                    GameEnd = null;
                }
                break;
            case "BakedValley":
                break;
            case "Base":
                if (GameEnd != null)
                {
                    GameEnd = null;
                }
                break;
        }
    }

    IEnumerator LoadBase()
    {
        Debug.Log("ロード開始");
        yield return new WaitForSeconds(3.0f);

        GameEnd?.Invoke();
    }
}
