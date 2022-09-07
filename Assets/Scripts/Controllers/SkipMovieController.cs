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
    static bool isPlayed = false;
    public static bool IsPlayed => isPlayed;

    void Start()
    {
        if (m_director == null) 
            m_director = GetComponent<PlayableDirector>();

        m_director.stopped += MovieFinished;
        m_director.stopped += CanOpenMenu;
        m_director.played += Skip;
        m_director.played += CanNotOpenMenu;

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
            isPlayed = false;
            StartCoroutine(SkipCol());
        }   
    }

    IEnumerator SkipCol()
    {
        while (!isPlayed)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 7"))
            {
                StartCoroutine(SkipMovie());
                AudioManager.StopSE();
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
        yield return new WaitForSeconds(1.0f);
        isPlayed = false;
    }

    /// <summary>
    /// 再生終了のコールバック
    /// </summary>
    /// <param name="director"></param>
    void MovieFinished(PlayableDirector director)
    {
        //isPlayed = true;
    }

    void CanNotOpenMenu(PlayableDirector director)
    {
        MenuManager.Instance.WhetherOpenMenu = false;
        GameManager.Instance.IsPlayingMovie = true;
    }

    void CanOpenMenu(PlayableDirector director)
    {
        MenuManager.Instance.WhetherOpenMenu = true;
        GameManager.Instance.IsPlayingMovie = false;
    }
}
