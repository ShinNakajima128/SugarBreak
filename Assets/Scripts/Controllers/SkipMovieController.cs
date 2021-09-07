using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// 再生中のムービーをスキップするコード
/// </summary>
public class SkipMovieController : MonoBehaviour
{
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

    void Skip(PlayableDirector director)
    {
        if (m_director == director)
        {
            Debug.Log("スキップ受付開始");
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
        LoadSceneManager.Instance.FadeIn();
        isPlayed = true;
        yield return new WaitForSeconds(1.0f);
        LoadSceneManager.Instance.FadeOut();
        m_director.playableGraph.GetRootPlayable(0).SetSpeed(500);
        Debug.Log("再生終了");
    }

    void MovieFinished(PlayableDirector director)
    {
        Debug.Log("再生終了");
        isPlayed = true;
    }
}
