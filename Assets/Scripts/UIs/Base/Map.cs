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
    [Tooltip("マップUIオブジェクト")]
    [SerializeField]
    GameObject m_mapUI;

    [Tooltip("体力や武器アイコンを表示するメインPanel")]
    [SerializeField]
    GameObject m_mainPanelUI = default;

    [SerializeField]
    RectTransform m_iconTrans = default;

    Transform _currentplayerTrans;
    Vector3 _beforePlayerPos; 
    bool pauseFlag = false;
    Animator _anim;

    public static Map Instance { get; private set; }

    public bool PauseFlag => pauseFlag;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        EventManager.ListenEvents(Events.OnHUD, OffMap);
        EventManager.ListenEvents(Events.OnMap, OnMap);
        _currentplayerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        _beforePlayerPos = _currentplayerTrans.position;
        _anim = m_iconTrans.gameObject.GetComponent<Animator>();

        if (_anim)
        {
            _anim.updateMode = AnimatorUpdateMode.UnscaledTime; //Time.timeScaleの影響を受けないように変更
        }

        //メインPanelがセットされていなかったら
        if (m_mainPanelUI == null)
        {
            Debug.LogError("メインPanelをInspectorに設定してください");
        }
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
                    EventManager.OnEvent(Events.OnMap);
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

        var pos = _currentplayerTrans.position - _beforePlayerPos; //前にマップを開いた位置と現在の位置の差分を計算
        m_iconTrans.localPosition += new Vector3(pos.x, pos.z, 0) * 2; //マップ上のアイコンに差分を反映し位置を調整
    }

    /// <summary>
    /// マップを非表示にする
    /// </summary>
    void OffMap()
    {
        m_mapUI.SetActive(false);
        m_mainPanelUI.SetActive(true);
        
        _beforePlayerPos = _currentplayerTrans.position;
    }
}
