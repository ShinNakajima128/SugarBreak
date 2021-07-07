using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : SingletonMonoBehaviour<LoadSceneManager>
{
    [SerializeField] Fade fade = default;
    [SerializeField] SoundManager soundManager = default;
    [SerializeField] float m_loadTime = 1.0f;
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

    public void AnyLoadScene(string name)
    {
        StartCoroutine(Load(name, LoadTime));
    }

    void Start()
    {
        StartCoroutine(PlaySound());
        fade.FadeOut(1.0f);
        LoadTime = m_loadTime;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            soundManager.PlaySeByName("Exprosion");
            fade.FadeIn(0.5f, () =>
            StartCoroutine(DelayFade())
            );
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(PlaySound());
            fade.FadeOut(1.0f);
        }
    }

    IEnumerator Load(string name, float loadTime)
    {
        yield return new WaitForSeconds(loadTime);

        SceneManager.LoadScene(name);
    }
    IEnumerator DelayFade()
    {
        yield return new WaitForSeconds(0.5f);

        StartCoroutine(PlaySound());
        fade.FadeOut(1.0f);
    }
    IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(0.1f);

        soundManager.PlaySeByName("Transition");
    }
}
