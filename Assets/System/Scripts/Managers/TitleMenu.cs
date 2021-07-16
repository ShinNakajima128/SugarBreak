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
    [SerializeField] GameObject m_titleMenuPanel = default;
    [SerializeField] Fade fade = default;
    [SerializeField] FadeImage fadeImage = default;
    [SerializeField] float m_loadTime = 1.0f;
    [SerializeField] Texture[] m_masks = default;
    [SerializeField] TitleMenuState titleState = TitleMenuState.Begin;
    [SerializeField] GameObject m_loadingAnim = default;
    [SerializeField] GameObject m_mainMenuBG = default;
    [SerializeField] GameObject m_confirmPanel = default;
    [SerializeField] GameObject[] m_menuPanels = default;

    static bool isStarted = false;
    bool isChanged = false;
    SoundManager soundManager;

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
            if (Input.anyKeyDown)
            {
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
            if (Input.GetKeyDown(KeyCode.Escape))
            {
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

    public void OnConfirmPanel()
    {
        m_confirmPanel.SetActive(true);
    }

    public void OffConfirmPanel()
    {
        m_confirmPanel.SetActive(false);
    }

    IEnumerator StartWait(Texture mask)
    {
        yield return new WaitForSeconds(0.5f);

        m_loadingAnim.SetActive(true);

        yield return new WaitForSeconds(3.0f);

        m_loadingAnim.SetActive(false);
        m_titleMenuPanel.SetActive(true);
        fadeImage.UpdateMaskTexture(mask);
        fade.FadeOut(1.0f);
    }
}
