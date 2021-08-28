﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    /// <summary> フェードさせるパネル </summary>
    [SerializeField] Fade fade = default;
    /// <summary> ロードにかける時間 </summary>
    [SerializeField] float m_loadTime = 1.0f;
    /// <summary> ロード画面中のアニメーション </summary>
    [SerializeField] GameObject m_loadAnim = default;
    /// <summary> フェードのマスクを管理するオブジェクト </summary>
    [SerializeField] FadeImage fadeImage = default;
    /// <summary> フェードさせる歳のマスク </summary>
    [SerializeField] Texture[] m_masks = default;
    /// <summary> サウンドマネージャー </summary>
    SoundManager soundManager;

    void Start()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        m_loadAnim.SetActive(false);

        fade.FadeOut(1.0f, () => TitleMenu.isInputtable = true);
    }

    /// <summary>
    /// 指定したSceneに遷移する
    /// </summary>
    /// <param name="name"> 遷移先のSceneの名前 </param>
    public void AnyLoadScene(string name)
    {
        fadeImage.UpdateMaskTexture(m_masks[2]);
        fade.FadeIn(1.0f, () =>
        {
            StartCoroutine(Load(name, 3.5f));
        });
    }

    /// <summary>
    /// ゲームを終了する
    /// </summary>
    public void QuitGame()
    {
        fadeImage.UpdateMaskTexture(m_masks[0]);
        fade.FadeIn(1.0f);
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

        SceneManager.LoadScene(name);
    }
    
    /// <summary>
    /// フェード後にゲームを終了するためのコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator DelayQuit()
    {
        yield return new WaitForSeconds(m_loadTime);

        Application.Quit();
    }
}