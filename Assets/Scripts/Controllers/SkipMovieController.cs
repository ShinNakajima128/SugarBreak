using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// 再生中のムービーをスキップするコード
/// </summary>
public class SkipMovieController : MonoBehaviour
{
    PlayableDirector m_director = default;
    bool isPlayed = false;

    void Start()
    {
        m_director = GetComponent<PlayableDirector>();
    }

    void Update()
    {
        if (!isPlayed && Input.anyKeyDown)
        {
            StartCoroutine(SkipMovie());
        }
        if (m_director.time >= m_director.duration)
        {
            Debug.Log("再生終了");
            isPlayed = true;
        }
    }

    IEnumerator SkipMovie()
    {
        LoadSceneManager.Instance.FadeIn();

        yield return new WaitForSeconds(1.0f);
        LoadSceneManager.Instance.FadeOut();
        m_director.playableGraph.GetRootPlayable(0).SetSpeed(500);
        isPlayed = true;
    }
}
