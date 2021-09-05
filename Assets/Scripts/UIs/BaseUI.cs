using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BaseUIState
{
    Main,
    StageSelect,
    ItemMake,
    Option,
    Tutorial,
    Exit
}

public class BaseUI : MonoBehaviour
{
    [SerializeField]
    BaseUIState m_baseUI = BaseUIState.Main;
    [SerializeField] GameObject[] m_menuPanels = default;

    void Start()
    {
        OnMain();
    }

    public void OnMain()
    {
        ChangeUIPanel(BaseUIState.Main);
    }

    public void OnStageSelect()
    {
        ChangeUIPanel(BaseUIState.StageSelect);
    }
    public void OnItemMake()
    {
        ChangeUIPanel(BaseUIState.ItemMake);
    }
    public void OnOption()
    {
        ChangeUIPanel(BaseUIState.Option);
    }
    public void OnTutorial()
    {
        ChangeUIPanel(BaseUIState.Tutorial);
    }
    public void OnExit()
    {
        ChangeUIPanel(BaseUIState.Exit);
    }

    void ChangeUIPanel(BaseUIState state)
    {
        m_baseUI = state;

        switch (m_baseUI)
        {
            case BaseUIState.Main:
                PanelChange(0);
                break;
            case BaseUIState.StageSelect:
                PanelChange(1);
                break;
            case BaseUIState.ItemMake:
                PanelChange(2);
                break;
            case BaseUIState.Option:
                PanelChange(3);
                break;
            case BaseUIState.Tutorial:
                PanelChange(4);
                break;
            case BaseUIState.Exit:
                PanelChange(5);
                break;
        }
    }

    void PanelChange(int index)
    {
        for (int i = 0; i < m_menuPanels.Length; i++)
        {
            if (i == index)
            {
                m_menuPanels[i].SetActive(true);
            }
            else
            {
                m_menuPanels[i].SetActive(false);
            }
        }
    }
}
