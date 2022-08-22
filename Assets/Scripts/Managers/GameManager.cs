using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Cinemachine;

[Serializable]
public class Stage
{
    public string StageName = "";
    public StageTypes StageType = default;
    public bool IsStageCleared = false;
    public bool ConfirmStageUnlocked = false;
    //public bool[] IsDungeonCleared = default;
}

/// <summary>
/// ステージの種類
/// </summary>
public enum StageTypes
{
    BakeleValley,
    RaindyClouds,
    DesertResort,
    GlaseSnowField,
}

public enum ClearTypes
{
    Stage,
    Dungion
}

/// <summary>
/// ゲーム全体を管理するクラス
/// </summary>
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField]
    bool isStageUpdated = false;

    [SerializeField]
    EnemyData[] m_stageBossData = default;

    [Header("デバッグ用")]
    [SerializeField]
    bool m_debugMode = false;

    [SerializeField]
    GameObject m_debugObject = default;

    CinemachineImpulseSource m_impulseSource;

    /// <summary>
    /// ステージクリア時のイベント
    /// </summary>
    public static Action GameEnd;

    /// <summary>
    /// 現在のステージ
    /// </summary>
    public Stage CurrentStage { get; set; }

    /// <summary>
    /// ゲームを開始フラグ
    /// </summary>
    public bool GameStarted { get; set; }

    /// <summary>
    /// ステージの更新がある時のフラグ
    /// </summary>
    public bool IsStageUpdated { get => isStageUpdated; set => isStageUpdated = value; }

    public bool IsPlayingMovie { get; set; } = false;

    public EnemyData CurrentBossData { get; set; }
    public bool DebugMode { get => m_debugMode; set => m_debugMode = value; }

    void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        m_impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    private void Start()
    {
        if (m_debugMode)
        {
            CurrentStage = DataManager.Instance.GetPlayerData.StageData[0];
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        var sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "Title":
                if (GameEnd != null)
                {
                    GameEnd = null;
                }
                AudioManager.PlayBGM(BGMType.Title);
                break;
            case "Base":
                if (GameEnd != null)
                {
                    GameEnd = null;
                }
                SaveManager.Save(DataTypes.All);
                AudioManager.PlayBGM(BGMType.Base_Main);
                break;
            case "BakedValley":
                AudioManager.PlayBGM(BGMType.BakeleValley_Main);
                CurrentBossData = m_stageBossData[0];
                break;
            case "CookieDungeon":
                MenuManager.Instance.WhetherOpenMenu = true;
                break;
            case "BossBattle":
                AudioManager.PlayBGM(BGMType.BakeleValley_Boss);
                CurrentBossData = m_stageBossData[0];
                break;
        }
        EventManager.ListenEvents(Events.CameraShake, CameraShake);
    }

    void Update()
    {
        if (m_debugMode)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                m_debugObject.SetActive(m_debugObject.activeSelf ? false : true);
            }
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
                AudioManager.PlayBGM(BGMType.Title);
                break;
            case "Base":
                if (GameEnd != null)
                {
                    GameEnd = null;
                }
                SaveManager.Save(DataTypes.All);
                AudioManager.PlayBGM(BGMType.Base_Main);
                break;
            case "BakedValley":
                AudioManager.PlayBGM(BGMType.BakeleValley_Main);
                CurrentBossData = m_stageBossData[0];
                break;
            case "BossBattle":
                AudioManager.PlayBGM(BGMType.BakeleValley_Boss);
                CurrentBossData = m_stageBossData[0];
                break;
        }
        EventManager.ListenEvents(Events.CameraShake, CameraShake);
    }

    /// <summary>
    /// カメラを揺らす
    /// </summary>
    void CameraShake()
    {
        m_impulseSource.GenerateImpulseAt(PlayerController.Instance.gameObject.transform.position, Vector3.down);
    }

    /// <summary>
    /// 拠点Sceneに遷移する
    /// </summary>
    IEnumerator LoadBase()
    {
        Debug.Log("ロード開始");
        yield return new WaitForSeconds(2.0f);

        GameEnd?.Invoke();
    }
}
