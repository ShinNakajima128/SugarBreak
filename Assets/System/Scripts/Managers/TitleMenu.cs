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

    static bool isStarted = false;
    bool isChanged = false;
    SoundManager soundManager;

    void Start()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        m_loadingAnim.SetActive(false);
    }

    void Update()
    {
        if (!isStarted)
        {
            if (Input.anyKeyDown)
            {
                soundManager.PlaySeByName("Transition");
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
                    isChanged = true;
                }
                break;
        }
    }

    IEnumerator StartWait()
    {
        yield return new WaitForSeconds(0.5f);

        m_loadingAnim.SetActive(true);

        yield return new WaitForSeconds(3.0f);

        m_loadingAnim.SetActive(false);
        m_titleMenuPanel.SetActive(true);
        fadeImage.UpdateMaskTexture(m_masks[1]);
        fade.FadeOut(1.0f);
    }
}
