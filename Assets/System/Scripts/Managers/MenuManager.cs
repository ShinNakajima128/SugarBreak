using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuState
{
    Close,
    Open,
    Tutorial,
    Audio,
    Option,
    Exit
}

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject[] m_menuPanels = default;
    Dictionary<MenuState, int> menuIndex = new Dictionary<MenuState, int>();


    void Start()
    {
        for (int i = 0; i < m_menuPanels.Length; i++)
        {
            MenuState menuState = (MenuState)(i + 1);
            menuIndex.Add(menuState, i);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ActiveMenu(MenuState.Audio);
        }
    }

    public void ActiveMenu(MenuState menu)
    {
        if (menu == MenuState.Close)
        {
            for (int i = 0; i < m_menuPanels.Length; i++)
            {
                m_menuPanels[i].SetActive(false);
            }
            return;
        }

        for (int i = 0; i < m_menuPanels.Length; i++)
        {
            if (i == GetMenu(menu))
            {
                m_menuPanels[i].SetActive(true);
            }
            else
            {
                m_menuPanels[i].SetActive(false);
            }
        }
    }

    public int GetMenu(MenuState menu)
    {
        if (menuIndex.ContainsKey(menu))
        {
            return menuIndex[menu];
        }
        else
        {
            return 0;
        }
    }
        
}
