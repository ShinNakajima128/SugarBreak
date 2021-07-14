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
    [SerializeField] SoundManager soundManager = default;
    [SerializeField] float m_loadTime = 1.0f;
    [SerializeField] Texture[] m_masks = default;
    [SerializeField] TitleMenuState titleState = TitleMenuState.Begin;
    static bool isStarted = false;
    bool isChanged = false;

    void Start()
    {

    }

    void Update()
    {
        if (!isStarted)
        {
            if (Input.anyKeyDown)
            {
                fadeImage.UpdateMaskTexture(m_masks[0]);
                fade.FadeIn(1.0f,() =>
                StartCoroutine(StartWait()));
                isStarted = false;
            }
        }

        switch (titleState)
        {
            case TitleMenuState.Begin:
                if (!isChanged)
                {
                    //m_titleMenuPanel.SetActive(true);
                    isChanged = true;
                }
                break;
        }
    }

    IEnumerator StartWait()
    {
        yield return new WaitForSeconds(0.5f);

        m_titleMenuPanel.SetActive(true);
        fadeImage.UpdateMaskTexture(m_masks[1]);
        fade.FadeOut(1.0f);
    }
}
