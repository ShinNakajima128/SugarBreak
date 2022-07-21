using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public enum TitleMode
{
    /// <summary> 通常のタイトル画面 </summary>
    Normal,
    /// <summary> TGS用のタイトル画面 </summary>
    TGS
}
public class TitleManager : MonoBehaviour
{
    [Tooltip("タイトルのモード")]
    [SerializeField]
    TitleMode _titleMode = default;

    [SerializeField]
    TitleMenuController _titleMenuCtrl = default;

    void Start()
    {
        AudioManager.PlayBGM(BGMType.Title);
        switch (_titleMode)
        {
            case TitleMode.Normal:
                _titleMenuCtrl.OnMenuPanel(MenuType.MainMenu);
                break;
            case TitleMode.TGS:
                _titleMenuCtrl.OnMenuPanel(MenuType.TGSMenu);
                break;
        }
    }
}
