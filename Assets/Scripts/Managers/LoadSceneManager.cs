using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Scene遷移を管理するクラス
/// </summary>
public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager Instance { get; private set; }
    /// <summary> フェードさせるパネル </summary>
    [SerializeField] 
    Fade fade = default;

    /// <summary> ロードにかける時間 </summary>
    [SerializeField]
    float m_fadeTime = 1.0f;

    /// <summary> ロードにかける時間 </summary>
    [SerializeField] 
    float m_loadTime = 1.0f;

    /// <summary> ロード画面中のアニメーション </summary>
    [SerializeField] 
    GameObject m_loadAnim = default;

    /// <summary> フェードのマスクを管理するオブジェクト </summary>
    [SerializeField] 
    FadeImage fadeImage = default;

    /// <summary> フェードさせる歳のマスク </summary>
    [SerializeField] 
    Texture[] m_masks = default;

    /// <summary> ロード中のフィルター </summary>
    [SerializeField]
    GameObject m_filter = default;

    public Texture[] Masks { get => m_masks; }

    public GameObject LoadAnim { get => m_loadAnim; set => m_loadAnim = value; }
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        m_loadAnim.SetActive(false);

        fade.FadeOut(m_fadeTime);
    }

    public void FadeIn(Texture mask = null, Action callback = null)
    {
        if (mask != null)
        {
            fadeImage.UpdateMaskTexture(mask);
        }
        else
        {
            fadeImage.UpdateMaskTexture(m_masks[2]);
        }

        fade.FadeIn(m_fadeTime, () => 
        {
            callback?.Invoke();
        });
    }

    public void FadeOut(Texture mask = null, Action callback = null)
    {
        if (mask != null)
        {
            fadeImage.UpdateMaskTexture(mask);
        }
        else
        {
            fadeImage.UpdateMaskTexture(m_masks[1]);
        }

        fade.FadeOut(m_fadeTime, () => 
        {
            callback?.Invoke();
        });
    }
    /// <summary>
    /// 指定したSceneに遷移する
    /// </summary>
    /// <param name="name"> 遷移先のSceneの名前 </param>
    public void AnyLoadScene(string name)
    {
        fadeImage.UpdateMaskTexture(m_masks[2]);
        m_filter.SetActive(true);
        fade.FadeIn(m_fadeTime, () =>
        {
            StartCoroutine(Load(name, 2.0f));
        });
    }

    /// <summary>
    /// ゲームを終了する
    /// </summary>
    public void QuitGame()
    {
        fadeImage.UpdateMaskTexture(m_masks[0]);
        fade.FadeIn(0.7f);
        StartCoroutine(DelayQuit());
    }

    /// <summary>
    /// ロード時のコルーチン
    /// </summary>
    /// <param name="name"> Sceneの名前 </param>
    /// <param name="loadTime"> ロードにかける時間 </param>
    /// <returns></returns>
    IEnumerator Load(string name, float loadTime)
    {
        yield return new WaitForSeconds(0.2f);

        m_loadAnim.SetActive(true);

        yield return new WaitForSeconds(loadTime);

        m_filter.SetActive(false);
        SceneManager.LoadScene(name);
    }
    
    /// <summary>
    /// フェード後にゲームを終了するためのコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator DelayQuit()
    {
        yield return new WaitForSeconds(m_loadTime);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
