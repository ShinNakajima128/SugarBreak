using System.Collections;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum TitleMenuState
{
    Begin,
    MainMenu,
    Option,
    Graphic,
    Audio,
    Extra,
    Config,
    MiniGame,
    Museum,
}

public class TitleMenu : MonoBehaviour
{
    /// <summary> タイトル画面のPanel </summary>
    [SerializeField] 
    GameObject m_titleMenuPanel = default;

    /// <summary>  </summary>
    [SerializeField] 
    Fade fade = default;

    /// <summary>  </summary>
    [SerializeField] 
    FadeImage fadeImage = default;

    /// <summary>  </summary>
    [SerializeField] 
    float m_loadTime = 1.0f;

    /// <summary>  </summary>
    [SerializeField] 
    Texture[] m_masks = default;

    /// <summary>  </summary>
    [SerializeField] 
    TitleMenuState titleState = TitleMenuState.Begin;

    /// <summary>  </summary>
    [SerializeField] 
    GameObject m_loadingAnim = default;

    /// <summary>  </summary>
    [SerializeField] 
    GameObject m_mainMenuBG = default;

    /// <summary>  </summary>
    [SerializeField] 
    GameObject m_mainMenuList = default;

    /// <summary>  </summary>
    [SerializeField]
    GameObject m_confirmPanel = default;

    /// <summary>  </summary>
    [SerializeField] 
    GameObject[] m_menuPanels = default;

    [SerializeField]
    Text m_playButtonText = default;

    [SerializeField]
    Button[] m_menuFirstButtons = default;

    [SerializeField]
    Slider m_masterVolume = default;

    [SerializeField]
    Slider m_bgmVolume = default;

    [SerializeField]
    Slider m_seVolume = default;

    [SerializeField]
    Slider m_voiceVolume = default;

    bool isStarted = false;
    bool isChanged = false;

    IEnumerator Start()
    {
        isStarted = false;
        SwitchingMenu(0);
        m_loadingAnim.SetActive(false);
        m_mainMenuBG.SetActive(false);
        m_confirmPanel.SetActive(false);

        SaveManager.Load();
        yield return new WaitForSeconds(0.5f);
        AudioManager.SetVolume(DataManager.Instance.GetOptionData.SoundOptionData);
    }

    void Update()
    {
        if (!isStarted)
        {
            if (Input.anyKeyDown)
            {
                TextAnimation.Instance.FinishAnim();
                AudioManager.PlaySE(SEType.UI_Select);
                SwitchTitleState(TitleMenuState.MainMenu);
                isStarted = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                AudioManager.PlaySE(SEType.UI_Cancel);
                SwitchTitleState(TitleMenuState.Begin);
                isStarted = false;
                StartCoroutine(Restart());
            }
        }
    }

    void SwitchTitleState(TitleMenuState state)
    {
        titleState = state;

        switch (titleState)
        {
            case TitleMenuState.Begin:
                SwitchingMenu(0);
                break;
            case TitleMenuState.MainMenu:
                SwitchingMenu(1);
                TextChange();
                break;
            case TitleMenuState.Option:
                SwitchingMenu(2);
                break;
            case TitleMenuState.Audio:
                SwitchingMenu(3);
                break;
            case TitleMenuState.Extra:
                SwitchingMenu(4);
                break;
        }
    }

    public void OnChangeColor(Text text)
    {
        text.color = new Color(0.99f, 1.0f, 0.71f);
    }

    public void OffChangeColor(Text text)
    {
        text.color = Color.white;
    }

    public void PlayGame()
    {
        AudioManager.PlaySE(SEType.UI_Load);
                
        //初プレイ時の場合
        if (!DataManager.Instance.GetPlayerData.IsFirstPlay)
        {
            ///あらすじのSceneができたらここの引数を書き換える
            LoadSceneManager.Instance.AnyLoadScene("Base");
            DataManager.Instance.GetPlayerData.IsFirstPlay = true;
        }
        else
        {
            LoadSceneManager.Instance.AnyLoadScene("Base");
        }
    }

    /// <summary>
    /// メインメニューを表示する
    /// </summary>
    public void MainMenuSelect()
    {
        AudioManager.PlaySE(SEType.UI_Select);
        SwitchTitleState(TitleMenuState.MainMenu);
    }

    /// <summary>
    /// オプションを表示する
    /// </summary>
    public void OptionSelect()
    {
        AudioManager.PlaySE(SEType.UI_Select);
        SwitchTitleState(TitleMenuState.Option);
    }

    public void AudioSelect()
    {
        AudioManager.PlaySE(SEType.UI_Select);
        SwitchTitleState(TitleMenuState.Audio);
    }

    public void ExtraSelect()
    {
        AudioManager.PlaySE(SEType.UI_Select);
        SwitchTitleState(TitleMenuState.Extra);
    }

    /// <summary>
    /// ゲーム終了前の確認画面を表示する
    /// </summary>
    public void OnConfirmPanel()
    {
        m_confirmPanel.SetActive(true);
        m_mainMenuList.SetActive(false);
        m_menuFirstButtons[3].Select();
    }

    /// <summary>
    /// 確認画面を閉じる
    /// </summary>
    public void OffConfirmPanel()
    {
        m_confirmPanel.SetActive(false);
        m_mainMenuList.SetActive(true);
        m_menuFirstButtons[0].Select();
    }

    public void MasterChange()
    {
    }

    public void BgmChange()
    {
    }

    public void SeChange()
    {
    }

    public void VoiceChange()
    {
    }

    void TextChange()
    {
        if (!DataManager.Instance.GetPlayerData.IsFirstPlay)
        {
            m_playButtonText.text = "はじめから";
        }
        else
        {
            m_playButtonText.text = "つづきから";
        }
    }

    void SwitchingMenu(int menuNum)
    {
        for (int i = 0; i < m_menuPanels.Length; i++)
        {
            if (i == menuNum)
            {
                m_menuPanels[i].SetActive(true);
                if (i != 0)
                {
                    m_menuFirstButtons[i - 1].Select();
                    Debug.Log(m_menuFirstButtons[i - 1]);
                }
            }
            else
            {
                m_menuPanels[i].SetActive(false);
            }
        }
    }

    IEnumerator Restart()
    {
        yield return null;

        TextAnimation.Instance.OnAnim();
    }
}
