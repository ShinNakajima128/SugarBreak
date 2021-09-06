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

public class StageSelect : MonoBehaviour
{
    [SerializeField]
    GameObject[] m_stageGuidePanels = default;

    [SerializeField]
    StageSelectState m_stageSelectState = default;

    private void OnDisable()
    {
        ChangeUIPanel(StageSelectState.None);
    }

    public void OnBakedValley()
    {
        ChangeUIPanel(StageSelectState.BakedValley);
    }

    public void OnRaindyClouds()
    {
        ChangeUIPanel(StageSelectState.RaindyClouds);
    }

    public void OnDesertResort()
    {
        ChangeUIPanel(StageSelectState.DessertResort);
    }

    public void OnGlaseSnowField()
    {
        ChangeUIPanel(StageSelectState.GlaseSnowField);
    }

    public void OnGanacheVolcano()
    {
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
