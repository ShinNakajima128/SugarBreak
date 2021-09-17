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
    public static MenuManager Instance;

    [Header("メニューのパネル")]
    [SerializeField] 
    GameObject[] m_menuPanels = default;

    [Header("メニューの根本のパネル")]
    [SerializeField]
    GameObject m_rootMenuPanel = default;

    [Header("ゲーム終了の確認画面")]
    [SerializeField]
    GameObject m_confirmPanel = default;

    [Header("HUDのパネル")]
    [SerializeField]
    GameObject m_hudPanel = default;

    Dictionary<MenuState, int> menuIndex = new Dictionary<MenuState, int>();
    MenuState state = MenuState.Close;

    /// <summary> メニューが開ける状態かどうかのフラグ </summary>
    public bool WhetherOpenMenu { get; set; } = false;

    public MenuState MenuStates => state;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
        if (WhetherOpenMenu)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !Map.Instance.PauseFlag)
            {
                if (state == MenuState.Close)   //メニューを開く
                {
                    m_rootMenuPanel.SetActive(true);
                    m_hudPanel.SetActive(false);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    Time.timeScale = 0f;
                    ActiveMenu(0);
                    state = MenuState.Open;
                    PlayerStatesManager.Instance.OffOperation();
                }
                else if (state != MenuState.Close)  //メニューを閉じる
                {
                    Time.timeScale = 1f;
                    m_rootMenuPanel.SetActive(false);
                    m_hudPanel.SetActive(true);
                    ActiveMenu(6);
                    state = MenuState.Close;
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    PlayerStatesManager.Instance.OnOperation();
                }
            }
        }  
    }

    /// <summary>
    /// 指定のメニューを開く
    /// </summary>
    /// <param name="index"> 開くメニュー </param>
    public void ActiveMenu(int index)
    {
        if (index == 6)
        {
            for (int i = 0; i < m_menuPanels.Length; i++)
            {
                m_menuPanels[i].SetActive(false);
            }
            return;
        }

        if (index == 5)
        {
            m_confirmPanel.SetActive(true);
            return;
        }

        if (m_confirmPanel.activeSelf)
        {
            m_confirmPanel.SetActive(false);
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
        Time.timeScale = 1f;
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
