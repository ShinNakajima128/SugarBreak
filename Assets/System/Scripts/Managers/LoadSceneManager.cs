using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    [SerializeField] Fade fade = default;
    [SerializeField] SoundManager soundManager = default;
    [SerializeField] float m_loadTime = 1.0f;
    [SerializeField] GameObject m_loadAnim = default;
    [SerializeField] FadeImage fadeImage = default;
    [SerializeField] Texture[] m_masks = default;
    static float LoadTime;

    void Start()
    {
        m_loadAnim.SetActive(false);

        fade.FadeOut(1.0f, () => TitleMenu.isInputtable = true);
        LoadTime = m_loadTime;
    }

    public void AnyLoadScene(string name)
    {
        fadeImage.UpdateMaskTexture(m_masks[2]);
        fade.FadeIn(1.0f, () =>
        {
            StartCoroutine(Load(name, 3.0f));
        });
    }

    public void QuitGame()
    {
        fadeImage.UpdateMaskTexture(m_masks[0]);
        fade.FadeIn(1.0f);
        StartCoroutine(DelayQuit());
    }

    IEnumerator Load(string name, float loadTime)
    {
        yield return new WaitForSeconds(1.0f);

        m_loadAnim.SetActive(true);

        yield return new WaitForSeconds(loadTime);

        SceneManager.LoadScene(name);
    }
    IEnumerator DelayFade()
    {
        yield return new WaitForSeconds(m_loadTime);

        StartCoroutine(PlaySound());
        fade.FadeOut(1.0f);
    }
    IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(0.1f);

        soundManager.PlaySeByName("Transition");
    }

    IEnumerator DelayQuit()
    {
        yield return new WaitForSeconds(m_loadTime);

        Application.Quit();
    }
}
