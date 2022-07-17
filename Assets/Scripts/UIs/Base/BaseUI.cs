using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 拠点のUIのステータス
/// </summary>
public enum BaseUIState
{
    Main,
    StageSelect,
    ItemMake,
    Weapon,
    WeaponMenu,
    Option,
    Collection,
    Exit
}

/// <summary>
/// 拠点のUIをコントロールするクラス
/// </summary>
public class BaseUI : MonoBehaviour
{
    [Header("UIのステータス")]
    [SerializeField]
    BaseUIState m_baseUI = BaseUIState.Main;
    
    [Header("各メニューのパネル")]
    [SerializeField] 
    GameObject[] m_menuPanels = default;

    [Header("タイトルに戻る時の確認画面")]
    [SerializeField]
    GameObject m_confirmPanel = default;

    [Header("ステージが更新された時に表示するアイコン")]
    [SerializeField]
    GameObject m_updateIcon = default;

    [Header("各メニューの最初に選択するボタン")]
    [SerializeField]
    Button[] m_menuButtons = default;

    public static Action OnButtonScaleReset = default;

    Button _currentButton = default;

    void Awake()
    {
        if (OnButtonScaleReset != null) OnButtonScaleReset = null;
    }
    void Start()
    {
        m_updateIcon.SetActive(false);
        OnMain();

        if (Cursor.lockState != CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    /// <summary>
    /// メイン画面を表示する
    /// </summary>
    public void OnMain()
    {
        if (m_baseUI != BaseUIState.Main)
        {
            //OnButtonScaleReset?.Invoke();
            _currentButton.Select();
        }

        ChangeUIPanel(BaseUIState.Main);

        if (!m_updateIcon.activeSelf && GameManager.Instance.IsStageUpdated)
        {
            m_updateIcon.SetActive(true);
        }
    }

    /// <summary>
    /// ステージ選択画面を表示する
    /// </summary>
    public void OnStageSelect()
    {
        ChangeUIPanel(BaseUIState.StageSelect);

        if (m_updateIcon.activeSelf)
        {
            m_updateIcon.SetActive(false);
        }
    }

    /// <summary>
    /// アイテム作成画面を表示する
    /// </summary>
    public void OnItemMake()
    {
        ChangeUIPanel(BaseUIState.ItemMake);
    }

    /// <summary>
    /// 装備画面を表示する
    /// </summary>
    public void OnEquipment()
    {
        ChangeUIPanel(BaseUIState.Weapon);
    }

    /// <summary>
    /// 装備のメニュー画面を表示する
    /// </summary>
    public void OnWeaponMenu()
    {
        ChangeUIPanel(BaseUIState.WeaponMenu);
    }

    /// <summary>
    /// オプション画面を表示する
    /// </summary>
    public void OnOption()
    {
        ChangeUIPanel(BaseUIState.Option);
    }

    /// <summary>
    /// チュートリアル画面を表示する
    /// </summary>
    public void OnCollection()
    {
        ChangeUIPanel(BaseUIState.Collection);
    }

    /// <summary>
    /// ゲーム終了画面を表示する
    /// </summary>
    public void OnExit()
    {
        ChangeUIPanel(BaseUIState.Exit);
    }

    public void CancelSE()
    {
        AudioManager.PlaySE(SEType.UI_Cancel);
    }

    public void PlaySE()
    {
        AudioManager.PlaySE(SEType.UI_CursolMove);
    }
    /// <summary>
    /// タイトルSceneに遷移する
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadTitle(string sceneName)
    {
        LoadSceneManager.Instance.AnyLoadScene(sceneName);
    }

    /// <summary>
    /// ステータスによってパネルを切り替える
    /// </summary>
    /// <param name="state"></param>
    void ChangeUIPanel(BaseUIState state)
    {
        switch (state)
        {
            case BaseUIState.Main:
                PanelChange(0);
                if (m_confirmPanel.activeSelf) m_confirmPanel.SetActive(false);
                break;
            case BaseUIState.StageSelect:
                SaveSelectButton();
                PanelChange(1);
                break;
            case BaseUIState.ItemMake:
                SaveSelectButton();
                PanelChange(2);
                break;
            case BaseUIState.Weapon:

                SaveSelectButton();
                
                //武器メニュー画面から戻る場合は画面フェードを入れる
                if (m_baseUI == BaseUIState.WeaponMenu)
                {
                    LoadSceneManager.Instance.FadeIn(callback: () =>
                    {
                        PanelChange(3);
                        LoadSceneManager.Instance.FadeOut();
                    });
                }
                else
                {
                    PanelChange(3);
                }
                break;
            case BaseUIState.WeaponMenu:
                SaveSelectButton();
                LoadSceneManager.Instance.FadeIn(callback:() => 
                {
                    PanelChange(6);
                    LoadSceneManager.Instance.FadeOut();
                });
                break;
            case BaseUIState.Option:
                SaveSelectButton();
                PanelChange(4);
                break;
            case BaseUIState.Collection:
                SaveSelectButton();
                PanelChange(5);
                break;
            case BaseUIState.Exit:
                SaveSelectButton();
                OnConfirmPanel();
                m_menuButtons[6].Select();
                break;
        }
        m_baseUI = state;
    }

    void PanelChange(int index)
    {
        for (int i = 0; i < m_menuPanels.Length; i++)
        {
            if (i == index)
            {
                m_menuPanels[i].SetActive(true);
                try
                {
                    m_menuButtons[i].Select();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    throw;
                }
            }
            else
            {
                m_menuPanels[i].SetActive(false);
            }
        }
    }
    void OnConfirmPanel()
    {
        m_confirmPanel.SetActive(true);
    }

    void SaveSelectButton()
    {
        _currentButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
    }
}
