using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : SingletonMonoBehaviour<LoadSceneManager>
{
    [SerializeField] Fade fade = default;
    [SerializeField] SoundManager soundManager = default;
    [SerializeField] float m_loadTime = 1.0f;
    [SerializeField] FadeImage fadeImage = default;
    [SerializeField] Texture m_masks = default;
    static float LoadTime;
    
    void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        fade.FadeOut(1.0f);
        LoadTime = m_loadTime;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    soundManager.PlaySeByName("Exprosion");
        //    fade.FadeIn(1.0f, () =>
        //    StartCoroutine(DelayFade())
        //    );
        //}

        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    StartCoroutine(PlaySound());
        //    fade.FadeOut(1.0f);
        //}

        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    StartCoroutine(PlaySound());
        //    fadeImage.UpdateMaskTexture(m_masks);
        //    fade.FadeIn(1.0f);
        //}
    }

    public void AnyLoadScene(string name)
    {
        StartCoroutine(Load(name, LoadTime));
    }

    public void QuitGame()
    {
        fadeImage.UpdateMaskTexture(m_masks);
        fade.FadeIn(1.0f);
        StartCoroutine(DelayQuit());
    }

    IEnumerator Load(string name, float loadTime)
    {
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
