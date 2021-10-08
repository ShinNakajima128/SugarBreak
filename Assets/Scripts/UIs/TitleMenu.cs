using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum TitleMenuState
{
    Begin,
    MainMenu,
    Option,
    Audio
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

    void Start()
    {
        VolumeSetup();
        isStarted = false;
        SwitchingMenu(0);
        m_loadingAnim.SetActive(false);
        m_mainMenuBG.SetActive(false);
        m_confirmPanel.SetActive(false);
    }

    void Update()
    {
        if (!isStarted)
        {
            if (Input.anyKeyDown)
            {
                TextAnimation.Instance.FinishAnim();
                SoundManager.Instance.PlaySeByName("Select");
                titleState = TitleMenuState.MainMenu;
                isChanged = false;
                isStarted = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SoundManager.Instance.PlaySeByName("Cancel");
                titleState = TitleMenuState.Begin;
                isChanged = false;
                isStarted = false;
                StartCoroutine(Restart());
            }
        }

        switch (titleState)
        {
            case TitleMenuState.Begin:
                if (!isChanged)
                {
                    SwitchingMenu(0);
                    isChanged = true;
                }
                break;
            case TitleMenuState.MainMenu:
                if (!isChanged)
                {
                    SwitchingMenu(1);
                    isChanged = true;
                    TextChange();
                }
                break;
            case TitleMenuState.Option:
                if (!isChanged)
                {
                    SwitchingMenu(2);
                    isChanged = true;
                }
                break;
            case TitleMenuState.Audio:
                if (!isChanged)
                {
                    SwitchingMenu(3);
                    isChanged = true;
                }
                break;
        }
    }

    void VolumeSetup()
    {
        if (m_masterVolume) m_masterVolume.value = SoundManager.Instance.GetMasterVolume;

        if (m_bgmVolume) m_bgmVolume.value = SoundManager.Instance.GetBgmVolume;

        if (m_seVolume) m_seVolume.value = SoundManager.Instance.GetSeVolume;

        if (m_voiceVolume) m_voiceVolume.value = SoundManager.Instance.GetVoiceVolume;
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
        SoundManager.Instance.PlaySeByName("Load");
        if (!GameManager.Instance.GameStarted)
        {
            ///あらすじのSceneができたらここの引数を書き換える
            LoadSceneManager.Instance.AnyLoadScene("Base");
            GameManager.Instance.GameStarted = true;
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
        SoundManager.Instance.PlaySeByName("Select");
        titleState = TitleMenuState.MainMenu;
        isChanged = false;
    }

    /// <summary>
    /// オプションを表示する
    /// </summary>
    public void OptionSelect()
    {
        SoundManager.Instance.PlaySeByName("Select");
        titleState = TitleMenuState.Option;
        isChanged = false;
    }

    public void AudioSelect()
    {
        SoundManager.Instance.PlaySeByName("Select");
        titleState = TitleMenuState.Audio;
        isChanged = false;
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
        SoundManager.Instance.MasterVolChange(m_masterVolume.value);
    }

    public void BgmChange()
    {
        SoundManager.Instance.BgmVolChange(m_bgmVolume.value);
    }

    public void SeChange()
    {
        SoundManager.Instance.SeVolChange(m_seVolume.value);
    }

    public void VoiceChange()
    {
        SoundManager.Instance.VoiceVolChange(m_voiceVolume.value);
    }

    void TextChange()
    {
        if (!GameManager.Instance.GameStarted)
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

    /// <summary>
    /// 各フェード時に呼び出すコルーチン
    /// </summary>
    /// <param name="mask"> フェードに使うマスク </param>
    /// <returns></returns>
    IEnumerator StartWait(Texture mask)
    {
        yield return new WaitForSeconds(0.5f);

        m_loadingAnim.SetActive(false);
        m_titleMenuPanel.SetActive(true);
        fadeImage.UpdateMaskTexture(mask);
        fade.FadeOut(1.0f);

        yield return new WaitForSeconds(1.0f);
    }
}
