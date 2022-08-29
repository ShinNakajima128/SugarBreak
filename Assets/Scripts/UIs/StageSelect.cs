using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum StageSelectState
{
    None,
    BakedValley,
    RaindyClouds,
    DessertResort,
    GlaseSnowField,
    GanacheVolcano
}

/// <summary>
/// ステージ選択画面の機能を持つクラス
/// </summary>
public class StageSelect : MonoBehaviour
{
    [SerializeField]
    GameObject[] m_stageGuidePanels = default;

    [SerializeField]
    StageSelectState m_stageSelectState = default;

    [SerializeField]
    Text[] m_stageNames = default;

    [SerializeField]
    Button[] m_stageButtons = default;

    [SerializeField]
    GameObject[] m_updateIcons = default;

    PlayerData m_currentPlayerData;
    bool m_init = false;

    private void OnEnable()
    {
        Debug.Log("ステージ選択");
        if (!m_init)
        {
            m_currentPlayerData = DataManager.Instance.GetPlayerData;
            m_init = true;
        }
        StageNameUpdate();
    }

    private void OnDisable()
    {
        ChangeUIPanel(StageSelectState.None);

        foreach (var s in m_updateIcons)
        {
            s.SetActive(false);
        }
    }

    /// <summary>
    /// 開放状況に応じてステージUIを表示する
    /// </summary>
    /// <param name="type"> 表示するステージの種類 </param>
    public void OnStage(int type)
    {
        var stage = (StageSelectState)type;

        GameManager.Instance.IsStageUpdated = false;
        GameManager.Instance.CurrentStage = m_currentPlayerData.StageData[type - 1];
        switch (stage)
        {
            case StageSelectState.None:
                Debug.LogError("値が不正です。ボタンに設定している値を再確認してください");
                break;
            case StageSelectState.BakedValley:
                ChangeUIPanel(StageSelectState.BakedValley);
                break;
            case StageSelectState.RaindyClouds:
                if (!m_currentPlayerData.StageData[0].IsStageCleared)
                {
                    return;
                }

                ChangeUIPanel(StageSelectState.RaindyClouds);
                m_updateIcons[0].SetActive(false);
                m_currentPlayerData.StageData[1].ConfirmStageUnlocked = true;
                break;
            case StageSelectState.DessertResort:
                if (!m_currentPlayerData.StageData[1].IsStageCleared)
                {
                    return;
                }

                ChangeUIPanel(StageSelectState.DessertResort);
                m_updateIcons[1].SetActive(false);
                GameManager.Instance.IsStageUpdated = false;
                m_currentPlayerData.StageData[2].ConfirmStageUnlocked = true; 
                break;
            case StageSelectState.GlaseSnowField:
                if (!m_currentPlayerData.StageData[2].IsStageCleared)
                {
                    return;
                }
                ChangeUIPanel(StageSelectState.GlaseSnowField);
                m_updateIcons[2].SetActive(false);
                m_currentPlayerData.StageData[3].ConfirmStageUnlocked = true;
                break;
            case StageSelectState.GanacheVolcano:
                if (!m_currentPlayerData.StageData[3].IsStageCleared) 
                {
                    return; 
                }

                ChangeUIPanel(StageSelectState.GanacheVolcano);
                m_updateIcons[3].SetActive(false);
                m_currentPlayerData.StageData[4].ConfirmStageUnlocked = true;
                break;
        }
    }

    /// <summary>
    /// UIの画面を変更する
    /// </summary>
    /// <param name="state"> 表示するステージ </param>
    void ChangeUIPanel(StageSelectState state)
    {
        m_stageSelectState = state;

        switch (m_stageSelectState)
        {
            case StageSelectState.None:
                PanelChange(5);
                break;
            case StageSelectState.BakedValley:
                PanelChange(0);
                break;
            case StageSelectState.RaindyClouds:
                PanelChange(1);
                break;
            case StageSelectState.DessertResort:
                PanelChange(2);
                break;
            case StageSelectState.GlaseSnowField:
                PanelChange(3);
                break;
            case StageSelectState.GanacheVolcano:
                PanelChange(4);
                break;
        }
    }

    public void PanelChange(int index)
    {
        if(index >= 5)
        {
            foreach (var panel in m_stageGuidePanels)
            {
                panel.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < m_stageGuidePanels.Length; i++)
            {
                if (i == index)
                {
                    m_stageGuidePanels[i].SetActive(true);
                }
                else
                {
                    m_stageGuidePanels[i].SetActive(false);
                }
            }
        }  
    }

    /// <summary>
    /// ステージ名の表示を更新
    /// </summary>
    void StageNameUpdate()
    {
        for (int i = 0; i < m_stageNames.Length; i++)
        {
            if (m_currentPlayerData.StageData[i].IsStageCleared)
            {
                m_stageNames[i].text = m_currentPlayerData.StageData[i + 1].StageName;
                if (!m_currentPlayerData.StageData[i + 1].ConfirmStageUnlocked)
                {
                    m_updateIcons[i].SetActive(true);
                }
            }
            else
            {
                m_stageButtons[i].interactable = false;
                m_stageNames[i].text = "？？？？？";
            }
        }
    }
}
