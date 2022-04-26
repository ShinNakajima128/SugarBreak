using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// マップを開閉して操作するクラス
/// </summary>
public class Map : MonoBehaviour
{
    public static Map Instance { get; private set; }

    [SerializeField]
    GameObject m_mapUI;

    [SerializeField]
    GameObject m_mainPanelUI = default;

    [SerializeField]
    Volume m_volume = default;

    DepthOfField m_depthOfField;
    bool pauseFlag = false;

    public bool PauseFlag => pauseFlag;

    void Awake()
    {
        Instance = this;
        m_volume.profile.TryGet(out m_depthOfField);
    }

    void Start()
    {
        EventManager.ListenEvents(Events.OnHUD, OffMap);
        EventManager.ListenEvents(Events.OffHUD, OnMap);
    }

    void Update()
    {

        if (MenuManager.Instance.MenuStates == MenuState.Close && !GameManager.Instance.IsPlayingMovie)     //プレイヤーが操作できる時且つメニューが開かれていない状態なら
        {
            if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown("joystick button 3"))   //キーボードのMかゲームパッドのYボタンが押されたら
            {
                pauseFlag = !pauseFlag;

                if (pauseFlag)
                {
                    EventManager.OnEvent(Events.OffHUD);
                    PlayerStatesManager.Instance.OffOperation();
                    Time.timeScale = 0;
                }
                else
                {
                    EventManager.OnEvent(Events.OnHUD);
                    Time.timeScale = 1;
                    PlayerStatesManager.Instance.OnOperation();
                }
            }          
        }
    }

    /// <summary>
    /// マップを表示する
    /// </summary>
    void OnMap()
    {
        m_mapUI.SetActive(true);
        m_mainPanelUI.SetActive(false);
        m_depthOfField.gaussianStart.value = 0;
        m_depthOfField.gaussianEnd.value = 0;
    }

    /// <summary>
    /// マップを非表示にする
    /// </summary>
    void OffMap()
    {
        m_mapUI.SetActive(false);
        m_mainPanelUI.SetActive(true);
        m_depthOfField.gaussianStart.value = 25.5f;
        m_depthOfField.gaussianEnd.value = 86;
    }
}
