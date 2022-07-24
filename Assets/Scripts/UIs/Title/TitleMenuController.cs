using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenuController : MonoBehaviour
{
    [SerializeField]
    List<Menu> _titleMenus = new List<Menu>();

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
    }

    #region button method
    public void OnButtonClick(int type)
    {
        var t = (ButtonType)type;

        switch (t)
        {
            case ButtonType.NewGame:
                break;
            case ButtonType.Continue:
                break;
            case ButtonType.Crefit:
                break;
            case ButtonType.Explore:
                break;
            case ButtonType.BossBattle:
                break;
            case ButtonType.GameEnd:
                break;
            default:
                break;
        }
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
    TGSMenu
}
