﻿using System.Collections;
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

    public static TitleManager Instance { get; private set; }

    public TitleMode TitleMode => _titleMode;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _titleMenuCtrl.OnMenuPanel(MenuType.Start);

        this.UpdateAsObservable()
            .Where(_ => Input.anyKeyDown && _titleMenuCtrl.CurrentMenuType == MenuType.Start)
            .Subscribe(_ => 
            {
                _titleMenuCtrl.OnMainMenuPanel(() => 
                {
                    ButtonUIController.Instance.OnCurrentPanelFirstButton(1);
                    AudioManager.PlaySE(SEType.UI_Select);
                    Debug.Log("メニューON");
                });
            });

        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Jump"))
            .Where(_ => _titleMenuCtrl.CurrentMenuType == MenuType.MainMenu || _titleMenuCtrl.CurrentMenuType == MenuType.TGSMenu)
            .Subscribe(_ =>
            {
                if (!LoadSceneManager.Instance.IsLoading)
                {
                    _titleMenuCtrl.OnMenuPanel(MenuType.Start);
                    AudioManager.PlaySE(SEType.UI_Cancel);
                    Debug.Log("メニューOFF");
                }     
            });
        SaveManager.Load(() => 
        {
            AudioManager.SetVolume(DataManager.Instance.GetOptionData.SoundOptionData);
            AudioManager.PlayBGM(BGMType.Title);
        });
    }

    #region button method
    public void OnSelectSE()
    {
        AudioManager.PlaySE(SEType.UI_CursolMove);
    }
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
        _titleMenuCtrl.OnMainMenuPanel();
        AudioManager.PlaySE(SEType.UI_Cancel);
    }
    #endregion
}
