using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuState
{
    Close,
    Open,
    Item,
    Option,
    Picturebook,
    Exit
}

public class MenuManager : MonoBehaviour
{
    [SerializeField] 
    GameObject[] m_menuPanels = default;

    [SerializeField]
    GameObject m_rootMenuPanel = default;

    [SerializeField]
    GameObject m_confirmPanel = default;

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
                m_rootMenuPanel.SetActive(true);
                Cursor.visible = true;
                Time.timeScale = 0f;
                ActiveMenu(1);
                state = MenuState.Open;
            }
            else if (state != MenuState.Close)  //メニューを閉じる
            {
                m_rootMenuPanel.SetActive(false);
                Cursor.visible = false;
                Time.timeScale = 1f;
                ActiveMenu(0);
                state = MenuState.Close;
            }
        }
    }

    /// <summary>
    /// 指定のメニューを開く
    /// </summary>
    /// <param name="index"> 開くメニュー </param>
    public void ActiveMenu(int index)
    {
        if (index == 5)
        {
            m_confirmPanel.SetActive(true);
            return;
        }

        if (m_confirmPanel.activeSelf)
        {
            m_confirmPanel.SetActive(false);
        }

        if (index == 0)
        {
            for (int i = 0; i < m_menuPanels.Length; i++)
            {
                m_menuPanels[i].SetActive(false);
            }
            return;
        }

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

    public void LoadBaseScene(string baseName)
    {
        LoadSceneManager.Instance.AnyLoadScene(baseName);
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
