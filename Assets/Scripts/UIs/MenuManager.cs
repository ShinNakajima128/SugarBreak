﻿using System.Collections;
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
    [SerializeField] 
    GameObject[] m_menuPanels = default;

    Dictionary<MenuState, int> menuIndex = new Dictionary<MenuState, int>();
    MenuState state = MenuState.Close;


    void Start()
    {
        Cursor.visible = false;
        for (int i = 0; i < m_menuPanels.Length; i++)
        {
            MenuState menuState = (MenuState)(i + 1);
            menuIndex.Add(menuState, i);
        }

        foreach (var menu in m_menuPanels)
        {
            menu.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))   
        {
            if (state == MenuState.Close)   //メニューを開く
            {
                Cursor.visible = true;
                Time.timeScale = 0f;
                ActiveMenu(MenuState.Open);
                state = MenuState.Open;
            }
            else if (state != MenuState.Close)  //メニューを閉じる
            {
                Cursor.visible = false;
                Time.timeScale = 1f;
                ActiveMenu(MenuState.Close);
                state = MenuState.Close;
            }
        }
    }

    /// <summary>
    /// 指定のメニューを開く
    /// </summary>
    /// <param name="menu"> 開くメニュー </param>
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