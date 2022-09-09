using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SugarBreak;

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
    public static MenuManager Instance { get; private set; }

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

    [Header("メニューを開いた時に選択するボタン")]
    [SerializeField]
    Button m_menuFirstButton = default;

    [Header("オプション開いた時に選択するボタン")]
    [SerializeField]
    Button m_optionFirstButton = default;

    [Header("拠点に戻るボタンを押した時に選択するボタン")]
    [SerializeField]
    Button m_exitFirstButton = default;

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
        if (WhetherOpenMenu && !PlayerStatesManager.Instance.IsDying)    //メニューが開ける状態だったら
        {
            if (!Map.Instance.PauseFlag && !GameManager.Instance.IsPlayingMovie)    //マップを開いていなかったらメニューを開ける
            {
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 7"))      //Escキーかゲームパッドのスタートボタンが押されたら
                {
                    if (state == MenuState.Close)   //メニューを開く
                    {
                        OnMenu();
                        ActiveMenu(0);
                    }
                    else if (state != MenuState.Close)  //メニューを閉じる
                    {
                        //CloseMenu();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Joystick1Button8))
                {
                    if (state == MenuState.Close)   //チュートリアル画面を開く
                    {
                        OnMenu();
                        ActiveMenu(4);
                    }
                }
            }          
        }
    }

    /// <summary>
    /// メニューを開いた時にボタンを選択
    /// </summary>
    public void MenuSelectButton() => m_menuFirstButton.Select();
   
    /// <summary>
    /// 拠点に戻る確認画面を開いた時にボタンを選択する
    /// </summary>
    public void ExitSelectButton() => m_exitFirstButton.Select();


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

        if (index == 4)
        {
            switch (WeaponListControl.Instance.CurrentEquipWeapon)
            {
                case WeaponListTypes.Equip1:
                    var currentEquip1 = WeaponListControl.Instance.CurrentWeapon1Data.WeaponType;
                    TutorialController.Instance.OnPanel(currentEquip1);
                    break;
                case WeaponListTypes.Equip2:
                    var currentEquip2 = WeaponListControl.Instance.CurrentWeapon2Data.WeaponType;
                    TutorialController.Instance.OnPanel(currentEquip2);
                    break;
                case WeaponListTypes.Equip3:
                    var currentEquip3 = WeaponListControl.Instance.CurrentWeapon3Data.WeaponType;
                    TutorialController.Instance.OnPanel(currentEquip3);
                    break;
                case WeaponListTypes.MainWeapon:
                    var mainWeapon = WeaponListControl.Instance.MainWeaponData.WeaponType;
                    TutorialController.Instance.OnPanel(mainWeapon);
                    break;
                default:
                    break;
            }
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
        AudioManager.PlaySE(SEType.UI_Load);
    }

    public void CloseMenu()
    {
        Time.timeScale = 1f;
        m_rootMenuPanel.SetActive(false);
        m_hudPanel.SetActive(true);
        ActiveMenu(6);
        state = MenuState.Close;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        PlayerStatesManager.Instance.OnOperation();
        
        EventManager.OnEvent(Events.OnHUD);
        ButtonUIController.Instance.IsActived = false;
    }

    public void PlayMenuSelectSE()
    {
        AudioManager.PlaySE(SEType.UI_CursolMove);
    }

    public void OnCursor()
    {
        MenuCursor.OnCursor();
    }

    public void OffCursor()
    {
        MenuCursor.OffCursor();
    }

    void OnMenu()
    {
        AudioManager.PlaySE(SEType.UI_Select);
        m_rootMenuPanel.SetActive(true);
        m_hudPanel.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        ActiveMenu(0);
        state = MenuState.Open;
        PlayerStatesManager.Instance.OffOperation();
        EventManager.OnEvent(Events.OffHUD);

        ButtonUIController.Instance.IsActived = true;
        ButtonUIController.Instance.OnCurrentPanelFirstButton(0);
    }

    void OnTutorialPanel()
    {

    }
}
