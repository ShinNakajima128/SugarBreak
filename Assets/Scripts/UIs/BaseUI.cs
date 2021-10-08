using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public enum BaseUIState
{
    Main,
    StageSelect,
    ItemMake,
    Weapon,
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

    Button m_currentButton = default;

    private void Awake()
    {
        if (OnButtonScaleReset != null) OnButtonScaleReset = null;
    }
    void Start()
    {
        m_updateIcon?.SetActive(false);
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
            m_currentButton.Select();
        }

        ChangeUIPanel(BaseUIState.Main);

        if ((bool)(!m_updateIcon?.activeSelf) && GameManager.Instance.IsStageUpdated)
        {
            m_updateIcon?.SetActive(true);
        }
    }

    /// <summary>
    /// ステージ選択画面を表示する
    /// </summary>
    public void OnStageSelect()
    {
        ChangeUIPanel(BaseUIState.StageSelect);

        if ((bool)(m_updateIcon?.activeSelf))
        {
            m_updateIcon?.SetActive(false);
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
    public void OnWeapon()
    {
        ChangeUIPanel(BaseUIState.Weapon);
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
        SoundManager.Instance.PlaySeByName("Cancel");
    }

    public void PlaySE()
    {
        //SoundManager.Instance.PlaySeByName("CursolMove");
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
        m_baseUI = state;

        switch (m_baseUI)
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
                PanelChange(3);
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
    }

    void PanelChange(int index)
    {
        for (int i = 0; i < m_menuPanels.Length; i++)
        {
            if (i == index)
            {
                m_menuPanels[i]?.SetActive(true);
                m_menuButtons[i].Select();
            }
            else
            {
                m_menuPanels[i]?.SetActive(false);
            }
        }
    }
    void OnConfirmPanel()
    {
        m_confirmPanel?.SetActive(true);
    }

    void SaveSelectButton()
    {
        m_currentButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
    }
}
