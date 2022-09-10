using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SugarBreak;

public class TitleMenuController : MonoBehaviour
{
    [SerializeField]
    List<Menu> _titleMenus = new List<Menu>();

    MenuType _currentMenuType;

    bool _isLoading = false;

    public MenuType CurrentMenuType => _currentMenuType;

    /// <summary>
    /// メニューのパネルを表示する
    /// </summary>
    /// <param name="type"> パネルの種類 </param>
    public void OnMenuPanel(MenuType type)
    {
        //一度パネルを全てOFFにする
        foreach (var m in _titleMenus)
        {
            m.MenuPanel.SetActive(false);
        }

        var panel = _titleMenus.FirstOrDefault(m => m.MenuType == type);

        panel.MenuPanel.SetActive(true);
        _currentMenuType = type;
    }

    #region button method
    /// <summary>
    /// 指定したボタンの処理を実行する
    /// </summary>
    /// <param name="type"> ボタンの種類 </param>
    public void OnButtonClick(int type)
    {
        var t = (ButtonType)type;

        switch (t)
        {
            case ButtonType.NewGame:
                if (!_isLoading)
                {
                    LoadSceneManager.Instance.AnyLoadScene("Base");
                    AudioManager.PlaySE(SEType.UI_Load);
                }
                _isLoading = true;
                break;
            case ButtonType.Continue:
                if (!_isLoading)
                {
                    LoadSceneManager.Instance.AnyLoadScene("Base");
                    AudioManager.PlaySE(SEType.UI_Load);
                }
                _isLoading = true;
                break;
            case ButtonType.Crefit:
                OnMenuPanel(MenuType.Crefit);
                AudioManager.PlaySE(SEType.UI_ButtonSelect);
                break;
            case ButtonType.Explore:
                if (!_isLoading)
                {
                    LoadSceneManager.Instance.AnyLoadScene("Base_TGS");
                    AudioManager.PlaySE(SEType.UI_Load);
                }
                _isLoading = true;    
                break;
            case ButtonType.BossBattle:
                LoadSceneManager.Instance.AnyLoadScene("BossBattle");
                AudioManager.PlaySE(SEType.UI_Load);
                break;
            case ButtonType.GameEnd:
                OnMenuPanel(MenuType.GameEnd);
                AudioManager.PlaySE(SEType.UI_ButtonSelect);
                break;
            case ButtonType.Option:
                OnMenuPanel(MenuType.Option);
                AudioManager.PlaySE(SEType.UI_ButtonSelect);
                MenuCursor.OnCursor();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnMainMenuPanel()
    {
        switch (TitleManager.Instance.TitleMode)
        {
            case TitleMode.Normal:
                OnMenuPanel(MenuType.MainMenu);
                break;
            case TitleMode.TGS:
                OnMenuPanel(MenuType.TGSMenu);
                break;
        }
        MenuCursor.OffCursor();
    }
    public void ClickSE()
    {
        AudioManager.PlaySE(SEType.UI_ButtonSelect);
    }
    #endregion
}

[Serializable]
public class Menu
{
    public GameObject MenuPanel;
    public MenuType MenuType;
    public TitleMenuButton[] MenuButtons;
}

public enum MenuType
{
    Start,
    MainMenu,
    Continue,
    Crefit,
    Explore,
    BossBattle,
    GameEnd,
    TGSMenu,
    Option
}
