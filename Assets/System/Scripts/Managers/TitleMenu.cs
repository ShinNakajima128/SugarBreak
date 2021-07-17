using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] GameObject m_titleMenuPanel = default;
    /// <summary>  </summary>
    [SerializeField] Fade fade = default;
    /// <summary>  </summary>
    [SerializeField] FadeImage fadeImage = default;
    /// <summary>  </summary>
    [SerializeField] float m_loadTime = 1.0f;
    /// <summary>  </summary>
    [SerializeField] Texture[] m_masks = default;
    /// <summary>  </summary>
    [SerializeField] TitleMenuState titleState = TitleMenuState.Begin;
    /// <summary>  </summary>
    [SerializeField] GameObject m_loadingAnim = default;
    /// <summary>  </summary>
    [SerializeField] GameObject m_mainMenuBG = default;
    /// <summary>  </summary>
    [SerializeField] GameObject m_mainMenuList = default;
    /// <summary>  </summary>
    [SerializeField] GameObject m_confirmPanel = default;
    /// <summary>  </summary>
    [SerializeField] GameObject[] m_menuPanels = default;

    static bool isStarted = false;
    public static bool isInputtable = true;
    bool isChanged = false;
    SoundManager soundManager;

    private void Awake()
    {
        isInputtable = false;
    }
    void Start()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        SwitchingMenu(0);
        m_loadingAnim.SetActive(false);
        m_mainMenuBG.SetActive(false);
        m_confirmPanel.SetActive(false);
    }

    void Update()
    {
        if (!isStarted)
        {
            if (Input.anyKeyDown && isInputtable)
            {
                isInputtable = false;
                soundManager.PlaySeByName("Transition2");
                fadeImage.UpdateMaskTexture(m_masks[0]);
                fade.FadeIn(1.0f, () =>
                 {
                     titleState = TitleMenuState.MainMenu;
                     isChanged = false;
                     m_mainMenuBG.SetActive(true);
                     StartCoroutine(StartWait(m_masks[1]));
                 });
                isStarted = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape) && isInputtable)
            {
                isInputtable = false;
                soundManager.PlaySeByName("Transition2");
                fadeImage.UpdateMaskTexture(m_masks[1]);
                fade.FadeIn(1.0f, () =>
                {
                    titleState = TitleMenuState.Begin;
                    isChanged = false;
                    m_mainMenuBG.SetActive(false);
                    StartCoroutine(StartWait(m_masks[0]));
                });
                isStarted = false;
            }
        }

        switch (titleState)
        {
            case TitleMenuState.Begin:
                if (!isChanged)
                {
                    SwitchingMenu(0);
                    isChanged = true;
                    Debug.Log("タイトル画面");
                }
                break;
            case TitleMenuState.MainMenu:
                if (!isChanged)
                {
                    SwitchingMenu(1);
                    isChanged = true;
                    Debug.Log("メインメニュー");
                }
                break;
            case TitleMenuState.Option:
                if (!isChanged)
                {
                    SwitchingMenu(2);
                    isChanged = true;
                    Debug.Log("オプション");
                }
                break;
        }
    }

    void SwitchingMenu(int menuNum)
    {
        for (int i = 0; i < m_menuPanels.Length; i++)
        {
            if (i == menuNum)
            {
                m_menuPanels[i].SetActive(true);
            }
            else
            {
                m_menuPanels[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// メインメニューを表示する
    /// </summary>
    public void MainMenuSelect()
    {
        titleState = TitleMenuState.MainMenu;
        isChanged = false;
    }

    /// <summary>
    /// オプションを表示する
    /// </summary>
    public void OptionSelect()
    {
        titleState = TitleMenuState.Option;
        isChanged = false;
    }

    /// <summary>
    /// ゲーム終了前の確認画面を表示する
    /// </summary>
    public void OnConfirmPanel()
    {
        m_confirmPanel.SetActive(true);
        m_mainMenuList.SetActive(false);
    }

    /// <summary>
    /// 確認画面を閉じる
    /// </summary>
    public void OffConfirmPanel()
    {
        m_confirmPanel.SetActive(false);
        m_mainMenuList.SetActive(true);
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

        isInputtable = true;
    }
}
