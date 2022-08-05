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

        _titleMenuCtrl.OnMenuPanel(MenuType.Start);

        this.UpdateAsObservable()
            .Where(_ => Input.anyKeyDown && _titleMenuCtrl.CurrentMenuType == MenuType.Start)
            .Subscribe(_ => 
            {
                OnMainMenuPanel();
                AudioManager.PlaySE(SEType.UI_Select);
            });

        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Escape) && _titleMenuCtrl.CurrentMenuType != MenuType.Start)
            .Subscribe(_ =>
            {
                _titleMenuCtrl.OnMenuPanel(MenuType.Start);
                AudioManager.PlaySE(SEType.UI_Cancel);
            });
    }

    #region button method
    /// <summary>
    /// ゲームを終了する
    /// </summary>
    public void OnGameExit()
    {
        LoadSceneManager.Instance.QuitGame();
    }
    /// <summary>
    /// ゲーム終了をキャンセルし、メインメニューへ戻る
    /// </summary>
    public void OnCancel()
    {
        _titleMenuCtrl.OnMenuPanel(MenuType.MainMenu);
    }
    #endregion
    void OnMainMenuPanel()
    {
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
