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
    Text[] m_StageNames = default;

    [SerializeField]
    GameObject[] m_updateIcons = default;

    static bool IsStage2Updated = false;

    private void OnEnable()
    {
        for (int i = 0; i < m_StageNames.Length; i++)
        {
            if (i == 0)
            {
                if (GameManager.Instance.IsBakeleValleyCleared)
                {
                    m_StageNames[i].text = "レインディ雲海";
                    if (!IsStage2Updated)
                    {
                        m_updateIcons[i].SetActive(true);
                        IsStage2Updated = true;
                    }
                }
                else
                {
                    m_StageNames[i].text = "？？？？？？？？";
                }
            }
            else
            {
                m_StageNames[i].text = "？？？？？？？？";
            }

        }
    }

    private void OnDisable()
    {
        ChangeUIPanel(StageSelectState.None);
        GameManager.Instance.IsStageUpdated = false;
    }

    public void OnBakedValley()
    {
        ChangeUIPanel(StageSelectState.BakedValley);
    }

    public void OnRaindyClouds()
    {
        if (!GameManager.Instance.IsBakeleValleyCleared) return;

        ChangeUIPanel(StageSelectState.RaindyClouds);
        m_updateIcons[0].SetActive(false);
    }

    public void OnDesertResort()
    {
        if (!GameManager.Instance.IsRaindyCloudsCleared) return;

        ChangeUIPanel(StageSelectState.DessertResort);
        m_updateIcons[1].SetActive(false);
    }

    public void OnGlaseSnowField()
    {
        if (!GameManager.Instance.IsDesertResortCleared) return;

        ChangeUIPanel(StageSelectState.GlaseSnowField);
        m_updateIcons[2].SetActive(false);
    }

    public void OnGanacheVolcano()
    {
        if (!GameManager.Instance.IsGlaseSnowFieldCleared) return;

        ChangeUIPanel(StageSelectState.GanacheVolcano);
    }

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

    void PanelChange(int index)
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
}
