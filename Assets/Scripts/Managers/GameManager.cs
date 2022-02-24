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
    public bool IsStageCleared = false;

    public bool[] IsDungeonCleared = default;
}

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

    CinemachineImpulseSource m_impulseSource;

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

    public bool IsPlayingMovie { get; set; } = false;

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
        SceneManager.sceneLoaded += OnSceneLoaded;

        var sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "Title")
        {
        }
        else if (sceneName == "Base")
        {
        }
        else if (sceneName == "BakedValley")
        {
        }
        else if (sceneName == "CookieDungeon")
        {
            MenuManager.Instance.WhetherOpenMenu = true;
        }
        EventManager.ListenEvents(Events.CameraShake, CameraShake);
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
        yield return new WaitForSeconds(3.0f);

        GameEnd?.Invoke();
    }
}
