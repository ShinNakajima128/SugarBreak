using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// 再生中のムービーをスキップするコード
/// </summary>
public class SkipMovieController : MonoBehaviour
{
    [Header("ゲーム開始時の再生フラグ")]
    [SerializeField]
    bool m_playOnAwake = false;

    PlayableDirector m_director = default;
    bool isPlayed = false;

    private void Awake()
    {
        if (m_director == null) 
            m_director = GetComponent<PlayableDirector>();

        m_director.stopped += MovieFinished;
        m_director.played += Skip;

        if (m_playOnAwake)
        {
            m_director.Play();
        }
    }

    /// <summary>
    /// スキップ受付開始のコールバック
    /// </summary>
    /// <param name="director"></param>
    void Skip(PlayableDirector director)
    {
        if (m_director == director)
        {
            StartCoroutine(SkipCol());
        }   
    }

    IEnumerator SkipCol()
    {
        while (true)
        {
            if (!isPlayed && Input.anyKeyDown)
            {
                StartCoroutine(SkipMovie());
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator SkipMovie()
    {
        LoadSceneManager.Instance.FadeIn(LoadSceneManager.Instance.Masks[2]);
        isPlayed = true;
        yield return new WaitForSeconds(1.0f);
        LoadSceneManager.Instance.FadeOut(LoadSceneManager.Instance.Masks[1]);
        m_director.playableGraph.GetRootPlayable(0).SetSpeed(500);
        Debug.Log("再生終了");
    }

    /// <summary>
    /// 再生終了のコールバック
    /// </summary>
    /// <param name="director"></param>
    void MovieFinished(PlayableDirector director)
    {
        Debug.Log("再生終了");
        isPlayed = true;
    }
}
